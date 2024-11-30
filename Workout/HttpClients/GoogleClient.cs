using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;
using Workout.Models;
using Workout.Services;

namespace Workout.HttpClients;

public class GoogleClient(HttpClient client, IOptions<JsonSerializerOptions> jsonSerializerOptions, PersistanceService persistanceService)
{
    public async Task<IReadOnlyCollection<WorkoutLogFile>> GetWorkoutLogAsync(int aantal = 5)
    {
        if (!persistanceService.Data.WorkoutLogs.Any())
        {
            var fileIds = await GetFileIdsAsync("workout.json", aantal);
            var files = new List<WorkoutLogFile>();
            foreach (var fileId in fileIds)
            {
                var file = await GetFileAsync<WorkoutLogFile>(fileId!);
                files.Add(file);
            }

            await persistanceService.SaveWorkoutLogsAsync(files.OrderBy(f => f.Changed).ToList());
        }

        return persistanceService.Data.WorkoutLogs;
    }

    public async Task SaveWorkoutLogAsync(Player player)
    {
        var fileName = $"{DateTime.Now:yyyy-MM} workout.json";

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
            await CreateOrUpdateFileCacheAsync(logFile);
        }
        else
        {
            var logFile = await GetFileAsync<WorkoutLogFile>(fileId);
            logFile.Changed = DateTime.Now;
            logFile.WorkoutLijst.RemoveAll(l => l.WorkoutStart == log.WorkoutStart);
            logFile.WorkoutLijst.Add(log);

            await UpdateFileAsync(fileId, logFile);
            await CreateOrUpdateFileCacheAsync(logFile);
        }
    }

    private Task CreateOrUpdateFileCacheAsync(WorkoutLogFile log)
    {
        var logs = persistanceService.Data.WorkoutLogs.ToList();
        var cachedLog = logs.SingleOrDefault(l => l.Changed.Date == DateTime.Today);

        if (cachedLog is not null)
        {
            cachedLog.WorkoutLijst = log.WorkoutLijst;
            cachedLog.Changed = log.Changed;
        }
        else
        {
            logs.Add(log);
        }

        return persistanceService.SaveWorkoutLogsAsync(logs);
    }

    public async Task<IReadOnlyCollection<PersoonlijkeGegevensLogFile>> GetPersoonlijkeGegevensLogsAsync(int aantal = 5)
    {
        if (!persistanceService.Data.PersoonlijkeGegevensLogs.Any())
        {
            var fileIds = await GetFileIdsAsync("persoonlijke info", aantal);
            var files = new List<PersoonlijkeGegevensLogFile>();
            foreach (var fileId in fileIds)
            {
                var file = await GetFileAsync<PersoonlijkeGegevensLogFile>(fileId!);
                files.Add(file);
            }

            await persistanceService.SavePersoonlijkeGegevensLogsAsync(files.OrderBy(f => f.Changed).ToList());
        }

        return persistanceService.Data.PersoonlijkeGegevensLogs;
    }

    public async Task SavePersoonlijkeGegevensLogAsync(decimal gewicht)
    {
        var fileName = $"{DateTime.Now:yyyy-MM} persoonlijke info.json";

        var log = new PersoonlijkeGegevensLog
        {
            Datum = DateTime.Now,
            Gewicht = gewicht
        };

        var fileId = await GetFileIdAsync(fileName);
        if (string.IsNullOrEmpty(fileId))
        {
            var logFile = new PersoonlijkeGegevensLogFile
            {
                Created = DateTime.Now,
                Changed = DateTime.Now,
                PersoonlijkeGegevensLijst = [log]
            };

            await CreateFileAsync(fileName, logFile);
            await CreateOrUpdateFileCacheAsync(logFile);
        }
        else
        {
            var logFile = await GetFileAsync<PersoonlijkeGegevensLogFile>(fileId);
            logFile.Changed = DateTime.Now;
            logFile.PersoonlijkeGegevensLijst.RemoveAll(l => l.Datum.Date == DateTime.Today);
            logFile.PersoonlijkeGegevensLijst.Add(log);

            await UpdateFileAsync(fileId, logFile);
            await CreateOrUpdateFileCacheAsync(logFile);
        }
    }

    private Task CreateOrUpdateFileCacheAsync(PersoonlijkeGegevensLogFile log)
    {
        var logs = persistanceService.Data.PersoonlijkeGegevensLogs.ToList();
        var cachedLog = logs.SingleOrDefault(l => l.Changed.Date == DateTime.Today);

        if (cachedLog is not null)
        {
            cachedLog.PersoonlijkeGegevensLijst = log.PersoonlijkeGegevensLijst;
            cachedLog.Changed = log.Changed;
        }
        else
        {
            logs.Add(log);
        }

        return persistanceService.SavePersoonlijkeGegevensLogsAsync(logs);
    }

    private async Task<string?> GetFileIdAsync(string fileName)
    {
        using var response = await client.GetAsync("/drive/v3/files?q=trashed=false");
        var fileList = await response.Content.ReadFromJsonAsync<FileListJsonModel>(jsonSerializerOptions.Value);
        var file = fileList.Files.SingleOrDefault(f => f.Name == fileName);
        return file.Id;
    }

    private async Task<string?> GetLastFileIdAsync(string fileNameContains)
    {
        var url = "/drive/v3/files" +
                  $"?q=trashed=false and name contains '{fileNameContains}'" +
                  "&orderBy=modifiedTime desc";

        using var response = await client.GetAsync(url);
        var fileList = await response.Content.ReadFromJsonAsync<FileListJsonModel>(jsonSerializerOptions.Value);
        var file = fileList.Files.FirstOrDefault();
        return file.Id;
    }

    private async Task<List<string?>> GetFileIdsAsync(string fileNameContains, int aantal = 100)
    {
        var url = "/drive/v3/files" +
                  $"?q=trashed=false and name contains '{fileNameContains}'" +
                  "&orderBy=modifiedTime desc" +
                  $"&pageSize={aantal}";

        using var response = await client.GetAsync(url);
        var fileList = await response.Content.ReadFromJsonAsync<FileListJsonModel>(jsonSerializerOptions.Value);
        return fileList.Files.Select(f => f.Id).ToList();
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