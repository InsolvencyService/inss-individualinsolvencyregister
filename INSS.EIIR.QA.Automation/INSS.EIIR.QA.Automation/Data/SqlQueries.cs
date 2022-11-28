using INSS.EIIR.QA.Automation.TestFramework;
using INSS.EIIR.QA.Automation.Helpers;
using System;

using System.Collections.Generic;



namespace INSS.EIIR.QA.Automation.Data
{
    public class SqlQueries
    {
        private static readonly string ConnectionString = WebDriverFactory.Config["DBConnectionString"];

        public static void Test1(string ULN)
        {
            var getPathwayId = "select Id from TqRegistrationPathway where TqRegistrationProfileId in (select Id from tqregistrationprofile where uniquelearnernumber = " + ULN + ")";
            var pathwayID = SqlDatabaseConncetionHelper.ReadDataFromDataBase(getPathwayId, ConnectionString);
            int pathwayID1 = Convert.ToInt32(pathwayID[0][0]);
            var updateAcademicYearSQL = "update TqRegistrationPathway set AcademicYear = 2021 where Id = " + pathwayID1;
            SqlDatabaseConncetionHelper.ExecuteSqlCommand(updateAcademicYearSQL, ConnectionString);
        }

        public static String Test2(string TLevel, string CoreGrade, string SpecialismGrade)
        {
            var SQLQuery1 = "Select tlup.Value from OverallGradeLookup og join TlPathway tp on og.TlPathwayId=tp.Id join TlLookup tl on og.TlLookupCoreGradeId=tl.id join TlLookup tlu on og.TlLookupSpecialismGradeId=tlu.id join TlLookup tlup on og.TlLookupOverallGradeId=tlup.id where ";
            var SQLQuery2 = "tp.name = '" + TLevel + "' and tl.Value = '" + CoreGrade + "' and tlu.Value = '" + SpecialismGrade + "' order by tp.Name";
            var SQLQuery3 = SQLQuery1 + SQLQuery2;

            var OverallResult = SqlDatabaseConncetionHelper.ReadDataFromDataBase(SQLQuery3, ConnectionString);
            string OverallResult1 = Convert.ToString(OverallResult[0][0]);
            return OverallResult1;
        }

        public static List<object[]> GetAllSubscribersListFromDb()
        {
            string SQLQuery = "select subscriber_id, organisation_name, subscribed_to from subscriber_account order by subscribed_to desc";
            List<object[]> result = SqlDatabaseConncetionHelper.GetFieldValue(SQLQuery, ConnectionString);
            return result;
        }

        public static List<object[]> GetAllActiveSubscribersListFromDb()
        {
            string SQLQuery = "select subscriber_id, organisation_name, subscribed_to from subscriber_account where (subscribed_from < GETDATE()) AND(subscribed_to > GETDATE()) AND(account_active = 'Y') order by subscribed_to desc";
            List<object[]> result = SqlDatabaseConncetionHelper.GetFieldValue(SQLQuery, ConnectionString);
            return result;
        }

        public static List<object[]> GetAllInactiveSubscribersListFromDb()
        {
            string SQLQuery = "SELECT subscriber_id, organisation_name, subscribed_to from subscriber_account where subscriber_id not in (select subscriber_id from subscriber_account where(subscribed_from<GETDATE()) AND(subscribed_to > GETDATE()) AND(account_active = 'Y')) order by subscribed_to desc";
            List<object[]> result = SqlDatabaseConncetionHelper.GetFieldValue(SQLQuery, ConnectionString);
            return result;
        }

        public static List<object[]> GetSubscriberDetails(string SubscriberName)
        {
            string SQLQuery = "SELECT SA.subscriber_id, SA.organisation_name, SA.account_active, SA.subscribed_from, SA.subscribed_to, SAP.contact_forename, SAP.contact_surname, SAP.contact_address1, SAP.contact_address2, SAP.contact_city, SAP.contact_postcode,SAP.contact_telephone, SAP.contact_email,SAP.contact_country, SAP.organisation_type,SAP.application_date ";
            string SQLQuery2 = "from subscriber_account SA, subscriber_application SAP where SA.subscriber_id = SAP.subscriber_id and SAP.organisation_name = '";
            string SQLQuery3 = SQLQuery + SQLQuery2 + SubscriberName + "'";

            List<object[]> result = SqlDatabaseConncetionHelper.GetFieldValue(SQLQuery3, ConnectionString);
            return result;
        }

        public static List<object[]> GetSubscriberEmailAddresses(string SubscriberName)
        {
            string SQLQuery = "select SC.email_address from subscriber_account SA, subscriber_contact SC where SA.subscriber_id = SC.subscriber_id and SA.organisation_name = '" + SubscriberName + "'";
            List<object[]> result = SqlDatabaseConncetionHelper.GetFieldValue(SQLQuery, ConnectionString);
            return result;  
        }

        public static void DeleteSubscriber(string subscriber)
        {
            var SQLQuery1 = "SELECT * FROM subscriber_account where organisation_name = '" + subscriber + "'";
            var subscriberID = SqlDatabaseConncetionHelper.ReadDataFromDataBase(SQLQuery1, ConnectionString);
            if (subscriberID.Count > 0)
            {
                string SQLQuery2 = "delete from subscriber_contact where subscriber_id = '" + subscriberID[0][0] + "'";
                string SQLQuery3 = "delete from subscriber_application where subscriber_id = '" + subscriberID[0][0] + "'";
                string SQLQuery4 = "delete from subscriber_account where subscriber_id = '" + subscriberID[0][0] + "'";
                SqlDatabaseConncetionHelper.ExecuteSqlCommand(SQLQuery2, ConnectionString);
                SqlDatabaseConncetionHelper.ExecuteSqlCommand(SQLQuery3, ConnectionString);
                SqlDatabaseConncetionHelper.ExecuteSqlCommand(SQLQuery4, ConnectionString);
            }
        }





    }
}
