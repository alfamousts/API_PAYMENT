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
        public class PSWServiceRequest
        {
            [StringLength(20)]
            public string Transdate { get; set; } = DateTime.Now.ToString("dd/MM/y");
            [StringLength(20)]
            public string Transtime { get; set; } = DateTime.Now.ToString("HH:mm:ss");
            [StringLength(20)]
            public string ChannelID { get; set; } = ConstantModels.ChannelID_CCBRI;
            [StringLength(20)]
            public string ProductID { get; set; } = ConstantModels.ProductID_Telkom;
            [StringLength(20)]
            public string SubProduct { get; set; }
            [StringLength(20)]
            public string SequenceTrx{ get; set; }
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
            public string InputData { get; set; }
            [Required]
            [StringLength(100)]
            public string Data1 { get; set; } = "";
            [StringLength(100)]
            public string Data2 { get; set; } = "-";
            [StringLength(100)]
            public string Data3 { get; set; } = "";
            [Required]
            [StringLength(100)]
            public string Remark { get; set; } = "";
            [StringLength(100)]
            public string Key { get; set; } = ConstantModels.Key_Telkom;
        }

        public class TelkomInquiryRequest
        {
            [StringLength(20)]
            public string institutionCode { get; set; }
            [StringLength(100)]
            public string institutionKey { get; set; }
            [Required]
            [StringLength(100)]
            public string billingNumber { get; set; } //InputData
        }

        public class TelkomInquiryResponse
        {
            public string responseCode { get; set; }
            public string responseDescription { get; set; }
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
            public string amount { get; set; }
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
            public string institutionCode { get; set; }
            [StringLength(100)]
            public string institutionKey { get; set; }
            [Required]
            [StringLength(20)]
            public string totalAmount { get; set; }
            [Required]
            [StringLength(20)]
            public string firstBill { get; set; } 
            [Required]
            [StringLength(20)]
            public string secondBill { get; set; } 
            [Required]
            [StringLength(20)]
            public string thirdBill { get; set; } 
            [Required]
            [StringLength(100)]
            public string billingNumber { get; set; } //InputData
            [StringLength(100)]
            public string name { get; set; } //Data2
            [Required]
            [StringLength(100)]
            public string billingCode { get; set; }
            [Required]
            [StringLength(100)]
            public string reference { get; set; }
        }

        public class TelkomPaymentResponse
        {
            public string responseCode { get; set; }
            public string responseDescription { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public TelkomPaymentResponseData data { get; set; } = new TelkomPaymentResponseData();
        }

        public class TelkomPaymentResponseData
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string billingNumber { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string reference { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string journalSeq { get; set; }
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
        Util util = new Util();

        public Boolean CheckReferenceTelkom(string noref, string kodeInst)
        {
            Boolean result;
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

        public void InsertLogInquiryTelkom(TelkomModels.TelkomInquiryRequest InqRequest, TelkomModels.PSWServiceRequest PswRequest, TelkomModels.TelkomInquiryResponse InqResponse, string wsStartTime, string wsEndTime, string ip, string rc_psw)
        {
            string sql;
            string errMsg = "";
            if (InqResponse.responseCode != "0100")
            {
                errMsg = "";
            }

            sql = "INSERT INTO TELKOMINQUIRYLOG ([CREATEDTIME],[WS_STARTTIME],[WS_ENDTIME],[ACTION],[INSTITUTION_CODE],[TRANSACTION_DATE],[TRANSACTION_TIME]" +
                ",[CHANNEL_ID],[PRODUCT_ID],[SUB_PRODUCT],[SEQUENCE_TRX],[BILLING_NUMBER],[SOURCE_ACCOUNT],[KEY],[RC],[RC_DESC],[ERRMSG],[IP_ADDRESS]) " +
                  "VALUES ('" + DateTime.Now.ToString(ConstantModels.FORMATDATETIME) + "', '" + wsStartTime + "', '" + wsEndTime + "', 'INQUIRY'," +
                  "'" + InqRequest.institutionCode + "', '" + PswRequest.Transdate + "', '" + PswRequest.Transtime + "', '" + PswRequest.ChannelID +
                  "','" + PswRequest.ProductID + "', '" + PswRequest.SubProduct + "', '" + PswRequest.SequenceTrx + "', '" + InqRequest.billingNumber +
                  "', '" + PswRequest.Data1 + "', '" + PswRequest.Key + "', '" + InqResponse.responseCode + "','" + InqResponse.responseDescription +
                  "','" + errMsg + "','" + ip + "')";

            util.cmdSQLScalar(sql);
        }

        public void InsertTelkomTransaction(TelkomModels.TelkomPaymentRequest PayRequest, TelkomModels.PSWServiceRequest PswRequest, TelkomModels.TelkomPaymentResponse PayResponse, string wsStartTime, string wsEndTime, string ip, string rc_psw)
        {
            string sql;
            //string transDate = DateTime.Parse(PayRequest.TransactionDate).ToString("dd-MM-yyyy");
            string errorMsg = "";
            if (PayResponse.responseCode != "0200")
            {
                errorMsg = "";
            }

            sql = "INSERT INTO TELKOMTRANSACTION ([CREATEDTIME],[WS_STARTTIME],[WS_ENDTIME],[INSTITUTION_CODE],[SEQUENCE_TRX],[TOTAL_AMOUNT],[FIRST_BILL]" +
                ",[SECOND_BILL],[THIRD_BILL],[BILLING_NUMBER],[SOURCE_ACCOUNT],[NAME],[BILLING_CODE],[TRANSACTION_DATE],[TRANSACTION_TIME],[RC],[RC_DESC]" +
                ",[ERRMSG],[JURNALSEQ],[IP_ADDRESS],[NOMOR_REFF]) " +
                  "VALUES ('" + DateTime.Now.ToString(ConstantModels.FORMATDATETIME) + "','" + wsStartTime + "', '" + wsEndTime + "', '" + PayRequest.institutionCode + 
                  "', '" + PswRequest.SequenceTrx + "', '" + PayRequest.totalAmount + "', '" + PayRequest.firstBill + "','" + PayRequest.secondBill + 
                  "', '" + PayRequest.thirdBill + "', '" + PayRequest.billingNumber + "','" + PswRequest.Data1 + "', '" + PayRequest.name + 
                  "', '" + PayRequest.billingCode + "', '" + PswRequest.Transdate + "', '" + PswRequest.Transtime + "', '" + PayResponse.responseCode + 
                  "', '" + PayResponse.responseDescription + "', '" + errorMsg + "','" + PayResponse.data.journalSeq + "', '" + ip + "', '" + PayRequest.reference + "')";

            util.cmdSQLScalar(sql);
        }

        public string GetSourceAccountTelkom(string institutionCode, string featureCode)
        {
            string result;
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
        public TelkomModels.PSWServiceInquiryResponse PSWServiceInquiryTelkom(ref TelkomModels.PSWServiceRequest requestParam)
        {
            string url = ConstantModels.URLINQPAY_TELKOM;

            string _requestInq = "[" + JsonConvert.SerializeObject(requestParam) + "]";
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
            string wsStartTime = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);

            TelkomHelper telkomHelper = new TelkomHelper();
            TelkomModels.PSWServiceRequest PswRequest = new TelkomModels.PSWServiceRequest();
            TelkomModels.TelkomInquiryResponse AutoInqResponse = new TelkomModels.TelkomInquiryResponse();
            TelkomModels.PSWServiceInquiryResponse GetInqResponse = new TelkomModels.PSWServiceInquiryResponse();

            Random random = new Random();
            PswRequest.SubProduct = ConstantModels.SubProductINQ_Telkom;
            PswRequest.SequenceTrx = DateTime.Now.ToString("HHmmssfff") + random.Next(0, 9).ToString();
            PswRequest.InputData = AutoInqRequest.billingNumber;
            PswRequest.Data1 = telkomHelper.GetSourceAccountTelkom(AutoInqRequest.institutionCode, ConstantModels.FeatureCode_Telkom);

            GetInqResponse = PSWServiceInquiryTelkom(ref PswRequest);

            if (GetInqResponse == null)
            {
                AutoInqResponse.responseCode = ConstantModels.TIMEOUTCODEINQ;
                AutoInqResponse.responseDescription = ResponseCodeModels.GetResponseDescription(ConstantModels.TIMEOUTCODEINQ);
            }
            else if (GetInqResponse.RC == "00") //success
            {
                double totalAmount = 0;
                List<TelkomModels.TelkomBillingDetailsData> billDetail = new List<TelkomModels.TelkomBillingDetailsData>();

                string[] splitData1 = ((GetInqResponse.Data1).Replace("||", "~")).Split('~');
                int countData1 = splitData1.Length - 1;

                AutoInqResponse.responseCode = ConstantModels.SUCCESSCODEINQ;
                AutoInqResponse.responseDescription = ResponseCodeModels.GetResponseDescription(ConstantModels.SUCCESSCODEINQ);
                AutoInqResponse.data.name = splitData1[0].ToString();
                AutoInqResponse.data.billingCode = GetInqResponse.Data2;

                for (int i = 1; i <= countData1; i++)
                {
                    string[] splitBillDetail = splitData1[i].Split('#');
                    totalAmount += Convert.ToDouble(String.IsNullOrEmpty(splitBillDetail[1].ToString()) ? "0" : splitBillDetail[1].ToString());
                                        
                    TelkomModels.TelkomBillingDetailsData listBillDetail = new TelkomModels.TelkomBillingDetailsData();

                    if (!String.IsNullOrEmpty(splitBillDetail[0].ToString()) || !String.IsNullOrEmpty(splitBillDetail[1].ToString()))
                    {
                        listBillDetail.referenceNumber =  splitBillDetail[0].ToString();
                        listBillDetail.amount = splitBillDetail[1].ToString();
                        billDetail.Add(listBillDetail);
                    }
                }
                AutoInqResponse.data.totalAmount = totalAmount;
                AutoInqResponse.data.billingDetail = billDetail;
            }
            else //fail
            {
                AutoInqResponse.responseCode = ResponseCodeModels.GetResponseCodePSW(GetInqResponse.RC);
                AutoInqResponse.responseDescription = ResponseCodeModels.GetResponseDescription(AutoInqResponse.responseCode);
            }

            string wsEndTime = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);
            string response = "RESPONSE : ResponseCode=" + AutoInqResponse.responseCode + "|ResponseDescription=" + AutoInqResponse.responseDescription;
            helper.logging(AutoInqRequest.institutionCode, "APIPAYMENT_INQUIRYTELKOM", response);
            telkomHelper.InsertLogInquiryTelkom(AutoInqRequest, PswRequest, AutoInqResponse, wsStartTime, wsEndTime, ip, GetInqResponse.RC);

            return AutoInqResponse;
        }
        #endregion

        #region PAYMENT TELKOM
        public TelkomModels.PSWServicePaymentResponse PSWServicePaymentTelkom(ref TelkomModels.PSWServiceRequest requestParam)
        {
            string url = ConstantModels.URLINQPAY_TELKOM;

            string _requestInq = "[" + JsonConvert.SerializeObject(requestParam) + "]";
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
            string wsStartTime = DateTime.Now.ToString(ConstantModels.FORMATDATETIME); //Hanum new

            TelkomHelper telkomHelper = new TelkomHelper();
            TelkomModels.PSWServiceRequest PswRequest = new TelkomModels.PSWServiceRequest();
            TelkomModels.TelkomPaymentResponse AutoPayResponse = new TelkomModels.TelkomPaymentResponse();
            TelkomModels.PSWServicePaymentResponse GetPayResponse = new TelkomModels.PSWServicePaymentResponse();

            Random random = new Random();
            PswRequest.SubProduct = ConstantModels.SubProductPAY_Telkom;
            PswRequest.SequenceTrx = DateTime.Now.ToString("HHmmssfff") + random.Next(0, 9).ToString();
            PswRequest.TotalAmount = AutoPayRequest.totalAmount;
            PswRequest.AddAmount1 = AutoPayRequest.firstBill;
            PswRequest.AddAmount2 = AutoPayRequest.secondBill;
            PswRequest.AddAmount3 = AutoPayRequest.thirdBill;
            PswRequest.InputData = AutoPayRequest.billingNumber;
            PswRequest.Data1 = telkomHelper.GetSourceAccountTelkom(AutoPayRequest.institutionCode, ConstantModels.FeatureCode_Telkom);;
            PswRequest.Data2 = AutoPayRequest.name;
            PswRequest.Data3 = AutoPayRequest.billingCode;

            GetPayResponse = PSWServicePaymentTelkom(ref PswRequest);
            string wsEndTime = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);

            if (GetPayResponse == null)
            {
                AutoPayResponse.responseCode = ConstantModels.TIMEOUTCODEPAY;
                AutoPayResponse.responseDescription = ResponseCodeModels.GetResponseDescription(ConstantModels.TIMEOUTCODEPAY);
            }

            if (GetPayResponse.RC == "00") //success
            {
                AutoPayResponse.responseCode = ConstantModels.SUCCESSCODEPAY;
                AutoPayResponse.responseDescription = ResponseCodeModels.GetResponseDescription(ConstantModels.SUCCESSCODEPAY);
                AutoPayResponse.data.billingNumber = AutoPayRequest.billingNumber;
                AutoPayResponse.data.reference = AutoPayRequest.reference;
                AutoPayResponse.data.journalSeq = GetPayResponse.JurnalSeq.Trim();
            }
            else //fail
            {
                AutoPayResponse.responseCode = ResponseCodeModels.GetResponseCodePSW(GetPayResponse.RC);
                AutoPayResponse.responseDescription = ResponseCodeModels.GetResponseDescription(AutoPayResponse.responseCode);
            }

            //insert ke tabel TELKOMTRANSACTION
            telkomHelper.InsertTelkomTransaction(AutoPayRequest, PswRequest, AutoPayResponse, wsStartTime, wsEndTime, ip, GetPayResponse.RC);

            string response = "RESPONSE : ResponseCode=" + AutoPayResponse.responseCode + "-" + GetPayResponse.RC + "|ResponseDescription=" + AutoPayResponse.responseDescription;
            helper.logging(AutoPayRequest.institutionCode, "APIPAYMENT_PAYMENTTELKOM", response);

            return AutoPayResponse;
        }
        #endregion
    }
}
