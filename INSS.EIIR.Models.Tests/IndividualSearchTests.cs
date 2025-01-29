

using AutoMapper;
using INSS.EIIR.Models.IndexModels;
using INSS.EIIR.Models.Helpers;
using INSS.EIIR.Models.AutoMapperProfiles;


namespace INSS.EIIR.Models.Tests
{
    public  class IndividualSearchTests
    {

        [Theory]
        [InlineData("<No Trading Names Found>", "")]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData("john was here", "")]
        [InlineData("<Trading><TradingDetails><TradingName>Macd MOTORS</TradingName><TradingAddress>40 Whiteley Croft Rise, Otley, Leeds</TradingAddress></TradingDetails></Trading>", "Macd MOTORS")]
        [InlineData("<Trading><TradingDetails><TradingName></TradingName><TradingAddress>40 Whiteley Croft Rise, Otley, Leeds</TradingAddress></TradingDetails></Trading>", "")]
        [InlineData("<Trading><TradingDetails><TradingName>Jmacd Services</TradingName><TradingAddress>40 Whiteley Croft Rise, Otley, Leeds</TradingAddress></TradingDetails>" +
                                "<TradingDetails><TradingName>Jmacd Developments</TradingName><TradingAddress>40 Whiteley Croft Rise, Otley, Leeds</TradingAddress></TradingDetails></Trading>", "Jmacd Services,Jmacd Developments")]
        public void TradingNames_Propery(string tradingData, string expected)
        {

            //Arrange
            var model = new IndividualSearch();
            model.TradingData = tradingData;

            //Act
            //Assert
            Assert.Equal(expected, model.TradingNames);

        }

        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, "No OtherNames Found", null)]
        [InlineData(null, "", null)]
        [InlineData(null, " ", null)]
        [InlineData("Macdonald", null, "Macdonald")]
        [InlineData("Macdonald", "No OtherNames Found", "Macdonald")]
        [InlineData("Macdonald", "", "Macdonald")]
        [InlineData("Macdonald", " ", "Macdonald")]
        [InlineData("Macdonald", "<OtherNames><OtherName><Forenames>James Stuart</Forenames><Surname>Macdonald</Surname></OtherName></OtherNames>", "Macdonald")]
        [InlineData("Macdonald", "<OtherNames><OtherName><Forenames>James Stuart</Forenames><Surname>Macdonald</Surname></OtherName><OtherName><Forenames>James</Forenames><Surname>Dove</Surname></OtherName></OtherNames>", "Macdonald Dove")]
        [InlineData("Macdonald", "<OtherNames><OtherName><Forenames>James Stuart</Forenames><Surname>MACDONALD</Surname></OtherName><OtherName><Forenames>James</Forenames><Surname>Dove</Surname></OtherName></OtherNames>", "Macdonald Dove")]
        [InlineData("Macdonald", "<OtherNames><OtherName><Forenames>James</Forenames><Surname></Surname></OtherName></OtherNames>", "Macdonald")]
        [InlineData("Macdonald", "<OtherNames><OtherName><Forenames>James</Forenames><Surname/></OtherName></OtherNames>", "Macdonald")]
        [InlineData("Macdonald", "<OtherNames><OtherName><Forenames>James</Forenames><Surname> </Surname></OtherName></OtherNames>", "Macdonald")]
        [InlineData("Macdonald", "<OtherNames><OtherName><Forenames>James</Forenames></OtherName></OtherNames>", "Macdonald")]
        [InlineData("Macdonald Dove", "No OtherNames Found", "Macdonald Dove")]

        public void LastNamesSearchField_Property(string lastname, string otherNames, string expected)
        {

            //Arrange
            var model = new IndividualSearch();
            model.FamilyName = lastname;
            model.AlternativeNames = otherNames;

            //Act
            //Assert
            Assert.Equal(expected, model.LastNamesSearchField);

        }


        [Theory]
        [InlineData(null, null, null)]
        [InlineData(null, "No OtherNames Found", null)]
        [InlineData(null, "", null)]
        [InlineData(null, " ", null)]
        [InlineData(null, "<OtherNames><OtherName><Forenames>James</Forenames><Surname>Macdonald</Surname></OtherName></OtherNames>", "James")]
        [InlineData("John Scott", null, "John Scott")]
        [InlineData("John Scott", "No OtherNames Found", "John Scott")]
        [InlineData("John Scott", "", "John Scott")]
        [InlineData("John Scott", " ", "John Scott")]
        [InlineData("John Scott", "<OtherNames><OtherName><Forenames>James Stuart</Forenames><Surname>Macdonald</Surname></OtherName></OtherNames>", "John Scott James Stuart")]
        [InlineData("John Scott", "<OtherNames><OtherName><Forenames>James Stuart</Forenames><Surname>Macdonald</Surname></OtherName><OtherName><Forenames>Jim Scott</Forenames><Surname>Dove</Surname></OtherName></OtherNames>", "John Scott James Stuart Jim")]
        [InlineData("John Scott", "<OtherNames><OtherName><Forenames>James Stuart</Forenames><Surname>Macdonald</Surname></OtherName><OtherName><Forenames>James</Forenames><Surname>Dove</Surname></OtherName></OtherNames>", "John Scott James Stuart")]
        [InlineData("John", "<OtherNames><OtherName><Forenames>James Stuart</Forenames><Surname>MACDONALD</Surname></OtherName><OtherName><Forenames>James</Forenames><Surname>Dove</Surname></OtherName></OtherNames>", "John James Stuart")]
        [InlineData("John", "<OtherNames><Forenames></Forenames><Surname>Dove</Surname></OtherName></OtherNames>", "John")]
        [InlineData("John", "<OtherNames><Forenames/><Surname>Dove</Surname></OtherName></OtherNames>", "John")]
        [InlineData("John", "<OtherNames><Surname>Dove</Surname></OtherName></OtherNames>", "John")]

        public void ForeNamesSearchField_Property(string forenames, string otherNames, string expected)
        {

            //Arrange
            var model = new IndividualSearch();
            model.FirstName = forenames;
            model.AlternativeNames = otherNames;

            //Act
            //Assert
            Assert.Equal(expected, model.ForeNamesSearchField);

        }

        [Theory]
        [InlineData(null, "No OtherNames Found")]
        [InlineData("No OtherNames Found", "No OtherNames Found")]
        [InlineData("", "No OtherNames Found")]
        [InlineData(" ", "No OtherNames Found")]
        [InlineData("<OtherNames><OtherName><Forenames>James</Forenames><Surname>Macdonald</Surname></OtherName></OtherNames>", "Macdonald James")]
        [InlineData("<OtherNames><OtherName><Forenames>James Stuart</Forenames><Surname>Macdonald</Surname></OtherName></OtherNames>", "Macdonald James Stuart")]
        [InlineData("<OtherNames><OtherName><Forenames>James Stuart</Forenames><Surname>Macdonald</Surname></OtherName><OtherName><Forenames>Jim Scott</Forenames><Surname>Dove</Surname></OtherName></OtherNames>", "Macdonald James Stuart, Dove Jim Scott")]
        [InlineData("<OtherNames><OtherName><Forenames>James Stuart</Forenames><Surname>Macdonald</Surname></OtherName><OtherName><Forenames>James</Forenames><Surname>Dove</Surname></OtherName></OtherNames>", "Macdonald James Stuart, Dove James")]
        [InlineData("<OtherNames><OtherName><Forenames></Forenames><Surname>Dove</Surname></OtherName></OtherNames>", "Dove")]
        [InlineData("<OtherNames><OtherName><Forenames/><Surname>Dove</Surname></OtherName></OtherNames>", "Dove")]
        [InlineData("<OtherNames><OtherName><Surname>Dove</Surname></OtherName></OtherNames>", "Dove")]
        [InlineData("<OtherNames><OtherName><Forenames>John</Forenames><Surname></Surname></OtherName></OtherNames>", "John")]
        [InlineData("<OtherNames><OtherName><Forenames>John</Forenames><Surname/></OtherName></OtherNames>", "John")]
        [InlineData("<OtherNames><OtherName><Forenames>John</Forenames></OtherName></OtherNames>", "John")]

        public void OtherNames_Property(string otherNames, string expected)
        {

            //Arrange
            //Act
            //Assert
            Assert.Equal(expected, OtherNameHelper.GetOtherNames(otherNames));

        }

        [Theory]
        [InlineData("Frank", "RnJhbms")]
        [InlineData("smith", "c21pdGg")]
        [InlineData("Macdonald", "TWFjZG9uYWxk")]
        [InlineData("C & C Block Paving", "QyAmIEMgQmxvY2sgUGF2aW5n")]
        [InlineData("Macdona", "TWFjZG9uYQ")]
        public void SearchTermEncode(string term, string expected)
        {
            //Arrange
            //Act
            //Assert
            Assert.Equal(expected, term.Base64Encode());
        }

        [Theory]
        [InlineData("RnJhbms", "Frank")]
        [InlineData("c21pdGg", "smith")]
        [InlineData("TWFjZG9uYWxk", "Macdonald")]
        [InlineData("QyAmIEMgQmxvY2sgUGF2aW5n","C & C Block Paving")]
        [InlineData("TWFjZG9uYQ", "Macdona")]
        public void SearchDecodeEncode(string encodedTerm, string expected)
        {
            //Arrange
            //Act
            //Assert
            Assert.Equal(expected, encodedTerm.Base64Decode());
        }


    }
}
