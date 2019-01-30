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
                case "0010":
                    return "Referral number must not be empty";
                case "0011":
                    return "Referral number is already used";
                case "0012":
                    return "Referral number must be a number";
                case "0100":
                    return "Success";
                case "0200":
                    return "Success";
                
                case "81":
                    return "Throw an exception";

                //Hanum, start response code TELKOM
                case "0201":
                    return "Billing number must not be empty";
                case "0202":
                    return "Source account has not been registered";
                case "0203":
                    return "Billing number must be a number";
                case "0204":
                    return "Source account must be a number";
                case "0205":
                    return "Total amount must not be empty";
                case "0206":
                    return "Name must not be empty";
                case "0207":
                    return "Billing code must not be empty";
                case "0208":
                    return "Total amount must be a number";
                case "0209":
                    return "Total amount must not be a negative number";
                //Hanum, end response code TELKOM


                default:
                    return "";
            }
        }
    }
}