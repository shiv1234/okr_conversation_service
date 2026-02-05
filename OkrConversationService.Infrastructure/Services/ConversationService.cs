using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Services.Contracts;
using OkrConversationService.Persistence.EntityFrameworkDataAccess;
using OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Services
{
    public class ConversationService : BaseService, IConversationService
    {
        private readonly IRepositoryAsync<Conversation> _conversationRepo;
        private readonly IRepositoryAsync<ConversationFile> _conversationFileRepo;
        private readonly IRepositoryAsync<EmployeeTag> _conversationEmployeeTagRepo;
        private readonly IRepositoryAsync<LikeReaction> _likeReactionRepo;
        private readonly IRepositoryAsync<ConversationLog> _conversationLogRepo;
        private readonly IRepositoryAsync<Employee> _employeeRepo;
        private readonly IKeyVaultService _keyVaultService;
        private readonly ICommonService _commonService;
        private readonly INotificationsEmailsService _notificationsService;
        private readonly ISystemService _systemService;
        private readonly IRepositoryAsync<NotificationsDetail> _notificationsDetailsRepo;

        [Obsolete("")]
        public ConversationService(IServicesAggregator servicesAggregateService, INotificationsEmailsService notificationsServices, IKeyVaultService keyVaultService, ICommonService commonService, ISystemService systemService) : base(servicesAggregateService)
        {
            _conversationRepo = UnitOfWorkAsync.RepositoryAsync<Conversation>();
            _conversationFileRepo = UnitOfWorkAsync.RepositoryAsync<ConversationFile>();
            _conversationEmployeeTagRepo = UnitOfWorkAsync.RepositoryAsync<EmployeeTag>();
            _likeReactionRepo = UnitOfWorkAsync.RepositoryAsync<LikeReaction>();
            _conversationLogRepo = UnitOfWorkAsync.RepositoryAsync<ConversationLog>();
            _employeeRepo = UnitOfWorkAsync.RepositoryAsync<Employee>();
            _keyVaultService = keyVaultService;
            _notificationsService = notificationsServices;
            _commonService = commonService;
            _systemService = systemService;
            _notificationsDetailsRepo = UnitOfWorkAsync.RepositoryAsync<NotificationsDetail>();

        }

        public async Task<Payload<ConversationResponse>> GetAll(ConversationGetAllQuery request)
        {
            var payload = new Payload<ConversationResponse>();
            var userIdentity = _commonService.GetUserIdentity();
            var conversations = await (from conversation in _conversationRepo.GetQueryable().Where(y => y.GoalSourceId == request.GoalSourceId && y.GoalTypeId == request.GoalTypeId && y.IsActive).OrderByDescending(x => x.CreatedOn)
                                       select new ConversationResponse
                                       {
                                           ConversationId = conversation.ConversationId,
                                           Description = conversation.Description,
                                           Type = conversation.Type,
                                           GoalTypeId = conversation.GoalTypeId,
                                           GoalId = conversation.GoalId,
                                           GoalSourceId = conversation.GoalSourceId,
                                           CreatedBy = conversation.CreatedBy,
                                           CreatedOn = conversation.CreatedOn,
                                           UpdatedOn = conversation.UpdatedOn,
                                           IsEdited = conversation.UpdatedOn != null,
                                           IsReadOnly = userIdentity.EmployeeId != conversation.CreatedBy,
                                           TotalReplies = _conversationRepo.GetQueryable().Where(x=>x.GoalId == conversation.ConversationId && x.GoalTypeId == 3 && x.Type == (int)ConversationType.ConversationReplyComment && x.IsActive).Count(),
                                           ConversationReactions = (from react in _likeReactionRepo.GetQueryable().Where(x => x.ModuleDetailsId == conversation.ConversationId && x.IsActive && x.ModuleId == (int)ModuleId.Conversation)
                                                                    select new ConversationReactionResponse()
                                                                    {
                                                                        LikeReactionId = react.LikeReactionId,
                                                                        EmployeeId = react.EmployeeId,
                                                                    }).ToList()
                                       }).ToListAsync();
            if (conversations.Count > 0)
            {
                //save data in log table
                ConversationLog(userIdentity.EmployeeId, conversations.FirstOrDefault().CreatedOn);

                request.PageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
                var skipAmount = request.PageSize * (request.PageIndex - 1);
                var totalRecords = conversations.Count;
                var totalPages = totalRecords / request.PageSize;

                if (totalRecords % request.PageSize > 0)
                    totalPages++;

                var result = new PageInfo
                {
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalRecords = totalRecords,
                    TotalPages = totalPages
                };

                var pageList = conversations.Skip(skipAmount).Take(request.PageSize).ToList();

                //get all users
                var userIds = pageList.SelectMany(x => x.ConversationReactions.Select(y => y.EmployeeId)).ToList();
                userIds.AddRange(pageList.Select(x => x.CreatedBy));               
                var conversationIds = pageList.Select(x => x.ConversationId).ToList();
                var replyConRepo = _conversationRepo.GetQueryable().Where(x => conversationIds.Contains(x.GoalId) && x.GoalTypeId == 3 && x.Type == (int)ConversationType.ConversationReplyComment && x.IsActive).ToList();
                userIds.AddRange(replyConRepo.Select(x => x.CreatedBy).ToList());
                var users = _employeeRepo.GetQueryable().Where(x => userIds.Contains(x.EmployeeId)).ToList();
                pageList.ForEach(x =>
                {
                    if (x.TotalReplies == 1)
                    {
                        var replyEmp = replyConRepo.FirstOrDefault(z=>z.GoalId == x.ConversationId);
                        x.ReplyFirstName = users.FirstOrDefault(b => b.EmployeeId == replyEmp.CreatedBy).FirstName;
                        x.ReplyLastName = users.FirstOrDefault(b => b.EmployeeId == replyEmp.CreatedBy).LastName;
                        x.ReplyFullName = users.FirstOrDefault(b => b.EmployeeId == replyEmp.CreatedBy).FirstName + " " + users.FirstOrDefault(b => b.EmployeeId == replyEmp.CreatedBy).LastName;
                        x.ReplyImagePath = users.FirstOrDefault(b => b.EmployeeId == replyEmp.CreatedBy).ImagePath;
                    }
                    var employee = users.FirstOrDefault(y => y.EmployeeId == x.CreatedBy);
                    x.FirstName = employee.FirstName;
                    x.LastName = employee.LastName;
                    x.FullName = employee.FirstName + " " + employee.LastName;
                    x.ImagePath = employee.ImagePath;
                    x.IsLiked = x.ConversationReactions.FirstOrDefault(y => y.EmployeeId == userIdentity.EmployeeId) != null;
                    x.TotalLikeCount = x.ConversationReactions.Count;
                    x.ConversationReactions.ForEach(z =>
                    {
                        var emp = users.FirstOrDefault(u => u.EmployeeId == z.EmployeeId);
                        z.FirstName = emp.FirstName;
                        z.LastName = emp.LastName;
                        z.FullName = emp.FirstName + " " + emp.LastName;
                        z.ImagePath = emp.ImagePath;
                        z.EmailId = emp.EmailId;
                    });
                });

                payload.EntityList = pageList;
                payload.PagingInfo = result;
                payload = GetPayloadStatusSuccess(payload);

            }
            else
            {
                payload = GetPayloadStatusNoContent(payload, "message", ResourceMessage.RecordNotFoundMessage);
            }

            return payload;
        }

        public async Task<Payload<ConversationCommentResponse>> GetConversationComments(ConversationCommentGetAllQuery request)
        {
            var payload = new Payload<ConversationCommentResponse>();
            var userIdentity = _commonService.GetUserIdentity();
            var conversationComment = await (from conversation in _conversationRepo.GetQueryable().Where(y => y.GoalId == request.GoalId && y.GoalTypeId == 3 && y.Type == (int)ConversationType.ConversationReplyComment && y.IsActive).OrderBy(x => x.CreatedOn)
                                             select new ConversationCommentResponse
                                             {
                                                 ConversationId = conversation.ConversationId,
                                                 Description = conversation.Description,
                                                 Type = conversation.Type,
                                                 GoalTypeId = conversation.GoalTypeId,
                                                 GoalId = conversation.GoalId,
                                                 GoalSourceId = conversation.GoalSourceId,
                                                 CreatedBy = conversation.CreatedBy,
                                                 CreatedOn = conversation.CreatedOn,
                                                 UpdatedOn = conversation.UpdatedOn,
                                                 IsEdited = conversation.UpdatedOn != null,
                                                 IsReadOnly = userIdentity.EmployeeId != conversation.CreatedBy,                                               
                                                 ConversationReactions = (from react in _likeReactionRepo.GetQueryable().Where(x => x.ModuleDetailsId == conversation.ConversationId && x.IsActive && x.ModuleId == (int)ModuleId.Conversation)
                                                                          select new ConversationReactionResponse()
                                                                          {
                                                                              LikeReactionId = react.LikeReactionId,
                                                                              EmployeeId = react.EmployeeId,
                                                                          }).ToList()
                                             }).ToListAsync();

            if (conversationComment.Count > 0)
            {
                request.PageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
                var skipAmount = request.PageSize * (request.PageIndex - 1);
                var totalRecords = conversationComment.Count;
                var totalPages = totalRecords / request.PageSize;

                if (totalRecords % request.PageSize > 0)
                    totalPages++;

                var result = new PageInfo
                {
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    TotalRecords = totalRecords,
                    TotalPages = totalPages
                };

                var pageList = conversationComment.Skip(skipAmount).Take(request.PageSize).ToList();

                //get all users
                var userIds = pageList.SelectMany(x => x.ConversationReactions.Select(y => y.EmployeeId)).ToList();
                userIds.AddRange(pageList.Select(x => x.CreatedBy));
                var users = _employeeRepo.GetQueryable().Where(x => userIds.Contains(x.EmployeeId)).ToList();
                pageList.ForEach(x =>
                {
                    var employee = users.FirstOrDefault(y => y.EmployeeId == x.CreatedBy);
                    x.FirstName = employee.FirstName;
                    x.LastName = employee.LastName;
                    x.FullName = employee.FirstName + " " + employee.LastName;
                    x.ImagePath = employee.ImagePath;
                    x.IsLiked = x.ConversationReactions.FirstOrDefault(y => y.EmployeeId == userIdentity.EmployeeId) != null;
                    x.TotalLikeCount = x.ConversationReactions.Count;
                    x.ConversationReactions.ForEach(z =>
                    {
                        var emp = users.FirstOrDefault(u => u.EmployeeId == z.EmployeeId);
                        z.FirstName = emp.FirstName;
                        z.LastName = emp.LastName;
                        z.FullName = emp.FirstName + " " + emp.LastName;
                        z.ImagePath = emp.ImagePath;
                        z.EmailId = emp.EmailId;
                    });
                });

                payload.EntityList = pageList;
                payload.PagingInfo = result;
                payload = GetPayloadStatusSuccess(payload);

            }
            else
            {
                payload.EntityList = new List<ConversationCommentResponse>();
                payload = GetPayloadStatusSuccess(payload);
            }
            return payload;
        }

        public async Task<Payload<string>> UploadConversationImageOnAzure(UploadFileCommand request)
        {
            var keyVault = await _keyVaultService.GetAzureBlobKeysAsync();
            var payload = new Payload<string>();
            string strFolderName;
            string azureLocation = string.Empty;
            if (keyVault != null)
            {
                var fileExt = Path.GetExtension(request.FormFile.FileName);
                var fSize = request.FormFile.Length;
                var fileSize = fSize / 1000;
                var formats = new[] { ".jpeg", ".jpg", ".png", ".svg" };
                if (fileExt != null && !formats.Contains(fileExt.ToLower()))
                {
                    return GetPayloadStatus(payload, "file", ResourceMessage.FileFormatMsg);
                }

                if (request.FormFile.Length == 0)
                    return GetPayloadStatus(payload, "file", ResourceMessage.FileUploadMsg);
                if (fileSize > 5000)
                    return GetPayloadStatus(payload, "file", ResourceMessage.FileFormatMsg);

                var imageGuid = Guid.NewGuid().ToString();
                if(request.Type == 1)
                {
                    strFolderName = Configuration.GetValue<string>("AzureBlob:ConversationImageFolderName");
                    azureLocation = strFolderName + "/" + imageGuid + fileExt;
                }
                if(request.Type == 2)
                {
                    strFolderName = Configuration.GetValue<string>("AzureBlob:RecognitionScreenshotImageFolderName");
                    azureLocation = strFolderName + "/" + imageGuid + fileExt;
                }
                else if (request.Type == 3)
                {
                    strFolderName = Configuration.GetValue<string>("AzureBlob:RecognitionCommentFolderName");
                    azureLocation = strFolderName + "/" + imageGuid + fileExt;
                }
                var account = new CloudStorageAccount(new StorageCredentials(keyVault.BlobAccountName, keyVault.BlobAccountKey), true);
                var cloudBlobClient = account.CreateCloudBlobClient();
                var cloudBlobContainer = cloudBlobClient.GetContainerReference(keyVault.BlobContainerName);

                if (await cloudBlobContainer.CreateIfNotExistsAsync())
                    await cloudBlobContainer.SetPermissionsAsync(
                        new BlobContainerPermissions
                        {
                            PublicAccess = BlobContainerPublicAccessType.Blob
                        }
                    );

                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(azureLocation);
                cloudBlockBlob.Properties.ContentType = request.FormFile.ContentType;
                HashSet<string> blocklist = new HashSet<string>();
                var file = request.FormFile;
                const int pageSizeInBytes = 900000;
                long prevLastByte = 0;
                long bytesRemain = file.Length;

                byte[] bytes;

                using (MemoryStream ms = new MemoryStream())
                {
                    var fileStream = file.OpenReadStream();
                    await fileStream.CopyToAsync(ms);
                    bytes = ms.ToArray();
                }

                do
                {
                    long bytesToCopy = Math.Min(bytesRemain, pageSizeInBytes);
                    byte[] bytesToSend = new byte[bytesToCopy];

                    Array.Copy(bytes, prevLastByte, bytesToSend, 0, bytesToCopy);
                    prevLastByte += bytesToCopy;
                    bytesRemain -= bytesToCopy;

                    //create blockId
                    string blockId = Guid.NewGuid().ToString();
                    string base64BlockId = Convert.ToBase64String(Encoding.UTF8.GetBytes(blockId));

                    await cloudBlockBlob.PutBlockAsync(
                        base64BlockId,
                        new MemoryStream(bytesToSend, true),
                        null
                        );

                    blocklist.Add(base64BlockId);

                } while (bytesRemain > 0);

                await cloudBlockBlob.PutBlockListAsync(blocklist);
               // await cloudBlockBlob.UploadFromStreamAsync(request.FormFile.OpenReadStream());

                var result = keyVault.BlobCdnUrl + keyVault.BlobContainerName + "/" + azureLocation;
                payload.Entity = result.Trim();
                payload = GetPayloadStatusSuccess(payload);
            }

            return payload;
        }

        public async Task<Payload<ConversationCreateRequest>> Create(ConversationCreateCommand request)
        {
            var userIdentity = _commonService.GetUserIdentity();
            var payload = new Payload<ConversationCreateRequest> { Entity = request.ConversationCreateRequest };

            //Add Conversation
            var conversations = new Conversation
            {
                Description = request.ConversationCreateRequest.Description,
                GoalId = request.ConversationCreateRequest.GoalId,
                GoalTypeId = request.ConversationCreateRequest.GoalTypeId,
                Type = request.ConversationCreateRequest.Type,
                IsActive = request.IsActive,
                CreatedBy = userIdentity.EmployeeId,
                CreatedOn = request.CreatedOn,
                GoalSourceId = request.ConversationCreateRequest.GoalSourceId > 0 ? request.ConversationCreateRequest.GoalSourceId : request.ConversationCreateRequest.GoalId
            };
            _conversationRepo.Add(conversations);
            var result = await UnitOfWorkAsync.SaveChangesAsync();

            request.ConversationCreateRequest.ConversationId = conversations.ConversationId;
            var GoalSourceId = conversations.GoalSourceId;

            //assigned files

            if (request.ConversationCreateRequest.assignedFiles != null && result.Success && request.ConversationCreateRequest.assignedFiles.Count > 0)
            {
                await CreateStorageFile(request.ConversationCreateRequest, userIdentity.EmployeeId).ConfigureAwait(false);
            }

            //employee Tags
            if (request.ConversationCreateRequest.employeeTags != null && result.Success && request.ConversationCreateRequest.employeeTags.Count > 0)
            {
                await CreateEmployeeTags(request.ConversationCreateRequest, userIdentity.EmployeeId).ConfigureAwait(false);
            }
            if (request.ConversationCreateRequest.employeeTags!.Select(x => x.EmployeeId).Distinct().ToList().Count > 0 && request.ConversationCreateRequest.Type != (int)ConversationType.ConversationReplyComment)
            {
                await _notificationsService.UserNotificationsAndEmails(request.ConversationCreateRequest.employeeTags.Select(x => x.EmployeeId).Distinct().ToList(), userIdentity.EmployeeId, request.ConversationCreateRequest.GoalId, request.ConversationCreateRequest.GoalTypeId, request.ConversationCreateRequest.ConversationId, GoalSourceId, request.ConversationCreateRequest.Description).ConfigureAwait(false);
                // Call signalR to broadcast
                var signalrRequestModel = new SignalrRequestModel()
                {
                    BroadcastValue = request.ConversationCreateRequest.employeeTags.Select(x => x.EmployeeId).Distinct().ToList(),
                    BroadcastTopic = AppConstants.TopicConversationTag
                };
                await _commonService.CallSignalRFunctionForContributors(signalrRequestModel);
            }
            else if(request.ConversationCreateRequest.Type == (int)ConversationType.ConversationReplyComment)
            {
                await _notificationsService.UserReplyConversationNotifications(userIdentity.EmployeeId, request.ConversationCreateRequest.GoalId, request.ConversationCreateRequest.ConversationId).ConfigureAwait(false);
            }

            payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.ConversationCreated);

            // Impersonate Audit Log            
            if (userIdentity.IsImpersonatedUser)
            {
                string conversationDescription = Regex.Replace(request.ConversationCreateRequest.Description, AppConstants.HtmlRejex, String.Empty).Replace(AppConstants.TagNbsp, AppConstants.SingleSpace);
                AuditLogRequest auditLogRequest = new AuditLogRequest
                {
                    ActivityName = AppConstants.AddConversation,
                    ActivityDescription = "Conversation Created - " + conversationDescription,
                    TransactionId = request.ConversationCreateRequest.ConversationId
                };
                await _commonService.ImpersonateAuditLog(auditLogRequest);
            }

            //call AuditEngagementReport
            if (request.ConversationCreateRequest.Type == 1)
                 await _commonService.AuditEngagementReport(new CreateEngagementReportRequest { EmployeeId = userIdentity.EmployeeId, EngagementTypeId = AppConstants.Engagement_Conversation })
                .ConfigureAwait(false);
            else if (request.ConversationCreateRequest.Type == 2)
                await _commonService.AuditEngagementReport(new CreateEngagementReportRequest { EmployeeId = userIdentity.EmployeeId, EngagementTypeId = AppConstants.Engagement_AddProgressNote })
              .ConfigureAwait(false);

            return payload;
        }


        public async Task<Payload<ConversationEditRequest>> Edit(ConversationEditCommand request)
        {
            var userIdentity = _commonService.GetUserIdentity();
            var payload = new Payload<ConversationEditRequest> { Entity = request.ConversationEditRequest };

            var convrDetails = await GetConversationById(request.ConversationEditRequest.ConversationId);
            if (convrDetails == null)
                return GetPayloadStatus(payload, "noteId", ResourceMessage.ConversationIdInvalid);

            var conversationDetailsJson = JsonConvert.SerializeObject(convrDetails);
            convrDetails.Description = request.ConversationEditRequest.Description;
            convrDetails.Type = request.ConversationEditRequest.Type;
            convrDetails.IsActive = request.ConversationEditRequest.IsActive;
            convrDetails.UpdatedBy = userIdentity.EmployeeId;
            convrDetails.UpdatedOn = request.UpdatedOn;
            _conversationRepo.Update(convrDetails);
            var operationStatus = await UnitOfWorkAsync.SaveChangesAsync();

            if (!operationStatus.Success)
                return GetPayloadStatus(payload, "message", ResourceMessage.SomethingWentWrong);

            //assigned files
            var convrOldFile = _conversationFileRepo.GetQueryable().Where(x => x.ConversationId == request.ConversationEditRequest.ConversationId && x.IsActive).Select(x => x.StorageFileName).ToList();
            var convrAllFiles = await _conversationFileRepo.GetQueryable().Where(x => x.ConversationId == request.ConversationEditRequest.ConversationId).ToListAsync();

            foreach (var assignFiles in request.ConversationEditRequest.assignedFiles)
            {

                var convrFiles = convrAllFiles.FirstOrDefault(x => x.StorageFileName == assignFiles.StorageFileName);
                if (convrFiles != null)
                {
                    convrFiles.FileName = assignFiles.FileName.Trim();
                    convrFiles.FilePath = assignFiles.FilePath;
                    convrFiles.StorageFileName = assignFiles.StorageFileName;
                    convrFiles.IsActive = request.IsActive;
                    convrFiles.UpdatedBy = userIdentity.EmployeeId;
                    convrFiles.UpdatedOn = request.UpdatedOn;
                    _conversationFileRepo.Update(convrFiles);
                }
                else
                {
                    var convr = new ConversationFile()
                    {
                        ConversationId = request.ConversationEditRequest.ConversationId,
                        FileName = assignFiles.FileName.Trim(),
                        FilePath = assignFiles.FilePath,
                        StorageFileName = assignFiles.StorageFileName,
                        IsActive = request.IsActive,
                        CreatedBy = userIdentity.EmployeeId,
                        CreatedOn = request.CreatedOn
                    };
                    _conversationFileRepo.Add(convr);
                }

            }

            var convrNewFile = request.ConversationEditRequest.assignedFiles.Select(x => x.StorageFileName).ToList();

            if (convrOldFile.Count > 0 && convrNewFile.Count > 0)
            {
                var convrFinalList = convrOldFile.Except(convrNewFile).ToList();
                if (convrFinalList.Count > 0)
                {
                    var convrFileRecords = await _conversationFileRepo.GetQueryable().Where(x => convrFinalList.Contains(x.StorageFileName)).ToListAsync();

                    ////update the status of File as inactive
                    if (convrFileRecords.Count > 0)
                    {
                        convrFileRecords.ForEach(a =>
                        {
                            a.IsActive = false;
                            a.UpdatedBy = userIdentity.EmployeeId;
                            a.UpdatedOn = request.UpdatedOn;
                            _conversationFileRepo.Update(a);
                        });
                        UnitOfWorkAsync.SaveChanges();
                    }
                }
            }

            if (convrOldFile.Count > 0 && convrNewFile.Count == 0)
            {
                var convrFileRecords = await _conversationFileRepo.GetQueryable().Where(x => convrOldFile.Contains(x.StorageFileName)).ToListAsync();

                ////update the status of File as inactive
                if (convrFileRecords.Count > 0)
                {
                    convrFileRecords.ForEach(a =>
                    {
                        a.IsActive = false;
                        a.UpdatedBy = userIdentity.EmployeeId;
                        a.UpdatedOn = request.UpdatedOn;
                        _conversationFileRepo.Update(a);
                    });
                    UnitOfWorkAsync.SaveChanges();
                }

            }


            //employee Tags

            var empOldTag = _conversationEmployeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == request.ConversationEditRequest.ConversationId && x.IsActive && x.ModuleId == (int)ModuleId.Conversation).Select(x => x.TagId).ToList();

            var allEmployeeTag = await _conversationEmployeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == request.ConversationEditRequest.ConversationId && x.ModuleId == (int)ModuleId.Conversation).ToListAsync();
            foreach (var assignUser in request.ConversationEditRequest.employeeTags.Select(x => x.EmployeeId).Distinct().ToList())
            {
                var employeeTag = allEmployeeTag.FirstOrDefault(x => x.TagId == assignUser);
                if (employeeTag != null)
                {
                    employeeTag.IsActive = request.IsActive;
                    employeeTag.UpdatedBy = userIdentity.EmployeeId;
                    employeeTag.UpdatedOn = request.UpdatedOn;
                    _conversationEmployeeTagRepo.Update(employeeTag);
                }
                else
                {
                    var convrEmployeeTag = new EmployeeTag()
                    {
                        ModuleDetailsId = request.ConversationEditRequest.ConversationId,
                        TagId = assignUser,
                        IsActive = request.IsActive,
                        CreatedBy = userIdentity.EmployeeId,
                        CreatedOn = request.CreatedOn,
                        ModuleId = (int)ModuleId.Conversation
                    };
                    _conversationEmployeeTagRepo.Add(convrEmployeeTag);
                }

            }

            var empNewTag = request.ConversationEditRequest.employeeTags.Select(x => x.EmployeeId).Distinct().ToList();
            if (empOldTag.Count > 0 && empNewTag.Count > 0)
            {
                var empFinalList = empOldTag.Except(empNewTag).ToList();
                if (empFinalList.Count > 0)
                {
                    var empFileRecords = await _conversationEmployeeTagRepo.GetQueryable().Where(x => empFinalList.Contains(x.TagId)).ToListAsync();

                    ////update the status of Note File as inactive
                    if (empFileRecords.Count > 0)
                    {
                        empFileRecords.ForEach(a =>
                        {
                            a.IsActive = false;
                            a.UpdatedBy = userIdentity.EmployeeId;
                            a.UpdatedOn = request.UpdatedOn;
                            _conversationEmployeeTagRepo.Update(a);
                        });
                        UnitOfWorkAsync.SaveChanges();
                    }
                }
            }

            if (empOldTag.Count > 0 && empNewTag.Count == 0 )
            {
                var empFileRecords = await _conversationEmployeeTagRepo.GetQueryable().Where(x => empOldTag.Contains(x.TagId)).ToListAsync();

                ////update the status of Employee as inactive
                if (empFileRecords.Count > 0)
                {
                    empFileRecords.ForEach(a =>
                    {
                        a.IsActive = false;
                        a.UpdatedBy = userIdentity.EmployeeId;
                        a.UpdatedOn = request.UpdatedOn;
                        _conversationEmployeeTagRepo.Update(a);
                    });
                    UnitOfWorkAsync.SaveChanges();
                }
            }

            await UnitOfWorkAsync.SaveChangesAsync();

            //mail and notifications           

            if (empOldTag.Count == 0 && empNewTag.Count > 0 && request.ConversationEditRequest.Type != (int)ConversationType.ConversationReplyComment)
                await _notificationsService.UserNotificationsAndEmails(empNewTag, userIdentity.EmployeeId, convrDetails.GoalId, convrDetails.GoalTypeId, convrDetails.ConversationId, convrDetails.GoalSourceId, convrDetails.Description).ConfigureAwait(false);
            else if (empOldTag.Count == 0 && empNewTag.Count > 0 && request.ConversationEditRequest.Type == (int)ConversationType.ConversationReplyComment)
                await _notificationsService.EditReplyConversation(empNewTag, userIdentity.EmployeeId, convrDetails.GoalId, convrDetails.ConversationId).ConfigureAwait(false);

            if (empOldTag.Count > 0 && empNewTag.Count > 0)
            {
                var empFinalList = empNewTag.Except(empOldTag).ToList();
                if (empFinalList.Count > 0 && request.ConversationEditRequest.Type != (int)ConversationType.ConversationReplyComment)
                    await _notificationsService.UserNotificationsAndEmails(empFinalList, userIdentity.EmployeeId, convrDetails.GoalId, convrDetails.GoalTypeId, convrDetails.ConversationId, convrDetails.GoalSourceId, convrDetails.Description).ConfigureAwait(false);
                else if (empFinalList.Count > 0 && request.ConversationEditRequest.Type == (int)ConversationType.ConversationReplyComment)
                    await _notificationsService.EditReplyConversation(empFinalList, userIdentity.EmployeeId, convrDetails.GoalId, convrDetails.ConversationId).ConfigureAwait(false);
            }

            payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.ConversationUpdated);

            // Impersonate Audit Log            
            if (userIdentity.IsImpersonatedUser)
            {
                var conversationUpdatedJson = JsonConvert.SerializeObject(convrDetails);
                AuditLogRequest auditLogRequest = new AuditLogRequest
                {
                    ActivityName = AppConstants.EditConversation,
                    ActivityDescription = "Conversation Updated - " + conversationUpdatedJson,
                    TransactionId = request.ConversationEditRequest.ConversationId,
                    ActivityPreviousData = conversationDetailsJson
                };
                await _commonService.ImpersonateAuditLog(auditLogRequest);
            }

            //call AuditEngagementReport
            await _commonService.AuditEngagementReport(new CreateEngagementReportRequest { EmployeeId = userIdentity.EmployeeId, EngagementTypeId = AppConstants.Engagement_Conversation})
                .ConfigureAwait(false);

            return payload;

        }

        public async Task<Payload<bool>> DeleteConversation(ConversationDeleteCommand request)
        {

            var userIdentity = _commonService.GetUserIdentity();
            var payload = new Payload<bool> { Entity = false };
            var getConversation = _conversationRepo.GetQueryable().FirstOrDefault(x => x.ConversationId == request.ConversationId && x.IsActive);          
            if (getConversation == null)
                return GetPayloadStatus(payload, "conversationId", ResourceMessage.RecordNotFound);
            else
            {
                getConversation.IsActive = false;
                getConversation.UpdatedOn = request.UpdatedOn;
                getConversation.UpdatedBy = userIdentity.EmployeeId;
                _conversationRepo.Update(getConversation);

                //Delete Employee Tag 
                DeleteEmployeeTag(request, userIdentity);

                //Delete Files
                await DeleteFiles(request, userIdentity);

                await DeleteReplyConversation(request.ConversationId, userIdentity).ConfigureAwait(false);

                await DeleteReplyConversationNotifications(new List<long> { request.ConversationId }).ConfigureAwait(false);

                UnitOfWorkAsync.SaveChanges();
                payload.Entity = true;
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", getConversation.Type == (int)ConversationType.ConversationReplyComment ? ResourceMessage.ReplyDeleted : ResourceMessage.ConversationDeleted);
            }
        

            // Impersonate Audit Log            
            if (userIdentity.IsImpersonatedUser)
            {
                AuditLogRequest auditLogRequest = new AuditLogRequest
                {
                    ActivityName = AppConstants.DeleteConversation,
                    ActivityDescription = "Conversation Deleted",
                    TransactionId = request.ConversationId
                };
                await _commonService.ImpersonateAuditLog(auditLogRequest);
            }

            return payload;

        }

        public async Task<Payload<UnreadConversationResponse>> GetAllUnreadConversation(GetAllUnreadConversationQuery request)
        {
            var payload = new Payload<UnreadConversationResponse>() { EntityList = new List<UnreadConversationResponse>() };
            var conversationLog = _conversationLogRepo.GetQueryable().FirstOrDefault(x => x.EmployeeId == request.EmpId);
            var conversations = await (from conversation in _conversationRepo.GetQueryable()
                                       .Where(y => y.CreatedOn > (conversationLog != null ? conversationLog.ConversationLastSeenOn : y.CreatedOn.AddDays(-1)) && y.CreatedBy != request.EmpId)
                                       .OrderByDescending(x => x.CreatedOn)
                                       select new UnreadConversationResponse
                                       {
                                           ConversationId = conversation.ConversationId,
                                           Description = conversation.Description,
                                           Type = conversation.Type,
                                           GoalTypeId = conversation.GoalTypeId,
                                           GoalId = conversation.GoalId,
                                           GoalSourceId = conversation.GoalSourceId,
                                           CreatedBy = conversation.CreatedBy,
                                           CreatedOn = conversation.CreatedOn,
                                           UpdatedOn = conversation.UpdatedOn
                                       }).ToListAsync();
            if (conversations.Count > 0)
            {
                payload.EntityList = conversations;
                payload = GetPayloadStatusSuccess(payload);
            }
            else
            {
                payload = GetPayloadStatusNoContent(payload, "message", ResourceMessage.RecordNotFoundMessage);
            }
            return payload;
        }

        public async Task<Payload<bool>> IsEmployeeTag(long conversationId)
        {
            var payload = new Payload<bool>();
            var isExist = await _conversationEmployeeTagRepo.FirstOrDefaultAsync(x => x.ModuleDetailsId == conversationId && x.IsActive && x.ModuleId == (int)ModuleId.Conversation);
            if (isExist != null)
                payload.Entity = true;
            else
                payload.Entity = false;

            payload = GetPayloadStatusSuccess(payload);
            return payload;
        }

        public async Task<Payload<ConversationLikeCreateRequest>> CreateLike(ConversationLikeCommand request)
        {
            var userIdentity = _commonService.GetUserIdentity();
            var payload = new Payload<ConversationLikeCreateRequest> { Entity = request.ConversationLikeCreateRequest };
            if (request.ConversationLikeCreateRequest.IsActive)
            {
                var conversationLike = await _likeReactionRepo.FirstOrDefaultAsync(x => x.EmployeeId == request.ConversationLikeCreateRequest.EmployeeId && x.ModuleDetailsId == request.ConversationLikeCreateRequest.ModuleDetailsId && x.ModuleId == request.ConversationLikeCreateRequest.ModuleId);
                if (conversationLike != null)
                {
                    conversationLike.IsActive = request.ConversationLikeCreateRequest.IsActive;
                    conversationLike.UpdatedOn = request.UpdatedOn;
                    conversationLike.UpdatedBy = userIdentity.EmployeeId;
                    _likeReactionRepo.Update(conversationLike);
                    await UnitOfWorkAsync.SaveChangesAsync();
                }
                else
                {
                    var conversationReaction = new LikeReaction
                    {
                        ModuleDetailsId = request.ConversationLikeCreateRequest.ModuleDetailsId,
                        ModuleId = request.ConversationLikeCreateRequest.ModuleId,
                        EmployeeId = request.ConversationLikeCreateRequest.EmployeeId,
                        IsActive = request.ConversationLikeCreateRequest.IsActive,
                        CreatedBy = userIdentity.EmployeeId,
                        CreatedOn = request.CreatedOn
                    };
                    _likeReactionRepo.Add(conversationReaction);
                    await UnitOfWorkAsync.SaveChangesAsync();
                    if (request.ConversationLikeCreateRequest.ModuleId == (int)ModuleId.Conversation)
                    {
                        await _commonService.AuditEngagementReport(new CreateEngagementReportRequest { EmployeeId = userIdentity.EmployeeId, EngagementTypeId = AppConstants.Engagement_LikeConversation })
                       .ConfigureAwait(false);
                       
                    }
                    else if (request.ConversationLikeCreateRequest.ModuleId == (int)ModuleId.Recognisation)
                    {
                        await _notificationsService.LikeNotifications(conversationReaction.ModuleDetailsId, conversationReaction.EmployeeId, userIdentity.EmployeeId, conversationReaction.LikeReactionId).ConfigureAwait(false);
                        await _commonService.AuditEngagementReport(new CreateEngagementReportRequest { EmployeeId = userIdentity.EmployeeId, EngagementTypeId = AppConstants.Engagement_LikeRecognition })
                        .ConfigureAwait(false);
                    }

                    else if (request.ConversationLikeCreateRequest.ModuleId == (int)LikeModule.CommentsLike || request.ConversationLikeCreateRequest.ModuleId == (int)LikeModule.CommentReplyLike)
                    {
                        await _notificationsService.CommentandReplyLikeNotifications(conversationReaction.ModuleDetailsId, conversationReaction.EmployeeId, userIdentity.EmployeeId, conversationReaction.LikeReactionId, conversationReaction.ModuleId).ConfigureAwait(false);
                    }
                    var conversationReply = await _conversationRepo.FirstOrDefaultAsync(x => x.ConversationId == request.ConversationLikeCreateRequest.ModuleDetailsId);
                    if (conversationReply.Type == (int)ConversationType.ConversationReplyComment && conversationReply.GoalTypeId == 3)
                        await _notificationsService.LikeUserConversation(conversationReaction.ModuleDetailsId, conversationReaction.EmployeeId, userIdentity.EmployeeId, conversationReaction.LikeReactionId).ConfigureAwait(false);
                    else if (conversationReply.Type != (int)ConversationType.ConversationReplyComment && conversationReply.GoalTypeId != 3 && request.ConversationLikeCreateRequest.ModuleId == (int)ModuleId.Conversation)
                        await _notificationsService.LikeUserCommentConversation(conversationReaction.ModuleDetailsId, conversationReaction.EmployeeId, userIdentity.EmployeeId, conversationReaction.LikeReactionId).ConfigureAwait(false);
                }
              
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.ConversationLiked);
            }
            else
            {
                var conversationLike = await _likeReactionRepo.FirstOrDefaultAsync(x => x.EmployeeId == request.ConversationLikeCreateRequest.EmployeeId && x.ModuleDetailsId == request.ConversationLikeCreateRequest.ModuleDetailsId && x.IsActive && x.ModuleId == request.ConversationLikeCreateRequest.ModuleId);
                if (conversationLike != null)
                {
                    conversationLike.IsActive = request.ConversationLikeCreateRequest.IsActive;
                    conversationLike.UpdatedOn = request.UpdatedOn;
                    conversationLike.UpdatedBy = userIdentity.EmployeeId;
                    _likeReactionRepo.Update(conversationLike);
                    await UnitOfWorkAsync.SaveChangesAsync();
                    await DeleteLikeNotifications(conversationLike.LikeReactionId).ConfigureAwait(false);
                }
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.ConversationDisliked);
            }
            await _commonService.CallSignalRForAllEmployees(userIdentity.EmployeeId).ConfigureAwait(false);
            return payload;
        }

        #region Private


        private async Task<Conversation> GetConversationById(long conversationId)
        {
            return await _conversationRepo.FindOneAsync(x => x.ConversationId == conversationId);
        }

        private void DeleteEmployeeTag(ConversationDeleteCommand request, UserIdentity userIdentity)
        {
            var conversationEmployeeTags = _conversationEmployeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == request.ConversationId && x.IsActive && x.ModuleId == (int)ModuleId.Conversation).ToList();
            if (conversationEmployeeTags.Count <= 0) return;
            foreach (var item in conversationEmployeeTags)
            {
                item.IsActive = false;
                item.UpdatedOn = request.UpdatedOn;
                item.UpdatedBy = userIdentity.EmployeeId;
                _conversationEmployeeTagRepo.Update(item);
            }
        }

        private async Task DeleteReplyEmployeeTag(List<long>conversationId, UserIdentity userIdentity)
        {
            var conversationEmployeeTags = await _conversationEmployeeTagRepo.GetQueryable().Where(x => conversationId.Contains(x.ModuleDetailsId) && x.IsActive).ToListAsync();
            if (conversationEmployeeTags.Count <= 0) return;
            foreach (var item in conversationEmployeeTags)
            {
                item.IsActive = false;
                item.UpdatedOn = DateTime.UtcNow ;
                item.UpdatedBy = userIdentity.EmployeeId;
                _conversationEmployeeTagRepo.Update(item);

            }
            await UnitOfWorkAsync.SaveChangesAsync();
        }

        private async Task DeleteFiles(ConversationDeleteCommand request, UserIdentity userIdentity)
        {
            string strFolderName = Configuration.GetValue<string>("AzureBlob:ConversationImageFolderName");
            var conversationFiles = _conversationFileRepo.GetQueryable().Where(x => x.ConversationId == request.ConversationId && x.IsActive).ToList();
            if (conversationFiles.Count > 0)
            {
                foreach (var item in conversationFiles)
                {
                    string deleteAzureLocation = strFolderName + "/" + item.StorageFileName;
                    var cloudBlobDelete = _systemService.GetCloudBlockBlob(deleteAzureLocation);
                    await cloudBlobDelete.DeleteAsync();
                    item.IsActive = false;
                    item.UpdatedOn = request.UpdatedOn;
                    item.UpdatedBy = userIdentity.EmployeeId;
                    _conversationFileRepo.Update(item);

                }
            }
        }

        private async Task DeleteReplyFiles(List<long> conversationId, UserIdentity userIdentity)
        {
            string strFolderName = Configuration.GetValue<string>("AzureBlob:ConversationImageFolderName");
            var conversationFiles = _conversationFileRepo.GetQueryable().Where(x => conversationId.Contains(x.ConversationId) && x.IsActive).ToList();
            if (conversationFiles.Count > 0)
            {
                foreach (var item in conversationFiles)
                {
                    string deleteAzureLocation = strFolderName + "/" + item.StorageFileName;
                    var cloudBlobDelete = _systemService.GetCloudBlockBlob(deleteAzureLocation);
                    await cloudBlobDelete.DeleteAsync();
                    item.IsActive = false;
                    item.UpdatedOn = DateTime.UtcNow;
                    item.UpdatedBy = userIdentity.EmployeeId;
                    _conversationFileRepo.Update(item);

                }
                await UnitOfWorkAsync.SaveChangesAsync();
            }
        }
         
        private async Task DeleteReplyConversation (long conversationId,UserIdentity userIdentity)
        {
            var getReplyConversation = await _conversationRepo.GetQueryable().Where(x => x.GoalId == conversationId && x.Type == (int)ConversationType.ConversationReplyComment && x.GoalTypeId == 3).ToListAsync();
            if (getReplyConversation.Count > 0)
            {
                getReplyConversation.ForEach(a =>
                {
                    a.IsActive = false;
                    a.UpdatedBy = userIdentity.EmployeeId;
                    a.UpdatedOn = DateTime.UtcNow;
                    _conversationRepo.Update(a);
                });
                await UnitOfWorkAsync.SaveChangesAsync();
                await DeleteReplyEmployeeTag(getReplyConversation.Select(x => x.ConversationId).ToList(), userIdentity).ConfigureAwait(false);
                await DeleteReplyFiles(getReplyConversation.Select(x => x.ConversationId).ToList(), userIdentity).ConfigureAwait(false);
                await DeleteReplyConversationNotifications(getReplyConversation.Select(x => x.ConversationId).ToList()).ConfigureAwait(false);

            }
        }

        private void ConversationLog(long loggedInUser, DateTime lastseenOn)
        {
            var conversationLog = _conversationLogRepo.GetQueryable().FirstOrDefault(x => x.EmployeeId == loggedInUser);

            if (conversationLog != null)
            {
                conversationLog.ConversationLastSeenOn = lastseenOn;
                conversationLog.UpdatedBy = loggedInUser;
                conversationLog.UpdatedOn = DateTime.UtcNow;
                _conversationLogRepo.Update(conversationLog);
            }
            else
            {
                _conversationLogRepo.Add(new ConversationLog()
                {
                    EmployeeId = loggedInUser,
                    ConversationLastSeenOn = lastseenOn,
                    IsActive = true,
                    CreatedBy = loggedInUser,
                    CreatedOn = DateTime.UtcNow
                });
            }
            UnitOfWorkAsync.SaveChanges();
        }

        private async Task CreateStorageFile(ConversationCreateRequest request, long userIdentity)
        {
            foreach (var item in request.assignedFiles)
            {
                var converFiles = new ConversationFile()
                {
                    ConversationId = request.ConversationId,
                    FileName = item.FileName.Trim(),
                    FilePath = item.FilePath,
                    StorageFileName = item.StorageFileName,
                    IsActive = true,
                    CreatedBy = userIdentity,
                    CreatedOn = DateTime.UtcNow,
                };
                _conversationFileRepo.Add(converFiles);
            }
            await UnitOfWorkAsync.SaveChangesAsync();
        }
        private async Task CreateEmployeeTags(ConversationCreateRequest request, long userIdentity)
        {
            var empIds = request.employeeTags.Select(x => x.EmployeeId).Distinct().ToList();
            foreach (var item in empIds)
            {
                var converEmpTags = new EmployeeTag()
                {
                    ModuleDetailsId = request.ConversationId,
                    TagId = item,
                    IsActive = true,
                    CreatedBy = userIdentity,
                    CreatedOn = DateTime.UtcNow,
                    ModuleId = (int)ModuleId.Conversation
                };
                _conversationEmployeeTagRepo.Add(converEmpTags);

            }
            await UnitOfWorkAsync.SaveChangesAsync();
        }

        private async Task DeleteLikeNotifications(long likeId)
        {
            var notifications = await _notificationsDetailsRepo.GetQueryable().Where(x => x.NotificationOnId == likeId && x.NotificationOnTypeId == 1).ToListAsync();
            if (notifications.Count > 0)
            {
                notifications.ForEach(a =>
                {
                    a.IsDeleted = true;
                    a.UpdatedOn = DateTime.UtcNow;
                    _notificationsDetailsRepo.Update(a);
                });
                UnitOfWorkAsync.SaveChanges();
            }

        }
        public async Task DeleteReplyConversationNotifications(List<long> conversationId)
        {
            var notifications = await _notificationsDetailsRepo.GetQueryable().Where(x => conversationId.Contains((long)x.NotificationOnId) && x.NotificationOnTypeId == 1 && x.NotificationTypeId == (int)EnumNotificationType.EmployeeTag).ToListAsync();
            if (notifications.Count > 0)
            {
                notifications.ForEach(a =>
                {
                    a.IsDeleted = true;
                    a.UpdatedOn = DateTime.UtcNow;
                    _notificationsDetailsRepo.Update(a);
                });
                _notificationsDetailsRepo.UpdateRange(notifications);
                UnitOfWorkAsync.SaveChanges();
            }

        }

        #endregion

    }
}
