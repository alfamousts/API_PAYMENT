using System;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Data.SqlClient;

namespace API_PAYMENT.Models
{
    public class Helper
    {
        public Helper() { }

        private static Object streamLocker = new Object();
        private masiril.kasihmas hajar = new masiril.kasihmas();
        Util util = new Util();

        public Boolean CheckIP(string ip, string kodeInst)
        {
            Boolean result;

            SqlCommand sqlCommand = new SqlCommand("SELECT 1 FROM IPMAP with (nolock) WHERE IP_ADDRESS = @IPaddress AND INSTITUTION_CODE = @institutionCode");
            sqlCommand.Parameters.Add("@IPaddress", SqlDbType.VarChar).Value = ip;
            sqlCommand.Parameters.Add("@institutionCode", SqlDbType.VarChar).Value = kodeInst;
            DataTable dt = util.setDataTable(sqlCommand);

            if (dt != null && dt.Rows.Count > 0)
            {
                result = true;
                return result;
            }
            else
            {
                result = false;
                return result;
            }
        }

        public DataTable GetParameterInstitusiDT(string kodeInst, string paramName)
        {
            SqlCommand sqlCommand = new SqlCommand("SELECT " + paramName + " FROM INSTITUTION with (nolock) WHERE INSTITUTION_CODE = @institutionCode");
            sqlCommand.Parameters.Add("@institutionCode", SqlDbType.VarChar).Value = kodeInst;
            DataTable dt = util.setDataTable(sqlCommand);

            return dt;
        }

        public Boolean FeatureCheck(string institutionCode, string featureCode)
        {
            Boolean result;

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM FEATUREMAP WITH (NOLOCK) WHERE INSTITUTION_CODE = @institutionCode AND FEATURE_CODE = @featureCode");
            sqlCommand.Parameters.Add("@institutionCode", SqlDbType.VarChar).Value = institutionCode;
            sqlCommand.Parameters.Add("@featureCode", SqlDbType.VarChar).Value = featureCode;
            DataTable dt = util.setDataTable(sqlCommand);

            if (dt!= null && dt.Rows.Count > 0)
            {
                result = true;
                return result;
            }
            else
            {
                result = false;
                return result;
            }
        }

        public string GetSourceAccount(string institutionCode, string featureCode)
        {
            string result;

            SqlCommand sqlCommand = new SqlCommand("SELECT TOP(1) SOURCE_ACCOUNT FROM FEATUREMAP WITH (NOLOCK) WHERE INSTITUTION_CODE = @institutionCode AND FEATURE_CODE = @featureCode");
            sqlCommand.Parameters.Add("@institutionCode", SqlDbType.VarChar).Value = institutionCode;
            sqlCommand.Parameters.Add("@featureCode", SqlDbType.VarChar).Value = featureCode;
            DataTable dt = util.setDataTable(sqlCommand);

            if (dt != null && dt.Rows.Count > 0)
            {
                result = (String.IsNullOrEmpty(dt.Rows[0]["SOURCE_ACCOUNT"].ToString().Trim()) ? "" : (dt.Rows[0]["SOURCE_ACCOUNT"].ToString().Trim()).PadLeft(15, '0'));
                return result;
            }
            else
            {
                result = "";
                return result;
            }
        }

        public void InsertActivityLog(string institutionCode, string institutionKey, string urlHit, string IP, string datetime, string method, string request)
        {
            string sql = "INSERT INTO ACTIVITYLOG ([INSTITUTION_CODE],[INSTITUTION_KEY],[URL_HIT],[IP],[DATETIME],[METHOD],[REQUEST]) " +
                  "VALUES (@institutionCode, @institutionKey, @urlHit, @IP, @datetime, @method, @request)";

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = sql;
            sqlCommand.Parameters.Add("@institutionCode", SqlDbType.VarChar).Value = institutionCode;
            sqlCommand.Parameters.Add("@institutionKey", SqlDbType.VarChar).Value = institutionKey;
            sqlCommand.Parameters.Add("@urlHit", SqlDbType.VarChar).Value = urlHit;
            sqlCommand.Parameters.Add("@IP", SqlDbType.VarChar).Value = IP;
            sqlCommand.Parameters.Add("@datetime", SqlDbType.VarChar).Value = datetime;
            sqlCommand.Parameters.Add("@method", SqlDbType.VarChar).Value = method;
            sqlCommand.Parameters.Add("@request", SqlDbType.VarChar).Value = (request.Replace("\n", "")).Replace("\t", "").ToString();
            util.ExecuteSqlCommand(sqlCommand);
        }

        public string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }

        public void logging(string instFolder, string filename, string data)
        {
            try
            {
                lock (streamLocker)
                {
                    string fulldir = @"D:\LOGGING\API_PAYMENT\" + DateTime.Now.ToString("yyyyMMdd") + @"\" + instFolder;
                    string fullpath = fulldir + @"\" + filename + ".txt";
                    if (!Directory.Exists(fulldir))
                        Directory.CreateDirectory(fulldir);
                    StreamWriter logwriter = new StreamWriter(new FileStream(fullpath, FileMode.Append, FileAccess.Write));
                    logwriter.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff"));
                    logwriter.WriteLine(data);
                    logwriter.WriteLine("-EOD");
                    logwriter.Close();
                }
            }
            catch(Exception ex) { }
        }
    }
}
