using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Text;

namespace API_PAYMENT.Models
{
    /// <summary>
    /// Credit card models.
    /// </summary>
    #region CCmodel
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


        public class PSWServiceInqResponse
        {
            public string RC;
            public string Description;
            public string Data1;
            public string Data2;
        }

        public class PSWServicePayResponse
        {
            public string RC;
            public string Description;
            public string JurnalSeq;
        }

        public class PSWServiceInqCCOtherResponse
        {
            public string RC;
            public string Description;
            public string NamaNasabah;
            public string KodeBank;
            public string NamaBank;
            public string OtherData;
        }

        public class inqData
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string cardName { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string bankName { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string billingAmount { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string minimumPayment { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string maturityDate { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string dataKey { get; set; }
        }

        public class payData
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string cardNumber { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string reference { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string journalSeq { get; set; }
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
            public inqData data { get; set; } = new inqData();
        }

        public class CreditCardPaymentRequest
        {
            [Required]
            public string cardNumber { get; set; }
            [Required]
            public string cardName { get; set; }
            [Required]
            public string amount { get; set; }
            [Required]
            public string reference { get; set; }
            [Required]
            public string dataKey { get; set; }
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
            public payData data { get; set; } = new payData();
        }
        //add Credit Card models request and response here
    }
    #endregion
    /// <summary>
    /// Credit card helper.
    /// </summary>
    public class CreditCardHelper
    {
        public CreditCardHelper()
        {
        }


        public static CreditCardModels.CreditCardInquiryRespone InquiryCC(ref CreditCardModels.CreditCardInquiryRequest requestInq, string featureCode)
        {
            string url = ConstantModels.URLINQPAY;
            string bankName = "";
            string bankCode = "";
            CreditCardModels.PSWRequest pswReq = new CreditCardModels.PSWRequest();
            CreditCardModels.CreditCardInquiryRespone responseInq = new CreditCardModels.CreditCardInquiryRespone();
            CreditCardModels.PSWServiceInqResponse pswRes = new CreditCardModels.PSWServiceInqResponse();
            CreditCardModels.PSWServiceInqCCOtherResponse pswResOther = new CreditCardModels.PSWServiceInqCCOtherResponse();
            DataTable dt = new DataTable();
            dt = GetBinMap(requestInq.cardNumber.Substring(0, 6));
            if (dt != null)
            {
                bankCode = dt.Rows[0]["BANK_CODE"].ToString();
                bankName = dt.Rows[0]["BANK_NAME"].ToString();
            }
            switch (bankCode.PadLeft(3, '0'))
            {
                case "002":
                    pswReq.Key = ConstantModels.Key_CCBRI;
                    pswReq.ProductID = ConstantModels.ProductID_CCBRI;
                    pswReq.SubProduct = ConstantModels.SubProductINQ;
                    break;
                default:
                    pswReq.Key = ConstantModels.Key_CCBRI;
                    pswReq.ProductID = ConstantModels.ProductID_CCOTHER;
                    pswReq.SubProduct = ConstantModels.SubProductINQOther;
                    break;
            }


            pswReq.InputData = requestInq.cardNumber;
            Random rand = new Random();
            pswReq.SequenceTrx = DateTime.Now.ToString("HHmmssfff") + rand.Next(0, 9).ToString(); //harusnya get ke DB


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
            if (bankCode == "002")
            {
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream answerStream = response.GetResponseStream();
                        StreamReader answerReader = new StreamReader(answerStream);
                        String jsonAnswer = answerReader.ReadToEnd();
                        pswRes = JsonConvert.DeserializeObject<CreditCardModels.PSWServiceInqResponse>(jsonAnswer);
                        responseInq.responseCode = pswRes.RC;
                        responseInq.responseDescription = pswRes.Description;
                        if (pswRes.RC == "00")
                        {
                            responseInq.responseCode = "0100";
                            responseInq.responseDescription = ResponseCodeModels.GetResponseDescription(responseInq.responseCode);
                            //responseInq.errorDescription = ResponseCodeModels.GetResponseDescription(responseInq.responseCode);

                            string pswresData = pswRes.Data1.Replace("||", "~");
                            string[] data = pswresData.Split('~');
                            responseInq.data.cardName = data[0];
                            responseInq.data.bankName = "BRI";
                            responseInq.data.billingAmount = data[1].Substring(0,1) + data[1].Substring(1).TrimStart('0');
                            responseInq.data.minimumPayment = data[2].TrimStart('0');
                            responseInq.data.maturityDate = data[3];
                            responseInq.data.dataKey = Base64Encode(pswRes.Data1);
                        }
                        else
                        {
                            responseInq.responseCode = ResponseCodeModels.GetResponseCodePSW(pswRes.RC);
                            responseInq.responseDescription = ResponseCodeModels.GetResponseDescription(responseInq.responseCode);
                        }
                    }
                    else
                    {
                        responseInq.responseCode = "0102";
                        responseInq.responseDescription = ResponseCodeModels.GetResponseDescription(responseInq.responseCode);
                    }
                }
                catch (Exception ex)
                {
                    responseInq.responseCode = "81";
                    responseInq.responseDescription = ResponseCodeModels.GetResponseDescription(responseInq.responseCode);

                }
                string wsEndTime = DateTime.Now.ToString();
                InsertLogInquiryCC(requestInq, responseInq, pswReq, pswRes, wsEndTime, wsStartTime, System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString());
                return responseInq;
            }
            #region ccotherbank 
            else //otherbank, fikri
            {
                try
                {
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream answerStream = response.GetResponseStream();
                        StreamReader answerReader = new StreamReader(answerStream);
                        String jsonAnswer = answerReader.ReadToEnd();
                        pswResOther = JsonConvert.DeserializeObject<CreditCardModels.PSWServiceInqCCOtherResponse>(jsonAnswer);
                        responseInq.responseCode = pswResOther.RC;
                        responseInq.responseDescription = pswResOther.Description;
                        if (pswResOther.RC == "00")
                        {
                            responseInq.responseCode = "0100";
                            responseInq.responseDescription = ResponseCodeModels.GetResponseDescription(responseInq.responseCode);
                            //responseInq.errorDescription = ResponseCodeModels.GetResponseDescription(responseInq.responseCode);

                            responseInq.data.cardName = pswResOther.NamaNasabah;
                            responseInq.data.bankName = pswResOther.NamaBank;
                            try
                            {
                                string pswresData = pswResOther.OtherData.Replace("||", "~");
                                string[] data = pswresData.Split('~');
                                responseInq.data.billingAmount = (data[4] == "C"? "+" : "-") + data[5].TrimStart('0');
                                responseInq.data.minimumPayment = data[6].TrimStart('0');
                                responseInq.data.maturityDate = data[7];
                                responseInq.data.dataKey = Base64Encode(pswResOther.OtherData);
                            }
                            catch
                            {
                                responseInq.data.billingAmount = responseInq.data.minimumPayment = responseInq.data.maturityDate = "";
                                responseInq.data.dataKey = Base64Encode(pswResOther.OtherData);
                            }

                        }
                        else
                        {
                            responseInq.responseCode = ResponseCodeModels.GetResponseCodePSW(pswResOther.RC);
                            responseInq.responseDescription = ResponseCodeModels.GetResponseDescription(responseInq.responseCode);
                        }
                    }
                    else
                    {
                        responseInq.responseCode = "0102";
                        responseInq.responseDescription = ResponseCodeModels.GetResponseDescription(responseInq.responseCode);
                    }
                }
                catch (Exception ex)
                {
                    responseInq.responseCode = "81";
                    responseInq.responseDescription = ResponseCodeModels.GetResponseDescription(responseInq.responseCode);

                }
                string wsEndTime = DateTime.Now.ToString();
                InsertLogInquiryCCOther(requestInq, responseInq, pswReq, pswResOther, wsEndTime, wsStartTime, System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString());
                return responseInq;
            }
            #endregion
        }

        public static CreditCardModels.CreditCardPaymentRespone PaymentCC(ref CreditCardModels.CreditCardPaymentRequest requestPay, string featureCode)
        {
            string url = ConstantModels.URLINQPAY;
            string bankCode = "";
            string bankName = "";
            CreditCardModels.PSWRequest pswReq = new CreditCardModels.PSWRequest();
            CreditCardModels.CreditCardPaymentRespone responsePay = new CreditCardModels.CreditCardPaymentRespone();
            CreditCardModels.PSWServicePayResponse pswRes = new CreditCardModels.PSWServicePayResponse();
            DataTable dt = new DataTable();
            dt = GetBinMap(requestPay.cardNumber.Substring(0, 6));
            if (dt != null)
            {
                bankCode = dt.Rows[0]["BANK_CODE"].ToString();
                bankName = dt.Rows[0]["BANK_NAME"].ToString();
            }
            switch (bankCode.PadLeft(3, '0'))
            {
                case "002":
                    pswReq.Key = ConstantModels.Key_CCBRI;
                    pswReq.ProductID = ConstantModels.ProductID_CCBRI;
                    pswReq.SubProduct = ConstantModels.SubProductPAY;
                    pswReq.Data2 = requestPay.cardName;
                    break;
                default:
                    pswReq.Key = ConstantModels.Key_CCBRI;
                    pswReq.ProductID = ConstantModels.ProductID_CCOTHER;
                    pswReq.SubProduct = ConstantModels.SubProductPAYOther;
                    pswReq.Data2 = requestPay.cardName;
                    pswReq.Data2 = Base64Decode(requestPay.dataKey);
                    break;
            }
            pswReq.InputData = requestPay.cardNumber;
            Random rand = new Random();
            pswReq.SequenceTrx = DateTime.Now.ToString("HHmmssfff") + rand.Next(0, 9).ToString(); //harusnya get ke DB
            pswReq.TotalAmount = requestPay.amount;
            pswReq.Data1 = GetSourceAccount(requestPay.instiutionCode, featureCode); //helper.getrekdb();




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

            string wsStartTime = DateTime.Now.ToString();
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    Stream answerStream = response.GetResponseStream();
                    StreamReader answerReader = new StreamReader(answerStream);
                    String jsonAnswer = answerReader.ReadToEnd();
                    pswRes = JsonConvert.DeserializeObject<CreditCardModels.PSWServicePayResponse>(jsonAnswer);

                    if (pswRes.RC == "00")
                    {
                        responsePay.data.cardNumber = requestPay.cardNumber;
                        responsePay.data.reference = requestPay.reference;
                        responsePay.data.journalSeq = pswRes.JurnalSeq;
                        responsePay.responseCode = "0200";
                        responsePay.responseDescription = ResponseCodeModels.GetResponseDescription(responsePay.responseCode);
                    }
                    else
                    {
                        responsePay.responseCode = ResponseCodeModels.GetResponseCodePSW(pswRes.RC);
                        responsePay.responseDescription = ResponseCodeModels.GetResponseDescription(responsePay.responseCode);
                    }
                    // responsePay.data.Data2 = respond.Data2;

                }
                else
                {
                    responsePay.responseCode = "0202";
                    responsePay.responseDescription = ResponseCodeModels.GetResponseDescription(responsePay.responseCode);

                }
            }
            catch (Exception ex)
            {
                responsePay.responseCode = "81";
                responsePay.responseDescription = ResponseCodeModels.GetResponseDescription(responsePay.responseCode);

            }

            string wsEndTime = DateTime.Now.ToString();
            InsertTransactionCC(requestPay, responsePay, pswReq, pswRes, wsStartTime, wsEndTime, System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString(), bankName);
            return responsePay;

        }

