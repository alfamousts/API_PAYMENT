using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
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
            public string SequenceTransaction { get; set; }
            //[Required]
            //[StringLength(20)]
            //public string TotalAmount { get; set; } = "";
            //[Required]
            //[StringLength(20)]
            //public string FeeAmount { get; set; } = "0";
            //[Required]
            //[StringLength(20)]
            //public string AddAmount1 { get; set; } = "0";
            //[Required]
            //[StringLength(20)]
            //public string AddAmount2 { get; set; } = "0";
            //[Required]
            //[StringLength(20)]
            //public string AddAmount3 { get; set; } = "0";
            [Required]
            [StringLength(100)]
            public string BillingNumber { get; set; } //InputData
            [Required]
            [StringLength(100)]
            public string SourceAccount { get; set; } //Data1
            //[StringLength(100)]
            //public string Data2 { get; set; } = "-"; //Data2
            //[Required]
            //[StringLength(100)]
            //public string Remark { get; set; } = "5221849000000259";
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
            public string name { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public double totalAmount { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string billingCode { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public List<TelkomBillingDetailsData> billingDetail { get; set; }
        }

        public class TelkomBillingDetailsData
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string referenceNumber { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public double amount { get; set; }
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
            public string TransactionDate { get; set; } = DateTime.Now.ToString("dd/MM/y");
            [StringLength(20)]
            public string TransactionTime { get; set; } = DateTime.Now.ToString("HH:mm:ss");
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
            //[Required]
            //[StringLength(20)]
            //public string FeeAmount { get; set; }
            [Required]
            [StringLength(20)]
            public string FirstBill { get; set; } 
            [Required]
            [StringLength(20)]
            public string SecondBill { get; set; } 
            [Required]
            [StringLength(20)]
            public string ThirdBill { get; set; } 
            [Required]
            [StringLength(100)]
            public string BillingNumber { get; set; } //InputData
            [Required]
            [StringLength(100)]
            public string SourceAccount { get; set; } //Data1
            [StringLength(100)]
            public string Name { get; set; } //Data2
            [Required]
            [StringLength(100)]
            public string BillingCode { get; set; }
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
            public string JurnalSeq { get; set; }
        }

        public class PSWServicePaymentResponse
        {
            public string RC;
            public string Description;
            public string JurnalSeq;
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

        public Boolean CheckReferralNumberTelkom(string noref, string kodeInst)
        {
            Boolean result;
            Util util = new Util();
            string sql;

            sql = "SELECT * FROM TELKOMTRANSACTION WITH (NOLOCK) WHERE NOMOR_REFF='" + noref + "' AND RC = '0200' AND INSTITUTION_CODE = '" + kodeInst + "'";
            DataTable dt = util.setDataTable(sql);

            if (dt.Rows.Count > 0)
            {
                result = true;
                return result;
            }
            else
            {
                result = false;
                return result;
            }
        }

        public void InsertLogInquiryTelkom(TelkomModels.TelkomInquiryRequest InqRequest, TelkomModels.TelkomInquiryResponse InqResponse, string wsStartTime, string wsEndTime, string ip, string rc_psw)
        {
            Util util = new Util();
            string sql;
            //string name = (InqResponse.Name == null ? "-" : InqResponse.Name);
            string errMsg = "";
            if (InqResponse.responseCode != "0100")
            {
                errMsg = ((InqResponse.errorDescription == null ? 0 : InqResponse.errorDescription.Length) > 250 ? InqResponse.errorDescription.Substring(0, 250) : InqResponse.errorDescription) + " (" + rc_psw + ")";
            }

            sql = "INSERT INTO TELKOMINQUIRYLOG ([CREATEDTIME],[WS_STARTTIME],[WS_ENDTIME],[ACTION],[INSTITUTION_CODE],[TRANSACTION_DATE],[TRANSACTION_TIME]" +
                ",[CHANNEL_ID],[PRODUCT_ID],[SUB_PRODUCT],[SEQUENCE_TRX],[BILLING_NUMBER],[SOURCE_ACCOUNT],[KEY],[RC],[RC_DESC],[ERRMSG],[IP_ADDRESS]) " +
                  "VALUES ('" + DateTime.Now.ToString(ConstantModels.FORMATDATETIME) + "', '" + wsStartTime + "', '" + wsEndTime + "', 'INQUIRY'," +
                  "'" + InqRequest.InstitutionCode + "', '" + InqRequest.TransactionDate + "', '" + InqRequest.TransactionTime + "', '" + InqRequest.ChannelID +
                  "','" + InqRequest.ProductID + "', '" + InqRequest.SubProduct + "', '" + InqRequest.SequenceTransaction + "', '" + InqRequest.BillingNumber +
                  "', '" + InqRequest.SourceAccount + "', '" + InqRequest.Key + "', '" + InqResponse.responseCode + "','" + InqResponse.responseDescription +
                  "','" + errMsg + "','" + ip + "')";

            util.cmdSQLScalar(sql);
        }

        public void InsertTelkomTransaction(TelkomModels.TelkomPaymentRequest PayRequest, TelkomModels.TelkomPaymentResponse PayResponse, string wsStartTime, string wsEndTime, string ip, string rc_psw)
        {
            Util util = new Util();
            string sql;
            string transDate = DateTime.Parse(PayRequest.TransactionDate).ToString("dd-MM-yyyy");
            string errorMsg = "";
            if (PayResponse.responseCode != "0200")
            {
                errorMsg = ((PayResponse.errorDescription == null ? 0 : PayResponse.errorDescription.Length) > 250 ? PayResponse.errorDescription.Substring(0, 250) : PayResponse.errorDescription) + " (" + rc_psw + ")";
            }
            sql = "";
            //sql = "INSERT INTO TRANSAKSI_ONLINE ([CREATEDTIME],[WS_STARTTIME],[WS_ENDTIME],[BANK_CODE],[INSTITUTION_CODE],[AMOUNT_TRANSAKSI]," +
            //      "[NO_REK_DB],[NO_REK_CR],[NAMA],[TRANSACTION_DATE],[TRANSACTION_TIME],[RC],[RC_DESC],[ERRMSG],[JURNALSEQ],[IP_ADDRESS],[NOMOR_REFF]) " +
            //      "VALUES ('" + DateTime.Now.ToString(ConstantModels.FORMATDATETIME) + "','" + wsStartTime + "', '" + wsEndTime + "', '" + PayRequest.bankCode + "', '" + PayRequest.institutionCode +
            //      "', '" + PayRequest.Amount + "', '" + PayRequest.sourceAccount + "','" + PayRequest.beneficiaryAccount + "', '" + PayRequest.beneficiaryAccountName + "', '" + transDate +
            //      "','" + PayRequest.transactionTime + "', '" + PayResponse.responseCode + "', '" + PayResponse.responseDescription + "', '" + errorMsg +
            //      "','" + PayResponse.JurnalSeq + "', '" + ip + "', '" + PayRequest.noReferral + "')";

            util.cmdSQLScalar(sql);
        }

        public string GetSourceAccountTelkom(string institutionCode, string featureCode)
        {
            string result;
            Util util = new Util();
            string sql;

            sql = "SELECT TOP(1) SOURCE_ACCOUNT FROM FEATUREMAP WITH (NOLOCK) WHERE INSTITUTION_CODE='" + institutionCode + "' AND FEATURE_CODE = '" + featureCode + "'";
            DataTable dt = util.setDataTable(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["SOURCE_ACCOUNT"].ToString().Trim();
                return result.PadLeft(15, '0');
            }
            else
            {
                result = "";
                return result;
            }
        }

        #region INQUIRY TELKOM
        public TelkomModels.PSWServiceInquiryResponse PSWServiceInquiryTelkom(ref TelkomModels.TelkomInquiryRequest requestParam)
        {
            string url = ConstantModels.URLINQPAY_TELKOM;

            string _requestInq = "[{\"Transdate\":\"" + DateTime.Now.ToString("dd/MM/y") + "\",\"Transtime\":\"" + DateTime.Now.ToString("HH:mm:ss") + "\",\"ChannelID\":\"" +
                                requestParam.ChannelID + "\",\"ProductID\":\"" + requestParam.ProductID + "\",\"SubProduct\":\"" + requestParam.SubProduct + "\",\"SequenceTrx\"" +
                                ":\"" + requestParam.SequenceTransaction + "\",\"InputData\":\"" + requestParam.BillingNumber + "\",\"Data1\":\"" + requestParam.SourceAccount + 
                                "\",\"Key\":\"" + requestParam.Key + "\"}]";
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

            //WebProxy proxy = new WebProxy("172.18.104.20", 1707);
            //request.Proxy = proxy;

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
            string request = "REQUEST : Transdate=" + DateTime.Now.ToString("dd/MM/y") + "|Transtime=" + DateTime.Now.ToString("HH:mm:ss") + "|ChannelID=" + AutoInqRequest.ChannelID +
                             "|ProductID=" + AutoInqRequest.ProductID + "|SubProduct=" + AutoInqRequest.SubProduct + "|SequenceTrx=" + AutoInqRequest.SequenceTransaction + 
                             "|InputData=" + AutoInqRequest.BillingNumber + "|Data1=" + AutoInqRequest.SourceAccount + "|Key=" + AutoInqRequest.Key + "|IP=" + ip;
            helper.logging(AutoInqRequest.InstitutionCode, "APIPAYMENT_INQUIRYTELKOM", request);

            string wsStartTime = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);

            TelkomHelper telkomHelper = new TelkomHelper();
            TelkomModels.TelkomInquiryResponse AutoInqResponse = new TelkomModels.TelkomInquiryResponse();
            TelkomModels.PSWServiceInquiryResponse GetInqResponse = new TelkomModels.PSWServiceInquiryResponse();

            Random random = new Random();
            AutoInqRequest.SequenceTransaction = DateTime.Now.ToString("HHmmssfff") + random.Next(0, 9).ToString();
            AutoInqRequest.SourceAccount = telkomHelper.GetSourceAccountTelkom(AutoInqRequest.InstitutionCode, ConstantModels.FeatureCode_Telkom);

            GetInqResponse = PSWServiceInquiryTelkom(ref AutoInqRequest);

            if (GetInqResponse == null)
            {
                AutoInqResponse.responseCode = "99";
                AutoInqResponse.responseDescription = "Inquiry gagal";
                AutoInqResponse.errorDescription = "General Error";
            }
            else if (GetInqResponse.RC == "00") //success
            {
                double totalAmount = 0;
                List<TelkomModels.TelkomBillingDetailsData> billDetail = new List<TelkomModels.TelkomBillingDetailsData>();

                string[] splitData1 = ((GetInqResponse.Data1.Trim()).Replace("||", "~")).Split('~');
                int countData1 = splitData1.Length - 1;

                AutoInqResponse.responseCode = ConstantModels.SUCCESSCODEINQ;
                AutoInqResponse.responseDescription = ResponseCodeModels.GetResponseDescription(ConstantModels.SUCCESSCODEINQ);
                AutoInqResponse.data.name = splitData1[0].ToString();
                AutoInqResponse.data.billingCode = GetInqResponse.Data2;

                for (int i = 1; i <= countData1; i++)
                {
                    string[] splitBillDetail = splitData1[i].Trim().Split('#');
                    int countBillDetail = splitBillDetail.Length;
                    totalAmount += Convert.ToDouble(splitBillDetail[1].ToString());
                                        
                    TelkomModels.TelkomBillingDetailsData listBillDetail = new TelkomModels.TelkomBillingDetailsData();
                    listBillDetail.referenceNumber = splitBillDetail[0].ToString();
                    listBillDetail.amount = Convert.ToDouble(splitBillDetail[1].ToString());
                    billDetail.Add(listBillDetail);
                }
                AutoInqResponse.data.totalAmount = totalAmount;
                AutoInqResponse.data.billingDetail = billDetail;
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
            telkomHelper.InsertLogInquiryTelkom(AutoInqRequest, AutoInqResponse, wsStartTime, wsEndTime, ip, GetInqResponse.RC);

            return AutoInqResponse;
        }
        #endregion

        #region PAYMENT TELKOM
        public TelkomModels.PSWServicePaymentResponse PSWServicePaymentTelkom(ref TelkomModels.TelkomPaymentRequest requestParam)
        {
            string url = ConstantModels.URLINQPAY_TELKOM;

            string _requestInq = "[{\"Transdate\":\"" + requestParam.TransactionDate + "\",\"Transtime\":\"" + requestParam.TransactionTime + "\",\"ChannelID\":\"" +
                                requestParam.ChannelID + "\",\"ProductID\":\"" + requestParam.ProductID + "\",\"SubProduct\":\"" + requestParam.SubProduct + "\",\"SequenceTrx\"" +
                                ":\"" + requestParam.SequenceTransaction + "\",\"TotalAmount\":\"" + requestParam.TotalAmount + "\",\"AddAmount1\":\"" + requestParam.FirstBill + 
                                "\",\"AddAmount2\":\"" + requestParam.SecondBill + "\",\"AddAmount3\":\"" + requestParam.ThirdBill + "\",\"InputData\":\"" + requestParam.BillingNumber + 
                                "\",\"Data1\":\"" + requestParam.Name + "\",\"Data2\":\"" + requestParam.BillingCode + "\",\"Key\":\"" + requestParam.Key + "\"}]";
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

            //WebProxy proxy = new WebProxy("172.18.104.20", 1707);
            //request.Proxy = proxy;

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
                             "|TotalAmount=" + AutoPayRequest.TotalAmount + "|AddAmount1=" + AutoPayRequest.FirstBill + "|AddAmount2=" + AutoPayRequest.SecondBill + 
                             "|AddAmount3=" + AutoPayRequest.ThirdBill + "|InputData=" + AutoPayRequest.BillingNumber + "|Data1=" + AutoPayRequest.SourceAccount + 
                             "|Data2=" + AutoPayRequest.Name + "|Data3=" + AutoPayRequest.BillingCode + "|Key=" + AutoPayRequest.Key + 
                             "|ReferralNumber=" + AutoPayRequest.ReferralNumber + "|IP=" + ip;
            helper.logging(AutoPayRequest.InstitutionCode, "WSOVERBOOKING_PAYMENTTELKOM", request);

            string wsStartTime = DateTime.Now.ToString(ConstantModels.FORMATDATETIME); //Hanum new

            TelkomHelper telkomHelper = new TelkomHelper();
            TelkomModels.TelkomPaymentResponse AutoPayResponse = new TelkomModels.TelkomPaymentResponse();
            TelkomModels.PSWServicePaymentResponse GetPayResponse = new TelkomModels.PSWServicePaymentResponse();

            Random random = new Random();
            AutoPayRequest.SequenceTransaction = DateTime.Now.ToString("HHmmssfff") + random.Next(0, 9).ToString();
            AutoPayRequest.SourceAccount = telkomHelper.GetSourceAccountTelkom(AutoPayRequest.InstitutionCode, ConstantModels.FeatureCode_Telkom);

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
                AutoPayResponse.responseDescription = ResponseCodeModels.GetResponseDescription(ConstantModels.SUCCESSCODEPAY);
                AutoPayResponse.data.JurnalSeq = GetPayResponse.JurnalSeq.Trim();
            }
            else //fail
            {
                AutoPayResponse.responseCode = ConstantModels.FAILEDCODEPAY;//ResponseCodeModels.GetResponseCode(GetInqResponse.RC);
                AutoPayResponse.responseDescription = "Payment gagal";
                AutoPayResponse.errorDescription = GetPayResponse.Description;
            }

            //else if (GetPayResponse.RC == "68")
            //{
            //    AutoPayResponse.responseCode = ResponseCodeModels.GetResponseCodePayment(GetPayResponse.RC);
            //    AutoPayResponse.responseDescription = "Do transfer online sedang diproses";
            //    AutoPayResponse.errorDescription = ResponseCodeModels.GetResponseDescription(AutoPayResponse.responseCode);
            //}

            //insert ke tabel TELKOMTRANSACTION
            telkomHelper.InsertTelkomTransaction(AutoPayRequest, AutoPayResponse, wsStartTime, wsEndTime, ip, GetPayResponse.RC);

            string response = "RESPONSE : ResponseCode=" + AutoPayResponse.responseCode + "-" + GetPayResponse.RC + "|ResponseDescription=" + AutoPayResponse.responseDescription + "|ErrDesc=" + AutoPayResponse.errorDescription;// + "|JurnalSeq=" + AutoPayResponse.JurnalSeq;
            helper.logging(AutoPayRequest.InstitutionCode, "APIPAYMENT_PAYMENTTELKOM", response);

            return AutoPayResponse;
        }
        #endregion
    }
}
