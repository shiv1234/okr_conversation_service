using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.RequestModel.Interface;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Services.Contracts;
using OkrConversationService.Persistence.EntityFrameworkDataAccess;
using OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Services
{
    public class NoteService : BaseService, INoteService
    {
        private readonly IKeyVaultService _keyVaultService;
        private readonly IRepositoryAsync<Note> _noteRepo;
        private readonly IRepositoryAsync<NoteFile> _noteFileRepo;
        private readonly IRepositoryAsync<NoteEmployeeTag> _noteEmployeeTagRepo;
        private readonly INotificationsEmailsService _notificationsService;
        public IConfiguration _configuration { get; set; }
        public ICommonService _iCommonService { get; set; }
        private readonly IRepositoryAsync<GoalKey> _goalKeyRepo;
        private readonly IRepositoryAsync<GoalObjective> _goalObjectiveRepo;
        private readonly IRepositoryAsync<OneToOneDetail> _oneToOneDetailRepo;

        [Obsolete("")]
        public NoteService(IServicesAggregator servicesAggregateService, INotificationsEmailsService notificationsServices, IKeyVaultService keyVaultService, ICommonService commonService, IConfiguration configuration) : base(servicesAggregateService)
        {
            _noteRepo = UnitOfWorkAsync.RepositoryAsync<Note>();
            _noteFileRepo = UnitOfWorkAsync.RepositoryAsync<NoteFile>();
            _noteEmployeeTagRepo = UnitOfWorkAsync.RepositoryAsync<NoteEmployeeTag>();
            _keyVaultService = keyVaultService;
            _notificationsService = notificationsServices;
            _iCommonService = commonService;
            _configuration = configuration;
            _goalKeyRepo = UnitOfWorkAsync.RepositoryAsync<GoalKey>();
            _goalObjectiveRepo = UnitOfWorkAsync.RepositoryAsync<GoalObjective>();
            _oneToOneDetailRepo = UnitOfWorkAsync.RepositoryAsync<OneToOneDetail>();
        }

        public async Task<Payload<NoteResponse>> GetAll(TeamGetAllQuery request)
        {

            var payload = new Payload<NoteResponse>();

            var userIdentity = _iCommonService.GetUserIdentity();
            _iCommonService.noteResponse = await (from note in _noteRepo.GetQueryable()
                               .Where(x => x.IsActive && x.GoalId == request.GoalId && x.GoalTypeId == request.GoalTypeId)
                               .OrderByDescending(x => x.CreatedOn)
                                                  select new NoteResponse
                                                  {
                                                      NoteId = note.NoteId,
                                                      Description = note.Description,
                                                      GoalTypeId = note.GoalTypeId,
                                                      GoalId = note.GoalId,
                                                      CreatedBy = note.CreatedBy,
                                                      CreatedOn = note.CreatedOn,
                                                      UpdatedOn = note.UpdatedOn,
                                                      IsEdited = note.UpdatedOn != null,
                                                      IsReadOnly = userIdentity.EmployeeId != note.CreatedBy,
                                                      IsPrivate = note.IsPrivate,
                                                      NoteFiles = _noteFileRepo.GetQueryable().Where(x => x.NoteId == note.NoteId && x.IsActive).Select(y => new NoteFileResponse()
                                                      {
                                                          NoteFileId = y.NoteFileId,
                                                          FileName = y.FileName,
                                                          FilePath = y.FilePath
                                                      }).ToList(),
                                                      NoteEmployeeTags = _noteEmployeeTagRepo.GetQueryable().Where(x => x.NoteId == note.NoteId && x.IsActive).Select(y => new NoteEmployeeTagResponse()
                                                      {
                                                          NoteEmployeeTagId = y.NoteEmployeeTagId,
                                                          EmployeeId = y.EmployeeId
                                                      }).ToList()
                                                  }).ToListAsync();
            if (_iCommonService.noteResponse.Count > 0)
            {
                request.PageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
                var skipAmount = request.PageSize * (request.PageIndex - 1);
                var totalRecords = _iCommonService.noteResponse.Count;
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

                payload.EntityList = _iCommonService.noteResponse.Skip(skipAmount).Take(request.PageSize).ToList();
                payload.PagingInfo = result;
                payload = GetPayloadStatusSuccess(payload);
            }
            else
            {
                payload = GetPayloadStatusNoContent(payload, "message", ResourceMessage.RecordNotFoundMessage);
            }
            return payload;
        }

        public async Task<Payload<string>> UploadNotesImageOnAzure(UploadFileCommand request)
        {
            var keyVault = await _keyVaultService.GetAzureBlobKeysAsync();
            var payload = new Payload<string>();
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
                var strFolderName = _configuration.GetValue<string>("AzureBlob:NotesImageFolderName");

                var azureLocation = strFolderName + "/" + imageGuid + fileExt;

                var cloudBlobContainer = _iCommonService.GetContainerRefByBlobClient(keyVault.BlobAccountName, keyVault.BlobAccountKey, keyVault.BlobContainerName);

                var fileStream = request.FormFile.OpenReadStream();
                await _iCommonService.UploadBlobByLocation(cloudBlobContainer, azureLocation, request.FormFile.ContentType, fileStream);

                var result = keyVault.BlobCdnUrl + keyVault.BlobContainerName + "/" + azureLocation;
                payload.Entity = result.Trim();
                payload = GetPayloadStatusSuccess(payload);
            }

            return payload;
        }
        public async Task<Payload<NoteCreateRequest>> Create(NoteCreateCommand request)
        {
            var userIdentity = _iCommonService.GetUserIdentity();

            var payload = new Payload<NoteCreateRequest> { Entity = request.NoteCreateRequest };
            //Add Note
            var notes = new Note
            {
                Description = request.NoteCreateRequest.Description,
                GoalId = request.NoteCreateRequest.GoalId,
                GoalTypeId = request.NoteCreateRequest.GoalTypeId,
                IsActive = request.IsActive,
                CreatedBy = userIdentity.EmployeeId,
                CreatedOn = request.CreatedOn,
                IsPrivate = request.NoteCreateRequest.IsPrivate
            };
            _noteRepo.Add(notes);

            if (request.NoteCreateRequest.GoalTypeId == 3 && !request.NoteCreateRequest.IsPrivate)///Request One to One
            {
                var oneToOneDetails = _oneToOneDetailRepo.GetQueryable().FirstOrDefault(x => x.OneToOneDetailId == request.NoteCreateRequest.GoalId);
                if (oneToOneDetails != null)
                {
                    oneToOneDetails.UpdatedBy = userIdentity.EmployeeId;
                    oneToOneDetails.UpdatedOn = request.UpdatedOn;
                    _oneToOneDetailRepo.Update(oneToOneDetails);                   
                    await _notificationsService.UserOneToOneNotifications(oneToOneDetails.RequestedTo, oneToOneDetails.RequestedFrom, userIdentity.EmployeeId, request.NoteCreateRequest.GoalId, oneToOneDetails.OneToOneTitle.Trim()).ConfigureAwait(false);
                }
            }

            var result = await UnitOfWorkAsync.SaveChangesAsync();

            request.NoteCreateRequest.NoteId = notes.NoteId;

            //assigned files
            if (result.Success && request.NoteCreateRequest.assignedFiles.Count > 0)
            {
                foreach (var item in request.NoteCreateRequest.assignedFiles)
                {
                    var noteFiles = new NoteFile()
                    {
                        NoteId = notes.NoteId,
                        FileName = item.FileName.Trim(),
                        FilePath = item.FilePath,
                        StorageFileName = item.StorageFileName,
                        IsActive = request.IsActive,
                        CreatedBy = userIdentity.EmployeeId,
                        CreatedOn = request.CreatedOn,
                    };
                    _noteFileRepo.Add(noteFiles);
                }
            }
            //employee Tags
            if (result.Success && request.NoteCreateRequest.employeeTags.Count > 0)
            {
                var empIds = request.NoteCreateRequest.employeeTags.Select(x => x.EmployeeId).Distinct().ToList();
                foreach (var item in empIds)
                {
                    var empTags = new NoteEmployeeTag()
                    {
                        NoteId = notes.NoteId,
                        EmployeeId = item,
                        IsActive = request.IsActive,
                        CreatedBy = userIdentity.EmployeeId,
                        CreatedOn = request.CreatedOn
                    };
                    _noteEmployeeTagRepo.Add(empTags);

                }

            }

            await UnitOfWorkAsync.SaveChangesAsync();

            if (request.NoteCreateRequest.employeeTags != null && request.NoteCreateRequest.employeeTags.Select(x => x.EmployeeId).ToList().Count > 0)
            {
                await _notificationsService.NoteUserNotificationsAndEmails(request.NoteCreateRequest.employeeTags.Select(x => x.EmployeeId).Distinct().ToList(), userIdentity.EmployeeId, request.NoteCreateRequest.GoalId, request.NoteCreateRequest.GoalTypeId, request.NoteCreateRequest.NoteId,request.NoteCreateRequest.Description).ConfigureAwait(false);

                if (!IsDraftOkrOrKr(request.NoteCreateRequest.GoalId, request.NoteCreateRequest.GoalTypeId))
                {
                    // Call signalR to broadcast
                    var signalrRequestModel = new SignalrRequestModel()
                    {
                        BroadcastValue = request.NoteCreateRequest.employeeTags.Select(x => x.EmployeeId).Distinct().ToList(),
                        BroadcastTopic = AppConstants.TopicNotesTag
                    };
                    await _iCommonService.CallSignalRFunctionForContributors(signalrRequestModel);
                }
            }

            payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.NewNoteCreated);

            // Impersonate Audit Log
            if (userIdentity.IsImpersonatedUser)
            {
                string noteDescription = Regex.Replace(request.NoteCreateRequest.Description, AppConstants.HtmlRejex, String.Empty).Replace(AppConstants.TagNbsp, AppConstants.SingleSpace);
                AuditLogRequest auditLogRequest = new AuditLogRequest
                {
                    ActivityName = AppConstants.AddNote,
                    ActivityDescription = "Note Created - " + noteDescription,
                    TransactionId = request.NoteCreateRequest.NoteId
                };
                await _iCommonService.ImpersonateAuditLog(auditLogRequest);
            }

            //call AuditEngagementReport
            if (request.NoteCreateRequest.GoalTypeId != 3 && !request.NoteCreateRequest.IsPrivate)
            {
                await _iCommonService.AuditEngagementReport(new CreateEngagementReportRequest
                {
                    EmployeeId = userIdentity.EmployeeId,
                    EngagementTypeId = AppConstants.Engagement_Notes
                }).ConfigureAwait(false);
            }
            return payload;
        }

        public async Task<Payload<long>> DeleteNote(NoteDeleteCommand request)
        {
            var payload = new Payload<long>();
            var userIdentity = _iCommonService.GetUserIdentity();
            var existNote = await _noteRepo.GetQueryable()
                .Where(x => x.NoteId == request.NoteId).FirstOrDefaultAsync();

            if (existNote == null)
                return GetPayloadStatus(payload, "noteId", ResourceMessage.RecordNotFound);
            else if (existNote.CreatedBy != userIdentity.EmployeeId)
            {
                return GetPayloadStatus(payload, "noteId", ResourceMessage.CannotDeleteOtherEmployeeNote);
            }
            else
            {
                existNote.IsActive = false;
                existNote.UpdatedBy = userIdentity.EmployeeId;
                existNote.UpdatedOn = DateTime.UtcNow;
                _noteRepo.Update(existNote);

                await DeleteEmployeeTag(request, userIdentity);

                await DeleteFiles(request, userIdentity);

                await UnitOfWorkAsync.SaveChangesAsync();
            }

            payload.Entity = request.NoteId;

            // Impersonate Audit Log
            if (userIdentity.IsImpersonatedUser)
            {
                AuditLogRequest auditLogRequest = new AuditLogRequest
                {
                    ActivityName = AppConstants.DeleteNote,
                    ActivityDescription = "Note Deleted.",
                    TransactionId = request.NoteId
                };
                await _iCommonService.ImpersonateAuditLog(auditLogRequest);
            }

            return GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.NoteDeletedSuccessfully);
        }

        public async Task<Payload<NoteEditRequest>> Edit(NoteEditCommand request)
        {
            var userIdentity = _iCommonService.GetUserIdentity();
            var payload = new Payload<NoteEditRequest> { Entity = request.NoteEditRequest };         
            var noteDetails = await GetNoteById(request.NoteEditRequest.NoteId);
            if (noteDetails == null)
                return GetPayloadStatus(payload, "noteId", ResourceMessage.NoteIdInvalid);

            var noteDetailsJson = JsonConvert.SerializeObject(noteDetails);
            var notedAdded = string.Equals(noteDetails.Description, request.NoteEditRequest.Description);
            if (noteDetails.GoalTypeId == 3 && !noteDetails.IsPrivate)
            {
                var oneToOneDetails = _oneToOneDetailRepo.GetQueryable().FirstOrDefault(x => x.OneToOneDetailId == noteDetails.GoalId);
                if (oneToOneDetails != null)
                {
                    oneToOneDetails.UpdatedBy = userIdentity.EmployeeId;
                    oneToOneDetails.UpdatedOn = request.UpdatedOn;
                    _oneToOneDetailRepo.Update(oneToOneDetails);  
                    if(!notedAdded)
                        await _notificationsService.UserOneToOneNotifications(oneToOneDetails.RequestedTo, oneToOneDetails.RequestedFrom, userIdentity.EmployeeId, noteDetails.GoalId, oneToOneDetails.OneToOneTitle.Trim()).ConfigureAwait(false);

                }
            }
            noteDetails.Description = request.NoteEditRequest.Description;
            noteDetails.IsActive = request.NoteEditRequest.IsActive;
            noteDetails.UpdatedBy = userIdentity.EmployeeId;
            noteDetails.UpdatedOn = request.UpdatedOn;
            noteDetails.IsPrivate = request.NoteEditRequest.IsPrivate;
            _noteRepo.Update(noteDetails);
            var operationStatus = await UnitOfWorkAsync.SaveChangesAsync();

            if (!operationStatus.Success)
                return GetPayloadStatus(payload, "message", ResourceMessage.SomethingWentWrong);

            //assigned files
            await UpdateStorageFile(request.NoteEditRequest, userIdentity.EmployeeId).ConfigureAwait(false);

            //employee Tags
            await UpdateEmployeeTags(request.NoteEditRequest, noteDetails.GoalId, noteDetails.GoalTypeId, userIdentity.EmployeeId).ConfigureAwait(false);


            payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.NoteUpdated);

            // Impersonate Audit Log
            if (userIdentity.IsImpersonatedUser)
            {
                var noteDetailsUpdatedJson = JsonConvert.SerializeObject(noteDetails);
                ///string noteDescription = Regex.Replace(request.NoteEditRequest.Description, AppConstants.HtmlRejex, String.Empty).Replace(AppConstants.TagNbsp, AppConstants.SingleSpace);
                AuditLogRequest auditLogRequest = new AuditLogRequest
                {
                    ActivityName = AppConstants.EditNote,
                    ActivityDescription = "Note Edited - " + noteDetailsUpdatedJson,
                    TransactionId = request.NoteEditRequest.NoteId,
                    ActivityPreviousData = noteDetailsJson

                };
                await _iCommonService.ImpersonateAuditLog(auditLogRequest);
            }
            return payload;

        }

        public async Task<Payload<bool>> IsEmployeeTag(long noteId)
        {
            var userIdentity = _iCommonService.GetUserIdentity();
            var payload = new Payload<bool>();
            _iCommonService.IsNoteEmployeeTag = await _noteEmployeeTagRepo.FirstOrDefaultAsync(x => x.NoteId == noteId && x.EmployeeId == userIdentity.EmployeeId && x.IsActive);
            if (_iCommonService.IsNoteEmployeeTag != null)
                payload.Entity = true;
            else
                payload.Entity = false;

            payload = GetPayloadStatusSuccess(payload);
            return payload;
        }

        public async Task<Payload<bool>> DraftToPublicUserNotificationsAndEmails(List<NoteDraftEmailRequest> goals)
        {
            var userIdentity = _iCommonService.GetUserIdentity();
            var payload = new Payload<bool>();
            foreach (var item in goals)
            {
                var noteDetails = _noteRepo.GetQueryable().Where(x => x.GoalId == item.GoalId && x.GoalTypeId == item.GoalTypeId).Select(x => x.NoteId).ToList();

                foreach (var note in noteDetails)
                {
                    var empIds = _noteEmployeeTagRepo.GetQueryable().Where(x => x.NoteId == note && x.IsActive).Select(x => x.EmployeeId).ToList();
                    await _notificationsService.NoteUserNotificationsAndEmails(empIds, userIdentity.EmployeeId, item.GoalId, item.GoalTypeId, note,"").ConfigureAwait(false);

                    // Call signalR to broadcast
                    var signalrRequestModel = new SignalrRequestModel()
                    {
                        BroadcastValue = empIds,
                        BroadcastTopic = AppConstants.TopicNotesTag
                    };
                    await _iCommonService.CallSignalRFunctionForContributors(signalrRequestModel);
                }
            }
            payload.Entity = true;
            payload = GetPayloadStatusSuccess(payload);
            return payload;

        }

        public async Task<Payload<UserNoteResponse>> GetAllUserNotes(UserNoteAllQuery request)
        {
            var payload = new Payload<UserNoteResponse>();

            var userIdentity = _iCommonService.GetUserIdentity();
            var userResponse = await (from note in _noteRepo.GetQueryable()
                                .Where(x => x.IsActive && x.GoalId == request.GoalId && x.GoalTypeId == request.GoalTypeId)
                                .OrderByDescending(x => x.CreatedOn)
                                      select new UserNoteResponse
                                      {
                                          NoteId = note.NoteId,
                                          Description = note.Description,
                                          GoalTypeId = note.GoalTypeId,
                                          GoalId = note.GoalId,
                                          CreatedBy = note.CreatedBy,
                                          CreatedOn = note.CreatedOn,
                                          UpdatedOn = note.UpdatedOn,
                                          IsEdited = note.UpdatedOn != null,
                                          IsReadOnly = userIdentity.EmployeeId != note.CreatedBy,
                                          IsPrivate = note.IsPrivate
                                      }).ToListAsync();

            if (userResponse.Count > 0)
            {
                userResponse.RemoveAll(x => x.IsPrivate && x.CreatedBy != userIdentity.EmployeeId);
                request.PageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
                var skipAmount = request.PageSize * (request.PageIndex - 1);
                var totalRecords = userResponse.Count;
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

                payload.EntityList = userResponse.Skip(skipAmount).Take(request.PageSize).ToList();
                payload.PagingInfo = result;
                payload = GetPayloadStatusSuccess(payload);
            }
            else
            {
                payload = GetPayloadStatusNoContent(payload, "message", ResourceMessage.RecordNotFoundMessage);
            }
            return payload;
        }

        #region Private
        private async Task<Note> GetNoteById(long noteId)
        {
            return await _noteRepo.FindOneAsync(x => x.NoteId == noteId);
        }
        private async Task DeleteEmployeeTag(NoteDeleteCommand request, IUserIdentity userIdentity)
        {
            var conversationEmployeeTags = await _noteEmployeeTagRepo.GetQueryable().Where(x => x.NoteId == request.NoteId && x.IsActive).ToListAsync();
            if (conversationEmployeeTags.Count > 0)
            {
                foreach (var item in conversationEmployeeTags)
                {
                    item.IsActive = false;
                    item.UpdatedOn = request.UpdatedOn;
                    item.UpdatedBy = userIdentity.EmployeeId;
                    _noteEmployeeTagRepo.Update(item);
                }
            }

        }
        private async Task DeleteFiles(NoteDeleteCommand request, IUserIdentity userIdentity)
        {
            var keyVault = await _keyVaultService.GetAzureBlobKeysAsync();
            string strFolderName = _configuration.GetValue<string>("AzureBlob:NotesImageFolderName");

            var cloudBlobContainer = _iCommonService.GetContainerRefByBlobClient(keyVault.BlobAccountName, keyVault.BlobAccountKey, keyVault.BlobContainerName);

            var conversationFiles = _noteFileRepo.GetQueryable().Where(x => x.NoteId == request.NoteId && x.IsActive).ToList();
            if (conversationFiles.Count > 0)
            {
                foreach (var item in conversationFiles)
                {
                    string deleteAzureLocation = strFolderName + "/" + item.StorageFileName;
                    await _iCommonService.DeleteBlobByLocation(cloudBlobContainer, deleteAzureLocation);

                    item.IsActive = false;
                    item.UpdatedOn = request.UpdatedOn;
                    item.UpdatedBy = userIdentity.EmployeeId;
                    _noteFileRepo.Update(item);
                }
            }
        }
        private bool IsDraftOkrOrKr(long goalId, int goalTypeId)
        {
            bool isDraftGoal = true;
            if (goalTypeId == 1)
            {
                var okr = _goalObjectiveRepo.GetQueryable().AsNoTracking().FirstOrDefault(x => x.GoalObjectiveId == goalId);
                isDraftGoal = okr == null || (okr.GoalStatusId != 2);
            }
            else
            {
                var kr = _goalKeyRepo.GetQueryable().AsNoTracking().FirstOrDefault(x => x.GoalKeyId == goalId);
                isDraftGoal = kr == null || (kr.GoalStatusId != 2);
            }
            return isDraftGoal;
        }
        private async Task UpdateStorageFile(NoteEditRequest request, long userIdentity)
        {
            var noteOldFile = _noteFileRepo.GetQueryable().Where(x => x.NoteId == request.NoteId && x.IsActive).Select(x => x.StorageFileName).ToList();
            var allNoteFiles = await _noteFileRepo.GetQueryable().Where(x => x.NoteId == request.NoteId).ToListAsync();
            foreach (var assignFiles in request.assignedFiles)
            {
                var noteFiles = allNoteFiles.FirstOrDefault(x => x.StorageFileName == assignFiles.StorageFileName);

                if (noteFiles != null)
                {
                    noteFiles.FileName = assignFiles.FileName.Trim();
                    noteFiles.FilePath = assignFiles.FilePath;
                    noteFiles.StorageFileName = assignFiles.StorageFileName;
                    noteFiles.IsActive = request.IsActive;
                    noteFiles.UpdatedBy = userIdentity;
                    noteFiles.UpdatedOn = DateTime.UtcNow;
                    _noteFileRepo.Update(noteFiles);
                }
                else
                {
                    var note = new NoteFile()
                    {
                        NoteId = request.NoteId,
                        FileName = assignFiles.FileName.Trim(),
                        FilePath = assignFiles.FilePath,
                        StorageFileName = assignFiles.StorageFileName,
                        IsActive = request.IsActive,
                        CreatedBy = userIdentity,
                        CreatedOn = DateTime.UtcNow
                    };
                    _noteFileRepo.Add(note);
                }

            }
            await UnitOfWorkAsync.SaveChangesAsync();

            var noteNewFile = request.assignedFiles.Select(x => x.StorageFileName).ToList();
            if (noteOldFile.Count > 0 && noteNewFile.Count > 0)
            {
                var noteFinalList = noteOldFile.Except(noteNewFile).ToList();
                if (noteFinalList.Count > 0)
                {
                    ////update the status of Note File as inactive
                    await InActiveNoteFiles(noteFinalList, userIdentity).ConfigureAwait(false);
                }
            }

            if (noteOldFile.Count > 0 && noteNewFile.Count == 0)
            {
                ////update the status of Note File as inactive
                await InActiveNoteFiles(noteOldFile, userIdentity).ConfigureAwait(false);

            }

        }

        private async Task InActiveNoteFiles(List<string> noteFinalList, long userIdentity)
        {
            var noteFileRecords = await _noteFileRepo.GetQueryable().Where(x => noteFinalList.Contains(x.StorageFileName)).ToListAsync();

            ////update the status of Note File as inactive
            if (noteFileRecords.Count > 0)
            {
                noteFileRecords.ForEach(a =>
                {
                    a.IsActive = false;
                    a.UpdatedBy = userIdentity;
                    a.UpdatedOn = DateTime.UtcNow;
                    _noteFileRepo.Update(a);
                });
                UnitOfWorkAsync.SaveChanges();
            }
        }
        private async Task UpdateEmployeeTags(NoteEditRequest request, long GoalId, int GoalTypeId, long userIdentity)
        {
            var empOldTag = _noteEmployeeTagRepo.GetQueryable().Where(x => x.NoteId == request.NoteId && x.IsActive).Select(x => x.EmployeeId).ToList();
            var allEmployeeTag = await _noteEmployeeTagRepo.GetQueryable().Where(x => x.NoteId == request.NoteId).ToListAsync();
            foreach (var assignUser in request.employeeTags.Select(x => x.EmployeeId).Distinct().ToList())
            {
                var employeeTag = allEmployeeTag.FirstOrDefault(x => x.EmployeeId == assignUser);
                if (employeeTag != null)
                {
                    employeeTag.IsActive = request.IsActive;
                    employeeTag.UpdatedBy = userIdentity;
                    employeeTag.UpdatedOn = DateTime.UtcNow;
                    _noteEmployeeTagRepo.Update(employeeTag);
                }
                else
                {
                    var noteEmployeeTag = new NoteEmployeeTag()
                    {
                        NoteId = request.NoteId,
                        EmployeeId = assignUser,
                        IsActive = request.IsActive,
                        CreatedBy = userIdentity,
                        CreatedOn = DateTime.UtcNow
                    };
                    _noteEmployeeTagRepo.Add(noteEmployeeTag);
                }

            }
            await UnitOfWorkAsync.SaveChangesAsync();
            var empNewTag = request.employeeTags.Select(x => x.EmployeeId).Distinct().ToList();
            if (empOldTag.Count > 0 && empNewTag.Count > 0)
            {
                var empFinalList = empOldTag.Except(empNewTag).ToList();
                if (empFinalList.Count > 0)
                {
                    await InActiveNoteEmployeeTags(empFinalList, userIdentity).ConfigureAwait(false);
                }
            }

            if (empOldTag.Count > 0 && empNewTag.Count == 0)
            {
                await InActiveNoteEmployeeTags(empOldTag, userIdentity).ConfigureAwait(false);
            }

            //mail and notifications   
            await NoteEmployeeTagsMailAndNotifications(request.NoteId, empOldTag, empNewTag, userIdentity, GoalId, GoalTypeId,request.Description).ConfigureAwait(false);

        }

        ////update the status of Employee as inactive
        private async Task InActiveNoteEmployeeTags(List<long> empFinalList, long userIdentity)
        {
            var empFileRecords = await _noteEmployeeTagRepo.GetQueryable().Where(x => empFinalList.Contains(x.EmployeeId)).ToListAsync();

            ////update the status of Note File as inactive
            if (empFileRecords.Count > 0)
            {
                empFileRecords.ForEach(a =>
                {
                    a.IsActive = false;
                    a.UpdatedBy = userIdentity;
                    a.UpdatedOn = DateTime.UtcNow;
                    _noteEmployeeTagRepo.Update(a);
                });
                UnitOfWorkAsync.SaveChanges();
            }
        }

        //mail and notifications   
        private async Task NoteEmployeeTagsMailAndNotifications(long NoteId, List<long> empOldTag, List<long> empNewTag, long userIdentity, long GoalId, int GoalTypeId,string noteDescription)
        {
            if (empOldTag.Count == 0 && empNewTag.Count > 0)
            {
                await _notificationsService.NoteUserNotificationsAndEmails(empNewTag, userIdentity, GoalId, GoalTypeId, NoteId, noteDescription).ConfigureAwait(false);
            }
            if (empOldTag.Count > 0 && empNewTag.Count > 0)
            {
                var empFinalList = empNewTag.Except(empOldTag).ToList();
                if (empFinalList.Count > 0)
                    await _notificationsService.NoteUserNotificationsAndEmails(empFinalList, userIdentity, GoalId, GoalTypeId, NoteId, noteDescription).ConfigureAwait(false);
            }
        }
        #endregion
    }
}