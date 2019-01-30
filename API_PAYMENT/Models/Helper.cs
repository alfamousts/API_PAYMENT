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

        public Boolean CheckIP(string ip, string kodeInst)
        {
            Boolean result;
            Util util = new Util();
            //util.ConnectToApplicationDbase();
            string sql;

            sql = "SELECT 1 FROM IPMAP with (nolock) WHERE IP_ADDRESS='" + ip + "' AND INSTITUTION_CODE = '" + kodeInst + "'";
            DataTable dt = util.setDataTable(sql);

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
            Util util = new Util();
            //util.ConnectToApplicationDbase();

            string sql;

            sql = "SELECT " + paramName + " FROM INSTITUTION with (nolock) WHERE INSTITUTION_CODE = '" + kodeInst + "'";
            DataTable dt = util.setDataTable(sql);

            return dt;
        }

        public Boolean featureCek(string kodeInst, string featureCode)
        {
            Boolean result;
            Util util = new Util();
            //util.ConnectToApplicationDbase();

            string sql;

            sql = "SELECT * FROM FEATUREMAP with (nolock) WHERE INSTITUTION_CODE = '" + kodeInst + "' AND FEATURE_CODE = '"+ featureCode +"'";
            DataTable dt = util.setDataTable(sql);
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