using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils
{
    public static class NumberTypeExtensions
    {
        public static string ToRomanNumber(this int number)
        {
            var result = string.Empty;
            var roman = new Dictionary<int, string>() { { 1, "I" }, { 4, "IV" }, { 5, "V" }, { 9, "IX" }, { 10, "X" }, { 40, "XL" }, { 50, "L" }, { 90, "XC" }, { 100, "C" }, { 400, "CD" }, { 500, "D" }, { 900, "CM" }, { 1000, "M" } };
            while (number > 0)
                foreach (var item in roman.OrderByDescending(x => x.Key))
                    if (number / item.Key >= 1)
                    {
                        number -= item.Key;
                        result += item.Value;
                        break;
                    }
            return result;
        }
        
        public static string ToShortTime(this float timeInSeconds)
        {
            if (timeInSeconds < 0f)
                timeInSeconds = 0f;

            var hours = (int)(timeInSeconds / 3600);
            var minutes = (int)((timeInSeconds % 3600) / 60);
            var seconds = timeInSeconds % 60;

            if (hours > 0)
                return $"{hours}h" + (minutes > 0 ? $" {minutes}m" : "");

            if (minutes > 0)
                return $"{minutes}m" + (seconds >= 1f ? $" {Math.Floor(seconds)}s" : "");

            return seconds >= 1f ? $"{Math.Floor(seconds)}s" : "1s";
        }
        
        public static string ToShortInt(this int number)
        {
            string[] suffixes = { "", "K", "M", "B", "T" };
            
            if (number == 0)
                return "0";

            var negative = number < 0;
            var num = Math.Abs((double)number);

            var index = 0;
            while (num >= 1000 && index < suffixes.Length - 1)
            {
                num /= 1000.0;
                index++;
            }

            var formatted = (num % 1 == 0)
                ? num.ToString("0")
                : num.ToString("0.##");

            return (negative ? "-" : "") + formatted + suffixes[index];
        }
        
        public static string ToShortFloat(this float number)
        {
            string[] suffixes = { "", "K", "M", "B", "T" };
            
            if (number == 0f)
                return "0";

            var negative = number < 0;
            double num = Math.Abs(number);
            var index = 0;

            while (num >= 1000 && index < suffixes.Length - 1)
            {
                num /= 1000.0;
                index++;
            }

            var formatted = (num % 1 == 0) ? num.ToString("0") : num.ToString("0.##");

            return (negative ? "-" : "") + formatted + suffixes[index];
        }
        
        public static string FormatTime(this float seconds)
        {
            if (seconds < 0f)
                seconds = 0f;

            var totalSeconds = (int)seconds;

            var hours = totalSeconds / 3600;
            var minutes = (totalSeconds % 3600) / 60;
            var secs = totalSeconds % 60;

            return hours > 0 ? $"{hours}:{minutes:D2}:{secs:D2}" : $"{minutes}:{secs:D2}";
        }
    }
}