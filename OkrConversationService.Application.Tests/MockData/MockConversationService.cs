using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System.Collections.Generic;

namespace OkrConversationService.Application.Tests.MockData
{
    public static class MockConversationService
    {
        public static Payload<ConversationResponse> MockGetAllResponse()
        {
            return new Payload<ConversationResponse>()
            {
                EntityList =
                      new List<ConversationResponse>() {
                          new ConversationResponse() {
                              ConversationId = 1
                          }
                      },


                IsSuccess = true
            };
        }

        public static Payload<string> MockConversationUploadFile()
        {
            return new Payload<string>()
            {
                Entity = "test",

                IsSuccess = true
            };
        }

        public static Payload<UnreadConversationResponse> MockUnreadConversationResponse()
        {
            return new Payload<UnreadConversationResponse>()
            {
                Entity = new UnreadConversationResponse()
                {
                    ConversationId = 1
                },
                IsSuccess = true
            };
        }

        public static Payload<ConversationLikeCreateRequest> MockConversationLikeCreateRequest()
        {
            return new Payload<ConversationLikeCreateRequest>()
            {
                Entity = new ConversationLikeCreateRequest()
                {
                    ModuleDetailsId = 1,
                    ModuleId = 1
                },
                IsSuccess = true
            };
        }


        public static Payload<bool> MockPayloadBool()
        {
            return new Payload<bool>()
            {
                Entity = true,

                IsSuccess = true
            };
        }

        public static Payload<ConversationCreateRequest> MockConversationCreateRequest()
        {
            return new Payload<ConversationCreateRequest>()
            {
                Entity = new ConversationCreateRequest
                {
                    ConversationId = 1,
                    Description = "test",
                    GoalTypeId = 1,
                    GoalId = 1
                }
            };
        }
        public static Payload<ConversationEditRequest> MockConversationEditRequest()
        {
            return new Payload<ConversationEditRequest>()
            {
                Entity = new ConversationEditRequest
                {
                    Description = "test",
                    ConversationId = 1

                }
            };
        }
        public static Payload<RecognitionReactionResponse> MockGetRecognitionLike()
        {
            return new Payload<RecognitionReactionResponse>()
            {
                EntityList =
                      new List<RecognitionReactionResponse>() {
                          new RecognitionReactionResponse() {
                              IsLiked=true,
                               RecognitionLikeResponses =new List<RecognitionLikeResponse>()
                               {
                                   new RecognitionLikeResponse { EmployeeId=1, FirstName="test", FullName="test", LastName="", ImagePath="https://abc.jpg", LikeReactionId=1 }
                               },
                               TotalLikeCount=10
                          }
                      },


                IsSuccess = true
            };
        }

        public static Payload<CommentResponse> MockGetCommentResponse()
        {
            return new Payload<CommentResponse>()
            {
                EntityList =
                      new List<CommentResponse>() {
                          new CommentResponse() {
                              CommentDetailResponses =new List<CommentDetailResponse>()
                              {

                                  new CommentDetailResponse{ EmployeeId=1, ModuleDetailsId=1, CommentDetailsId=1,
                                   Comments="", CreatedOn=System.DateTime.UtcNow, FirstName ="", FullName="", ImagePath=""}
                              }
                          }
                      },


                IsSuccess = true
            };
        }
        public static Payload<RecognitionCategoryResponse> MockGetRecognitionCategoryResponse()
        {
            return new Payload<RecognitionCategoryResponse>()
            {
                EntityList =
                      new List<RecognitionCategoryResponse>() {
                          new RecognitionCategoryResponse() {
                              ImageFilePath="",
                              IsOnlyManager=true,
                              Name="",
                              RecognitionCategoryId=1,
                              RecognitionCategoryTypeId=1

                          }
                      },


                IsSuccess = true
            };
        }

