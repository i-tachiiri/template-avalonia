namespace BackupService;

public interface IBackupOrchestrator
{
    Task SyncAsync(CancellationToken cancellationToken = default);
}
