using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Nancy.Json;
using System.Text.Json;
using System.ServiceProcess;
using Serilog;

namespace Process {
    public sealed class Routine {
     
        private Records output { get; set; }
        private Config config { get; set; }
        private ILogger logger = Log.ForContext<Routine>();
        public Routine(Config config) {
            this.config = config;
        }
        
        public static async Task<Records> RunAsync(Config config) {
            Routine routine = new Routine(config);
            await routine.RunInnerAsync();
            return routine.output;

        }
        private async Task RunInnerAsync() {
            Log.Information("Downloading data from source");
            var rawData=await DownloadDataAsync();
            Log.Information("Normalizing received data");
            this.output=Processor.Process(rawData);
            Log.Information("Saving data to target location");
            await SaveDataAsync();
        }
        private async Task<string> DownloadDataAsync() {
            using (HttpClient client = new HttpClient()) {
                HttpRequestMessage message = new HttpRequestMessage {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(config.InputUrl)
                };
                var response = await client.SendAsync(message);
                var payload = await response.Content.ReadAsStringAsync();
                var rawData = JsonSerializer.Deserialize<Dictionary<string, object>>(payload);
                string result = rawData["historicalData"].ToString();
                return result;
            }
        }
        private async Task SaveDataAsync() {
            var payload = JsonSerializer.Serialize(this.output);
            File.WriteAllText(config.OutputFile??Constants.OUTPUT_FILE, payload);
        }
    }
}
