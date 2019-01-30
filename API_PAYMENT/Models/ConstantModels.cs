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

        public const string ChannelID_CCBRI = "IBBISNIS";
        public const string ProductID_CCBRI = "CCBRI";
        public const string Key_CCBRI = "IBBIZ000000000000001";
        public const string SubProductINQ = "301000";
        public const string SubProductPAY = "401000";

        //FIKRI, end API_PAYMENT CCBRI
        // Add Constant Here

    }
}
