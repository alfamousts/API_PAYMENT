﻿using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Collections.Specialized;
using System.Text;

namespace API_PAYMENT.Models
{
    public class Util
    {
        private SqlConnection DBConns;
        private static Util _instance;

        public Util()
        { }

        public static Util Instance()
        {
            if (_instance == null)
                _instance = new Util();

            return _instance;
        }

        public void ConnectToApplicationDbase()
        {
            this.DBConns = new SqlConnection(ConfigurationManager.ConnectionStrings["BRI_APIPAYMENTConnectionString"].ConnectionString);
        }

        public DataTable setDataTable(string SqlCmd)
        {
            DataTable dtSetSQL = new DataTable();
            SqlDataAdapter DataAdapterSQL = new SqlDataAdapter();

            using (SqlConnection DBConn = new SqlConnection(ConfigurationManager.ConnectionStrings["BRI_APIPAYMENTConnectionString"].ConnectionString))
            {
                try
                {
                    DBConn.Open();
                    DataAdapterSQL.SelectCommand = new SqlCommand();
                    DataAdapterSQL.SelectCommand.Connection = DBConn;
                    DataAdapterSQL.SelectCommand.CommandText = SqlCmd;
                    DataAdapterSQL.Fill(dtSetSQL);
                    DBConn.Close();
                }
                catch (Exception ex)
                {
                    DBConn.Close();
                }
            }

            return dtSetSQL;

        }

        public DataTable setDataTable(SqlCommand SqlCmd)
        {
            SqlDataAdapter DataAdapterSQL = new SqlDataAdapter(SqlCmd);
            DataTable dtSetSQL = new DataTable();

            using (SqlConnection DBConn = new SqlConnection(ConfigurationManager.ConnectionStrings["BRI_APIPAYMENTConnectionString"].ConnectionString))
            {
                try
                {
                    DBConn.Open();
                    DataAdapterSQL.SelectCommand.Connection = DBConn;
                    DataAdapterSQL.Fill(dtSetSQL);
                    DBConn.Close();
                }
                catch
                {
                    DBConn.Close();
                }
            }

            return dtSetSQL;
        }

        public DataSet setDataSet(string SqlCmd)
        {
            SqlDataAdapter DataAdapterSQL = new SqlDataAdapter();
            DataSet dtSetSQL = new DataSet();
            DataAdapterSQL.SelectCommand = new SqlCommand();

            using (SqlConnection DBConn = new SqlConnection(ConfigurationManager.ConnectionStrings["BRI_APIPAYMENTConnectionString"].ConnectionString))
            {
                try
                {
                    DBConn.Open();
                    DataAdapterSQL.SelectCommand.Connection = DBConn;
                    DataAdapterSQL.SelectCommand.CommandText = SqlCmd;
                    DataAdapterSQL.Fill(dtSetSQL, SqlCmd);
                    DBConn.Close();
                }
                catch
                {
                    DBConn.Close();
                }
            }

            return dtSetSQL;
        }

        public int cmdSQL(string SqlCmd)
        {
            SqlCommand objCmd = new SqlCommand();
            int x = -1;

            using (SqlConnection DBConn = new SqlConnection(ConfigurationManager.ConnectionStrings["BRI_APIPAYMENTConnectionString"].ConnectionString))
            {
                try
                {
                    DBConn.Open();
                    objCmd.Connection = DBConn;
                    objCmd.CommandText = SqlCmd;
                    x = objCmd.ExecuteNonQuery();
                    DBConn.Close();
                }
                catch (Exception ex)
                {
                    DBConn.Close();
                }
            }

            return x;
        }

        public Int32 cmdSQLScalar(string SqlCmd)
        {
            SqlCommand objCmd = new SqlCommand();
            Int32 x = -1;
            Helper helper = new Helper();
            using (SqlConnection DBConn = new SqlConnection(ConfigurationManager.ConnectionStrings["BRI_APIPAYMENTConnectionString"].ConnectionString))
            {
                try
                {
                    DBConn.Open();
                    objCmd.Connection = DBConn;
                    objCmd.CommandText = SqlCmd;
                    helper.logging("", "APIPAYMENTERROR", SqlCmd);
                    x = Convert.ToInt32(objCmd.ExecuteScalar());
                    DBConn.Close();
                }
                catch (Exception ex)
                {
                    DBConn.Close();
                    helper.logging("", "APIPAYMENTERROR", ex.ToString());
                }
            }

            return x;
        }

        public int ExecuteSqlCommand(SqlCommand sqlCommand)
        {
            Helper helper = new Helper();
            int affectedRow = 0;
            using (SqlConnection conn = new SqlConnection(ConfigurationManager.ConnectionStrings["BRI_APIPAYMENTConnectionString"].ConnectionString))
            {
                conn.Open();
                try
                {
                    sqlCommand.Connection = conn;
                    affectedRow = sqlCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    helper.logging("", "APIPAYMENTERROR", ex.ToString());
                }
            }
            return affectedRow;
        }

        public int Exec_SP(SqlCommand sqlCommand, ref string ErrorMsg)
        {
            int affectedRow = 0;
            using (SqlConnection DBConn = new SqlConnection(ConfigurationManager.ConnectionStrings["BRI_APIPAYMENTConnectionString"].ConnectionString))
            {

                sqlCommand.Connection = DBConn;
                DBConn.Open();
                try
                {
                    affectedRow = sqlCommand.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    ErrorMsg = ex.Message;
                }
                DBConn.Close();
            }
            return affectedRow;
        }

        public string ToQueryString(NameValueCollection urlParams)
        {
            StringBuilder sb = new StringBuilder();

            foreach (String name in urlParams)
                sb.Append(String.Concat(name, "=", urlParams[name], "&"));

            if (sb.Length > 0)
                return sb.ToString(0, sb.Length - 1);

            return String.Empty;
        }

        public override String ToString()
        {
            String result = "";
            result = Newtonsoft.Json.JsonConvert.SerializeObject(this);
            return result;
        }
    }
}