        public static string GetSourceAccount(string kodeInst, string featureCode)
        {
            Util util = new Util();
            //util.ConnectToApplicationDbase();
            string acct;
            string sql;

            sql = "SELECT * FROM FEATUREMAP with (nolock) WHERE INSTITUTION_CODE = '" + kodeInst + "' and FEATURE_CODE = '" + featureCode + "'";
            DataTable dt = util.setDataTable(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                acct = dt.Rows[0]["SOURCE_ACCOUNT"].ToString().Trim();
                return acct.PadLeft(15, '0');
            }
            else
            {
                acct = "";
                return acct;
            }
        }

        public static DataTable GetBinMap(string bincode)
        {
            Util util = new Util();
            string BANK_CODE = "";

            string sql = "SELECT * FROM BINMAP with (nolock) WHERE BIN_CODE = '" + bincode + "'";
            DataTable dt = util.setDataTable(sql);
            if (dt.Rows.Count > 0)
                return dt;
            //BANK_CODE = dt.Rows[0]["BANK_CODE"].ToString();
            else
                return null;
        }

        public static void InsertLogInquiryCC(CreditCardModels.CreditCardInquiryRequest InqRequest, CreditCardModels.CreditCardInquiryRespone InqResponse, CreditCardModels.PSWRequest pswReq, CreditCardModels.PSWServiceInqResponse pswRes, string wsStartTime, string wsEndTime, string ip)
        {
            Util util = new Util();
            string name;
            string minimum_pay;
            string billing;
            string maturity_date;
            if (pswRes.Data1 != null && pswRes.Data1 != "")
            {
                string pswresData = pswRes.Data1.Replace("||", "~");
                string[] data = pswresData.Split('~');
                name = data[0];
                billing = data[1].Substring(0, 1) + data[1].Substring(1).TrimStart('0');
                minimum_pay = data[2].TrimStart('0');
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

            string sql = "INSERT INTO CCINQUIRYLOG ([CREATEDTIME],[WS_STARTTIME],[WS_ENDTIME],[ACTION],[INSTITUTION_CODE],[CHANNEL_ID],[PRODUCT_ID]," +
                  "[SUB_PRODUCT],[SEQUENCE_TRX],[CC_NUM],[KEY],[NAME],[BANK],[BILLING],[MINIMUM_PAYMENT],[MATURITY_DATE],[RC],[RC_DESC],[ERRMSG],[IP_ADDRESS]) " +
                  "VALUES (@createdTime, @wsStartTime, @wsEndTime, @action, @institutionCode, @channelId, @productId, @subProduct, @sequenceTrx, @ccNumber, " +
                  "@key, @name, @bank, @billing, @minimumPayment, @maturityDate, @rc, @rcDesc, @errmsg, @ip)";


            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = sql;
            sqlCommand.Parameters.Add("@createdTime", SqlDbType.VarChar).Value = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);
            sqlCommand.Parameters.Add("@wsStartTime", SqlDbType.VarChar).Value = wsStartTime;
            sqlCommand.Parameters.Add("@wsEndTime", SqlDbType.VarChar).Value = wsEndTime;
            sqlCommand.Parameters.Add("@action", SqlDbType.VarChar).Value = "INQUIRY_CC";
            sqlCommand.Parameters.Add("@institutionCode", SqlDbType.VarChar).Value = InqRequest.instiutionCode;
            sqlCommand.Parameters.Add("@channelId", SqlDbType.VarChar).Value = pswReq.ChannelID;
            sqlCommand.Parameters.Add("@productId", SqlDbType.VarChar).Value = pswReq.ProductID;
            sqlCommand.Parameters.Add("@subProduct", SqlDbType.VarChar).Value = pswReq.SubProduct;
            sqlCommand.Parameters.Add("@sequenceTrx", SqlDbType.VarChar).Value = pswReq.SequenceTrx;
            sqlCommand.Parameters.Add("@ccNumber", SqlDbType.VarChar).Value = pswReq.InputData;
            sqlCommand.Parameters.Add("@key", SqlDbType.VarChar).Value = pswReq.Key;
            sqlCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
            sqlCommand.Parameters.Add("@bank", SqlDbType.VarChar).Value = "BRI";
            sqlCommand.Parameters.Add("@billing", SqlDbType.VarChar).Value = billing;
            sqlCommand.Parameters.Add("@minimumPayment", SqlDbType.VarChar).Value = minimum_pay;
            sqlCommand.Parameters.Add("@maturityDate", SqlDbType.VarChar).Value = maturity_date;
            sqlCommand.Parameters.Add("@rc", SqlDbType.VarChar).Value = InqResponse.responseCode;
            sqlCommand.Parameters.Add("@rcDesc", SqlDbType.VarChar).Value = InqResponse.responseDescription;
            sqlCommand.Parameters.Add("@errmsg", SqlDbType.VarChar).Value = errMsg;
            sqlCommand.Parameters.Add("@ip", SqlDbType.VarChar).Value = ip;
            util.ExecuteSqlCommand(sqlCommand);
        }

