using System;
using System.Collections.Generic;
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

        public static T GetRandomFrom<T>(this T[] arr, int[] ints = null)
        {
            int r = Random.Range(0, arr.Length);
            if (ints != null)
                while (Array.Exists(ints, element => element == r))
                    r = Random.Range(0, arr.Length);
            return arr[r];
        }

        public static int GetRandomExcept<T>(this List<T> list, int except)
        {
            
            int index = 0;
            while (index == except)
                index = Random.Range(0, list.Count);
            return index;
        }
    }
}