using Blazored.LocalStorage;
using Workout.Models;

namespace Workout.Services;

public record PersistedData
{
    public IReadOnlyCollection<PersoonlijkeGegevensLogFile> PersoonlijkeGegevensLogs { get; init; } = [];
    public IReadOnlyCollection<WorkoutLogFile> WorkoutLogs { get; init; } = [];
}

public class PersistanceService(ILocalStorageService localStorage)
{
    private const string LocalstorageKey = "Workout";
    public PersistedData Data { get; private set; } = null!;

    public async Task LoadAsync()
    {
        Data = await localStorage.GetItemAsync<PersistedData>(LocalstorageKey) ?? new PersistedData();
    }

    public Task SavePersoonlijkeGegevensLogsAsync(IList<PersoonlijkeGegevensLogFile> logs)
    {
        Data = Data with
        {
            PersoonlijkeGegevensLogs = logs.AsReadOnly()
        };
        return SaveAsync();
    }

    public Task SaveWorkoutLogsAsync(IList<WorkoutLogFile> logs)
    {
        Data = Data with
        {
            WorkoutLogs = logs.AsReadOnly()
        };
        return SaveAsync();
    }

    public async Task DeleteAsync()
    {
        Data = new PersistedData();
        await localStorage.RemoveItemAsync(LocalstorageKey);
    }

    private async Task SaveAsync() => await localStorage.SetItemAsync(LocalstorageKey, Data);
}