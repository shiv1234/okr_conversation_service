using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OkrConversationService.Domain.Ports
{
    public interface INoteService
    {
        Task<Payload<NoteResponse>> GetAll(TeamGetAllQuery request);
        Task<Payload<string>> UploadNotesImageOnAzure(UploadFileCommand request);
        Task<Payload<long>> DeleteNote(NoteDeleteCommand request);
        Task<Payload<NoteCreateRequest>> Create(NoteCreateCommand request);
        Task<Payload<NoteEditRequest>> Edit(NoteEditCommand request);
        Task<Payload<bool>> IsEmployeeTag(long noteId);
        Task<Payload<bool>> DraftToPublicUserNotificationsAndEmails(List<NoteDraftEmailRequest> goals);
        Task<Payload<UserNoteResponse>> GetAllUserNotes(UserNoteAllQuery request);

    }
}
