using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace Presentation.Functions;

public class SyncFunction
{
    [Function("Sync")]
    public HttpResponseData Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "sync")] HttpRequestData req, FunctionContext executionContext)
    {
        var logger = executionContext.GetLogger("Sync");
        logger.LogInformation("Sync endpoint called");

        var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
        return response;
    }
}
