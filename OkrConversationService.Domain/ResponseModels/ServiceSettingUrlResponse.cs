namespace OkrConversationService.Domain.ResponseModels
{
    public class ServiceSettingUrlResponse
    {
        public string UnlockLog { get; set; }
        public string OkrBaseAddress { get; set; }
        public string OkrUnlockTime { get; set; }
        public string FrontEndUrl { get; set; }
        public string ResetPassUrl { get; set; }
        public string NotificationBaseAddress { get; set; }
        public string TenantBaseAddress { get; set; }
        public string FacebookUrl { get; set; }
        public string TwitterUrl { get; set; }
        public string InstagramUrl { get; set; }
        public string LinkedInUrl { get; set; }
        public string AzureConnectionString { get; set; }
        public string ReportBaseAddress { get; set; }
    }
}
