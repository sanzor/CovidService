using System;
using System.Collections.Generic;
using System.Text;

namespace Process {
    public class DailyRecord {
        public DateTime Date { get; set; }
        public RegionStat[] Regions { get; set; }
    }
}
