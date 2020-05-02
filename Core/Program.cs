using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Context;
using System;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.ServiceProcess;
using System.Text.Json;
using System.Threading.Tasks;

namespace Process {
    class Program {

        private static Config GetConfiguration() {
            Config config = null;
            var configPath = Constants.CONFIG_FILE.ToCurrentAssemblyRootPath();

            var iconfig = new ConfigurationBuilder()
               .AddJsonFile(configPath)
               .Build();
            try {
                config = iconfig.GetSection("config").Get<Config>();
                if (config == null) {
                    throw new Exception("Could not retrieve configuration from selected path");
                }
                return config;
            } catch (Exception ex) {
                Log.Error(ex.Message);
                Log.Information($"Could not find config file at path:\t{configPath}\nWill use default one");
                config = Config.Default;
                File.WriteAllText(configPath,JsonSerializer.Serialize(config)));
            }
            return config;

        }
        public static void CreateLogger() {
            var logPath = Constants.LOG_FILE.ToCurrentAssemblyRootPath();
            Log.Logger = new LoggerConfiguration()
                            .WriteTo.File(logPath, outputTemplate: Constants.LOG_OUTPUT_TEMPLATE)
                            .WriteTo.ColoredConsole(outputTemplate: Constants.LOG_OUTPUT_TEMPLATE)
                            .Enrich.FromLogContext()
                            .CreateLogger();
        }
        static void Main(string[] args) {
            CreateLogger();
            using (LogContext.PushProperty(Constants.CORELLATION_ID, Guid.NewGuid().ToString())) {
                var config = Program.GetConfiguration();
#if DEBUG
                Task task = Task.Run(async()=>await Routine.RunAsync(config));
                task.Wait();
#else
                ServiceBase.Run(new[] { new CovidService(config) });
#endif
                Log.CloseAndFlush();
            }
            

        }
    }
}
