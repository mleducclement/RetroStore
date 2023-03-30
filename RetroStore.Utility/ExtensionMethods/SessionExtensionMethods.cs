using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RetroStore.Utility.ExtensionMethods {
    public static class SessionExtensionMethods {

        // Extension methods to set a string as a session variable key
        public static void Set<T>(this ISession session, string key, T value) {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        // Extension methods to retrieve the value of a session variable key
        public static T Get<T>(this ISession session, string key) {
            var data = session.GetString(key);
            return data == null ? default : JsonSerializer.Deserialize<T>(data);
        }
    }
}
