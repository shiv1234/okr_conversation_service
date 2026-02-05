using OkrConversationService.Domain.ResponseModels;
using System.Threading.Tasks;

namespace OkrConversationService.Domain.Ports
{
    public interface IKeyVaultService
    {
        Task<BlobVaultResponse> GetAzureBlobKeysAsync();
        Task<ServiceSettingUrlResponse> GetSettingsAndUrlsAsync();
    }
}
