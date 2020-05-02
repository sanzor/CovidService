using Serilog;
using System;
using System.Collections.Generic;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace Process {
    public class CovidService:ServiceBase {
        private bool IsDataUpdatedToday = false;
        private readonly Config config;
        private DateTime lastUpdate;
     
        public CovidService(Config config) {
            this.config = config;
        }
        protected override void OnSessionChange(SessionChangeDescription changeDescription) {
            
            if (IsDataUpdatedToday) {
                return;
            }
            if (!((lastUpdate - DateTime.UtcNow)> TimeSpan.FromHours(24))){
                return;
            }
            Log.Information("Running routine");
            var routineTask = Task.Run(async () => await Routine.RunAsync(this.config));
            routineTask.Wait();
            lastUpdate = DateTime.UtcNow;
            Log.Information("Routine run was successful");

            
        }
    }
}
