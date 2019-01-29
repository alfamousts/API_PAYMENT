using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Net;
using System.Text;

namespace API_PAYMENT.Models
{
    /// <summary>
    ///  Telkom Model Data, Hanum
    /// </summary>
    public class TelkomModels
    {
        public TelkomModels()
        {
        }

        //add models request and response here
        /// <summary>
        ///  Telkom Inquiry Models
        /// </summary>
        public class TelkomInquiryRequest
        {
            [StringLength(20)]
            public string InstitutionCode { get; set; }
            [StringLength(100)]
            public string InstitutionKey { get; set; }
            [StringLength(20)]
            public string TransactionDate { get; set; } = DateTime.Now.ToString("dd/MM/y");
            [StringLength(20)]
            public string TransactionTime { get; set; } = DateTime.Now.ToString("HH:mm:ss");
            [StringLength(20)]
            public string ChannelID { get; set; } = ConstantModels.ChannelID_Telkom;
            [StringLength(20)]
            public string ProductID { get; set; } = ConstantModels.ProductID_Telkom;
            [StringLength(20)]
            public string SubProduct { get; set; } = ConstantModels.SubProductINQ_Telkom;
            [StringLength(20)]
            public string SequenceTransaction { get; set; } = "123456789015";
            [Required]
            [StringLength(20)]
            public string TotalAmount { get; set; } = "";
            [Required]
            [StringLength(20)]
            public string FeeAmount { get; set; } = "0";
            [Required]
            [StringLength(20)]
            public string AddAmount1 { get; set; } = "0";
            [Required]
            [StringLength(20)]
            public string AddAmount2 { get; set; } = "0";
            [Required]
            [StringLength(20)]
            public string AddAmount3 { get; set; } = "0";
            [Required]
            [StringLength(100)]
            public string BillingNumber { get; set; } //InputData
            [Required]
            [StringLength(100)]
            public string BeneficiaryAccount { get; set; } //Data1
            [StringLength(100)]
            public string Data2 { get; set; } = "-"; //Data2
            [Required]
            [StringLength(100)]
            public string Remark { get; set; } = "5221849000000259";
            [StringLength(100)]
            public string Key { get; set; } = ConstantModels.Key_Telkom;
        }

