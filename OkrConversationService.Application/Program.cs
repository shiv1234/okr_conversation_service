using Azure.Extensions.AspNetCore.Configuration.Secrets;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace OkrConversationService.Application
{
    [ExcludeFromCodeCoverage]
    public static class Program
    {
        public static void Main(string[] args)
        {

            var host = CreateWebHostBuilder(args).Build();
            host.Run();

        }
        public static IHostBuilder CreateWebHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
                webBuilder.ConfigureAppConfiguration((context, config) =>
                {
                    var builtConfig = config.Build();
                    var kvUrl = builtConfig["KeyVaultConfig:KVUrl"];
                    var kvTenantId = builtConfig["KeyVaultConfig:TenantId"];
                    var kvClientId = builtConfig["KeyVaultConfig:ClientId"];
                    var kvClientSecretId = builtConfig["KeyVaultConfig:ClientSecretId"];

                    var credentials = new ClientSecretCredential(kvTenantId, kvClientId, kvClientSecretId);
                    var client = new SecretClient(new Uri(kvUrl), credentials);
                    config.AddAzureKeyVault(client, new AzureKeyVaultConfigurationOptions() { ReloadInterval = TimeSpan.FromMinutes(15) });
                }).UseStartup<Startup>());
    }
}