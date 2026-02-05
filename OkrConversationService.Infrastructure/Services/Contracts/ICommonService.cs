using OkrConversationService.Domain.RequestModel;
using System.Net.Http;
using Microsoft.Extensions.Caching.Distributed;
using System.Threading.Tasks;
using OkrConversationService.Domain.ResponseModels;
using System.Collections.Generic;
using OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;
using OkrConversationService.Domain.Common;

namespace OkrConversationService.Infrastructure.Services.Contracts
{
    public interface ICommonService
    {
        string LoggedInUserEmail { get; }
        string UserToken { get; }
        string TenantId { get; }
        bool IsTokenActive { get; }
        IDistributedCache DistributedCache { get; }
        HttpClient GetHttpClient(string baseUrl);
        UserIdentity GetUserIdentity();
        Task<MailerTemplateRequest> GetMailerTemplate(string templateCode);
        void Notifications(NotificationsRequest request, ServiceSettingUrlResponse settings);
        void SendEmail(MailRequest request, ServiceSettingUrlResponse settings);
        ClientDetail SetClientDetail();
        Task<bool> CallSignalRFunctionForContributors(SignalrRequestModel signalrRequestModel);
        Task ImpersonateAuditLog(AuditLogRequest auditLogRequest);
        Task AuditEngagementReport(CreateEngagementReportRequest request);
        Task CallSignalRForAllEmployees(long loginUserId);
        Task CallSignalRforNotifications(long recognitionId, UserIdentity userIdentity);
        public async Task NoteCallSignalRforNotifications(List<long> empIds)
        {
            var signalrRequestModel = new SignalrRequestModel()
            {
                BroadcastValue = empIds,
                BroadcastTopic = AppConstants.TopicRequestOneToOne
            };
            await CallSignalRFunctionForContributors(signalrRequestModel);
        }
        Task<Payload<bool>> IsBlockedWords(string comments);
        Task CallSignalRForEditRecognition(List<long> empIds);
        void SendBulkEmail(TeamMailRequest request, ServiceSettingUrlResponse settings);
        NoteEmployeeTag IsNoteEmployeeTag { get; set; }
        List<NoteResponse> noteResponse { get; set; }



        CloudBlobContainer GetContainerRefByBlobClient(string BlobAccountName, string BlobAccountKey, string BlobContainerName);
        Task<CloudBlobContainer> UploadContainerRefByBlobClient(string BlobAccountName, string BlobAccountKey, string BlobContainerName);
        Task<bool> DeleteBlobByLocation(CloudBlobContainer objCloudBlobContainer, string deleteAzureLocation);
        Task<bool> UploadBlobByLocation(CloudBlobContainer objCloudBlobContainer, string azureLocation, string ContentType, Stream stream);

    }
}
