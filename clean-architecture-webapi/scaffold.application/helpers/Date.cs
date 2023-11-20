using System;
using System.Collections.Generic;
using System.Text;

namespace Scaffold.Application.Helpers
{
    public static class Date
    {
        public static string ToLongDateFormat(DateTime? dateTime, string timeZone)
        {
            if (!String.IsNullOrEmpty(timeZone))
            {
                TimeZoneInfo timeZoneId = TimeZoneConverter.TZConvert.GetTimeZoneInfo(timeZone);
                DateTime dateTimeVal = (dateTime == null ? DateTime.MinValue : dateTime.GetValueOrDefault());
                dateTimeVal = TimeZoneInfo.ConvertTimeFromUtc(dateTimeVal, timeZoneId);
                if (dateTimeVal == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return $"{dateTimeVal.ToShortDateString()} {dateTimeVal.ToShortTimeString()}";
                }
            } else
            {
                return $"{dateTime?.ToShortDateString()} {dateTime?.ToShortTimeString()}";
            }

        }
        public static string ToShortDateFormat(DateTime? dateTime, string timeZone)
        {
            if (!String.IsNullOrEmpty(timeZone))
            {
                TimeZoneInfo timeZoneId = TimeZoneConverter.TZConvert.GetTimeZoneInfo(timeZone);
                DateTime dateTimeVal = (dateTime == null ? DateTime.MinValue : dateTime.GetValueOrDefault());
                dateTimeVal = TimeZoneInfo.ConvertTimeFromUtc(dateTimeVal, timeZoneId);
                if (dateTimeVal == DateTime.MinValue)
                {
                    return "";
                }
                else
                {
                    return dateTimeVal.ToString("MM/dd/yyyy");
                }
            }
            else
            {
                return dateTime?.ToString("MM/dd/yyyy");
            }

        }
        public static bool IsDateBeforeOrToday(string input, string timeZone)
        {
            bool result = true;

            if (input != null)
            {
                string dTCurrent = Helpers.Date.ToShortDateFormat(DateTime.UtcNow, timeZone);
                int currentDateValues = Convert.ToInt32(dTCurrent.Replace("/", ""));
                int inputDateValues = Convert.ToInt32(input.Replace("/", ""));

                result = inputDateValues <= currentDateValues;
            }
            else
            {
                result = true;
            }

            return result;
        }
        public static string ToLongDateFormat(DateTime? dateTime)
        {
            return ToLongDateFormat(dateTime, String.Empty);
        }
    }
}
