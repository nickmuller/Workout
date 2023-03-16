using Workout.Authentication;

namespace Workout.Services.Google;

public abstract class BaseService
{
    protected readonly HttpClient Client;

    protected BaseService(GoogleClient googleClient)
    {
        Client = googleClient.Client;
    }
}