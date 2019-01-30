using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IO;
using System.Net;
using System.Text;

namespace API_PAYMENT.Models
{
    /// <summary>
    /// Credit card models.
    /// </summary>
    public class CreditCardModels
    {
        public CreditCardModels()
        {
        }
        public class PSWRequest
        {
            [StringLength(20)]
            public string Transdate { get; set; } = DateTime.Now.ToString("dd/MM/y");
            [StringLength(20)]
            public string Transtime { get; set; } = DateTime.Now.ToString("HH:mm:ss");
            [StringLength(20)]
            public string ChannelID { get; set; } = ConstantModels.ChannelID_CCBRI;
            [StringLength(20)]
            public string ProductID { get; set; }
            [StringLength(20)]
            public string SubProduct { get; set; }
            [StringLength(20)]
            public string SequenceTrx { get; set; }
            [Required]
            [StringLength(20)]
            public string TotalAmount { get; set; } = "0";
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
            public string InputData { get; set; } //InputData
            [Required]
            [StringLength(100)]
            public string Data1 { get; set; } = "";
            [StringLength(100)]
            public string Data2 { get; set; } = "-";
            [Required]
            [StringLength(100)]
            public string Remark { get; set; } = "";
            [StringLength(100)]
            public string Key { get; set; }
        }
        

        public class PSWServiceResponse
        {
            public string RC;
            public string Description;
            public string Data1;
            public string Data2;
        }

