using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System.Threading.Tasks;

namespace OkrConversationService.Domain.Ports
{
   public interface IRecognitionService
    {
        Task<Payload<RecognitionReactionResponse>> GetRecognitionLike(RecognitionLikeQuery request);
        Task<Payload<CommentDetailsRequest>> CreateComments(CommentCreateCommand request);
        Task<Payload<bool>> DeleteComment(CommentDeleteCommand request);
        Task<Payload<CommentResponse>> GetComments(GetCommentQuery request);
        Task<Payload<RecognitionCreateRequest>> Create(RecognitionCreateCommand request);
        Task<Payload<RecognitionCategoryResponse>> GetCategory(RecognitionCategoryGetQuery request);
        Task<Payload<bool>> Delete(RecognitionDeleteCommand request);
        Task<Payload<RecognitionEditRequest>> EditRecognition(RecognitionEditCommand request);
        Task<Payload<MyWallOfFameResponse>> GetMyWallOfFameGetQuery(MyWallOfFameGetQuery request);
        Task<Payload<RecognitionByTeamIdResponse>> EmployeesLeaderBoard(EmployeesLeaderBoardGetQuery request);
        Task<Payload<TotalRecognitionByTeamIdResponse>> TotalRecognitionByTeamId(TotalRecognitionByTeamIdGetQuery request);
        Task<Payload<TeamByEmpIdResponse>> GetTeamsByEmpId(TeamsByEmpIdGetQuery request);
        Task<Payload<OrgRecognitionResponse>> GetOrgRecognition(GetOrgRecognitionQuery request);
        Task<Payload<RecognitionResponse>> GetRecognitionById(GetRecognitionByIdQuery request);
        Task<Payload<MyWallOfFameDashBoardResponse>> MyWallOfFameDashBoard(MyWallOfFameDashBoardGetQuery request);
        Task<Payload<RecognitionTeamsResponse>> TeamsLeaderBoard(TeamsLeaderBoardGetQuery request);
        Task<Payload<RecognitionDetailsResponse>> GetRecognition(GetRecognitionForWallQuery request);
       
    }
}
