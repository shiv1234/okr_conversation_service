using OkrConversationService.Domain.RequestModel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OkrConversationService.Domain.Ports
{
    public interface INotificationsEmailsService
    {
        Task NoteUserNotificationsAndEmails(List<long> employees, long loginUser, long goalId, int goalType, long noteId, string noteDescription);
        Task UserNotificationsAndEmails(List<long> employees, long loginUser, long goalId, int goalType, long conversationId, long goalSourceId, string conversationDescription);
        Task CreateRecognitionNotifications(UserIdentity userIdentity, long recognitionId, bool isAttachment, string name, List<long> receiverIds, string fileName, string message);
        Task UpdateNotificationsBadges(List<long> receiverId, UserIdentity userIdentity, long recognitionId);
        Task LikeNotifications(long recognitionId, long empId, long loginUserId, long likeId);
        Task<List<long>> CommentNotifications(long recognitionId, long createdBy, long commentDetailsId, long commmentId);
        Task TagPostNotifications(UserIdentity userIdentity, long recognitionId);
        Task<List<long>> TagCommentNotifications(long recognitionId, List<RecognitionEmployeeTags> employeeTags, UserIdentity userIdentity, long commentId, List<long> removeEmpIds);
        Task CommentPostNotifications(List<long> postTag, long loginUserId, UserIdentity empDetails, long commmentId, long recognitionId);
        Task<List<long>> UpateEmployeeTag(List<RecognitionEmployeeTags> employeeTags, UserIdentity userIdentity, long recognitionId);
        Task<List<long>> TagFinalCommentNotifications(long recognitionId, List<long> empFinalList, UserIdentity userIdentity, long commentId, List<long> removeEmpIds);
        Task<List<long>> UpateFinalEmployeeTag(List<long> employeeTags, UserIdentity userIdentity, long recognitionId);
        Task CommentandReplyLikeNotifications(long commentId, long empId, long loginUserId, long likeId, int moduleId);
        Task CommentRepliesNotifications(long moduleDetailsId, long createdBy, long commentDetailsId);
        Task UserReplyConversationNotifications(long loginUser, long goalId, long conversationId);
        Task LikeUserConversation(long moduleDetailsId, long employeeId, long loginUser, long likeReactionId);
        Task EditReplyConversation(List<long> empIds, long loginUser, long goalId, long conversationId);
        Task UserOneToOneNotifications(long requestTo, long requestFrom, long loginUser, long goalId, string title);
        Task LikeUserCommentConversation(long moduleDetailsId, long employeeId, long loginUser, long likeReactionId);
    }
}
