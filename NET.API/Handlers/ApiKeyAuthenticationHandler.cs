using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;
using NET.API.Models;

namespace NET.API.Handlers
{
    public class ApiKeyAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private IConfiguration configuration;

        public ApiKeyAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration iConfig
        ) : base(options, logger, encoder, clock)
        {
            configuration = iConfig;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(Constants.API_KEY_HEADER))
            {
                return AuthenticateResult.Fail("Header Not Found.");
            }

            string apiKeyToValidate = Request.Headers[Constants.API_KEY_HEADER];

            if (apiKeyToValidate != configuration.GetSection(Constants.APPSETTINGS).GetSection(Constants.APIKEY).Value)
            {
                return AuthenticateResult.Fail("Invalid key.");
            }

            return AuthenticateResult.Success(CreateTicket(new IdentityUser()));
        }

        private AuthenticationTicket CreateTicket(IdentityUser user)
        {
            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, configuration.GetSection(Constants.APPSETTINGS).GetSection(Constants.APIUSER).Value)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return ticket;
        }
    }
}
