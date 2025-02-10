

using AutoMapper;
using INSS.EIIR.Models.IndexModels;
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
        [InlineData("Macdonald", "Macdonald James Stuart", "Macdonald")]
        [InlineData("Macdonald", "Macdonald James Stuart,Dove James", "Macdonald Dove")]
        [InlineData("Macdonald", ",  Macdonald James Stuart, Dove James", "Macdonald Dove")]
        [InlineData("Macdonald", ",  MACDONALD James Stuart, Dove James", "Macdonald Dove")]

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
        [InlineData(null, "Macdonald James", "James")]
        [InlineData("John Scott", null, "John Scott")]
        [InlineData("John Scott", "No OtherNames Found", "John Scott")]
        [InlineData("John Scott", "", "John Scott")]
        [InlineData("John Scott", " ", "John Scott")]
        [InlineData("John Scott", "Macdonald James Stuart", "John Scott James Stuart")]
        [InlineData("John Scott", "Macdonald James Stuart,Dove Jim Scott", "John Scott James Stuart Jim")]
        [InlineData("John Scott", ",  Macdonald James Stuart, Dove James", "John Scott James Stuart")]
        [InlineData("John", ",  MACDONALD James Stuart, Dove James", "John James Stuart")]

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


    }
}
