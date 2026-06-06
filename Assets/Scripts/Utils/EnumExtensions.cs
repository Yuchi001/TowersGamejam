using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class EnumExtensions
    {
        public static List<T> ToList<T>() where T : Enum
            => ((T[])Enum.GetValues(typeof(T))).ToList();

        public static int Count<T>() where T : Enum 
            => Enum.GetValues(typeof(T)).Length;
        
        public static T Last<T>() where T : Enum 
            => ((T[])Enum.GetValues(typeof(T))).Last();
        
        public static T First<T>() where T : Enum 
            => ((T[])Enum.GetValues(typeof(T))).First();
    }
}