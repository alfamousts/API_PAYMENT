using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace API_PAYMENT.Models
{
    /// <summary>
    /// Models for API credentials response 
    /// </summary>
    public class CredentialModels
    {
        public string responseCode;
        public string responseDescription;

        public CredentialModels()
        {

        }

        public CredentialModels(string _responseCode)
        {
            responseCode = _responseCode;
            responseDescription = ResponseCodeModels.GetResponseDescription(_responseCode);
        }
    }

    /// <summary>
    /// Institution credentials helper
    /// </summary>
    public class InstitutionCredentials
    {
        /// <summary>
        /// get institution conde from authorization header value
        /// </summary>
        /// <param name="authHeader"></param>
        /// <returns>string institution code</returns>
        public static string InstitutionCode(AuthenticationHeaderValue authHeader)
        {
            string result = "";
            if (authHeader != null)
            {
                var authenticationToken = authHeader.Parameter;
                var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                var usernamePasswordArray = decodedAuthenticationToken.Split(':');
                result = usernamePasswordArray[0];
            }
            return result;
        }

        /// <summary>
        /// get institution key from authorization header value
        /// </summary>
        /// <param name="authHeader"></param>
        /// <returns>string institution key</returns>
        public static string InstitutionKey(AuthenticationHeaderValue authHeader)
        {
            string result = "";
            if (authHeader != null)
            {
                var authenticationToken = authHeader.Parameter;
                var decodedAuthenticationToken = Encoding.UTF8.GetString(Convert.FromBase64String(authenticationToken));
                var usernamePasswordArray = decodedAuthenticationToken.Split(':');
                result = usernamePasswordArray[1];
            }
            return result;
        }

        /// <summary>
        /// get client IP address
        /// </summary>
        /// <returns>string client IP address</returns>
        public static string IP()
        {
            string IPStr = System.Web.HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            return IPStr;
        }
    }
}