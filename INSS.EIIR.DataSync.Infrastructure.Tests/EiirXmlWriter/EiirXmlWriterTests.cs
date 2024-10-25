using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Infrastructure.Sink.XML;


namespace INSS.EIIR.DataSync.Infrastructure.Tests.EiirXmlWriter
{
    public class EiirXmlWriterTests
    {
        public EiirXmlWriterTests()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }
        

        [Theory]
        [MemberData(nameof(EiirXmlWriterTestsData.GetEiirXmlWriterData), MemberType = typeof(EiirXmlWriterTestsData))]
        public async Task EiirXMLWriter_IndividualDetails(InsolventIndividualRegisterModel model, string expected)
        {


            // arrange
            
            var xmlStream = new MemoryStream();

            // act
            IirXMLWriterHelper.WriteIirIndividualDetailsToStream(model, ref xmlStream);

            // assert
            xmlStream.Seek(0, SeekOrigin.Begin);    

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            using (XmlReader reader1 = XmlReader.Create(xmlStream, settings))
            {
                reader1.Read();
                XElement? elt = XNode.ReadFrom(reader1) as XElement;

                using (XmlReader reader2 = XmlReader.Create(new StringReader(expected), settings))
                {
                    reader2.Read();
                    XElement? elt2 = XNode.ReadFrom(reader2) as XElement;
                    Assert.True(XNode.DeepEquals(elt, elt2.XPathSelectElements(elt.Name.LocalName).First()));
                }
            }               
        }

        [Theory]
        [MemberData(nameof(EiirXmlWriterTestsData.GetEiirCaseDetailsXmlWriterData), MemberType = typeof(EiirXmlWriterTestsData))]
        public async Task EiirXMLWriter_CaseDetails(InsolventIndividualRegisterModel model, string expected)
        {
            // arrange
            var xmlStream = new MemoryStream();

            // act
            IirXMLWriterHelper.WriteIirCaseDetailsToStream(model, ref xmlStream);

            // assert
            xmlStream.Seek(0, SeekOrigin.Begin);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            using (XmlReader reader1 = XmlReader.Create(xmlStream, settings))
            {
                reader1.Read();
                XElement? elt = XNode.ReadFrom(reader1) as XElement;

                using (XmlReader reader2 = XmlReader.Create(new StringReader(expected), settings))
                {
                    reader2.Read();
                    XElement? elt2 = XNode.ReadFrom(reader2) as XElement;
                    Assert.True(XNode.DeepEquals(elt, elt2.XPathSelectElements(elt.Name.LocalName).First()));
                }
            }
        }

        
        [Theory]
        [MemberData(nameof(EiirXmlWriterTestsData.GetEiirBktRestrictionXmlWriterData), MemberType = typeof(EiirXmlWriterTestsData))]
        public async Task EiirXMLWriter_BKTRestrictionDetails(InsolventIndividualRegisterModel model, string expected)
        {
            // arrange
            var xmlStream = new MemoryStream();

            // act
            IirXMLWriterHelper.WriteIirBktRestrictionDetailsToStream(model, ref xmlStream);

            // assert
            xmlStream.Seek(0, SeekOrigin.Begin);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            using (XmlReader reader1 = XmlReader.Create(xmlStream, settings))
            {
                reader1.Read();
                XElement? elt = XNode.ReadFrom(reader1) as XElement;

                using (XmlReader reader2 = XmlReader.Create(new StringReader(expected), settings))
                {
                    reader2.Read();
                    XElement? elt2 = XNode.ReadFrom(reader2) as XElement;
                    Assert.True(XNode.DeepEquals(elt, elt2.XPathSelectElements(elt.Name.LocalName).First()));
                }
            }
        }

        [Theory]
        [MemberData(nameof(EiirXmlWriterTestsData.GetEiirDroRestrictionXmlWriterData), MemberType = typeof(EiirXmlWriterTestsData))]
        public async Task EiirXMLWriter_DRORestrictionDetails(InsolventIndividualRegisterModel model, string expected)
        {
            // arrange
            var xmlStream = new MemoryStream();

            // act
            IirXMLWriterHelper.WriteIirDroRestrictionDetailsToStream(model, ref xmlStream);

            // assert
            xmlStream.Seek(0, SeekOrigin.Begin);

            XmlReaderSettings settings = new XmlReaderSettings();
            settings.IgnoreWhitespace = true;

            using (XmlReader reader1 = XmlReader.Create(xmlStream, settings))
            {
                reader1.Read();
                XElement? elt = XNode.ReadFrom(reader1) as XElement;

                using (XmlReader reader2 = XmlReader.Create(new StringReader(expected), settings))
                {
                    reader2.Read();
                    XElement? elt2 = XNode.ReadFrom(reader2) as XElement;
                    Assert.True(XNode.DeepEquals(elt, elt2.XPathSelectElements(elt.Name.LocalName).First()));
                }
            }
        }



    }
}
