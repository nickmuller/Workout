using System.Security.Claims;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using Workout.Services;

namespace Workout.Authentication;

public class GoogleUserFactory(IAccessTokenProviderAccessor accessor, IServiceProvider services)
    : AccountClaimsPrincipalFactory<RemoteUserAccount>(accessor)
{
    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
    {
        var user = await base.CreateUserAsync(account, options);

        if (user.Identity!.IsAuthenticated)
        {
            var stateService = services.GetRequiredService<StateService>();
            await stateService.LoadAsync();
        }

        return user;
    }
}