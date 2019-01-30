using System;
using System.Net.Http.Headers;
using System.Text;

namespace API_PAYMENT.Models
{
    /// <summary>
    /// Models for API Constant Param 
    /// </summary>
    public class ConstantModels
    {
        public ConstantModels()
        {
        }

        //FIKRI, start API_PAYMENT CCBRI
        public const string URLINQPAY = "http://10.107.11.108/poscgi/pswcgi/method=post&data=json";

        public const string ChannelID_CCBRI = "APIBILL";
        public const string ProductID_CCBRI = "CCBRI";
        public const string Key_CCBRI = "APIBILL1234567890";
        public const string SubProductINQ = "301000";
        public const string SubProductPAY = "401000";

        //FIKRI, end API_PAYMENT CCBRI
        // Add Constant Here
        public const string SUCCESSCODEINQ = "0100";
        public const string FAILEDCODEINQ = "0101";
        public const string EXCEPTIONCODEINQ = "81";
        public const string TIMEOUTCODEINQ = "0102";

        public const string SUCCESSCODEPAY = "0200";
        public const string FAILEDCODEPAY = "0201";
        public const string EXCEPTIONCODEPAY = "81";
        public const string TIMEOUTCODEPAY = "0202";

        public const string FORMATDATETIME = "dd-MM-yyyy HH:mm:ss";
        public const string FORMATDATE = "yyyyMMdd";
        public const string FORMATTIME = "HHmmss";

        //Hanum, start API_PAYMENT Telkom
        public const string URLINQPAY_TELKOM = "http://10.107.11.108/poscgi/pswcgi/method=post&data=json/";

        public const string ChannelID_Telkom = "APIBILL";
        public const string ProductID_Telkom = "TELKOM";
        public const string Key_Telkom = "APIBILL1234567890";
        public const string SubProductINQ_Telkom = "301000";
        public const string SubProductPAY_Telkom = "401000";

        public const string FeatureCode_Telkom = "00002";
        public const string FeatureCode_CC = "00001";
        //Hanum, end API_PAYMENT Telkom
    }
}
