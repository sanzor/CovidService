using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace Process {
    public static class Extensions {
        public static Dictionary<string,object> ToDict(this string data) {
            var result = JsonSerializer.Deserialize<Dictionary<string, object>>(data);
            return result;
        }
        public static string ToCurrentAssemblyRootPath(this string target) {
            var path = Path.Combine(Directory.GetParent(Assembly.GetExecutingAssembly().FullName).FullName, target);
            return path;
        }
    }
}
