using INSS.EIIR.DataSync.Application.UseCase.SyncData.Model;
using INSS.EIIR.Models.Constants;

namespace INSS.EIIR.DataSync.Application.Tests.FakeData
{
    public class ValidData
    {
        public static InsolventIndividualRegisterModel Standard() 
        {
            Random r = new Random();

            return new InsolventIndividualRegisterModel()
            {
                caseNo = r.Next(100000000, 800000000),
                individualForenames = "Carl",
                individualSurname = "McIntyre"
            };
        }

        #region Valid Alias Data
        public static InsolventIndividualRegisterModel AliasNo_OtherNames_Found()
        {
            return new InsolventIndividualRegisterModel()
            {
                individualAlias = Common.NoOtherNames
            };
        }

        public static InsolventIndividualRegisterModel AliasNo_Other_Names_Found()
        {
            return new InsolventIndividualRegisterModel()
            {
                individualAlias = "No Other Names Found"
            };
        }

        public static InsolventIndividualRegisterModel Alias_Valid_Xml()
        {
            return new InsolventIndividualRegisterModel()
            {
                individualAlias = "<OtherNames><OtherName><Forenames>John Scott</Forenames><Surname>Macdonald</Surname></OtherName></OtherNames>"
            };
        }
        #endregion Valid Alias Data

        public static IEnumerable<object[]> ValidAliasData()
        {
            yield return new object[] { AliasNo_OtherNames_Found() };
            yield return new object[] { AliasNo_Other_Names_Found() };
            yield return new object[] { Alias_Valid_Xml() };
        }

        #region Valid TradingNames Data

        public static InsolventIndividualRegisterModel TradingNames_No_Trading_Names_Found()
        {
            return new InsolventIndividualRegisterModel()
            {
                tradingNames = Common.NoTradingNames
            };
        }

        public static InsolventIndividualRegisterModel TradingNamesValidXML_amp()
        {
            return new InsolventIndividualRegisterModel()
            {
                tradingNames = "<Trading><TradingDetails><TradingName>A &amp; SON</TradingName><TradingAddress>28 St Johns Terrace, Tawa</TradingAddress></TradingDetails></Trading>"
            };
        }

        public static IEnumerable<object[]> ValidTradingNamesData()
        {
            yield return new object[] { TradingNames_No_Trading_Names_Found() };
            yield return new object[] { TradingNamesValidXML_amp() };
        }

        #endregion Valid TradingNames Data

    }

    public class InvalidData
    {
        public static InsolventIndividualRegisterModel NegativeId()
        {
            return new InsolventIndividualRegisterModel()
            {
                caseNo = -1,
                individualForenames = "Carl",
                individualSurname = "McIntyre"
            };
        }

        public static InsolventIndividualRegisterModel NoData()
        {
            return new InsolventIndividualRegisterModel();
        }

        #region Invalid Alias Data

        public static InsolventIndividualRegisterModel AliasNotXML()
        {
            return new InsolventIndividualRegisterModel()
            {
                individualAlias = "john was here"
            };
        }

        public static IEnumerable<object[]> InvalidAliasData()
        {
            yield return new object[] { InvalidData.NoData() };
            yield return new object[] { InvalidData.AliasNotXML() };
        }

        #endregion Invalid Alias Data

        #region Invalid TradingNames Data


        public static InsolventIndividualRegisterModel TradingNamesNotValidXML()
        {
            return new InsolventIndividualRegisterModel()
            {
                tradingNames = "john was here"
            };
        }

        public static InsolventIndividualRegisterModel TradingNamesNotValidXML_amp()
        {
            return new InsolventIndividualRegisterModel()
            {
                tradingNames = "<Trading><TradingDetails><TradingName>A & SON</TradingName><TradingAddress>28 St Johns Terrace, Tawa</TradingAddress></TradingDetails></Trading>"
            };
        }

        public static IEnumerable<object[]> InvalidTradingNamesData()
        {
            yield return new object[] { InvalidData.NoData() };
            yield return new object[] { InvalidData.TradingNamesNotValidXML() };
            yield return new object[] { InvalidData.TradingNamesNotValidXML_amp() };
        }

        #endregion Invalid TradingNames Data

    }

}
