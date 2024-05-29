using Car_Connect.exception;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Car_Connect.util
{
    public class DatabaseContext
    {
        private static string connectionString = "Data Source=DESKTOP-TC25K55\\SQLEXPRESS;Initial Catalog=Car_Connect;Integrated Security=True;";

        public static SqlConnection GetSqlConnection()
        {
            SqlConnection connection = new SqlConnection(connectionString);

            try
            {
                connection.Open();
                //Console.WriteLine("\t*Database connection established successfully.");
                return connection;
            }
            catch (SqlException ex)
            {
                throw new DatabaseConnectionException("Error establishing database connection!" + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

    }
}
