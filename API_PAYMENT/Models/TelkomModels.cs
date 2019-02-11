using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
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
            public string SequenceTrx { get; set; }
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
            public string totalAmount { get; set; }
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
            [StringLength(100)]
            public string billingNumber { get; set; } //InputData
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

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM TELKOMTRANSACTION WITH (NOLOCK) WHERE NOMOR_REFF = @noref AND RC = '0200' AND INSTITUTION_CODE = @institutionCode");
            sqlCommand.Parameters.Add("@noref", SqlDbType.VarChar).Value = noref;
            sqlCommand.Parameters.Add("@institutionCode", SqlDbType.VarChar).Value = kodeInst;
            DataTable dt = util.setDataTable(sqlCommand);

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
            string errMsg = "";
            if (InqResponse.responseCode != "0100")
            {
                errMsg = "";
            }

            string sql = "INSERT INTO TELKOMINQUIRYLOG ([CREATEDTIME],[WS_STARTTIME],[WS_ENDTIME],[ACTION],[INSTITUTION_CODE],[TRANSACTION_DATE],[TRANSACTION_TIME]" +
                ",[CHANNEL_ID],[PRODUCT_ID],[SUB_PRODUCT],[SEQUENCE_TRX],[BILLING_NUMBER],[SOURCE_ACCOUNT],[KEY],[RC],[RC_DESC],[ERRMSG],[IP_ADDRESS]) " +
                  "VALUES (@createdTime, @wsStartTime, @wsEndTime, @action, @institutionCode, @transactionDate, @transactionTime, @channelId, @productId, @subProduct, " +
                  "@sequenceTrx, @billingNumber, @sourceAccount, @key, @rc, @rcDesc, @errmsg, @ip)";

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = sql;
            sqlCommand.Parameters.Add("@createdTime", SqlDbType.VarChar).Value = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);
            sqlCommand.Parameters.Add("@wsStartTime", SqlDbType.VarChar).Value = wsStartTime;
            sqlCommand.Parameters.Add("@wsEndTime", SqlDbType.VarChar).Value = wsEndTime;
            sqlCommand.Parameters.Add("@action", SqlDbType.VarChar).Value = "INQUIRY_TELKOM";
            sqlCommand.Parameters.Add("@institutionCode", SqlDbType.VarChar).Value = InqRequest.institutionCode;
            sqlCommand.Parameters.Add("@transactionDate", SqlDbType.VarChar).Value = PswRequest.Transdate;
            sqlCommand.Parameters.Add("@transactionTime", SqlDbType.VarChar).Value = PswRequest.Transtime;
            sqlCommand.Parameters.Add("@channelId", SqlDbType.VarChar).Value = PswRequest.ChannelID;
            sqlCommand.Parameters.Add("@productId", SqlDbType.VarChar).Value = PswRequest.ProductID;
            sqlCommand.Parameters.Add("@subProduct", SqlDbType.VarChar).Value = PswRequest.SubProduct;
            sqlCommand.Parameters.Add("@sequenceTrx", SqlDbType.VarChar).Value = PswRequest.SequenceTrx;
            sqlCommand.Parameters.Add("@billingNumber", SqlDbType.VarChar).Value = InqRequest.billingNumber;
            sqlCommand.Parameters.Add("@sourceAccount", SqlDbType.VarChar).Value = PswRequest.Data1;
            sqlCommand.Parameters.Add("@key", SqlDbType.VarChar).Value = PswRequest.Key;
            sqlCommand.Parameters.Add("@rc", SqlDbType.VarChar).Value = InqResponse.responseCode;
            sqlCommand.Parameters.Add("@rcDesc", SqlDbType.VarChar).Value = InqResponse.responseDescription;
            sqlCommand.Parameters.Add("@errmsg", SqlDbType.VarChar).Value = errMsg;
            sqlCommand.Parameters.Add("@ip", SqlDbType.VarChar).Value = ip;
            util.ExecuteSqlCommand(sqlCommand);
        }

        public void InsertTelkomTransaction(TelkomModels.TelkomPaymentRequest PayRequest, TelkomModels.PSWServiceRequest PswRequest, TelkomModels.TelkomPaymentResponse PayResponse, string wsStartTime, string wsEndTime, string ip, string rc_psw)
        {
            //string transDate = DateTime.Parse(PayRequest.TransactionDate).ToString("dd-MM-yyyy");
            string errorMsg = "";
            if (PayResponse.responseCode != "0200")
            {
                errorMsg = "";
            }

            string sql = "INSERT INTO TELKOMTRANSACTION ([CREATEDTIME],[WS_STARTTIME],[WS_ENDTIME],[INSTITUTION_CODE],[SEQUENCE_TRX],[TOTAL_AMOUNT],[FIRST_BILL]" +
                ",[SECOND_BILL],[THIRD_BILL],[BILLING_NUMBER],[SOURCE_ACCOUNT],[NAME],[BILLING_CODE],[ENCODE_DATA],[TRANSACTION_DATE],[TRANSACTION_TIME],[RC],[RC_DESC]" +
                ",[ERRMSG],[JURNALSEQ],[IP_ADDRESS],[NOMOR_REFF]) " +
                "VALUES (@createdTime, @wsStartTime, @wsEndTime, @institutionCode, @sequenceTrx, @totalAmount, @firstBill, @secondBill, @thirdBill, " +
                "@billingNumber, @sourceAccount, @name, @billingCode, @encodeData, @transactionDate, @transactionTime, @rc, @rcDesc, @errmsg, @jurnalSeq," +
                "@ip, @nomorReff)";

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = sql;
            sqlCommand.Parameters.Add("@createdTime", SqlDbType.VarChar).Value = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);
            sqlCommand.Parameters.Add("@wsStartTime", SqlDbType.VarChar).Value = wsStartTime;
            sqlCommand.Parameters.Add("@wsEndTime", SqlDbType.VarChar).Value = wsEndTime;
            sqlCommand.Parameters.Add("@institutionCode", SqlDbType.VarChar).Value = PayRequest.institutionCode;
            sqlCommand.Parameters.Add("@sequenceTrx", SqlDbType.VarChar).Value = PswRequest.SequenceTrx;
            sqlCommand.Parameters.Add("@totalAmount", SqlDbType.Decimal).Value = Convert.ToDecimal(PayRequest.totalAmount);
            sqlCommand.Parameters.Add("@firstBill", SqlDbType.VarChar).Value = PswRequest.AddAmount1;
            sqlCommand.Parameters.Add("@secondBill", SqlDbType.VarChar).Value = PswRequest.AddAmount2;
            sqlCommand.Parameters.Add("@thirdBill", SqlDbType.VarChar).Value = PswRequest.AddAmount3;
            sqlCommand.Parameters.Add("@billingNumber", SqlDbType.VarChar).Value = PayRequest.billingNumber;
            sqlCommand.Parameters.Add("@sourceAccount", SqlDbType.VarChar).Value = PswRequest.Data1;
            sqlCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = PswRequest.Data2;
            sqlCommand.Parameters.Add("@billingCode", SqlDbType.VarChar).Value = PswRequest.Data3;
            sqlCommand.Parameters.Add("@encodeData", SqlDbType.VarChar).Value = PayRequest.billingCode;
            sqlCommand.Parameters.Add("@transactionDate", SqlDbType.VarChar).Value = PswRequest.Transdate;
            sqlCommand.Parameters.Add("@transactionTime", SqlDbType.VarChar).Value = PswRequest.Transtime;
            sqlCommand.Parameters.Add("@rc", SqlDbType.VarChar).Value = PayResponse.responseCode;
            sqlCommand.Parameters.Add("@rcDesc", SqlDbType.VarChar).Value = PayResponse.responseDescription;
            sqlCommand.Parameters.Add("@errmsg", SqlDbType.VarChar).Value = errorMsg;
            sqlCommand.Parameters.Add("@jurnalSeq", SqlDbType.VarChar).Value = PayResponse.data.journalSeq;
            sqlCommand.Parameters.Add("@ip", SqlDbType.VarChar).Value = ip;
            sqlCommand.Parameters.Add("@nomorReff", SqlDbType.VarChar).Value = PayRequest.reference;
            util.ExecuteSqlCommand(sqlCommand);
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
                    TelkomModels.PSWServiceInquiryResponse respond = new TelkomModels.PSWServiceInquiryResponse();
                    respond.RC = ConstantModels.TIMEOUTCODEINQ;
                    return respond;
                }
            }catch(Exception ex)
            {
                TelkomModels.PSWServiceInquiryResponse respond = new TelkomModels.PSWServiceInquiryResponse();
                respond.RC = ConstantModels.EXCEPTIONCODEINQ;
                return respond;
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
            PswRequest.Data1 = helper.GetSourceAccount(AutoInqRequest.institutionCode, ConstantModels.FeatureCode_Telkom);

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
                AutoInqResponse.data.name = splitData1[0].ToString().Trim();
                AutoInqResponse.data.billingCode = helper.Base64Encode(GetInqResponse.Data1 + "~" + GetInqResponse.Data2);

                for (int i = 1; i <= countData1; i++)
                {
                    string[] splitBillDetail = splitData1[i].Split('#');
                    totalAmount += Convert.ToDouble(String.IsNullOrEmpty(splitBillDetail[1].ToString()) ? "0" : splitBillDetail[1].ToString());

                    TelkomModels.TelkomBillingDetailsData listBillDetail = new TelkomModels.TelkomBillingDetailsData();

                    if (!String.IsNullOrEmpty(splitBillDetail[0].ToString()) || !String.IsNullOrEmpty(splitBillDetail[1].ToString()))
                    {
                        listBillDetail.referenceNumber = splitBillDetail[0].ToString();
                        listBillDetail.amount = splitBillDetail[1].ToString();
                        billDetail.Add(listBillDetail);
                    }
                }

                AutoInqResponse.data.totalAmount = (totalAmount.ToString() == "0" ? null : totalAmount.ToString());
                AutoInqResponse.data.billingDetail = billDetail;
            }
            else if (GetInqResponse.RC == ConstantModels.TIMEOUTCODEINQ)
            {
                AutoInqResponse.responseCode = ConstantModels.TIMEOUTCODEINQ;
                AutoInqResponse.responseDescription = ResponseCodeModels.GetResponseDescription(GetInqResponse.RC);
            }
            else if (GetInqResponse.RC == ConstantModels.EXCEPTIONCODEINQ)
            {
                AutoInqResponse.responseCode = ConstantModels.EXCEPTIONCODEINQ;
                AutoInqResponse.responseDescription = ResponseCodeModels.GetResponseDescription(GetInqResponse.RC);
            }
            else //fail
            {
                AutoInqResponse.responseCode = ResponseCodeModels.GetResponseCodePSW(GetInqResponse.RC);
                AutoInqResponse.responseDescription = ResponseCodeModels.GetResponseDescription(AutoInqResponse.responseCode);
            }

            string wsEndTime = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);
            telkomHelper.InsertLogInquiryTelkom(AutoInqRequest, PswRequest, AutoInqResponse, wsStartTime, wsEndTime, ip, (GetInqResponse == null ? "" : GetInqResponse.RC));

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
                    TelkomModels.PSWServicePaymentResponse respond = new TelkomModels.PSWServicePaymentResponse();
                    respond.RC = ConstantModels.TIMEOUTCODEPAY;
                    return respond;
                }
            }catch(Exception ex)
            {
                TelkomModels.PSWServicePaymentResponse respond = new TelkomModels.PSWServicePaymentResponse();
                respond.RC = ConstantModels.EXCEPTIONCODEPAY;
                return respond;
            }
        }

        public TelkomModels.TelkomPaymentResponse PaymentTelkom(ref TelkomModels.TelkomPaymentRequest AutoPayRequest, string ip)
        {
            string wsStartTime = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);
            string wsEndTime = "";

            TelkomHelper telkomHelper = new TelkomHelper();
            TelkomModels.PSWServiceRequest PswRequest = new TelkomModels.PSWServiceRequest();
            TelkomModels.TelkomPaymentResponse AutoPayResponse = new TelkomModels.TelkomPaymentResponse();
            TelkomModels.PSWServicePaymentResponse GetPayResponse = new TelkomModels.PSWServicePaymentResponse();

            string decodeBillingCode = helper.Base64Decode(AutoPayRequest.billingCode);

            if (decodeBillingCode == "")
            {
                AutoPayResponse.responseCode = "0210";
                AutoPayResponse.responseDescription = ResponseCodeModels.GetResponseDescription(AutoPayResponse.responseCode);
            }
            else
            {
                string[] splitBillingCode = decodeBillingCode.Split('~');
                string[] splitData1PSW = ((splitBillingCode[0]).Replace("||", "~")).Split('~');

                Random random = new Random();
                PswRequest.SubProduct = ConstantModels.SubProductPAY_Telkom;
                PswRequest.SequenceTrx = DateTime.Now.ToString("HHmmssfff") + random.Next(0, 9).ToString();
                PswRequest.TotalAmount = AutoPayRequest.totalAmount;
                PswRequest.AddAmount1 = splitData1PSW[1];
                PswRequest.AddAmount2 = splitData1PSW[2];
                PswRequest.AddAmount3 = splitData1PSW[3];
                PswRequest.InputData = AutoPayRequest.billingNumber;
                PswRequest.Data1 = helper.GetSourceAccount(AutoPayRequest.institutionCode, ConstantModels.FeatureCode_Telkom);
                PswRequest.Data2 = splitData1PSW[0];
                PswRequest.Data3 = splitBillingCode[1];

                GetPayResponse = PSWServicePaymentTelkom(ref PswRequest);
                wsEndTime = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);

                if (GetPayResponse == null)
                {
                    AutoPayResponse.responseCode = ConstantModels.TIMEOUTCODEPAY;
                    AutoPayResponse.responseDescription = ResponseCodeModels.GetResponseDescription(ConstantModels.TIMEOUTCODEPAY);
                }
                else if (GetPayResponse.RC == "00") //success
                {
                    AutoPayResponse.responseCode = ConstantModels.SUCCESSCODEPAY;
                    AutoPayResponse.responseDescription = ResponseCodeModels.GetResponseDescription(ConstantModels.SUCCESSCODEPAY);
                    AutoPayResponse.data.billingNumber = AutoPayRequest.billingNumber;
                    AutoPayResponse.data.reference = AutoPayRequest.reference;
                    AutoPayResponse.data.journalSeq = GetPayResponse.JurnalSeq.Trim();
                }
                else if (GetPayResponse.RC == ConstantModels.TIMEOUTCODEPAY)
                {
                    AutoPayResponse.responseCode = ConstantModels.TIMEOUTCODEPAY;
                    AutoPayResponse.responseDescription = ResponseCodeModels.GetResponseDescription(GetPayResponse.RC);
                }
                else if (GetPayResponse.RC == ConstantModels.EXCEPTIONCODEPAY)
                {
                    AutoPayResponse.responseCode = ConstantModels.EXCEPTIONCODEPAY;
                    AutoPayResponse.responseDescription = ResponseCodeModels.GetResponseDescription(GetPayResponse.RC);
                }
                else //fail
                {
                    AutoPayResponse.responseCode = ResponseCodeModels.GetResponseCodePSW(GetPayResponse.RC);
                    AutoPayResponse.responseDescription = ResponseCodeModels.GetResponseDescription(AutoPayResponse.responseCode);
                }
            }

            //insert ke tabel TELKOMTRANSACTION
            telkomHelper.InsertTelkomTransaction(AutoPayRequest, PswRequest, AutoPayResponse, wsStartTime, wsEndTime, ip, (GetPayResponse == null ? "" : GetPayResponse.RC));
            return AutoPayResponse;
        }
        #endregion
    }
}
