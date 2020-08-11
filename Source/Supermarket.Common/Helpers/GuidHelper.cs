using System;
using System.Diagnostics;

namespace Supermarket.Common.Helpers
{
    public static class GuidHelper
    {
        public static bool IsUid(this string text)
        {
            var isValid = Guid.TryParse(text, out var uid);
            return isValid
                   && IsNotEmptyGuid(uid);
        }

        public static bool IsNotUid(this string text)
        {
            return !IsUid(text);
        }

        public static void ValidateUid(this string text)
        {
            if (IsUid(text))
            {
                return;
            }

            var method = new StackTrace().GetFrame(1).GetMethod();

            throw new ArgumentException($"the Uid is not valid > {text} [{method.DeclaringType}.{method.Name}]");
        }

        public static string GetNewUid()
        {
            var uid = Guid.NewGuid().ToUidString().ToUpper();
            return uid;
        }

        public static bool IsEmptyGuid(this Guid guid)
        {
            return guid == Guid.Empty;
        }

        public static bool IsNotEmptyGuid(this Guid guid)
        {
            return !IsEmptyGuid(guid);
        }

        public static string ToUidString(this Guid guid)
        {
            return guid.ToString("N");
        }
    }
}