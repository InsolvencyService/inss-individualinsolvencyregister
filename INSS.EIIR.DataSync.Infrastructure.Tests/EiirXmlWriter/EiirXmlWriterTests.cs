using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.DataSync.Infrastructure.Sink.XML;
using INSS.EIIR.Models;


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


        /// <summary>
        /// 
        /// </summary>
        /// <param name="source1252Array">mixture of UTF8 Encoding and 1252 encoding</param>
        /// <param name="expected">UTF8 encoded bytes</param>
        [Theory]
        [InlineData(1, new byte[] { }, new byte[] { })]
        [InlineData(2, new byte[] { 0x6A,0x6F, 0x68, 0x6E }, new byte[] { 0x6A, 0x6F, 0x68, 0x6E })]  //john
        [InlineData(3, new byte[] { 0xF0, 0x93, 0x80, 0x80 }, new byte[] { 0xF0, 0x93, 0x80, 0x80 })]  //4-byte Egytiam Hieroglyph
        [InlineData(4, new byte[] { 0x6A, 0x6F, 0x68, 0x6E, 0xF0, 0x93, 0x80, 0x80 }, new byte[] { 0x6A, 0x6F, 0x68, 0x6E, 0xF0, 0x93, 0x80, 0x80 })] //4 byte UTF 8 at end
        [InlineData(5, new byte[] { 0xF0, 0x93, 0x80, 0x80 , 0x6A, 0x6F, 0x68, 0x6E }, new byte[] { 0xF0, 0x93, 0x80, 0x80, 0x6A, 0x6F, 0x68, 0x6E })] //4 byte UTF 8 at start
        [InlineData(6, new byte[] { 0x6A, 0x6F, 0x68, 0x6E, 0xF0, 0x93, 0x80 }, new byte[] { 0x6A, 0x6F, 0x68, 0x6E, 0xC3, 0xB0, 0xE2, 0x80, 0x9C, 0xE2, 0x82, 0xAC })] //truncated 4 byte at end
        [InlineData(7, new byte[] { 0xF0, 0x93, 0x80, 0x6A, 0x6F, 0x68, 0x6E }, new byte[] { 0xC3, 0xB0, 0xE2, 0x80, 0x9C, 0xE2, 0x82, 0xAC, 0x6A, 0x6F, 0x68, 0x6E  })] //truncated 4 byte at start
        [InlineData(8, new byte[] { 0x6A, 0x6F, 0x99, 0x68, 0x6E }, new byte[] { 0x6A, 0x6F, 0xE2, 0x84, 0xA2, 0x68, 0x6E })] // 1252 TM in middle
        [InlineData(9, new byte[] { 0x99, 0x6A, 0x6F, 0x68, 0x6E }, new byte[] { 0xE2, 0x84, 0xA2, 0x6A, 0x6F, 0x68, 0x6E })] // 1252 TM at start
        [InlineData(10, new byte[] { 0x6A, 0x6F, 0x68, 0x6E, 0x99 }, new byte[] { 0x6A, 0x6F, 0x68, 0x6E, 0xE2, 0x84, 0xA2 })] // 1252 TM at end
        [InlineData(11, new byte[] { 0x6A, 0x99, 0x6F, 0x68, 0xE2, 0x80, 0x99, 0x6E }, new byte[] { 0x6A, 0xE2, 0x84, 0xA2, 0x6F, 0x68, 0xE2, 0x80, 0x99, 0x6E })] // 1252 and UTF8 encoding present
        [InlineData(12, new byte[] { 0x99, 0x99, 0xE2, 0x80, 0x99, 0x6E }, new byte[] { 0xE2, 0x84, 0xA2, 0xE2, 0x84, 0xA2, 0xE2, 0x80, 0x99, 0x6E })] // 1252, 1252 UTF8 encoding present


        public void Fix1252_UTF8_Encoding(int number ,byte[] source1252Array, byte[] expected)
        {

            //Arrange
            var inputString = Encoding.GetEncoding(1252).GetString(source1252Array);

            //Act
            var convertedString = IirEncodingHelper.FixSQLEncoding(inputString);

            //Assert
            Assert.Equal(expected, Encoding.UTF8.GetBytes(convertedString));

        }




    }
}
