using apiHypster.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Web;
using System.Web.Http;

namespace apiHypster.Controllers
{
    public class UserController : ApiController
    {
        public UserController() { }
        hypster_tv_DAL.memberManagement memberManager = new hypster_tv_DAL.memberManagement();
        hypster_tv_DAL.Member member = new hypster_tv_DAL.Member();
        Resources res = new Resources();
        [HttpGet]
        public responseData validate()
        {
            responseData obj = new responseData { status = (int)Resources.xhrCode.ERROR, message = "API error: The requested resource does not support http method 'GET'." }; ;
            return obj;
        }
        [HttpPost]
        public responseData validate([FromBody] User user)
        {
            HttpRequest httpReq = HttpContext.Current.Request;
            string clientIp = GetUserIP();
            responseData obj = new responseData();
            string foundIp = "";
            foreach (string allow in res.allowIPAddress)
            {
                if (clientIp == allow)
                {
                    foundIp = allow;
                    break;
                }                    
                else
                    continue;

            }
            if (foundIp != "")
            {
                HttpContext.Current.Response.AddHeader("ClientIPAddr", clientIp);
                try
                {
                    member = memberManager.getMemberByUserName(user.Username);
                    if (user.Username != null && user.Password != null)
                    {
                        if (member.username == "")
                        {
                            obj = new responseData { status = (int)Resources.xhrCode.NOT_FOUND, message = "User Not Found" };
                        }
                        else if (member.password != user.Password)
                        {
                            obj = new responseData { status = (int)Resources.xhrCode.INVALID, message = "Invalid Password" };
                        }
                        else
                        {
                            memberUser mem = new memberUser { id = member.id, username = member.username, email = member.email, name = member.name, adminlevel = member.adminLevel };
                            obj = new responseData { status = (int)Resources.xhrCode.OK, message = "Validation Successful", user = mem };
                        }
                    }
                    else
                    {
                        obj = new responseData { status = (int)Resources.xhrCode.ERROR, message = "API error: One (or both) of the field(s) missing: username & password" };
                    }
                }
                catch (Exception e)
                {
                    HttpResponse httpRes = HttpContext.Current.Response;
                    httpRes.Status = "500 Internal Server Error";
                    httpRes.StatusCode = 500;
                    httpRes.StatusDescription = "Internal Server Error during the process";
                    httpRes.AddHeader("ClientIPAddr", clientIp);
                    httpRes.AddHeader("Status", "500 Internal Server Error");
                    httpRes.AddHeader("StatusCode", "500");
                    httpRes.AddHeader("InnerException", e.InnerException.ToString());
                    httpRes.AddHeader("Message", e.Message);
                    httpRes.AddHeader("StackTrace", e.StackTrace);
                    httpRes.Flush();
                    obj = new responseData { status = (int)Resources.xhrCode.ERROR, message = "Internal Server Error during the process" };
                }
            }
            else
            {
                HttpResponse httpRes = HttpContext.Current.Response;
                httpRes.ClearHeaders();
                httpRes.Status = "403 Forbidden";
                httpRes.StatusCode = 403;
                httpRes.StatusDescription = "Unauthorized Location";
                httpRes.AddHeader("ClientIPAddr", clientIp);
                httpRes.AddHeader("Status", "403 Forbidden");
                httpRes.AddHeader("StatusCode", "403");
                httpRes.AddHeader("StatusDescription", "Unauthorized Location from the IP Address: " + clientIp);
                httpRes.Flush();
            }
            return obj;        
        }

        private string GetUserIP()
        {
            string strIP = String.Empty;
            HttpRequest httpReq = HttpContext.Current.Request;

            //test for non-standard proxy server designations of client's IP
            if (httpReq.ServerVariables["HTTP_CLIENT_IP"] != null)
            {
                strIP = httpReq.ServerVariables["HTTP_CLIENT_IP"].ToString();
            }
            else if (httpReq.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
            {
                strIP = httpReq.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            //test for host address reported by the server
            else if
            (
                //if exists
                (httpReq.UserHostAddress.Length != 0)
                &&
                //and if not localhost IPV6 or localhost name
                ((httpReq.UserHostAddress != "::1") || (httpReq.UserHostAddress != "localhost"))
            )
            {
                strIP = httpReq.UserHostAddress;
            }
            //finally, if all else fails, get the IP from a web scrape of another server: Check IP: This means No IP Address was Found.
            else
            {
                WebRequest request = WebRequest.Create("http://checkip.thismeansnoipaddfound.com/");
                using (WebResponse response = request.GetResponse())
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    strIP = sr.ReadToEnd();
                }
                //scrape ip from the html
                int i1 = strIP.IndexOf("Address: ") + 9;
                int i2 = strIP.LastIndexOf("</body>");
                strIP = strIP.Substring(i1, i2 - i1);
            }
            return strIP;
        }
    }
}
