using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System.Collections.Generic;

namespace OkrConversationService.Infrastructure.Tests.MockData
{
    public static class MockNoteService
    {
        public static Payload<NoteResponse> MockGetAllResponse()
        {
            return new Payload<NoteResponse>()
            {
                Entity =
                        new NoteResponse()
                        {
                            NoteId = 1,
                            Description = "Test",
                            GoalTypeId = 1,
                            GoalId = 1,
                            CreatedBy = 1,
                            IsReadOnly = true
                        },
                IsSuccess = true
            };
        }
        public static Payload<NoteCreateRequest> MockNoteCreateRequest()
        {
            var employeeTags = new List<NoteEmployeeTags> { new NoteEmployeeTags
            {
                    EmployeeId=1,
             }};
            var assignedFiles = new List<NoteFiles> { new NoteFiles
            {
                    StorageFileName="Test",
                    FileName="Test",
                   FilePath="Test"
            }};
            return new Payload<NoteCreateRequest>()
            {
                Entity = new NoteCreateRequest
                {
                    Description = "test",
                    NoteId = 498,
                    GoalTypeId = 1,
                    GoalId = 1,
                    employeeTags = employeeTags,
                    assignedFiles = assignedFiles
                }
            };
        }
        public static Payload<NoteEditRequest> MockNoteEditRequest()
        {
            var employeeTags = new List<NoteEmployeeTags> { new NoteEmployeeTags
            {
                    EmployeeId=1,
             }};
            var assignedFiles = new List<NoteFiles> { new NoteFiles
            {
                    StorageFileName="Test",
                    FileName="Test",
                   FilePath="Test"
            }};
            return new Payload<NoteEditRequest>()
            {
                Entity = new NoteEditRequest
                {
                    Description = "test",
                    NoteId = 498,
                    employeeTags = employeeTags,
                    assignedFiles = assignedFiles
                }
            };
        }
        public static Payload<long> MockNoteDeleteResponse()
        {
            return new Payload<long>()
            {
                EntityList = new List<long>() { 1 },
                IsSuccess = true
            };
        }

        public static Payload<NoteCreateRequest> MockUploadFileRequest()
        {
            var employeeTags = new List<NoteEmployeeTags> { new NoteEmployeeTags
            {
                    EmployeeId=1,
             }};
            var assignedFiles = new List<NoteFiles> { new NoteFiles
            {
                    StorageFileName="Test",
                    FileName="Test",
                   FilePath="Test"
            }};
            return new Payload<NoteCreateRequest>()
            {
                Entity = new NoteCreateRequest
                {
                    Description = "test",
                    NoteId = 498,
                    GoalTypeId = 1,
                    GoalId = 1,
                    employeeTags = employeeTags,
                    assignedFiles = assignedFiles
                }
            };
        }
    }
}
