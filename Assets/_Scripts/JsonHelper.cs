using System;
using UnityEngine;

namespace _Scripts
{
    public class JsonHelper
    {
        public static T[] FromJsonGetArray<T>(string json)
        {
            string newJson = "{\"array\":" + json + "}";
            JsonObject<T> obj = JsonUtility.FromJson<JsonObject<T>>(newJson);
            return obj.array;
        }

        [Serializable]
        private class JsonObject<T>
        {
            public T[] array;
        }
    }
}