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
        //bool test = false;
        [Route("user/validate")]
        [HttpGet]
        public responseData validate()
        {
            responseData obj = new responseData { status = (int)Resources.xhrCode.ERROR, message = "API error: The requested resource does not support http method 'GET'." }; ;
            return obj;
        }
        [Route("user/validate")]
        [HttpPost]
        public responseData validate([FromBody] User user)
        {
            HttpRequest httpReq = HttpContext.Current.Request;            
            responseData obj = new responseData();
            string clientIp = res.GetUserIP();
            //if (test?true:res.checkIpAddr(clientIp))
            if (res.checkIpAddr(clientIp))
            {
                HttpContext.Current.Response.AddHeader("ClientIPAddr", clientIp);
                try
                {
                    member = memberManager.getMemberByUserName(user.Username);
                    if (user.Username != null && user.Password != null)
                    {
                        if (member.username == "")
                            obj = new responseData { status = (int)Resources.xhrCode.NOT_FOUND, message = "User Not Found" };
                        else if (member.password != user.Password)
                            obj = new responseData { status = (int)Resources.xhrCode.INVALID, message = "Invalid Password" };
                        else
                        {
                            memberUser mem = new memberUser {
                                id = member.id,
                                username = member.username,
                                email = member.email,
                                name = member.name,
                                country = (member.country != "" ? member.country.ToLower() : null),
                                city = (member.city != "" ? member.city.ToLower() : null),
                                zipcode = (member.zipcode != "" ? member.zipcode.ToLower() : null),
                                birth = Convert.ToDateTime(member.birth).ToString("MM/dd/yyyy"),
                                adminlevel = member.adminLevel
                            };
                            obj = new responseData { status = (int)Resources.xhrCode.OK, message = "Validation Successful", user = mem };
                        }
                    }
                    else
                        obj = new responseData { status = (int)Resources.xhrCode.ERROR, message = "API error: One (or both) of the field(s) missing: username & password" };
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
                obj = new responseData { status = (int)Resources.xhrCode.ERROR, message = "Unauthorized Location from the IP Address: " + clientIp };
            }
            return obj;
        }

        [Route("user/{userId:int}")]
        public responseData getUser(int userId)
        {
            HttpResponse httpRes = HttpContext.Current.Response;
            responseData obj = new responseData();
            string clientIp = res.GetUserIP();
            //if (test?true:res.checkIpAddr(clientIp))
            if (res.checkIpAddr(clientIp))
            {
                HttpContext.Current.Response.AddHeader("ClientIPAddr", clientIp);
                try
                {
                    member = memberManager.getMemberByID(userId);
                    if (member.id != 0)
                    {
                        memberUser mem = new memberUser
                        {
                            id = member.id,
                            username = member.username,
                            email = member.email,
                            name = member.name,
                            country = (member.country!=""?member.country.ToLower() : null),
                            city = (member.city != "" ? member.city.ToLower() : null),
                            zipcode = (member.zipcode != "" ? member.zipcode.ToLower() : null),
                            birth = Convert.ToDateTime(member.birth).ToString("MM/dd/yyyy"),
                            adminlevel = member.adminLevel
                        };
                        obj = new responseData { status = (int)Resources.xhrCode.OK, message = "User Found", user = mem };
                    }
                    else
                        obj = new responseData { status = (int)Resources.xhrCode.NOT_FOUND, message = "User Not Found" };
                }
                catch (Exception e)
                {
                    obj = new responseData { status = (int)Resources.xhrCode.ERROR, message = "Internal Server Error during the process" };
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
                }
            }
            else
            {
                obj = new responseData { status = (int)Resources.xhrCode.ERROR, message = "Unauthorized Location from the IP Address: " + clientIp };
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
    }
}
