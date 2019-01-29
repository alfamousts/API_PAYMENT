using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

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
            public string CardNumber { get; set; } //InputData
            [Required]
            [StringLength(100)]
            public string BeneficiaryAccount { get; set; } //Data1
            [StringLength(100)]
            public string Data2 { get; set; } = "-"; //Data2
            [Required]
            [StringLength(100)]
            public string Remark { get; set; }
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
            public string Data1 { get; set; }
            [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
            public string Data2 { get; set; }
        }

        public class PSWServiceResponse
        {
            public string RC;
            public string Description;
            public string Data1;
            public string Data2;
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

        ////add helper here
        //public GetInquiryAccountOnlineResponse getInquiryAccountOnline(ref InquiryTransferOnlineRequest requestParam)
        //{
        //    string url = ConstantModels.URLINQPAYOTHERBANK; ;

        //    string _requestInq = "[{\"Transdate\":\"" + requestParam.transactionDate + "\",\"Transtime\":\"" + requestParam.transactionTime + "\",\"ChannelID\":\"" +
        //                         requestParam.channelID + "\",\"ProductID\":\"" + requestParam.productID + "\",\"SubProduct\":\"" + requestParam.subProduct + "\",\"TotalAmount\":\"" +
        //                         requestParam.Amount + "\",\"InputData\":\"" + requestParam.beneficiaryAccount + "\",\"Data1\":\"" + requestParam.bankCode + "\",\"Key\":\"" +
        //                         requestParam.keyOtherBank + "\"}]";
        //    string requestInq = "data=" + _requestInq;

        //    ASCIIEncoding ascii = new ASCIIEncoding();
        //    byte[] postBytes = ascii.GetBytes(requestInq);
        //    HttpWebRequest request;

        //    try
        //    {
        //        request = (HttpWebRequest)HttpWebRequest.Create(url);
        //    }
        //    catch (UriFormatException)
        //    {
        //        return null;
        //    }

        //    request.Method = "POST";
        //    request.Accept = "application/x-www-form-urlencoded";
        //    request.ContentType = "application/x-www-form-urlencoded";
        //    request.ContentLength = postBytes.Length;

        //    // add post data to request
        //    Stream postStream = request.GetRequestStream();
        //    postStream.Write(postBytes, 0, postBytes.Length);
        //    postStream.Flush();
        //    postStream.Close();
        //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();
        //    if (response.StatusCode == HttpStatusCode.OK)
        //    {
        //        Stream answerStream = response.GetResponseStream();
        //        StreamReader answerReader = new StreamReader(answerStream);
        //        String jsonAnswer = answerReader.ReadToEnd();
        //        GetInquiryAccountOnlineResponse respond = JsonConvert.DeserializeObject<GetInquiryAccountOnlineResponse>(jsonAnswer);
        //        return respond;
        //    }
        //    else
        //    {
        //        return null;
        //    }
        //}

        ////added by Hanum 4-9-2018
        //public InquiryTransferOnlineResponse inquiryAccountOnline(ref InquiryTransferOnlineRequest AutoInqRequest, string ip)
        //{
        //    string request = "REQUEST : Transdate=" + AutoInqRequest.transactionDate + "|Transtime=" + AutoInqRequest.transactionTime + "|ChannelID=" + AutoInqRequest.channelID +
        //                     "|ProductID=" + AutoInqRequest.productID + "|SubProduct=" + AutoInqRequest.subProduct + "|TotalAmount=" + AutoInqRequest.Amount + "|InputData=" +
        //                     AutoInqRequest.beneficiaryAccount + "|Data1=" + AutoInqRequest.keyOtherBank + "|Key=" + AutoInqRequest.keyOtherBank + "|IP=" + ip;
        //    helper.logging(AutoInqRequest.institutionCode, "WSOVERBOOKING_INQUIRYACCTONLINE", request);

        //    string wsStartTime = DateTime.Now.ToString(ConstantModels.FORMATDATETIME); //Hanum new

        //    InquiryTransferOnlineResponse AutoInqResponse = new InquiryTransferOnlineResponse();
        //    GetInquiryAccountOnlineResponse GetInqResponse = new GetInquiryAccountOnlineResponse();

        //    GetInqResponse = getInquiryAccountOnline(ref AutoInqRequest);

        //    if (GetInqResponse == null)
        //    {
        //        AutoInqResponse.responseCode = "99";
        //        AutoInqResponse.responseDescription = "Inquiry account online gagal";
        //        AutoInqResponse.errorDescription = "General Error";
        //    }

        //    if (GetInqResponse.RC == "00") //success
        //    {
        //        AutoInqResponse.responseCode = "0600";
        //        AutoInqResponse.responseDescription = GetInqResponse.Description;
        //        AutoInqResponse.Name = GetInqResponse.Name.Trim();
        //    }
        //    else //fail
        //    {
        //        AutoInqResponse.responseCode = ResponseCodeModels.GetResponseCode(GetInqResponse.RC);
        //        AutoInqResponse.responseDescription = "Inquiry account online gagal";
        //        AutoInqResponse.errorDescription = GetInqResponse.Description;
        //    }

        //    string wsEndTime = DateTime.Now.ToString(ConstantModels.FORMATDATETIME);
        //    string response = "RESPONSE : ResponseCode=" + AutoInqResponse.responseCode + "|ResponseDescription=" + AutoInqResponse.responseDescription + "|ErrDesc=" + AutoInqResponse.errorDescription + "|Name=" + AutoInqResponse.Name;
        //    helper.logging(AutoInqRequest.institutionCode, "WSOVERBOOKING_INQUIRYACCTONLINE", response);
        //    helper.InsertLogInquiryAccountOnline(AutoInqRequest, AutoInqResponse, wsStartTime, wsEndTime, ip, GetInqResponse.RC);

        //    return AutoInqResponse;
        //}
    }
}
