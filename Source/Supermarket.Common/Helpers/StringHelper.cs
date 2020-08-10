using System;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Supermarket.Common.Helpers
{
    public static class StringHelper
    {
        /// <summary>
        /// Checks if a string contains number, upper case, lower case and more than 7 character 
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsValidPassword(this string text)
        {
            if (string.IsNullOrWhiteSpace(text)
                || text.Length < 8)
            {
                return false;
            }

            var hasNumber = new Regex(@"[0-9]+", RegexOptions.Compiled);
            var hasLowerChar = new Regex(@"[a-z]+", RegexOptions.Compiled);
            var hasUpperChar = new Regex(@"[A-Z]+", RegexOptions.Compiled);

            var isValidated = hasNumber.IsMatch(text)
                              && hasUpperChar.IsMatch(text)
                              && hasLowerChar.IsMatch(text);
            return isValidated;
        }

        public static bool IsNotValidPassword(this string text)
        {
            return !IsValidPassword(text);
        }

        public static bool IsEmail(this string text)
        {
            try
            {
                var mailAddress = new MailAddress(text);
                return mailAddress.Address == text;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsNotEmail(this string text)
        {
            return !IsEmail(text);
        }

        public static bool IsEmpty(this string text)
        {
            return string.IsNullOrWhiteSpace(text);
        }

        public static bool IsNotEmpty(this string text)
        {
            return !IsEmpty(text);
        }

        public static bool IsUrl(this string text)
        {
            var result = Uri.TryCreate(text, UriKind.Absolute, out var uriResult)
                         && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);

            return result;
        }

        public static bool IsNotUrl(this string text)
        {
            return !IsUrl(text);
        }

        public static string TrimOrDefault(this string text)
        {
            return text == null ? string.Empty : text.Trim();
        }

        public static string CleanWhitespaces(this string text)
        {
            return Regex.Replace(text, @"\s+", "");
        }
    }
}