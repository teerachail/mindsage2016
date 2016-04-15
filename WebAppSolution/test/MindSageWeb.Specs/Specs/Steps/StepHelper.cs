using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace MindSageWeb.Specs.Steps
{
    public static class StepHelper
    {
        const string NullKeyword = "NULL";
        const string EmptyValue = "Empty";

        public static string TryGetRowStringValue(this TableRow row, string keyword)
        {
            string DataString;
            row.TryGetValue(keyword, out DataString);
            return DataString;
        }

        public static int ConvertToInt(this TableRow row, string keyword)
        {
            string DataString;
            row.TryGetValue(keyword, out DataString);
            int value;
            int.TryParse(DataString, out value);
            return value;
        }
        public static int? ConvertToNullableInt(this TableRow row, string keyword)
        {
            string DataString;
            row.TryGetValue(keyword, out DataString);
            int value;
            if (int.TryParse(DataString, out value) == false) return null;
            return value;
        }

        public static double ConvertToDouble(this TableRow row, string keyword)
        {
            string DataString;
            row.TryGetValue(keyword, out DataString);
            double value;
            double.TryParse(DataString, out value);
            return value;
        }
        public static double? ConvertToNullableDouble(this TableRow row, string keyword)
        {
            string DataString;
            row.TryGetValue(keyword, out DataString);
            double value;
            if (double.TryParse(DataString, out value) == false) return null;
            return value;
        }

        public static DateTime ConvertToDateTime(this TableRow row, string keyword)
        {
            string DataString;
            row.TryGetValue(keyword, out DataString);
            DateTime value;
            DateTime.TryParse(DataString, out value);
            return value;
        }
        public static DateTime? ConvertToNullableDateTime(this TableRow row, string keyword)
        {
            string DataString;
            row.TryGetValue(keyword, out DataString);
            if (string.IsNullOrEmpty(DataString))
            {
                return null;
            }
            else
            {
                DateTime value;
                DateTime.TryParse(DataString, out value);
                return value;
            }

        }

        public static Boolean ConvertToBoolean(this TableRow row, string keyword)
        {
            string DataString;
            row.TryGetValue(keyword, out DataString);
            Boolean value;
            Boolean.TryParse(DataString, out value);
            return value;
        }

        public static string GetMockStrinValue(this string value)
        {
            if (string.IsNullOrEmpty(value)) return value;
            if (value.Equals(NullKeyword, StringComparison.CurrentCultureIgnoreCase)) return null;
            else if (value.Equals(EmptyValue, StringComparison.CurrentCultureIgnoreCase)) return string.Empty;
            return value;
        }
    }
}
