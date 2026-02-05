using Microsoft.EntityFrameworkCore;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Infrastructure.Services.Contracts;
using OkrConversationService.Persistence.EntityFrameworkDataAccess;
using OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Services
{
    public class NotificationsEmailsService : BaseService, INotificationsEmailsService
    {
        private readonly IKeyVaultService _keyVaultService;
        private readonly ICommonService _commonService;
        private readonly IRepositoryAsync<Employee> _employeeRepo;
        private readonly IRepositoryAsync<GoalObjective> _goalObjectiveRepo;
        private readonly IRepositoryAsync<GoalKey> _goalKeyRepo;
        private readonly IRepositoryAsync<CycleSymbol> _cycleSymbolsRepo;
        private readonly IRepositoryAsync<TeamCycleDetail> _teamCycleDetailsRepo;
        private readonly IRepositoryAsync<Recognition> _recognitionRepo;
        private readonly IRepositoryAsync<EmployeeTag> _employeeTagRepo;
        private readonly IRepositoryAsync<EmployeeTeamMapping> _employeeTeamMappingRepo;
        private readonly IRepositoryAsync<RecognitionEmployeeTeamMapping> _recognitionEmployeeTeamMappingRepo;
        private readonly IRepositoryAsync<CommentDetails> _commentDetailsRepo;
        private readonly IRepositoryAsync<Conversation> _conversationRepo;
        [Obsolete("")]
        public NotificationsEmailsService(IServicesAggregator servicesAggregateService, IKeyVaultService keyVault, ICommonService commonService) :
            base(servicesAggregateService)
        {
            _keyVaultService = keyVault;
            _commonService = commonService;
            _employeeRepo = UnitOfWorkAsync.RepositoryAsync<Employee>();
            _goalObjectiveRepo = UnitOfWorkAsync.RepositoryAsync<GoalObjective>();
            _goalKeyRepo = UnitOfWorkAsync.RepositoryAsync<GoalKey>();
            _cycleSymbolsRepo = UnitOfWorkAsync.RepositoryAsync<CycleSymbol>();
            _teamCycleDetailsRepo = UnitOfWorkAsync.RepositoryAsync<TeamCycleDetail>();
            _recognitionRepo = UnitOfWorkAsync.RepositoryAsync<Recognition>();
            _employeeTagRepo = UnitOfWorkAsync.RepositoryAsync<EmployeeTag>();
            _employeeTeamMappingRepo = UnitOfWorkAsync.RepositoryAsync<EmployeeTeamMapping>();
            _recognitionEmployeeTeamMappingRepo = UnitOfWorkAsync.RepositoryAsync<RecognitionEmployeeTeamMapping>();
            _commentDetailsRepo = UnitOfWorkAsync.RepositoryAsync<CommentDetails>();
            _conversationRepo = UnitOfWorkAsync.RepositoryAsync<Conversation>();
        }
        public async Task UserNotificationsAndEmails(List<long> employees, long loginUser, long goalId, int goalType, long conversationId, long goalSourceId,string conversationDescription)
        {
            employees.Add(loginUser);
            var userDetails = _employeeRepo.GetQueryable().Where(x => employees.Contains(x.EmployeeId));

            var keyVault = await _keyVaultService.GetAzureBlobKeysAsync();
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var template = await _commonService.GetMailerTemplate(TemplateCodes.CTAG.ToString());
            var loginUserDetails = userDetails.FirstOrDefault(x => x.EmployeeId == loginUser);

            if (loginUserDetails != null)
            {
                var subject = template.Subject.Replace("Youhave", AppConstants.Youhave).Replace("loginEmp", loginUserDetails.FirstName);
                GoalObjective okr;

                if (goalType == 1)
                    okr = GetGoalObjective(goalId);
                else
                {
                    var kr = GetGoalKeyDetails(goalId);
                    okr = GetGoalObjective(kr.GoalObjectiveId);
                }
                CycleDetailRequest cycleDetailRequest = new CycleDetailRequest();
                if (okr != null)
                {
                    cycleDetailRequest = await GetCycleDetailGoalObject(okr);
                }
                employees.RemoveAll(item => item == loginUser);
                foreach (var emp in employees)
                {
                    var employee = userDetails.FirstOrDefault(x => x.EmployeeId == emp);
                    if (employee != null)
                    {
                        long redirectEmpId = 0;
                        if (goalType == 1)
                        {
                            var isAnyOkr = _goalObjectiveRepo.GetQueryable().FirstOrDefault(x => (x.Source == goalSourceId || x.GoalObjectiveId == goalSourceId) && x.GoalStatusId != (int)KrStatus.Declined && x.EmployeeId == emp && (x.IsActive ?? true));
                            redirectEmpId = (isAnyOkr != null) ? employee.EmployeeId : _goalObjectiveRepo.GetQueryable().FirstOrDefault(x => x.GoalObjectiveId == goalId).EmployeeId;

                        }
                        else
                        {
                            var isAnyOkr = _goalKeyRepo.GetQueryable().FirstOrDefault(x => (x.Source == goalSourceId || x.GoalKeyId == goalSourceId) && x.KrStatusId != (int)KrStatus.Declined && x.EmployeeId == emp && (x.IsActive ?? true));
                            redirectEmpId = (isAnyOkr != null) ? employee.EmployeeId : _goalKeyRepo.GetQueryable().FirstOrDefault(x => x.GoalKeyId == goalId).EmployeeId;

                        }
                        var notificationListTo = new List<long> { emp };
                        var body = template.Body;
                        var loginUrl = settings.FrontEndUrl + "?redirectUrl=conversations/" + conversationId + AppConstants.BackSlash + goalType + AppConstants.BackSlash + goalSourceId + AppConstants.BackSlash + redirectEmpId + "&empId=" + employee.EmployeeId + "&cycleId=" + okr?.ObjectiveCycleId + "&year=" + okr?.Year;
                        body = body.Replace("year", DateTime.Now.Year.ToString())
                            .Replace("topBar", keyVault.BlobCdnCommonUrl + AppConstants.TopBar)
                            .Replace("logo", keyVault.BlobCdnCommonUrl + AppConstants.LogoImages)
                            .Replace("tick", keyVault.BlobCdnCommonUrl + AppConstants.TickImages);
                        body = body.Replace("Username", employee.FirstName).Replace("OKRS", okr.ObjectiveName).Replace("loginEmp", loginUserDetails.FirstName).Replace("srcInstagram", keyVault.BlobCdnCommonUrl + AppConstants.Instagram).Replace("srcLinkedin", keyVault.BlobCdnCommonUrl + AppConstants.Linkedin)
                            .Replace("srcTwitter", keyVault.BlobCdnCommonUrl + AppConstants.Twitter).Replace("srcFacebook", keyVault.BlobCdnCommonUrl + AppConstants.Facebook)
                            .Replace("fb", settings.FacebookUrl).Replace("terp", settings.TwitterUrl).Replace("lk", settings.LinkedInUrl).Replace("ijk", settings.InstagramUrl).Replace("<url>", settings.FrontEndUrl);
                        body = body.Replace("<signIn>", loginUrl).Replace("<cycle>", cycleDetailRequest.Cycle + " " + "cycle" + " " + cycleDetailRequest.SymbolName + " " + cycleDetailRequest.Year);
                        body = body.Replace("jhiyo", conversationDescription);

                        var mailRequest = new MailRequest
                        {
                            MailTo = employee.EmailId,
                            Subject = subject,
                            Body = body
                        };
                        _commonService.SendEmail(mailRequest, settings);
                        var notificationRequest = new NotificationsRequest()
                        {
                            By = loginUser,
                            Text = AppConstants.TagMessage.Replace("<OKR>", okr.ObjectiveName).Replace("<Username>", loginUserDetails.FirstName).Replace("<cycle>", cycleDetailRequest.Cycle + " " + AppConstants.Cycle + " " + cycleDetailRequest.SymbolName + " " + cycleDetailRequest.Year),
                            NotificationType = (int)EnumNotificationType.EmployeeTag,
                            AppId = AppConstants.AppIdForOkrService,
                            MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                            Url = "conversations/" + conversationId + AppConstants.BackSlash + goalType + AppConstants.BackSlash + goalSourceId + AppConstants.BackSlash + redirectEmpId + AppConstants.BackSlash + okr.ObjectiveCycleId,
                            To = null,
                            Actionable = true,
                            NotificationOnTypeId = 1,
                            NotificationOnId = conversationId
                        };
                        notificationRequest.To ??= notificationListTo;
                        _commonService.Notifications(notificationRequest, settings);
                    }
                }

            }
        }

        public async Task UserReplyConversationNotifications(long loginUser,long goalId,long conversationId)
        {
            GoalObjective okr;
            GoalKey kr;
            var loginUserDetails = await _employeeRepo.FirstOrDefaultAsync(x => x.EmployeeId == loginUser);
            var parentConversationDetails = await _conversationRepo.FirstOrDefaultAsync(x => x.ConversationId == goalId && x.IsActive);
            if (parentConversationDetails.GoalTypeId == 1)
                okr = GetGoalObjective(parentConversationDetails.GoalId);
            else
            {
                kr = GetGoalKeyDetails(parentConversationDetails.GoalId);
                okr = GetGoalObjective(kr.GoalObjectiveId);
            }

            string msg = "";
            var ids = new List<long> { parentConversationDetails.ConversationId };
            ids.Add(conversationId);
            var employeeTagConversation = await _employeeTagRepo.GetQueryable().Where(x => ids.Contains(x.ModuleDetailsId) && x.ModuleId == (int)ModuleId.Conversation && x.IsActive && x.TagId != loginUser).Select(x => x.TagId).ToListAsync();
            if (loginUser == parentConversationDetails.CreatedBy)
            {
                msg = AppConstants.AddedConversationTag.Replace("Username", loginUserDetails.FirstName + " " + loginUserDetails.LastName).Replace("title", okr.ObjectiveName);
                employeeTagConversation.Remove(parentConversationDetails.CreatedBy);
                await ConversationReplyNotifications(loginUser, msg, employeeTagConversation.Distinct().ToList(), parentConversationDetails.ConversationId, conversationId, parentConversationDetails.GoalSourceId,parentConversationDetails.GoalTypeId,parentConversationDetails.GoalId,okr).ConfigureAwait(false);

            }
            else if (loginUser != parentConversationDetails.CreatedBy)
            {
                msg = AppConstants.AddedConversationCreator.Replace("Username", loginUserDetails.FirstName + " " + loginUserDetails.LastName).Replace("title", okr.ObjectiveName);
                await ConversationReplyNotifications(loginUser, msg, new List<long> { parentConversationDetails.CreatedBy}, parentConversationDetails.ConversationId, conversationId, parentConversationDetails.GoalSourceId, parentConversationDetails.GoalTypeId, parentConversationDetails.GoalId, okr).ConfigureAwait(false);
                employeeTagConversation.Remove(parentConversationDetails.CreatedBy);
                msg = AppConstants.AddedConversationTag.Replace("Username", loginUserDetails.FirstName + " " + loginUserDetails.LastName).Replace("title", okr.ObjectiveName);
                await ConversationReplyNotifications(loginUser, msg, employeeTagConversation.Distinct().ToList(), parentConversationDetails.ConversationId, conversationId, parentConversationDetails.GoalSourceId, parentConversationDetails.GoalTypeId, parentConversationDetails.GoalId, okr).ConfigureAwait(false);
            }
        }

        public async Task LikeUserConversation (long moduleDetailsId,long employeeId,long loginUser,long likeReactionId)
        {
            GoalObjective okr;
            GoalKey kr;
            var loginUserDetails = await _employeeRepo.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
            var likeReplyDetails = await _conversationRepo.FirstOrDefaultAsync(x => x.ConversationId == moduleDetailsId);
            var likeMainDetails = await _conversationRepo.FirstOrDefaultAsync(x => x.ConversationId == likeReplyDetails.GoalId);
            if (likeMainDetails.GoalTypeId == 1)
                okr = GetGoalObjective(likeMainDetails.GoalId);
            else
            {
                kr = GetGoalKeyDetails(likeMainDetails.GoalId);
                okr = GetGoalObjective(kr.GoalObjectiveId);
            }
            string msg = "";
            var ids = new List<long> { likeMainDetails.ConversationId };
            ids.Add(likeReplyDetails.ConversationId);
            var employeeTagConversation = await _employeeTagRepo.GetQueryable().Where(x => ids.Contains(x.ModuleDetailsId) && x.ModuleId == (int)ModuleId.Conversation && x.IsActive && x.TagId != loginUser).Select(x => x.TagId).ToListAsync();
            if (loginUser == likeMainDetails.CreatedBy)
            {
                msg = AppConstants.ConversationCommentTagLiked.Replace("Username", loginUserDetails.FirstName + " " + loginUserDetails.LastName).Replace("title", okr.ObjectiveName);
                employeeTagConversation.Remove(likeMainDetails.CreatedBy);
                await ConversationLikeReplyNotifications(loginUser, msg, employeeTagConversation.Distinct().ToList(), likeMainDetails.ConversationId, likeReplyDetails.ConversationId, likeReactionId, likeMainDetails.GoalSourceId, likeMainDetails.GoalTypeId, likeMainDetails.GoalId, okr).ConfigureAwait(false);

            }
            else if (loginUser != likeMainDetails.CreatedBy)
            {
                msg = AppConstants.ConversationCommentLiked.Replace("Username", loginUserDetails.FirstName + " " + loginUserDetails.LastName).Replace("title", okr.ObjectiveName);
                await ConversationLikeReplyNotifications(loginUser, msg, new List<long> { likeMainDetails.CreatedBy }, likeMainDetails.ConversationId, likeReplyDetails.ConversationId, likeReactionId, likeMainDetails.GoalSourceId, likeMainDetails.GoalTypeId, likeMainDetails.GoalId, okr).ConfigureAwait(false);
                employeeTagConversation.Remove(likeMainDetails.CreatedBy);
                msg = AppConstants.ConversationCommentTagLiked.Replace("Username", loginUserDetails.FirstName + " " + loginUserDetails.LastName).Replace("title", okr.ObjectiveName);
                await ConversationLikeReplyNotifications(loginUser, msg, employeeTagConversation.Distinct().ToList(), likeMainDetails.ConversationId, likeReplyDetails.ConversationId, likeReactionId, likeMainDetails.GoalSourceId, likeMainDetails.GoalTypeId, likeMainDetails.GoalId, okr).ConfigureAwait(false);
            }

        }

        public async Task LikeUserCommentConversation(long moduleDetailsId, long employeeId, long loginUser, long likeReactionId)
        {
            GoalObjective okr;
            GoalKey kr;
            var loginUserDetails = await _employeeRepo.FirstOrDefaultAsync(x => x.EmployeeId == employeeId);
            var likeMainDetails = await _conversationRepo.FirstOrDefaultAsync(x => x.ConversationId == moduleDetailsId);
            if (likeMainDetails.GoalTypeId == 1)
                okr = GetGoalObjective(likeMainDetails.GoalId);
            else
            {
                kr = GetGoalKeyDetails(likeMainDetails.GoalId);
                okr = GetGoalObjective(kr.GoalObjectiveId);
            }
            string msg = "";
            var employeeTagConversation = await _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == moduleDetailsId && x.ModuleId == (int)ModuleId.Conversation && x.IsActive && x.TagId != loginUser).Select(x => x.TagId).ToListAsync();
            if (loginUser == likeMainDetails.CreatedBy)
            {
                msg = AppConstants.ConversationCommentTagLiked.Replace("Username", loginUserDetails.FirstName + " " + loginUserDetails.LastName).Replace("title", okr.ObjectiveName);
                employeeTagConversation.Remove(likeMainDetails.CreatedBy);
                await ConversationLikeCommentNotifications(loginUser, msg, employeeTagConversation.Distinct().ToList(), moduleDetailsId, likeReactionId, likeMainDetails.GoalSourceId, likeMainDetails.GoalTypeId, likeMainDetails.GoalId, okr).ConfigureAwait(false);

            }
            else if (loginUser != likeMainDetails.CreatedBy)
            {
                msg = AppConstants.ConversationCommentLiked.Replace("Username", loginUserDetails.FirstName + " " + loginUserDetails.LastName).Replace("title", okr.ObjectiveName);
                await ConversationLikeCommentNotifications(loginUser, msg, new List<long> { likeMainDetails.CreatedBy }, moduleDetailsId, likeReactionId, likeMainDetails.GoalSourceId, likeMainDetails.GoalTypeId, likeMainDetails.GoalId, okr).ConfigureAwait(false);
                employeeTagConversation.Remove(likeMainDetails.CreatedBy);
                msg = AppConstants.ConversationCommentTagLiked.Replace("Username", loginUserDetails.FirstName + " " + loginUserDetails.LastName).Replace("title", okr.ObjectiveName);
                await ConversationLikeCommentNotifications(loginUser, msg, employeeTagConversation.Distinct().ToList(), moduleDetailsId, likeReactionId, likeMainDetails.GoalSourceId, likeMainDetails.GoalTypeId, likeMainDetails.GoalId, okr).ConfigureAwait(false);
            }

        }
        public async Task EditReplyConversation (List<long>empIds, long loginUser, long goalId, long conversationId)
        {
            GoalObjective okr;
            GoalKey kr;
            var loginUserDetails = await _employeeRepo.FirstOrDefaultAsync(x => x.EmployeeId == loginUser);
            var parentConversationDetails = await _conversationRepo.FirstOrDefaultAsync(x => x.ConversationId == goalId && x.IsActive);
            if (parentConversationDetails.GoalTypeId == 1)
                okr = GetGoalObjective(parentConversationDetails.GoalId);
            else
            {
                kr = GetGoalKeyDetails(parentConversationDetails.GoalId);
                okr = GetGoalObjective(kr.GoalObjectiveId);
            }

            string msg = "";
            msg = AppConstants.AddedConversationTag.Replace("Username", loginUserDetails.FirstName + " " + loginUserDetails.LastName).Replace("title", okr.ObjectiveName);
            empIds.Remove(parentConversationDetails.CreatedBy);
            empIds.Remove(loginUser);
            await ConversationReplyNotifications(loginUser, msg, empIds.Distinct().ToList(), parentConversationDetails.ConversationId, conversationId, parentConversationDetails.GoalSourceId, parentConversationDetails.GoalTypeId, parentConversationDetails.GoalId, okr).ConfigureAwait(false);

        }
        public async Task CreateRecognitionNotifications(UserIdentity userIdentity, long recognitionId, bool isAttachment, string name, List<long> receiverIds,string fileName, string message)
        {
            var mapping = new List<RecognitionEmployeeTeamMapping>();
            if (receiverIds.Count > 0)
            {
                mapping = _recognitionEmployeeTeamMappingRepo.GetQueryable().Where(x => x.RecognitionId == recognitionId && (receiverIds.Contains(x.EmployeeId) || receiverIds.Contains((long)x.TeamId))).ToList();
            }
            else
            {
                mapping = _recognitionEmployeeTeamMappingRepo.GetQueryable().Where(x => x.RecognitionId == recognitionId).ToList();
            }
            var teamId = mapping.Where(x => x.TeamId > 0).Where(x => x.EmployeeId != userIdentity.EmployeeId).Select(x => x.EmployeeId).Distinct().ToList();
            var empId = mapping.Where(x => x.TeamId == 0).Where(x => x.EmployeeId != userIdentity.EmployeeId).Select(x => x.EmployeeId).Distinct().ToList();
            string msg = "";
            if (teamId.Count > 0 && empId.Count == 0)
            {
                msg = isAttachment ? AppConstants.TeamRecognitionWithBadge.Replace("Givers", userIdentity.FirstName + " " + userIdentity.LastName)
             .Replace("name", name) : AppConstants.TeamRecognition
                .Replace("Givers", userIdentity.FirstName + " " + userIdentity.LastName);
                await CreateRecognitionNotifications(userIdentity.EmployeeId, msg, teamId, recognitionId).ConfigureAwait(false);
                await SendEmailToTeam(teamId, userIdentity, recognitionId, isAttachment, name,fileName,message).ConfigureAwait(false);
            }
            else if (empId.Count > 0 && teamId.Count == 0)
            {
                msg = isAttachment ? AppConstants.NotificationsWithBadge.Replace("Givers", userIdentity.FirstName + " " + userIdentity.LastName)
                .Replace("name", name) : AppConstants.NotificationsWithoutBadge
                .Replace("Givers", userIdentity.FirstName + " " + userIdentity.LastName);
                await CreateRecognitionNotifications(userIdentity.EmployeeId, msg, empId, recognitionId).ConfigureAwait(false);
                await SendEmailCreateRecognition(empId, userIdentity, recognitionId, isAttachment, name, fileName, message).ConfigureAwait(false);
            }
            else if (empId.Count > 0 && teamId.Count > 0)
            {
                var ids = teamId.Intersect(empId).ToList();
                foreach (var item in ids)
                {
                    empId.Remove(item);
                }
                if (empId.Count > 0)
                {
                    msg = isAttachment ? AppConstants.NotificationsWithBadge.Replace("Givers", userIdentity.FirstName + " " + userIdentity.LastName)
                 .Replace("name", name) : AppConstants.NotificationsWithoutBadge
                    .Replace("Givers", userIdentity.FirstName + " " + userIdentity.LastName);
                    await CreateRecognitionNotifications(userIdentity.EmployeeId, msg, empId, recognitionId).ConfigureAwait(false);
                    await SendEmailCreateRecognitionForEmployees(empId, userIdentity, recognitionId, isAttachment, name, fileName, message).ConfigureAwait(false);
                }
                msg = isAttachment ? AppConstants.TeamRecognitionWithBadge.Replace("Givers", userIdentity.FirstName + " " + userIdentity.LastName)
                 .Replace("name", name) : AppConstants.TeamRecognition
                    .Replace("Givers", userIdentity.FirstName + " " + userIdentity.LastName);
                await CreateRecognitionNotifications(userIdentity.EmployeeId, msg, teamId, recognitionId).ConfigureAwait(false);
                await SendEmailToTeam(teamId, userIdentity, recognitionId, isAttachment, name,fileName, message).ConfigureAwait(false);
            }

        }

        public async Task CreateRecognitionNotifications(long loginEmployeeId, string msg, List<long> empId, long recognitionId)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var notificationRequest = new NotificationsRequest()
            {
                By = loginEmployeeId,
                Text = msg,
                NotificationType = (int)EnumNotificationType.Recognition,
                AppId = AppConstants.AppIdForOkrService,
                MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                Url = "recognize/" + recognitionId,
                To = empId,
                Actionable = false,
                NotificationOnTypeId = 1,
                NotificationOnId = recognitionId
            };

            _commonService.Notifications(notificationRequest, settings);
        }

        private async Task SendEmailCreateRecognition(List<long> empIds, UserIdentity userIdentity, long recognitionId, bool isAttachment, string name,string fileName, string message)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var keyVault = await _keyVaultService.GetAzureBlobKeysAsync();
            var template = await _commonService.GetMailerTemplate(TemplateCodes.RW.ToString());
            var empDetails = await _employeeRepo.GetQueryable().Where(x => empIds.Contains(x.EmployeeId)).ToListAsync();
            var body = template.Body;
            var subject = template.Subject;           
            var empId = "fgdg";
            if (isAttachment)
            {
                var htmlReplace = HtmlTag();
                body = body.Replace("asj", "with a  '" + name + "'  badge");
                subject = subject.Replace(".<badge>", " with a badge.");
                body = body.Replace("Givers", "'" + userIdentity.FirstName + " " + userIdentity.LastName + "'.");
                body = body.Replace("<sccsdddddd>", htmlReplace);
                body = body.Replace("gdgs9s", keyVault.BlobCdnUrl + keyVault.BlobContainerName + "/" + AppConstants.RecognitionFolderName + "/" + fileName);
                body = body.Replace("ccbcxsx", name);
            }
            else
            {
                body = body.Replace("asj.", " ");
                subject = subject.Replace("<badge>", " ");
                body = body.Replace("Givers", "'" + userIdentity.FirstName + " " + userIdentity.LastName + "'.");
                body = body.Replace("<sccsdddddd>", " ");
            }

            body = body.Replace("year", DateTime.Now.Year.ToString())
                       .Replace("topBar", keyVault.BlobCdnCommonUrl + AppConstants.TopBar)
                       .Replace("logo", keyVault.BlobCdnCommonUrl + AppConstants.LogoImages)
                       .Replace("tick", keyVault.BlobCdnCommonUrl + AppConstants.TickImages)
                      .Replace("srcInstagram", keyVault.BlobCdnCommonUrl + AppConstants.Instagram).Replace("srcLinkedin", keyVault.BlobCdnCommonUrl + AppConstants.Linkedin)
             .Replace("srcTwitter", keyVault.BlobCdnCommonUrl + AppConstants.Twitter).Replace("srcFacebook", keyVault.BlobCdnCommonUrl + AppConstants.Facebook)
             .Replace("fb", settings.FacebookUrl).Replace("terp", settings.TwitterUrl).Replace("lk", settings.LinkedInUrl).Replace("ijk", settings.InstagramUrl)
             .Replace("<url>", settings.FrontEndUrl).Replace("<sddhdh>", settings.FrontEndUrl + "?redirectUrl=recognize/" + recognitionId + "&empId=" + empId)
             .Replace("hjgjr", message);
            List<EmployeeResponse> employees = new List<EmployeeResponse>();
            foreach (var (item, employeeResponse) in from item in empDetails
                                                     let employeeResponse = new EmployeeResponse()
                                                     select (item, employeeResponse))
            {
                employeeResponse.EmailId = item.EmailId;
                employeeResponse.EmpId = item.EmployeeId;
                employeeResponse.Name = item.FirstName + " " + item.LastName;
                employees.Add(employeeResponse);
            }
            var teamMailRequest = new TeamMailRequest
            {
                Employees = employees,
                MailTo = "",
                Subject = subject,
                Body = body,
                MailBody = body
            };
            _commonService.SendBulkEmail(teamMailRequest, settings);
           
        }

        private async Task SendEmailToTeam(List<long> empIds, UserIdentity userIdentity, long recognitionId, bool isAttachment, string name, string fileName,string message)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var keyVault = await _keyVaultService.GetAzureBlobKeysAsync();
            var template = await _commonService.GetMailerTemplate(TemplateCodes.RWT.ToString());
            var empDetails = await _employeeRepo.GetQueryable().Where(x => empIds.Contains(x.EmployeeId)).ToListAsync();
            var body = template.Body;
            var subject = template.Subject;
            var empId = "fgdg";
            if (isAttachment)
            {
                var htmlReplace = HtmlTag();
                body = body.Replace("asj", "with a  '" + name + "'  badge");
                subject = subject.Replace(".<badge>", " with a badge.");
                body = body.Replace("Givers", "'" + userIdentity.FirstName + " " + userIdentity.LastName + "'.");
                body = body.Replace("<sccsdddddd>", htmlReplace);
                body = body.Replace("gdgs9s", keyVault.BlobCdnUrl + keyVault.BlobContainerName + "/" + AppConstants.RecognitionFolderName + "/" + fileName);
                body = body.Replace("ccbcxsx", name);

            }
            else
            {
                body = body.Replace("asj.", " ");
                subject = subject.Replace("<badge>", " ");
                body = body.Replace("Givers", "'" + userIdentity.FirstName + " " + userIdentity.LastName + "'.");
                body = body.Replace("<sccsdddddd>", " ");
            }

            body = body.Replace("year", DateTime.Now.Year.ToString())
                       .Replace("topBar", keyVault.BlobCdnCommonUrl + AppConstants.TopBar)
                       .Replace("logo", keyVault.BlobCdnCommonUrl + AppConstants.LogoImages)
                       .Replace("tick", keyVault.BlobCdnCommonUrl + AppConstants.TickImages)
                      .Replace("srcInstagram", keyVault.BlobCdnCommonUrl + AppConstants.Instagram).Replace("srcLinkedin", keyVault.BlobCdnCommonUrl + AppConstants.Linkedin)
             .Replace("srcTwitter", keyVault.BlobCdnCommonUrl + AppConstants.Twitter).Replace("srcFacebook", keyVault.BlobCdnCommonUrl + AppConstants.Facebook)
             .Replace("fb", settings.FacebookUrl).Replace("terp", settings.TwitterUrl).Replace("lk", settings.LinkedInUrl).Replace("ijk", settings.InstagramUrl)
             .Replace("<url>", settings.FrontEndUrl).Replace("<sddhdh>", settings.FrontEndUrl + "?redirectUrl=recognize/" + recognitionId + "&empId=" + empId)
             .Replace("hjgjr", message);
            List<EmployeeResponse> employees = new List<EmployeeResponse>();
            foreach (var (item, employeeResponse) in from item in empDetails
                                                     let employeeResponse = new EmployeeResponse()
                                                     select (item, employeeResponse))
            {
                employeeResponse.EmailId = item.EmailId;
                employeeResponse.EmpId = item.EmployeeId;
                employeeResponse.Name = item.FirstName + " " + item.LastName;
                employees.Add(employeeResponse);
            }
            var teamMailRequest = new TeamMailRequest
            {
                Employees = employees,
                MailTo = "",
                Subject = subject,
                Body = body,
                MailBody = body
            };
            _commonService.SendBulkEmail(teamMailRequest, settings);
        }

        public async Task UpdateNotificationsBadges(List<long> receiverId, UserIdentity userIdentity, long recognitionId)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            foreach(var item in receiverId)
            {
                if (item == userIdentity.EmployeeId)
                {
                    receiverId.Remove(item);
                }
            }
            var notificationRequest = new NotificationsRequest()
            {
                By = userIdentity.EmployeeId,
                Text = AppConstants.UpdateNotifications.Replace("Givers", userIdentity.FirstName + " " + userIdentity.LastName),
                NotificationType = (int)EnumNotificationType.Recognition,
                AppId = AppConstants.AppIdForOkrService,
                MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                Url = "recognize/" + recognitionId,
                To = receiverId.Distinct().ToList(),
                Actionable = true,
                NotificationOnTypeId = 1,
                NotificationOnId = recognitionId
            };

            _commonService.Notifications(notificationRequest, settings);
        }
        public async Task LikeNotifications(long recognitionId, long empId, long loginUserId, long likeId)
        {
            var recognition = await _recognitionRepo.FirstOrDefaultAsync(x => x.RecognitionId == recognitionId && x.IsActive);
            var recognitionMapping = await _recognitionEmployeeTeamMappingRepo.GetQueryable().Where(x => x.RecognitionId == recognitionId && x.IsActive && x.EmployeeId != loginUserId).ToListAsync();
            var empDetails = await _employeeRepo.FirstOrDefaultAsync(x => x.EmployeeId == empId && x.IsActive);
            var postTag = await _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == recognitionId && (x.ModuleId == (int)ModuleId.Recognisation || x.ModuleId == (int)ModuleId.TeamTagInRecognisationInBody) && x.IsActive).ToListAsync();
            if (recognition != null)
            {
                if (recognition.CreatedBy == empId)
                {
                    var receiverEmpId = new List<long>();
                    receiverEmpId.AddRange(recognitionMapping.Select(x => x.EmployeeId));
                    receiverEmpId.Remove(empId);
                    var response = await SendLikeNotifications(receiverEmpId, loginUserId, empDetails, likeId, recognitionId, recognitionMapping, 0).ConfigureAwait(false);
                    if (postTag.Count > 0)
                        await TeamPostTagNotifications(response, postTag, empDetails, likeId, recognitionId).ConfigureAwait(false);
                    await SignalRLikeNotifications(receiverEmpId).ConfigureAwait(false);
                }
                else if ((recognitionMapping.FirstOrDefault(x => x.EmployeeId == empId) == null ? 0 : recognitionMapping.FirstOrDefault(x => x.EmployeeId == empId).EmployeeId) == empId)
                {
                    var createdEmpId = recognitionMapping.Select(x => x.EmployeeId).ToList();
                    createdEmpId.Remove(empId);
                    var response = await SendLikeNotifications(createdEmpId, loginUserId, empDetails, likeId, recognitionId, recognitionMapping, recognition.CreatedBy).ConfigureAwait(false);
                    if (postTag.Count > 0)
                        await TeamPostTagNotifications(response, postTag, empDetails, likeId, recognitionId).ConfigureAwait(false);
                    await SignalRLikeNotifications(createdEmpId).ConfigureAwait(false);
                }
                else if ((recognitionMapping.FirstOrDefault(x => x.EmployeeId == empId) == null ? 0 : recognitionMapping.FirstOrDefault(x => x.EmployeeId == empId).EmployeeId) != empId && recognition.CreatedBy != empId)
                {
                    var emDetails = recognitionMapping.Select(x => x.EmployeeId).ToList();
                    var response = await SendLikeNotifications(emDetails, loginUserId, empDetails, likeId, recognitionId, recognitionMapping, recognition.CreatedBy).ConfigureAwait(false);
                    if (postTag.Count > 0)
                        await TeamPostTagNotifications(response, postTag, empDetails, likeId, recognitionId).ConfigureAwait(false);
                    await SignalRLikeNotifications(emDetails).ConfigureAwait(false);
                }

            }
        }
        private async Task<CommentLikeResponse> SendLikeNotifications(List<long> notificationTo, long loginUserId, Employee empDetails, long likeId, long recognitionId, List<RecognitionEmployeeTeamMapping> mapping, long recognitionCreatedBy)
        {
            var teamId = mapping.Where(x => x.TeamId > 0 && notificationTo.Contains(x.EmployeeId)).Select(x => x.EmployeeId).Distinct().ToList();
            var empId = mapping.Where(x => x.TeamId == 0 && notificationTo.Contains(x.EmployeeId)).Select(x => x.EmployeeId).Distinct().ToList();
            string msg = "";
            empId.Add(recognitionCreatedBy);
            CommentLikeResponse commentLikeResponse = new CommentLikeResponse();
            if (teamId.Count > 0 && empId.Count == 0)
            {
                msg = AppConstants.TeamLikeMessage
                .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                await TeamLikeNotifications(recognitionId, likeId, loginUserId, msg, teamId).ConfigureAwait(false);
                commentLikeResponse.TeamId = teamId;
            }
            else if (empId.Count > 0 && teamId.Count == 0)
            {
                msg = AppConstants.LikeMessage
                .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                await TeamLikeNotifications(recognitionId, likeId, loginUserId, msg, empId.Distinct().ToList()).ConfigureAwait(false);
                commentLikeResponse.EmpIds = empId;
            }
            else if (empId.Count > 0 && teamId.Count > 0)
            {
                var ids = teamId.Intersect(empId).ToList();
                foreach (var item in ids)
                {
                    empId.Remove(item);
                }
                if (empId.Count > 0)
                {
                    msg = AppConstants.LikeMessage
                    .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                    await TeamLikeNotifications(recognitionId, likeId, loginUserId, msg, empId.Distinct().ToList()).ConfigureAwait(false);
                    commentLikeResponse.EmpIds = empId;
                }
                msg = AppConstants.TeamLikeMessage
                    .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                await TeamLikeNotifications(recognitionId, likeId, loginUserId, msg, teamId).ConfigureAwait(false);
                commentLikeResponse.TeamId = teamId;
            }
            return commentLikeResponse;
        }

        public async Task TeamPostTagNotifications(CommentLikeResponse commentLike, List<EmployeeTag> postTag, Employee userIdentity, long likeId, long recognitionId)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var likeTeam = commentLike.TeamId;
            var likeEmpId = commentLike.EmpIds;
            var employeeTag = postTag.Where(x => x.ModuleId == (int)ModuleId.TeamTagInRecognisationInBody).Select(x => x.TagId).Distinct();
            var teamEmpIds = _employeeTeamMappingRepo.GetQueryable().Where(x => employeeTag.Contains(x.TeamId) && x.IsActive).Select(x => x.EmployeeId).Distinct().ToList();
            var deleteTeamEmployees = likeTeam.Intersect(teamEmpIds).ToList();
            foreach (var item in deleteTeamEmployees)
            {
                teamEmpIds.Remove(item);
            }
            var emp = postTag.Where(x => x.ModuleId == (int)ModuleId.Recognisation).Select(x => x.TagId).Distinct().ToList();
            var deleteEmployeeIds = emp.Intersect(likeEmpId).ToList();
            foreach (var item in deleteEmployeeIds)
            {
                emp.Remove(item);
            }
            var teamEmployeeIds = emp.Intersect(likeTeam).ToList();
            foreach (var item in teamEmployeeIds)
            {
                emp.Remove(item);
            }
            var loginempIds = emp.Intersect(new List<long> { userIdentity.EmployeeId }).ToList();
            foreach(var item in loginempIds)
            {
                emp.Remove(item);
            }
            if (emp.Count > 0)
            {
                var tagLikeNotificationRequest = new NotificationsRequest()
                {
                    By = userIdentity.EmployeeId,
                    Text = AppConstants.TagLikeMessage.Replace("UserName", userIdentity.FirstName + " " + userIdentity.LastName),
                    NotificationType = (int)EnumNotificationType.RecognitionLike,
                    AppId = AppConstants.AppIdForOkrService,
                    MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                    Url = "recognize/" + recognitionId,
                    To = emp,
                    Actionable = true,
                    NotificationOnTypeId = 1,
                    NotificationOnId = likeId
                };
                _commonService.Notifications(tagLikeNotificationRequest, settings);
            }

            if (teamEmpIds.Count > 0)
            {
                var TeamtagLikeNotificationRequest = new NotificationsRequest()
                {
                    By = userIdentity.EmployeeId,
                    Text = AppConstants.TeamTagLikeMessage.Replace("UserName", userIdentity.FirstName + " " + userIdentity.LastName),
                    NotificationType = (int)EnumNotificationType.RecognitionLike,
                    AppId = AppConstants.AppIdForOkrService,
                    MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                    Url = "recognize/" + recognitionId,
                    To = teamEmpIds,
                    Actionable = true,
                    NotificationOnTypeId = 1,
                    NotificationOnId = likeId
                };
                _commonService.Notifications(TeamtagLikeNotificationRequest, settings);
            }

        }

        public async Task TeamLikeNotifications(long recognitionId, long likedId, long loginUserId, string msg, List<long> notificationTo)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var notificationRequest = new NotificationsRequest()
            {
                By = loginUserId,
                Text = msg,
                NotificationType = (int)EnumNotificationType.RecognitionLike,
                AppId = AppConstants.AppIdForOkrService,
                MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                Url = "recognize/" + recognitionId,
                To = notificationTo,
                Actionable = true,
                NotificationOnTypeId = 1,
                NotificationOnId = likedId
            };
            _commonService.Notifications(notificationRequest, settings);
        }
        public async Task<List<long>> CommentNotifications(long recognitionId, long createdBy, long commentDetailsId, long commmentId)
        {
            var empIds = new List<long>();
            var recognition = await _recognitionRepo.FirstOrDefaultAsync(x => x.RecognitionId == recognitionId && x.IsActive);
            var recognitionMapping = await _recognitionEmployeeTeamMappingRepo.GetQueryable().Where(x => x.RecognitionId == recognitionId && x.IsActive).ToListAsync();
            var empDetails = await _employeeRepo.FirstOrDefaultAsync(x => x.EmployeeId == createdBy && x.IsActive);
            var postTag = await _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == recognitionId && (x.ModuleId == (int)ModuleId.Recognisation || x.ModuleId == (int)ModuleId.TeamTagInRecognisationInBody) && x.IsActive).ToListAsync();
            if (recognition != null)
            {
                if (recognition.CreatedBy == createdBy)
                {
                    var receiverEmpId = new List<long>();
                    var listEmpId = recognitionMapping.Where(x => x.EmployeeId != createdBy).Select(x => x.EmployeeId).ToList();
                    receiverEmpId.AddRange(listEmpId);
                    var response = await TeamCommentNotifications(receiverEmpId.Distinct().ToList(), createdBy, empDetails, commmentId, recognitionId, recognitionMapping, commentDetailsId, 0).ConfigureAwait(false);
                    if (postTag.Count > 0)
                    {
                        var postResponse = await TeamCommentTagNotifications(response, postTag, empDetails, commmentId, recognitionId).ConfigureAwait(false);
                        empIds.AddRange(postResponse);
                    }
                    empIds.AddRange(response.EmpIds);
                    empIds.AddRange(response.TeamId);
                    await _commonService.CallSignalRForEditRecognition(receiverEmpId).ConfigureAwait(false);
                }
                else if ((recognitionMapping.FirstOrDefault(x => x.EmployeeId == createdBy) == null ? 0 : recognitionMapping.FirstOrDefault(x => x.EmployeeId == createdBy).EmployeeId) == createdBy)
                {
                    var createdEmpId = recognitionMapping.Where(x => x.EmployeeId != createdBy).Select(x => x.EmployeeId).ToList();
                    var response = await TeamCommentNotifications(createdEmpId.Distinct().ToList(), createdBy, empDetails, commmentId, recognitionId, recognitionMapping, commentDetailsId, recognition.CreatedBy).ConfigureAwait(false);
                    if (postTag.Count > 0)
                    {
                        var postResponse = await TeamCommentTagNotifications(response, postTag, empDetails, commmentId, recognitionId).ConfigureAwait(false);
                        empIds.AddRange(postResponse);
                    }
                    empIds.AddRange(response.EmpIds);
                    empIds.AddRange(response.TeamId);
                    await _commonService.CallSignalRForEditRecognition(createdEmpId).ConfigureAwait(false);

                }
                else if ((recognitionMapping.FirstOrDefault(x => x.EmployeeId == createdBy) == null ? 0 : recognitionMapping.FirstOrDefault(x => x.EmployeeId == createdBy).EmployeeId) != createdBy && recognition.CreatedBy != createdBy)
                {
                    var emDetails = recognitionMapping.Where(x => x.EmployeeId != createdBy).Select(x => x.EmployeeId).ToList();
                    var response = await TeamCommentNotifications(emDetails.Distinct().ToList(), createdBy, empDetails, commmentId, recognitionId, recognitionMapping, commentDetailsId, recognition.CreatedBy).ConfigureAwait(false);
                    if (postTag.Count > 0)
                    {
                        var postResponse = await TeamCommentTagNotifications(response, postTag, empDetails, commmentId, recognitionId).ConfigureAwait(false);
                        empIds.AddRange(postResponse);
                    }
                    empIds.AddRange(response.EmpIds);
                    empIds.AddRange(response.TeamId);
                    await _commonService.CallSignalRForEditRecognition(emDetails).ConfigureAwait(false);
                }
            }
            return empIds;
        }


        public async Task CommentRepliesNotifications(long moduleDetailsId, long createdBy, long commentDetailsId)
        {
            string msg = "";
            var creatorComment = await _commentDetailsRepo.FirstOrDefaultAsync(x => x.CommentDetailsId == moduleDetailsId);
            var creatorRecognition = await _recognitionRepo.FirstOrDefaultAsync(x => x.RecognitionId == creatorComment.ModuleDetailsId);
            var receiverRecognition = await _recognitionEmployeeTeamMappingRepo.GetQueryable().Where(x => x.RecognitionId == creatorRecognition.RecognitionId && x.IsActive && x.EmployeeId != createdBy).Select(x => x.EmployeeId).ToListAsync();
            var replyTag = await _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == commentDetailsId).ToListAsync();
            var replyEmpTag = replyTag.Where(x => x.ModuleId == (int)ModuleId.ReplyComments && x.TagId != createdBy).Select(x => x.TagId).ToList();
            var replyTeamTag = replyTag.Where(x => x.ModuleId == (int)ModuleId.ReplyTeamComments).Select(x => x.TagId).ToList();
            var employeeTeamEmpIds = await _employeeTeamMappingRepo.GetQueryable().Where(x => replyTeamTag.Contains(x.TeamId) && x.EmployeeId != createdBy).Select(x => x.EmployeeId).ToListAsync();
            var empDetails = await _employeeRepo.FirstOrDefaultAsync(x => x.EmployeeId == createdBy);
            replyEmpTag.AddRange(employeeTeamEmpIds);
            receiverRecognition.Add(creatorRecognition.CreatedBy);
            if (creatorComment.CreatedBy == createdBy)
            {
                msg = AppConstants.AddedCommentTagReplies
                 .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                await CreatorReplyComment(replyEmpTag.Distinct().ToList(), commentDetailsId, createdBy, msg, creatorRecognition.RecognitionId, creatorComment.CommentDetailsId).ConfigureAwait(false);
                receiverRecognition.Remove(createdBy);
                var finalist = replyEmpTag.Intersect(receiverRecognition).ToList();
                foreach (var item in finalist)
                {
                    receiverRecognition.Remove(item);
                }
                msg = AppConstants.AddedCommentRecognitionReceiverAndCreator
               .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                await CreatorReplyComment(receiverRecognition.Distinct().ToList(), commentDetailsId, createdBy, msg, creatorRecognition.RecognitionId, creatorComment.CommentDetailsId).ConfigureAwait(false);
                await _commonService.CallSignalRForEditRecognition(receiverRecognition.Distinct().ToList()).ConfigureAwait(false);
                await _commonService.CallSignalRForEditRecognition(replyEmpTag.Distinct().ToList()).ConfigureAwait(false);
            }
            else if (creatorComment.CreatedBy != createdBy)
            {
                msg = AppConstants.AddedCommentReplies
               .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                await CreatorReplyComment(new List<long> { creatorComment.CreatedBy }, commentDetailsId, createdBy, msg, creatorRecognition.RecognitionId, creatorComment.CommentDetailsId).ConfigureAwait(false);
                replyEmpTag.Remove(creatorComment.CreatedBy);
                var itemList = receiverRecognition.Intersect(new List<long> { creatorComment.CreatedBy }).ToList();
                foreach (var item1 in itemList)
                {
                    receiverRecognition.Remove(item1);
                    receiverRecognition.Remove(item1);
                }
                if (replyEmpTag.Count > 0)
                {
                    msg = AppConstants.AddedCommentTagReplies
                            .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                    await CreatorReplyComment(replyEmpTag.Distinct().ToList(), commentDetailsId, createdBy, msg, creatorRecognition.RecognitionId, creatorComment.CommentDetailsId).ConfigureAwait(false);
                    await _commonService.CallSignalRForEditRecognition(replyEmpTag.Distinct().ToList()).ConfigureAwait(false);
                }
                var finalist = replyEmpTag.Intersect(receiverRecognition).ToList();
                foreach (var item in finalist)
                {
                    receiverRecognition.Remove(item);
                }
                if (receiverRecognition.Count > 0)
                {
                    msg = AppConstants.AddedCommentRecognitionReceiverAndCreator
                    .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                    await CreatorReplyComment(receiverRecognition.Distinct().ToList(), commentDetailsId, createdBy, msg, creatorRecognition.RecognitionId, creatorComment.CommentDetailsId).ConfigureAwait(false);
                    await _commonService.CallSignalRForEditRecognition(receiverRecognition.Distinct().ToList()).ConfigureAwait(false);
                }
                await _commonService.CallSignalRForEditRecognition(new List<long> { creatorComment.CreatedBy }).ConfigureAwait(false);
            }

        }
        public async Task<List<long>> TeamCommentTagNotifications(CommentLikeResponse commentLike, List<EmployeeTag> postTag, Employee userIdentity, long commmentId, long recognitionId)
        {
            var empId = new List<long>();
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var likeTeam = commentLike.TeamId;
            var likeEmpId = commentLike.EmpIds;
            var employeeTag = postTag.Where(x => x.ModuleId == (int)ModuleId.TeamTagInRecognisationInBody).Select(x => x.TagId).Distinct();
            var teamEmpIds = _employeeTeamMappingRepo.GetQueryable().Where(x => employeeTag.Contains(x.TeamId) && x.IsActive && x.EmployeeId != userIdentity.EmployeeId).Select(x => x.EmployeeId).Distinct().ToList();
            var deleteTeamEmployees = likeTeam.Intersect(teamEmpIds).ToList();
            foreach (var item in deleteTeamEmployees)
            {
                teamEmpIds.Remove(item);
            }

            var emp = postTag.Where(x => x.ModuleId == (int)ModuleId.Recognisation && x.TagId != userIdentity.EmployeeId).Select(x => x.TagId).Distinct().ToList();

            var deleteEmployeeIds = emp.Intersect(likeEmpId).ToList();
            foreach (var item in deleteEmployeeIds)
            {
                emp.Remove(item);
            }
            var teamEmployeeIds = emp.Intersect(likeTeam).ToList();
            foreach (var item in teamEmployeeIds)
            {
                emp.Remove(item);
            }

            if (emp.Count > 0)
            {
                var tagLikeNotificationRequest = new NotificationsRequest()
                {
                    By = userIdentity.EmployeeId,
                    Text = AppConstants.TagCommentMessage.Replace("UserName", userIdentity.FirstName + " " + userIdentity.LastName),
                    NotificationType = (int)EnumNotificationType.RecognitionLike,
                    AppId = AppConstants.AppIdForOkrService,
                    MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                    Url = "recognize/" + recognitionId + "/" + commmentId,
                    To = emp,
                    Actionable = true,
                    NotificationOnTypeId = 1,
                    NotificationOnId = commmentId
                };
                _commonService.Notifications(tagLikeNotificationRequest, settings);
                empId.AddRange(emp);
            }


            if (teamEmpIds.Count > 0)
            {
                var TeamtagLikeNotificationRequest = new NotificationsRequest()
                {
                    By = userIdentity.EmployeeId,
                    Text = AppConstants.TeamTagCommentMessage.Replace("UserName", userIdentity.FirstName + " " + userIdentity.LastName),
                    NotificationType = (int)EnumNotificationType.RecognitionComment,
                    AppId = AppConstants.AppIdForOkrService,
                    MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                    Url = "recognize/" + recognitionId + "/" + commmentId,
                    To = teamEmpIds,
                    Actionable = true,
                    NotificationOnTypeId = 1,
                    NotificationOnId = commmentId
                };
                _commonService.Notifications(TeamtagLikeNotificationRequest, settings);
                empId.AddRange(teamEmpIds);
            }
            return empId;
        }


        private async Task SendCommentNotifications(List<long> notificationTo, long createdBy, string msg, long commmentId, long recognitionId)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var notificationRequest = new NotificationsRequest()
            {
                By = createdBy,
                Text = msg,
                NotificationType = (int)EnumNotificationType.RecognitionComment,
                AppId = AppConstants.AppIdForOkrService,
                MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                Url = "recognize/" + recognitionId + "/" + commmentId,
                To = notificationTo,
                Actionable = true,
                NotificationOnTypeId = 1,
                NotificationOnId = commmentId
            };
            _commonService.Notifications(notificationRequest, settings);
        }



        public async Task LikeTagNotifications(List<long> postTag, long loginUserId, Employee empDetails, ServiceSettingUrlResponse settings, long likeId, long recognitionId)
        {
            var tagLikeNotificationRequest = new NotificationsRequest()
            {
                By = loginUserId,
                Text = AppConstants.TagLikeMessage.Replace("UserName", empDetails.FirstName + " " + empDetails.LastName),
                NotificationType = (int)EnumNotificationType.RecognitionLike,
                AppId = AppConstants.AppIdForOkrService,
                MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                Url = "recognize/" + recognitionId,
                To = postTag,
                Actionable = true,
                NotificationOnTypeId = 1,
                NotificationOnId = likeId
            };
            _commonService.Notifications(tagLikeNotificationRequest, settings);
        }

        public async Task CommentPostNotifications(List<long> postTag, long loginUserId, UserIdentity empDetails, long commmentId, long recognitionId)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var tagLikeNotificationRequest = new NotificationsRequest()
            {
                By = loginUserId,
                Text = AppConstants.TagCommentMessage.Replace("UserName", empDetails.FirstName + " " + empDetails.LastName),
                NotificationType = (int)EnumNotificationType.RecognitionComment,
                AppId = AppConstants.AppIdForOkrService,
                MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                Url = "recognize/" + recognitionId + "/" + commmentId,
                To = postTag,
                Actionable = true,
            };
            _commonService.Notifications(tagLikeNotificationRequest, settings);
        }
        public async Task TagPostNotifications(UserIdentity userIdentity, long recognitionId)
        {
            var receiverEmpId = new List<long>();
            var tagEmpId = new List<long>();
            var finalEmpId = new List<long>();
            var mapping = _recognitionEmployeeTeamMappingRepo.GetQueryable().Where(x => x.RecognitionId == recognitionId).ToList();
            receiverEmpId.AddRange(mapping.Select(x => x.EmployeeId));
            var employeeTags = _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == recognitionId).ToList();
            var individualTag = employeeTags.Where(x => x.ModuleId == (int)ModuleId.Recognisation).Select(x => x.TagId).ToList();
            var teamTag = employeeTags.Where(x => x.ModuleId == (int)ModuleId.TeamTagInRecognisationInBody).ToList();
            var employeeMapping = _employeeTeamMappingRepo.GetQueryable().Where(x => teamTag.Select(x => x.TagId).Contains(x.TeamId) && x.IsActive).Select(x => x.EmployeeId).ToList();
            tagEmpId.AddRange(employeeMapping);
            tagEmpId.AddRange(individualTag);
            var ids = receiverEmpId.Intersect(tagEmpId);
            finalEmpId.AddRange(tagEmpId);
            finalEmpId.Remove(userIdentity.EmployeeId);
            foreach (var item in ids)
            {
                finalEmpId.Remove(item);
            }
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var notificationRequest = new NotificationsRequest()
            {
                By = userIdentity.EmployeeId,
                Text = AppConstants.TagPost.Replace("UserName", userIdentity.FirstName + " " + userIdentity.LastName),
                NotificationType = (int)EnumNotificationType.TagPost,
                AppId = AppConstants.AppIdForOkrService,
                MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                Url = "recognize/" + recognitionId,
                To = finalEmpId,
                Actionable = true,
                NotificationOnTypeId = 1,
                NotificationOnId = recognitionId

            };
            _commonService.Notifications(notificationRequest, settings);
        }

        public async Task<List<long>> UpateEmployeeTag(List<RecognitionEmployeeTags> employeeTags, UserIdentity userIdentity, long recognitionId)
        {
            var empIds = new List<long>();
            var empTag = employeeTags.Where(x => x.SearchType == 1).Select(x => x.Id).ToList();
            var teamTag = employeeTags.Where(x => x.SearchType == 2).Select(x => x.Id).ToList();
            if (teamTag.Count > 0)
            {
                var employeeMapping = _employeeTeamMappingRepo.GetQueryable().Where(x => teamTag.Contains(x.TeamId) && x.IsActive).Select(x => x.EmployeeId).ToList();
                empIds.AddRange(employeeMapping);
            }
            if (empTag.Count > 0)
                empIds.AddRange(empTag);

            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var notificationRequest = new NotificationsRequest()
            {
                By = userIdentity.EmployeeId,
                Text = AppConstants.TagPost.Replace("UserName", userIdentity.FirstName + " " + userIdentity.LastName),
                NotificationType = (int)EnumNotificationType.TagPost,
                AppId = AppConstants.AppIdForOkrService,
                MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                Url = "recognize/" + recognitionId,
                To = empIds.Distinct().ToList(),
                Actionable = true,
                NotificationOnTypeId = 1,
                NotificationOnId = recognitionId

            };
            _commonService.Notifications(notificationRequest, settings);
            return empIds;

        }

        public async Task<List<long>> UpateFinalEmployeeTag(List<long> employeeTags, UserIdentity userIdentity, long recognitionId)
        {
            var empIds = new List<long>();
            var receive = _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == recognitionId && employeeTags.Contains(x.TagId) && (x.ModuleId == (int)ModuleId.Recognisation || x.ModuleId == (int)ModuleId.TeamTagInRecognisationInBody)).ToList();
            var empId = receive.Where(x => x.ModuleId == (int)ModuleId.Recognisation);
            var teamId = receive.Where(x => x.ModuleId == (int)ModuleId.TeamTagInRecognisationInBody);
            var employeeMapping = _employeeTeamMappingRepo.GetQueryable().Where(x => teamId.Select(x => x.TagId).Contains(x.TeamId) && x.IsActive).Select(x => x.EmployeeId).ToList();
            empIds.AddRange(empId.Select(x => x.TagId));
            empIds.AddRange(employeeMapping);
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var notificationRequest = new NotificationsRequest()
            {
                By = userIdentity.EmployeeId,
                Text = AppConstants.TagPost.Replace("UserName", userIdentity.FirstName + " " + userIdentity.LastName),
                NotificationType = (int)EnumNotificationType.TagPost,
                AppId = AppConstants.AppIdForOkrService,
                MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                Url = "recognize/" + recognitionId,
                To = empIds.Distinct().ToList(),
                Actionable = true,
                NotificationOnTypeId = 1,
                NotificationOnId = recognitionId

            };
            _commonService.Notifications(notificationRequest, settings);
            return empIds;

        }


        public async Task<List<long>> TagCommentNotifications(long recognitionId, List<RecognitionEmployeeTags> employeeTags, UserIdentity userIdentity, long commentId, List<long> removeEmpIds)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var empIds = new List<long>();
            var teamEmpIds = new List<long>();
            var finalEmpIds = new List<long>();
            var empTag = employeeTags.Where(x => x.SearchType == 1).Select(x => x.Id).ToList();
            var teamTag = employeeTags.Where(x => x.SearchType == 2).Select(x => x.Id).ToList();
            if (teamTag.Count > 0)
            {
                var employeeMapping = _employeeTeamMappingRepo.GetQueryable().Where(x => teamTag.Contains(x.TeamId) && x.IsActive).Select(x => x.EmployeeId).ToList();
                var finalList = employeeMapping.Intersect(removeEmpIds).ToList();
                teamEmpIds.AddRange(employeeMapping);
                foreach (var item in finalList)
                {
                    teamEmpIds.Remove(item);
                }
                var notificationRequest = new NotificationsRequest()
                {
                    By = userIdentity.EmployeeId,
                    Text = AppConstants.TeamCommentTag.Replace("UserName", userIdentity.FirstName + " " + userIdentity.LastName),
                    NotificationType = (int)EnumNotificationType.TagComment,
                    AppId = AppConstants.AppIdForOkrService,
                    MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                    Url = "recognize/" + recognitionId + "/" + commentId,
                    To = teamEmpIds.Distinct().ToList(),
                    Actionable = true,
                    NotificationOnTypeId = 1,
                    NotificationOnId = commentId
                };
                _commonService.Notifications(notificationRequest, settings);
                finalEmpIds.AddRange(teamEmpIds);

            }
            if (empTag.Count > 0)
            {
                var finalList = empTag.Intersect(removeEmpIds).ToList();
                empIds.AddRange(empTag);
                foreach (var item in finalList)
                {
                    empIds.Remove(item);
                }

                var listRemove = teamEmpIds.Intersect(empIds).ToList();
                foreach (var item in listRemove)
                {
                    empIds.Remove(item);
                }

                var notificationRequest = new NotificationsRequest()
                {
                    By = userIdentity.EmployeeId,
                    Text = AppConstants.TagComment.Replace("UserName", userIdentity.FirstName + " " + userIdentity.LastName),
                    NotificationType = (int)EnumNotificationType.TagComment,
                    AppId = AppConstants.AppIdForOkrService,
                    MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                    Url = "recognize/" + recognitionId + "/" + commentId,
                    To = empIds.Distinct().ToList(),
                    Actionable = true,
                    NotificationOnTypeId = 1,
                    NotificationOnId = commentId
                };
                _commonService.Notifications(notificationRequest, settings);
                finalEmpIds.AddRange(empIds);

            }
            return finalEmpIds;
        }



        public async Task<List<long>> TagFinalCommentNotifications(long recognitionId, List<long> empFinalList, UserIdentity userIdentity, long commentId, List<long> removeEmpIds)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var receive = _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == recognitionId && empFinalList.Contains(x.TagId) && (x.ModuleId == (int)ModuleId.Comments || x.ModuleId == (int)ModuleId.TeamTagInComment)).ToList();
            var empIds = new List<long>();
            var teamEmpIds = new List<long>();
            var finalEmpIds = new List<long>();
            var empId = receive.Where(x => x.ModuleId == (int)ModuleId.Comments);
            var teamId = receive.Where(x => x.ModuleId == (int)ModuleId.TeamTagInComment);
            var employeeMapping = _employeeTeamMappingRepo.GetQueryable().Where(x => teamId.Select(x => x.TagId).Contains(x.TeamId) && x.IsActive).Select(x => x.EmployeeId).ToList();
            empIds.AddRange(empId.Select(x => x.TagId));
            teamEmpIds.AddRange(employeeMapping);
            if (teamEmpIds.Count > 0)
            {
                var finalList = teamEmpIds.Intersect(removeEmpIds).ToList();
                foreach (var item in finalList)
                {
                    teamEmpIds.Remove(item);
                }
                teamEmpIds.Remove(userIdentity.EmployeeId);
                var notificationRequest = new NotificationsRequest()
                {
                    By = userIdentity.EmployeeId,
                    Text = AppConstants.TeamCommentTag.Replace("UserName", userIdentity.FirstName + " " + userIdentity.LastName),
                    NotificationType = (int)EnumNotificationType.TagComment,
                    AppId = AppConstants.AppIdForOkrService,
                    MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                    Url = "recognize/" + recognitionId + "/" + commentId,
                    To = teamEmpIds.Distinct().ToList(),
                    Actionable = true,
                    NotificationOnTypeId = 1,
                    NotificationOnId = commentId
                };
                _commonService.Notifications(notificationRequest, settings);
                finalEmpIds.AddRange(teamEmpIds);

            }

            if (empIds.Count > 0)
            {
                var finalList = empIds.Intersect(removeEmpIds).ToList();
                foreach (var item in finalList)
                {
                    empIds.Remove(item);
                }
                empIds.Remove(userIdentity.EmployeeId);
                var listRemove = teamEmpIds.Intersect(empIds).ToList();
                foreach (var item in listRemove)
                {
                    empIds.Remove(item);
                }
                var notificationRequest = new NotificationsRequest()
                {
                    By = userIdentity.EmployeeId,
                    Text = AppConstants.TagComment.Replace("UserName", userIdentity.FirstName + " " + userIdentity.LastName),
                    NotificationType = (int)EnumNotificationType.TagComment,
                    AppId = AppConstants.AppIdForOkrService,
                    MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                    Url = "recognize/" + recognitionId + "/" + commentId,
                    To = empIds.Distinct().ToList(),
                    Actionable = true,
                    NotificationOnTypeId = 1,
                    NotificationOnId = commentId
                };
                _commonService.Notifications(notificationRequest, settings);
                finalEmpIds.AddRange(empIds);
            }

            return finalEmpIds;
        }

        public async Task CommentandReplyLikeNotifications(long commentId, long empId, long loginUserId, long likeId,int moduleId)
        {
            var commentDetails = await _commentDetailsRepo.FirstOrDefaultAsync(x => x.CommentDetailsId == commentId);
            var commentDetailRecognize = await _commentDetailsRepo.FirstOrDefaultAsync(x => x.CommentDetailsId == commentDetails.ModuleDetailsId);
            var employeeTag = await _employeeTagRepo.GetQueryable().Where(x => x.ModuleDetailsId == commentId && x.IsActive).ToListAsync();
            var employeesTag = new List<long>();
            var teamTag = new List<long>();
            string url = "";
            if (moduleId == (int)LikeModule.CommentsLike)
            {
                employeesTag = employeeTag.Where(x => x.ModuleId == (int)ModuleId.Comments && x.TagId != loginUserId).Select(x => x.TagId).ToList();
                teamTag = employeeTag.Where(x => x.ModuleId == (int)ModuleId.TeamTagInComment).Select(x => x.TagId).ToList();
                url = "recognize/" + commentDetails.ModuleDetailsId + "/" + commentDetails.CommentDetailsId;
            }
            else if (moduleId == (int)LikeModule.CommentReplyLike)
            {
                employeesTag = employeeTag.Where(x => x.ModuleId == (int)ModuleId.ReplyComments && x.TagId != loginUserId).Select(x => x.TagId).ToList();
                teamTag = employeeTag.Where(x => x.ModuleId == (int)ModuleId.ReplyTeamComments).Select(x => x.TagId).ToList();
                url = "recognize/" + commentDetailRecognize.ModuleDetailsId + "/" + commentDetailRecognize.CommentDetailsId + "/" + commentId;
            }
            var employeeTeamMapping = await _employeeTeamMappingRepo.GetQueryable().Where(x => teamTag.Contains(x.TeamId) && x.IsActive && x.EmployeeId != loginUserId).Select(x => x.EmployeeId).ToListAsync();
            var empDetails = await _employeeRepo.FirstOrDefaultAsync(x => x.EmployeeId == empId && x.IsActive);
            employeesTag.AddRange(employeeTeamMapping);
            var msg = "";
            if (commentDetails.CreatedBy == empId)
            {
                employeesTag.Remove(empId);
                msg = AppConstants.CommentTagLikeMessage
                 .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                await CreatorCommentLike(employeesTag.Distinct().ToList(), commentId,  likeId, loginUserId, msg,moduleId, url).ConfigureAwait(false);
            }
            else if (commentDetails.CreatedBy != empId)
            {
                var IsCreatedBy = employeesTag.Contains(commentDetails.CreatedBy);
                if (IsCreatedBy)
                {
                    msg = AppConstants.CommentLikeMessage
                        .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                    await CreatorCommentLike(new List<long> { commentDetails.CreatedBy}, commentId, likeId, loginUserId, msg, moduleId, url).ConfigureAwait(false);
                    employeesTag.Remove(commentDetails.CreatedBy);
                    msg = AppConstants.CommentTagLikeMessage
                    .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                    await CreatorCommentLike(employeesTag.Distinct().ToList(), commentId, likeId, loginUserId, msg, moduleId, url).ConfigureAwait(false);

                }
                else
                {
                    msg = AppConstants.CommentLikeMessage
                       .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                    await CreatorCommentLike(new List<long> { commentDetails.CreatedBy }, commentId, likeId, loginUserId, msg, moduleId, url).ConfigureAwait(false);
                    msg = AppConstants.CommentTagLikeMessage
                    .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                    await CreatorCommentLike(employeesTag.Distinct().ToList(), commentId, likeId, loginUserId, msg, moduleId, url).ConfigureAwait(false);
                }
            }

        }

            private GoalObjective GetGoalObjective(long goalObjectiveId)
        {
            return _goalObjectiveRepo.GetQueryable().FirstOrDefault(x => x.GoalObjectiveId == goalObjectiveId);
        }

        private GoalKey GetGoalKeyDetails(long goalKeyId)
        {
            return _goalKeyRepo.GetQueryable().FirstOrDefault(x => x.GoalKeyId == goalKeyId);
        }

        private async Task CreatorCommentLike(List<long> empId, long commentId, long likeId, long loginUserId, string msg,int moduleId,string url)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var notificationRequest = new NotificationsRequest()
            {
                By = loginUserId,
                Text = msg,
                NotificationType = moduleId == (int)LikeModule.CommentsLike  ? (int)EnumNotificationType.RecognitionCommentLike : (int)EnumNotificationType.RecognitionReplyCommentLike,
                AppId = AppConstants.AppIdForOkrService,
                MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                Url = url,
                To = empId,
                Actionable = true,
                NotificationOnTypeId = 1,
                NotificationOnId = likeId
            };
            _commonService.Notifications(notificationRequest, settings);
        }

        public  async Task CreatorReplyComment(List<long> empId, long commentId, long loginUserId, string msg,long recognizeId,long parentCommentId)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var notificationRequest = new NotificationsRequest()
            {
                By = loginUserId,
                Text = msg,
                NotificationType =   (int)EnumNotificationType.RecognitionReplyComment,
                AppId = AppConstants.AppIdForOkrService,
                MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                Url = "recognize/" + recognizeId + "/" + parentCommentId + "/" + commentId,
                To = empId,
                Actionable = true,
                NotificationOnTypeId = 1,
                NotificationOnId = commentId
            };
            _commonService.Notifications(notificationRequest, settings);
        }
        private async Task<CycleDetailRequest> GetCycleDetailGoalObject(GoalObjective goalObject)
        {
            CycleDetailRequest cycleDetailRequest = new CycleDetailRequest();
            if (goalObject != null)
            {
                var cycle = string.Empty;
                var cycleDetails = await _teamCycleDetailsRepo.FirstOrDefaultAsync(x =>
                        x.TeamCycleDetailId == goalObject.ObjectiveCycleId);
                var cycleSymbols = await _cycleSymbolsRepo.FirstOrDefaultAsync(x => x.CycleId == cycleDetails.CycleId && x.CycleSymbolId == cycleDetails.CycleSymbolId);
                if (cycleDetails.CycleStartDate.Date <= DateTime.UtcNow.Date && cycleDetails.CycleEndDate.Date >= DateTime.UtcNow.Date)
                    cycle = AppConstants.CurrentCycle;
                else if (cycleDetails.CycleEndDate.Date > DateTime.UtcNow.Date)
                    cycle = AppConstants.FutureCycle;
                else if (cycleDetails.CycleStartDate.Date < DateTime.UtcNow.Date)
                    cycle = AppConstants.PastCycle;
                cycleDetailRequest.SymbolName = cycleSymbols.SymbolName;
                cycleDetailRequest.Year = cycleDetails.CycleYear ?? 0;
                cycleDetailRequest.Cycle = cycle;

            }

            return cycleDetailRequest;
        }
        private async Task SignalRLikeNotifications(List<long> empIds)
        {
            var signalrRequestModel = new SignalrRequestModel()
            {
                BroadcastValue = empIds,
                BroadcastTopic = AppConstants.TopicRecognitionNotifications
            };
            await _commonService.CallSignalRFunctionForContributors(signalrRequestModel).ConfigureAwait(false);
        }

        private async Task SendEmailCreateRecognitionForEmployees(List<long> empIds, UserIdentity userIdentity, long recognitionId, bool isAttachment, string name,string fileName, string message)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var keyVault = await _keyVaultService.GetAzureBlobKeysAsync();
            var template = await _commonService.GetMailerTemplate(TemplateCodes.RW.ToString());
            var empDetails = await _employeeRepo.GetQueryable().Where(x => empIds.Contains(x.EmployeeId)).ToListAsync();
            var body = template.Body;
            var subject = template.Subject;
            var empId = "fgdg";
            if (isAttachment)
            {
                var htmlReplace = HtmlTag();
                body = body.Replace("asj", "with a  '" + name + "'  badge");
                subject = subject.Replace(".<badge>", " with a badge.");
                body = body.Replace("Givers", "'" + userIdentity.FirstName + " " + userIdentity.LastName + "'.");
                body = body.Replace("<sccsdddddd>", htmlReplace);
                body = body.Replace("gdgs9s", keyVault.BlobCdnUrl + keyVault.BlobContainerName + "/" + AppConstants.RecognitionFolderName + "/" + fileName);
                body = body.Replace("ccbcxsx", name);
            }
            else
            {
                body = body.Replace("asj.", " ");
                subject = subject.Replace("<badge>", " ");
                body = body.Replace("Givers", "'" + userIdentity.FirstName + " " + userIdentity.LastName + "'.");
                body = body.Replace("<sccsdddddd>", " ");
            }

            body = body.Replace("year", DateTime.Now.Year.ToString())
                       .Replace("topBar", keyVault.BlobCdnCommonUrl + AppConstants.TopBar)
                       .Replace("logo", keyVault.BlobCdnCommonUrl + AppConstants.LogoImages)
                       .Replace("tick", keyVault.BlobCdnCommonUrl + AppConstants.TickImages)
                      .Replace("srcInstagram", keyVault.BlobCdnCommonUrl + AppConstants.Instagram).Replace("srcLinkedin", keyVault.BlobCdnCommonUrl + AppConstants.Linkedin)
             .Replace("srcTwitter", keyVault.BlobCdnCommonUrl + AppConstants.Twitter).Replace("srcFacebook", keyVault.BlobCdnCommonUrl + AppConstants.Facebook)
             .Replace("fb", settings.FacebookUrl).Replace("terp", settings.TwitterUrl).Replace("lk", settings.LinkedInUrl).Replace("ijk", settings.InstagramUrl)
             .Replace("<url>", settings.FrontEndUrl).Replace("<sddhdh>", settings.FrontEndUrl + "?redirectUrl=recognize/" + recognitionId + "&empId=" + empId)
              .Replace("hjgjr", message);
            foreach (var item in empDetails)
            {
                var mailBody = body;
                body = mailBody.Replace("Username", item.FirstName + " " + item.LastName).Replace("fgdg", Convert.ToString(item.EmployeeId));
                var mailRequest = new MailRequest
                {
                    MailTo = item.EmailId,
                    Subject = subject,
                    Body = body
                };
                _commonService.SendEmail(mailRequest, settings);
               //// body = body.Replace(item.FirstName + " " + item.LastName, "Username").Replace(Convert.ToString(item.EmployeeId), "fgdg");
            }

        }

        private string HtmlTag()
        {
            string html = "<tr>\n" +
              "\t<td align=\"left\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:16px;line-height:24px;color:#292929;font-family: Calibri,Arial;padding-top:26px;text-align: center;\">\n" +
              "\t\t<table width=\"70\" cellspacing=\"0\" cellpadding=\"0\" style=\"width:60px;\">\n" +
              "\t\t\t<tr>\n" +
              "\t\t\t\t<td valign=\"middle\" align=\"left\" cellpadding=\"0\" cellspacing=\"0\" style=\"border:7px solid #fef5f5;-webkit-border-radius: 50px; -moz-border-radius: 50px; border-radius: 50px;\">\n" +
              "\t\t\t\t\t<img src=\"gdgs9s\" alt=\"Badges\" width=\"50\" height=\"50\" style=\"width:50px;height:50px;-webkit-border-radius: 50px; -moz-border-radius: 50px; border-radius: 50px; display: block;\"/>\n" +
              "\t\t\t\t</td>\n" +
              "\t\t\t</tr>\n" +
              "\t\t</table>\n" +
              "\t</td>\n" +
              "</tr>\n" +
              "<tr>\n" +
              "\t<td align=\"left\" cellpadding=\"0\" cellspacing=\"0\" style=\"font-size:16px;line-height:24px;color:#292929;font-family: Calibri,Arial;padding-top:0px;text-align: left;\">\n" +
              "\t\t<strong>ccbcxsx</strong>\n" +
              "\t</td>\n" +
              "</tr>";

            return html;
        }

        private async Task ConversationReplyNotifications(long loginUser, string msg, List<long> empIds, long parentConversationId, long conversationId, long goalSourceId, int goalType, long goalId,GoalObjective okr)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            foreach (var item in empIds)
            {
                var redirect = RedirectionReplyConversation(goalSourceId, goalType, item, goalId);
                var notificationRequest = new NotificationsRequest()
                {
                    By = loginUser,
                    Text = msg,
                    NotificationType = (int)EnumNotificationType.EmployeeTag,
                    AppId = AppConstants.AppIdForOkrService,
                    MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                    Url = "conversations/" + parentConversationId + AppConstants.BackSlash + goalType + AppConstants.BackSlash + goalSourceId + AppConstants.BackSlash + redirect + AppConstants.BackSlash + okr.ObjectiveCycleId + AppConstants.BackSlash + conversationId,
                    To = new List<long> { item},
                    Actionable = true,
                    NotificationOnTypeId = 1,
                    NotificationOnId = conversationId
                };
                _commonService.Notifications(notificationRequest, settings);
            }
          
        }

        private async Task ConversationLikeReplyNotifications(long loginUser, string msg, List<long> empIds, long parentConversationId, long conversationId, long likeId ,long goalSourceId, int goalType, long goalId, GoalObjective okr)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            foreach (var item in empIds)
            {
                var redirect =  RedirectionReplyConversation(goalSourceId, goalType, item, goalId);
                var notificationRequest = new NotificationsRequest()
                {
                    By = loginUser,
                    Text = msg,
                    NotificationType = (int)EnumNotificationType.EmployeeTag,
                    AppId = AppConstants.AppIdForOkrService,
                    MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                    Url = "conversations/" + parentConversationId + AppConstants.BackSlash + goalType + AppConstants.BackSlash + goalSourceId + AppConstants.BackSlash + redirect + AppConstants.BackSlash + okr.ObjectiveCycleId + AppConstants.BackSlash + conversationId,
                    To = new List<long> { item},
                    Actionable = true,
                    NotificationOnTypeId = 1,
                    NotificationOnId = likeId
                };
                _commonService.Notifications(notificationRequest, settings);
            }
           
        }
        private long RedirectionReplyConversation(long goalSourceId, int goalType, long empId, long goalId)
        {
            long redirectEmpId = 0;
            if (goalType == 1)
            {
                var isAnyOkr = _goalObjectiveRepo.GetQueryable().FirstOrDefault(x => (x.Source == goalSourceId || x.GoalObjectiveId == goalSourceId) && x.GoalStatusId != (int)KrStatus.Declined && x.EmployeeId == empId && (x.IsActive ?? true));
                redirectEmpId = (isAnyOkr != null) ? empId : _goalObjectiveRepo.GetQueryable().FirstOrDefault(x => x.GoalObjectiveId == goalId).EmployeeId;

            }
            else
            {
                var isAnyOkr = _goalKeyRepo.GetQueryable().FirstOrDefault(x => (x.Source == goalSourceId || x.GoalKeyId == goalSourceId) && x.KrStatusId != (int)KrStatus.Declined && x.EmployeeId == empId && (x.IsActive ?? true));
                redirectEmpId = (isAnyOkr != null) ? empId : _goalKeyRepo.GetQueryable().FirstOrDefault(x => x.GoalKeyId == goalId).EmployeeId;

            }
            return redirectEmpId;
        }

        private async Task ConversationLikeCommentNotifications(long loginUser, string msg, List<long> empIds, long parentConversationId, long likeId, long goalSourceId, int goalType, long goalId, GoalObjective okr)
        {
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            foreach (var item in empIds)
            {
                var redirect = RedirectionReplyConversation(goalSourceId, goalType, item, goalId);
                var notificationRequest = new NotificationsRequest()
                {
                    By = loginUser,
                    Text = msg,
                    NotificationType = (int)EnumNotificationType.EmployeeTag,
                    AppId = AppConstants.AppIdForOkrService,
                    MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                    Url = "conversations/" + parentConversationId + AppConstants.BackSlash + goalType + AppConstants.BackSlash + goalSourceId + AppConstants.BackSlash + redirect + AppConstants.BackSlash + okr.ObjectiveCycleId,
                    To = new List<long> { item },
                    Actionable = true,
                    NotificationOnTypeId = 1,
                    NotificationOnId = likeId
                };
                _commonService.Notifications(notificationRequest, settings);
            }

        }
        private async Task<CommentLikeResponse> TeamCommentNotifications(List<long> notificationTo, long loginUserId, Employee empDetails, long commmentId, long recognitionId, List<RecognitionEmployeeTeamMapping> mapping, long commentDetailId, long recognitionCreatedBy)
        {
            var teamId = mapping.Where(x => x.TeamId > 0 && notificationTo.Contains(x.EmployeeId)).Select(x => x.EmployeeId).Distinct().ToList();
            var empId = mapping.Where(x => x.TeamId == 0 && notificationTo.Contains(x.EmployeeId)).Select(x => x.EmployeeId).Distinct().ToList();
            string msg = "";
            var commentLike = new CommentLikeResponse();
            empId.Add(recognitionCreatedBy);
            if (teamId.Count > 0 && empId.Count == 0)
            {
                msg = commentDetailId == 0 ? AppConstants.CommentTeamMessage
                .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName) : AppConstants.CommentEditMessage
                .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                await SendCommentNotifications(teamId, loginUserId, msg, commmentId, recognitionId).ConfigureAwait(false);
                commentLike.TeamId = teamId;
            }
            else if (empId.Count > 0 && teamId.Count == 0)
            {
                msg = commentDetailId == 0 ? AppConstants.CommentMessage
                .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName) : AppConstants.CommentEditMessage
                .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                await SendCommentNotifications(empId.Distinct().ToList(), loginUserId, msg, commmentId, recognitionId).ConfigureAwait(false);
                commentLike.EmpIds = empId;
            }
            else if (empId.Count > 0 && teamId.Count > 0)
            {
                var ids = teamId.Intersect(empId).ToList();
                foreach (var item in ids)
                {
                    empId.Remove(item);
                }
                if (empId.Count > 0)
                {
                    msg = commentDetailId == 0 ? AppConstants.CommentMessage
                .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName) : AppConstants.CommentEditMessage
                .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                    await SendCommentNotifications(empId.Distinct().ToList(), loginUserId, msg, commmentId, recognitionId).ConfigureAwait(false);
                    commentLike.EmpIds = empId;
                }
                msg = commentDetailId == 0 ? AppConstants.CommentTeamMessage
                .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName) : AppConstants.CommentEditMessage
                .Replace("UserName", empDetails.FirstName + " " + empDetails.LastName);
                await SendCommentNotifications(teamId, loginUserId, msg, commmentId, recognitionId).ConfigureAwait(false);
                commentLike.TeamId = teamId;
            }
            return commentLike;
        }

        public async Task UserOneToOneNotifications(long requestTo, long requestFrom, long loginUser, long goalId, string title)
        {
            var loginUserDetails = _commonService.GetUserIdentity();
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            long finalUser;
            if (loginUser == requestTo)
            {
                finalUser = requestFrom;
            }
            else
            {
                finalUser = requestTo;

            }
            var userDetails = _employeeRepo.GetQueryable().FirstOrDefault(x => x.EmployeeId == finalUser);
            var notificationRequest = new NotificationsRequest()
            {
                By = loginUser,
                Text = AppConstants.MeetingMsg.Replace("<MeetingTitle>", title).Replace("<HostName>", loginUserDetails.FirstName),
                NotificationType = (int)EnumNotificationType.Request1To1,
                AppId = AppConstants.AppIdForOkrService,
                MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                Url = "OneToOne/OneToOneRequestId/" + goalId,
                To = null,
                Actionable = true
            };

            List<long> emp = new List<long>
            {
                finalUser
            };
            notificationRequest.To ??= emp;
            _commonService.Notifications(notificationRequest, settings);
            await _commonService.NoteCallSignalRforNotifications(emp).ConfigureAwait(false);
            //call AuditEngagementReport
            await _commonService.AuditEngagementReport(new CreateEngagementReportRequest { EmployeeId = loginUser, EngagementTypeId = AppConstants.Engagement_Notes }).ConfigureAwait(false);
        }
        public async Task NoteUserNotificationsAndEmails(List<long> employees, long loginUser, long goalId, int goalType, long noteId, string noteDescription)
        {
            employees.Add(loginUser);

            var userDetails = _employeeRepo.GetQueryable().Where(x => employees.Contains(x.EmployeeId));

            var notificationListTo = new List<long>();
            var keyVault = await _keyVaultService.GetAzureBlobKeysAsync();
            var settings = await _keyVaultService.GetSettingsAndUrlsAsync();
            var template = await _commonService.GetMailerTemplate(TemplateCodes.NTAG.ToString());
            var loginUserDetails = userDetails.FirstOrDefault(x => x.EmployeeId == loginUser);

            var subject = template.Subject.Replace("Youhave", AppConstants.Youhave).Replace("loginEmp", loginUserDetails.FirstName);
            GoalObjective okr;

            if (goalType == 1)
                okr = GetGoalObjective(goalId);
            else
            {
                var kr = GetGoalKeyDetails(goalId);
                okr = GetGoalObjective(kr.GoalObjectiveId);
            }
            var cycleDetailRequest = await GetCycleDetailGoalObject(okr);

            employees.RemoveAll(item => item == loginUser);
            foreach (var emp in employees)
            {
                var employee = userDetails.FirstOrDefault(x => x.EmployeeId == emp);
                if (employee != null)
                {
                    notificationListTo.Add(emp);

                    var body = template.Body;
                    var loginUrl = settings.FrontEndUrl + "?redirectUrl=notes/" + noteId + AppConstants.BackSlash + goalType + AppConstants.BackSlash + goalId + AppConstants.BackSlash + loginUser + "&empId=" + employee.EmployeeId + "&cycleId=" + okr.ObjectiveCycleId + "&year=" + okr.Year;
                    body = body.Replace("year", DateTime.Now.Year.ToString())
                        .Replace("topBar", keyVault.BlobCdnCommonUrl + AppConstants.TopBar)
                        .Replace("logo", keyVault.BlobCdnCommonUrl + AppConstants.LogoImages)
                        .Replace("tick", keyVault.BlobCdnCommonUrl + AppConstants.TickImages).Replace("<signIn>", loginUrl);
                    body = body.Replace("Username", employee.FirstName).Replace("OKRS", okr.ObjectiveName).Replace("Youhave", AppConstants.Youhave).Replace("loginEmp", loginUserDetails.FirstName).Replace("srcInstagram", keyVault.BlobCdnCommonUrl + AppConstants.Instagram).Replace("srcLinkedin", keyVault.BlobCdnCommonUrl + AppConstants.Linkedin)
                        .Replace("srcTwitter", keyVault.BlobCdnCommonUrl + AppConstants.Twitter).Replace("srcFacebook", keyVault.BlobCdnCommonUrl + AppConstants.Facebook)
                        .Replace("<cycle>", cycleDetailRequest.Cycle + AppConstants.SingleSpace + AppConstants.Cycle + " " + cycleDetailRequest.SymbolName + " " + cycleDetailRequest.Year)
                        .Replace("fb", settings.FacebookUrl).Replace("terp", settings.TwitterUrl).Replace("lk", settings.LinkedInUrl).Replace("ijk", settings.InstagramUrl).Replace("<url>", settings.FrontEndUrl);
                    body = body.Replace("<signIn>", loginUrl).Replace("jhiyo", noteDescription);
                    var mailRequest = new MailRequest
                    {
                        MailTo = employee.EmailId,
                        Subject = subject,
                        Body = body
                    };

                    _commonService.SendEmail(mailRequest, settings);
                }
            }

            var notificationRequest = new NotificationsRequest()
            {
                By = loginUser,
                Text = AppConstants.TagMessage.Replace("<OKR>", okr.ObjectiveName).Replace("<User>", loginUserDetails.FirstName).Replace("<cycle>", cycleDetailRequest.Cycle + " " + "cycle" + " " + cycleDetailRequest.SymbolName + " " + cycleDetailRequest.Year),
                NotificationType = (int)EnumNotificationType.EmployeeTag,
                AppId = AppConstants.AppIdForOkrService,
                MessageType = (int)MessageTypeForNotifications.NotificationsMessages,
                Url = "notes/" + noteId + "/" + goalType + "/" + goalId + "/" + okr.ObjectiveCycleId,
                To = null,
                Actionable = true
            };
            notificationRequest.To ??= notificationListTo;
            _commonService.Notifications(notificationRequest, settings);

        }

    }
}




