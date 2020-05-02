using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CovidService {
    public class CovidService : ServiceBase {

        private readonly Config config;
        private DateTime lastUpdate;
        public CovidService(Config config) {
            this.config = config;
        }
      
        protected override void OnStart(string[] args) {
            try {
                if (!ShoulRunRoutineOnStartAsync().Result) {
                    return;
                }
                Log.Information("Running Routine OnStart");
                RunRoutineAsync().Wait();

            } catch (Exception ex) {
                Log.Error(ex.Message);
            }

        }
        protected override void OnSessionChange(SessionChangeDescription changeDescription) {
          
            try {
                if (!(changeDescription.Reason == SessionChangeReason.SessionLogon || changeDescription.Reason == SessionChangeReason.SessionUnlock)) {
                    return;
                }
                if (!TimeCalculator.IsOutDated(this.lastUpdate, TimeSpan.FromHours(config.QueryDelay))) {
                    return;
                }
                Log.Information("Running Routine from SessionChange");
                RunRoutineAsync().Wait();
            } catch (Exception ex) {
                Log.Error("Session Change Error", ex.Message);
                return;
            }
        }

        private async Task RunRoutineAsync() {
            this.lastUpdate = DateTime.UtcNow;
            Log.Information("Running routine");
            var routineTask = Task.Run(async () => await Routine.RunAsync(this.config));
            var records=await routineTask;
            lastUpdate = DateTime.UtcNow;
            Log.Information("Routine run was successful");
        }
        
        /// <summary>
        /// Checks if routine should run on start event
        /// </summary>
        /// <returns>If routine should start</returns>
        private async Task<bool> ShoulRunRoutineOnStartAsync() {
            try {
                var data = JsonSerializer.Deserialize<Records>(await File.ReadAllTextAsync(config.OutputFile));
               
                if (data.Items == null || data.Items.Count == 0) {
                    return true;
                }
                this.lastUpdate = data.Items.Max(x => x.Date);
                if (TimeCalculator.IsOutDated(lastUpdate, TimeSpan.FromHours(this.config.QueryDelay))) {
                    return true;
                }
                return false;

            } catch (Exception ex) {
                Log.Error("OnStart reading records file error",ex.Message);
                return true;
            }
            
        }
      
    }
}
