using System.Diagnostics.CodeAnalysis;

namespace OkrConversationService.Domain.Common
{
    [ExcludeFromCodeCoverage]
    public static class ResourceMessage
    {
        public static string RecordNotFoundMessage => AuthResources.GetResourceKeyValue("RecordNotFound");
        public static string NewRoleCreated => AuthResources.GetResourceKeyValue("NewRoleCreated");
        public static string RoleUpdationMessage => AuthResources.GetResourceKeyValue("RoleUpdationMessage");
        public static string RoleIdInvalid => AuthResources.GetResourceKeyValue("RoleIdInvalid");
        public static string RoleAlreadyExist => AuthResources.GetResourceKeyValue("RoleAlreadyExist");
        public static string SomethingWentWrong => AuthResources.GetResourceKeyValue("SomethingWentWrong");
        public static string AssignRoleSuccessfully => AuthResources.GetResourceKeyValue("AssignRoleSuccessfully");
        public static string Must20CharactersLong => AuthResources.GetResourceKeyValue("Must20CharactersLong");
        public static string Required => AuthResources.GetResourceKeyValue("Required");
        public static string RecordNotFound => AuthResources.GetResourceKeyValue("RecordNotFound");
        public static string NoBadges => AuthResources.GetResourceKeyValue("NoBadges");
        public static string BlockedWordErrorMessage => AuthResources.GetResourceKeyValue("BlockedWordErrorMessage");
        public static string RoleNameUpdatedSuccessfully => AuthResources.GetResourceKeyValue("RoleNameUpdatedSuccessfully");
        public static string RolePermissionSuccessfully => AuthResources.GetResourceKeyValue("RolePermissionSuccessfully");
        public static string CeoRoleCantBeEdited => AuthResources.GetResourceKeyValue("CeoRoleCantBeEdited");
        public static string CeoRoleHaveOneUserOnly => AuthResources.GetResourceKeyValue("CeoRoleHaveOneUserOnly");
        public static string FileFormatMsg => AuthResources.GetResourceKeyValue("FileFormatMsg");
        public static string FileUploadMsg => AuthResources.GetResourceKeyValue("FileUploadMsg");
        public static string ImageSize => AuthResources.GetResourceKeyValue("ImageSize");
        public static string ConversationAlreadyExist => AuthResources.GetResourceKeyValue("NoteAlreadyExist");
        public static string ConversationCreated => AuthResources.GetResourceKeyValue("ConversationCreated");
        public static string ConversationUpdated => AuthResources.GetResourceKeyValue("ConversationUpdated");
        public static string ConversationIdInvalid => AuthResources.GetResourceKeyValue("ConversationIdInvalid");
        public static string ConversationDeleted => AuthResources.GetResourceKeyValue("ConversationDeleted");
        public static string ConversationLiked => AuthResources.GetResourceKeyValue("ConversationLiked");
        public static string ConversationDisliked => AuthResources.GetResourceKeyValue("ConversationDisliked");
        public static string CommentCreated => AuthResources.GetResourceKeyValue("CommentCreated");
        public static string CommentUpdated => AuthResources.GetResourceKeyValue("CommentUpdated");
        public static string CommentDeleted => AuthResources.GetResourceKeyValue("CommentDeleted");
        public static string CommentSuccess => AuthResources.GetResourceKeyValue("CommentSuccess");
        public static string RecognitionSaved => AuthResources.GetResourceKeyValue("RecognitionSaved");
        public static string RecognitionDeleted => AuthResources.GetResourceKeyValue("RecognitionDeleted");
        public static string RecognitionUpdated => AuthResources.GetResourceKeyValue("RecognitionUpdated");
        public static string RecordFetched => AuthResources.GetResourceKeyValue("RecordFetched");
        public static string NoRecordFound => AuthResources.GetResourceKeyValue("NoRecordFound");
        public static string CommentReplyCreated => AuthResources.GetResourceKeyValue("CommentReplyCreated");
        public static string CommentReplyUpdated => AuthResources.GetResourceKeyValue("CommentReplyUpdated");
        public static string ReplyDeleted => AuthResources.GetResourceKeyValue("ReplyDeleted");
        public static string CheckInCreated => AuthResources.GetResourceKeyValue("CheckInCreated");
        public static string CheckIn2daysAlert => AuthResources.GetResourceKeyValue("CheckIn2daysAlert");
        public static string CheckIn1dayAlert => AuthResources.GetResourceKeyValue("CheckIn1dayAlert");
        public static string CheckInSubmitted => AuthResources.GetResourceKeyValue("CheckInSubmitted");
        public static string CheckinVisibleByReportingManagerOnly => AuthResources.GetResourceKeyValue("CheckinVisibleByReportingManagerOnly");
        public static string InvalidCheckInVisibilty => AuthResources.GetResourceKeyValue("InvalidCheckInVisibilty");
        public static string UserCantUpdateCheckInVisibality => AuthResources.GetResourceKeyValue("UserCantUpdateCheckInVisibality");
        public static string CheckInVisibalityUpdatedMsg => AuthResources.GetResourceKeyValue("CheckInVisibalityUpdatedMsg");
        public static string ImportPastTask => AuthResources.GetResourceKeyValue("ImportPastTask");
        public static string CheckInDashboard => AuthResources.GetResourceKeyValue("CheckInDashboard");
        public static string RecordAdded => AuthResources.GetResourceKeyValue("RecordAdded");
        public static string RecordDeleted => AuthResources.GetResourceKeyValue("RecordDeleted");
        public static string CanNotAddedReportingManger => AuthResources.GetResourceKeyValue("CanNotAddedReportingManger");
        public static string AlreadyAdded => AuthResources.GetResourceKeyValue("AlreadyAdded");
        public static string CanNotAddedSelf => AuthResources.GetResourceKeyValue("CanNotAddedSelf");

        public static string NoteDeletedSuccessfully => AuthResources.GetResourceKeyValue("NoteDeletedSuccessfully");
        public static string CannotDeleteOtherEmployeeNote => AuthResources.GetResourceKeyValue("CannotDeleteOtherEmployeeNote");
        public static string NewNoteCreated => AuthResources.GetResourceKeyValue("NewNoteCreated");
        public static string NoteUpdated => AuthResources.GetResourceKeyValue("NoteUpdated");
        public static string NoteIdInvalid => AuthResources.GetResourceKeyValue("NoteIdInvalid");
        public static string NoteAlreadyExist => AuthResources.GetResourceKeyValue("NoteAlreadyExist");



    }
}
