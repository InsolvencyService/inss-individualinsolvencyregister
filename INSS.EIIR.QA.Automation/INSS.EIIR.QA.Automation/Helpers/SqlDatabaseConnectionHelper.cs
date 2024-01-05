using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;



namespace INSS.EIIR.QA.Automation.Helpers
{
    public class SqlDatabaseConncetionHelper
    {
        public static int ExecuteSqlCommand(string queryToExecute, string connectionString)
        {
            SqlConnection databaseConnection = new SqlConnection(connectionString);
            databaseConnection.Open();
            SqlCommand command = new SqlCommand(queryToExecute, databaseConnection);
            int result = command.ExecuteNonQuery();
            databaseConnection.Close();
            return result;
        }

        public static void ExecuteDeleteSqlCommand(string queryToExecute, string connectionString)
        {
            SqlConnection databaseConnection = new SqlConnection(connectionString);
            SqlDataAdapter adapter = new SqlDataAdapter();
            databaseConnection.Open();
            SqlCommand command = new SqlCommand(queryToExecute, databaseConnection);
            adapter.DeleteCommand = new SqlCommand(queryToExecute, databaseConnection);
            adapter.DeleteCommand.ExecuteNonQuery();
            command.Dispose();
            databaseConnection.Close();
        }

        public static List<object[]> ReadDataFromDataBase(string queryToExecute, string connectionString)
        {
            try
            {
                using SqlConnection databaseConnection = new SqlConnection(connectionString);
                databaseConnection.Open();
                SqlCommand command = new SqlCommand(queryToExecute, databaseConnection);
                SqlDataReader dataReader = command.ExecuteReader();

                List<object[]> result = new List<object[]>();
                while (dataReader.Read())
                {
                    object[] items = new object[100];
                    dataReader.GetValues(items);
                    result.Add(items);
                }
                return result;
            }
            catch (Exception exception)
            {
                throw new Exception($"Exception occurred while executing SQL query\n Exception: {exception}");
            }
        }

        public static void UpdateSqlCommand(string queryToExecute, string connectionString)
        {
            SqlConnection databaseConnection = new SqlConnection(connectionString);
            databaseConnection.Open();
            SqlCommand sqlCommand = new SqlCommand(queryToExecute, databaseConnection);
            sqlCommand.ExecuteNonQuery();
            databaseConnection.Close();
        }
        public static int ExecuteStoreProc(string storedProcName, string connectionString)
        {
            SqlConnection databaseConnection = new SqlConnection(connectionString);
            databaseConnection.Open();
            SqlCommand command = new SqlCommand(storedProcName, databaseConnection)
            {
                CommandType = CommandType.StoredProcedure
            };
            command.Parameters.AddWithValue("@InputFile", "C:\\Temp\\RegistrationsValidData.csv");
            int result = command.ExecuteNonQuery();
            databaseConnection.Close();
            return result;
        }
        public static void ExecuteSqlFile(string queryToExecute, string connectionString)
        {
            SqlConnection databaseConnection = new SqlConnection(connectionString);
            databaseConnection.Open();
            SqlCommand command = new SqlCommand(queryToExecute, databaseConnection);
            command.ExecuteNonQuery();
            databaseConnection.Close();
        }
       
        public static List<long> GetUlnsFromDatabse (string queryToExecute, string connectionString)
        {
            try
            {
                using SqlConnection databaseConnection = new SqlConnection(connectionString);
                databaseConnection.Open();
                SqlCommand command = new SqlCommand(queryToExecute, databaseConnection);
                SqlDataReader dataReader = command.ExecuteReader();

                List<long> result = new List<long>();
                while (dataReader.Read())
                {
                    var Uln = dataReader.GetValue(0);
                    result.Add((long)Uln);
                }
                return result;
            }
            catch (Exception exception)
            {
                throw new Exception($"Exception occurred while executing SQL query\n Exception: {exception}");
            }
        }
        public static List<object[]> GetFieldValue (string queryToExecute, string connectionString)
        {
            try
            {
                using SqlConnection databaseConnection = new SqlConnection(connectionString);
                databaseConnection.Open();
                SqlCommand command = new SqlCommand(queryToExecute, databaseConnection);
                SqlDataReader dataReader = command.ExecuteReader();
                var result = new List<object[]>();
                while (dataReader.Read())
                {
                    //var record = dataReader.GetValue(0);
                    var record = new object[dataReader.FieldCount]; 
                    dataReader.GetValues(record);
                    //var data = record;
                    result.Add(record);
                }

                return result;
            }
            catch (Exception e)
            {
                throw new Exception($"Exception occurred while executing SQL query\n Exception: {e}");
            }
        }
    }
}
