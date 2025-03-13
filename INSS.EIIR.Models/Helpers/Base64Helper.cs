using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace INSS.EIIR.Models.Helpers
{
    public static class Base64Helper
    {

        public static string Base64Encode(this string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes).TrimEnd('=').Replace('+', '-').Replace('/','_');
        }

        public static string Base64Decode(this string base64EncodedData)
        {

            base64EncodedData = base64EncodedData.Replace('_', '/').Replace('-', '+');
            switch (base64EncodedData.Length % 4)
            {
                case 2:
                    base64EncodedData += "==";
                    break;
                case 3:
                    base64EncodedData += "=";
                    break;
            }

            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

    }
}
