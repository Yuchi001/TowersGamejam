using System;

namespace Utils
{
    public static class Utils
    {
        public static void Repeat(this int count, Action action)
        {
            for (var i = 0; i < count; i++) action();
        }
    }
}