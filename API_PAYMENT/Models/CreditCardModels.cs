using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
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
        

        public class PSWServiceInquiryResponse
        {
            public string RC;
            public string Description;
            public string Data1;
            public string Data2;
        }

        public class data
        {
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Data1 { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Data2 { get; set; }
        }

        public class CreditCardInquiryRequest
        {
            public string cardNumber { get; set; }
            public string issuerBank { get; set; }
            public string instiutionCode { get; set; }
            public string instiutionKey { get; set; }
        }

        public class CreditCardInquiryRespone
        {
            public string responseCode { get; set; }
            public string responseDescription { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string errorDescription { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public data data { get; set; } = new data();
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
        public static CreditCardModels.CreditCardInquiryRespone InquiryCCBRI(ref CreditCardModels.CreditCardInquiryRequest requestInq)
        {
            string url = ConstantModels.URLINQPAY;
            CreditCardModels.PSWRequest pswReq = new CreditCardModels.PSWRequest();
            CreditCardModels.CreditCardInquiryRespone responseInq = new CreditCardModels.CreditCardInquiryRespone();

            pswReq.InputData = requestInq.cardNumber;
            pswReq.Key = ConstantModels.Key_CCBRI;
            pswReq.ProductID = ConstantModels.ProductID_CCBRI;
            pswReq.SubProduct = ConstantModels.SubProductINQ;
            Random rand = new Random();
            pswReq.SequenceTrx = rand.Next(1231212, 999999999).ToString(); //harusnya get ke DB


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
                    CreditCardModels.PSWServiceInquiryResponse respond = JsonConvert.DeserializeObject<CreditCardModels.PSWServiceInquiryResponse>(jsonAnswer);
                    responseInq.responseCode = respond.RC;
                    responseInq.responseDescription = respond.Description;
                    responseInq.data.Data1 = respond.Data1;
                    responseInq.data.Data2 = respond.Data2;

                    return responseInq;
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



        //add Credit Card helper here
    }
}
