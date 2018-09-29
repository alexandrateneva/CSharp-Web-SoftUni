namespace SIS.HTTP.Extensions
{
    using System;

    public static class StringExtensions
    {
        public static string Capitalize(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException($"{nameof(str)} cannot be null.");
            }
            return Char.ToUpper(str[0]) + str.Substring(1).ToLower();
        }
    }
}
