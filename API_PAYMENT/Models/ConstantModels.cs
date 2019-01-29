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

        // Add Constant Here
        //Hanum, start API_PAYMENT Telkom
        public const string URLINQPAY_TELKOM = "http://10.107.11.108/poscgi/pswcgi/method=post&data=json/";

        public const string ChannelID_Telkom = "IBBISNIS";
        public const string ProductID_Telkom = "TELKOM";
        public const string Key_Telkom = "1234567890QWERTY";
        public const string SubProductINQ_Telkom = "301000";
        public const string SubProductPAY_Telkom = "401000";
        //Hanum, end API_PAYMENT Telkom
    }
}