        public static Payload<OrgRecognitionResponse> MockGetOrgRecognitionResponse()
        {
            return new Payload<OrgRecognitionResponse>()
            {
                EntityList =
                      new List<OrgRecognitionResponse>() {
                          new OrgRecognitionResponse() {
                             AttachmentImagePath="",
                             RecognitionId=1,
                             ReceiverId=1,
                             TotalCommentCount=0,
                             IsAttachment=true,
                             SenderId=1,
                             IsCommented=true,
                              RecognitionCategoryId=1,
                              RecognitionCategoryTypeId=1

                          }
                      },


                IsSuccess = true
            };
        }



        public static Payload<MyWallOfFameResponse> MockMyWallOfFameResponse()
        {
            return new Payload<MyWallOfFameResponse>()
            {
                EntityList =
                      new List<MyWallOfFameResponse>() {
                          new MyWallOfFameResponse() {
                             GivenByText="",
                             //IsGivenByManager=false,
                             RecognitionImageMappings= new List<RecognitionImageMappingResponse>{
                             new RecognitionImageMappingResponse { CreatedBy=1, FileName="abc",
                              GuidFileName="", ImageFilePath="", Name="abc",
                               RecognitionUserDetails =new List<RecognitionUserDetailsResponse>()
                               {
                                    new RecognitionUserDetailsResponse{ Count=1, Designation="",
                                        EmailId="test@gmail.com", EmployeeId=1, FirstName="test", LastName="test" }
                               } } }
                          }
                      },


                IsSuccess = true
            };
        }

        public static Payload<TeamByEmpIdResponse> MockRecognitionByTeamIdResponse()
        {
            return new Payload<TeamByEmpIdResponse>()
            {
                EntityList =
                      new List<TeamByEmpIdResponse>() {
                          new TeamByEmpIdResponse() {
                             TeamId=1,
                             TeamName="",
                             BackGroundColorCode="",
                             Colorcode=""
                             }
                          }

            ,

                IsSuccess = true
            };
        }

        public static Payload<RecognitionResponse> MockRecognitionResponse()
        {
            return new Payload<RecognitionResponse>()
            {
                EntityList =
                      new List<RecognitionResponse>() {
                          new RecognitionResponse() {
                            AttachmentImagePath="",
                            CommentDetailResponses =new List<CommentDetailResponse>{ },
                            IsLiked=true,
                            IsAttachment=true,
                            ReceiverId=1,
                            RecognitionCategoryId=1,
                            RecognitionCategoryTypeId=1,
                            IsCommented=true,
                            SenderId=1,
                            RecognitionId=1,
                            Message="",
                            CreatedOn =System.DateTime.UtcNow,
                            TotalCommentCount=1,
                            TotalLikeCount=1,
                            Headlines="",
                            RecognitionLikeResponses= new List<RecognitionLikeResponse>{ },
                            UpdatedOn = System.DateTime.UtcNow
                             }
                          }

            ,

                IsSuccess = true
            };
        }


        public static Payload<MyWallOfFameDashBoardResponse> MyWallOfFameDashBoardResponse()
        {
            return new Payload<MyWallOfFameDashBoardResponse>()
            {
                EntityList =
                      new List<MyWallOfFameDashBoardResponse>() {
                          new MyWallOfFameDashBoardResponse() {
                             RecognitionImageMappings= new List<RecognitionImageMappingResponse>{
                             new RecognitionImageMappingResponse { CreatedBy=1, FileName="abc",
                              GuidFileName="", ImageFilePath="", Name="abc",
                               RecognitionUserDetails =new List<RecognitionUserDetailsResponse>()
                               {
                                    new RecognitionUserDetailsResponse{ Count=1, Designation="",
                                        EmailId="test@gmail.com", EmployeeId=1, FirstName="test", LastName="test" }
                               } } }
                          }
                      },


                IsSuccess = true
            };
        }

