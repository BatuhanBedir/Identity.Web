using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Identity.Web.Requirements;

//parametre alabilme ihtimaline karşı.
public class ExchangeExpireRequirement : IAuthorizationRequirement
{
    // public int Age { get; set; }
}

public class ExchangeExpireRequirementHandler : AuthorizationHandler<ExchangeExpireRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        ExchangeExpireRequirement requirement)
    {
        var hasExchangeExpireClaim = context.User.HasClaim(x => x.Type == "ExchangeExpireDate");

        if (!hasExchangeExpireClaim)
        {
            context.Fail();
            return Task.CompletedTask;
        }

        Claim exchangeExpireDate = context.User.FindFirst("ExchangeExpireDate");

        if (DateTime.Now > Convert.ToDateTime(exchangeExpireDate.Value))
        {
            context.Fail();
            return Task.CompletedTask;
        }

        context.Succeed(requirement);
        return Task.CompletedTask;
    }
}