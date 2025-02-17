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

        public static IEnumerable<object[]> ValidAliasData()
        {
            yield return new object[] { AliasNo_OtherNames_Found() };
            yield return new object[] { AliasNo_Other_Names_Found() };
            yield return new object[] { Alias_Valid_Xml() };
        }
        #endregion Valid Alias Data


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

        #region Valid RestrictionType Data

        public static InsolventIndividualRegisterModel RestrictionsType_Order()
        {
            return new InsolventIndividualRegisterModel()
            {
                restrictionsType = "Order"
            };
        }

        public static InsolventIndividualRegisterModel RestrictionsType_Undertaking()
        {
            return new InsolventIndividualRegisterModel()
            {
                restrictionsType = "Undertaking"
            };
        }

        public static InsolventIndividualRegisterModel RestrictionsType_Interim_Order()
        {
            return new InsolventIndividualRegisterModel()
            {
                restrictionsType = "Interim Order"
            };
        }

        //Doesn't match the Data Properties document - require because FCMC current providing ""
        public static InsolventIndividualRegisterModel RestrictionsType_EmptyString()
        {
            return new InsolventIndividualRegisterModel()
            {
                restrictionsType = ""
            };
        }

        //Be kind to INSSight currently supplying "Order Made" rather than "Order"
        public static InsolventIndividualRegisterModel RestrictionsType_OrderMade()
        {
            return new InsolventIndividualRegisterModel()
            {
                restrictionsType = "Order Made"
            };
        }


        public static IEnumerable<object[]> ValidRestrictionsTypeData()
        {
            //null restrictionsType
            yield return new object[] { InvalidData.NoData() };
            yield return new object[] { RestrictionsType_Order() };
            yield return new object[] { RestrictionsType_Undertaking() };
            yield return new object[] { RestrictionsType_Interim_Order() };
            yield return new object[] { RestrictionsType_EmptyString() };
            yield return new object[] { RestrictionsType_OrderMade() };
        }

        #endregion Valid RestrictionsType Data

        #region Valid Restriction Data

        public static InsolventIndividualRegisterModel ValidRestriction_false_null_null()
        {
            return new InsolventIndividualRegisterModel()
            {
                hasRestrictions = false,
                restrictionsType = null,
                restrictionsStartDate = null,
            };
        }

        //Be kind to INSSight
        public static InsolventIndividualRegisterModel ValidRestriction_false_space_null()
        {
            return new InsolventIndividualRegisterModel()
            {
                hasRestrictions = false,
                restrictionsType = "",
                restrictionsStartDate = null,
            };
        }

        public static InsolventIndividualRegisterModel ValidRestriction_true_order_date()
        {
            return new InsolventIndividualRegisterModel()
            {
                hasRestrictions = true,
                restrictionsType = "Order",
                restrictionsStartDate = new DateTime(2025,12,11,0,0,0)
            };
        }

        public static InsolventIndividualRegisterModel ValidRestriction_true_ordermade_date()
        {
            return new InsolventIndividualRegisterModel()
            {
                hasRestrictions = true,
                restrictionsType = "Order Made",
                restrictionsStartDate = new DateTime(2025, 12, 11, 0, 0, 0)
            };
        }

        public static IEnumerable<object[]> ValidRestrictionData()
        {
            yield return new object[] { ValidRestriction_false_null_null() };
            yield return new object[] { ValidRestriction_false_space_null() };
            yield return new object[] { ValidRestriction_true_order_date() };
            yield return new object[] { ValidRestriction_true_ordermade_date()};
        }

        #endregion Valid Restrictions Data

        #region Valid BKT Status Data

        public static InsolventIndividualRegisterModel ValidBKTStatus_CurrentBKT_DateAtEnd()
        {
            return new InsolventIndividualRegisterModel()
            {
                insolvencyType = "Bankruptcy",
                caseStatus = "Currently Bankrupt : Automatic Discharge  will be  20/11/2025"
            };
        }

        public static InsolventIndividualRegisterModel ValidBKTStatus_DischargedOn_DateAtEnd()
        {
            return new InsolventIndividualRegisterModel()
            {
                insolvencyType = "Bankruptcy",
                caseStatus = "Discharged On 20/11/2025"
            };
        }

        public static InsolventIndividualRegisterModel ValidBKTStatus_ANNULLED_DateAtEnd()
        {
            return new InsolventIndividualRegisterModel()
            {
                insolvencyType = "Bankruptcy",
                caseStatus = "ANNULLED (Order Revoked) On 20/11/2025"
            };
        }


        public static IEnumerable<object[]> ValidBKTStatusData()
        {
            yield return new object[] { ValidBKTStatus_CurrentBKT_DateAtEnd() };
            yield return new object[] { ValidBKTStatus_DischargedOn_DateAtEnd() };
            yield return new object[] { ValidBKTStatus_ANNULLED_DateAtEnd() };
        }

        #endregion Valid BKT Status Data

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

        #region Invalid RestrictionsType Data

        public static InsolventIndividualRegisterModel InvalidRestrictionsType()
        {
            return new InsolventIndividualRegisterModel()
            {
                restrictionsType = "john was here"
            };
        }

        public static IEnumerable<object[]> InvalidRestrictionsTypeData()
        {
            yield return new object[] { InvalidData.InvalidRestrictionsType() };
        }
        #endregion Invalid RestrictionsType Data

        #region Invalid Restrictions Data

        public static InsolventIndividualRegisterModel InvalidRestrictions_StartDate_noHasRestrictions()
        {
            return new InsolventIndividualRegisterModel()
            {
                restrictionsStartDate = new DateTime(2025, 12, 11, 0, 0, 0),
                hasRestrictions = false,
                restrictionsType = "Order"
            };
        }

        public static InsolventIndividualRegisterModel InvalidRestrictions_StartDate_noRestrictionType()
        {
            return new InsolventIndividualRegisterModel()
            {
                restrictionsStartDate = new DateTime(2025, 12, 11, 0, 0, 0),
                hasRestrictions = true,
                restrictionsType = null
            };
        }

        public static InsolventIndividualRegisterModel InvalidRestrictions_StartDate_noRestrictionType_noHasRestictions()
        {
            return new InsolventIndividualRegisterModel()
            {
                restrictionsStartDate = new DateTime(2025, 12, 11, 0, 0, 0),
                hasRestrictions = false,
                restrictionsType = null
            };
        }

        public static InsolventIndividualRegisterModel InvalidRestrictions_HasRestrictions_noType_noStartDate()
        {
            return new InsolventIndividualRegisterModel()
            {
                restrictionsStartDate = null,
                hasRestrictions = true,
                restrictionsType = null
            };
        }

        public static InsolventIndividualRegisterModel InvalidRestrictions_HasRestrictions_Type_noStartDate()
        {
            return new InsolventIndividualRegisterModel()
            {
                restrictionsStartDate = null,
                hasRestrictions = true,
                restrictionsType = "Order"
            };
        }

        public static InsolventIndividualRegisterModel InvalidRestrictions_HasRestrictions_noType_StartDate()
        {
            return new InsolventIndividualRegisterModel()
            {
                restrictionsStartDate = new DateTime(2025, 12, 11, 0, 0, 0),
                hasRestrictions = true,
                restrictionsType = null
            };
        }

        public static IEnumerable<object[]> InvalidRestrictionsData()
        {
            yield return new object[] { InvalidData.InvalidRestrictions_HasRestrictions_noType_noStartDate() };
            yield return new object[] { InvalidData.InvalidRestrictions_HasRestrictions_noType_StartDate() };
            yield return new object[] { InvalidData.InvalidRestrictions_HasRestrictions_Type_noStartDate() };
            yield return new object[] { InvalidData.InvalidRestrictions_StartDate_noHasRestrictions() };
            yield return new object[] { InvalidData.InvalidRestrictions_StartDate_noRestrictionType() };
            yield return new object[] { InvalidData.InvalidRestrictions_StartDate_noRestrictionType_noHasRestictions() };
        }
        #endregion Invalid Restrictions Data

        #region Invalid BKT Status Data

        public static InsolventIndividualRegisterModel InvalidBKTStatus_CurrentBKT_DateAtEnd()
        {
            return new InsolventIndividualRegisterModel()
            {
                insolvencyType = "Bankruptcy",
                caseStatus = "Currently Bankrupt : Automatic Discharge  will be  20/11/2025 "
            };
        }

        public static InsolventIndividualRegisterModel InvalidBKTStatus_DischargedOn_DateAtEnd()
        {
            return new InsolventIndividualRegisterModel()
            {
                insolvencyType = "Bankruptcy",
                caseStatus = "Discharged On 20/11/2025 "
            };
        }

        public static InsolventIndividualRegisterModel InvalidBKTStatus_ANNULLED_DateAtEnd()
        {
            return new InsolventIndividualRegisterModel()
            {
                insolvencyType = "Bankruptcy",
                caseStatus = "ANNULLED (Order Revoked) On 20/11/2025 "
            };
        }

        public static InsolventIndividualRegisterModel InvalidBKTStatus_CurrentBKT_DateAtEnd_US()
        {
            return new InsolventIndividualRegisterModel()
            {
                insolvencyType = "Bankruptcy",
                caseStatus = "Currently Bankrupt : Automatic Discharge  will be  11/20/2025"
            };
        }

        public static InsolventIndividualRegisterModel InvalidBKTStatus_DischargedOn_DateAtEnd_US()
        {
            return new InsolventIndividualRegisterModel()
            {
                insolvencyType = "Bankruptcy",
                caseStatus = "Discharged On 11/20/2025"
            };
        }

        public static InsolventIndividualRegisterModel InvalidBKTStatus_ANNULLED_DateAtEnd_US()
        {
            return new InsolventIndividualRegisterModel()
            {
                insolvencyType = "Bankruptcy",
                caseStatus = "ANNULLED (Order Revoked) On 11/20/2025"
            };
        }


        public static IEnumerable<object[]> InvalidBKTStatusData()
        {
            yield return new object[] { InvalidBKTStatus_CurrentBKT_DateAtEnd() };
            yield return new object[] { InvalidBKTStatus_DischargedOn_DateAtEnd() };
            yield return new object[] { InvalidBKTStatus_ANNULLED_DateAtEnd() };
            yield return new object[] { InvalidBKTStatus_CurrentBKT_DateAtEnd_US() };
            yield return new object[] { InvalidBKTStatus_DischargedOn_DateAtEnd_US() };
            yield return new object[] { InvalidBKTStatus_ANNULLED_DateAtEnd_US() };
        }


        #endregion Invalid BKT Status Data


    }

}
