using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System.Threading.Tasks;

namespace OkrConversationService.Domain.Ports
{
    public interface IConversationService
    {
        Task<Payload<ConversationResponse>> GetAll(ConversationGetAllQuery request);
        Task<Payload<string>> UploadConversationImageOnAzure(UploadFileCommand request);
        Task<Payload<ConversationCreateRequest>> Create(ConversationCreateCommand request);
        Task<Payload<ConversationEditRequest>> Edit(ConversationEditCommand request);
        Task<Payload<bool>> DeleteConversation(ConversationDeleteCommand request);
        Task<Payload<UnreadConversationResponse>> GetAllUnreadConversation(GetAllUnreadConversationQuery request);
        Task<Payload<bool>> IsEmployeeTag(long conversationId);
        Task<Payload<ConversationLikeCreateRequest>> CreateLike(ConversationLikeCommand request);
        Task<Payload<ConversationCommentResponse>> GetConversationComments(ConversationCommentGetAllQuery request);
    }
}
