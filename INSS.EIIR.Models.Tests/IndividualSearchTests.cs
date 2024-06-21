

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
    }
}
