using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;
using System.Globalization;
using System.Net.Http.Headers;
using System.Net.Http;

namespace ESTORE.Client.Helpers
{
    public class Helpers
    {
        public static string FormatToPhilippinePeso(float price)
        {
            CultureInfo cultureInfo = new CultureInfo("en-PH");
            return price.ToString("C", cultureInfo);
        }
        public static string TruncateString(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
            {
                return text;
            }
            else
            {
                return text.Substring(0, maxLength) + "...";
            }
        }

        public static string TruncateName(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text) || text.Length <= maxLength)
            {
                return text;
            }
            else
            {
                return text.Substring(0, maxLength) + ".";
            }
        }

        public static string ToLongDateString(DateTime dateTime)
        {
            return dateTime.ToString("MMMM d, yyyy", CultureInfo.InvariantCulture);
        }

    }
}
