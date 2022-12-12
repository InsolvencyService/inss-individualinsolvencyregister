using INSS.EIIR.QA.Automation.TestFramework;
using INSS.EIIR.QA.Automation.Helpers;
using System;

using System.Collections.Generic;



namespace INSS.EIIR.QA.Automation.Data
{
    public class SqlQueries
    {
        private static readonly string ConnectionString = WebDriverFactory.Config["DBConnectionString"];

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

        public static void createCaseFeedbackData()
        {
            string DeleteSQL = "delete from CI_Case_Feedback";
            SqlDatabaseConncetionHelper.ExecuteSqlCommand(DeleteSQL, ConnectionString);
            
            string[] Organisation = { "Other", "Debt recovery agency", "Financial services", "Government department", "Mortgage provider", "Bank or building society", "Credit card issuer", "Credit reference agency", "Member of the public" };

            string[] CaseNo = { "366226", "600000021", "11010005" };

            int count = 0;
            foreach (string member in Organisation)
            {
                int count1 = 0;
                foreach (string CN in CaseNo)
                {

                    string SQLQuery1 = "insert into CI_Case_Feedback(FeedbackDate, CaseId, Message, ReporterFullname, ReporterEmailAddress, ReporterOrganisation, Viewed) values('2022-11-15'," + CaseNo[count1] +  ", 'Test Message', 'John Smith', 'jsmith@test.com','" + Organisation[count] + "', 0)";
                    SqlDatabaseConncetionHelper.ExecuteSqlCommand(SQLQuery1, ConnectionString);
                    count1 = count1+1;
                }
                count = count + 1;
            }
        }

        public static List<object[]> GetCaseFeedbackDetails(string Organisation, string Type)
        {
            string Type1;
            if (Type == "IVAs")
            {
                Type1 = "I";
            }
            else if (Type == "Bankruptcies")
            {
                Type1 = "B";
            }
            else
            {
                Type1 = "D";
            }

            string SQLQuery = "select CICF.ReporterFullname, CICF.ReporterEmailAddress, CICF.Viewed, CICF.ReporterOrganisation, CI.insolvency_type, CI.case_name, CICF.FeedbackDate, CICF.CaseId, cicf.Message, CI.insolvency_date from CI_Case_Feedback CICF, ci_case CI where CI.case_no = CICF.caseid";
            string SQLQuery1 = SQLQuery + " and CICF.ReporterOrganisation = '" + Organisation + "' and CI.insolvency_type = '" + Type1 + "'";
            List<object[]> result = SqlDatabaseConncetionHelper.GetFieldValue(SQLQuery1, ConnectionString);
            return result;
        }

        public static List<object[]> GetLatestCaseFeedbackRecord()
        {
            string SQLQuery = "SELECT TOP 1 FeedbackDate, CaseId, Message, ReporterFullname, ReporterEmailAddress, ReporterOrganisation FROM CI_Case_Feedback order by feedbackDate desc";
            List<object[]> result = SqlDatabaseConncetionHelper.GetFieldValue(SQLQuery, ConnectionString);
            return result;
        }

    }
}
