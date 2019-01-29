using API_PAYMENT.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace API_PAYMENT.Controllers
{
    /// <summary>
    ///  Telkom Controller
    /// </summary>
    public class TelkomController : Controller
    {
        // GET: api/AccountInfoOther/5
        //public TelkomModels.TelkomInquiryResponse Get(string cardNumber, string beneficiaryAccount)
        //{
        //    TelkomModels.TelkomInquiryResponse result = new TelkomModels.TelkomInquiryResponse();
        //    TelkomModels.TelkomInquiryRequest request = new TelkomModels.TelkomInquiryRequest();
        //    TelkomHelper helper = new TelkomHelper();
        //    //WsAccountOnline accountOnline = new WsAccountOnline();

        //    var context = new ValidationContext(Request, serviceProvider: null, items: null);
        //    var validationResults = new List<ValidationResult>();

        //    var authHeader = Request.Headers.Authorization;

        //    cardNumber = (cardNumber is null ? "" : cardNumber);
        //    beneficiaryAccount = (beneficiaryAccount is null ? "" : beneficiaryAccount);

        //    request.InstitutionCode = InstitutionCredentials.InstitutionCode(authHeader);
        //    request.InstitutionKey = InstitutionCredentials.InstitutionKey(authHeader);
        //    request.CardNumber = cardNumber;
        //    request.BeneficiaryAccount = beneficiaryAccount;

        //    string IP = InstitutionCredentials.IP();

        //    var isValid = Validator.TryValidateObject(Request, context, validationResults);
        //    if (!isValid)
        //    {
        //        foreach (var validationResult in validationResults)
        //        {
        //            result.responseCode = "01";
        //            result.responseDescription += validationResult.ErrorMessage;
        //        }
        //        return result;
        //    }

        //    string rc = helper.ValidateInputAccountOnline(ref request, IP);

        //    if (rc.Equals("0005"))
        //    {
        //        result = accountOnline.inquiryAccountOnline(ref request, IP);
        //    }
        //    else
        //    {
        //        result.responseCode = rc;
        //        result.responseDescription = "Inquiry account online gagal";
        //        result.errorDescription = ResponseCodeModels.GetResponseDescription(result.responseCode);
        //    }

        //    return result;
        //}
    }
}
