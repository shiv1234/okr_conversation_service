using Azure.Messaging.ServiceBus;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Services.Contracts;
using OkrConversationService.Persistence.EntityFrameworkDataAccess;
using OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Services
{
    public class CommonService : BaseService, ICommonService
    {
        private readonly IServicesAggregator _servicesAggregateService;
        public ISystemService _systemService { get; set; }

        private ServiceBusClient _client;

        private ServiceBusSender _clientSender;
        private readonly IRepositoryAsync<Employee> _employeeRepo;
        private readonly IRepositoryAsync<RecognitionEmployeeTeamMapping> _recognitionEmployeeTeamMappingRepo;
        private readonly IRepositoryAsync<EmployeeTag> _employeeTagRepo;
        private readonly IRepositoryAsync<EmployeeTeamMapping> _employeeTeamMappingRepo;
        public NoteEmployeeTag IsNoteEmployeeTag { get; set; }
        public List<NoteResponse> noteResponse { get; set; }

        [Obsolete("")]
        public CommonService(IServicesAggregator servicesAggregateService, ISystemService systemService) : base(servicesAggregateService)
        {
            _servicesAggregateService = servicesAggregateService;
            _systemService = systemService;
            _employeeRepo = UnitOfWorkAsync.RepositoryAsync<Employee>();
            _recognitionEmployeeTeamMappingRepo = UnitOfWorkAsync.RepositoryAsync<RecognitionEmployeeTeamMapping>();
            _employeeTagRepo = UnitOfWorkAsync.RepositoryAsync<EmployeeTag>();
            _employeeTeamMappingRepo = UnitOfWorkAsync.RepositoryAsync<EmployeeTeamMapping>();
        }

        public UserIdentity GetUserIdentity()
        {
            var loginUserDetail = new UserIdentity();
            var hasIdentity = _systemService.HttpContext.Request.Headers.TryGetValue("UserIdentity", out var userIdentity);

            if (!hasIdentity)
            {
                using var httpClient = GetHttpClient(Configuration.GetSection("OkrUser:BaseUrl").Value);
                using var response = httpClient.GetAsync($"Identity");
                if (!response.Result.IsSuccessStatusCode)
                    return loginUserDetail;
                var apiResponse = response.Result.Content.ReadAsStringAsync();
                var user = JsonConvert.DeserializeObject<Payload<UserIdentity>>(apiResponse.Result);
                if (user != null)
                    loginUserDetail = user.Entity;
            }
            else
            {
                var decryptVal = Encryption.DecryptStringAes(userIdentity, AppConstants.EncryptionSecretKey, AppConstants.EncryptionSecretIvKey);
                if (decryptVal != null)
                    loginUserDetail = JsonConvert.DeserializeObject<UserIdentity>(decryptVal);
            }

            return loginUserDetail;
        }

        public HttpClient GetHttpClient(string baseUrl)
        {
            string identity = string.Empty;
            var hasIdentity = _systemService.HttpContext.Request.Headers.TryGetValue("UserIdentity", out var userIdentity);
            if (hasIdentity)
            {
                identity = userIdentity;
            }
            var hasTenant = _systemService.HttpContext.Request.Headers.TryGetValue("TenantId", out var tenantId);
            if ((!hasTenant && _systemService.HttpContext.Request.Host.Value.Contains("localhost")))
                tenantId = _servicesAggregateService.Configuration.GetValue<string>("TenantId");
            string domain;
            var hasOrigin = _systemService.HttpContext.Request.Headers.TryGetValue("OriginHost", out var origin);
            if (!hasOrigin && _systemService.HttpContext.Request.Host.Value.Contains("localhost"))
                domain = _servicesAggregateService.Configuration.GetValue<string>("FrontEndUrl");
            else
                domain = string.IsNullOrEmpty(origin) ? string.Empty : origin.ToString();
            HttpClient httpClient = _systemService.SystemHttpClient();

            httpClient.BaseAddress = new Uri(baseUrl);
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + UserToken);
            httpClient.DefaultRequestHeaders.Add("TenantId", tenantId.ToString());
            httpClient.DefaultRequestHeaders.Add("OriginHost", domain);
            httpClient.DefaultRequestHeaders.Add("UserIdentity", identity);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        #region BaseService
        private IDistributedCache _distributedCache;
        public IDistributedCache DistributedCache => _distributedCache ??= _systemService.HttpContext.RequestServices.GetRequiredService<IDistributedCache>();
        public string LoggedInUserEmail => _systemService.HttpContext.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "email")?.Value;

        public string UserToken => _systemService.HttpContext.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "token")?.Value;

        public string TenantId => _systemService.HttpContext.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "tenantId")?.Value;

        public bool IsTokenActive => (!string.IsNullOrEmpty(LoggedInUserEmail) && !string.IsNullOrEmpty(UserToken));

        #endregion

        #region Notification
        public async Task<MailerTemplateRequest> GetMailerTemplate(string templateCode)
        {
            var template = new MailerTemplateRequest();
            var httpClient = GetHttpClient(_systemService.KeyVaultService.GetSettingsAndUrlsAsync().Result.NotificationBaseAddress);
            var response = await httpClient.GetAsync($"api/v2/OkrNotifications/GetTemplate?templateCode=" + templateCode);
            if (!response.IsSuccessStatusCode) return template;

            var payload = JsonConvert.DeserializeObject<Payload<MailerTemplateRequest>>(await response.Content.ReadAsStringAsync());
            if (payload != null) template = payload.Entity;
            return template;
        }

        public void Notifications(NotificationsRequest request, ServiceSettingUrlResponse settings)
        {
            var clientDetail = SetClientDetail();
            clientDetail.BaseUrl = settings.NotificationBaseAddress;
            var payload = new AzureBusPayload<NotificationsRequest>
            {
                Data = request,
                AzureBusServiceName = AzureBusServiceName.Notification,
                ClientDetail = clientDetail
            };

            _systemService.SendServiceBusMessageByBusClient(settings.AzureConnectionString,
                AppConstants.NotificationTopicName,
                System.Text.Json.JsonSerializer.Serialize(payload));
        }

        public void SendEmail(MailRequest request, ServiceSettingUrlResponse settings)
        {
            var clientDetail = SetClientDetail();
            clientDetail.BaseUrl = settings.NotificationBaseAddress;
            var payload = new AzureBusPayload<MailRequest>
            {
                Data = request,
                AzureBusServiceName = AzureBusServiceName.Email,
                ClientDetail = clientDetail,
                QueueName = AppConstants.QueueEmail,
                
            };

            _systemService.SendServiceBusMessageByBusClient(settings.AzureConnectionString, AppConstants.EmailTopicName, System.Text.Json.JsonSerializer.Serialize(payload));
        }

        public void SendBulkEmail(TeamMailRequest request, ServiceSettingUrlResponse settings)
        {
           
            var clientDetail = SetClientDetail();
            clientDetail.BaseUrl = settings.NotificationBaseAddress;
            var payload = new AzureBusPayload<TeamMailRequest>
            {
                Data = request,
                AzureBusServiceName = AzureBusServiceName.Email,
                ClientDetail = clientDetail,
                QueueName = AppConstants.QueueEmail,
            };
            _systemService.SendServiceBusMessageByBusClient(settings.AzureConnectionString, AppConstants.EmailTopicName, System.Text.Json.JsonSerializer.Serialize(payload));
        }


        public ClientDetail SetClientDetail()
        {
            string identity = string.Empty;
            var hasIdentity = _systemService.HttpContext.Request.Headers.TryGetValue("UserIdentity", out var userIdentity);
            if (hasIdentity)
            {
                identity = userIdentity;
            }
            var clientDetail = new ClientDetail();
            var hasTenant = _systemService.HttpContext.Request.Headers.TryGetValue("TenantId", out var tenantId);
            if (!hasTenant && _systemService.HttpContext.Request.Host.Value.Contains("localhost"))
                tenantId = _servicesAggregateService.Configuration.GetValue<string>("TenantId");
            string domain;
            var hasOrigin = _systemService.HttpContext.Request.Headers.TryGetValue("OriginHost", out var origin);
            if (!hasOrigin && _systemService.HttpContext.Request.Host.Value.Contains("localhost"))
                domain = _servicesAggregateService.Configuration.GetValue<string>("FrontEndUrl");
            else
                domain = string.IsNullOrEmpty(origin) ? string.Empty : origin.ToString();

            clientDetail.OriginHost = domain;
            clientDetail.TenantId = origin;
            clientDetail.Token = UserToken;
            clientDetail.TenantId = tenantId;
            clientDetail.UserIdentity = identity;
            return clientDetail;
        }
        #endregion

        #region SignalR
        public async Task<bool> CallSignalRFunctionForContributors(SignalrRequestModel signalrRequestModel)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(signalrRequestModel.BroadcastTopic))
            {
                if (signalrRequestModel.BroadcastTopic != null && signalrRequestModel.BroadcastValue != null || signalrRequestModel.EmployeeId > 0)
                {
                    var url = Configuration.GetSection("SignalR:SignalRAzureFunctionEndpoint").Value;
                    using var httpClient = new HttpClient();
                    httpClient.DefaultRequestHeaders
                       .Accept
                       .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    using var response = await httpClient.
                        PostAsJsonAsync(url, signalrRequestModel);

                    if (response.IsSuccessStatusCode)
                    {
                        result = response.IsSuccessStatusCode;
                    }
                }
            }
            return result;
        }

        public async Task CallSignalRForAllEmployees(long loginUserId)
        {
            var employees = await _employeeRepo.GetQueryable().Where(x => x.IsActive && x.EmployeeId != loginUserId).Select(x => x.EmployeeId).ToListAsync();
            var signalrRequestModel = new SignalrRequestModel()
            {
                BroadcastValue = employees,
                BroadcastTopic = AppConstants.TopicRecognition
            };
            await CallSignalRFunctionForContributors(signalrRequestModel);
        }

        public async Task CallSignalRforNotifications(long recognitionId,UserIdentity userIdentity)
        {
            var empIds = new List<long>();
            var employeeTeamMapping = new List<EmployeeTeamMapping>();
            var recognitionMapping = _recognitionEmployeeTeamMappingRepo.GetQueryable().Where(x => x.RecognitionId == recognitionId).ToList();
            var employeeTag = _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == recognitionId && (x.ModuleId == (int)ModuleId.TeamTagInRecognisationInBody || x.ModuleId == (int)ModuleId.Recognisation) && x.IsActive).ToList();
            if (employeeTag.Count > 0)
            {
                var tagId = employeeTag.Where(x => x.ModuleId == (int)ModuleId.TeamTagInRecognisationInBody).Select(x => x.TagId).ToList();
                employeeTeamMapping = _employeeTeamMappingRepo.GetQueryable().Where(x => tagId.Contains(x.TeamId) && x.IsActive).ToList();
                empIds.AddRange(employeeTeamMapping.Select(x => x.EmployeeId));
            }
            empIds.AddRange(recognitionMapping.Select(x => x.EmployeeId));          
            empIds.AddRange(employeeTag.Where(x => x.ModuleId == (int)ModuleId.Recognisation).Select(x => x.TagId));
            empIds.Remove(userIdentity.EmployeeId);
            var signalrRequestModel = new SignalrRequestModel()
            {
                BroadcastValue = empIds.Distinct().ToList(),
                BroadcastTopic = AppConstants.TopicRecognitionNotifications
            };
            await CallSignalRFunctionForContributors(signalrRequestModel).ConfigureAwait(false) ;
        }

        public async Task CallSignalRForEditRecognition(List<long>empIds)
        {
            var signalrRequestModel = new SignalrRequestModel()
            {
                BroadcastValue = empIds.Distinct().ToList(),
                BroadcastTopic = AppConstants.TopicRecognitionNotifications
            };
            await CallSignalRFunctionForContributors(signalrRequestModel).ConfigureAwait(false);
        }
        public async Task NoteCallSignalRforNotifications(List<long> empIds)
        {
            var signalrRequestModel = new SignalrRequestModel()
            {
                BroadcastValue = empIds,
                BroadcastTopic = AppConstants.TopicRequestOneToOne
            };
            await CallSignalRFunctionForContributors(signalrRequestModel);
        }

        #endregion       

        #region ImpersonateAuditLog

        public async Task ImpersonateAuditLog(AuditLogRequest auditLogRequest)
        {
            var settings = _systemService.KeyVaultService.GetSettingsAndUrlsAsync().Result;
            var clientDetail = SetClientDetail();
            clientDetail.BaseUrl = settings.ReportBaseAddress;
            var payload = new AzureBusPayload<AuditLogRequest>
            {
                Data = auditLogRequest,
                AzureBusServiceName = AzureBusServiceName.AuditLog,
                ClientDetail = clientDetail,
                QueueName = AppConstants.QueueAuditLog
            };
            ServiceBusClient _client = new ServiceBusClient(settings.AzureConnectionString);
            ServiceBusSender _clientSender = _client.CreateSender(AppConstants.EmailTopicName);
            var message = new ServiceBusMessage(System.Text.Json.JsonSerializer.Serialize(payload));
            await _clientSender.SendMessageAsync(message).ConfigureAwait(false);
        }
        #endregion
        #region Report
        [ExcludeFromCodeCoverage]
        public async Task AuditEngagementReport(CreateEngagementReportRequest request)
        {
            var settings = _systemService.KeyVaultService.GetSettingsAndUrlsAsync().Result;
            var clientDetail = SetClientDetail();
            clientDetail.BaseUrl = settings.ReportBaseAddress;
            var payload = new AzureBusPayload<CreateEngagementReportRequest>
            {
                Data = request,
                AzureBusServiceName = AzureBusServiceName.Report,
                ClientDetail = clientDetail,
                QueueName = AppConstants.QueueReport
            };
            _client = new ServiceBusClient(settings.AzureConnectionString);
            _clientSender = _client.CreateSender(AppConstants.EmailTopicName);
            var message = new ServiceBusMessage(System.Text.Json.JsonSerializer.Serialize(payload));
            await _clientSender.SendMessageAsync(message).ConfigureAwait(false);
        }
        #endregion

        #region Tenant

        public async Task<Payload<bool>> IsBlockedWords(string comments)
        {
            using var httpClient = GetHttpClient(_servicesAggregateService.Configuration.GetValue<string>("TenantService:BaseUrl"));
            //httpClient.BaseAddress = _commonService.SystemUri(_IServicesAggregator.Configuration.GetValue<string>("TenantService:BaseUrl"));
            Payload<bool> payload = new Payload<bool>();
            var response = await httpClient.PostAsJsonAsync($"isblockedwords", new BlockedWordsRequest { Text = comments });
            if (response.IsSuccessStatusCode)
            {
                payload = JsonConvert.DeserializeObject<Payload<bool>>(await response.Content.ReadAsStringAsync());
            }
            return payload;
        }
        #endregion


        #region BlobFunctions
        [ExcludeFromCodeCoverage]
        public CloudBlobContainer GetContainerRefByBlobClient(string BlobAccountName, string BlobAccountKey, string BlobContainerName)
        {
            var account = new CloudStorageAccount(new StorageCredentials(BlobAccountName, BlobAccountKey), true);
            var cloudBlobClient = account.CreateCloudBlobClient();
            return cloudBlobClient.GetContainerReference(BlobContainerName);
        }
        [ExcludeFromCodeCoverage]
        public async Task<CloudBlobContainer> UploadContainerRefByBlobClient(string BlobAccountName, string BlobAccountKey, string BlobContainerName)
        {
            var account = new CloudStorageAccount(new StorageCredentials(BlobAccountName, BlobAccountKey), true);
            var cloudBlobClient = account.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(BlobContainerName);

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
                await cloudBlobContainer.SetPermissionsAsync(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    }
                );
            return cloudBlobContainer;
        }
        [ExcludeFromCodeCoverage]
        public async Task<bool> DeleteBlobByLocation(CloudBlobContainer objCloudBlobContainer, string deleteAzureLocation)
        {
            bool status = false;
            if (!string.IsNullOrWhiteSpace(deleteAzureLocation))
            {
                CloudBlockBlob cloudBlobDelete = objCloudBlobContainer.GetBlockBlobReference(deleteAzureLocation);
                await cloudBlobDelete.DeleteAsync();
                status = true;
            }
            return status;
        }
        [ExcludeFromCodeCoverage]
        public async Task<bool> UploadBlobByLocation(CloudBlobContainer objCloudBlobContainer, string azureLocation, string ContentType, Stream stream)
        {
            var cloudBlockBlob = objCloudBlobContainer.GetBlockBlobReference(azureLocation);
            cloudBlockBlob.Properties.ContentType = ContentType;

            await cloudBlockBlob.UploadFromStreamAsync(stream);
            return true;
        }

        #endregion

    }
}
