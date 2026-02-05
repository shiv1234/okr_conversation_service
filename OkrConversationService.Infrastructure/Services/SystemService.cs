using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using OkrConversationService.Domain.Ports;
using OkrConversationService.Infrastructure.Services.Contracts;

namespace OkrConversationService.Infrastructure.Services
{
    [ExcludeFromCodeCoverage]
    public class SystemService : ISystemService
    {
        public ServiceBusClient Client { get; set; }
        public ServiceBusSender ClientSender { get; set; }
        public HttpContext HttpContext => new HttpContextAccessor().HttpContext;
        public HttpClient SystemHttpClient()
        {
            return new HttpClient();
        }
        public IKeyVaultService KeyVaultService => _keyVaultService ??= HttpContext.RequestServices.GetRequiredService<IKeyVaultService>();
        public IKeyVaultService _keyVaultService { get; set; }
        public void SendServiceBusMessageByBusClient(string AzureConnectionString, string NotificationTopicName, string payload)
        {
            Client = new ServiceBusClient(AzureConnectionString);
            ClientSender = Client.CreateSender(NotificationTopicName);
            var message = new ServiceBusMessage(payload);
            ClientSender.SendMessageAsync(message);
        }

        public Uri SystemUri(string path)
        {
            return new Uri(path);
        }

        public CloudBlockBlob GetCloudBlockBlob(string location)
        {  
            var account = new CloudStorageAccount(new StorageCredentials(_keyVaultService.GetAzureBlobKeysAsync().Result?.BlobAccountName, _keyVaultService.GetAzureBlobKeysAsync().Result?.BlobAccountKey), true);
            var cloudBlobClient = account.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(_keyVaultService.GetAzureBlobKeysAsync().Result?.BlobContainerName);
            CloudBlockBlob cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(location);
            return cloudBlockBlob;
        }

        public string ImpersonatedBy => HttpContext?.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "impersonateBy")?.Value;
        public string ImpersonatedById => HttpContext?.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "impersonateById")?.Value;
        public string ImpersonatedByUserName => HttpContext?.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "impersonateByUserName")?.Value;
        public string ImpersonatedUserId => HttpContext?.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "impersonateUserId")?.Value;
        public string ImpersonatedUserName => HttpContext?.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "name")?.Value;
        public bool IsSSO => Convert.ToBoolean(HttpContext?.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "isSSO")?.Value);
        public string EncryptIdentity => HttpContext?.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "encryptIdentity")?.Value;
    }
}
