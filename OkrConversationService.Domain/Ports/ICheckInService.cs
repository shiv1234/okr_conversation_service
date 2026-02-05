using OkrConversationService.Domain.Commands;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Domain.ResponseModels;
using System;
using System.Threading.Tasks;

namespace OkrConversationService.Domain.Ports
{
    public interface ICheckInService
    {
        Task<Payload<CheckInPointsResponse>> GetAll(CheckInGetAllQuery request);
        Task<Payload<CheckInDetailRequest>> Create(CheckInCreateCommand request);
        Task<Payload<CheckInDatesPermissionResponse>> GetAllCheckInWeeklyDates(CheckInWeeklyDatesQuery request);
        Task<Payload<DirectreportsResponseResult>> GetAllDirectReportsByIds(AllDirectReportsEmployeeByIdQuery request);
        Task<Payload<CheckInAlertResponse>> IsCheckInSubmitted();
        Task<Payload<CheckInVisible>> UpdateCheckinVisibility(UpdateCheckinVisibilityCommand request);
        Task<Payload<bool>> ImportPastTask(ImportPastTaskCommand request);        
        Task<Payload<DashboardCheckInResponse>> GetDashboardCheckInDetails(CheckInDashboardQuery request);
        Task<Payload<bool>> DeleteEmployeeCheckinVisible(DeleteEmployeeCheckInVisibleCommand request);
        Task<Payload<EmployeeCheckInVisibleResponse>> GetEmployeeCheckInVisible(GetEmployeeCheckInVisibleQuery request);
        Task<Payload<bool>> AddEmployeeCheckInVisible(EmployeeCheckInVisibleCommand request);
        Task<Payload<IsAddedEmployeeCheckInResponse>> IsAddedEmployeeCheckIn(IsAddedEmployeeCheckInQuery request);

    }
}
