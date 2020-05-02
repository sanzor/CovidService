using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace CovidService {
    [Serializable]
    public class Config {
        [JsonPropertyName("outputFile")]
        public string OutputFile { get; set; }
        [JsonPropertyName("inputUrl")]
        public string InputUrl { get; set; }
        /// <summary>
        /// Delay in Hours
        /// </summary>
        [JsonPropertyName("delay")]
        public int QueryDelay { get; set; }
        public static Config Default => new Config {
            InputUrl = "https://api1.datelazi.ro/api/v2/data",
            OutputFile= Constants.OUTPUT_FILE,
            QueryDelay=24
        };
    }
}
