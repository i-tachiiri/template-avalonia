using System.Net.Http.Json;

namespace BackupService;

public class SimpleBackupOrchestrator : IBackupOrchestrator
{
    private readonly HttpClient _httpClient;

    public SimpleBackupOrchestrator(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SyncAsync(CancellationToken cancellationToken = default)
    {
        var content = JsonContent.Create(new { Timestamp = DateTime.UtcNow });
        using var response = await _httpClient.PostAsync("api/sync", content, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
