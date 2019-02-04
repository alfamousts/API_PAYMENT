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
            
            var context = new ValidationContext(Request, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();

            var authHeader = Request.Headers.Authorization;

            billingNumber = (billingNumber is null ? "" : billingNumber);
            
            request.institutionCode = InstitutionCredentials.InstitutionCode(authHeader);
            request.institutionKey = InstitutionCredentials.InstitutionKey(authHeader);
            request.billingNumber = billingNumber;

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

            string rc = telkomValidation.ValidateInputInquiryTelkom(ref request);

            if (rc.Equals("0005"))
            {
                result = telkomHelper.InquiryTelkom(ref request, IP);
            }
            else
            {
                result.responseCode = rc;
                result.responseDescription = ResponseCodeModels.GetResponseDescription(rc);
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

            request.institutionCode = InstitutionCredentials.InstitutionCode(authHeader);
            request.institutionKey = InstitutionCredentials.InstitutionKey(authHeader);
            request.totalAmount = (request.totalAmount is null ? "" : request.totalAmount);
            request.billingNumber = (request.billingNumber is null ? "" : request.billingNumber);
            request.billingCode = (request.billingCode is null ? "" : request.billingCode);
            request.reference = (request.reference is null ? "" : request.reference);
            
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

            string rc = telkomValidation.ValidatePaymentTelkom(ref request);

            if (rc.Equals("0005"))
            {
                result = telkomHelper.PaymentTelkom(ref request, IP);
            }
            else
            {
                result.responseCode = rc;
                result.responseDescription = ResponseCodeModels.GetResponseDescription(rc);
            }

            return Ok(result);
        }
    }
}
