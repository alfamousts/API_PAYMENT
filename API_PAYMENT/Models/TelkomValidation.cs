using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_PAYMENT.Models
{
    public class TelkomValidation
    {
        TelkomHelper telkomHelper = new TelkomHelper();
        Helper helper = new Helper();

        public string ValidateInputInquiryTelkom(ref TelkomModels.TelkomInquiryRequest AutoInqRequest)
        {
            decimal number;
            string rc = "";

            if (helper.FeatureCheck(AutoInqRequest.institutionCode, ConstantModels.FeatureCode_Telkom))
            {
                if (String.IsNullOrEmpty(AutoInqRequest.billingNumber))
                {
                    rc = "0210";
                }
                else if (!decimal.TryParse(AutoInqRequest.billingNumber, out number))
                {
                    rc = "0204";
                }
                else
                {
                    rc = "0005"; //Validasi sukses
                }
            }
            else
            {
                rc = "0011";
            }

            return rc;
        }

        public string ValidatePaymentTelkom(ref TelkomModels.TelkomPaymentRequest PayRequest)
        {
            Boolean ceknoref = telkomHelper.CheckReferenceTelkom(PayRequest.reference, PayRequest.institutionCode);
            decimal number;
            string rc = "";

            if (helper.FeatureCheck(PayRequest.institutionCode, ConstantModels.FeatureCode_Telkom))
            {
                if (!ceknoref)
                {
                    if (String.IsNullOrEmpty(PayRequest.totalAmount))
                    {
                        rc = "0205"; //Total amount tidak boleh kosong
                    }
                    else if (String.IsNullOrEmpty(PayRequest.billingNumber))
                    {
                        rc = "0210"; //Billing number tidak boleh kosong
                    }
                    else if (String.IsNullOrEmpty(PayRequest.name))
                    {
                        rc = "0206"; //Name tidak boleh kosong
                    }
                    else if (String.IsNullOrEmpty(PayRequest.billingCode))
                    {
                        rc = "0207"; //Billing code tidak boleh kosong
                    }
                    else if (String.IsNullOrEmpty(PayRequest.reference))
                    {
                        rc = "0012"; //Reference tidak boleh kosong
                    }
                    else if (!decimal.TryParse(PayRequest.totalAmount.Replace(",", ""), out number))
                    {
                        rc = "0208"; //Total amount mengandung karakter bukan angka
                    }
                    else if (System.Convert.ToDouble(PayRequest.totalAmount.Replace(",", "")) < 1)
                    {
                        rc = "0209"; //Total amount tidak boleh 0 atau bernilai negatif
                    }
                    else if ((PayRequest.firstBill.Split('#')).Length == 1 || (PayRequest.secondBill.Split('#')).Length == 1 || (PayRequest.thirdBill.Split('#')).Length == 1)
                    {
                        rc = "0212"; //Cek format
                    }
                    else if ((PayRequest.billingCode.Split('#')).Length < 3)
                    {
                        rc = "0213"; //Cek format
                    }
                    else
                    {
                        rc = "0005"; //Validasi success
                    }
                }
                else
                {
                    rc = "0013"; //Reference sudah pernah digunakan
                }
            }
            else
            {
                rc = "0011";
            }

            return rc;
        }
    }
}