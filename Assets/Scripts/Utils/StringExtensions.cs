using System.Globalization;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Utils
{
    public static class StringExtensions
    {
        public static string SplitCamelCase(this string current)
        {
            return Regex.Replace(current, "(\\B[A-Z])", " $1");
        }

        public static string UpperCasePerWord(this string current)
        {
            var newStr = "";
            foreach (var word in current.Split(" "))
            {
                newStr += " " + word[..1].ToUpper() + word[1..];
            }

            return newStr[1..];
        }

        public static string Color(this string current, Color color)
        {
            return $"<color=#{ColorUtility.ToHtmlStringRGBA(color)}>{current}</color>";
        }
        
        public static string Color(this string current, string color) => $"<color=#{color}>{current}</color>";
        
        public static string AsSprite(this string current) => $"<sprite name={current}>";
        
        public static Color AsColor(this string hex)
        {
            if (string.IsNullOrEmpty(hex))
                return UnityEngine.Color.white;

            if (hex.StartsWith("#")) hex = hex[1..];

            var r = byte.Parse(hex.Substring(0, 2), NumberStyles.HexNumber);
            var g = byte.Parse(hex.Substring(2, 2), NumberStyles.HexNumber);
            var b = byte.Parse(hex.Substring(4, 2), NumberStyles.HexNumber);

            if (hex.Length != 8) return new Color(r / 255f, g / 255f, b / 255f, 1f);
            var a = byte.Parse(hex.Substring(6, 2), NumberStyles.HexNumber);
            return new Color(r / 255f, g / 255f, b / 255f, a / 255f);
        }
    }
}