using System;
using System.Collections.Generic;
using System.Text;

namespace CovidService {
    internal class TimeCalculator {
        public static bool IsOutDated(DateTime lastUpdate,TimeSpan delay) {
            var isOutdated = DateTime.UtcNow - lastUpdate > delay;
            return isOutdated;
            

        }
    }
}
