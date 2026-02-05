using System.Diagnostics.CodeAnalysis;

namespace OkrConversationService.Domain.Common
{
    [ExcludeFromCodeCoverage]
    public static class AppConstants
    {

        public const string EncryptionPrivateKey = "11b8ad45-152d-4ab4-b848-be7378a0baeb";
        public const string EncryptionSecretKey = "aB8978GGjkio02K4";
        public const string EncryptionSecretIvKey = "huI5K8o90Lhn4Jel";

        public const string Base64Regex = @"^[a-zA-Z0-9\+/]*={0,3}$";
        public const string Domain = "Domain";
        public const string Employee = "employee";
        public const string Role = "Role";
        public const string Request = "Request";
        public const int AppIdForOkrService = 3;
        public const string Handshake = "hand-shake.png";
        public const string Credentials = "credentials.png";
        public const string Facebook = "facebook.png";
        public const string Linkedin = "linkedin.png";
        public const string Twitter = "twitter.png";
        public const string Instagram = "instagram.png";
        public const string TopBar = "topBar.png";
        public const string LogoImages = "logo.png";
        public const string UnlockSupportEmailId = "adminsupport@unlockokr.com";
        public const string TickImages = "tick.png";
        public const string AdminRole = "Admin";
        public const string UserRemovalFromSystem = "was deleted from the system.";
        public const int AppIdForAdmin = 4;
        public const string UserRemovalMessage = " was removed from your company!";
        public const string LoginButtonImage = "login.png";
        public const string TagMessage = "<Username> has tagged you in a conversation for the OKR <OKR> which belongs to <cycle>";
        public const string Youhave = "You've";
        public const string BlockedWords = "BlockedWords";
        public const string EmailTopicName = "email-topic";
        public const string NotificationTopicName = "notification-topic";
        public const string TopicConversationTag = "TopicConversationTag";
        public const string CurrentCycle = "current";
        public const string FutureCycle = "future";
        public const string PastCycle = "past";
        public const string BackSlash = "/";
        public const string Cycle = "cycle";
        public const string RecognizationWithoutBadge = "<b>Receiver</b> was recognized by <b>login</b>";
        public const string RecognizationWithBadge = "<b>Receiver</b> was recognized by <b>login</b> with a <b>sticker</b> badge";
        public const string RecognitionFolderName = "RecognitionImage";
        public const string NotificationsWithBadge = "You have been recognized by 'Givers' with 'name' badge.";
        public const string NotificationsWithoutBadge = "You have been recognized by 'Givers'.";
        public const string UpdateNotifications = "'Givers' has updated the recognition post given to you.";
        public const string BadgesGivenByPeers = "Badges given by Peers";
        public const string OnlyTeamBadges = "Only Team Badges";
        public const string TotalRecognitionsReceivedByTeamMembers = "Total Recognitions Received by Team Members";
        public const string TotalBadgesReceivedByTeamMembers = "Total Badges Received by Team Members";
        public const string RecognitionsReceived = "RecognitionsReceived";
        public const string BadgesReceived = "BadgesReceived";
        public const string RecognitionsGiven = "RecognitionsGiven";
        public const string LikeMessage = "'UserName' liked your post.";
        public const string CommentMessage = "'UserName' has added a comment on your post.";
        public const string CommentEditMessage = "Comment on your post has been edited by 'UserName'.";
        public const string TagPost = "You have been tagged in a post by 'UserName'.";
        public const string TagComment = "You have been tagged in a comment by 'UserName'.";
        public const string TagLikeMessage = "'UserName' liked a post you are tagged in.";
        public const string TagCommentMessage = "'UserName' commented on a post you are tagged in.";
        public const string TopicRecognition = "TopicRecognition";
        public const string TopicRecognitionNotifications = "TopicRecognitionNotifications";
        public const string TeamRecognition = "Your Team has been recognized by 'Givers'.";
        public const string TeamRecognitionWithBadge = "Your Team has been recognized by 'Givers' with 'name' badge.";
        public const string TeamLikeMessage = "'UserName' liked a recognition post given to your Team.";
        public const string CommentTeamMessage = "'UserName' has added a comment on a recognition post given to your Team.";
        public const string TeamTagLikeMessage = "'UserName' liked a post where your team is tagged.";
        public const string TeamTagCommentMessage = "'UserName' commented on a post where your team is tagged.";
        public const string TeamCommentTag = "Your Team has been tagged in a comment by 'UserName'.";
        public const string CommentLikeMessage = "'UserName' liked your comment.";
        public const string CommentTagLikeMessage = "'UserName' liked a comment where you or your team is tagged in.";
        public const string AddedCommentTagReplies = " 'UserName' has tagged you or your team in a reply to a comment added on a post.";
        public const string AddedCommentReplies = " 'UserName' has added a reply to your comment on a post.";
        public const string AddedCommentRecognitionReceiverAndCreator = "'UserName' has added a reply to a comment on your post.";
        public const string AddedConversationCreator = "'Username' has added a reply to your comment on the conversation of the Objective 'title'.";
        public const string AddedConversationTag = "'Username' has tagged you in a comment on the conversation of the Objective 'title'.";
        public const string ConversationCommentLiked = "'Username' liked your comment on the conversation of the Objective 'title'.";
        public const string ConversationCommentTagLiked = "'Username' liked a comment you are tagged on the conversation of the Objective 'title'.";

        #region ImpersonationConstant

