using apiHypster.Models;
using hypster_tv_DAL;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http;

namespace apiHypster.Controllers
{
    public class NewsController : ApiController
    {
        Resources res = new Resources();
        newsManagement newsManager = new newsManagement();
        List<newsPost> nPost = new List<newsPost>();
        int maxLength = 20;
        [Route("news/feed")]
        [HttpGet]
        public newsResponseData feed()
        {
            HttpRequest httpReq = HttpContext.Current.Request;
            HttpResponse httpRes = HttpContext.Current.Response;
            newsResponseData obj = new newsResponseData();
            newsPosts[] newsPo = new newsPosts[maxLength];
            string clientIp = res.GetUserIP();
            if (res.checkIpAddr(clientIp))
            {
                HttpContext.Current.Response.AddHeader("ClientIPAddr", clientIp);
                try
                {
                    for (int i = 0; i < maxLength; i++)
                    {
                        newsPosts news = new newsPosts();
                        news.href = (newsManager.GetAllNews()[i].post_guid != null && newsManager.GetAllNews()[i].post_guid != "") ? "http://hypster.com/breaking/details/" + newsManager.GetAllNews()[i].post_guid : null;
                        news.imgsrc = (newsManager.GetAllNews()[i].post_image != null && newsManager.GetAllNews()[i].post_image != "") ? "http://hypster.com/imgs/i_posts/" + newsManager.GetAllNews()[i].post_image : null;
                        news.title = newsManager.GetAllNews()[i].post_title;
                        newsPo[i] = news;
                    }
                    obj = new newsResponseData { status = (int)Resources.xhrCode.OK, message = "News Listed", content = newsPo };
                }
                catch (Exception e)
                {
                    obj = new newsResponseData { status = (int)Resources.xhrCode.ERROR, message = e.ToString() };
                    httpRes.Status = "500 Internal Server Error";
                    httpRes.StatusCode = 500;
                    httpRes.StatusDescription = "Internal Server Error during the process";
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
                obj = new newsResponseData { status = (int)Resources.xhrCode.ERROR, message = "Unauthorized Location from the IP Address: " + clientIp };
            }
            return obj;
        }
    }
}
