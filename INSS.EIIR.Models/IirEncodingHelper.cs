using System.Text;

namespace INSS.EIIR.Models
{
    public static class IirEncodingHelper
    {


        /// <summary>
        ///Addresses previous 8 bit encoding issues with various data fields, may need to be modified for INSSight depending on their encoding
        ///Here we are using 1252 which is what EIIR database is configure for
        ///First scans through arrray looking for any single byte 1252 characters > 7F, converting them to UTF8, before reading all bytes as UTF8
        /// </summary>
        /// <param name="instr">Needs in general to be lower case to work given typically problematic characters </param>
        /// <returns></returns>
        public static string FixSQLEncoding(string instr)
        {
            if (instr == null)
                return instr;

            var windows1252Encoding = Encoding.GetEncoding(1252);
            var windows1252bytes = windows1252Encoding.GetBytes(instr);

            var utf8BytesList = new List<byte[]>();

            //Find all single bytes over 127 convert them to UTF8
            //Any sequence than looks like UTF8 encoding copy to output

            var byteLength = windows1252bytes.Length;
            var startIndex = 0;

            for (int i = 0; i < byteLength;)
            {
                var byteCount = 1;

                AdvancePastUTF8Characters(windows1252bytes, byteLength, ref i, ref byteCount);

                if (byteCount == 1)
                {
                    if (windows1252bytes[i] >= 0x80)
                    {
                        var utf8CharBytes = Encoding.Convert(windows1252Encoding, Encoding.UTF8, windows1252bytes, i, 1);

                        if (i - startIndex > 0)
                        {
                            utf8BytesList.Add(windows1252bytes[startIndex..i]);
                            startIndex = i;
                        }

                        utf8BytesList.Add(utf8CharBytes);
                        startIndex++;

                        i++;
                    }
                    else
                    {
                        i++;
                    }

                }
            }

            if (startIndex < byteLength)
            {
                utf8BytesList.Add(windows1252bytes[startIndex..byteLength]);
            }

            return Encoding.UTF8.GetString(utf8BytesList.SelectMany(i => i).ToArray());
        }

        private static void AdvancePastUTF8Characters(byte[] windows1252bytes, int byteLength, ref int i, ref int byteCount)
        {
            if (windows1252bytes[i] >= 0xF0)  //11110000
            {
                while (i <= byteLength - 4 && byteCount < 4)
                {
                    var aByte = windows1252bytes[i + byteCount];
                    if (aByte > 0x7F && aByte < 0xA0)
                        byteCount++;
                    else
                        break;
                }

                if (byteCount == 4)
                    i = i + 4;
                else
                    byteCount = 1;
            }
            //three byte utf8
            else if (windows1252bytes[i] >= 0xE0) //11100000
            {
                while (i <= byteLength - 3 && byteCount < 3)
                {
                    var aByte = windows1252bytes[i + byteCount];
                    if (aByte > 0x7F && aByte < 0xC0)
                        byteCount++;
                    else
                        break;
                }

                if (byteCount == 3)
                    i = i + 3;
                else
                    byteCount = 1;
            }
            //two byte utf8
            else if (windows1252bytes[i] >= 0xC0) //11000000
            {
                while (i <= byteLength - 2 && byteCount < 2)
                {
                    var aByte = windows1252bytes[i + byteCount];
                    if (aByte > 0x7F && aByte < 0xC0)
                        byteCount++;
                    else
                        break;
                }

                if (byteCount == 2)
                    i = i + 2;
                else
                    byteCount = 1;
            }
        }
    }
}