        public class TelkomInquiryResponse
        {
            public string responseCode { get; set; }
            public string responseDescription { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string errorDescription { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public TelkomInquiryResponseData data { get; set; } = new TelkomInquiryResponseData();
        }

        public class TelkomInquiryResponseData
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Data1 { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Data2 { get; set; }
        }

        public class PSWServiceInquiryResponse
        {
            public string RC;
            public string Description;
            public string Data1;
            public string Data2;
        }

        /// <summary>
        ///  Telkom Payment Models
        /// </summary>
        public class TelkomPaymentRequest
        {
            [StringLength(20)]
            public string InstitutionCode { get; set; }
            [StringLength(100)]
            public string InstitutionKey { get; set; }
            [StringLength(20)]
            public string TransactionDate { get; set; } 
            [StringLength(20)]
            public string TransactionTime { get; set; } 
            [StringLength(20)]
            public string ChannelID { get; set; } = ConstantModels.ChannelID_Telkom;
            [StringLength(20)]
            public string ProductID { get; set; } = ConstantModels.ProductID_Telkom;
            [StringLength(20)]
            public string SubProduct { get; set; } = ConstantModels.SubProductPAY_Telkom;
            [StringLength(20)]
            public string SequenceTransaction { get; set; }
            [Required]
            [StringLength(20)]
            public string TotalAmount { get; set; }
            [Required]
            [StringLength(20)]
            public string FeeAmount { get; set; }
            [Required]
            [StringLength(20)]
            public string AddAmount1 { get; set; } 
            [Required]
            [StringLength(20)]
            public string AddAmount2 { get; set; } 
            [Required]
            [StringLength(20)]
            public string AddAmount3 { get; set; } 
            [Required]
            [StringLength(100)]
            public string BillingNumber { get; set; } //InputData
            [Required]
            [StringLength(100)]
            public string BeneficiaryAccount { get; set; } //Data1
            [StringLength(100)]
            public string BeneficiaryName { get; set; } //Data2
            [Required]
            [StringLength(100)]
            public string Remark { get; set; }
            [StringLength(100)]
            public string Key { get; set; } = ConstantModels.Key_Telkom;
            [Required]
            [StringLength(100)]
            public string ReferralNumber { get; set; }
        }

        public class TelkomPaymentResponse
        {
            public string responseCode { get; set; }
            public string responseDescription { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string errorDescription { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public TelkomPaymentResponseData data { get; set; } = new TelkomPaymentResponseData();
        }

        public class TelkomPaymentResponseData
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Data1 { get; set; }
        }

        public class PSWServicePaymentResponse
        {
            public string RC;
            public string Description;
            public string Data1;
        }
    }

    /// <summary>
    /// Telkom Helper Transaction
    /// </summary>
    public class TelkomHelper
    {
        public TelkomHelper()
        {
        }

        Helper helper = new Helper();

        #region INQUIRY TELKOM
        //public string ValidateInputInquiryTelkom(ref TelkomModels.TelkomInquiryRequest AutoInqRequest, string ip)
        //{
        //    Boolean cekip = CheckIP(ip, AutoInqRequest.institutionCode);
        //    Boolean cekInst = CheckInstitusi(AutoInqRequest.institutionCode);
        //    decimal number;
        //    string rc = "";

        //    try
        //    {
        //        DataTable dtInst = GetParameterInstitusiDT(AutoInqRequest.institutionCode, "INSTITUTION_LOGIN_NAME, INSTITUTION_KEY, MEMBER_REGISTERED, INST_ACC_VALIDATION");

        //        if (String.IsNullOrEmpty(AutoInqRequest.institutionCode))
        //        {
        //            rc = "0007"; //Kode institusi tidak boleh kosong
        //        }
        //        else if (cekInst)
        //        {
        //            if (cekip)
        //            {

        //                if (String.IsNullOrEmpty(AutoInqRequest.Amount) || String.IsNullOrEmpty(AutoInqRequest.bankCode) || String.IsNullOrEmpty(AutoInqRequest.beneficiaryAccount))
        //                {
        //                    rc = "0602"; //Data request (total amount, kode bank) ada yang kosong
        //                }
        //                else if (!decimal.TryParse(AutoInqRequest.bankCode, out number))
        //                {
        //                    rc = "0603"; //Kode bank mengandung karakter bukan angka
        //                }
        //                else if (System.Convert.ToDouble(AutoInqRequest.Amount.Replace(",", "")) < 1)
        //                {
        //                    rc = "0207"; //Amount tidak boleh 0 atau kurang dari 0
        //                }
        //                else if (!decimal.TryParse(AutoInqRequest.beneficiaryAccount, out number))
        //                {
        //                    rc = "0601"; //Nomor Rekening asal/tujuan mengandung karakter bukan angka
        //                }
        //                else
        //                {
        //                    rc = "0005"; //Validasi sukses
        //                }
        //            }
        //            else
        //            {
        //                rc = "0010"; //IP address tidak diijinkan
        //            }
        //        }
        //        else
        //        {
        //            rc = "0008"; //Institusi tidak dikenali
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logging(AutoInqRequest.institutionCode, "WSOVERBOOKING_INQUIRYACCTONLINE", ex.Message);
        //    }

        //    return rc;
        //}

        public TelkomModels.PSWServiceInquiryResponse PSWServiceInquiryTelkom(ref TelkomModels.TelkomInquiryRequest requestParam)
        {
            string url = ConstantModels.URLINQPAY_TELKOM;

            string _requestInq = "[{\"Transdate\":\"" + requestParam.TransactionDate + "\",\"Transtime\":\"" + requestParam.TransactionTime + "\",\"ChannelID\":\"" +
                                requestParam.ChannelID + "\",\"ProductID\":\"" + requestParam.ProductID + "\",\"SubProduct\":\"" + requestParam.SubProduct + "\",\"SequenceTrx\"" +
                                ":\"" + requestParam.SequenceTransaction + "\",\"TotalAmount\":\"" + requestParam.TotalAmount + "\",\"FeeAmount\":\"" + requestParam.FeeAmount + 
                                "\",\"AddAmount1\":\"" + requestParam.AddAmount1 + "\",\"AddAmount2\":\"" + requestParam.AddAmount2 + "\",\"AddAmount3\":\"" + requestParam.AddAmount3 +
                                "\",\"InputData\":\"" + requestParam.BillingNumber + "\",\"Data1\":\"" + requestParam.BeneficiaryAccount + "\",\"Data2\":\"" + requestParam.Data2 +
                                "\",\"Remark\":\"" + requestParam.Remark + "\",\"Key\":\"" + requestParam.Key + "\"}]";
            string requestInq = "data=" + _requestInq;

            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] postBytes = ascii.GetBytes(requestInq);
            HttpWebRequest request;

            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(url);
            }
            catch (UriFormatException)
            {
                return null;
            }

            request.Method = "POST";
            request.Accept = "application/x-www-form-urlencoded";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;
            System.Net.ServicePointManager.Expect100Continue = false;

            WebProxy proxy = new WebProxy("172.18.104.20", 1707);
            request.Proxy = proxy;

            // add post data to request
            Stream postStream = request.GetRequestStream();
            postStream.Write(postBytes, 0, postBytes.Length);
            postStream.Flush();
            postStream.Close();
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream answerStream = response.GetResponseStream();
                    StreamReader answerReader = new StreamReader(answerStream);
                    String jsonAnswer = answerReader.ReadToEnd();
                    TelkomModels.PSWServiceInquiryResponse respond = JsonConvert.DeserializeObject<TelkomModels.PSWServiceInquiryResponse>(jsonAnswer);
                    return respond;
                }
                else
                {
                    return null;
                }
            }catch(Exception ex)
            {
                return null;
            }
        }

