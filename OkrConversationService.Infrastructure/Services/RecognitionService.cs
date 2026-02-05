using Microsoft.EntityFrameworkCore;
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
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Services
{
    public class RecognitionService : BaseService, IRecognitionService
    {

        private readonly IRepositoryAsync<LikeReaction> _likeReactionRepo;
        private readonly IRepositoryAsync<Employee> _employeeRepo;
        private readonly IKeyVaultService _keyVaultService;
        private readonly ICommonService _commonService;
        private readonly INotificationsEmailsService _notificationsService;
        private readonly ISystemService _systemService;
        private readonly IRepositoryAsync<CommentDetails> _commentDetailsRepo;
        private readonly IRepositoryAsync<Recognition> _recognitionRepo;
        private readonly IRepositoryAsync<RecognitionImageMapping> _recognitionImageMappingRepo;
        private readonly IRepositoryAsync<RecognitionCategory> _recognitionCategoryRepo;
        private readonly IRepositoryAsync<NotificationsDetail> _notificationsDetailsRepo;
        private readonly IRepositoryAsync<EmployeeTeamMapping> _employeeTeamMappingRepo;
        private readonly IRepositoryAsync<Team> _teamRepo;
        private readonly IRepositoryAsync<EmployeeTag> _employeeTagRepo;
        private readonly IRepositoryAsync<RecognitionEmployeeTeamMapping> _recognitionEmployeeTeamMappingRepo;
        private readonly IRepositoryAsync<EmployeeEngagement> _employeeEngagementRepo;
        public RecognitionService(IServicesAggregator servicesAggregateService,
            INotificationsEmailsService notificationsServices, IKeyVaultService keyVaultService, ICommonService commonService, ISystemService systemService) : base(servicesAggregateService)
        {

            _likeReactionRepo = UnitOfWorkAsync.RepositoryAsync<LikeReaction>();
            _employeeRepo = UnitOfWorkAsync.RepositoryAsync<Employee>();
            _keyVaultService = keyVaultService;
            _notificationsService = notificationsServices;
            _commonService = commonService;
            _systemService = systemService;
            _commentDetailsRepo = UnitOfWorkAsync.RepositoryAsync<CommentDetails>();
            _recognitionRepo = UnitOfWorkAsync.RepositoryAsync<Recognition>();
            _recognitionImageMappingRepo = UnitOfWorkAsync.RepositoryAsync<RecognitionImageMapping>();
            _recognitionCategoryRepo = UnitOfWorkAsync.RepositoryAsync<RecognitionCategory>();
            _notificationsDetailsRepo = UnitOfWorkAsync.RepositoryAsync<NotificationsDetail>();
            _employeeTeamMappingRepo = UnitOfWorkAsync.RepositoryAsync<EmployeeTeamMapping>();
            _teamRepo = UnitOfWorkAsync.RepositoryAsync<Team>();
            _employeeTagRepo = UnitOfWorkAsync.RepositoryAsync<EmployeeTag>();
            _recognitionEmployeeTeamMappingRepo = UnitOfWorkAsync.RepositoryAsync<RecognitionEmployeeTeamMapping>();
            _employeeEngagementRepo = UnitOfWorkAsync.RepositoryAsync<EmployeeEngagement>();

        }

        public async Task<Payload<RecognitionReactionResponse>> GetRecognitionLike(RecognitionLikeQuery request)
        {
            var payload = new Payload<RecognitionReactionResponse>();
            var userIdentity = _commonService.GetUserIdentity();
            var recognitionLike = new List<RecognitionLikeResponse>();
            var likeReactions = await _likeReactionRepo.GetQueryable().Where(x => x.ModuleDetailsId == request.ModuleDetailsId && x.IsActive && x.ModuleId == request.ModuleId).ToListAsync();
            var empIds = likeReactions.Select(x => x.EmployeeId).ToList();
            var empDetails = _employeeRepo.GetQueryable().Where(x => empIds.Contains(x.EmployeeId)).ToList();
            foreach (var item in likeReactions)
            {
                var userDetails = empDetails.FirstOrDefault(x => x.EmployeeId == item.EmployeeId);
                recognitionLike.Add(new RecognitionLikeResponse
                {
                    EmployeeId = item.EmployeeId,
                    LastName = userDetails.LastName,
                    FirstName = userDetails.FirstName,
                    FullName = userDetails.FirstName + " " + userDetails.LastName,
                    ImagePath = userDetails.ImagePath,
                    LikeReactionId = item.LikeReactionId,
                    EmailId = userDetails.EmailId
                });

            }
            var recognitionReactionResponse = new RecognitionReactionResponse
            {
                IsLiked = likeReactions.FirstOrDefault(y => y.EmployeeId == userIdentity.EmployeeId) != null,
                TotalLikeCount = likeReactions.Count,
                RecognitionLikeResponses = recognitionLike
            };

            payload.Entity = recognitionReactionResponse;
            payload = GetPayloadStatusSuccess(payload);
            return payload;
        }

        public async Task<Payload<CommentDetailsRequest>> CreateComments(CommentCreateCommand request)
        {
            var userIdentity = _commonService.GetUserIdentity();
            var payload = new Payload<CommentDetailsRequest> { Entity = request.CommentDetailsRequest };
            var commentInsert = new CommentDetails();
            var empIds = new List<long>();
            var commentResponse = new List<long>();
            string blockedWords = string.Empty;
            var blockedResponse = await _commonService.IsBlockedWords(request.CommentDetailsRequest.Comments);
            if (blockedResponse.IsSuccess)
            {
                if (blockedResponse.MessageList.ToList().Any())
                    blockedWords = blockedResponse.MessageList.FirstOrDefault().Value;
                payload.MessageList.Add(AppConstants.BlockedWords,
                    string.Format(ResourceMessage.BlockedWordErrorMessage, blockedWords));
                payload.Status = (int)HttpStatusCode.BadRequest;
                payload.IsSuccess = false;
                return payload;
            }
            if (request.CommentDetailsRequest.CommentDetailsId == 0)
            {
                var comment = new CommentDetails
                {
                    Comments = request.CommentDetailsRequest.Comments,
                    ModuleDetailsId = request.CommentDetailsRequest.ModuleDetailsId,
                    ModuleId = request.CommentDetailsRequest.ModuleId,
                    CreatedOn = request.CreatedOn,
                    CreatedBy = userIdentity.EmployeeId,
                    IsActive = request.IsActive
                };
                _commentDetailsRepo.Add(comment);
                await UnitOfWorkAsync.SaveChangesAsync();
                commentInsert = comment;
                if (request.CommentDetailsRequest.ModuleId == (int)CommentModuleId.Recognisation)
                {
                    commentResponse = await _notificationsService.CommentNotifications(request.CommentDetailsRequest.ModuleDetailsId, userIdentity.EmployeeId, 0, comment.CommentDetailsId).ConfigureAwait(false);
                    payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.CommentCreated);
                    await _commonService.AuditEngagementReport(new CreateEngagementReportRequest { EmployeeId = userIdentity.EmployeeId, EngagementTypeId = AppConstants.Engagement_CommentRecognition })
                           .ConfigureAwait(false);
                }
                else if (request.CommentDetailsRequest.ModuleId == (int)CommentModuleId.ReplyComments)
                {
                    payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.CommentReplyCreated);
                }
                    
            }
            else
            {
                var comments = await _commentDetailsRepo.FirstOrDefaultAsync(x => x.CommentDetailsId == request.CommentDetailsRequest.CommentDetailsId && x.IsActive);
                comments.Comments = request.CommentDetailsRequest.Comments;
                comments.UpdatedBy = userIdentity.EmployeeId;
                comments.UpdatedOn = request.UpdatedOn;
                _commentDetailsRepo.Update(comments);
                await UnitOfWorkAsync.SaveChangesAsync();
                if (request.CommentDetailsRequest.ModuleId == (int)CommentModuleId.Recognisation)
                {
                    commentResponse = await _notificationsService.CommentNotifications(comments.ModuleDetailsId, userIdentity.EmployeeId, comments.CommentDetailsId, comments.CommentDetailsId).ConfigureAwait(false);
                    payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.CommentUpdated);
                }
                else if (request.CommentDetailsRequest.ModuleId == (int)CommentModuleId.ReplyComments)
                {
                    payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.CommentReplyUpdated);
                }
            }
            await CommentImage(request, userIdentity).ConfigureAwait(false);
            var commentTagId = await CommentTags(request, userIdentity, commentInsert.CommentDetailsId, commentResponse);
            empIds.AddRange(commentTagId);
             if (request.CommentDetailsRequest.ModuleId == (int)CommentModuleId.ReplyComments && request.CommentDetailsRequest.CommentDetailsId == 0)
                await _notificationsService.CommentRepliesNotifications(request.CommentDetailsRequest.ModuleDetailsId, userIdentity.EmployeeId, commentInsert.CommentDetailsId).ConfigureAwait(false);
            await _commonService.CallSignalRForEditRecognition(empIds.Distinct().ToList()).ConfigureAwait(false);
            await _commonService.CallSignalRForAllEmployees(userIdentity.EmployeeId).ConfigureAwait(false);
            return payload;
        }
        public async Task<Payload<bool>> DeleteComment(CommentDeleteCommand request)
        {
            var userIdentity = _commonService.GetUserIdentity();
            var moduleId = 0;
            var teamModuleId = 0;
            var recognitionCategoryId = 0;
            var payload = new Payload<bool> { Entity = false };
            var getComments = await _commentDetailsRepo.FirstOrDefaultAsync(x => x.IsActive && x.CommentDetailsId == request.CommentDetailsId);
            var getReplyComments = await _commentDetailsRepo.GetQueryable().Where(x => x.IsActive && x.ModuleDetailsId == request.CommentDetailsId).ToListAsync();
            if (getComments == null)
                return GetPayloadStatus(payload, "commentDetailsId", ResourceMessage.RecordNotFound);
            else
            {
              
                if (getComments.ModuleId == (int)CommentModuleId.Recognisation)
                {
                    moduleId = (int)ModuleId.Comments;
                    teamModuleId = (int)ModuleId.TeamTagInComment;
                    recognitionCategoryId = (int)RecognitionCategoryType.RecognitionCommentScreenshot;
                }
                else if (getComments.ModuleId == (int)CommentModuleId.ReplyComments)
                {
                    moduleId = (int)ModuleId.ReplyComments;
                    teamModuleId = (int)ModuleId.ReplyTeamComments;
                    recognitionCategoryId = (int)RecognitionCategoryType.RecognitionReplyCommentScreenshot;
                }
                getComments.IsActive = false;
                getComments.UpdatedBy = userIdentity.EmployeeId;
                getComments.UpdatedOn = request.UpdatedOn;
                _commentDetailsRepo.Update(getComments);
                var images = await _recognitionImageMappingRepo.GetQueryable().Where(x => x.RecognitionId == getComments.ModuleDetailsId && x.RecognitionCategoryTypeId == recognitionCategoryId && x.IsActive).ToListAsync();
                if (images.Count > 0)
                {
                    images.ForEach(a =>
                    {
                        a.IsActive = false;
                        a.UpdatedBy = userIdentity.EmployeeId;
                        a.UpdatedOn = request.UpdatedOn;
                        _recognitionImageMappingRepo.Update(a);
                    });
                    UnitOfWorkAsync.SaveChanges();
                }
                var commentTags = await _employeeTagRepo.GetQueryable().Where(x => x.IsActive && x.ModuleDetailsId == request.CommentDetailsId && (x.ModuleId == moduleId || x.ModuleId == teamModuleId)).ToListAsync();
                if (commentTags.Count > 0)
                {
                    commentTags.ForEach(a =>
                    {
                        a.IsActive = false;
                        a.UpdatedBy = userIdentity.EmployeeId;
                        a.UpdatedOn = request.UpdatedOn;
                        _employeeTagRepo.Update(a);
                    });
                    UnitOfWorkAsync.SaveChanges();
                }

                await UnitOfWorkAsync.SaveChangesAsync();
                payload.Entity = true;
                var ids = new List<long> { getComments.CommentDetailsId };
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", getComments.ModuleId == (int)CommentModuleId.Recognisation? ResourceMessage.CommentDeleted : ResourceMessage.ReplyDeleted);
                await DeleteCommentNotifications(ids).ConfigureAwait(false);
                await _commonService.CallSignalRForAllEmployees(userIdentity.EmployeeId).ConfigureAwait(false);
            }
            if (getReplyComments.Count > 0)
            {
                getReplyComments.ForEach(a =>
                {
                    a.IsActive = false;
                    a.UpdatedOn = DateTime.UtcNow;
                    _commentDetailsRepo.Update(a);
                });
                _commentDetailsRepo.UpdateRange(getReplyComments);

                var images = await _recognitionImageMappingRepo.GetQueryable().Where(x => getReplyComments.Select(x=>x.ModuleDetailsId).Contains(x.RecognitionId) && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.RecognitionReplyCommentScreenshot && x.IsActive).ToListAsync();
                if (images.Count > 0)
                {
                    images.ForEach(a =>
                    {
                        a.IsActive = false;
                        a.UpdatedBy = userIdentity.EmployeeId;
                        a.UpdatedOn = request.UpdatedOn;
                        _recognitionImageMappingRepo.Update(a);
                    });
                   
                }
                var commentTags = await _employeeTagRepo.GetQueryable().Where(x => x.IsActive && getReplyComments.Select(x => x.CommentDetailsId).Contains(x.ModuleDetailsId) && (x.ModuleId == (int)ModuleId.ReplyComments || x.ModuleId == (int)ModuleId.ReplyTeamComments)).ToListAsync();
                if (commentTags.Count > 0)
                {
                    commentTags.ForEach(a =>
                    {
                        a.IsActive = false;
                        a.UpdatedBy = userIdentity.EmployeeId;
                        a.UpdatedOn = request.UpdatedOn;
                        _employeeTagRepo.Update(a);
                    });
                  
                }
                await UnitOfWorkAsync.SaveChangesAsync();
                await DeleteReplyCommentNotifications(getReplyComments.Select(x=>x.CommentDetailsId).ToList()).ConfigureAwait(false);
            }
            return payload;
        }
        public async Task<Payload<CommentResponse>> GetComments(GetCommentQuery request)
        {
            var commentDetail = new List<CommentDetailResponse>();
            var userIdentity = _commonService.GetUserIdentity();
            var payload = new Payload<CommentResponse> { };
            List<CommentDetails> comments = new List<CommentDetails>();
            var commentList = await _commentDetailsRepo.GetQueryable().Where(x => x.ModuleDetailsId == request.ModuleDetailsId && x.IsActive).ToListAsync();
            if (request.ModuleId == (int)CommentModuleId.Recognisation)
                comments = commentList.Where(x => x.ModuleId == (int)CommentModuleId.Recognisation).OrderByDescending(x => x.CreatedOn).ToList();
            else if (request.ModuleId == (int)CommentModuleId.ReplyComments)
                comments = commentList.Where(x => x.ModuleId == (int)CommentModuleId.ReplyComments).OrderBy(x => x.CreatedOn).ToList();
            var likes = await _likeReactionRepo.GetQueryable().Where(x => (x.ModuleId == (int)LikeModule.CommentsLike || x.ModuleId == (int)LikeModule.CommentReplyLike) && x.IsActive).ToListAsync();
            var totalReplies = await _commentDetailsRepo.GetQueryable().Where(x => x.IsActive && x.ModuleId == (int)CommentModuleId.ReplyComments).ToListAsync();
            var createdDetails = comments.Select(x => x.CreatedBy).ToList();
            createdDetails.AddRange(totalReplies.Select(x => x.CreatedBy).ToList());
            var empDetails = await _employeeRepo.GetQueryable().Where(x => createdDetails.Contains(x.EmployeeId)).ToListAsync();
            var commentResponse = new CommentResponse
            {
                TotalComments = 0,               
               
                CommentDetailResponses = new List<CommentDetailResponse>()
            };
            if (comments.Count > 0)
            {
                foreach (var item in comments)
                {
                    var replyFirstName = "";
                    var replyLastName = "";
                    var replyImagePath = "";
                    var employeeDetails = empDetails.FirstOrDefault(x => x.EmployeeId == item.CreatedBy);
                    var likemoduleId = item.ModuleId == (int)CommentModuleId.Recognisation ? (int)LikeModule.CommentsLike : (int)LikeModule.CommentReplyLike;
                    var totalRepliesCount = item.ModuleId == (int)CommentModuleId.Recognisation ? totalReplies.Count(x => x.ModuleDetailsId == item.CommentDetailsId) : 0;
                    if (totalRepliesCount == 1 && item.ModuleId == (int)CommentModuleId.Recognisation)
                    {
                        var replyDetails = totalReplies.FirstOrDefault(x => x.ModuleDetailsId == item.CommentDetailsId);
                        replyFirstName = empDetails.FirstOrDefault(x => x.EmployeeId == replyDetails.CreatedBy).FirstName;
                        replyLastName = empDetails.FirstOrDefault(x => x.EmployeeId == replyDetails.CreatedBy).LastName;
                        replyImagePath = empDetails.FirstOrDefault(x => x.EmployeeId == replyDetails.CreatedBy).ImagePath;
                    }
                    commentDetail.Add(new CommentDetailResponse
                    {
                        CommentDetailsId = item.CommentDetailsId,
                        Comments = item.Comments,
                        CreatedOn = item.CreatedOn,
                        UpdatedOn = item.UpdatedOn,
                        ModuleDetailsId = item.ModuleDetailsId,
                        FirstName = employeeDetails.FirstName,
                        LastName = employeeDetails.LastName,
                        ImagePath = employeeDetails.ImagePath,
                        FullName = employeeDetails.FirstName + " " + employeeDetails.LastName,
                        EmployeeId = employeeDetails.EmployeeId,
                        IsEdited = item.UpdatedOn != null && item.CreatedOn != item.UpdatedOn,
                        ModuleId = item.ModuleId,
                        TotalLikes = likes.Count(x => x.ModuleDetailsId == item.CommentDetailsId && x.ModuleId == likemoduleId),
                        TotalReplies = totalRepliesCount,
                        IsLiked = likes.Any(x => x.EmployeeId == userIdentity.EmployeeId && x.ModuleDetailsId == item.CommentDetailsId && x.ModuleId == likemoduleId),
                        ReplyFirstName = replyFirstName,
                        ReplyLastName = replyLastName,
                        ReplyFullName = replyFirstName + " " + replyLastName,
                        ReplyImagePath = replyImagePath
                    });
                }
                request.PageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
                var skipAmount = request.PageSize * (request.PageIndex - 1);
                var totalRecords = commentDetail.Count;
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
                var commentsResponse = commentDetail.Skip(skipAmount).Take(request.PageSize).ToList();
                commentResponse.TotalComments = commentList.Count(x => x.ModuleId == (int)CommentModuleId.Recognisation);
                commentResponse.CommentDetailResponses = commentsResponse;
                payload.Entity = commentResponse;
                payload.PagingInfo = result;
                payload.IsSuccess = true;
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.CommentSuccess);
            }
            else
            {
                payload = GetPayloadStatusSuccess(payload, "moduleDetailsId", ResourceMessage.RecordNotFound);
                payload.Entity = commentResponse;
            }
            return payload;
        }
        public async Task<Payload<RecognitionCreateRequest>> Create(RecognitionCreateCommand request)
        {
            var userIdentity = _commonService.GetUserIdentity();
            var badges = new RecognitionCategory();
            var payload = new Payload<RecognitionCreateRequest> { Entity = request.RecognitionRequest };
            string blockedWords = string.Empty;
            string fileName = string.Empty;
            var blockedResponse = await _commonService.IsBlockedWords(request.RecognitionRequest.Message);
            if (blockedResponse.IsSuccess)
            {
                if (blockedResponse.MessageList.ToList().Any())
                    blockedWords = blockedResponse.MessageList.FirstOrDefault().Value;
                payload.MessageList.Add(AppConstants.BlockedWords,
                    string.Format(ResourceMessage.BlockedWordErrorMessage, blockedWords));
                payload.Status = (int)HttpStatusCode.BadRequest;
                payload.IsSuccess = false;
                return payload;
            }
            if (request.RecognitionRequest.IsAttachment)
                badges = await _recognitionCategoryRepo.FirstOrDefaultAsync(x => x.RecognitionCategoryId == request.RecognitionRequest.RecognitionCategoryId);
            var recognition = new Recognition
            {
                Message = request.RecognitionRequest.Message,
                CreatedBy = userIdentity.EmployeeId,
                CreatedOn = request.CreatedOn,
                IsActive = request.IsActive,
                IsAttachment = request.RecognitionRequest.IsAttachment,
                RecognitionCategoryTypeId = request.RecognitionRequest.RecognitionCategoryTypeId,
                IsGivenByManager = false,
                UpdatedOn = request.CreatedOn
            };
            _recognitionRepo.Add(recognition);
            await UnitOfWorkAsync.SaveChangesAsync();
            if (request.RecognitionRequest.IsAttachment)
            {
                var recognitionImageMapping = new RecognitionImageMapping
                {
                    RecognitionId = recognition.RecognitionId,
                    RecognitionCategoryId = request.RecognitionRequest.RecognitionCategoryId,
                    RecognitionCategoryTypeId = request.RecognitionRequest.RecognitionCategoryTypeId,
                    IsActive = request.IsActive,
                    CreatedBy = userIdentity.EmployeeId,
                    CreatedOn = request.CreatedOn,
                    FileName = badges.FileName,
                    GuidFileName = badges.GuidFileName,
                    Name = badges.Name,
                    UpdatedOn = request.CreatedOn
                };
                _recognitionImageMappingRepo.Add(recognitionImageMapping);
                 await UnitOfWorkAsync.SaveChangesAsync();
                fileName = recognitionImageMapping.GuidFileName;
            }
            await CreateEmployeeTags(request.RecognitionRequest, userIdentity, recognition.RecognitionId).ConfigureAwait(false);
            await CreateTeamRecognitionMapping(request.RecognitionRequest.ReceiverRequest, recognition.RecognitionId, userIdentity).ConfigureAwait(false);
            if (request.RecognitionRequest.RecognitionImageRequests.Count > 0)
            {
                foreach (var item in request.RecognitionRequest.RecognitionImageRequests)
                {
                    var recognitionImageMapping = new RecognitionImageMapping
                    {
                        RecognitionId = recognition.RecognitionId,
                        RecognitionCategoryId = 0,
                        RecognitionCategoryTypeId = (int)RecognitionCategoryType.RecognitionScreenshot,
                        IsActive = request.IsActive,
                        CreatedBy = userIdentity.EmployeeId,
                        CreatedOn = request.CreatedOn,
                        FileName = item.FileName,
                        GuidFileName = item.GuidFileName,
                        Name = "",
                        UpdatedOn = request.CreatedOn
                    };
                    _recognitionImageMappingRepo.Add(recognitionImageMapping);

                }
                await UnitOfWorkAsync.SaveChangesAsync();
            }
            await _notificationsService.CreateRecognitionNotifications(userIdentity, recognition.RecognitionId, request.RecognitionRequest.IsAttachment, badges.Name, new List<long>(),fileName, request.RecognitionRequest.Message).ConfigureAwait(false);
            if (request.RecognitionRequest.RecognitionEmployeeTags.Count > 0)
                await _notificationsService.TagPostNotifications(userIdentity, recognition.RecognitionId).ConfigureAwait(false);
            await _commonService.CallSignalRforNotifications(recognition.RecognitionId, userIdentity).ConfigureAwait(false);
            await _commonService.CallSignalRForAllEmployees(userIdentity.EmployeeId).ConfigureAwait(false);
            //call AuditEngagementReport
            await _commonService.AuditEngagementReport(new CreateEngagementReportRequest { EmployeeId = userIdentity.EmployeeId, EngagementTypeId = AppConstants.Engagement_ShareRecognition })
                .ConfigureAwait(false);
            payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.RecognitionSaved);
            return payload;
        }
        public async Task<Payload<RecognitionEditRequest>> EditRecognition(RecognitionEditCommand request)
        {
            var userIdentity = _commonService.GetUserIdentity();
            var badges = new RecognitionImageMapping();
            var payload = new Payload<RecognitionEditRequest> { Entity = request.RecognitionEditRequest };
            string blockedWords = string.Empty;
            var blockedResponse = await _commonService.IsBlockedWords(request.RecognitionEditRequest.Message);
            if (blockedResponse.IsSuccess)
            {
                if (blockedResponse.MessageList.ToList().Any())
                    blockedWords = blockedResponse.MessageList.FirstOrDefault().Value;
                payload.MessageList.Add(AppConstants.BlockedWords,
                    string.Format(ResourceMessage.BlockedWordErrorMessage, blockedWords));
                payload.Status = (int)HttpStatusCode.BadRequest;
                payload.IsSuccess = false;
                return payload;
            }

            bool isBadgeUpdated = false;
            bool isContentChange = false;
            bool isReceiverChange = false;
            var empIds = new List<long>();
            var recognitionDetails = await _recognitionRepo.FirstOrDefaultAsync(x => x.RecognitionId == request.RecognitionEditRequest.RecognitionId && x.IsActive);
            var receiverDetails = await _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == request.RecognitionEditRequest.RecognitionId && (x.ModuleId == (int)ModuleId.EmployeeReceiver || x.ModuleId == (int)ModuleId.TeamReceiver)).ToListAsync();
            var recognitionCategory = await _recognitionCategoryRepo.FirstOrDefaultAsync(x => x.RecognitionCategoryId == request.RecognitionEditRequest.RecognitionCategoryId);
            if (recognitionDetails == null) return payload;
            if (request.RecognitionEditRequest.IsAttachment)
            {
                badges = await _recognitionImageMappingRepo.FirstOrDefaultAsync(x => x.RecognitionId == request.RecognitionEditRequest.RecognitionId && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.Badge && x.IsActive);
                await DeleteRecognitionImage(request.RecognitionEditRequest.RecognitionId, badges, userIdentity, request.RecognitionEditRequest.IsAttachment, recognitionCategory).ConfigureAwait(false);
                if (badges != null)
                {
                    if (request.RecognitionEditRequest.RecognitionCategoryId != badges.RecognitionCategoryId)
                    {
                        isBadgeUpdated = true;
                    }
                }
            }
            else if (!request.RecognitionEditRequest.IsAttachment)
            {
                badges = await _recognitionImageMappingRepo.FirstOrDefaultAsync(x => x.RecognitionId == request.RecognitionEditRequest.RecognitionId && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.Badge && x.IsActive);
                if (badges != null)
                {
                    badges.IsActive = false;
                    badges.UpdatedBy = userIdentity.EmployeeId;
                    badges.UpdatedOn = DateTime.UtcNow;
                    _recognitionImageMappingRepo.Update(badges);
                }

            }
            var receiverEmpIds = new List<long>();
            var oldReceiverEmpIds = new List<long>();
            var requestreceiver = request.RecognitionEditRequest.ReceiverRequest.Where(x => x.SearchType == 2).Select(x => x.Id).ToList();
            foreach (var item in requestreceiver)
            {
                var receiverTeamPresemt = receiverDetails.FirstOrDefault(x => x.TagId == item && x.ModuleId == (int)ModuleId.TeamReceiver && x.ModuleDetailsId == request.RecognitionEditRequest.RecognitionId && x.IsActive);
                if (receiverTeamPresemt == null)
                {
                    receiverEmpIds.Add(item);
                    isReceiverChange = true;
                }
                else
                {
                    oldReceiverEmpIds.Add(receiverTeamPresemt.TagId);
                }

            }
            var empRequestReceiver = request.RecognitionEditRequest.ReceiverRequest.Where(x => x.SearchType == 1).Select(x => x.Id).ToList();
            foreach (var item in empRequestReceiver)
            {
                var receiverEmpPresent = receiverDetails.FirstOrDefault(x => x.TagId == item && x.ModuleId == (int)ModuleId.EmployeeReceiver && x.ModuleDetailsId == request.RecognitionEditRequest.RecognitionId && x.IsActive);
                if (receiverEmpPresent == null)
                {
                    receiverEmpIds.Add(item);
                    isReceiverChange = true;
                }
                else
                {
                    oldReceiverEmpIds.Add(receiverEmpPresent.TagId);
                }
            }
            var deleteReceiverEmpIds = _recognitionEmployeeTeamMappingRepo.GetQueryable().Where(x => x.RecognitionId == request.RecognitionEditRequest.RecognitionId && x.IsActive && !(oldReceiverEmpIds.Contains((long)x.TeamId) || oldReceiverEmpIds.Contains(x.EmployeeId))).ToList();
            var deletedIds = deleteReceiverEmpIds.Select(x => x.EmployeeId).ToList();
            if (deletedIds.Count > 0)
            {
                await DeleteNotificationsReceiverId(request.RecognitionEditRequest.RecognitionId, deletedIds).ConfigureAwait(false);
            }

            if (request.RecognitionEditRequest.IsContentChange)
            {
                isContentChange = true;

            }

            await DeleteRecognitionScreenshot(request, userIdentity).ConfigureAwait(false);

            //EmployeeTags
            var tagEmpIds = await UpdateEmployeeTag(request, userIdentity).ConfigureAwait(false);
            await UpdateRecognitionMapping(request.RecognitionEditRequest, userIdentity).ConfigureAwait(false);
            recognitionDetails.Message = request.RecognitionEditRequest.Message;
            recognitionDetails.RecognitionCategoryTypeId = request.RecognitionEditRequest.RecognitionCategoryTypeId;
            recognitionDetails.IsAttachment = request.RecognitionEditRequest.IsAttachment;
            recognitionDetails.IsGivenByManager = false;
            recognitionDetails.UpdatedBy = userIdentity.EmployeeId;
            recognitionDetails.UpdatedOn = request.UpdatedOn;
            _recognitionRepo.Update(recognitionDetails);
            await UnitOfWorkAsync.SaveChangesAsync();
            if (isReceiverChange)
            {
                await _notificationsService.CreateRecognitionNotifications(userIdentity, request.RecognitionEditRequest.RecognitionId, request.RecognitionEditRequest.IsAttachment, recognitionCategory == null ? "" : recognitionCategory.Name, receiverEmpIds,recognitionCategory == null ? "" : recognitionCategory.GuidFileName,request.RecognitionEditRequest.Message).ConfigureAwait(false);
                empIds.AddRange(receiverEmpIds);
            }
            else if (isBadgeUpdated || isContentChange)
            {
                var deletedContent = _recognitionEmployeeTeamMappingRepo.GetQueryable().Where(x => x.RecognitionId == request.RecognitionEditRequest.RecognitionId && x.IsActive && !(receiverEmpIds.Contains((long)x.TeamId) || receiverEmpIds.Contains(x.EmployeeId))).Select(x => x.EmployeeId).ToList();
                var empDetails = await _recognitionEmployeeTeamMappingRepo.GetQueryable().Where(x => x.RecognitionId == request.RecognitionEditRequest.RecognitionId).Select(x => x.EmployeeId).ToListAsync();
                var contentEmpIds = isReceiverChange ? deletedContent : empDetails;
                contentEmpIds.Remove(userIdentity.EmployeeId);
                await _notificationsService.UpdateNotificationsBadges(empDetails.Distinct().ToList(), userIdentity, request.RecognitionEditRequest.RecognitionId).ConfigureAwait(false);
                empIds.AddRange(empDetails);
            }
            empIds.AddRange(tagEmpIds);
            empIds.Remove(userIdentity.EmployeeId);
            await _commonService.CallSignalRForEditRecognition(empIds.Distinct().ToList()).ConfigureAwait(false);
            await _commonService.CallSignalRForAllEmployees(userIdentity.EmployeeId).ConfigureAwait(false);
            payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.RecognitionSaved);
            return payload;
        }

        private async Task UpdateRecognitionMapping(RecognitionEditRequest request, UserIdentity userIdentity)
        {
            var deleteRecognitionMapping = await _recognitionEmployeeTeamMappingRepo.GetQueryable().Where(x => x.RecognitionId == request.RecognitionId).ToListAsync();
            if (deleteRecognitionMapping.Count > 0)
            {
                deleteRecognitionMapping.ForEach(a =>
                {
                    a.IsActive = false;
                });
                _recognitionEmployeeTeamMappingRepo.UpdateRange(deleteRecognitionMapping);
            }

            var deleteEmployeeTag = await _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == request.RecognitionId && x.IsActive && (x.ModuleId == (int)ModuleId.EmployeeReceiver || x.ModuleId == (int)ModuleId.TeamReceiver)).ToListAsync();
            if (deleteEmployeeTag.Count > 0)
            {
                deleteEmployeeTag.ForEach(a =>
                {
                    a.IsActive = false;
                });
                _employeeTagRepo.UpdateRange(deleteEmployeeTag);
            }
            var employeeTagRequest = new List<EmployeeTag>();
            foreach (var item in request.ReceiverRequest.Distinct())
            {
                var recogEmpTags = new EmployeeTag()
                {
                    ModuleDetailsId = request.RecognitionId,
                    TagId = item.Id,
                    IsActive = true,
                    CreatedBy = userIdentity.EmployeeId,
                    CreatedOn = DateTime.UtcNow,
                    ModuleId = item.SearchType == 1 ? (int)ModuleId.EmployeeReceiver : (int)ModuleId.TeamReceiver
                };
                employeeTagRequest.Add(recogEmpTags);
            }
            await _employeeTagRepo.AddRangeAsync(employeeTagRequest).ConfigureAwait(false);
            await UnitOfWorkAsync.SaveChangesAsync();
            await CreateTeamRecognitionMapping(request.ReceiverRequest, request.RecognitionId, userIdentity).ConfigureAwait(false);
        }
        private async Task<List<long>> UpdateEmployeeTag(RecognitionEditCommand request, UserIdentity userIdentity)
        {
            var empIds = new List<long>();
            var empOldTag = _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == request.RecognitionEditRequest.RecognitionId && x.IsActive && (x.ModuleId == (int)ModuleId.Recognisation || x.ModuleId == (int)ModuleId.TeamTagInRecognisationInBody)).ToList();
            var allEmployeeTag = await _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == request.RecognitionEditRequest.RecognitionId && (x.ModuleId == (int)ModuleId.Recognisation || x.ModuleId == (int)ModuleId.TeamTagInRecognisationInBody)).ToListAsync();
            foreach (var assignUser in request.RecognitionEditRequest.RecognitionEmployeeTags)
            {
                var employeeTag = allEmployeeTag.FirstOrDefault(x => x.TagId == assignUser.Id);
                if (employeeTag != null)
                {
                    employeeTag.IsActive = request.IsActive;
                    employeeTag.UpdatedBy = userIdentity.EmployeeId;
                    employeeTag.UpdatedOn = request.UpdatedOn;
                    _employeeTagRepo.Update(employeeTag);
                }
                else
                {
                    var recogEmployeeTag = new EmployeeTag()
                    {
                        ModuleDetailsId = request.RecognitionEditRequest.RecognitionId,
                        TagId = assignUser.Id,
                        IsActive = request.IsActive,
                        CreatedBy = userIdentity.EmployeeId,
                        CreatedOn = request.CreatedOn,
                        ModuleId = assignUser.SearchType == 1 ? (int)ModuleId.Recognisation : (int)ModuleId.TeamTagInRecognisationInBody
                    };
                    _employeeTagRepo.Add(recogEmployeeTag);
                }

            }
            var empNewTag = request.RecognitionEditRequest.RecognitionEmployeeTags.ToList();
            if (empOldTag.Count > 0 && empNewTag.Count > 0)
            {
                var empFinalList = empOldTag.Select(x => x.TagId).Except(empNewTag.Select(x => x.Id)).ToList();
                if (empFinalList.Count > 0)
                {
                    var empFileRecords = await _employeeTagRepo.GetQueryable().Where(x => empFinalList.Contains(x.TagId)).ToListAsync();

                    ////update the status of Note File as inactive
                    if (empFileRecords.Count > 0)
                    {
                        empFileRecords.ForEach(a =>
                        {
                            a.IsActive = false;
                            a.UpdatedBy = userIdentity.EmployeeId;
                            a.UpdatedOn = request.UpdatedOn;
                            _employeeTagRepo.Update(a);
                        });
                        UnitOfWorkAsync.SaveChanges();
                    }
                }
            }

            if (empOldTag.Count > 0 && empNewTag.Count == 0)
            {
                var empFileRecords = await _employeeTagRepo.GetQueryable().Where(x => empOldTag.Select(x => x.TagId).Contains(x.TagId)).ToListAsync();

                ////update the status of Employee as inactive
                if (empFileRecords.Count > 0)
                {
                    empFileRecords.ForEach(a =>
                    {
                        a.IsActive = false;
                        a.UpdatedBy = userIdentity.EmployeeId;
                        a.UpdatedOn = request.UpdatedOn;
                        _employeeTagRepo.Update(a);
                    });
                    UnitOfWorkAsync.SaveChanges();
                }
            }
            await UnitOfWorkAsync.SaveChangesAsync();
            if (empOldTag.Count == 0 && empNewTag.Count > 0)
            {

                var newTag = await _notificationsService.UpateEmployeeTag(empNewTag, userIdentity, request.RecognitionEditRequest.RecognitionId).ConfigureAwait(false);
                empIds.AddRange(newTag);
            }

            if (empOldTag.Count > 0 && empNewTag.Count > 0)
            {
                var empFinalList = empNewTag.Select(x => x.Id).Except(empOldTag.Select(x => x.TagId)).ToList();
                if (empFinalList.Count > 0)
                {

                    var finalTag = await _notificationsService.UpateFinalEmployeeTag(empFinalList, userIdentity, request.RecognitionEditRequest.RecognitionId).ConfigureAwait(false);
                    empIds.AddRange(finalTag);

                }

            }
            return empIds;
        }
        private async Task DeleteRecognitionScreenshot(RecognitionEditCommand request, UserIdentity userIdentity)
        {
            var recOldFile = _recognitionImageMappingRepo.GetQueryable().Where(x => x.RecognitionId == request.RecognitionEditRequest.RecognitionId && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.RecognitionScreenshot && x.IsActive).Select(x => x.GuidFileName).ToList();
            var recAllFiles = await _recognitionImageMappingRepo.GetQueryable().Where(x => x.RecognitionId == request.RecognitionEditRequest.RecognitionId && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.RecognitionScreenshot).ToListAsync();
            foreach (var assignFiles in request.RecognitionEditRequest.RecognitionImageRequests)
            {

                var recFiles = recAllFiles.FirstOrDefault(x => x.GuidFileName == assignFiles.GuidFileName);
                if (recFiles != null)
                {
                    recFiles.RecognitionId = request.RecognitionEditRequest.RecognitionId;
                    recFiles.RecognitionCategoryId = 0;
                    recFiles.RecognitionCategoryTypeId = (int)RecognitionCategoryType.RecognitionCommentScreenshot;
                    recFiles.IsActive = request.IsActive;
                    recFiles.FileName = assignFiles.FileName;
                    recFiles.GuidFileName = assignFiles.GuidFileName;
                    recFiles.Name = "";
                    recFiles.UpdatedOn = request.UpdatedOn;
                    recFiles.UpdatedBy = userIdentity.EmployeeId;
                    _recognitionImageMappingRepo.Update(recFiles);
                }
                else
                {
                    var recognitionImageMapping = new RecognitionImageMapping
                    {
                        RecognitionId = request.RecognitionEditRequest.RecognitionId,
                        RecognitionCategoryId = 0,
                        RecognitionCategoryTypeId = (int)RecognitionCategoryType.RecognitionCommentScreenshot,
                        IsActive = request.IsActive,
                        CreatedBy = userIdentity.EmployeeId,
                        CreatedOn = request.CreatedOn,
                        FileName = assignFiles.FileName,
                        GuidFileName = assignFiles.GuidFileName,
                        Name = "",
                        UpdatedOn = request.CreatedOn
                    };
                    _recognitionImageMappingRepo.Add(recognitionImageMapping);
                }

            }

            var recNewFile = request.RecognitionEditRequest.RecognitionImageRequests.Select(x => x.GuidFileName).ToList();

            if (recOldFile.Count > 0 && recNewFile.Count > 0)
            {
                var convrFinalList = recOldFile.Except(recNewFile).ToList();
                if (convrFinalList.Count > 0)
                {
                    var convrFileRecords = await _recognitionImageMappingRepo.GetQueryable().Where(x => convrFinalList.Contains(x.GuidFileName)).ToListAsync();

                    ////update the status of File as inactive
                    if (convrFileRecords.Count > 0)
                    {
                        convrFileRecords.ForEach(a =>
                        {
                            a.IsActive = false;
                            a.UpdatedBy = userIdentity.EmployeeId;
                            a.UpdatedOn = request.UpdatedOn;
                            _recognitionImageMappingRepo.Update(a);
                        });
                        UnitOfWorkAsync.SaveChanges();
                    }
                }
            }

            if (recOldFile.Count > 0 && recNewFile.Count == 0)
            {
                var convrFileRecords = await _recognitionImageMappingRepo.GetQueryable().Where(x => recOldFile.Contains(x.GuidFileName)).ToListAsync();

                ////update the status of File as inactive
                if (convrFileRecords.Count > 0)
                {
                    convrFileRecords.ForEach(a =>
                    {
                        a.IsActive = false;
                        a.UpdatedBy = userIdentity.EmployeeId;
                        a.UpdatedOn = request.UpdatedOn;
                        _recognitionImageMappingRepo.Update(a);
                    });
                    UnitOfWorkAsync.SaveChanges();
                }

            }
            await UnitOfWorkAsync.SaveChangesAsync();
        }
        public async Task<Payload<RecognitionCategoryResponse>> GetCategory(RecognitionCategoryGetQuery request)
        {
            var payload = new Payload<RecognitionCategoryResponse> { };
            var blobDetails = await _keyVaultService.GetAzureBlobKeysAsync();
            var reportingUser = _employeeRepo.GetQueryable().Any(x => x.ReportingTo == request.EmployeeId && x.IsActive);
            var badges = new List<RecognitionCategory>();
            if (reportingUser)
            {
                badges = await _recognitionCategoryRepo.GetQueryable().Where(x => !x.IsDeleted && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.Badge && x.IsActive).ToListAsync();
            }
            else
            {
                badges = await _recognitionCategoryRepo.GetQueryable().Where(x => !x.IsDeleted && !x.IsOnlyManager && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.Badge && x.IsActive).ToListAsync();
            }

            var recognitionBadges = (from rec in badges
                                     select new RecognitionCategoryResponse
                                     {
                                         RecognitionCategoryId = rec.RecognitionCategoryId,
                                         RecognitionCategoryTypeId = rec.RecognitionCategoryTypeId,
                                         IsOnlyManager = rec.IsOnlyManager,
                                         Name = rec.Name,
                                         ImageFilePath = blobDetails.BlobCdnUrl + blobDetails.BlobContainerName + "/" + AppConstants.RecognitionFolderName + "/" + rec.GuidFileName
                                     }).OrderByDescending(x => x.RecognitionCategoryId).ToList();

            payload.EntityList = recognitionBadges;
            payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.CommentSuccess);
            return payload;
        }
        public async Task<Payload<bool>> Delete(RecognitionDeleteCommand request)
        {
            var payload = new Payload<bool> { Entity = false };
            var userIdentity = _commonService.GetUserIdentity();
            var recognitionDetails = await _recognitionRepo.FirstOrDefaultAsync(x => x.RecognitionId == request.RecognitionId && x.IsActive);
            if (recognitionDetails == null) return payload;
            else
            {
                recognitionDetails.IsActive = false;
                recognitionDetails.UpdatedBy = userIdentity.EmployeeId;
                recognitionDetails.UpdatedOn = request.UpdatedOn;
                _recognitionRepo.Update(recognitionDetails);
                await UnitOfWorkAsync.SaveChangesAsync();
                await DeleteRecognitionAttachment(request.RecognitionId, userIdentity.EmployeeId).ConfigureAwait(false);
                await DeleteRecognitionComment(request.RecognitionId, userIdentity.EmployeeId).ConfigureAwait(false);
                await DeleteRecognitionLike(request.RecognitionId, userIdentity.EmployeeId).ConfigureAwait(false);
                await DeleteRecognitionEmployeeTag(request.RecognitionId, userIdentity.EmployeeId).ConfigureAwait(false);
                await DeleteRecognitionEmployeeMapping(request.RecognitionId).ConfigureAwait(false);
                await DeleteRecognitionNotifications(request.RecognitionId).ConfigureAwait(false);
                await _commonService.CallSignalRForAllEmployees(userIdentity.EmployeeId).ConfigureAwait(false);
                payload.Entity = true;
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.RecognitionDeleted);
            }
            return payload;
        }



        public async Task<Payload<OrgRecognitionResponse>> GetOrgRecognition(GetOrgRecognitionQuery request)
        {
            var payload = new Payload<OrgRecognitionResponse> { };
            var orgRecognitionResponses = new List<OrgRecognitionResponse>();
            var userIdentity = _commonService.GetUserIdentity();

            var recognitionDetails = await GetRecognitionForWall(request.RecognitionId, request.IsMyPost, request.Id, userIdentity, request.SearchType);

            if (recognitionDetails.Count == 0) return payload;

            request.PageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
            var skipAmount = request.PageSize * (request.PageIndex - 1);
            var totalRecords = recognitionDetails.Count;
            var totalPages = totalRecords / request.PageSize;

            if (totalRecords % request.PageSize > 0)
                totalPages++;

            var pageResult = new PageInfo
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
            payload.PagingInfo = pageResult;
            var finalRecognitions = recognitionDetails.Skip(skipAmount).Take(request.PageSize).ToList();


            List<long> ids = finalRecognitions.Select(x => x.RecognitionId).ToList();
            var likeDetails = await _likeReactionRepo.GetQueryable().Where(x => ids.Contains(x.ModuleDetailsId) && x.IsActive && x.ModuleId == (int)ModuleId.Recognisation).ToListAsync();
            var allemployeeTag = await _employeeTagRepo.GetQueryable().Where(x => x.IsActive && ids.Contains(x.ModuleDetailsId) && (x.ModuleId == (int)ModuleId.EmployeeReceiver || x.ModuleId == (int)ModuleId.TeamReceiver)).ToListAsync();

            List<long> taggedTeamids = allemployeeTag.Where(x => x.ModuleId == (int)ModuleId.TeamReceiver).Select(x => x.TagId).ToList();
            var allTeam = _teamRepo.GetQueryable().Where(x => taggedTeamids.Contains(x.TeamId)).ToList();

            List<long> empIds = allemployeeTag.Where(x => x.ModuleId == (int)ModuleId.EmployeeReceiver).Select(x => x.TagId).ToList();
            empIds.AddRange(finalRecognitions.Select(x => x.CreatedBy).ToList());
            empIds.AddRange(likeDetails.Select(x => x.CreatedBy).ToList());
            var employees = _employeeRepo.GetQueryable().Where(x => empIds.Contains(x.EmployeeId)).ToList();

            var commentDetails = await _commentDetailsRepo.GetQueryable().Where(x => ids.Contains(x.ModuleDetailsId) && x.IsActive && x.ModuleId == (int)CommentModuleId.Recognisation).ToListAsync();
            var attachmentDetails = _recognitionImageMappingRepo.GetQueryable().Where(x => ids.Contains(x.RecognitionId) && x.IsActive && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.Badge).ToList();
            var blobDetails = await _keyVaultService.GetAzureBlobKeysAsync();

            foreach (var item in finalRecognitions)
            {
                var senderDetails = employees.FirstOrDefault(x => x.EmployeeId == item.CreatedBy);
                var recognisationLike = likeDetails.Where(x => x.ModuleDetailsId == item.RecognitionId).ToList();
                var recognisationComment = commentDetails.Where(x => x.ModuleDetailsId == item.RecognitionId).ToList();
                var recognisationAttachment = attachmentDetails.FirstOrDefault(x => x.RecognitionId == item.RecognitionId);
                OrgRecognitionResponse data = new OrgRecognitionResponse
                {
                    RecognitionId = item.RecognitionId,
                    Headlines = item.Headlines,
                    Message = item.Message,
                    IsAttachment = item.IsAttachment,
                    RecognitionCategoryTypeId = item.RecognitionCategoryTypeId,
                    ReceiverId = item.ReceiverId,
                    SenderId = item.CreatedBy,
                    IsEditable = item.CreatedBy == userIdentity.EmployeeId,
                    CreatedOn = item.CreatedOn,
                    UpdatedOn = item.UpdatedOn == null ? item.CreatedOn : (DateTime)item.UpdatedOn,
                    SenderEmailId = senderDetails == null ? "" : senderDetails.EmailId,
                    SenderFirstName = senderDetails == null ? "" : senderDetails.FirstName,
                    SenderLastName = senderDetails == null ? "" : senderDetails.LastName,
                    SenderFullName = senderDetails == null ? "" : senderDetails.FirstName + " " + senderDetails.LastName,
                    SenderEmployeeId = senderDetails == null ? 0 : senderDetails.EmployeeId,
                    SenderImagePath = senderDetails == null ? "" : senderDetails.ImagePath,
                    receiverDetails = ReceiverDetails(item.RecognitionId, allemployeeTag, allTeam, employees),
                    IsEdited = item.UpdatedOn == null ? false : item.CreatedOn != item.UpdatedOn
                };

                ///Get Like details
                if (likeDetails.Count > 0)
                {
                    data.TotalLikeCount = recognisationLike.Count;
                    data.IsLiked = recognisationLike.Any(x => x.CreatedBy == userIdentity.EmployeeId);
                    var recognitionLikes = (from emp in employees
                                            join rec in recognisationLike on emp.EmployeeId equals rec.CreatedBy
                                            select new RecognitionLikeResponse
                                            {
                                                LikeReactionId = rec.LikeReactionId,
                                                EmployeeId = emp.EmployeeId,
                                                FirstName = emp.FirstName,
                                                LastName = emp.LastName,
                                                FullName = emp.FirstName + " " + emp.LastName,
                                                ImagePath = emp.ImagePath,
                                                EmailId = emp.EmailId
                                            }).ToList();
                    data.RecognitionLikeResponses = recognitionLikes;
                }
                ///Get Comment details
                if (recognisationComment.Count > 0)
                {
                    data.TotalCommentCount = recognisationComment.Count;
                    data.IsCommented = recognisationComment.Any(x => x.CreatedBy == userIdentity.EmployeeId);
                }

                if (recognisationAttachment != null)
                {
                    data.AttachmentImagePath = blobDetails.BlobCdnUrl + blobDetails.BlobContainerName + "/" + AppConstants.RecognitionFolderName + "/" + recognisationAttachment.GuidFileName;
                    data.RecognitionCategoryId = recognisationAttachment.RecognitionCategoryId;
                    data.AttachmentName = recognisationAttachment.Name;
                }


                orgRecognitionResponses.Add(data);
            }
            //call AuditEngagementReport
            await _commonService.AuditEngagementReport(new CreateEngagementReportRequest { EmployeeId = userIdentity.EmployeeId, EngagementTypeId = AppConstants.Engagement_RecognitionUpdate })
                .ConfigureAwait(false);
            payload.EntityList = orgRecognitionResponses;
            return payload;
        }

        #region mywalloffame
        public async Task<Payload<MyWallOfFameResponse>> GetMyWallOfFameGetQuery(MyWallOfFameGetQuery request)
        {
            var payload = new Payload<MyWallOfFameResponse> { EntityList = new List<MyWallOfFameResponse>() };
            var userIdentity = _commonService.GetUserIdentity();
            request.MyMyWallOfFameRequest.PageIndex = request.MyMyWallOfFameRequest.PageIndex <= 0 ? 1 :
                           request.MyMyWallOfFameRequest.PageIndex;

            var myWallOfFames = await MyWallOfFameFilter(request);
            var skipAmount = request.MyMyWallOfFameRequest.PageSize * (request.MyMyWallOfFameRequest.PageIndex - 1);

            List<MyWallOfFameResponse> result = new List<MyWallOfFameResponse>();
            var firstTab = await WallofFameFirst(myWallOfFames, userIdentity.EmployeeId);
            var secondTab = await WallofFameSecond(myWallOfFames, userIdentity.EmployeeId);
            result.Add(firstTab);
            result.Add(secondTab);
            var totalRecords = result.Count;
            var totalPages = totalRecords / request.MyMyWallOfFameRequest.PageSize;
            var pageResult = new PageInfo
            {
                PageIndex = request.MyMyWallOfFameRequest.PageIndex,
                PageSize = request.MyMyWallOfFameRequest.PageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };

            payload.PagingInfo = pageResult;
            var finalRecognitions = result.Skip(skipAmount).Take(request.MyMyWallOfFameRequest.PageSize).ToList();
            payload.EntityList = finalRecognitions;
            if (payload.EntityList.Count() > 0)
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.RecordFetched);
            else
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.NoBadges);
            return payload;
        }
        public async Task<Payload<MyWallOfFameDashBoardResponse>> MyWallOfFameDashBoard(MyWallOfFameDashBoardGetQuery request)
        {
            var userIdentity = _commonService.GetUserIdentity();           
            var payload = new Payload<MyWallOfFameDashBoardResponse> { Entity = new MyWallOfFameDashBoardResponse() };
            var blobDetails = await _keyVaultService.GetAzureBlobKeysAsync();
            string imgPath = blobDetails.BlobCdnUrl +
                        blobDetails.BlobContainerName + "/" + AppConstants.RecognitionFolderName + "/";

            MyWallOfFameGetQuery myWallofFamerequest = new MyWallOfFameGetQuery
            {

                MyMyWallOfFameRequest = new MyWallOfFameRequest
                {
                    Id = userIdentity.EmployeeId,                  
                    PageIndex = 1,
                    PageSize = 10,
                    SearchType = 0
                }
            };
            List<RecognitionImageMappingResponse> badges = new List<RecognitionImageMappingResponse>();
            var myWallOfFames = await MyWallOfFameFilter(myWallofFamerequest);
            var result = await WallofFameFirst(myWallOfFames, userIdentity.EmployeeId);
            if (result.RecognitionImageMappings.Any())
            {
                var finalResult = result.RecognitionImageMappings.ToList().Take(5);
                badges.AddRange(finalResult);

            }
            payload.Entity.RecognitionImageMappings.AddRange(badges);
            if (payload.Entity.RecognitionImageMappings.Any())
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.RecordFetched);
            else
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.RecordNotFound);
            return payload;
        }

        #endregion
        public async Task<Payload<TotalRecognitionByTeamIdResponse>> TotalRecognitionByTeamId(TotalRecognitionByTeamIdGetQuery request)
        {
            var payload = new Payload<TotalRecognitionByTeamIdResponse> { };
            var recognitions = await (from rec in _recognitionRepo.GetQueryable().Where(x => x.IsActive
                                    && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.Badge)
                                      join emp in _employeeTeamMappingRepo.GetQueryable().Where(x =>
                                             x.TeamId == request.Team.Id && x.IsActive).Select(x => x.EmployeeId)
                                   on rec.ReceiverId equals emp
                                      select new { Recognition = rec }
                                ).ToListAsync();

            var result = recognitions.GroupBy(x => new { x.Recognition.IsAttachment }, (key, items) =>
             new TotalRecognitionByTeamIdResponse
             {

                 Text = items.FirstOrDefault().Recognition.IsAttachment ?
                 AppConstants.TotalBadgesReceivedByTeamMembers :
                 AppConstants.TotalRecognitionsReceivedByTeamMembers,
                 ToTalCount = items.Select(x => x.Recognition.RecognitionId).Count()
             }).ToList();
            payload.EntityList = result;
            if (payload.EntityList.Any())
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.RecordFetched);
            else
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.RecordNotFound);

            return payload;
        }
        public async Task<Payload<TeamByEmpIdResponse>> GetTeamsByEmpId(TeamsByEmpIdGetQuery request)
        {
            var payload = new Payload<TeamByEmpIdResponse> { };
            TeamByEmpIdResponse result = new TeamByEmpIdResponse();
            var teams = await (from team in _teamRepo.GetQueryable().Where(x => x.IsActive)
                               join empTeam in _employeeTeamMappingRepo.GetQueryable().Where(x => x.IsActive)
                                   on team.TeamId equals empTeam.TeamId
                               where empTeam.EmployeeId == request.EmployeeId
                               select new TeamByEmpIdResponse
                               {
                                   TeamId = team.TeamId,
                                   TeamName = team.TeamName,
                                   BackGroundColorCode = team.BackGroundColorCode,
                                   Colorcode = team.Colorcode
                               }
                                         ).ToListAsync()
                                         ;
            payload.EntityList = teams;
            if (payload.EntityList.Any())
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.RecordFetched);
            else
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.RecordNotFound);

            return payload;
        }
        public async Task<Payload<RecognitionResponse>> GetRecognitionById(GetRecognitionByIdQuery request)
        {
            var payload = new Payload<RecognitionResponse> { };
            var orgRecognitionResponses = new List<OrgRecognitionResponse>();
            var userIdentity = _commonService.GetUserIdentity();

            var recognitionDetails = _recognitionRepo.GetQueryable().FirstOrDefault(x => x.IsActive && x.RecognitionId == request.RecognitionId);

            if (recognitionDetails == null) return payload;

            RecognitionResponse data = new RecognitionResponse
            {
                RecognitionId = recognitionDetails.RecognitionId,
                Headlines = recognitionDetails.Headlines,
                Message = recognitionDetails.Message,
                IsAttachment = recognitionDetails.IsAttachment,
                RecognitionCategoryTypeId = recognitionDetails.RecognitionCategoryTypeId,
                ReceiverId = recognitionDetails.ReceiverId,
                SenderId = recognitionDetails.CreatedBy,
                CreatedOn = recognitionDetails.CreatedOn,
                UpdatedOn = recognitionDetails.UpdatedOn == null ? recognitionDetails.CreatedOn : (DateTime)recognitionDetails.UpdatedOn
            };
            var likeDetails = await _likeReactionRepo.GetQueryable().Where(x => x.ModuleDetailsId == request.RecognitionId && x.IsActive && x.ModuleId == (int)ModuleId.Recognisation).ToListAsync();
            var commentDetails = await _commentDetailsRepo.GetQueryable().Where(x => x.ModuleDetailsId == request.RecognitionId && x.IsActive && x.ModuleId == (int)ModuleId.Recognisation).ToListAsync();

            var employees = _employeeRepo.GetQueryable().ToList();
            ///Get Like details
            if (likeDetails.Count > 0)
            {
                data.TotalLikeCount = likeDetails.Count;
                data.IsLiked = likeDetails.Any(x => x.CreatedBy == userIdentity.EmployeeId);
                var recognitionLikes = (from emp in employees
                                        join rec in likeDetails on emp.EmployeeId equals rec.CreatedBy
                                        select new RecognitionLikeResponse
                                        {
                                            LikeReactionId = rec.LikeReactionId,
                                            EmployeeId = emp.EmployeeId,
                                            FirstName = emp.FirstName,
                                            LastName = emp.LastName,
                                            FullName = emp.FirstName + " " + emp.LastName,
                                            ImagePath = emp.ImagePath,
                                            EmailId = emp.EmailId
                                        }).ToList();
                data.RecognitionLikeResponses = recognitionLikes;
            }
            ///Bind Attachment
            if (recognitionDetails.IsAttachment)
            {
                var blobDetails = await _keyVaultService.GetAzureBlobKeysAsync();
                var attachmentDetails = _recognitionImageMappingRepo.GetQueryable().FirstOrDefault(x => x.RecognitionId == request.RecognitionId && x.IsActive);
                data.AttachmentImagePath = blobDetails.BlobCdnUrl + blobDetails.BlobContainerName + "/" + AppConstants.RecognitionFolderName + "/" + attachmentDetails.GuidFileName;
                data.RecognitionCategoryId = attachmentDetails.RecognitionCategoryId;
            }

            ///Get Comment details
            if (commentDetails.Count > 0)
            {
                data.TotalCommentCount = commentDetails.Count;
                data.IsCommented = commentDetails.Any(x => x.CreatedBy == userIdentity.EmployeeId);
                var recognitionComments = (from emp in employees
                                           join rec in commentDetails on emp.EmployeeId equals rec.CreatedBy
                                           select new CommentDetailResponse
                                           {
                                               CommentDetailsId = rec.CommentDetailsId,
                                               EmployeeId = emp.EmployeeId,
                                               FirstName = emp.FirstName,
                                               LastName = emp.LastName,
                                               FullName = emp.FirstName + " " + emp.LastName,
                                               ImagePath = emp.ImagePath,
                                           }).ToList();
                data.CommentDetailResponses = recognitionComments;
            }

            payload.Entity = data;
            return payload;
        }

        #region LeaderBoard
        public async Task<Payload<RecognitionByTeamIdResponse>> EmployeesLeaderBoard(EmployeesLeaderBoardGetQuery request)
        {
            long Id = request.Request.Id; //EmployeeId and TeamID
            int serarchType = request.Request.SearchType;
            var payload = new Payload<RecognitionByTeamIdResponse> { };
            var teamEmployeeMappings = await (from team in _teamRepo.GetQueryable().Where(x => x.IsActive)
                                              join empTeam in _employeeTeamMappingRepo.GetQueryable().Where(x =>
                                                     ((serarchType == (int)SearchType.Team ||
                                                       serarchType == (int)SearchType.All) ? x.TeamId == request.Request.Id
                                                       : x.EmployeeId == request.Request.Id)
                                                               && x.IsActive
                                                               )
                                                        on team.TeamId equals empTeam.TeamId
                                              join emp in _employeeRepo.GetQueryable().Where(x =>
                                                                     x.IsActive && !x.IsSystemUser)
                                                               on empTeam.EmployeeId equals emp.EmployeeId
                                              select new RecognitionByTeamIdEmployee
                                              {
                                                  TeamId = team.TeamId,
                                                  TeamName = team.TeamName,
                                                  BackGroundColorCode = team.BackGroundColorCode,
                                                  ColorCode = team.Colorcode,
                                                  EmployeeId = empTeam.EmployeeId,
                                                  FirstName = emp.FirstName,
                                                  LastName = emp.LastName,
                                                  EmailId = emp.EmailId,
                                                  ImagePath = emp.ImagePath
                                              }).ToListAsync();

            var empids = teamEmployeeMappings.Select(x => x.EmployeeId).ToList();
            var teamIds = teamEmployeeMappings.Select(x => x.TeamId).ToList();
            var recognitionsData = await (_recognitionRepo.GetQueryable().Where(x => x.IsActive)).ToListAsync();
            var allRecognitionIds = recognitionsData.Select(x => x.RecognitionId).Distinct().ToList();

            var employeeTagReceivers = _employeeTagRepo.GetQueryable().Where(x => x.IsActive
                                        && x.ModuleId == (int)ModuleId.EmployeeReceiver && allRecognitionIds.Contains(x.ModuleDetailsId)
                                       //&& empids.Contains(x.TagId)
                                       ).ToList();//employeeWiseList
                                                  //ignore Team ModuleDetailsId

            RecognitionByTeamIdResponse finalResult = new RecognitionByTeamIdResponse();
            List<RecognitionTeam> recognitionEmployees = new List<RecognitionTeam>();

            foreach (var teamMemeber in teamEmployeeMappings.ToList())
            {
                var recognitionsReceiveds = employeeTagReceivers.Where(x => x.TagId == teamMemeber.EmployeeId).ToList();
                var recognitionsGivens = recognitionsData.Where(x => x.CreatedBy == teamMemeber.EmployeeId).ToList();//recevice

                var badgesReceiveds = recognitionsData.Where(x => x.IsAttachment &&
                             x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.Badge
                                 && recognitionsReceiveds.Select(y => y.ModuleDetailsId).Contains(x.RecognitionId)).ToList();


                RecognitionTeam value = new RecognitionTeam()
                {
                    EmailId = teamMemeber.EmailId,
                    EmployeeId = teamMemeber.EmployeeId,
                    FirstName = teamMemeber.FirstName,
                    ImagePath = string.IsNullOrEmpty(teamMemeber.ImagePath) ? string.Empty : teamMemeber.ImagePath,
                    LastName = teamMemeber.LastName,
                    OrderByDateTime = recognitionsReceiveds.Any() ? recognitionsReceiveds.OrderByDescending(x => x.CreatedOn).FirstOrDefault().CreatedOn : DateTime.UtcNow,

                    TotalRecognitionsReceived = recognitionsReceiveds.Count,
                    TotalRecognitionsGiven = recognitionsGivens.Count,
                    TotalBadgesReceived = badgesReceiveds.Count

                };
                recognitionEmployees.Add(value);
            }
            if (teamEmployeeMappings.Any())
            {
                finalResult.TeamId = teamEmployeeMappings.FirstOrDefault().TeamId;
                finalResult.TeamName = teamEmployeeMappings.FirstOrDefault().TeamName;
            }
            finalResult.RecognitionEmployees = recognitionEmployees
             .OrderByDescending(y => y.TotalRecognitionsReceived)
                 .ThenByDescending(z => z.TotalRecognitionsGiven)
                 .ThenByDescending(a => a.TotalBadgesReceived)
                 .ThenBy(b => b.FirstName).ThenBy(c => c.LastName)
                 .ThenBy(y => y.OrderByDateTime).ToList();
            var result = new List<RecognitionByTeamIdResponse>();
            result.Add(finalResult);
            payload.EntityList = result;
            if (payload.EntityList.Any())
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.RecordFetched);
            else
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.RecordNotFound);

            return payload;
        }
        public async Task<Payload<RecognitionTeamsResponse>> TeamsLeaderBoard(TeamsLeaderBoardGetQuery request)
        {
            var payload = new Payload<RecognitionTeamsResponse> { };
            long Id = request.Request.Id; //EmployeeId and TeamID
            int serarchType = request.Request.SearchType;

            var teamEmployeeMappings = await (from team in _teamRepo.GetQueryable().Where(x => x.IsActive)
                                              join empTeam in _employeeTeamMappingRepo.GetQueryable().Where(x =>
                                                     (serarchType == (int)SearchType.Team ? x.TeamId == request.Request.Id : x.EmployeeId == request.Request.Id)
                                                               && x.IsActive
                                                               )
                                                        on team.TeamId equals empTeam.TeamId
                                              join emp in _employeeRepo.GetQueryable().Where(x =>
                                                                     x.IsActive && !x.IsSystemUser)
                                                               on empTeam.EmployeeId equals emp.EmployeeId
                                              select new RecognitionByTeamIdEmployee
                                              {
                                                  TeamId = team.TeamId,
                                                  TeamName = team.TeamName,
                                                  BackGroundColorCode = team.BackGroundColorCode,
                                                  ColorCode = team.Colorcode,
                                                  EmployeeId = empTeam.EmployeeId,
                                                  FirstName = emp.FirstName,
                                                  LastName = emp.LastName,
                                                  EmailId = emp.EmailId,
                                                  ImagePath = team.LogoImagePath
                                              }).ToListAsync();

            var teamWithEmployees = (from emp in _employeeRepo.GetQueryable().Where(x => x.IsActive
                                 && !x.IsSystemUser)
                                     join empTeam in _employeeTeamMappingRepo.GetQueryable().Where(x =>
                                     x.IsActive
                                     && teamEmployeeMappings.Select(z => z.TeamId).Contains(x.TeamId))
                                     on emp.EmployeeId equals empTeam.EmployeeId
                                     select new { empTeam.TeamId, emp.EmployeeId }).ToList()
                            .GroupBy(z => z.TeamId)
                             .Select(grp => new { TeamId = grp.Key, TeamMembers = grp.ToList() }).ToList();

            var teamEmployeesCount = teamWithEmployees.Select(p => new
            {
                TeamId = p.TeamId,

                TeamMemberCount = p.TeamMembers.Count
            });
            /////
            //
            var teamIds = teamWithEmployees.Select(x => x.TeamId).ToList();

            var recognitionsData = await (_recognitionRepo.GetQueryable().Where(x => x.IsActive)).ToListAsync();
            var allRecognitionIds = recognitionsData.Select(x => x.RecognitionId).Distinct().ToList();

            var employeeTags = _employeeTagRepo.GetQueryable().Where(x => x.IsActive
                         && x.ModuleId == (int)ModuleId.TeamReceiver && allRecognitionIds.Contains(x.ModuleDetailsId)
                          && teamIds.Contains(x.TagId)
           ).ToList();

            var recognitionIds = employeeTags.Select(x => x.TagId).Distinct().ToList(); //Recog Id

            List<RecognitionTeamsResponse> data = new List<RecognitionTeamsResponse>();


            foreach (var item in teamWithEmployees)
            {
                RecognitionTeamsResponse value = new RecognitionTeamsResponse();
                var currentTeam = teamEmployeeMappings.FirstOrDefault(x => x.TeamId == item.TeamId);
                if (currentTeam == null)
                    currentTeam = new RecognitionByTeamIdEmployee();

                value.BackGroundColorCode = currentTeam.BackGroundColorCode;
                value.ColorCode = currentTeam.ColorCode;
                value.TeamId = item.TeamId;
                value.ImagePath = currentTeam.ImagePath;
                var member = teamEmployeesCount.FirstOrDefault(x => x.TeamId == item.TeamId);
                value.TeamName = currentTeam.TeamName;                //value.BackGroundColorCode = item.EmailId;
                value.TeamMemberCount = member == null ? 0 : member.TeamMemberCount;
                var teamRec = employeeTags.Where(x => x.TagId == item.TeamId).ToList();
                value.RecognitionsReceived = new RecognitionteamsData { Total = teamRec.Count() };

                var teamrocIds = teamRec.Select(x => x.ModuleDetailsId).ToList();
                var badgeData = recognitionsData.Count(x => x.IsAttachment
                                && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.Badge
                                                     && teamrocIds.Contains(x.RecognitionId));
                value.BadgesReceived = new RecognitionteamsData { Total = badgeData };
                data.Add(value);
            }

            var result = data;
            result = result.ToList().OrderByDescending(q => q.RecognitionsReceived.Total)
                                         .ThenByDescending(q => q.BadgesReceived.Total).ToList();
            payload.EntityList = result;
            if (payload.EntityList.Any())
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.RecordFetched);
            else
                payload = GetPayloadStatusSuccess(payload, "messageSuccess", ResourceMessage.RecordNotFound);

            return payload;

        }
        #endregion
        public async Task<Payload<RecognitionDetailsResponse>> GetRecognition(GetRecognitionForWallQuery request)
        {
            var payload = new Payload<RecognitionDetailsResponse> { };
            var orgRecognitionResponses = new List<RecognitionDetailsResponse>();
            var recognitionDetails = new List<Recognition>();
            var userIdentity = _employeeRepo.GetQueryable().FirstOrDefault(x => x.EmailId == request.emailId);

            recognitionDetails = _recognitionRepo.GetQueryable().Where(x => x.IsActive && x.CreatedOn.Date >= request.StartDate.Date && x.CreatedOn.Date <= request.EndDate.Date).OrderByDescending(x => x.CreatedOn).ToList();//await GetRecognitionForWall(request.RecognitionId, request.IsMyPost, request.EmployeeId, request.StartDate, request.EndDate, userIdentity, 0);
            if (recognitionDetails.Count == 0) return payload;

            request.PageIndex = request.PageIndex <= 0 ? 1 : request.PageIndex;
            var skipAmount = request.PageSize * (request.PageIndex - 1);
            var totalRecords = recognitionDetails.Count;
            var totalPages = totalRecords / request.PageSize;

            if (totalRecords % request.PageSize > 0)
                totalPages++;

            var pageResult = new PageInfo
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRecords,
                TotalPages = totalPages
            };
            payload.PagingInfo = pageResult;
            var finalRecognitions = recognitionDetails.Skip(skipAmount).Take(request.PageSize).ToList();


            List<long> ids = finalRecognitions.Select(x => x.RecognitionId).ToList();
            var likeDetails = await _likeReactionRepo.GetQueryable().Where(x => ids.Contains(x.ModuleDetailsId) && x.IsActive && x.ModuleId == (int)ModuleId.Recognisation).ToListAsync();
            var allemployeeTag = await _employeeTagRepo.GetQueryable().Where(x => x.IsActive && ids.Contains(x.ModuleDetailsId) && (x.ModuleId == (int)ModuleId.EmployeeReceiver || x.ModuleId == (int)ModuleId.TeamReceiver)).ToListAsync();
            var commentDetails = await _commentDetailsRepo.GetQueryable().Where(x => ids.Contains(x.ModuleDetailsId) && x.IsActive && x.ModuleId == (int)CommentModuleId.Recognisation).ToListAsync();

            List<long> taggedTeamids = allemployeeTag.Where(x => x.ModuleId == (int)ModuleId.TeamReceiver).Select(x => x.TagId).ToList();
            var allTeam = _teamRepo.GetQueryable().Where(x => taggedTeamids.Contains(x.TeamId)).ToList();

            List<long> empIds = allemployeeTag.Where(x => x.ModuleId == (int)ModuleId.EmployeeReceiver).Select(x => x.TagId).ToList();
            empIds.AddRange(finalRecognitions.Select(x => x.CreatedBy).ToList());
            empIds.AddRange(likeDetails.Select(x => x.CreatedBy).ToList());
            empIds.AddRange(commentDetails.Select(x => x.CreatedBy).ToList());
            var employees = _employeeRepo.GetQueryable().Where(x => empIds.Contains(x.EmployeeId)).ToList();


            var attachmentDetails = _recognitionImageMappingRepo.GetQueryable().Where(x => ids.Contains(x.RecognitionId) && x.IsActive && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.Badge).ToList();
            var blobDetails = await _keyVaultService.GetAzureBlobKeysAsync();

            foreach (var item in finalRecognitions)
            {
                var senderDetails = employees.FirstOrDefault(x => x.EmployeeId == item.CreatedBy);
                var recognisationLike = likeDetails.Where(x => x.ModuleDetailsId == item.RecognitionId).ToList();
                var recognisationComment = commentDetails.Where(x => x.ModuleDetailsId == item.RecognitionId).ToList();
                var recognisationAttachment = attachmentDetails.FirstOrDefault(x => x.RecognitionId == item.RecognitionId);
                RecognitionDetailsResponse data = new RecognitionDetailsResponse
                {
                    RecognitionId = item.RecognitionId,
                    Headlines = item.Headlines,
                    Message = item.Message,
                    IsAttachment = item.IsAttachment,
                    RecognitionCategoryTypeId = item.RecognitionCategoryTypeId,
                    ReceiverId = item.ReceiverId,
                    SenderId = item.CreatedBy,
                    IsEditable = userIdentity == null ? false : item.CreatedBy == userIdentity.EmployeeId,
                    CreatedOn = item.CreatedOn,
                    UpdatedOn = item.UpdatedOn == null ? item.CreatedOn : (DateTime)item.UpdatedOn,
                    SenderEmailId = senderDetails == null ? "" : senderDetails.EmailId,
                    SenderFirstName = senderDetails == null ? "" : senderDetails.FirstName,
                    SenderLastName = senderDetails == null ? "" : senderDetails.LastName,
                    SenderFullName = senderDetails == null ? "" : senderDetails.FirstName + " " + senderDetails.LastName,
                    SenderEmployeeId = senderDetails == null ? 0 : senderDetails.EmployeeId,
                    SenderImagePath = senderDetails == null ? "" : senderDetails.ImagePath,
                    receiverDetails = ReceiverDetails(item.RecognitionId, allemployeeTag, allTeam, employees),
                    IsEdited = item.UpdatedOn == null ? false : item.CreatedOn != item.UpdatedOn
                };

                ///Get Like details
                if (likeDetails.Count > 0)
                {
                    data.TotalLikeCount = recognisationLike.Count;
                    data.IsLiked = userIdentity == null ? false : recognisationLike.Any(x => x.CreatedBy == userIdentity.EmployeeId);
                    var recognitionLikes = (from emp in employees
                                            join rec in recognisationLike on emp.EmployeeId equals rec.CreatedBy
                                            select new RecognitionLikeResponse
                                            {
                                                LikeReactionId = rec.LikeReactionId,
                                                EmployeeId = emp.EmployeeId,
                                                FirstName = emp.FirstName,
                                                LastName = emp.LastName,
                                                FullName = emp.FirstName + " " + emp.LastName,
                                                ImagePath = emp.ImagePath,
                                                EmailId = emp.EmailId
                                            }).ToList();
                    data.RecognitionLikeResponses = recognitionLikes;
                }
                ///Get Comment details
                if (recognisationComment.Count > 0)
                {
                    data.TotalCommentCount = recognisationComment.Count;
                    data.IsCommented = userIdentity == null ? false : recognisationComment.Any(x => x.CreatedBy == userIdentity.EmployeeId);
                    data.CommentDetailResponses = CommentDetailResponses(employees, recognisationComment);
                }

                if (recognisationAttachment != null)
                {
                    data.AttachmentImagePath = blobDetails.BlobCdnUrl + blobDetails.BlobContainerName + "/" + AppConstants.RecognitionFolderName + "/" + recognisationAttachment.GuidFileName;
                    data.RecognitionCategoryId = recognisationAttachment.RecognitionCategoryId;
                    data.AttachmentName = recognisationAttachment.Name;
                }

                orgRecognitionResponses.Add(data);
            }
            payload.EntityList = orgRecognitionResponses;
            return payload;
        }

        #region Private


        private async Task CreateEmployeeTags(RecognitionCreateRequest request, UserIdentity userIdentity, long recognitionId)
        {
            foreach (var item in request.RecognitionEmployeeTags.Distinct())
            {
                var recogEmpTags = new EmployeeTag()
                {
                    ModuleDetailsId = recognitionId,
                    TagId = item.Id,
                    IsActive = true,
                    CreatedBy = userIdentity.EmployeeId,
                    CreatedOn = DateTime.UtcNow,
                    ModuleId = item.SearchType == 1 ? (int)ModuleId.Recognisation : (int)ModuleId.TeamTagInRecognisationInBody
                };
                _employeeTagRepo.Add(recogEmpTags);

            }
            foreach (var item in request.ReceiverRequest.Distinct())
            {
                var recogEmpTags = new EmployeeTag()
                {
                    ModuleDetailsId = recognitionId,
                    TagId = item.Id,
                    IsActive = true,
                    CreatedBy = userIdentity.EmployeeId,
                    CreatedOn = DateTime.UtcNow,
                    ModuleId = item.SearchType == 1 ? (int)ModuleId.EmployeeReceiver : (int)ModuleId.TeamReceiver
                };
                _employeeTagRepo.Add(recogEmpTags);
            }
            await UnitOfWorkAsync.SaveChangesAsync();


        }
        private async Task CreateTeamRecognitionMapping(List<ReceiverRequest> request, long recognitionId, UserIdentity userIdentity)
        {
            var teams = request.Where(x => x.SearchType == 2).Select(x => x.Id).Distinct().ToList();
            var employeeTeams = _employeeTeamMappingRepo.GetQueryable().Where(x => teams.Contains(x.TeamId) && x.IsActive).ToList();
            var reportingDetails = _employeeRepo.GetQueryable().Where(x => employeeTeams.Select(x => x.EmployeeId).Contains(x.EmployeeId)).ToList();
            foreach (var teamRecognitionMapping in from item in teams
                                                   let empDetails = employeeTeams.Where(x => x.TeamId == item)
                                                   from emp in empDetails
                                                   let details = reportingDetails.FirstOrDefault(x => x.EmployeeId == emp.EmployeeId)
                                                   let teamRecognitionMapping = new RecognitionEmployeeTeamMapping()
                                                   {
                                                       RecognitionId = recognitionId,
                                                       EmployeeId = emp.EmployeeId,
                                                       TeamId = item,
                                                       CreatedOn = DateTime.UtcNow,
                                                       IsActive = true,
                                                       CreatedBy = userIdentity.EmployeeId,
                                                       IsGivenByManager = !(details == null) && details.ReportingTo == userIdentity.EmployeeId

                                                   }
                                                   select teamRecognitionMapping)
            {
                _recognitionEmployeeTeamMappingRepo.Add(teamRecognitionMapping);
            }

            var employeeIds = request.Where(x => x.SearchType == 1).Select(x => x.Id).Distinct().ToList();
            var reporting = _employeeRepo.GetQueryable().Where(x => employeeIds.Contains(x.EmployeeId)).ToList();
            foreach (var item in employeeIds)
            {
                var details = reporting.FirstOrDefault(x => x.EmployeeId == item);
                var teamRecognitionMapping = new RecognitionEmployeeTeamMapping()
                {
                    RecognitionId = recognitionId,
                    EmployeeId = item,
                    TeamId = 0,
                    CreatedOn = DateTime.UtcNow,
                    IsActive = true,
                    CreatedBy = userIdentity.EmployeeId,
                    IsGivenByManager = details.ReportingTo == userIdentity.EmployeeId
                };
                _recognitionEmployeeTeamMappingRepo.Add(teamRecognitionMapping);
            }

            await UnitOfWorkAsync.SaveChangesAsync();
        }
        private List<ReceiverDetail> ReceiverDetails(long recogId, List<EmployeeTag> employeeTags, List<Team> teams, List<Employee> employees)
        {
            List<ReceiverDetail> receiverDetails = new List<ReceiverDetail>();

            var recvrDetails = employeeTags.Where(x => x.ModuleDetailsId == recogId).ToList();
            foreach (var item in recvrDetails)
            {
                var data = new ReceiverDetail();

                if (item.ModuleId == (int)ModuleId.EmployeeReceiver)
                {
                    var employee = employees.FirstOrDefault(x => x.EmployeeId == item.TagId);
                    if (employee != null)
                    {
                        data.ReceiverName = employee.FirstName + " " + employee.LastName;
                        data.ReceiverId = employee.EmployeeId;
                        data.ReceiverImagePath = employee.ImagePath;
                        data.SearchType = (int)SearchType.Employee;
                        data.ReceiverEmailId = employee.EmailId;
                    }
                }
                else ///Tagged Team
                {
                    var team = teams.FirstOrDefault(x => x.TeamId == item.TagId);
                    if (team != null)
                    {
                        data.ReceiverName = team.TeamName;
                        data.ReceiverId = team.TeamId;
                        data.ReceiverImagePath = team.LogoImagePath;
                        data.SearchType = (int)SearchType.Team;
                        data.ColorCode = team.Colorcode;
                        data.BackGroundColorCode = team.BackGroundColorCode;
                    }


                }
                receiverDetails.Add(data);
            }
            var rnd = new Random();
            var result = receiverDetails.OrderBy(item => rnd.Next()).ToList();
            return result;
        }
        private List<Recognition> GetMyRecognitions(UserIdentity userIdentity)
        {
            var result = new List<Recognition>();
            var teamEmployeeMapping = _recognitionEmployeeTeamMappingRepo.GetQueryable().Where(x => x.IsActive && (x.CreatedBy == userIdentity.EmployeeId || x.EmployeeId == userIdentity.EmployeeId)).ToList();
            if (teamEmployeeMapping != null)
            {
                List<long> recogIds = teamEmployeeMapping.Select(x => x.RecognitionId).ToList();
                result = _recognitionRepo.GetQueryable().Where(x => x.IsActive && recogIds.Contains(x.RecognitionId)).OrderByDescending(x => x.CreatedOn).ToList();
            }
            return result;
        }
        private List<Recognition> GetEmployeeRecognitions(long employeeId)
        {
            var result = new List<Recognition>();
            var teamEmployeeMapping = _recognitionEmployeeTeamMappingRepo.GetQueryable().Where(x => x.IsActive && (x.CreatedBy == employeeId || x.EmployeeId == employeeId)).ToList();
            if (teamEmployeeMapping != null)
            {
                List<long> recogIds = teamEmployeeMapping.Select(x => x.RecognitionId).ToList();
                result = _recognitionRepo.GetQueryable().Where(x => x.IsActive && recogIds.Contains(x.RecognitionId)).OrderByDescending(x => x.CreatedOn).ToList();
            }
            return result;
        }

        private async Task DeleteNotificationsReceiverId(long recognitionId, List<long> empIds)
        {
            var notifications = await _notificationsDetailsRepo.GetQueryable().Where(x => x.NotificationOnId == recognitionId && x.NotificationOnTypeId == 1 && x.NotificationTypeId == (int)EnumNotificationType.Recognition && empIds.Contains((long)x.NotificationsTo)).ToListAsync();
            if (notifications.Count > 0)
            {
                notifications.ForEach(a =>
                {
                    a.IsDeleted = true;
                    a.UpdatedOn = DateTime.UtcNow;

                });
                _notificationsDetailsRepo.UpdateRange(notifications);
                UnitOfWorkAsync.SaveChanges();
            }
        }
        private async Task DeleteRecognitionNotifications(long recognitionId)
        {
            var notifications = await _notificationsDetailsRepo.GetQueryable().Where(x => x.NotificationOnId == recognitionId && x.NotificationOnTypeId == 1 && (x.NotificationTypeId == (int)EnumNotificationType.TagPost || x.NotificationTypeId == (int)EnumNotificationType.Recognition)).ToListAsync();
            if (notifications.Count > 0)
            {
                notifications.ForEach(a =>
                {
                    a.IsDeleted = true;
                    a.UpdatedOn = DateTime.UtcNow;
                });
                _notificationsDetailsRepo.UpdateRange(notifications);
                await UnitOfWorkAsync.SaveChangesAsync();
            }
        }
        private async Task DeleteLikeNotifications(List<long> likeId)
        {
            var notifications = await _notificationsDetailsRepo.GetQueryable().Where(x => likeId.Contains((long)x.NotificationOnId) && x.NotificationOnTypeId == 1 && x.NotificationTypeId == (int)EnumNotificationType.RecognitionLike).ToListAsync();
            if (notifications.Count > 0)
            {
                notifications.ForEach(a =>
                {
                    a.IsDeleted = true;
                    a.UpdatedOn = DateTime.UtcNow;

                });
                _notificationsDetailsRepo.UpdateRange(notifications);
                await UnitOfWorkAsync.SaveChangesAsync();
            }

        }
        private async Task DeleteCommentNotifications(List<long> commentId)
        {
            var notifications = await _notificationsDetailsRepo.GetQueryable().Where(x => commentId.Contains((long)x.NotificationOnId) && x.NotificationOnTypeId == 1 && (x.NotificationTypeId == (int)EnumNotificationType.RecognitionComment || x.NotificationTypeId == (int)EnumNotificationType.TagComment || x.NotificationTypeId == (int)EnumNotificationType.RecognitionReplyComment)).ToListAsync();
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

        public async Task DeleteReplyCommentNotifications(List<long> commentId)
        {
            var notifications = await _notificationsDetailsRepo.GetQueryable().Where(x => commentId.Contains((long)x.NotificationOnId) && x.NotificationOnTypeId == 1 && x.NotificationTypeId == (int)EnumNotificationType.RecognitionReplyComment).ToListAsync();
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
        private async Task DeleteRecognitionImage(long recognitionId, RecognitionImageMapping badges, UserIdentity userIdentity, bool isAttachment, RecognitionCategory recognitionCategory)
        {

            if (badges != null)
            {
                badges.IsActive = false;
                badges.UpdatedOn = DateTime.UtcNow;
                badges.UpdatedBy = userIdentity.EmployeeId;
                _recognitionImageMappingRepo.Update(badges);
                if (isAttachment)
                {
                    var mapping = new RecognitionImageMapping
                    {
                        RecognitionCategoryId = recognitionCategory.RecognitionCategoryId,
                        RecognitionCategoryTypeId = recognitionCategory.RecognitionCategoryTypeId,
                        IsActive = true,
                        CreatedBy = userIdentity.EmployeeId,
                        CreatedOn = DateTime.UtcNow,
                        FileName = recognitionCategory.FileName,
                        GuidFileName = recognitionCategory.GuidFileName,
                        Name = recognitionCategory.Name,
                        RecognitionId = recognitionId
                    };
                    _recognitionImageMappingRepo.Add(mapping);
                }
                await UnitOfWorkAsync.SaveChangesAsync();
            }

            else
            {
                if (isAttachment)
                {
                    var mapping = new RecognitionImageMapping
                    {
                        RecognitionCategoryId = recognitionCategory.RecognitionCategoryId,
                        RecognitionCategoryTypeId = recognitionCategory.RecognitionCategoryTypeId,
                        IsActive = true,
                        CreatedBy = userIdentity.EmployeeId,
                        CreatedOn = DateTime.UtcNow,
                        FileName = recognitionCategory.FileName,
                        GuidFileName = recognitionCategory.GuidFileName,
                        Name = recognitionCategory.Name,
                        RecognitionId = recognitionId
                    };
                    _recognitionImageMappingRepo.Add(mapping);
                    await UnitOfWorkAsync.SaveChangesAsync();
                }

            }

        }
        private async Task<List<long>> CommentTags(CommentCreateCommand request, UserIdentity userIdentity, long commentId, List<long> removeEmpIds)
        {
            var empIds = new List<long>();
            var moduleId = 0;
            var teamModuleId = 0;
            if (request.CommentDetailsRequest.ModuleId == (int)CommentModuleId.Recognisation)
            {
                moduleId = (int)ModuleId.Comments;
                teamModuleId = (int)ModuleId.TeamTagInComment;
            }
            else if (request.CommentDetailsRequest.ModuleId == (int)CommentModuleId.ReplyComments)
            {
                moduleId = (int)ModuleId.ReplyComments;
                teamModuleId = (int)ModuleId.ReplyTeamComments;
            }
            var empOldTag = _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == request.CommentDetailsRequest.CommentDetailsId && x.IsActive && (x.ModuleId == moduleId || x.ModuleId == teamModuleId)).Select(x => x.TagId).ToList();
            var allEmployeeTag = await _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == request.CommentDetailsRequest.CommentDetailsId && (x.ModuleId == moduleId || x.ModuleId == teamModuleId)).ToListAsync();
            foreach (var assignUser in request.CommentDetailsRequest.RecognitionEmployeeTags.Distinct().ToList())
            {
                var employeeTag = allEmployeeTag.FirstOrDefault(x => x.TagId == assignUser.Id);
                if (employeeTag != null)
                {
                    employeeTag.IsActive = request.IsActive;
                    employeeTag.UpdatedBy = userIdentity.EmployeeId;
                    employeeTag.UpdatedOn = request.UpdatedOn;
                    _employeeTagRepo.Update(employeeTag);
                }
                else
                {
                    var commentEmployeeTag = new EmployeeTag()
                    {
                        ModuleDetailsId = request.CommentDetailsRequest.CommentDetailsId == 0 ? commentId : request.CommentDetailsRequest.CommentDetailsId,
                        TagId = assignUser.Id,
                        IsActive = request.IsActive,
                        CreatedBy = userIdentity.EmployeeId,
                        CreatedOn = request.CreatedOn,
                        ModuleId = assignUser.SearchType == 1 ? moduleId : teamModuleId
                    };
                    _employeeTagRepo.Add(commentEmployeeTag);
                }

            }
            var empNewTag = request.CommentDetailsRequest.RecognitionEmployeeTags.Distinct().ToList();
            if (empOldTag.Count > 0 && empNewTag.Count > 0)
            {
                var empFinalList = empOldTag.Except(empNewTag.Select(x => x.Id)).ToList();
                if (empFinalList.Count > 0)
                {
                    var empFileRecords = await _employeeTagRepo.GetQueryable().Where(x => empFinalList.Contains(x.TagId)).ToListAsync();

                    ////update the status of Note File as inactive
                    if (empFileRecords.Count > 0)
                    {
                        empFileRecords.ForEach(a =>
                        {
                            a.IsActive = false;
                            a.UpdatedBy = userIdentity.EmployeeId;
                            a.UpdatedOn = request.UpdatedOn;
                            _employeeTagRepo.Update(a);
                        });
                        UnitOfWorkAsync.SaveChanges();
                    }
                }
            }

            if (empOldTag.Count > 0 && empNewTag.Count == 0)
            {
                var empFileRecords = await _employeeTagRepo.GetQueryable().Where(x => empOldTag.Contains(x.TagId)).ToListAsync();

                ////update the status of Employee as inactive
                if (empFileRecords.Count > 0)
                {
                    empFileRecords.ForEach(a =>
                    {
                        a.IsActive = false;
                        a.UpdatedBy = userIdentity.EmployeeId;
                        a.UpdatedOn = request.UpdatedOn;
                        _employeeTagRepo.Update(a);
                    });
                    UnitOfWorkAsync.SaveChanges();
                }
            }
            await UnitOfWorkAsync.SaveChangesAsync();

            if (empOldTag.Count == 0 && empNewTag.Count > 0 && request.CommentDetailsRequest.ModuleId == (int)CommentModuleId.Recognisation)
            {

                var newTag = await _notificationsService.TagCommentNotifications(request.CommentDetailsRequest.ModuleDetailsId, empNewTag, userIdentity, request.CommentDetailsRequest.CommentDetailsId == 0 ? commentId : request.CommentDetailsRequest.CommentDetailsId, removeEmpIds).ConfigureAwait(false);
                empIds.AddRange(newTag);
            }

            if (empOldTag.Count > 0 && empNewTag.Count > 0 && request.CommentDetailsRequest.ModuleId == (int)CommentModuleId.Recognisation)
            {
                var empFinalList = empNewTag.Select(x => x.Id).Except(empOldTag).ToList();
                if (empFinalList.Count > 0)
                {

                    var finalTag = await _notificationsService.TagFinalCommentNotifications(request.CommentDetailsRequest.ModuleDetailsId, empFinalList, userIdentity, request.CommentDetailsRequest.CommentDetailsId == 0 ? commentId : request.CommentDetailsRequest.CommentDetailsId, removeEmpIds).ConfigureAwait(false);
                    empIds.AddRange(finalTag);

                }

            }
            return empIds;
        }
        private async Task CommentImage(CommentCreateCommand request, UserIdentity userIdentity)
        {
            var recOldFile = new List<string>();
            var recAllFiles = new List<RecognitionImageMapping>();
            if (request.CommentDetailsRequest.ModuleId == (int)CommentModuleId.Recognisation)
            {
                recOldFile = _recognitionImageMappingRepo.GetQueryable().Where(x => x.RecognitionId == request.CommentDetailsRequest.ModuleDetailsId && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.RecognitionCommentScreenshot && x.IsActive).Select(x => x.GuidFileName).ToList();
                recAllFiles = await _recognitionImageMappingRepo.GetQueryable().Where(x => x.RecognitionId == request.CommentDetailsRequest.ModuleDetailsId && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.RecognitionCommentScreenshot).ToListAsync();
            }
            else if (request.CommentDetailsRequest.ModuleId == (int)CommentModuleId.ReplyComments)
            {
                recOldFile = _recognitionImageMappingRepo.GetQueryable().Where(x => x.RecognitionId == request.CommentDetailsRequest.ModuleDetailsId && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.RecognitionReplyCommentScreenshot && x.IsActive).Select(x => x.GuidFileName).ToList();
                recAllFiles = await _recognitionImageMappingRepo.GetQueryable().Where(x => x.RecognitionId == request.CommentDetailsRequest.ModuleDetailsId && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.RecognitionReplyCommentScreenshot).ToListAsync();
            }
             
            foreach (var assignFiles in request.CommentDetailsRequest.RecognitionImageRequests)
            {

                var recFiles = recAllFiles.FirstOrDefault(x => x.GuidFileName == assignFiles.GuidFileName);
                if (recFiles != null)
                {
                    recFiles.RecognitionId = request.CommentDetailsRequest.ModuleDetailsId;
                    recFiles.RecognitionCategoryId = 0;
                    recFiles.RecognitionCategoryTypeId = request.CommentDetailsRequest.ModuleId == (int)CommentModuleId.Recognisation ? (int)RecognitionCategoryType.RecognitionCommentScreenshot : (int)RecognitionCategoryType.RecognitionReplyCommentScreenshot;
                    recFiles.IsActive = request.IsActive;
                    recFiles.FileName = assignFiles.FileName;
                    recFiles.GuidFileName = assignFiles.GuidFileName;
                    recFiles.Name = "";
                    recFiles.UpdatedOn = request.UpdatedOn;
                    recFiles.UpdatedBy = userIdentity.EmployeeId;
                    _recognitionImageMappingRepo.Update(recFiles);
                    await UnitOfWorkAsync.SaveChangesAsync();
                }
                else
                {
                    var recognitionImageMapping = new RecognitionImageMapping
                    {
                        RecognitionId = request.CommentDetailsRequest.ModuleDetailsId,
                        RecognitionCategoryId = 0,
                        RecognitionCategoryTypeId = request.CommentDetailsRequest.ModuleId == (int)CommentModuleId.Recognisation ? (int)RecognitionCategoryType.RecognitionCommentScreenshot : (int)RecognitionCategoryType.RecognitionReplyCommentScreenshot,
                        IsActive = request.IsActive,
                        CreatedBy = userIdentity.EmployeeId,
                        CreatedOn = request.CreatedOn,
                        FileName = assignFiles.FileName,
                        GuidFileName = assignFiles.GuidFileName,
                        Name = "",
                        UpdatedOn = request.CreatedOn
                    };
                    _recognitionImageMappingRepo.Add(recognitionImageMapping);
                    await UnitOfWorkAsync.SaveChangesAsync();
                }

            }

            var recNewFile = request.CommentDetailsRequest.RecognitionImageRequests.Select(x => x.GuidFileName).ToList();

            if (recOldFile.Count > 0 && recNewFile.Count > 0)
            {
                var convrFinalList = recOldFile.Except(recNewFile).ToList();
                if (convrFinalList.Count > 0)
                {
                    var convrFileRecords = await _recognitionImageMappingRepo.GetQueryable().Where(x => convrFinalList.Contains(x.GuidFileName)).ToListAsync();

                    ////update the status of File as inactive
                    if (convrFileRecords.Count > 0)
                    {
                        convrFileRecords.ForEach(a =>
                        {
                            a.IsActive = false;
                            a.UpdatedBy = userIdentity.EmployeeId;
                            a.UpdatedOn = request.UpdatedOn;
                            _recognitionImageMappingRepo.Update(a);
                        });
                        UnitOfWorkAsync.SaveChanges();
                    }
                }
            }

            if (recOldFile.Count > 0 && recNewFile.Count == 0)
            {
                var convrFileRecords = await _recognitionImageMappingRepo.GetQueryable().Where(x => recOldFile.Contains(x.GuidFileName)).ToListAsync();

                ////update the status of File as inactive
                if (convrFileRecords.Count > 0)
                {
                    convrFileRecords.ForEach(a =>
                    {
                        a.IsActive = false;
                        a.UpdatedBy = userIdentity.EmployeeId;
                        a.UpdatedOn = request.UpdatedOn;
                        _recognitionImageMappingRepo.Update(a);
                    });
                    UnitOfWorkAsync.SaveChanges();
                }

            }
        }
        private List<CommentDetailResponse> CommentDetailResponses(List<Employee> employees, List<CommentDetails> commentDetails)
        {
            List<CommentDetailResponse> comments = new List<CommentDetailResponse>();
            foreach (var item in commentDetails)
            {
                var commentedEmp = employees.FirstOrDefault(x => x.EmployeeId == item.CreatedBy);
                var commentDetailResponse = new CommentDetailResponse()
                {
                    CommentDetailsId = item.CommentDetailsId,
                    Comments = item.Comments,
                    EmployeeId = item.CreatedBy,
                    FirstName = commentedEmp == null ? "" : commentedEmp.FirstName,
                    LastName = commentedEmp == null ? "" : commentedEmp.LastName,
                    FullName = commentedEmp == null ? "" : commentedEmp.FirstName + " " + commentedEmp.LastName,
                    ImagePath = commentedEmp == null ? "" : commentedEmp.ImagePath,
                    CreatedOn = item.CreatedOn,
                    UpdatedOn = item.UpdatedOn,
                    IsEdited = item.UpdatedOn == null ? false : item.CreatedOn != item.UpdatedOn
                };
                comments.Add(commentDetailResponse);
            }
            return comments;
        }

        private async Task<List<Recognition>> GetRecognitionForWall(long recognitionId, bool isMyPost, long id, UserIdentity userIdentity, int searchType)
        {
            var recognitionDetails = new List<Recognition>();
            if (recognitionId > 0)
                recognitionDetails = _recognitionRepo.GetQueryable().Where(x => x.IsActive && x.RecognitionId == recognitionId).OrderByDescending(x => x.CreatedOn).ToList();
            else
            {
                if (isMyPost)
                {
                    recognitionDetails = GetMyRecognitions(userIdentity);
                }
                else if (!isMyPost && id > 0 && searchType == (int)SearchType.Employee)
                {
                    recognitionDetails = GetEmployeeRecognitions(id);
                }
                else if (searchType == (int)SearchType.Team && id > 0)
                {
                    var teamRecognition = _employeeTagRepo.GetQueryable().Where(x => x.IsActive  && (x.ModuleId == (int)ModuleId.TeamReceiver || x.ModuleId == (int)ModuleId.TeamTagInRecognisationInBody) && x.TagId == id).ToList();
                    if (teamRecognition.Count > 0)
                    {
                        List<long> ids = teamRecognition.Select(x => x.ModuleDetailsId).ToList();
                        recognitionDetails = _recognitionRepo.GetQueryable().Where(x => x.IsActive && ids.Contains(x.RecognitionId)).OrderByDescending(x => x.CreatedOn).ToList();
                    }
                }
                else
                    recognitionDetails = _recognitionRepo.GetQueryable().Where(x => x.IsActive).OrderByDescending(x => x.CreatedOn).ToList();
            }
            return recognitionDetails;
        }

        private async Task<MyWallOfFameTeam> MyWallOfFameFilter(MyWallOfFameGetQuery request, bool isOrderbyDesc = false)
        {
            long Id = request.MyMyWallOfFameRequest.Id; //EmployeeId and TeamID
            int serarchType = request.MyMyWallOfFameRequest.SearchType;          
            // long RecognitionId = 250;
            //List<long> RecognitionId = new List<long> { 212, 250, 261 };
            //List<long> RecognitionId = new List<long> { 215, 135, 241, 248, 249 };
            // ALL:-> Emp+Team
            //Employee :->Emp
            //Team :->
            MyWallOfFameTeam myWallOfFameTeam = new MyWallOfFameTeam();
            myWallOfFameTeam.Id = request.MyMyWallOfFameRequest.Id;
            myWallOfFameTeam.SearchType = request.MyMyWallOfFameRequest.SearchType;

            var recognitionsData = await (_recognitionRepo.GetQueryable().Where(x => x.IsActive
                                                   && x.IsAttachment
                                 && x.RecognitionCategoryTypeId == (int)RecognitionCategoryType.Badge
                                 // && RecognitionId.Contains(x.RecognitionId)

                                 )).ToListAsync();

            var recognitionIds = recognitionsData.Select(x => x.RecognitionId).Distinct().ToList();


            var employeeTeamMappings = await _recognitionEmployeeTeamMappingRepo.GetQueryable().Where(x =>
                          x.IsActive && recognitionIds.Contains(x.RecognitionId)
                         && (serarchType == (int)SearchType.Employee ? (x.EmployeeId == Id) //Team+emp need ignore emp.
                            : serarchType == (int)SearchType.Team ? (x.TeamId == Id)
                                                 : (x.TeamId >= 0 && x.EmployeeId == Id)
                                                )).ToListAsync();

            var employeeWiseList = employeeTeamMappings.Where(x =>
                       (x.TeamId == 0)).ToList();
            var teamWiseList = employeeTeamMappings.Where(x =>
                      (x.TeamId > 0)).ToList();
            List<RecognitionEmployeeTeamMapping> updatedEmployeeWiseList = new List<RecognitionEmployeeTeamMapping>();

            //Ignore Employee if Team has same badge
            foreach (var empTeam in employeeWiseList)
            {
                var findTeam = teamWiseList.FirstOrDefault(x => x.RecognitionId == empTeam.RecognitionId);
                if (findTeam == null)
                    updatedEmployeeWiseList.Add(empTeam);
            }
            List<RecognitionEmployeeTeamMapping> updatedEmployeeTeamWiseList = new List<RecognitionEmployeeTeamMapping>();
            //Group by Base on Team
            if (employeeTeamMappings.Any())
            {
                //Team Waise Single Count and Added

                var teamsMappings = teamWiseList.GroupBy(w => new { w.RecognitionId, w.TeamId }, (key, item) => new
                {
                    TeamId = key.TeamId,
                    RecognitionId = key.RecognitionId,
                    Items = item
                }).Select(y => new RecognitionEmployeeTeamMapping
                {
                    TeamId = y.TeamId,
                    RecognitionId = y.RecognitionId,
                    EmployeeId = y.Items.FirstOrDefault().EmployeeId,
                    CreatedOn = y.Items.FirstOrDefault().CreatedOn,
                    IsActive = y.Items.FirstOrDefault().IsActive,
                    CreatedBy = y.Items.FirstOrDefault().CreatedBy,
                    RecognitionEmployeeTeamMappingId = y.Items.FirstOrDefault().RecognitionEmployeeTeamMappingId
                }).ToList();
                updatedEmployeeTeamWiseList.AddRange(teamsMappings);

                if (serarchType == (int)SearchType.Employee || serarchType == (int)SearchType.All)
                {
                    //All Employee data added
                    var employeesMappings = updatedEmployeeWiseList
                .Select(y => new RecognitionEmployeeTeamMapping
                {
                    TeamId = y.TeamId,
                    RecognitionId = y.RecognitionId,
                    EmployeeId = y.EmployeeId,
                    CreatedOn = y.CreatedOn,
                    IsActive = y.IsActive,
                    CreatedBy = y.CreatedBy,
                    RecognitionEmployeeTeamMappingId = y.RecognitionEmployeeTeamMappingId
                }).ToList();

                    updatedEmployeeTeamWiseList.AddRange(employeesMappings);
                }
            }
            myWallOfFameTeam.RecognitionEmployeeTeamMappings = updatedEmployeeTeamWiseList;
            myWallOfFameTeam.Recognitions = recognitionsData;

            var finalRecognitionIds = updatedEmployeeTeamWiseList.Select(x => x.RecognitionId).Distinct().ToList();
            var teamIds = updatedEmployeeTeamWiseList.Where(x => x.TeamId != null).Select(x => x.TeamId.Value).Distinct().ToList();

            var teams = _teamRepo.GetQueryable().Where(x => x.IsActive && teamIds.Contains(x.TeamId)).ToList();
            myWallOfFameTeam.Teams = teams;
            var images = _recognitionImageMappingRepo.GetQueryable().Where(x => x.IsActive
                           && finalRecognitionIds.Contains(x.RecognitionId) && x.RecognitionCategoryTypeId==(int)(RecognitionCategoryType.Badge)
                     ).ToList();
            var lastimag = _recognitionCategoryRepo.GetQueryable().Where(x => x.IsActive
             && images.Select(y => y.RecognitionCategoryId).Contains(x.RecognitionCategoryId)).ToList();

            List<RecognitionImageMapping> updatedImages = new List<RecognitionImageMapping>();
            foreach (var itemImg in images)
            {
                if (itemImg != null)
                {
                    var lastImg = lastimag.FirstOrDefault(x => x.RecognitionCategoryId == itemImg.RecognitionCategoryId);
                    if (lastImg != null)
                    {
                        itemImg.GuidFileName = lastImg.GuidFileName;
                        itemImg.FileName = lastImg.FileName;
                        itemImg.Name = lastImg.Name;
                    }

                }
                updatedImages.Add(itemImg);
            }

            myWallOfFameTeam.RecognitionImageMappings = updatedImages;
            myWallOfFameTeam.RecognitionImageMappings = myWallOfFameTeam.RecognitionImageMappings.ToList()
                                     .OrderByDescending(x => x.CreatedOn)
                                     .ThenByDescending(x => x.CreatedOn).ToList();
            return myWallOfFameTeam;
        }

        private async Task DeleteRecognitionAttachment(long recognitionId, long loginEmpId)
        {
            var recognitionAttachment = await _recognitionImageMappingRepo.GetQueryable().Where(x => x.RecognitionId == recognitionId && x.IsActive).ToListAsync();
            if (recognitionAttachment.Count > 0)
            {
                recognitionAttachment.ForEach(a =>
                {
                    a.IsActive = false;
                    a.UpdatedBy = loginEmpId;
                    a.UpdatedOn = DateTime.UtcNow;
                });
                _recognitionImageMappingRepo.UpdateRange(recognitionAttachment);
                await UnitOfWorkAsync.SaveChangesAsync();
            }
        }

        private async Task DeleteRecognitionComment(long recognitionId, long loginEmpId)
        {
            var recognitionComment = await _commentDetailsRepo.GetQueryable().Where(x => x.ModuleDetailsId == recognitionId && x.ModuleId == (int)CommentModuleId.Recognisation && x.IsActive).ToListAsync();
            var replyComment = await _commentDetailsRepo.GetQueryable().Where(x => recognitionComment.Select(x => x.CommentDetailsId).Contains(x.ModuleDetailsId) && x.ModuleId == (int)CommentModuleId.ReplyComments && x.IsActive).ToListAsync();
            if (recognitionComment.Count > 0)
            {
                recognitionComment.ForEach(a =>
                {
                    a.IsActive = false;
                    a.UpdatedBy = loginEmpId;
                    a.UpdatedOn = DateTime.UtcNow;
                });
                _commentDetailsRepo.UpdateRange(recognitionComment);
                await DeleteCommentNotifications(recognitionComment.Select(x => x.CommentDetailsId).ToList()).ConfigureAwait(false);
               
            }
            if (replyComment.Count > 0)
            {
                replyComment.ForEach(a =>
                {
                    a.IsActive = false;
                    a.UpdatedBy = loginEmpId;
                    a.UpdatedOn = DateTime.UtcNow;
                });
                _commentDetailsRepo.UpdateRange(replyComment);
                await DeleteCommentNotifications(replyComment.Select(x => x.CommentDetailsId).ToList()).ConfigureAwait(false);
            }

            await UnitOfWorkAsync.SaveChangesAsync();
        }

        private async Task DeleteRecognitionLike(long recognitionId, long loginEmpId)
        {
            var likes = await _likeReactionRepo.GetQueryable().Where(x => x.ModuleDetailsId == recognitionId && x.ModuleId != (int)ModuleId.Conversation && x.IsActive).ToListAsync();
            if (likes.Count > 0)
            {
                likes.ForEach(a =>
                {
                    a.IsActive = false;
                    a.UpdatedBy = loginEmpId;
                    a.UpdatedOn = DateTime.UtcNow;
                });
                _likeReactionRepo.UpdateRange(likes);
                await DeleteLikeNotifications(likes.Select(x => x.LikeReactionId).ToList()).ConfigureAwait(false);
                await UnitOfWorkAsync.SaveChangesAsync();
            }
        }

        private async Task DeleteRecognitionEmployeeTag(long recognitionId, long loginEmpId)
        {
            var employeeTag = await _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == recognitionId).ToListAsync();
            if (employeeTag.Count > 0)
            {
                employeeTag.ForEach(a =>
                {
                    a.IsActive = false;
                    a.UpdatedBy = loginEmpId;
                    a.UpdatedOn = DateTime.UtcNow;

                });
                _employeeTagRepo.UpdateRange(employeeTag);
                await UnitOfWorkAsync.SaveChangesAsync();
            }
        }

        private async Task DeleteRecognitionEmployeeMapping(long recognitionId)
        {
            var recognitionMapping = await _recognitionEmployeeTeamMappingRepo.GetQueryable().Where(x => x.RecognitionId == recognitionId && x.IsActive).ToListAsync();
            if (recognitionMapping.Count > 0)
            {
                recognitionMapping.ForEach(a =>
                {
                    a.IsActive = false;
                });
                _recognitionEmployeeTeamMappingRepo.UpdateRange(recognitionMapping);
                await UnitOfWorkAsync.SaveChangesAsync();
            }
        }
        private async Task<MyWallOfFameResponse> WallofFameFirst(MyWallOfFameTeam myWallOfFames, long loggedInUserId)
        {
            MyWallOfFameResponse mywall = new MyWallOfFameResponse();
            mywall.GivenByText = AppConstants.BadgesGivenByPeers;
            var blobDetails = await _keyVaultService.GetAzureBlobKeysAsync();
            string imgPath = blobDetails.BlobCdnUrl +
                        blobDetails.BlobContainerName + "/" + AppConstants.RecognitionFolderName + "/";
            List<long> employessIds = new List<long>();
            employessIds.AddRange(myWallOfFames.Recognitions.Select(x => x.ReceiverId).Distinct().ToList());
            employessIds.AddRange(myWallOfFames.Recognitions.Select(x => x.CreatedBy).Distinct().ToList());
            employessIds.AddRange(myWallOfFames.RecognitionEmployeeTeamMappings.Where(x => x != null).Select(x => x.CreatedBy).Distinct().ToList());
            employessIds.AddRange(myWallOfFames.RecognitionEmployeeTeamMappings.Where(x => x != null).Select(x => x.EmployeeId).Distinct().ToList());
            employessIds = employessIds.Distinct().ToList();
            var employees = _employeeRepo.GetQueryable().Where(x => x.IsActive
                   && employessIds.Any(y => y == x.EmployeeId)).ToList();


            var getEmployeeEng = _employeeEngagementRepo.GetQueryable().OrderByDescending(x => x.EmployeeEngagementId).FirstOrDefault(x => x.IsActive.Value &&
           x.EngagementTypeId == AppConstants.Engagement_RecognitionUpdate && x.EmployeeId == loggedInUserId);

            var filterRecognitionImages = myWallOfFames.RecognitionImageMappings.Where(x =>
                           x != null)
                // .Where(x => x.Name == "Daredevil")
                .ToList();

            var distinctRecognitionIds = (from p in filterRecognitionImages select p.RecognitionId).Distinct().ToList();
            var distinctfileNames = (from p in filterRecognitionImages select p.Name).Distinct().ToList();
            var groupByFileNames = filterRecognitionImages.Where(x => distinctfileNames.Contains(x.Name)).ToList();

            foreach (var file in groupByFileNames.Select(x => new
            {
                FileName = x.FileName,
                GuidFileName = x.GuidFileName,
                Name = x.Name
            }).Distinct().ToList())//group by
            {
                var myWalls = myWallOfFames.RecognitionImageMappings.Where(x =>
                x.Name == file.Name && x.FileName == file.FileName && x.GuidFileName == file.GuidFileName);
                var fileRecognitions = myWalls.Select(x => x).Where(x => x.Name == file.Name
                  && x.FileName == file.FileName && x.GuidFileName == file.GuidFileName)
                 .Select(y => y.RecognitionId).ToList().Distinct();
                var fileEmployeeTeamMappings =
                  myWalls.Select(x => x)
                  .Where(y => fileRecognitions.Contains(y.RecognitionId)).ToList();

                var employeeTeamMappingEmployees = myWallOfFames.RecognitionEmployeeTeamMappings.Where(x =>
                         (x.TeamId == 0 && fileRecognitions.Contains(x.RecognitionId))).ToList();

                var employeeTeamMappingsTeams = myWallOfFames.RecognitionEmployeeTeamMappings.Where(x =>
                     (x.TeamId > 0) && fileRecognitions.Contains(x.RecognitionId)).ToList();


                //file Wise
                if (employeeTeamMappingEmployees.Count > 0 || employeeTeamMappingsTeams.Count > 0)
                {
                    RecognitionImageMappingResponse recognitionImg = new RecognitionImageMappingResponse();
                    foreach (var item in myWalls)
                    {
                        if (getEmployeeEng != null && getEmployeeEng.CreatedOn <= item.CreatedOn)
                        {
                            recognitionImg.IsNewBadge = true;
                            break;
                        }
                    }


                    recognitionImg.FileName = file.FileName;
                    recognitionImg.GuidFileName = file.GuidFileName;
                    recognitionImg.Name = file.Name;
                    recognitionImg.ImageFilePath = imgPath
                           + file.GuidFileName;




                    recognitionImg.TotalCount = employeeTeamMappingEmployees.Count + employeeTeamMappingsTeams.Count;

                    recognitionImg.RecognitionUserDetails.AddRange(employeeTeamMappingEmployees.GroupBy(w => new { w.CreatedBy }, (key, item) => new RecognitionUserDetailsResponse
                    {
                        EmployeeId = key.CreatedBy,
                        Count = employeeTeamMappingEmployees.Where(y => y.CreatedBy == key.CreatedBy).ToList().Count,
                        EmailId = employees.FirstOrDefault(x => x.EmployeeId == key.CreatedBy)?.EmailId,
                        ImagePath = employees.FirstOrDefault(x => x.EmployeeId == key.CreatedBy)?.ImagePath,
                        FirstName = employees.FirstOrDefault(x => x.EmployeeId == key.CreatedBy)?.FirstName,
                        LastName = employees.FirstOrDefault(x => x.EmployeeId == key.CreatedBy)?.LastName,
                    }).ToList());

                    //Teams
                    recognitionImg.RecognitionUserDetails.AddRange(employeeTeamMappingsTeams.GroupBy(w => new
                    {
                        w.CreatedBy,
                        w.TeamId
                    }, (key, item) => new RecognitionUserDetailsResponse
                    {

                        EmployeeId = key.CreatedBy,
                        Count = employeeTeamMappingsTeams.Where(y => y.CreatedBy == key.CreatedBy && y.TeamId == key.TeamId).ToList().Count,
                        EmailId = employees.FirstOrDefault(x => x.EmployeeId == key.CreatedBy)?.EmailId,
                        ImagePath = employees.FirstOrDefault(x => x.EmployeeId == key.CreatedBy)?.ImagePath,
                        FirstName = employees.FirstOrDefault(x => x.EmployeeId == key.CreatedBy)?.FirstName,
                        LastName = employees.FirstOrDefault(x => x.EmployeeId == key.CreatedBy)?.LastName,
                        TeamId = key.TeamId.Value,
                        TeamName = myWallOfFames.Teams.FirstOrDefault(x => x.TeamId == key.TeamId.Value)?.TeamName,
                        LogoName = myWallOfFames.Teams.FirstOrDefault(x => x.TeamId == key.TeamId.Value)?.LogoName,
                        LogoImagePath = myWallOfFames.Teams.FirstOrDefault(x => x.TeamId == key.TeamId.Value)?.LogoImagePath,
                        BackGroundColorCode = myWallOfFames.Teams.FirstOrDefault(x => x.TeamId == key.TeamId.Value)?.BackGroundColorCode,
                        ColorCode = myWallOfFames.Teams.FirstOrDefault(x => x.TeamId == key.TeamId.Value)?.Colorcode,

                    }).ToList());
                    mywall.RecognitionImageMappings.Add(recognitionImg);
                }
            }

            return mywall;
        }

        private async Task<MyWallOfFameResponse> WallofFameSecond(MyWallOfFameTeam myWallOfFames, long loggedInUserId)
        {
            MyWallOfFameResponse mywall = new MyWallOfFameResponse();
            mywall.GivenByText = AppConstants.OnlyTeamBadges;
            var blobDetails = await _keyVaultService.GetAzureBlobKeysAsync();
            string imgPath = blobDetails.BlobCdnUrl +
                        blobDetails.BlobContainerName + "/" + AppConstants.RecognitionFolderName + "/";
            List<long> employessIds = new List<long>();
            employessIds.AddRange(myWallOfFames.Recognitions.Select(x => x.ReceiverId).Distinct().ToList());
            employessIds.AddRange(myWallOfFames.Recognitions.Select(x => x.CreatedBy).Distinct().ToList());
            employessIds.AddRange(myWallOfFames.RecognitionEmployeeTeamMappings.Where(x => x != null).Select(x => x.CreatedBy).Distinct().ToList());
            employessIds.AddRange(myWallOfFames.RecognitionEmployeeTeamMappings.Where(x => x != null).Select(x => x.EmployeeId).Distinct().ToList());
            employessIds = employessIds.Distinct().ToList();
            var employees = _employeeRepo.GetQueryable().Where(x => x.IsActive
                   && employessIds.Any(y => y == x.EmployeeId)).ToList();


            var getEmployeeEng = _employeeEngagementRepo.GetQueryable().OrderByDescending(x => x.EmployeeEngagementId).FirstOrDefault(x => x.IsActive.Value &&
           x.EngagementTypeId == AppConstants.Engagement_RecognitionUpdate && x.EmployeeId == loggedInUserId);

            var filterRecognitionImages = myWallOfFames.RecognitionImageMappings.Where(x =>
                           x != null)
                // .Where(x => x.Name == "Daredevil")
                .ToList();

            var distinctRecognitionIds = (from p in filterRecognitionImages select p.RecognitionId).Distinct().ToList();
            var distinctfileNames = (from p in filterRecognitionImages select p.Name).Distinct().ToList();
            var groupByFileNames = filterRecognitionImages.Where(x => distinctfileNames.Contains(x.Name)).ToList();

            foreach (var file in groupByFileNames.Select(x => new
            {
                FileName = x.FileName,
                GuidFileName = x.GuidFileName,
                Name = x.Name
            }).Distinct().ToList())//group by
            {
                //var myWalls = myWallOfFames.RecognitionImageMappings.Where(x => x.Name == file.Name);
                //var fileRecognitions = myWalls.Select(x => x).Where(x => x.Name == file.Name)
                //    .Select(y => y.RecognitionId).Distinct();
                var myWalls = myWallOfFames.RecognitionImageMappings.Where(x =>
                x.Name == file.Name && x.FileName == file.FileName && x.GuidFileName == file.GuidFileName);
                var fileRecognitions = myWalls.Select(x => x).Where(x => x.Name == file.Name
                  && x.FileName == file.FileName && x.GuidFileName == file.GuidFileName)
                 .Select(y => y.RecognitionId).ToList().Distinct();
                var fileEmployeeTeamMappings =
                             myWalls.Select(x => x)
                             .Where(y => fileRecognitions.Contains(y.RecognitionId)).ToList();
                var employeeTeamMappingsTeams = myWallOfFames.RecognitionEmployeeTeamMappings.Where(x =>
                                      (x.TeamId > 0 && fileRecognitions.Contains(x.RecognitionId))).ToList();
                if (employeeTeamMappingsTeams.Count > 0)
                {
                    RecognitionImageMappingResponse recognitionImg = new RecognitionImageMappingResponse();
                    recognitionImg.FileName = file.FileName;
                    recognitionImg.GuidFileName = file.GuidFileName;
                    recognitionImg.Name = file.Name;
                    recognitionImg.ImageFilePath = imgPath
                           + file.GuidFileName;

                    //file Wise

                    foreach (var item in myWalls)
                    {
                        if (getEmployeeEng != null && getEmployeeEng.CreatedOn <= item.CreatedOn)
                        {
                            recognitionImg.IsNewBadge = true;
                            break;
                        }
                    }


                    recognitionImg.TotalCount = employeeTeamMappingsTeams.Count;


                    //Teams
                    recognitionImg.RecognitionUserDetails.AddRange(employeeTeamMappingsTeams.GroupBy(w => new
                    {
                        w.CreatedBy,
                        w.TeamId
                    }, (key, item) => new RecognitionUserDetailsResponse
                    {

                        EmployeeId = key.CreatedBy,
                        Count = employeeTeamMappingsTeams.Where(y => y.CreatedBy == key.CreatedBy && y.TeamId == key.TeamId).ToList().Count,
                        EmailId = employees.FirstOrDefault(x => x.EmployeeId == key.CreatedBy)?.EmailId,
                        ImagePath = employees.FirstOrDefault(x => x.EmployeeId == key.CreatedBy)?.ImagePath,
                        FirstName = employees.FirstOrDefault(x => x.EmployeeId == key.CreatedBy)?.FirstName,
                        LastName = employees.FirstOrDefault(x => x.EmployeeId == key.CreatedBy)?.LastName,
                        TeamId = key.TeamId.Value,
                        TeamName = myWallOfFames.Teams.FirstOrDefault(x => x.TeamId == key.TeamId.Value)?.TeamName,
                        LogoName = myWallOfFames.Teams.FirstOrDefault(x => x.TeamId == key.TeamId.Value)?.LogoName,
                        LogoImagePath = myWallOfFames.Teams.FirstOrDefault(x => x.TeamId == key.TeamId.Value)?.LogoImagePath,
                        BackGroundColorCode = myWallOfFames.Teams.FirstOrDefault(x => x.TeamId == key.TeamId.Value)?.BackGroundColorCode,
                        ColorCode = myWallOfFames.Teams.FirstOrDefault(x => x.TeamId == key.TeamId.Value)?.Colorcode,

                    }).ToList());
                    mywall.RecognitionImageMappings.Add(recognitionImg);
                }
            }

            return mywall;
        }

        #endregion


    }

}

