using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_PAYMENT.Models
{
    public class CreditCardValidation
    {

        public static string ValidateInputInquiryCC(ref CreditCardModels.CreditCardInquiryRequest InqRequest, string feature)
        {
            string rc = "";
            decimal number;

            Helper helper = new Helper();
            if (!helper.FeatureCheck(InqRequest.instiutionCode, feature))
            {
                rc = "0011";
            }
            else if (String.IsNullOrEmpty(InqRequest.cardNumber))
            {
                rc = "0101";
            }
            else if (!decimal.TryParse(InqRequest.cardNumber, out number))
            {
                rc = "0104";
            }
            else
            {
                rc = "0005"; //Validasi sukses
            }

            return rc;
        }

        public static string ValidatePaymentCC(ref CreditCardModels.CreditCardPaymentRequest PayRequest, string feature)
        {
            Boolean ceknoref = CreditCardHelper.CheckReferenceCreditCard(PayRequest.reference, PayRequest.instiutionCode);
            decimal number;
            string rc = "";
            Helper helper = new Helper();

            if (!helper.FeatureCheck(PayRequest.instiutionCode, feature))
            {
                rc = "0011";
            }
            else if (!ceknoref)
            {
                if (String.IsNullOrEmpty(helper.GetSourceAccount(PayRequest.instiutionCode, ConstantModels.FeatureCode_Telkom)))
                {
                    rc = "0111";
                }
                else if (String.IsNullOrEmpty(PayRequest.cardNumber))
                {
                    rc = "0110"; //Card number tidak boleh kosong
                }
                else if (!decimal.TryParse(PayRequest.cardNumber, out number))
                {
                    rc = "0104";
                }
                else if (String.IsNullOrEmpty(PayRequest.amount))
                {
                    rc = "0105"; //Total amount tidak boleh kosong
                }
                else if (String.IsNullOrEmpty(PayRequest.cardName))
                {
                    rc = "0106"; //Name tidak boleh kosong
                }
                else if (String.IsNullOrEmpty(PayRequest.reference))
                {
                    rc = "0014"; //Reference tidak boleh kosong
                }
                else if (!decimal.TryParse(PayRequest.amount.Replace(",", ""), out number))
                {
                    rc = "0108"; //Total amount mengandung karakter bukan angka
                }
                else if (System.Convert.ToDouble(PayRequest.amount.Replace(",", "")) < 1)
                {
                    rc = "0109"; //Total amount tidak boleh 0 atau bernilai negatif
                }
                else
                {
                    rc = "0005"; //Success
                }
            }
            else
            {
                rc = "0013"; //Reference sudah pernah digunakan
            }

            return rc;
        }
    }
}