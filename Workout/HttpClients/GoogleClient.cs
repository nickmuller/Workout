using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Caching.Memory;
using Workout.Models;

namespace Workout.HttpClients;

public class GoogleClient(HttpClient client, IMemoryCache cache)
{
    private const string StateFileName = "workout.json";

    public async Task<WorkoutState?> LoadStateAsync()
    {
        var fileId = await GetFileIdAsync(StateFileName);
        return !string.IsNullOrEmpty(fileId) ? await GetFileAsync(fileId) : null;
    }

    public Task SaveStateAsync(WorkoutState state)
    {
        return CreateOrUpdateFileAsync(StateFileName, state);
    }

    public Task SaveWorkoutLogAsync(Player player)
    {
        var fileName = $"{DateTime.Now:yyyy-MM-dd} workout.json";

        if (player.WorkoutStart == null)
            throw new ArgumentException("WorkoutStart moet gevuld zijn!");

        var log = new WorkoutLog
        {
            Created = DateTime.Now,
            Changed = DateTime.Now,
            WorkoutStart = player.WorkoutStart.Value,
            WorkoutEind = player.WorkoutEind,
            Categorie = player.Categorie
        };

        return CreateOrUpdateFileAsync(fileName, log);
    }

    public Task SavePersoonlijkeGegevensLogAsync(int gewicht)
    {
        var fileName = $"{DateTime.Now:yyyy-MM-dd} persoonlijke info.json";

        var log = new PersoonlijkeGegevensLog
        {
            Created = DateTime.Now,
            Changed = DateTime.Now,
            Gewicht = gewicht
        };

        return CreateOrUpdateFileAsync(fileName, log);
    }

    private async Task<string?> GetFileIdAsync(string fileName)
    {
        var files = (await cache.GetOrCreateAsync("files", async e =>
        {
            e.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);

            using var response = await client.GetAsync("/drive/v3/files?q=trashed=false");
            var fileList = await response.Content.ReadFromJsonAsync<FileListJsonModel>();
            return fileList.Files;
        }))!;

        var file = files.SingleOrDefault(f => f.Name == fileName);
        return file.Id;
    }

    private async Task<WorkoutState> GetFileAsync(string fileId)
    {
        using var response = await client.GetAsync($"/drive/v3/files/{fileId}?alt=media");
        return (await response.Content.ReadFromJsonAsync<WorkoutState>())!;
    }

    private async Task CreateOrUpdateFileAsync(string fileName, object content)
    {
        var fileId = await GetFileIdAsync(fileName);
        if (string.IsNullOrEmpty(fileId))
            await CreateFileAsync(fileName, content);
        else
            await UpdateFileAsync(fileId, content);
    }

    private async Task CreateFileAsync(string fileName, object content)
    {
        var fileContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, MediaTypeNames.Application.Json);
        var metaContent = JsonContent.Create(new { name = fileName });
        var multipart = new MultipartContent { metaContent, fileContent };
        using var response = await client.PostAsync("/upload/drive/v3/files?uploadType=multipart", multipart);
        response.EnsureSuccessStatusCode();
    }

    private async Task UpdateFileAsync(string fileId, object content)
    {
        var fileContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, MediaTypeNames.Application.Json);
        using var response = await client.PatchAsync($"/upload/drive/v3/files/{fileId}?uploadType=media", fileContent);
        response.EnsureSuccessStatusCode();
    }

    private readonly record struct FileListJsonModel
    (
        FileJsonModel[] Files
    );

    private readonly record struct FileJsonModel
    (
        string? Id,
        string? Name
    );
}