using apiHypster.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
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
        [HttpGet]
        public responseData validate()
        {
            responseData obj = new responseData { status = "2", message = "API error: The requested resource does not support http method 'GET'." }; ;
            return obj;
        }
        [HttpPost]
        public responseData validate([FromBody] User user)
        {
            responseData obj = new responseData();
            try
            {
                member = memberManager.getMemberByUserName(user.Username);
                if (user.Username != null && user.Password != null)
                {
                    if (member.username == "")
                    {
                        obj = new responseData { status = "7", message = "User Not Found" };
                    }
                    else if (member.password != user.Password)
                    {
                        obj = new responseData { status = "4", message = "Invalid Password" };
                    }
                    else                    
                    {
                        memberUser mem = new memberUser { id = member.id, username = member.username, email = member.email, name = member.name, adminLevel = member.adminLevel };
                        obj = new responseData { status = "1", message = "Validation Successful", user = mem };
                    }                    
                }
                else
                {
                    obj = new responseData { status = "2", message = "API error" };
                }
            }
            catch (Exception e)
            {
                obj = new responseData { status = "0", message = "Exception " + e.Message };
            }
            return obj;
        }
    }
}
