using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_PAYMENT.Models
{
    /// <summary>
    /// Models for API response code and descriptions
    /// </summary>
    public class ResponseCodeModels
    {
        /// <summary>
        /// get response code description
        /// </summary>
        /// <param name="responseCode"></param>
        /// <returns>string response desriptions</returns>
        public static string GetResponseDescription(string responseCode)
        {
            switch (responseCode)
            {
                case "0005":
                    return "Validasi Sukses";
                case "0006":
                    return "Username (institution code) must not be empty";
                case "0007":
                    return "Password (institution key) must not be empty";
                case "0008":
                    return "Invalid username (institution code) or password (institution key)";
                case "0009":
                    return "IP address not allowed";                

                case "81":
                    return "Throw an exception";

                default:
                    return "";
            }
        }
    }
}