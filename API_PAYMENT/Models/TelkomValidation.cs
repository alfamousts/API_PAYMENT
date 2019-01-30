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

            if (helper.FeatureCheck(AutoInqRequest.InstitutionCode, ConstantModels.FeatureCode_Telkom))
            {
                if (String.IsNullOrEmpty(AutoInqRequest.BillingNumber))
                {
                    rc = "0210";
                }
                else if (String.IsNullOrEmpty(AutoInqRequest.SourceAccount))
                {
                    rc = "0211";
                }
                else if (!decimal.TryParse(AutoInqRequest.BillingNumber, out number))
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
            Boolean ceknoref = telkomHelper.CheckReferralNumberTelkom(PayRequest.ReferralNumber, PayRequest.InstitutionCode);
            decimal number;
            string rc = "";

            if (helper.FeatureCheck(PayRequest.InstitutionCode, ConstantModels.FeatureCode_Telkom))
            {
                if (!ceknoref)
                {
                    if (String.IsNullOrEmpty(PayRequest.TotalAmount))
                    {
                        rc = "0205"; //Total amount tidak boleh kosong
                    }
                    else if (String.IsNullOrEmpty(PayRequest.BillingNumber))
                    {
                        rc = "0210"; //Billing number tidak boleh kosong
                    }
                    else if (String.IsNullOrEmpty(PayRequest.SourceAccount))
                    {
                        rc = "0211"; //Source account tidak boleh kosong
                    }
                    else if (String.IsNullOrEmpty(PayRequest.Name))
                    {
                        rc = "0206"; //Name tidak boleh kosong
                    }
                    else if (String.IsNullOrEmpty(PayRequest.BillingCode))
                    {
                        rc = "0207"; //Billing code tidak boleh kosong
                    }
                    else if (String.IsNullOrEmpty(PayRequest.ReferralNumber))
                    {
                        rc = "0014"; //Referral number tidak boleh kosong
                    }
                    else if (!decimal.TryParse(PayRequest.TotalAmount.Replace(",", ""), out number))
                    {
                        rc = "0208"; //Total amount mengandung karakter bukan angka
                    }
                    else if (System.Convert.ToDouble(PayRequest.TotalAmount.Replace(",", "")) < 1)
                    {
                        rc = "0209"; //Total amount tidak boleh 0 atau bernilai negatif
                    }
                    else if (!decimal.TryParse(PayRequest.ReferralNumber, out number))
                    {
                        rc = "0012"; //Referral number mengandung karakter bukan angka
                    }
                    else
                    {
                        rc = "0005"; //Success
                    }
                }
                else
                {
                    rc = "0013"; //Nomor referral sudah pernah digunakan
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