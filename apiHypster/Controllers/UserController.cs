using apiHypster.Models;
using hypster_tv_DAL;
using System;
using System.Web;
using System.Web.Http;

namespace apiHypster.Controllers
{
    public class UserController : ApiController
    {
        public UserController() { }
        memberManagement memberManager = new memberManagement();
        Member member = new Member();
        Resources res = new Resources();
        UserRsurcs userRs = new UserRsurcs();

        [Route("user/validate")]
        [HttpGet]
        public userResponseData validate()
        {
            userResponseData obj = new userResponseData { status = (int)Resources.xhrCode.ERROR, message = "API error: The requested resource does not support http method 'GET'." };
            return obj;
        }

        [Route("user/validate")]
        [HttpPost]
        public userResponseData validate([FromBody] User user)
        {
            HttpResponse httpRes = HttpContext.Current.Response;
            userResponseData obj = new userResponseData();
            memberUser mem = new memberUser();
            string clientIp = res.GetUserIP();
            if (res.checkIpAddr(clientIp))
            {
                HttpContext.Current.Response.AddHeader("ClientIPAddr", clientIp);
                try
                {
                    member = memberManager.getMemberByUserName(user.Username);
                    if (user.Username != null && user.Password != null)
                    {
                        if (member.username == "")
                            obj = new userResponseData { status = (int)Resources.xhrCode.NOT_FOUND, message = "User Not Found" };
                        else if (member.password != user.Password)
                            obj = new userResponseData { status = (int)Resources.xhrCode.INVALID, message = "Invalid Password" };
                        else
                        {
                            mem = userRs.padMemberData(member, new memberUser());
                            obj = new userResponseData { status = (int)Resources.xhrCode.OK, message = "Validation Successful", user = mem };
                        }
                    }
                    else
                        obj = new userResponseData { status = (int)Resources.xhrCode.ERROR, message = "API error: One (or both) of the field(s) missing: username & password" };
                }
                catch (Exception e)
                {
                    obj = new userResponseData { status = (int)Resources.xhrCode.ERROR, message = e.ToString() };
                    httpRes.Status = "500 Internal Server Error";
                    httpRes.StatusCode = 500;
                    httpRes.StatusDescription = "500 ISE: General Exception";
                    httpRes.Flush();
                }
            }
            else
            {
                httpRes.ClearHeaders();
                httpRes.Status = "403 Forbidden";
                httpRes.StatusCode = 403;
                httpRes.StatusDescription = "Unauthorized Location";
                httpRes.AddHeader("ClientIPAddr", clientIp);
                httpRes.AddHeader("Status", "403 Forbidden");
                httpRes.AddHeader("StatusCode", "403");
                httpRes.AddHeader("StatusDescription", "Unauthorized Location from the IP Address: " + clientIp);
                httpRes.Flush();
                obj = new userResponseData { status = (int)Resources.xhrCode.ERROR, message = "Unauthorized Location from the IP Address: " + clientIp };
            }
            return obj;
        }

        [Route("user/{userId:int}")]
        public userResponseData getUser(int userId)
        {
            HttpResponse httpRes = HttpContext.Current.Response;
            userResponseData obj = new userResponseData();
            memberUser mem = new memberUser();
            string clientIp = res.GetUserIP();
            if (res.checkIpAddr(clientIp))
            {
                HttpContext.Current.Response.AddHeader("ClientIPAddr", clientIp);
                try
                {
                    member = memberManager.getMemberByID(userId);
                    if (member.id != 0)
                    {
                        mem = userRs.padMemberData(member, new memberUser());
                        obj = new userResponseData { status = (int)Resources.xhrCode.OK, message = "User Found", user = mem };
                    }
                    else
                        obj = new userResponseData { status = (int)Resources.xhrCode.NOT_FOUND, message = "User Not Found" };
                }
                catch (Exception e)
                {
                    obj = new userResponseData { status = (int)Resources.xhrCode.ERROR, message = e.ToString() };
                    httpRes.Status = "500 Internal Server Error";
                    httpRes.StatusCode = 500;
                    httpRes.StatusDescription = "Internal Server Error during the process";
                    httpRes.Flush();
                }
            }
            else
            {
                obj = new userResponseData { status = (int)Resources.xhrCode.ERROR, message = "Unauthorized Location from the IP Address: " + clientIp };
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

        [Route("user/{userId:int}")]
        [HttpPut]
        public userResponseData putUser([FromUri] int userId, [FromBody] memberUser user)
        {
            HttpResponse httpRes = HttpContext.Current.Response;
            memberUser mem = new memberUser();
            userResponseData obj = new userResponseData();
            string clientIp = res.GetUserIP();
            string querystream = "";
            string message = "";

            if (res.checkIpAddr(clientIp))
            {
                try
                {
                    member = memberManager.getMemberByID(userId);
                    if (member.id != 0)
                    {                        
                        if (user != null)
                        {
                            HttpContext.Current.Response.AddHeader("ClientIPAddr", clientIp);
                            user.id = member.id;
                            mem = userRs.padMemberData(member, user);
                            querystream = userRs.getMemberQueryStream(member, mem);
                            if (querystream != "")
                            {
                                memberManager.UpdateMemberData(user.id, querystream);
                                member = memberManager.getMemberByID(userId);
                                mem = userRs.padMemberData(member, user);
                                message = "User Updated";
                            }
                            else
                                message = "User Found Without Update";
                        }
                        else
                        {
                            mem = userRs.padMemberData(member, new memberUser());
                            message = "User Found";
                        }
                        obj = new userResponseData { status = (int)Resources.xhrCode.OK, message = message, user = mem };
                    }
                    else
                        obj = new userResponseData { status = (int)Resources.xhrCode.NOT_FOUND, message = "User Not Found" };
                }
                catch (ArgumentNullException ane)
                {
                    obj = new userResponseData { status = (int)Resources.xhrCode.ERROR, message = ane.ToString() };
                    httpRes.Status = "500 Internal Server Error";
                    httpRes.StatusCode = 500;
                    httpRes.StatusDescription = "500 ISE: ArgumentNullException";
                    httpRes.Flush();
                }
                catch (FormatException fe)
                {
                    obj = new userResponseData { status = (int)Resources.xhrCode.ERROR, message = fe.ToString() };
                    httpRes.Status = "500 Internal Server Error";
                    httpRes.StatusCode = 500;
                    httpRes.StatusDescription = "500 ISE: FormatException";
                    httpRes.Flush();
                }
                catch (Exception e)
                {
                    obj = new userResponseData { status = (int)Resources.xhrCode.ERROR, message = e.ToString() };
                    httpRes.Status = "500 Internal Server Error";
                    httpRes.StatusCode = 500;
                    httpRes.StatusDescription = "Internal Server Error during the process";
                    httpRes.Flush();
                }
            }
            else
            {
                obj = new userResponseData { status = (int)Resources.xhrCode.ERROR, message = "Unauthorized Location from the IP Address: " + clientIp };
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
