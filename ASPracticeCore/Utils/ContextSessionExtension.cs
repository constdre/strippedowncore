﻿using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace ASPracticeCore.Utils
{
    public static class ContextSessionExtension
    {

        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
        public static async void SetAsync<T>(this ISession session, string key, T value)
        {
            await session.LoadAsync();
            session.SetString(key, JsonConvert.SerializeObject(value));
        }
    }
}