        public TelkomModels.TelkomInquiryResponse InquiryTelkom(ref TelkomModels.TelkomInquiryRequest AutoInqRequest, string ip)
        {
            string request = "REQUEST : Transdate=" + AutoInqRequest.TransactionDate + "|Transtime=" + AutoInqRequest.TransactionTime + "|ChannelID=" + AutoInqRequest.ChannelID +
                             "|ProductID=" + AutoInqRequest.ProductID + "|SubProduct=" + AutoInqRequest.SubProduct + "|SequenceTrx=" + AutoInqRequest.SequenceTransaction + 
                             "|TotalAmount=" + AutoInqRequest.TotalAmount + "|FeeAmount=" + AutoInqRequest.FeeAmount + "|AddAmount1=" + AutoInqRequest.AddAmount1 +
                             "|AddAmount2=" + AutoInqRequest.AddAmount2 + "|AddAmount3=" + AutoInqRequest.AddAmount3 + "|InputData=" + AutoInqRequest.BillingNumber + 
                             "|Data1=" + AutoInqRequest.BeneficiaryAccount + "|Data2=" + AutoInqRequest.Data2 + "|Remark=" + AutoInqRequest.Remark + 
                             "|Key=" + AutoInqRequest.Key + "|IP=" + ip;
            helper.logging(AutoInqRequest.InstitutionCode, "APIPAYMENT_INQUIRYTELKOM", request);

            string wsStartTime = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);

            TelkomModels.TelkomInquiryResponse AutoInqResponse = new TelkomModels.TelkomInquiryResponse();
            TelkomModels.PSWServiceInquiryResponse GetInqResponse = new TelkomModels.PSWServiceInquiryResponse();

            GetInqResponse = PSWServiceInquiryTelkom(ref AutoInqRequest);

            if (GetInqResponse == null)
            {
                AutoInqResponse.responseCode = "99";
                AutoInqResponse.responseDescription = "Inquiry gagal";
                AutoInqResponse.errorDescription = "General Error";
            }
            else if (GetInqResponse.RC == "00") //success
            {
                AutoInqResponse.responseCode = ConstantModels.SUCCESSCODEINQ;
                AutoInqResponse.responseDescription = GetInqResponse.Description;
                AutoInqResponse.data.Data1 = GetInqResponse.Data1.Trim();
                AutoInqResponse.data.Data2 = GetInqResponse.Data2.Trim();
            }
            else //fail
            {
                AutoInqResponse.responseCode = ConstantModels.FAILEDCODEINQ;//ResponseCodeModels.GetResponseCode(GetInqResponse.RC);
                AutoInqResponse.responseDescription = "Inquiry gagal";
                AutoInqResponse.errorDescription = GetInqResponse.Description;
            }

