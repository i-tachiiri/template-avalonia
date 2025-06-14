using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace Presentation.Functions;

public class BackupTimer
{
    [Function("BackupTimer")]
    public void Run([TimerTrigger("0 0 18 * * *")] object timer, FunctionContext context)
    {
        var logger = context.GetLogger("BackupTimer");
        logger.LogInformation("Backup timer triggered");
    }
}