        public static void InsertLogInquiryCCOther(CreditCardModels.CreditCardInquiryRequest InqRequest, CreditCardModels.CreditCardInquiryRespone InqResponse, CreditCardModels.PSWRequest pswReq, CreditCardModels.PSWServiceInqCCOtherResponse pswResOther, string wsStartTime, string wsEndTime, string ip)
        {
            Util util = new Util();
            string name;
            string bankName;
            string minimum_pay;
            string billing;
            string maturity_date;
            if (pswResOther.OtherData != null && pswResOther.OtherData != "")
            {
                try
                {
                    string pswresData = pswResOther.OtherData.Replace("||", "~");
                    string[] data = pswresData.Split('~');
                    name = pswResOther.NamaNasabah;
                    bankName = pswResOther.NamaBank;
                    billing = (data[4] == "C" ? "+" : "-") + data[5].TrimStart('0');
                    minimum_pay = data[6];
                    maturity_date = data[7];
                }
                catch
                {
                    name = pswResOther.NamaNasabah;
              billing =
              minimum_pay =
              maturity_date =
              bankName = pswResOther.NamaBank;
                }
            }
            else
            {
                name =
                billing =
                minimum_pay =
                maturity_date =
                bankName = "-";
            }
            string errMsg = "";



            string sql = "INSERT INTO CCINQUIRYLOG ([CREATEDTIME],[WS_STARTTIME],[WS_ENDTIME],[ACTION],[INSTITUTION_CODE],[CHANNEL_ID],[PRODUCT_ID]," +
                  "[SUB_PRODUCT],[SEQUENCE_TRX],[CC_NUM],[KEY],[NAME],[BANK],[BILLING],[MINIMUM_PAYMENT],[MATURITY_DATE],[RC],[RC_DESC],[ERRMSG],[OTHER_DATA],[IP_ADDRESS]) " +
                  "VALUES (@createdTime, @wsStartTime, @wsEndTime, @action, @institutionCode, @channelId, @productId, @subProduct, @sequenceTrx, @ccNumber," +
                  "@key, @name, @bank, @billing, @minimumPayment, @maturityDate, @rc, @rcDesc, @errmsg, @otherData, @ip)";

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = sql;
            sqlCommand.Parameters.Add("@createdTime", SqlDbType.VarChar).Value = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);
            sqlCommand.Parameters.Add("@wsStartTime", SqlDbType.VarChar).Value = wsStartTime;
            sqlCommand.Parameters.Add("@wsEndTime", SqlDbType.VarChar).Value = wsEndTime;
            sqlCommand.Parameters.Add("@action", SqlDbType.VarChar).Value = "INQUIRY_CC";
            sqlCommand.Parameters.Add("@institutionCode", SqlDbType.VarChar).Value = InqRequest.instiutionCode;
            sqlCommand.Parameters.Add("@channelId", SqlDbType.VarChar).Value = pswReq.ChannelID;
            sqlCommand.Parameters.Add("@productId", SqlDbType.VarChar).Value = pswReq.ProductID;
            sqlCommand.Parameters.Add("@subProduct", SqlDbType.VarChar).Value = pswReq.SubProduct;
            sqlCommand.Parameters.Add("@sequenceTrx", SqlDbType.VarChar).Value = pswReq.SequenceTrx;
            sqlCommand.Parameters.Add("@ccNumber", SqlDbType.VarChar).Value = pswReq.InputData;
            sqlCommand.Parameters.Add("@key", SqlDbType.VarChar).Value = pswReq.Key;
            sqlCommand.Parameters.Add("@name", SqlDbType.VarChar).Value = name;
            sqlCommand.Parameters.Add("@bank", SqlDbType.VarChar).Value = bankName;
            sqlCommand.Parameters.Add("@billing", SqlDbType.VarChar).Value = billing;
            sqlCommand.Parameters.Add("@minimumPayment", SqlDbType.VarChar).Value = minimum_pay;
            sqlCommand.Parameters.Add("@maturityDate", SqlDbType.VarChar).Value = maturity_date;
            sqlCommand.Parameters.Add("@rc", SqlDbType.VarChar).Value = InqResponse.responseCode;
            sqlCommand.Parameters.Add("@rcDesc", SqlDbType.VarChar).Value = InqResponse.responseDescription;
            sqlCommand.Parameters.Add("@errmsg", SqlDbType.VarChar).Value = errMsg;
            sqlCommand.Parameters.Add("@otherData", SqlDbType.VarChar).Value = pswResOther.OtherData;
            sqlCommand.Parameters.Add("@ip", SqlDbType.VarChar).Value = ip;
            util.ExecuteSqlCommand(sqlCommand);
        }

        public static void InsertTransactionCC(CreditCardModels.CreditCardPaymentRequest PayRequest, CreditCardModels.CreditCardPaymentRespone PayResponse, CreditCardModels.PSWRequest pswReq, CreditCardModels.PSWServicePayResponse pswRes, string wsStartTime, string wsEndTime, string ip, string bankName)
        {
            Util util = new Util();

            string errMsg = "";


            string sql = "INSERT INTO CCTRANSACTION ([CREATEDTIME],[WS_STARTTIME],[WS_ENDTIME],[INSTITUTION_CODE],[CC_TYPE],[SEQUENCE_TRX],[TOTAL_AMOUNT]," +
                 "[CARD_NUMBER],[NAMA],[BANK],[TRANSACTION_DATE],[TRANSACTION_TIME],[RC],[RC_DESC],[ERRMSG],[JURNALSEQ],[IP_ADDRESS],[NOMOR_REFF])" +
                  "VALUES (@createdTime, @wsStartTime, @wsEndTime, @institutionCode, @ccType, @sequenceTrx, @totalAmount," +
                 "@cardNumber, @nama, @bank, @transactionDate, @transactionTime, @rc, @rcDesc, @errmsg, @jurnalSeq, @ip, @nomorReff)";

            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandType = CommandType.Text;
            sqlCommand.CommandText = sql;
            sqlCommand.Parameters.Add("@createdTime", SqlDbType.VarChar).Value = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);
            sqlCommand.Parameters.Add("@wsStartTime", SqlDbType.VarChar).Value = wsStartTime;
            sqlCommand.Parameters.Add("@wsEndTime", SqlDbType.VarChar).Value = wsEndTime;
            sqlCommand.Parameters.Add("@institutionCode", SqlDbType.VarChar).Value = PayRequest.instiutionCode;
            sqlCommand.Parameters.Add("@ccType", SqlDbType.VarChar).Value = pswReq.ProductID;
            sqlCommand.Parameters.Add("@sequenceTrx", SqlDbType.VarChar).Value = pswReq.SequenceTrx;
            sqlCommand.Parameters.Add("@totalAmount", SqlDbType.Decimal).Value = Convert.ToDecimal(PayRequest.amount);
            sqlCommand.Parameters.Add("@cardNumber", SqlDbType.VarChar).Value = PayRequest.cardNumber;
            sqlCommand.Parameters.Add("@nama", SqlDbType.VarChar).Value = PayRequest.cardName;
            sqlCommand.Parameters.Add("@bank", SqlDbType.VarChar).Value = bankName;
            sqlCommand.Parameters.Add("@transactionDate", SqlDbType.VarChar).Value = DateTime.Now.ToShortDateString();
            sqlCommand.Parameters.Add("@transactionTime", SqlDbType.VarChar).Value = DateTime.Now.ToShortTimeString();
            sqlCommand.Parameters.Add("@rc", SqlDbType.VarChar).Value = PayResponse.responseCode;
            sqlCommand.Parameters.Add("@rcDesc", SqlDbType.VarChar).Value = PayResponse.responseDescription;
            sqlCommand.Parameters.Add("@errmsg", SqlDbType.VarChar).Value = errMsg;
            sqlCommand.Parameters.Add("@jurnalSeq", SqlDbType.VarChar).Value = pswRes.JurnalSeq;
            sqlCommand.Parameters.Add("@ip", SqlDbType.VarChar).Value = ip;
            sqlCommand.Parameters.Add("@nomorReff", SqlDbType.VarChar).Value = PayRequest.reference;
            util.ExecuteSqlCommand(sqlCommand);
        }

        public static Boolean CheckReferenceCreditCard(string noref, string kodeInst)
        {
            Boolean result;
            Util util = new Util();

            SqlCommand sqlCommand = new SqlCommand("SELECT * FROM CCTRANSACTION WITH (NOLOCK) WHERE NOMOR_REFF = @noref AND RC = '0200' AND INSTITUTION_CODE = @institutionCode");
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

        public static string Base64Encode(string plainText)
        {
            //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(Zip(plainText));
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            // System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
            return Unzip(base64EncodedBytes);
        }

        public static byte[] Zip(string str)
        {
            var bytes = Encoding.UTF8.GetBytes(str);

            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(mso, CompressionMode.Compress))
                {
                    //msi.CopyTo(gs);
                    CopyTo(msi, gs);
                }

                return mso.ToArray();
            }
        }

        public static string Unzip(byte[] bytes)
        {
            using (var msi = new MemoryStream(bytes))
            using (var mso = new MemoryStream())
            {
                using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                {
                    //gs.CopyTo(mso);
                    CopyTo(gs, mso);
                }

                return Encoding.UTF8.GetString(mso.ToArray());
            }
        }

        public static void CopyTo(Stream src, Stream dest)
        {
            byte[] bytes = new byte[4096];

            int cnt;

            while ((cnt = src.Read(bytes, 0, bytes.Length)) != 0)
            {
                dest.Write(bytes, 0, cnt);
            }
        }
    }
}
