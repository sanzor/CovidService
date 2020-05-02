using System;
using System.Collections.Generic;
using System.Text;

namespace CovidService {
    public class Constants {
        public const string LOG_FILE = @"log/log.txt";
        public const string CONFIG_FILE = "appsettings.json";
        public const string LOG_OUTPUT_TEMPLATE = "{Timestamp: HH:mm: ss} [{Level:u3}] {Properties} {Message:lj}{NewLine}{Exception}";
        public const string CORELLATION_ID = "corellationId";
        public const string OUTPUT_FILE = "output.json";
    }
}
