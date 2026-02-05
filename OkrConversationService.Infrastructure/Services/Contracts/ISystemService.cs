using System;
using System.Net.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage.Blob;
using OkrConversationService.Domain.Ports;

namespace OkrConversationService.Infrastructure.Services.Contracts
{
    public interface ISystemService
    {
        HttpContext HttpContext { get; }
        HttpClient SystemHttpClient();
        IKeyVaultService _keyVaultService { get; set; }
        IKeyVaultService KeyVaultService { get; }
        Uri SystemUri(string path);
        void SendServiceBusMessageByBusClient(string AzureConnectionString, string NotificationTopicName, string payload);
        CloudBlockBlob GetCloudBlockBlob(string location);

        string ImpersonatedByUserName { get; }
        string ImpersonatedBy { get; }
        string ImpersonatedById { get; }
        string ImpersonatedUserId { get; }
        string ImpersonatedUserName { get; }
        bool IsSSO { get; }
        string EncryptIdentity { get; }
    }
}
