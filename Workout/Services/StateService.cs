using Workout.HttpClients;
using Workout.Models;

namespace Workout.Services;

public class StateService(GoogleClient client)
{
    public WorkoutState? Model { get; set; }

    public async Task LoadAsync() => Model = await client.LoadStateAsync();

    public Task SaveUrlAsync(string url)
    {
        Model ??= new WorkoutState { Created = DateTime.Now, Changed = DateTime.Now };
        Model.Changed = DateTime.Now;
        Model.Url = url;

        return client.SaveStateAsync(Model);
    }
}