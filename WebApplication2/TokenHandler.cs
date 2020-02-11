using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace WebApplication2
{
    public class TokenOption : AuthenticationSchemeOptions
    {

    }

    public class TokenHandler : AuthenticationHandler<TokenOption>
    {

        //private readonly CarpoolContext _context;
        public TokenHandler(
        IOptionsMonitor<TokenOption> options,
        ILoggerFactory logger, UrlEncoder encoder,
        ISystemClock clock)
        : base(options, logger, encoder, clock)
        {

        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // 如果帶有`Token`標頭
            if (Context.Request.Headers.TryGetValue("Token", out StringValues token))
            {
                var customerEmail = "test@test.com";
                var customerToken = "YOUR_TOKEN";
                if (customerToken.Equals(token))
                {
                    var claims = new ClaimsPrincipal(new ClaimsIdentity[]{
                        new ClaimsIdentity(
                        new Claim[] {
                        new Claim(ClaimsIdentity.DefaultNameClaimType, customerEmail) // 直接回使用者ID
                        },
                        "Token"
                        )
                    });
                    return AuthenticateResult.Success(new AuthenticationTicket(claims, "Token"));
                }
                else
                {
                    return AuthenticateResult.Fail("Token Error");
                }
            }
            else
            {
                return AuthenticateResult.NoResult();
            }
        }
    }

}
