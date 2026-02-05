using MediatR;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Domain.Queries;
using OkrConversationService.Domain.ResponseModels;
using System.Threading;
using System.Threading.Tasks;

namespace OkrConversationService.Infrastructure.Adapters.QueryHandlers
{
    public class AllDirectReportsEmployeeByIdHandler : IRequestHandler<AllDirectReportsEmployeeByIdQuery, Payload<DirectreportsResponseResult>>
    {
        private readonly ICheckInService _checkInService;

        public AllDirectReportsEmployeeByIdHandler(ICheckInService checkInService)
        {
            _checkInService = checkInService;
        }

        public async Task<Payload<DirectreportsResponseResult>> Handle(AllDirectReportsEmployeeByIdQuery request,
            CancellationToken cancellationToken)
        {
            return await _checkInService.GetAllDirectReportsByIds(request);
        }
    }
}