using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace MyBookShop_Utility
{
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T Get<T>(this ISession session, string key )
        {
            var Value = session.GetString(key);
            return Value == null ? default : JsonSerializer.Deserialize<T>(Value);
         
        }
    }
}
