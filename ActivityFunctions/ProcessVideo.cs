using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace DurableVideoProcessor.ActivityFunctions
{
    public static class ProcessVideo
    {

        [FunctionName(nameof(Transcode))]
        public static async Task<string> Transcode([ActivityTrigger] string inputVideo, ILogger log)
        {
            log.LogInformation($"Transcoding {inputVideo}.");
            // Simulte transcoding
            await Task.Delay(5000);
            return $"{Path.GetFileNameWithoutExtension(inputVideo)}-transcoded.mp4";
        }

        [FunctionName(nameof(Thumbnail))]
        public static async Task<string> Thumbnail([ActivityTrigger] string inputVideo, ILogger log)
        {
            log.LogInformation($"Thumbnailing {inputVideo}.");
            // Simulte thumbnailing
            await Task.Delay(5000);
            return $"{Path.GetFileNameWithoutExtension(inputVideo)}-thumbnailed.mp4";
        }

        [FunctionName(nameof(PrependIntro))]
        public static async Task<string> PrependIntro([ActivityTrigger] string inputVideo, ILogger log)
        {
            var introLocation = Environment.GetEnvironmentVariable("IntroLocation");
            log.LogInformation($"Prepending intro into {introLocation} to {inputVideo}.");
            // Simulte transcoding
            await Task.Delay(5000);
            return $"{Path.GetFileNameWithoutExtension(inputVideo)}-withintro.mp4";
        }
    }
}