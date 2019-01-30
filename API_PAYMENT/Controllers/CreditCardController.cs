using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_PAYMENT.Models;
using System.ComponentModel.DataAnnotations;
using API_PAYMENT.Filters;

namespace API_PAYMENT.Controllers
{
    [BasicAuthentication]
    public class CreditCardController : ApiController
    {
        // GET: api/CreditCard


        // POST: api/CreditCard
        public IHttpActionResult Post([FromBody]CreditCardModels.CreditCardPaymentRequest request)
        {
            if (request == null)
            {
                return BadRequest();
            }
            CreditCardModels.CreditCardPaymentRespone response = new CreditCardModels.CreditCardPaymentRespone();

            var context = new ValidationContext(Request, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var authHeader = Request.Headers.Authorization;
            string featureCode = "";

            request.instiutionCode = InstitutionCredentials.InstitutionCode(authHeader);
            request.instiutionKey = InstitutionCredentials.InstitutionKey(authHeader);

            var isValid = Validator.TryValidateObject(Request, context, validationResults);
            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    response.responseCode = "01";
                    response.responseDescription += validationResult.ErrorMessage;
                }
                return Ok(response);
            }

            featureCode = "00001";

            string rc = CreditCardHelper.ValidateInputPaymentCreditCard(ref request, featureCode);//telkomHelper.ValidateInputInquiryTelkom(ref request, IP);

            if (rc.Equals("0005"))
            {
                response = CreditCardHelper.PaymentCC(ref request, featureCode); //accountOnline.inquiryAccountOnline(ref request, IP);
            }
            else
            {
                response.responseCode = rc;
                response.responseDescription = "Inquiry failed";
                response.errorDescription = ResponseCodeModels.GetResponseDescription(response.responseCode);
            }

            return Ok(response);
        }

        // PUT: api/CreditCard/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/CreditCard/5
        public void Delete(int id)
        {
        }

        public IHttpActionResult Get(string cardNumber, string issuerBank)
        {
            CreditCardModels.CreditCardInquiryRequest request = new CreditCardModels.CreditCardInquiryRequest();
            CreditCardModels.CreditCardInquiryRespone response = new CreditCardModels.CreditCardInquiryRespone();

            request.cardNumber = cardNumber;
            request.issuerBank = issuerBank;

            if (cardNumber == null || cardNumber == "" || issuerBank == null || issuerBank == "")
            {
                return BadRequest();
            }


            var context = new ValidationContext(Request, serviceProvider: null, items: null);
            var validationResults = new List<ValidationResult>();
            var authHeader = Request.Headers.Authorization;
            string featureCode = "";

            request.instiutionCode = InstitutionCredentials.InstitutionCode(authHeader);
            request.instiutionKey = InstitutionCredentials.InstitutionKey(authHeader);

            var isValid = Validator.TryValidateObject(Request, context, validationResults);
            if (!isValid)
            {
                foreach (var validationResult in validationResults)
                {
                    response.responseCode = "01";
                    response.responseDescription += validationResult.ErrorMessage;
                }
                return Ok(response);
            }

            featureCode = "00001";

            string rc = CreditCardHelper.ValidateInputInquiryCreditCard(ref request, featureCode);//telkomHelper.ValidateInputInquiryTelkom(ref request, IP);

            if (rc.Equals("0005"))
            {
                response = CreditCardHelper.InquiryCC(ref request, featureCode); //accountOnline.inquiryAccountOnline(ref request, IP);
            }
            else
            {
                response.responseCode = rc;
                response.responseDescription = "Inquiry failed";
                response.errorDescription = ResponseCodeModels.GetResponseDescription(response.responseCode);
            }

            return Ok(response);
        }
    }
}