        public const string ImpersonateActivities = "ImpersonateActivities";
        public const string CreateOKR = "CreateOKR";
        public const string EditOKR = "EditOKR";
        public const string DeleteOKR = "DeleteOKR";
        public const string AddContributor = "AddContributor";
        public const string RemoveContributor = "RemoveContributor";
        public const string AlignOKR = "AlignOKR";
        public const string UpdateProgress = "UpdateProgress";
        public const string ViewAlignmentMapsAndPerformActions = "ViewAlignmentMapsAndPerformActions";
        public const string ViewDirectReport = "ViewDirectReport";
        public const string PeopleViewReportDownload = "PeopleViewReportDownload";
        public const string DirectReportDownload = "DirectReportDownload";
        public const string ViewReport = "ViewReport";
        public const string AddConversation = "AddConversation";
        public const string EditConversation = "EditConversation";
        public const string DeleteConversation = "DeleteConversation";
        public const string AddTask = "AddTask";
        public const string EditTask = "EditTask";
        public const string DeleteTask = "DeleteTask";
        public const string AddNote = "AddNote";
        public const string EditNote = "EditNote";
        public const string DeleteNote = "DeleteNote";
        public const string RequestPersonalFeedback = "RequestPersonalFeedback";
        public const string GivePersonalFeedback = "GivePersonalFeedback";
        public const string RequestPersonal1To1 = "RequestPersonal1To1";
        public const string RequestOKR1To1 = "RequestOKR1To1";
        public const string AddCheckIn = "AddCheckIn";
        public const string EditCheckIn = "EditCheckIn";
        public const string CreateUser = "CreateUser";
        public const string EditUser = "EditUser";
        public const string DeleteUser = "DeleteUser";
        public const string CreateRole = "CreateRole";
        public const string EditRole = "EditRole";
        public const string DeleteRole = "DeleteRole";
        public const string CreateOrg = "CreateOrg";
        public const string EditOrg = "EditOrg";
        public const string DeleteOrg = "DeleteOrg";
        public const string AdminReportDownload = "AdminReportDownload";
        public const string ViewAdminPanel = "ViewAdminPanel";
        public const string SingleSpace = " ";
        public const string TagNbsp = "&nbsp;";
        public const string HtmlRejex = "<.*?>";

        #endregion
        #region Topics
        public const string QueueEmail = "Email";
        public const string QueueReport = "Report";
        public const string QueueTeam = "Team";
        public const string QueueRole = "Role";
        public const string QueueUser = "User";
        public const string QueueNotification = "Notification";
        public const string QueueAuditLog = "AuditLog";
        #endregion

        #region  EngagementType
        public const int Engagement_AllOkrDashboard = 1;
        public const int Engagement_TeamDashboard = 2;
        public const int Engagement_PeopleSearch = 3;
        public const int Engagement_Alignment = 4;
        public const int Engagement_AskPersonalFeedback = 5;
        public const int Engagement_GivePersonalFeedback = 6;
        public const int Engagement_PersonalRequestOneandOne = 7;
        public const int Engagement_RequestingFeedback = 8;
        public const int Engagement_GivingFeedback = 9;
        public const int Engagement_RequestOneandOne = 10;
        public const int Engagement_ViewAlignmentMapAll = 11;
        public const int Engagement_ViewAlignmentMapTeam = 12;
        public const int Engagement_ViewAlignmentMapPeople = 13;
        public const int Engagement_ViewAlignmentMapOther = 14;
        public const int Engagement_UpdateOkrProgress = 15;
        public const int Engagement_Conversation = 16;
        public const int Engagement_Task = 17;
        public const int Engagement_Notes = 18;
        public const int Engagement_CheckIn = 19;
        public const int Engagement_ConfidenceUpdate = 20;
        public const int Engagement_RecognitionUpdate = 21;
        public const int Engagement_LikeConversation = 22;
        public const int Engagement_ShareRecognition = 23;
        public const int Engagement_LikeRecognition = 24;
        public const int Engagement_CommentRecognition = 25;
        public const int Engagement_SubmitOKR = 26;
        public const int Engagement_AddProgressNote = 27;
        public const int Engagement_ReplyFeedback = 28;
        public const int Engagement_MeetingRequest = 29;
        public const int Engagement_AddMeetingNotes = 30;
        public const int Engagement_CompletetedTask = 31;
        #endregion


        public const int FourWeek = 4;
        public const int TwentyFiveWeek = 25;
        public const int CheckInAlertDays = 2;
        public const string CheckIn2daysAlert = "X day(s) remaining for current week";
        public const string CheckIn1dayAlert = "Last day to submit for current week";
        public const string CheckInSubmitted = "Check-in submitted for the executing week";
        public const int CheckInfirstAlertDays = 0;
        public const string TopicCheckInUpdate = "TopicCheckInUpdate";
        public const string CheckInEndDateInDay = "CheckInEndDateInDay";
        public const string Days = "days";
        public const string Day = "day";
        public const string CheckInFutureWeekDisplayCount = "CheckInFutureWeekDisplayCount";
        public const string CheckInFutureWeekFirstQuestion = "Plan for the upcoming week";
        public const string CheckInFutureWeekLastQuestion = "Anticipated challenges & roadblocks";
        public const string ImportPastTask = "ImportPastTask";
        public const int ImportPastTaskWeek = 2;
        public const string DefaultCheckInVisibleSorting = "name";
        public const string TopicNotesTag = "TopicNotesTag";      
        public const string MeetingMsg = "Your 1-on-1 meeting '<MeetingTitle>' has been modified by '<HostName>'";
        public const string TopicRequestOneToOne = "TopicRequestOneToOne";

      
    }
}
