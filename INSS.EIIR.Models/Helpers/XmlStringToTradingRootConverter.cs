using AutoMapper;
using INSS.EIIR.Models.CaseModels;
using System.Xml.Serialization;

namespace INSS.EIIR.Models.Helpers
{
    public class XmlStringToTradingRootConverter : ITypeConverter<string, Trading>
    {
        public Trading Convert(string source, Trading destination, ResolutionContext context)
        {
            var serializer = new XmlSerializer(typeof(Trading));

            if (source == null)
                return null;

            using (StringReader reader = new StringReader(source))
            {
                    return (Trading)serializer.Deserialize(reader);
            }
        }
    }
    public class XmlStringToTradingDetailsRootConverter : ITypeConverter<string, TradingDetails>
    {
        public TradingDetails Convert(string source, TradingDetails destination, ResolutionContext context)
        {
            var serializer = new XmlSerializer(typeof(TradingDetails));

            using (StringReader reader = new StringReader(source))
            {
                return (TradingDetails)serializer.Deserialize(reader);
            }
        }
    }
}
