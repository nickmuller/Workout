using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Workout.Models;

namespace Workout.HttpClients;

public class GoogleClient(HttpClient client, IOptions<JsonSerializerOptions> jsonSerializerOptions)
{
    private const string StateFileName = "workout.json";

    public async Task<WorkoutState?> LoadStateAsync()
    {
        var fileId = await GetFileIdAsync(StateFileName);
        return !string.IsNullOrEmpty(fileId) ? await GetFileAsync<WorkoutState>(fileId) : null;
    }

    public Task SaveStateAsync(WorkoutState state)
    {
        return CreateOrUpdateFileAsync(StateFileName, state);
    }

    public async Task SaveWorkoutLogAsync(Player player)
    {
        var fileName = $"{DateTime.Now:yyyy-MM-dd} workout.json";

        if (player.WorkoutStart == null)
            throw new ArgumentException("WorkoutStart moet gevuld zijn!");

        var log = new WorkoutLog
        {
            WorkoutStart = player.WorkoutStart.Value,
            WorkoutEind = player.WorkoutEind,
            Categorie = player.Categorie
        };

        var fileId = await GetFileIdAsync(fileName);
        if (string.IsNullOrEmpty(fileId))
        {
            var logFile = new WorkoutLogFile
            {
                Created = DateTime.Now,
                Changed = DateTime.Now,
                WorkoutLijst = [log]
            };

            await CreateFileAsync(fileName, logFile);
        }
        else
        {
            var logFile = await GetFileAsync<WorkoutLogFile>(fileId);
            logFile.Changed = DateTime.Now;
            logFile.WorkoutLijst.Add(log);

            await UpdateFileAsync(fileId, logFile);
        }
    }

    public Task SavePersoonlijkeGegevensLogAsync(double gewicht)
    {
        var fileName = $"{DateTime.Now:yyyy-MM-dd} persoonlijke info.json";

        var log = new PersoonlijkeGegevensLogFile
        {
            Created = DateTime.Now,
            Changed = DateTime.Now,
            Gewicht = gewicht
        };

        return CreateOrUpdateFileAsync(fileName, log);
    }

    private async Task<string?> GetFileIdAsync(string fileName)
    {
        using var response = await client.GetAsync("/drive/v3/files?q=trashed=false");
        var fileList = await response.Content.ReadFromJsonAsync<FileListJsonModel>(jsonSerializerOptions.Value);
        var file = fileList.Files.SingleOrDefault(f => f.Name == fileName);
        return file.Id;
    }

    private async Task<T> GetFileAsync<T>(string fileId) where T : class
    {
        using var response = await client.GetAsync($"/drive/v3/files/{fileId}?alt=media");
        return (await response.Content.ReadFromJsonAsync<T>(jsonSerializerOptions.Value))!;
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
        var fileContent = new StringContent(JsonSerializer.Serialize(content, jsonSerializerOptions.Value), Encoding.UTF8, MediaTypeNames.Application.Json);
        var metaContent = JsonContent.Create(new { name = fileName });
        var multipart = new MultipartContent { metaContent, fileContent };
        using var response = await client.PostAsync("/upload/drive/v3/files?uploadType=multipart", multipart);
        response.EnsureSuccessStatusCode();
    }

    private async Task UpdateFileAsync(string fileId, object content)
    {
        var fileContent = new StringContent(JsonSerializer.Serialize(content, jsonSerializerOptions.Value), Encoding.UTF8, MediaTypeNames.Application.Json);
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