        public class inqData
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string name { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string billingAmount { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string minimumPayment { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string maturityDate { get; set; }
        }

        public class payData
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string name { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string billingAmount { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string minimumPayment { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string maturityDate { get; set; }
        }

        public class CreditCardInquiryRequest
        {
            [Required]
            public string cardNumber { get; set; }
            [Required]
            public string issuerBank { get; set; }
            [Required]
            public string instiutionCode { get; set; }
            [Required]
            public string instiutionKey { get; set; }
        }

        public class CreditCardInquiryRespone
        {
            public string responseCode { get; set; }
            public string responseDescription { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string errorDescription { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public inqData data { get; set; } = new inqData();
        }

        public class CreditCardPaymentRequest
        {
            [Required]
            public string cardNumber { get; set; }
            [Required]
            public string issuerBank { get; set; }
            [Required]
            public string cardName { get; set; }
            [Required]
            public string amount { get; set; }
            [Required]
            public string reference { get; set; }
            [Required]
            public string instiutionCode { get; set; }
            [Required]
            public string instiutionKey { get; set; }
        }

        public class CreditCardPaymentRespone
        {
            public string responseCode { get; set; }
            public string responseDescription { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string errorDescription { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public payData data { get; set; } = new payData();
        }
        //add Credit Card models request and response here
    }

    /// <summary>
    /// Credit card helper.
    /// </summary>
    public class CreditCardHelper
    { 
        public CreditCardHelper()
        {
        }

        public static string ValidateInputInquiryCreditCard(ref CreditCardModels.CreditCardInquiryRequest requestInq, string featureCode)
        {
            Helper helper = new Helper();
            if (!helper.featureCek(requestInq.instiutionCode, featureCode))
            {
                return "0011";
            }
            else
            {
                return "0005";
            }
        }

        public static string ValidateInputPaymentCreditCard(ref CreditCardModels.CreditCardPaymentRequest requestPay, string featureCode)
        {
            Helper helper = new Helper();
            if (!helper.featureCek(requestPay.instiutionCode, featureCode))
            {
                return "0011";
            }
            else
            {
                return "0005";
            }
        }

        public static CreditCardModels.CreditCardInquiryRespone InquiryCC(ref CreditCardModels.CreditCardInquiryRequest requestInq, string featureCode)
        {
            string url = ConstantModels.URLINQPAY;
            CreditCardModels.PSWRequest pswReq = new CreditCardModels.PSWRequest();
            CreditCardModels.CreditCardInquiryRespone responseInq = new CreditCardModels.CreditCardInquiryRespone();
            CreditCardModels.PSWServiceResponse respond = new CreditCardModels.PSWServiceResponse();

            string bankCode = GetBinMap(requestInq.cardNumber.Substring(0, 6));

            switch (bankCode)
            {
                case "BRI":
                    pswReq.Key = ConstantModels.Key_CCBRI;
                    pswReq.ProductID = ConstantModels.ProductID_CCBRI;
                    pswReq.SubProduct = ConstantModels.SubProductINQ;
                    break;
                case "BCA":
                    pswReq.Key = ConstantModels.Key_CCBRI;
                    pswReq.ProductID = ConstantModels.ProductID_CCBRI;
                    pswReq.SubProduct = ConstantModels.SubProductINQ;
                    break;
                case "BNI":
                    pswReq.Key = ConstantModels.Key_CCBRI;
                    pswReq.ProductID = ConstantModels.ProductID_CCBRI;
                    pswReq.SubProduct = ConstantModels.SubProductINQ;
                    break;
                default:
                    pswReq.Key = "";
                    pswReq.ProductID = "";
                    pswReq.SubProduct = "";
                    break;
            }


            pswReq.InputData = requestInq.cardNumber;
            pswReq.Key = ConstantModels.Key_CCBRI;
            pswReq.ProductID = ConstantModels.ProductID_CCBRI;
            pswReq.SubProduct = ConstantModels.SubProductINQ;
            Random rand = new Random();
            pswReq.SequenceTrx = DateTime.Now.ToString("HHmmssfff") + rand.Next(0,9).ToString(); //harusnya get ke DB


            string _requestInq = "data=" + "[" + JsonConvert.SerializeObject(pswReq) + "]";


            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] postBytes = ascii.GetBytes(_requestInq);
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

            string wsStartTime = DateTime.Now.ToString();
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
                    respond = JsonConvert.DeserializeObject<CreditCardModels.PSWServiceResponse>(jsonAnswer);
                    responseInq.responseCode = respond.RC;
                    responseInq.responseDescription = respond.Description;
                    if (respond.RC == "00")
                    {
                        responseInq.responseCode = "0100";
                        responseInq.responseDescription = "Inquiry Success";
                        //responseInq.errorDescription = ResponseCodeModels.GetResponseDescription(responseInq.responseCode);

                        string pswresData = respond.Data1.Replace("||", "~");
                        string[] data = pswresData.Split('~');
                        responseInq.data.name = data[0];
                        responseInq.data.billingAmount = data[1];
                        responseInq.data.minimumPayment = data[2];
                        responseInq.data.maturityDate = data[3];
                    }
                    else
                    {
                        responseInq.responseCode = "0101";
                        responseInq.responseDescription = "Inquiry Failed";
                        responseInq.errorDescription = ResponseCodeModels.GetResponseDescription(responseInq.responseCode);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
            string wsEndTime = DateTime.Now.ToString();
            InsertLogInquiryCC(requestInq, responseInq, pswReq, respond, wsEndTime, wsStartTime, System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString());
            return responseInq;
        }

        public static CreditCardModels.CreditCardPaymentRespone PaymentCC(ref CreditCardModels.CreditCardPaymentRequest requestPay, string featureCode)
        {
            string url = ConstantModels.URLINQPAY;
            CreditCardModels.PSWRequest pswReq = new CreditCardModels.PSWRequest();
            CreditCardModels.CreditCardPaymentRespone responsePay = new CreditCardModels.CreditCardPaymentRespone();

            pswReq.InputData = requestPay.cardNumber;
            pswReq.Key = ConstantModels.Key_CCBRI;
            pswReq.ProductID = ConstantModels.ProductID_CCBRI;
            pswReq.SubProduct = ConstantModels.SubProductPAY;
            Random rand = new Random();
            pswReq.SequenceTrx = DateTime.Now.ToString("HHmmssfff") + rand.Next(0, 9).ToString(); //harusnya get ke DB
            pswReq.TotalAmount = requestPay.amount;
            pswReq.Data1 = GetSourceAccount(requestPay.instiutionCode,featureCode); //helper.getrekdb();
            pswReq.Data2 = requestPay.cardName;

     
            string _requestInq = "data=" + "[" + JsonConvert.SerializeObject(pswReq) + "]";


            ASCIIEncoding ascii = new ASCIIEncoding();
            byte[] postBytes = ascii.GetBytes(_requestInq);
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
                    CreditCardModels.PSWServiceResponse respond = JsonConvert.DeserializeObject<CreditCardModels.PSWServiceResponse>(jsonAnswer);
                    responsePay.responseCode = respond.RC;
                    responsePay.responseDescription = respond.Description;
                   // responsePay.data.Data1 = respond.Data1;
                   // responsePay.data.Data2 = respond.Data2;

                    return responsePay;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public static string GetSourceAccount(string kodeInst, string featureCode)
        {
            Util util = new Util();
            //util.ConnectToApplicationDbase();
            string acct;
            string sql;

            sql = "SELECT * FROM CCTRANSACTION with (nolock) WHERE INSTITUTION_CODE = '" + kodeInst + "' and FEATURE_CODE = '"+ featureCode +"'";
            DataTable dt = util.setDataTable(sql);

            acct = dt.Rows[0]["SOURCE_ACCOUNT"].ToString();

            return acct;
        }

        public static string GetBinMap(string bincode)
        {
            Util util = new Util();
            //util.ConnectToApplicationDbase();
            string BANK_CODE = "";
            string sql;

            sql = "SELECT * FROM BINMAP with (nolock) WHERE BIN_CODE = '" + bincode + "'";
            DataTable dt = util.setDataTable(sql);
            if(dt.Rows.Count > 0)
            BANK_CODE = dt.Rows[0]["BANK_CODE"].ToString();

            return BANK_CODE;
        }

        public static void InsertLogInquiryCC(CreditCardModels.CreditCardInquiryRequest InqRequest, CreditCardModels.CreditCardInquiryRespone InqResponse, CreditCardModels.PSWRequest pswReq, CreditCardModels.PSWServiceResponse pswRes, string wsStartTime, string wsEndTime, string ip)
        {
            Util util = new Util();
            string sql;
            string name;
            string minimum_pay;
            string billing;
            string maturity_date;
            if (pswRes.Data1 != null)
            {
                string pswresData = pswRes.Data1.Replace("||", "~");
                string[] data = pswresData.Split('~');
                name = data[0];
                billing = data[1];
                minimum_pay = data[2];
                maturity_date = data[3];
            }
            else
            {
                name = "-";
                billing = "-";
                minimum_pay = "-";
                maturity_date = "-";
            }
            string errMsg = "";
            if (InqResponse.responseCode != "0600")
            {
                InqResponse.errorDescription = InqResponse.errorDescription == null ? "" : InqResponse.errorDescription;
                errMsg = ((InqResponse.responseCode == null ? 0 : InqResponse.errorDescription.Length) > 250 ? InqResponse.errorDescription.Substring(0, 250) : InqResponse.errorDescription) + " (" + pswRes.RC + ")";
            }

            sql = "INSERT INTO CCINQUIRYLOG ([CREATEDTIME],[WS_STARTTIME],[WS_ENDTIME],[ACTION],[INSTITUTION_CODE],[CHANNEL_ID],[PRODUCT_ID]," +
                  "[SUB_PRODUCT],[SEQUENCE_TRX],[CC_NUM],[KEY],[NAME],[BILLING],[MINIMUM_PAYMENT],[MATURITY_DATE],[RC],[RC_DESC],[ERRMSG],[IP_ADDRESS]) " +
                  "VALUES ('" + DateTime.Now.ToString(ConstantModels.FORMATDATETIME) + "', '" + wsStartTime + "', '" + wsEndTime + "', 'INQUIRY_CC'," +
                  "'" + InqRequest.instiutionCode + "', '" + InqRequest.instiutionCode + "','" + pswReq.ProductID + "', '" + pswReq.SubProduct + "', '" + pswReq.SequenceTrx + "', '" +
                  pswReq.InputData + "', '" + pswReq.Key + "', '" + name + "','" + billing + "','" + minimum_pay + "','" + maturity_date + "','" + InqResponse.responseCode + "','" + InqResponse.responseDescription +
                  "','" + errMsg + "','" + ip + "')";

            util.cmdSQLScalar(sql);
        }
        //add Credit Card helper here
    }
}
