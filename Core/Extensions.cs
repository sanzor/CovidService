using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace CovidService {
    public static class Extensions {
        public static object GetJsonProperty(this string data,string propertyName) {
            var dict = data.ToDict();
            if(!dict.TryGetValue(propertyName, out object prop)) {
                throw new NotSupportedException($"There is no property with the name:{propertyName}");
            }
            return prop;
        }
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
