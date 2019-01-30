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
        
        //GET: api/Telkom
        public TelkomModels.TelkomInquiryResponse Get(string billingNumber)
        {
            TelkomModels.TelkomInquiryResponse result = new TelkomModels.TelkomInquiryResponse();
            TelkomModels.TelkomInquiryRequest request = new TelkomModels.TelkomInquiryRequest();
            TelkomHelper telkomHelper = new TelkomHelper();
            TelkomValidation telkomValidation = new TelkomValidation();
            //WsAccountOnline accountOnline = new WsAccountOnline();

            var context = new ValidationContext(Request, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            var authHeader = Request.Headers.Authorization;

            billingNumber = (billingNumber is null ? "" : billingNumber);
            //beneficiaryAccount = (beneficiaryAccount is null ? "" : beneficiaryAccount);

            request.InstitutionCode = InstitutionCredentials.InstitutionCode(authHeader);
            request.InstitutionKey = InstitutionCredentials.InstitutionKey(authHeader);
            request.BillingNumber = billingNumber;
            request.SourceAccount = telkomHelper.GetSourceAccountTelkom(request.InstitutionCode, ConstantModels.FeatureCode_Telkom);

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

            string rc = telkomValidation.ValidateInputInquiryTelkom(ref request, IP);

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
            TelkomHelper telkomHelper = new TelkomHelper();
            TelkomValidation telkomValidation = new TelkomValidation();

            string IP = InstitutionCredentials.IP();
            
            var context = new ValidationContext(Request, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var authHeader = Request.Headers.Authorization;

            request.InstitutionCode = InstitutionCredentials.InstitutionCode(authHeader);
            request.InstitutionKey = InstitutionCredentials.InstitutionKey(authHeader);
            request.TotalAmount = (request.TotalAmount is null ? "" : request.TotalAmount);
            request.FirstBill = (request.FirstBill is null ? "" : request.FirstBill);
            request.SecondBill = (request.SecondBill is null ? "" : request.SecondBill);
            request.ThirdBill = (request.ThirdBill is null ? "" : request.ThirdBill);
            request.SourceAccount = (request.SourceAccount is null ? "0".PadLeft(15, '0') : request.SourceAccount.PadLeft(15, '0'));
            request.Name = (request.Name is null ? "" : request.Name);
            request.BillingCode = (request.BillingCode is null ? "" : request.BillingCode);
            request.ReferralNumber = (request.ReferralNumber is null ? "" : request.ReferralNumber);
            
            var isValid = Validator.TryValidateObject(Request, context, validationResults);
            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    result.responseCode = "01";
                    result.responseDescription += validationResult.ErrorMessage;
                }
                
                return BadRequest();
            }

            string rc = telkomValidation.ValidatePaymentTelkom(ref request, IP);

            if (rc.Equals("0005"))
            {
                result = telkomHelper.PaymentTelkom(ref request, IP);
            }
            else
            {
                result.responseCode = rc;
                result.responseDescription = "Payment gagal";
                result.errorDescription = ResponseCodeModels.GetResponseDescription(result.responseCode);
            }

            return Ok(result);
        }
    }
}
