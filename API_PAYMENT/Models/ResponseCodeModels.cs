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
                case "0012":
                    return "Reference must not be empty";
                case "0013":
                    return "Reference is already used";

                case "0100":
                    return "Inquiry Success";
                case "0200":
                    return "Payment Success";
                
                case "0011":
                    return "Feature not allowed";

                case "81":
                    return "Throw an exception";

                case "0102":
                    return "Inquiry timeout";
                case "0202":
                    return "Payment timeout";

                //Fikri, start response code TELKOM
                case "0104":
                    return "Card number must be a number";
                case "0105":
                    return "Total amount must be fillef";
                case "0106":
                    return "Card Name must be filled";
                case "0108":
                    return "Total amount must be a number";
                case "0109":
                    return "Total amount must must be positive number";
                case "0110":
                    return "Card Number must be filled";
                case "0111":
                    return "Source account has not been registered, please contact the administrator";
                //Fikri, end response code TELKOM

                //Hanum, start response code TELKOM
                case "0204":
                    return "Billing number must be a number";
                case "0205":
                    return "Total amount must not be empty";
                case "0206":
                    return "Billing code must not be empty";
                case "0207":
                    return "Total amount must be a number";
                case "0208":
                    return "Total amount must not be a negative number";
                case "0209":
                    return "Billing number must not be empty";
                case "0210":
                    return "Source account has not been registered";
                //Hanum, end response code TELKOM

                //Hanum, start response code PSW
                case "0301":
                    return "Your IP and key has not been registered";
                case "0302":
                    return "This feature cannot be used";
                case "0303":
                    return "Connection lost";
                case "0304":
                    return "Account not found";
                case "0305":
                    return "Insufficient balance";
                case "0306":
                    return "Unprocess, please check your account statement";
                case "0307":
                    return "Invoice not found";
                case "0308":
                    return "Duplicate transaction sequence";
                case "0309":
                    return "Account is not active";
                case "0310":
                    return "Passive account";
                case "0311":
                    return "Cardlink end of day";
                case "0312":
                    return "Invalid credit card number";
                case "0313":
                    return "BNI credit card is not registered";
                case "0314":
                    return "Invalid credit card format";
                //Hanum, end response code PSW

                default:
                    return "";
            }
        }

        //Hanum, start response code dari PSW
        public static string GetResponseCodePSW(string responseCode)
        {
            switch (responseCode)
            {
                case "0X":
                    return "0301"; //IP & Key Anda Belum Terdaftar
                case "99":
                    return "0302"; //Fitur Ini Tidak Dapat Digunakan
                case "91":
                    return "0303"; //Koneksi Terputus (3rd Party)
                case "Q1":
                    return "0303"; //Koneksi Terputus (PSW)
                case "53":
                    return "0304"; //Rekening Tidak Ditemukan
                case "51":
                    return "0305"; //Saldo Tidak Cukup
                case "Q4":
                    return "0306"; //Transaksi Timeout (PSW)
                case "68":
                    return "0306"; //Transaksi Timeout (3rd Party)
                //Hanum, start abnormal case Telkom
                case "88":
                    return "0307"; //Data Not Found
                //Hanum, end
                case "86":
                    return "0308"; //Duplicate Sequence Transaction (Brinets)
                case "93":
                    return "0308"; //Duplicate Sequence Transaction (PSW)
                case "NF":
                    return "0309"; //Rekening Close
                case "NH":
                    return "0310"; //Rekening Pasif

                //Fikri, start abnormal case CC
                //CC BRI
                case "N1":
                    return "0311"; //Cardlink end of day
                case "N2":
                    return "0312"; //Invalid credit card number
                //CC Bank Lain
                case "06":
                    return "0313"; //BNI credit card is not registered
                case "12":
                    return "0314"; //Invalid credit card format
                //Fikri, end

                default:
                    return "";
            }
        }
        //Hanum, end response code dari PSW
    }
}