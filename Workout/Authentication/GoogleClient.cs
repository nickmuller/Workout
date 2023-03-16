namespace Workout.Authentication;

public class GoogleClient
{
    public HttpClient Client { get; }

    public GoogleClient(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("https://www.googleapis.com");
        Client = httpClient;
    }
}