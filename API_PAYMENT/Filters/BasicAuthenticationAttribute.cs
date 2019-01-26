using System;
using System.Web;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Data;
using API_PAYMENT.Models;


namespace API_PAYMENT.Filters
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        private masiril.kasihmas hajar = new masiril.kasihmas();

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //WsHelper helper = new WsHelper();
            string key = "";
            var authHeader = actionContext.Request.Headers.Authorization;

            if (authHeader != null)
            {
                var authenticationToken = actionContext.Request.Headers.Authorization.Parameter;
                var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                var usernamePasswordArray = decodedAuthenticationToken.Split(':');
                var institutionCode = usernamePasswordArray[0];
                var institutionKey = usernamePasswordArray[1];

                //Check source IP
                string sourceIP = InstitutionCredentials.IP();//System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
                bool allowedIP = true; //helper.CheckIP(sourceIP, institutionCode);

                //Check Institution Credential
                DataTable dtInst = null;//helper.GetParameterInstitusiDT(institutionCode, "INSTITUTION_LOGIN_NAME, INSTITUTION_KEY, MEMBER_REGISTERED, INST_ACC_VALIDATION");
                if (dtInst.Rows.Count > 0)
                {
                    key = hajar.sikat(dtInst.Rows[0]["INSTITUTION_KEY"].ToString(), "cashpickup" + dtInst.Rows[0]["INSTITUTION_LOGIN_NAME"].ToString());
                }
                // Replace this with your own system of security / means of validating credentials
                var isValid = dtInst.Rows.Count > 0 && institutionKey == key && allowedIP;

                if (isValid)
                {
                    var principal = new GenericPrincipal(new GenericIdentity(institutionCode), null);
                    Thread.CurrentPrincipal = principal;
                    return;
                }
                else
                {
                    CredentialModels response = new CredentialModels();

                    if (institutionCode == "")
                    {
                        response = new CredentialModels("0006");
                    }

                    //else if (dtInst.Rows.Count < 1)
                    //{
                    //    response = new InstitutionCredential("0008");
                    //}
                    //else if (!allowedIP)
                    //{
                    //    response = new InstitutionCredential("0010");
                    //    //response.responseDescription = IPStr;
                    //}
                    //else if (String.IsNullOrEmpty(institutionKey))
                    //{
                    //    response = new InstitutionCredential("0009");
                    //}
                    //else if (institutionKey != key)
                    //{
                    //    response = new CredentialModels("0012");
                    //}

                    actionContext.Response =
                       actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                         response);

                    return;
                }
            }

            HandleUnathorized(actionContext);
        }

        private static void HandleUnathorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='Data' location = 'http://localhost:");
        }
    }
}
