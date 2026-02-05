using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using OkrConversationService.Domain.Common;
using OkrConversationService.Domain.RequestModel;
using OkrConversationService.Infrastructure.Services.Contracts;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace OkrConversationService.Application.Extensions
{
    [ExcludeFromCodeCoverage]
    public class TokenManagerMiddleware : IMiddleware
    {

        private readonly IConfiguration _configuration;
        public ISystemService _systemService { get; set; }
        public TokenManagerMiddleware(IConfiguration configuration, ISystemService systemService)
        {
            _configuration = configuration;
            _systemService = systemService;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            if (!context.Request.Path.Value.Contains("health"))
            {
                string authorization = context.Request.Headers["Authorization"];
                if (string.IsNullOrEmpty(authorization))
                {
                    authorization = context.Request.Headers["Token"];
                    if (string.IsNullOrEmpty(authorization))
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        return;
                    }
                }

                var token = string.Empty;
                if (authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    token = authorization.Substring("Bearer ".Length).Trim();

                }
                // If no token found, no further work possible
                if (string.IsNullOrEmpty(token))
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                var stream = token;
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(stream);

                var principal = jsonToken as JwtSecurityToken;
                if (principal != null)
                {
                    var expiryDateUnix = long.Parse(principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
                    var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddSeconds(expiryDateUnix);
                    var claimIdentity = GetAllClaims(principal, context, token);
                    context.User = new ClaimsPrincipal(claimIdentity);
                    string LoggedInUserEmail = context?.User.Identities.FirstOrDefault()?.Claims.FirstOrDefault(x => x.Type == "email")?.Value;
                    if (expiryDateTimeUtc < DateTime.UtcNow)
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                        var myByteArray = Encoding.UTF8.GetBytes("TokenExpired");
                        await context.Response.Body.WriteAsync(myByteArray, 0, myByteArray.Length);
                        return;
                    }
                }
            }
            await next(context);
        }

        private ClaimsIdentity GetAllClaims(JwtSecurityToken principal, HttpContext context, string token)
        {
            UserIdentity userIdentity = GetUserIdentity(context);
            string email = string.Empty;
            string name = string.Empty;
            bool isImpersonatedUser = false;
            string impersonatedBy = string.Empty;
            string impersonatedByUserName = string.Empty;
            long impersonatedById = 0;
            long impersonatedUserId = 0;
            bool isSSO = true;
            string encryptIdentity = string.Empty;

            var emailClaim = principal.Claims.FirstOrDefault(x => x.Type == "email");
            if (emailClaim != null && !string.IsNullOrEmpty(emailClaim.Value))
                email = emailClaim.Value;
            else
            {
                var preferredClaim = principal.Claims.FirstOrDefault(x => x.Type == "preferred_username");
                if (preferredClaim != null && !string.IsNullOrEmpty(preferredClaim.Value))
                    email = preferredClaim.Value;
                else
                {
                    var uniqueClaim = principal.Claims.FirstOrDefault(x => x.Type == "unique_name");
                    if (uniqueClaim != null && !string.IsNullOrEmpty(uniqueClaim.Value))
                        email = uniqueClaim.Value;
                }
            }

            if (principal.Claims.FirstOrDefault(x => x.Type == "name") != null)
                name = principal.Claims.FirstOrDefault(x => x.Type == "name")?.Value;


            if (IsIssuerUnlockTalent(principal))
            {
                isSSO = false;
            }

            if (string.IsNullOrEmpty(email))
            {
                if (userIdentity == null || string.IsNullOrEmpty(userIdentity?.EmailId))
                {
                    email = "";
                }
                else
                {
                    email = userIdentity.EmailId;
                    name = userIdentity.FirstName + " " + userIdentity.LastName;
                }
            }

            if (userIdentity.IsImpersonatedUser)
            {
                email = userIdentity.EmailId;
                name = userIdentity.FirstName + " " + userIdentity.LastName;
                isImpersonatedUser = true;
                impersonatedBy = userIdentity.ImpersonatedBy;
                impersonatedById = userIdentity.ImpersonatedById;
                impersonatedByUserName = userIdentity.ImpersonatedByUserName;
                impersonatedUserId = userIdentity.EmployeeId;
            }

            var claimList = new List<Claim>
                {
                   new Claim("email", email),
                    new Claim("name", name),
                    new Claim("token", token),
                    new Claim("tenantId", GetTenantCliams(context)),
                    new Claim("origin", GetOriginCliams(context)),
                    new Claim("isImpersonateUser",isImpersonatedUser.ToString(),ClaimValueTypes.Boolean),
                    new Claim("impersonateBy", impersonatedBy),
                    new Claim("impersonateById", impersonatedById.ToString(),ClaimValueTypes.Integer64),
                    new Claim("impersonateByUserName", impersonatedByUserName),
                    new Claim("impersonateUserId", impersonatedUserId.ToString(),ClaimValueTypes.Integer64),
                    new Claim("isSSO", isSSO.ToString(),ClaimValueTypes.Boolean),
                    new Claim("encryptIdentity" ,GetUserIdentityClaims(context))
                };

            return new ClaimsIdentity(claimList);
        }

        private UserIdentity GetUserIdentity(HttpContext httpContext)
        {
            var hasIdentity = httpContext.Request.Headers.TryGetValue("UserIdentity", out var userIdentity);
            UserIdentity identity = new UserIdentity();
            if (hasIdentity)
            {
                var decryptVal = Encryption.DecryptStringAes(userIdentity, AppConstants.EncryptionSecretKey, AppConstants.EncryptionSecretIvKey);
                if (string.IsNullOrEmpty(decryptVal)) return identity;
                identity = JsonConvert.DeserializeObject<UserIdentity>(decryptVal);
            }
            return identity;
        }
        private string GetUserIdentityClaims(HttpContext context)
        {
            string encryptUserIdentity = string.Empty;
            var hasIdentity = context.Request.Headers.TryGetValue("UserIdentity", out var userIdentity);
            if (hasIdentity && !string.IsNullOrEmpty(userIdentity) && userIdentity != "null")
            {
                encryptUserIdentity = userIdentity;
            }
            return encryptUserIdentity;
        }

        private string GetOriginCliams(HttpContext context)
        {
            var hasOrigin = context.Request.Headers.TryGetValue("OriginHost", out var origin);
            if (!hasOrigin && context.Request.Host.Value.Contains("localhost"))
                origin = new Uri(_configuration.GetValue<string>("FrontEndUrl")).Host;

            if (hasOrigin)
                origin = new Uri(origin).Host;

            return origin;
        }

        private string GetTenantCliams(HttpContext context)
        {
            var hasTenant = context.Request.Headers.TryGetValue("TenantId", out var tenantId);
            if (!hasTenant && context.Request.Host.Value.Contains("localhost") || string.IsNullOrEmpty(tenantId) || tenantId == "null")
                tenantId = _configuration.GetValue<string>("TenantId");
            else
                tenantId = Encryption.DecryptRijndael(tenantId, AppConstants.EncryptionPrivateKey);

            return tenantId;
        }
        private bool IsIssuerUnlockTalent(JwtSecurityToken principal)
        {
            bool isDBLogin = false;
            string issuerSub = principal.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Sub).Value;
            if (!string.IsNullOrEmpty(issuerSub) && issuerSub == "DBLogin")
            {
                isDBLogin = true;
            }
            return isDBLogin;
        }
    }

}
