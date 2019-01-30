using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API_PAYMENT.Models
{
    public class TelkomValidation
    {
        TelkomHelper telkomHelper = new TelkomHelper();

        public string ValidateInputInquiryTelkom(ref TelkomModels.TelkomInquiryRequest AutoInqRequest, string ip)
        {
            decimal number;
            string rc = "";

            string sourceAccount = telkomHelper.GetSourceAccountTelkom(AutoInqRequest.InstitutionCode, ConstantModels.FeatureCode_Telkom);

            if (String.IsNullOrEmpty(AutoInqRequest.BillingNumber))
            {
                rc = "0201";
            }
            else if (String.IsNullOrEmpty(sourceAccount))
            {
                rc = "0202";
            }
            else if (!decimal.TryParse(AutoInqRequest.BillingNumber, out number))
            {
                rc = "0203";
            }
            else if (!decimal.TryParse(AutoInqRequest.SourceAccount, out number))
            {
                rc = "0204";
            }
            else
            {
                rc = "0005"; //Validasi sukses
            }

            return rc;
        }

        public string ValidatePaymentTelkom(ref TelkomModels.TelkomPaymentRequest PayRequest, string ip)
        {
            Boolean ceknoref = telkomHelper.CheckReferralNumberTelkom(PayRequest.ReferralNumber, PayRequest.InstitutionCode);
            decimal number;
            string rc = "";

            string sourceAccount = telkomHelper.GetSourceAccountTelkom(PayRequest.InstitutionCode, ConstantModels.FeatureCode_Telkom);

            if (!ceknoref)
            {
                //if (String.IsNullOrEmpty(PayRequest.Amount) || String.IsNullOrEmpty(PayRequest.beneficiaryAccount) || String.IsNullOrEmpty(PayRequest.bankCode) || String.IsNullOrEmpty(PayRequest.beneficiaryAccountName) || String.IsNullOrEmpty(PayRequest.sourceAccount) || String.IsNullOrEmpty(PayRequest.noReferral))
                if (String.IsNullOrEmpty(PayRequest.TotalAmount))
                {
                    rc = "0205"; //Total amount tidak boleh kosong
                }
                else if (String.IsNullOrEmpty(PayRequest.BillingNumber))
                {
                    rc = "0201"; //Billing number tidak boleh kosong
                }
                else if (String.IsNullOrEmpty(sourceAccount))
                {
                    rc = "0202"; //Source account tidak boleh kosong
                }
                else if (String.IsNullOrEmpty(PayRequest.Name))
                {
                    rc = "0206"; //Name tidak boleh kosong
                }
                else if (String.IsNullOrEmpty(PayRequest.BillingCode))
                {
                    rc = "0207";
                }
                else if (String.IsNullOrEmpty(PayRequest.ReferralNumber))
                {
                    rc = "0010"; //Referral number tidak boleh kosong
                }
                else if (!decimal.TryParse(PayRequest.TotalAmount.Replace(",", ""), out number))
                {
                    rc = "0208"; //Total amount mengandung karakter bukan angka
                }
                else if (System.Convert.ToDouble(PayRequest.TotalAmount.Replace(",", "")) < 1)
                {
                    rc = "0209"; //Total amount tidak boleh 0 atau bernilai negatif
                }
                else if (!decimal.TryParse(PayRequest.SourceAccount.Replace(",", ""), out number))
                {
                    rc = "0204"; //Source account mengandung karakter bukan angka
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
                rc = "0011"; //Nomor referral sudah pernah digunakan
            }

            return rc;
        }
    }
}