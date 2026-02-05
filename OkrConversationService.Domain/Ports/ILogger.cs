using OkrConversationService.Domain.Common;

namespace OkrConversationService.Domain.Ports
{
    public interface ILogger
    {
        void LoggingInfo(string controller, string method, MessageType messageType, string message);
    }
}
