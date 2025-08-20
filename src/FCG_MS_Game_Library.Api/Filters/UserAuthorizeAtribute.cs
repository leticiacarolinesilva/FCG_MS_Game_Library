using Microsoft.AspNetCore.Mvc;

using UserRegistrationAndGameLibrary.Domain.Enums;

namespace UserRegistrationAndGameLibrary.Api.Filters;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public sealed class UserAuthorizeAtribute : TypeFilterAttribute
{
    public UserAuthorizeAtribute(params AuthorizationPermissions[] mandatory) 
        : base(typeof(AuthorizationUserFilter))
    {
        base.Arguments = new object[] { mandatory };
    }
}
