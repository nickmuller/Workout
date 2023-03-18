using System.Net.Http.Json;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using Workout.Authentication;
using Workout.Models;

namespace Workout.Services.Google;

public class GoogleDriveService : BaseService
{
    private const string FileName = "workout.json";

    public GoogleDriveService(GoogleClient googleClient) : base(googleClient) { }

    public async Task<PersistedModel?> LoadAsync()
    {
        var fileId = await GetFileIdAsync();
        if (!string.IsNullOrEmpty(fileId))
            return await GetFileAsync(fileId);

        return null;
    }

    public async Task SaveAsync(PersistedModel model)
    {
        var fileId = await GetFileIdAsync();
        if (!string.IsNullOrEmpty(fileId))
            await UpdateFileAsync(fileId, model);
        else
            await CreateFileAsync(model);
    }

    private async Task<string?> GetFileIdAsync()
    {
        using var response = await Client.GetAsync(@"/drive/v3/files?q=trashed=false");
        var lijst = await response.Content.ReadFromJsonAsync<ListJsonModel>();
        var file = lijst.Files.SingleOrDefault(f => f.Name == FileName);
        return file.Id;
    }

    private async Task<PersistedModel> GetFileAsync(string fileId)
    {
        using var response = await Client.GetAsync($@"/drive/v3/files/{fileId}?alt=media");
        return (await response.Content.ReadFromJsonAsync<PersistedModel>())!;
    }

    private async Task CreateFileAsync(PersistedModel model)
    {
        var fileContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, MediaTypeNames.Application.Json);
        var metaContent = JsonContent.Create(new { name = FileName });
        var multipart = new MultipartContent { metaContent, fileContent };
        using var response = await Client.PostAsync(@"/upload/drive/v3/files?uploadType=multipart", multipart);
        response.EnsureSuccessStatusCode();
    }

    private async Task UpdateFileAsync(string fileId, PersistedModel model)
    {
        var fileContent = new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, MediaTypeNames.Application.Json);
        using var response = await Client.PatchAsync($@"/upload/drive/v3/files/{fileId}?uploadType=media", fileContent);
        response.EnsureSuccessStatusCode();
    }

    private readonly record struct ListJsonModel
    (
        FileJsonModel[] Files
    );

    private readonly record struct FileJsonModel
    (
        string? Id,
        string? Name
    );
}