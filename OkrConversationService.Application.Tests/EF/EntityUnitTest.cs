using OkrConversationService.Domain.ResponseModels;
using OkrConversationService.Persistence.EntityFrameworkDataAccess;
using OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities;
using System;

using System.Reflection;
using Xunit;
using Module = OkrConversationService.Persistence.EntityFrameworkDataAccess.Entities.Module;

namespace OkrConversationService.Application.Tests.EF
{
    public class EntityUnitTest
    {
        [Fact]
        public void ApplicationMasterModel()
        {
            ApplicationMaster model = new ApplicationMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void AssignmentTypeMasterModel()
        {
            AssignmentTypeMaster model = new AssignmentTypeMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void BlockedDomainModel()
        {
            BlockedDomain model = new BlockedDomain();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

        [Fact]
        public void CheckInDetailModel()
        {
            CheckInDetail model = new CheckInDetail();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void CheckInEmployeeMappingModel()
        {
            CheckInEmployeeMapping model = new CheckInEmployeeMapping();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

        [Fact]
        public void CheckInPointModel()
        {
            CheckInEmployeeMapping model = new CheckInEmployeeMapping();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void ColorCodeModel()
        {
            ColorCode model = new ColorCode();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void CommentModel()
        {
            Comment model = new Comment();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void ConfidenceAuditModel()
        {
            ConfidenceAudit model = new ConfidenceAudit();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void ConfidenceMasterModel()
        {
            ConfidenceMaster model = new ConfidenceMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void ConstantModel()
        {
            Constant model = new Constant();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

        [Fact]
        public void ConversationEmployeeTagModel()
        {
            ConversationEmployeeTag model = new ConversationEmployeeTag();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void ConversationFileModel()
        {
            ConversationFile model = new ConversationFile();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void ConversationLogModel()
        {
            ConversationLog model = new ConversationLog();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void ConversationReactionModel()
        {
            ConversationReaction model = new ConversationReaction();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

        [Fact]
        public void CriteriaFeedbackMappingModel()
        {
            CriteriaFeedbackMapping model = new CriteriaFeedbackMapping();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void CriteriaMasterModel()
        {
            CriteriaMaster model = new CriteriaMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void CriteriaTypeMasterModel()
        {
            CriteriaTypeMaster model = new CriteriaTypeMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void CyclesModel()
        {
            Cycle model = new Cycle();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void CycleSymbolsModel()
        {
            CycleSymbol model = new CycleSymbol();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void DirectReporteesFilterModel()
        {
            DirectReporteesFilter model = new DirectReporteesFilter();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void EmailsModel()
        {
            Email model = new Email();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void EmployeeContactDetailModel()
        {
            EmployeeContactDetail model = new EmployeeContactDetail();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }


        [Fact]
        public void EmployeeEngagementModel()
        {
            EmployeeEngagement model = new EmployeeEngagement();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void EmployeeOkrProgressModel()
        {
            EmployeeOkrProgress model = new EmployeeOkrProgress();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void EmployeeOkrProgressDummyModel()
        {
            EmployeeOkrProgressDummy model = new EmployeeOkrProgressDummy();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void EmployeePermissionMappingsModel()
        {
            EmployeePermissionMapping model = new EmployeePermissionMapping();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void EmployeeProgressNatureModel()
        {
            EmployeeProgressNature model = new EmployeeProgressNature();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void EmployeeProgressNatureDummyModel()
        {
            EmployeeProgressNatureDummy model = new EmployeeProgressNatureDummy();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void EmployeeRoleMappingsModel()
        {
            EmployeeRoleMapping model = new EmployeeRoleMapping();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

        [Fact]
        public void EmployeeTeamMappingsModel()
        {
            EmployeeTeamMapping model = new EmployeeTeamMapping();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void EngagementTypeModel()
        {
            EngagementType model = new EngagementType();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void ErrorLogModel()
        {
            ErrorLog model = new ErrorLog();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        
        [Fact]
        public void FeedbackDetailModel()
        {
            FeedbackDetail model = new FeedbackDetail();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void FeedbackOnTypeMasterModel()
        {
            FeedbackOnTypeMaster model = new FeedbackOnTypeMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void FeedbackRequestModel()
        {
            FeedbackRequest model = new FeedbackRequest();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void GoalKeyModel()
        {
            GoalKey model = new GoalKey();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void GoalKeyAuditModel()
        {
            GoalKeyAudit model = new GoalKeyAudit();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void GoalKeyHistoryModel()
        {
            GoalKeyHistory model = new GoalKeyHistory();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void GoalObjectiveModel()
        {
            GoalObjective model = new GoalObjective();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void GoalSequenceModel()
        {
            GoalSequence model = new GoalSequence();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void GoalStatusMasterModel()
        {
            GoalStatusMaster model = new GoalStatusMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void GoalTypeMasterModel()
        {
            GoalTypeMaster model = new GoalTypeMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void GohierarchyModel()
        {
            Gohierarchy model = new Gohierarchy();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void GuidedTourModel()
        {
            GuidedTour model = new GuidedTour();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void GuidedTourControlModel()
        {
            GuidedTourControl model = new GuidedTourControl();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void JobsAuditModel()
        {
            JobsAudit model = new JobsAudit();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void KrStatusMasterModel()
        {
            KrStatusMaster model = new KrStatusMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void KrStatusMessageModel()
        {
            KrStatusMessage model = new KrStatusMessage();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void MailModel()
        {
            Mail model = new Mail();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void MailAddressTypeModel()
        {
            MailAddressType model = new MailAddressType();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void MailerTemplateModel()
        {
            MailerTemplate model = new MailerTemplate();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void MailSentLogModel()
        {
            MailSentLog model = new MailSentLog();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void MailSetupConfigModel()
        {
            MailSetupConfig model = new MailSetupConfig();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void MessageMasterModel()
        {
            MessageMaster model = new MessageMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void MessageTypeModel()
        {
            MessageType model = new MessageType();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void MetricDataMasterModel()
        {
            MetricDataMaster model = new MetricDataMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void MetricMasterModel()
        {
            MetricMaster model = new MetricMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void ModulesModel()
        {
            Module model = new Module();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void NotesModel()
        {
            Note model = new Note();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void NoteEmployeeTagsModel()
        {
            NoteEmployeeTag model = new NoteEmployeeTag();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void NoteFilesModel()
        {
            NoteFile model = new NoteFile();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

        [Fact]
        public void NotificationsDetailsModel()
        {
            NotificationsDetail model = new NotificationsDetail();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void NotificationTypeModel()
        {
            NotificationType model = new NotificationType();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

        [Fact]
        public void OkrAutoSubmitLogModel()
        {
            OkrAutoSubmitLog model = new OkrAutoSubmitLog();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void OkrNatureMasterModel()
        {
            OkrNatureMaster model = new OkrNatureMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void OkrstatusMasterModel()
        {
            OkrstatusMaster model = new OkrstatusMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

        [Fact]
        public void OkrTypeFilterModel()
        {
            OkrTypeFilter model = new OkrTypeFilter();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void OnBoardingControlModel()
        {
            OnBoardingControl model = new OnBoardingControl();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void OnBoardingScreenModel()
        {
            OnBoardingScreen model = new OnBoardingScreen();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void OneToOneDetailModel()
        {
            OneToOneDetail model = new OneToOneDetail();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void PermissionsModel()
        {
            Permission model = new Permission();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void ProgressAuditModel()
        {
            ProgressAudit model = new ProgressAudit();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void RaisedTypeMasterModel()
        {
            RaisedTypeMaster model = new RaisedTypeMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void RequestMasterModel()
        {
            RequestMaster model = new RequestMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void RoleModel()
        {
            Role model = new Role();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void RolePermissionMappingsModel()
        {
            RolePermissionMapping model = new RolePermissionMapping();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

        [Fact]
        public void StatusMasterModel()
        {
            StatusMaster model = new StatusMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void TaskDetailModel()
        {
            TaskDetail model = new TaskDetail();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void TeamModel()
        {
            Team model = new Team();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void TeamCycleModel()
        {
            TeamCycle model = new TeamCycle();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void TeamCycleDetailsModel()
        {
            TeamCycleDetail model = new TeamCycleDetail();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

        [Fact]
        public void TeamProgressModel()
        {
            TeamProgress model = new TeamProgress();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void TeamProgressDummyModel()
        {
            TeamProgressDummy model = new TeamProgressDummy();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void TeamProgressNatureModel()
        {
            TeamProgressNature model = new TeamProgressNature();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void TeamProgressNatureDummyModel()
        {
            TeamProgressNatureDummy model = new TeamProgressNatureDummy();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

        [Fact]
        public void TeamSequenceModel()
        {
            TeamSequence model = new TeamSequence();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void TeamSettingsModel()
        {
            TeamSetting model = new TeamSetting();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void TestTableModel()
        {
            TestTable model = new TestTable();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void TypeOfGoalCreationModel()
        {
            TypeOfGoalCreation model = new TypeOfGoalCreation();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void UnLockLogModel()
        {
            UnLockLog model = new UnLockLog();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void UnlockSupportTeamModel()
        {
            UnlockSupportTeam model = new UnlockSupportTeam();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void UserTokenTeamModel()
        {
            UserToken model = new UserToken();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void WeightMasterModel()
        {
            WeightMaster model = new WeightMaster();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void OperationStatusModel()
        {
            OperationStatus model = new OperationStatus();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

        [Fact]
        public void PayloadModel()
        {
            Payload<UnlockSupportTeam> model = new Payload<UnlockSupportTeam>();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }
        [Fact]
        public void PayloadCheckInDetailModel()
        {
            Payload<CheckInDetail> model = new Payload<CheckInDetail>();
            var resultGet = GetModelTestData(model);
            var resultSet = SetModelTestData(model);
            Assert.NotNull(resultGet);
            Assert.NotNull(resultSet);
        }

        private T GetModelTestData<T>(T newModel)
        {
            Type type = newModel.GetType();
            PropertyInfo[] properties = type.GetProperties();
            object value = null;
            foreach (var prop in properties)
            {
                var propTypeInfo = type.GetProperty(prop.Name.Trim());
                if (propTypeInfo.CanRead)
                    value = prop.GetValue(newModel);
            }
            return newModel;
        }
        private T SetModelTestData<T>(T newModel)
        {
            Type type = newModel.GetType();
            PropertyInfo[] properties = type.GetProperties();
            foreach (var prop in properties)
            {
                var propTypeInfo = type.GetProperty(prop.Name.Trim());
                var propType = prop.GetType();
                if (propTypeInfo.CanWrite)
                {
                    if (prop.PropertyType.Name == "String")
                    {
                        prop.SetValue(newModel, String.Empty);
                    }
                    else if (propType.IsValueType)
                    {
                        prop.SetValue(newModel, Activator.CreateInstance(propType));
                    }
                    else
                    {
                        prop.SetValue(newModel, null);
                    }
                }
            }
            return newModel;
        }

    }
}