        public static Payload<RecognitionByTeamIdResponse> EmployeeLeaderBoardResponse()
        {
            return new Payload<RecognitionByTeamIdResponse>()
            {
                EntityList =
                      new List<RecognitionByTeamIdResponse>() {
                          new RecognitionByTeamIdResponse() {
                               TeamId=1,
                               TeamName="",
                               RecognitionEmployees=new List<RecognitionTeam>{ new RecognitionTeam { EmployeeId=1, FirstName="", LastName="", TotalBadgesReceived=1,
                                   TotalRecognitionsGiven=1, TotalRecognitionsReceived=1 } }
                          }
                      },


                IsSuccess = true
            };
        }

        public static Payload<RecognitionTeamsResponse> TeamsLeaderBoardResponse()
        {
            return new Payload<RecognitionTeamsResponse>()
            {
                EntityList =
                      new List<RecognitionTeamsResponse>() {
                          new RecognitionTeamsResponse() {
                               TeamId=1,
                               TeamName="",
                               BackGroundColorCode="",
                          }
                      },


                IsSuccess = true
            };
        }




        public static Payload<TotalRecognitionByTeamIdResponse> TotalRecognitionByTeamIdResponse()
        {
            return new Payload<TotalRecognitionByTeamIdResponse>()
            {
                EntityList =
                      new List<TotalRecognitionByTeamIdResponse>() {
                          new TotalRecognitionByTeamIdResponse() {
                               Text="test", ToTalCount=1
                          }
                      },


                IsSuccess = true
            };
        }

        public static Payload<CommentDetailsRequest> CommentDetailsRequest()
        {
            return new Payload<CommentDetailsRequest>()
            {
                EntityList =
                      new List<CommentDetailsRequest>() {
                          new CommentDetailsRequest() {
                               CommentDetailsId=1,
                               Comments="",
                               ModuleDetailsId=1,
                               RecognitionImageRequests=new List<RecognitionImageRequest>{

                               new RecognitionImageRequest{ GuidFileName="", FileName="" }
                               }
                          }
                      },


                IsSuccess = true
            };
        }
        public static Payload<RecognitionCreateRequest> CommentRequest()
        {
            return new Payload<RecognitionCreateRequest>()
            {
                EntityList =
                      new List<RecognitionCreateRequest>() {
                          new RecognitionCreateRequest() {
                               RecognitionId=1,
                                RecognitionCategoryTypeId=1,
                                 RecognitionCategoryId=1,
                                   Message="Test",
                                    IsAttachment=true,
                               RecognitionImageRequests=new List<RecognitionImageRequest>{

                               new RecognitionImageRequest{ GuidFileName="", FileName="" }
                               }
                          }
                      },


                IsSuccess = true
            };
        }

        public static Payload<RecognitionEditRequest> EditRequest()
        {
            return new Payload<RecognitionEditRequest>()
            {
                EntityList =
                      new List<RecognitionEditRequest>() {
                          new RecognitionEditRequest() {
                               RecognitionId=1,
                               IsContentChange=true,
                                RecognitionCategoryTypeId=1,
                                 RecognitionCategoryId=1,
                                   Message="Test",
                                    IsAttachment=true,
                               RecognitionImageRequests=new List<RecognitionImageRequest>{

                               new RecognitionImageRequest{ GuidFileName="", FileName="" }
                               }
                          }
                      },


                IsSuccess = true
            };
        }


        public static Payload<OrgRecognitionResponse> GetOrgRecognition()
        {
            return new Payload<OrgRecognitionResponse>()
            {
                EntityList =
                      new List<OrgRecognitionResponse>() {
                          new OrgRecognitionResponse() {
                             Message=""
                             }
                          }

            ,

                IsSuccess = true
            };
        }


        public static Payload<RecognitionDetailsResponse> GetRecognitionForWall()
        {
            return new Payload<RecognitionDetailsResponse>()
            {
                EntityList =
                      new List<RecognitionDetailsResponse>() {
                          new RecognitionDetailsResponse() {
                             Message=""
                             }
                          }

            ,

                IsSuccess = true
            };
        }
        
    }
}
