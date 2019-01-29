using API_PAYMENT.Models;
using API_PAYMENT.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Net.Http;
using System.Web.Mvc;

namespace API_PAYMENT.Controllers
{
    /// <summary>
    ///  Telkom Controller
    /// </summary>
    [BasicAuthentication]
    public class TelkomController : ApiController
    {
        /// <summary>
        ///  Telkom Inquriy
        /// </summary>
        
        //GET: api/Telkom/5
        public TelkomModels.TelkomInquiryResponse Get(string billingNumber, string beneficiaryAccount)
        {
            TelkomModels.TelkomInquiryResponse result = new TelkomModels.TelkomInquiryResponse();
            TelkomModels.TelkomInquiryRequest request = new TelkomModels.TelkomInquiryRequest();
            TelkomHelper telkomHelper = new TelkomHelper();
            //WsAccountOnline accountOnline = new WsAccountOnline();

            var context = new ValidationContext(Request, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            var authHeader = Request.Headers.Authorization;

            billingNumber = (billingNumber is null ? "" : billingNumber);
            beneficiaryAccount = (beneficiaryAccount is null ? "" : beneficiaryAccount);

            request.InstitutionCode = InstitutionCredentials.InstitutionCode(authHeader);
            request.InstitutionKey = InstitutionCredentials.InstitutionKey(authHeader);
            request.BillingNumber = billingNumber;
            request.BeneficiaryAccount = beneficiaryAccount;

            string IP = InstitutionCredentials.IP();

            var isValid = Validator.TryValidateObject(Request, context, validationResults);
            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    result.responseCode = "01";
                    result.responseDescription += validationResult.ErrorMessage;
                }
                return result;
            }

            string rc = "0005";//telkomHelper.ValidateInputInquiryTelkom(ref request, IP);

            if (rc.Equals("0005"))
            {
                result = telkomHelper.InquiryTelkom(ref request, IP); //accountOnline.inquiryAccountOnline(ref request, IP);
            }
            else
            {
                result.responseCode = rc;
                result.responseDescription = "Inquiry gagal";
                result.errorDescription = ResponseCodeModels.GetResponseDescription(result.responseCode);
            }

            return result;
        }

        /// <summary>
        ///  Telkom Payment
        /// </summary>
        
        // POST: api/Telkom
        public IHttpActionResult Post([FromBody]TelkomModels.TelkomPaymentRequest request)
        {
            if (request is null)
            {
                return BadRequest();
            }

            TelkomModels.TelkomPaymentResponse result = new TelkomModels.TelkomPaymentResponse();
            //WsAccountOnline accountOnline = new WsAccountOnline();
            TelkomHelper telkomHelper = new TelkomHelper();

            string IP = InstitutionCredentials.IP();
            decimal number;

            var context = new ValidationContext(Request, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var authHeader = Request.Headers.Authorization;

            request.InstitutionCode = InstitutionCredentials.InstitutionCode(authHeader);
            request.InstitutionKey = InstitutionCredentials.InstitutionKey(authHeader);
            request.TotalAmount = (request.TotalAmount is null ? "" : request.TotalAmount);
            request.FeeAmount = (request.FeeAmount is null ? "" : request.FeeAmount);
            request.AddAmount1 = (request.AddAmount1 is null ? "" : request.AddAmount1);
            request.AddAmount2 = (request.AddAmount2 is null ? "" : request.AddAmount2);
            request.AddAmount3 = (request.AddAmount3 is null ? "" : request.AddAmount3);
            request.BeneficiaryAccount = (request.BeneficiaryAccount is null ? "0".PadLeft(15, '0') : request.BeneficiaryAccount.PadLeft(15, '0'));
            request.BeneficiaryName = (request.BeneficiaryName is null ? "" : request.BeneficiaryName);
            request.Remark = (request.Remark is null ? "" : request.Remark);
            request.ReferralNumber = (request.ReferralNumber is null ? "" : request.ReferralNumber);
            
            var isValid = Validator.TryValidateObject(Request, context, validationResults);
            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    result.responseCode = "01";
                    result.responseDescription += validationResult.ErrorMessage;
                }
                //return result;
                return BadRequest();
            }

            string rc = "0005";// telkomHelper.ValidatePaymentTelkom(ref request, IP);

            if (rc.Equals("0005"))
            {
                result = telkomHelper.PaymentTelkom(ref request, IP);//accountOnline.doTransferOnline(ref request, IP);
            }
            else
            {
                result.responseCode = rc;
                result.responseDescription = "Do transfer online gagal";
                result.errorDescription = ResponseCodeModels.GetResponseDescription(result.responseCode);
            }

            return Ok(result);
        }
    }
}
