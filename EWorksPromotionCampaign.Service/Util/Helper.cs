using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static EWorksPromotionCampaign.Shared.Util.Enums;

namespace EWorksPromotionCampaign.Service.Util
{
    public static class Helper
    {
        public static string RandomString(int length, DigitType type)
        {
            string chars = PossibleCharsByType(type);
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[new Random().Next(s.Length)]).ToArray());
        }
        public static string PossibleCharsByType(DigitType type)
        {
            string possible = type switch
            {
                DigitType.alphanumeric => "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789",
                DigitType.numeric => "0123456789",
                _ => "",
            };
            return possible;
        }
        public static bool HasDuplicate<T>(this IEnumerable<T> source)
        {
            if (source == null)
                throw new ArgumentException(null, nameof(source));

            Dictionary<T, bool> set = new();
            foreach (var item in source)
            {
                if (set.ContainsKey(item))
                    return true;
                set.Add(item, true);
            }
            return false;
        }
    }
}