            string wsEndTime = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);
            string response = "RESPONSE : ResponseCode=" + AutoInqResponse.responseCode + "|ResponseDescription=" + AutoInqResponse.responseDescription + "|ErrDesc=" + AutoInqResponse.errorDescription;// + "|Name=" + AutoInqResponse.Name;
            helper.logging(AutoInqRequest.InstitutionCode, "APIPAYMENT_INQUIRYTELKOM", response);
            //helper.InsertLogInquiryAccountOnline(AutoInqRequest, AutoInqResponse, wsStartTime, wsEndTime, ip, GetInqResponse.RC);

            return AutoInqResponse;
        }
        #endregion

        #region PAYMENT TELKOM
        //public string ValidatePaymentTelkom(ref TransferOnlineRequest PayRequest, string ip)
        //{
        //    Boolean cekip = CheckIP(ip, PayRequest.institutionCode);
        //    Boolean cekInst = CheckInstitusi(PayRequest.institutionCode);
        //    Boolean CekRekInst = CheckRekInstitusi(PayRequest.institutionCode, PayRequest.sourceAccount); //cek norekasal
        //    Boolean ceknoref = CheckNorefTransaksiOnline(PayRequest.noReferral, PayRequest.institutionCode);
        //    decimal number;
        //    string rc = "";

        //    try
        //    {
        //        DataTable dtInst = GetParameterInstitusiDT(PayRequest.institutionCode, "INSTITUTION_LOGIN_NAME, INSTITUTION_KEY, MEMBER_REGISTERED, INST_ACC_VALIDATION");
        //        if (!ceknoref)
        //        {
        //            if (String.IsNullOrEmpty(PayRequest.Amount) || String.IsNullOrEmpty(PayRequest.beneficiaryAccount) || String.IsNullOrEmpty(PayRequest.bankCode) || String.IsNullOrEmpty(PayRequest.beneficiaryAccountName) || String.IsNullOrEmpty(PayRequest.sourceAccount) || String.IsNullOrEmpty(PayRequest.noReferral))
        //            {
        //                rc = "0702"; //Data request (total amount, norek asal, kode bank, norek tujuan, nama) ada yang kosong
        //            }
        //            else if (!decimal.TryParse(PayRequest.Amount.Replace(",", ""), out number))
        //            {
        //                rc = "0703"; //Amount mengandung karakter bukan angka
        //            }
        //            else if (System.Convert.ToDouble(PayRequest.Amount.Replace(",", "")) < 1)
        //            {
        //                rc = "0704"; //Amount tidak boleh 0 atau kurang dari 0
        //            }
        //            else if (!decimal.TryParse(PayRequest.beneficiaryAccount, out number) || !decimal.TryParse(PayRequest.sourceAccount, out number))
        //            {
        //                rc = "0701"; //Nomor Rekening asal/tujuan mengandung karakter bukan angka
        //            }
        //            else if (!CekRekInst)
        //            {
        //                rc = "0011"; //Rekening (asal) tidak terdaftar di rekening institusi
        //            }
        //            else if (!decimal.TryParse(PayRequest.bankCode, out number))
        //            {
        //                rc = "0706"; //Kode bank mengandung karakter bukan angka
        //            }
        //            else
        //            {
        //                rc = "0005"; //Validasi sukses
        //            }
        //        }
        //        else
        //        {
        //            rc = "0705"; //Nomor referral sudah pernah digunakan
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        logging(PayRequest.institutionCode, "WSOVERBOOKING_DOTRANSFERONLINE", ex.Message);
        //    }

        //    return rc;
        //}

        public TelkomModels.PSWServicePaymentResponse PSWServicePaymentTelkom(ref TelkomModels.TelkomPaymentRequest requestParam)
        {
            string url = ConstantModels.URLINQPAY_TELKOM;

            string _requestInq = "[{\"Transdate\":\"" + requestParam.TransactionDate + "\",\"Transtime\":\"" + requestParam.TransactionTime + "\",\"ChannelID\":\"" +
                                requestParam.ChannelID + "\",\"ProductID\":\"" + requestParam.ProductID + "\",\"SubProduct\":\"" + requestParam.SubProduct + "\",\"SequenceTrx\"" +
                                ":\"" + requestParam.SequenceTransaction + "\",\"TotalAmount\":\"" + requestParam.TotalAmount + "\",\"FeeAmount\":\"" + requestParam.FeeAmount +
                                "\",\"AddAmount1\":\"" + requestParam.AddAmount1 + "\",\"AddAmount2\":\"" + requestParam.AddAmount2 + "\",\"AddAmount3\":\"" + requestParam.AddAmount3 +
                                "\",\"InputData\":\"" + requestParam.BillingNumber + "\",\"Data1\":\"" + requestParam.BeneficiaryAccount + "\",\"Data2\":\"" + requestParam.BeneficiaryName +
                                "\",\"Remark\":\"" + requestParam.Remark + "\",\"Key\":\"" + requestParam.Key + "\"}]";
            string requestInq = "data=" + _requestInq;

            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] postBytes = ascii.GetBytes(requestInq);
            HttpWebRequest request;

            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(url);
            }
            catch (UriFormatException)
            {
                return null;
            }

            request.Method = "POST";
            request.Accept = "application/x-www-form-urlencoded";
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = postBytes.Length;
            System.Net.ServicePointManager.Expect100Continue = false;

            WebProxy proxy = new WebProxy("172.18.104.20", 1707);
            request.Proxy = proxy;

            // add post data to request
            Stream postStream = request.GetRequestStream();
            postStream.Write(postBytes, 0, postBytes.Length);
            postStream.Flush();
            postStream.Close();
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream answerStream = response.GetResponseStream();
                    StreamReader answerReader = new StreamReader(answerStream);
                    String jsonAnswer = answerReader.ReadToEnd();
                    TelkomModels.PSWServicePaymentResponse respond = JsonConvert.DeserializeObject<TelkomModels.PSWServicePaymentResponse>(jsonAnswer);
                    return respond;
                }
                else
                {
                    return null;
                }
            }catch(Exception ex)
            {
                return null;
            }
        }

        //added by Hanum 4-9-2018
        public TelkomModels.TelkomPaymentResponse PaymentTelkom(ref TelkomModels.TelkomPaymentRequest AutoPayRequest, string ip)
        {
            string request = "REQUEST : Transdate=" + AutoPayRequest.TransactionDate + "|Transtime=" + AutoPayRequest.TransactionTime + "|ChannelID=" + AutoPayRequest.ChannelID +
                             "|ProductID=" + AutoPayRequest.ProductID + "|SubProduct=" + AutoPayRequest.SubProduct + "|SequenceTrx=" + AutoPayRequest.SequenceTransaction +
                             "|TotalAmount=" + AutoPayRequest.TotalAmount + "|FeeAmount=" + AutoPayRequest.FeeAmount + "|AddAmount1=" + AutoPayRequest.AddAmount1 +
                             "|AddAmount2=" + AutoPayRequest.AddAmount2 + "|AddAmount3=" + AutoPayRequest.AddAmount3 + "|InputData=" + AutoPayRequest.BillingNumber +
                             "|Data1=" + AutoPayRequest.BeneficiaryAccount + "|Data2=" + AutoPayRequest.BeneficiaryName + "|Remark=" + AutoPayRequest.Remark +
                             "|Key=" + AutoPayRequest.Key + "|ReferralNumber=" + AutoPayRequest.ReferralNumber + "|IP=" + ip;
            helper.logging(AutoPayRequest.InstitutionCode, "WSOVERBOOKING_DOTRANSFERONLINE", request);

            string wsStartTime = DateTime.Now.ToString(ConstantModels.FORMATDATETIME); //Hanum new

            TelkomModels.TelkomPaymentResponse AutoPayResponse = new TelkomModels.TelkomPaymentResponse();
            TelkomModels.PSWServicePaymentResponse GetPayResponse = new TelkomModels.PSWServicePaymentResponse();

            GetPayResponse = PSWServicePaymentTelkom(ref AutoPayRequest);
            string wsEndTime = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);

            if (GetPayResponse == null)
            {
                AutoPayResponse.responseCode = "99";
                AutoPayResponse.responseDescription = "Payment gagal";
                AutoPayResponse.errorDescription = "General Error";
            }

            if (GetPayResponse.RC == "00") //success
            {
                AutoPayResponse.responseCode = ConstantModels.SUCCESSCODEPAY;
                AutoPayResponse.responseDescription = GetPayResponse.Description;
                AutoPayResponse.data.Data1 = GetPayResponse.Data1.Trim();
            }
            else //fail
            {
                AutoPayResponse.responseCode = ConstantModels.FAILEDCODEINQ;//ResponseCodeModels.GetResponseCode(GetInqResponse.RC);
                AutoPayResponse.responseDescription = "Inquiry gagal";
                AutoPayResponse.errorDescription = GetPayResponse.Description;
            }

            string response = "RESPONSE : ResponseCode=" + AutoPayResponse.responseCode + "-" + GetPayResponse.RC + "|ResponseDescription=" + AutoPayResponse.responseDescription + "|ErrDesc=" + AutoPayResponse.errorDescription;// + "|JurnalSeq=" + AutoPayResponse.JurnalSeq;
            helper.logging(AutoPayRequest.InstitutionCode, "APIPAYMENT_PAYMENTTELKOM", response);
            
                //else if (GetPayResponse.RC == "68")
                //{
                //    AutoPayResponse.responseCode = ResponseCodeModels.GetResponseCodePayment(GetPayResponse.RC);
                //    AutoPayResponse.responseDescription = "Do transfer online sedang diproses";
                //    AutoPayResponse.errorDescription = ResponseCodeModels.GetResponseDescription(AutoPayResponse.responseCode);
                //}

            return AutoPayResponse;
        }
        #endregion
    }
}
