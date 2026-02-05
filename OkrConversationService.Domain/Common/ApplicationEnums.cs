using System.ComponentModel;

namespace OkrConversationService.Domain.Common
{
    public enum Modules
    {
        General = 1,
        Organization,
        UserManagement,
        RoleManagement,
        CoachFeature
    }

    public enum Permissions
    {
        CreateOkrs = 1,
        EditOkrs,
        AssignOkr,
        AllowtoaddContributorforOkr,
        Feedbackmodule,
        OneToOneModule,
        ViewOrganizationManagementPage = 7,
        CreateTeams,
        EditMainOrganization,
        EditTeams,
        DeleteTeams,
        ViewUserManagementPage = 12,
        AddNewUsers,
        EditUsersFrom,
        DeleteUsersFrom,
        ViewRoleManagement = 16,
        AddNewRole,
        EditExistingRole,
        DeleteRole,
        AllowCreateOkrsOnBehalfOfAnotherPerson = 20
    }

    public enum GeneralPermissions
    {
        CreateOkrs = 1,
        AssignOkr,
        AllowtoaddContributorforOkr,
        Feedbackmodule,
        OneToOneModule
    }

    public enum OrganizationPermissions
    {
        ViewOrganizationManagementPage = 6,
        CreateTeams,
        EditMainOrganization,
        EditTeams,
        DeleteTeams
    }

    public enum UserManagementPermissions
    {
        ViewUserManagementPage = 11,
        AddNewUsers,
        EditUsersFrom,
        DeleteUsersFrom
    }

    public enum RoleManagementPermissions
    {
        ViewRoleManagement = 15,
        AddNewRole,
        EditExistingRole,
        DeleteRole
    }

    public enum CoachFeaturePermissions
    {
        AllowCreateOkrsOnBehalfOfAnotherPerson = 19
    }

    public enum MessageType
    {
        /// <summary>
        /// The information
        /// </summary>
        Info,
        /// <summary>
        /// The success
        /// </summary>
        Success,
        /// <summary>
        /// The alert
        /// </summary>
        Alert,
        /// <summary>
        /// The warning
        /// </summary>
        Warning,
        /// <summary>
        /// The error
        /// </summary>
        Error
    }

    public enum EmployeePermissionType
    {
        PermissionRemoved,
        PermissionAdded
    }
    public enum EmployeeDefaultRole
    {
        CEO = 2,
        User = 4

    }

    public enum ConversationType
    {
        Conversation = 1,
        ProgressConversation = 2,
        ConversationReplyComment = 3
    }
    public enum EnumNotificationType
    {
        EmployeeTag = 27,
        Recognition = 34,
        RecognitionLike = 35,
        RecognitionComment = 36,
        TagPost = 38,
        TagComment = 39,
        RecognitionCommentLike = 45,
        RecognitionReplyCommentLike = 46,
        RecognitionReplyComment = 47,        
        Request1To1 = 37
    }
    public enum MessageTypeForNotifications
    {
        NotificationsMessages = 1
    }
    public enum TemplateCodes
    {
        CTAG = 98,
        RW = 116,
        RWT = 118,
        NTAG = 97,
    }
    public enum AzureBusServiceName
    {
        Email = 1,
        Report,
        Team,
        Role,
        User,
        Notification,
        AuditLog
    }
    public enum KrStatus
    {
        Pending = 1,
        Accepted,
        Declined
    }
    public enum GoalStatus
    {
        Draft = 1,
        Public = 2,
        Archived = 3
    }

    public enum ModuleId
    {
        Conversation = 1,
        Recognisation = 2,
        Comments = 3,
        TeamTagInComment = 4,
        TeamTagInRecognisationInBody = 5,
        EmployeeReceiver = 6,
        TeamReceiver = 7,
        ReplyComments = 8,
        ReplyTeamComments = 9,

    }

    public enum CommentModuleId
    {
        Recognisation = 1,
        ReplyComments = 2
    }

    public enum RecognitionCategoryType
    {
        Badge = 1,
        Sticker = 2,
        RecognitionScreenshot = 3,
        RecognitionCommentScreenshot = 4,
        RecognitionReplyCommentScreenshot = 5
    }
    public enum SearchType
    {
        All=0,
        Employee = 1,
        Team = 2
    }

    public enum LikeModule
    {
        CommentsLike = 3,
        CommentReplyLike = 4
    }

   
   
    public enum EnumCheckInStatus
    {
        //[Display(Name = "Check-in Missed")]
        [Description("Check-in Missed")]
        CheckInMissed = 1,  //GREY CIRCLE
        [Description("Checked-in")]
        CheckedIn, //GREEN CIRCLE 
        [Description("Not Yet Checked-in")]
        NotYetCheckedIn, //BLUE BLANK CIRCLE
        [Description("No Data")]
        NoData
    }    

    public enum CheckInVisible
    {
        [Description("Visible to All")]
        VisibleToAll = 1,
        [Description("Visible to Reporting Manager")]
        VisibleToReportingManager
    }

    public enum EnumTaskType
    {
        Okr = 1,
        CheckIn
    }

    public enum EnumCheckInWeekType
    {
        AccomplishedWeek = -1,
        ExecutingWeek = 0,
        PlanningWeek = 1
    }
}
