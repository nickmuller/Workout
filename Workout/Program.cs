using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Workout.Authentication;
using Workout.HttpClients;
using Workout.Services;

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
        builder.Services.AddMemoryCache();
        builder.Services.Configure<JsonSerializerOptions>(options =>
        {
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
        });

        builder.Services.AddOidcAuthentication(options => builder.Configuration.Bind("GoogleAuth", options.ProviderOptions))
            .AddAccountClaimsPrincipalFactory<RemoteAuthenticationState, RemoteUserAccount, GoogleUserFactory>();

        builder.Services.AddHttpClient<GoogleClient>(client => client.BaseAddress = new Uri("https://www.googleapis.com"))
            .AddHttpMessageHandler<GoogleApiAuthorizationMessageHandler>();

        builder.Services.AddTransient<GoogleApiAuthorizationMessageHandler>();
        builder.Services.AddScoped<CacheService>();

        await builder.Build().RunAsync();
    }
}