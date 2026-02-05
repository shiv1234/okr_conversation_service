using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Diagnostics.CodeAnalysis;

namespace OkrConversationService.Domain.Common
{
    [ExcludeFromCodeCoverage]
    public static class AuthResources
    {
        public static ResourceSet GetResources(string cultureName)
        {
            ResourceManager resourceManager = new ResourceManager("OkrConversationService.Domain.Resources.AuthResource", Assembly.GetExecutingAssembly());
            ResourceSet resourceSet = resourceManager.GetResourceSet(CultureInfo.CreateSpecificCulture(cultureName), true, true);
            return resourceSet;
        }

        public static string GetResourceKeyValue(string keyName, string culture = "en-US")
        {
            var resultSet = GetResources(culture);
            return (resultSet != null && !string.IsNullOrEmpty(resultSet.GetString(keyName, true))) ? resultSet.GetString(keyName, true) : string.Empty;
        }
    }
}
