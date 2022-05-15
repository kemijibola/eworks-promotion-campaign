using EWorksPromotionCampaign.Shared.Util;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public static string StringifyValidationErrors(ModelStateDictionary modelState)
        {
            return
                string.Join(" | ", modelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage));
        }

        public static (int, string) MapDbResponseCodeToStatusCode(string code)
        {
            var responseCode = code switch
            {
                "00" => (200, ResponseCodes.Success),
                "09" => (409, ResponseCodes.Conflict),
                "04" => (404, ResponseCodes.NotFound),
                "05" => (400, ResponseCodes.InvalidRequest),
                "08" => (401, ResponseCodes.Unauthorized),
                "07" => (403, ResponseCodes.Forbidden),
                _ => (201, ResponseCodes.Success)
            };
            return responseCode;
        }
    }
}
