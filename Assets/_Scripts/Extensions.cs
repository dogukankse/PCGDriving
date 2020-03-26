using System;
using Random = UnityEngine.Random;

namespace _Scripts
{
    public static class Extensions
    {
        public static T GetRandomFromEnum<T>()
        {
            var values = Enum.GetValues(typeof(T));
            return (T) values.GetValue(Random.Range(0, values.Length));
        }

        public static T GetRandomFrom<T>(this T[] arr)
        {
            return arr[Random.Range(0, arr.Length)];
        }
    }
}