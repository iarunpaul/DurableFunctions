using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

namespace DurableVideoProcessor.OrchestratorFunctions
{
    public static class OrchestratorFunction
    {
        [FunctionName(nameof(DurableVideoProcessorOrchestrator))]
        public static async Task<object> DurableVideoProcessorOrchestrator(
        [OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {            
            log = context.CreateReplaySafeLogger(log);
            var videoLocation = context.GetInput<string>();

            log.LogInformation("Orchestrator starts the transcode and goes to sleep...");
            var transcodelLocation = await context.CallActivityAsync<string>(nameof(ActivityFunctions.ProcessVideo.Transcode), videoLocation);
           
            log.LogInformation("Orchestrator starts the thumbnailing and goes to sleep...");
            var thumpnailLocation = await context.CallActivityAsync<string>(nameof(ActivityFunctions.ProcessVideo.Thumbnail), transcodelLocation);
            
            log.LogInformation("Orchestrator starts the intro preluding and goes to sleep...");
            var introLocation = await context.CallActivityAsync<string>(nameof(ActivityFunctions.ProcessVideo.PrependIntro), transcodelLocation);

            return new
            {
                Transcoded = transcodelLocation,
                Thumpnail = thumpnailLocation,
                Intro = introLocation
            };
        }
    }


}