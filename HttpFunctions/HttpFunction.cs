using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace DurableVideoProcessor.HttpFunctions
{
    public static class HttpFunction
    {

        [FunctionName("HttpStarterFunction")]
        public static async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req,
            [DurableClient] IDurableOrchestrationClient starter,
            ILogger log)
        {
            var inputVideo = req.GetQueryParameterDictionary()["video"];
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("DurableVideoProcessorOrchestrator", null, inputVideo);

            log.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
