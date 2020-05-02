using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace Process {
    public sealed class Processor {

        public static Records Process(string input) {
            List<DailyRecord> dailyRecords = new List<DailyRecord>();
            foreach (var item in input.ToDict()) {
                
                var countryData = item.Value.ToString().ToDict()["countyInfectionsNumbers"].ToString().ToDict();
                List<RegionStat> stats = new List<RegionStat>();
                foreach (var rawRegion in countryData) {
                    var regionstat = new RegionStat {
                        RegionName = rawRegion.Key,
                        Cases = int.Parse(rawRegion.Value.ToString())
                    };
                    stats.Add(regionstat);
                }
                dailyRecords.Add(new DailyRecord { Date = DateTime.Parse(item.Key), Regions = stats.ToArray() });
            }
            var records = new Records { Items = dailyRecords };
            return records;
       



        }
    }
}
