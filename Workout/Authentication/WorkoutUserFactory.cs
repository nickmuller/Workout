using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;
using System.Security.Claims;
using Workout.Services;
using Workout.Services.Google;

namespace Workout.Authentication;

public class WorkoutUserFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
{
    private readonly IServiceProvider services;

    public WorkoutUserFactory(IAccessTokenProviderAccessor accessor, IServiceProvider services) : base(accessor)
    {
        this.services = services;
    }

    public override async ValueTask<ClaimsPrincipal> CreateUserAsync(RemoteUserAccount account, RemoteAuthenticationUserOptions options)
    {
        var user = await base.CreateUserAsync(account, options);

        if (user.Identity!.IsAuthenticated)
        {
            var stateService = services.GetRequiredService<StateService>();
            var googleDriveService = services.GetRequiredService<GoogleDriveService>();
            stateService.Model = await googleDriveService.LoadAsync();
        }

        return user;
    }
}