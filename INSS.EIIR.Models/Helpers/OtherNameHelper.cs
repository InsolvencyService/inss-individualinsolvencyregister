using INSS.EIIR.Models.CaseModels;
using INSS.EIIR.Models.Constants;
using System.Xml;
using System.Xml.Serialization;

namespace INSS.EIIR.Models.Helpers
{
    public static class OtherNameHelper
    {

        /// <summary>
        /// Returns any Othernames in "surname fornames, surname forenames" format in AlertativeNames XML or "No OtherNames Found"
        /// </summary>
        public static string GetOtherNames(string alternativeNames)
        {
            if (alternativeNames == Common.NoOtherNames || string.IsNullOrWhiteSpace(alternativeNames)) return Common.NoOtherNames;

            OtherNames othernames;

            try
            {
                var serializer = new XmlSerializer(typeof(OtherNames));

                using (TextReader reader = new StringReader(alternativeNames))
                {
                    othernames = (OtherNames)serializer.Deserialize(reader);
                }

            }
            catch (InvalidOperationException ex)
            {
                if (ex.InnerException is XmlException)
                    return "";

                throw;
            }

            return string.Join(", ", othernames.Names.Select(on => $"{on.Surname ?? ""} {on.Forenames ?? ""}".Trim()).ToArray());
        }
    }
}