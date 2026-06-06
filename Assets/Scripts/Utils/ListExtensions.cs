using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class ListExtensions
    {
        public static T RandomElement<T>(this List<T> list)
        {
            var randomIndex = Random.Range(0, list.Count);
            return list[randomIndex];
        }
        
        public static T RandomElementOrDefault<T>(this List<T> list)
        {
            if (!list.Any()) return default;
            var randomIndex = Random.Range(0, list.Count);
            return list[randomIndex];
        }
        
        public static T TakeRandomElement<T>(this List<T> list)
        {
            var randomIndex = Random.Range(0, list.Count);
            var randomElement = list[randomIndex];
            list.Remove(randomElement);
            return randomElement;
        }
    }
}