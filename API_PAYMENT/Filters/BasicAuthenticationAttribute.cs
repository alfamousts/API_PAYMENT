﻿using System;
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
using System.IO;
using System.Threading.Tasks;

namespace API_PAYMENT.Filters
{
    public class BasicAuthenticationAttribute : AuthorizationFilterAttribute
    {
        private masiril.kasihmas hajar = new masiril.kasihmas();

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            Helper helper = new Helper();
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
                string sourceIP = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString(); //InstitutionCredentials.IP();
                bool allowedIP = helper.CheckIP(sourceIP, institutionCode);

                //Insert ke tabel activity log
                string datetime = DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
                string urlHit = actionContext.Request.RequestUri.ToString();
                string method = actionContext.Request.Method.ToString();
                string requestBody = new StreamReader(System.Web.HttpContext.Current.Request.InputStream).ReadToEnd();

                //Thread.Sleep(10000);
                Task.Factory.StartNew(() => ActivityLog(institutionCode, institutionKey, urlHit, sourceIP, datetime, method, requestBody));
                //helper.InsertActivityLog(institutionCode, institutionKey, urlHit, sourceIP, datetime, method, requestBody);

                //Check Institution Credential
                DataTable dtInst = helper.GetParameterInstitusiDT(institutionCode, "INSTITUTION_SHORTNAME, INSTITUTION_KEY");
                if (dtInst.Rows.Count > 0)
                {
                    key = hajar.sikat(dtInst.Rows[0]["INSTITUTION_KEY"].ToString(), "cashpickup" + dtInst.Rows[0]["INSTITUTION_SHORTNAME"].ToString());
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

                    if (String.IsNullOrEmpty(institutionCode))
                    {
                        response = new CredentialModels("0006");
                    }
                    else if (String.IsNullOrEmpty(institutionKey))
                    {
                        response = new CredentialModels("0007");
                    }
                    else if (dtInst.Rows.Count < 1)
                    {
                        response = new CredentialModels("0008");
                    }
                    else if (institutionKey != key)
                    {
                        response = new CredentialModels("0008");
                    }
                    else if (!allowedIP)
                    {
                        response = new CredentialModels("0009");
                    }

                    actionContext.Response =
                       actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,
                         response);

                    return;
                }
            }

            HandleUnathorized(actionContext);
        }

        private static async Task<string> ActivityLog(string institutionCode, string institutionKey, string urlHit,
            string sourceIP, string datetime, string method, string requestBody)
        {
            Helper helper = new Helper();
            //Thread.Sleep(600000);
            helper.InsertActivityLog(institutionCode, institutionKey, urlHit, sourceIP, datetime, method, requestBody);
            return "OK";
        }

        private static void HandleUnathorized(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
            actionContext.Response.Headers.Add("WWW-Authenticate", "Basic Scheme='Data' location = 'http://localhost:");
        }
    }
}
