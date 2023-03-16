using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace Workout.Authentication;

public class GoogleApiAuthorizationMessageHandler : AuthorizationMessageHandler
{
    public GoogleApiAuthorizationMessageHandler(IAccessTokenProvider provider, NavigationManager navigationManager) : base(provider, navigationManager)
    {
        ConfigureHandler(new[]
        {
            "https://www.googleapis.com",
        }, new[]
        {
            "https://www.googleapis.com/auth/drive.appdata",
            "https://www.googleapis.com/auth/drive.file",
        });
    }

    protected override async Task<HttpResponseMessage>? SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        try
        {
            return await base.SendAsync(request, cancellationToken);
        }
        catch (AccessTokenNotAvailableException ex)
        {
            // Tokens are not valid - redirect the user to log in again
            ex.Redirect();
            throw;
        }
    }
}