using System.Globalization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Workout.Authentication;
using Workout.Services;
using Workout.Services.Google;

namespace Workout;

public class Program
{
    public static async Task Main(string[] args)
    {
        CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("nl-NL");
        CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("nl-NL");

        var builder = WebAssemblyHostBuilder.CreateDefault(args);
        builder.RootComponents.Add<App>("#app");
        builder.RootComponents.Add<HeadOutlet>("head::after");
        builder.Logging.SetMinimumLevel(LogLevel.Warning);
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddHttpClient<GoogleClient>().AddHttpMessageHandler<GoogleApiAuthorizationMessageHandler>();
        builder.Services.AddTransient<GoogleApiAuthorizationMessageHandler>();
        builder.Services.AddOidcAuthentication(options =>
        {
            builder.Configuration.Bind("GoogleAuth", options.ProviderOptions);
            options.ProviderOptions.DefaultScopes.Add("https://www.googleapis.com/auth/drive.appdata");
            options.ProviderOptions.DefaultScopes.Add("https://www.googleapis.com/auth/drive.file");
        }).AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, RemoteUserAccount, WorkoutUserFactory>();

        builder.Services.AddSingleton<StateService>();
        builder.Services.AddSingleton<GoogleDriveService>();

        await builder.Build().RunAsync();
    }
}