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
        newsManagement_Admin newsManager_admin = new newsManagement_Admin();
        List<newsPost> nPost = new List<newsPost>();
        int maxLength = 20;
        [Route("news/feed")]
        [HttpGet]
        public newsResponseData feed()
        {
            HttpResponse httpRes = HttpContext.Current.Response;
            newsResponseData obj = new newsResponseData();
            item[] newsPo = new item[maxLength];
            string clientIp = res.GetUserIP();
            HttpContext.Current.Response.AddHeader("ClientIPAddr", clientIp);
            try
            {
                for (int i = 0; i < maxLength; i++)
                {
                    item news = new item();
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
            return obj;
        }

        [Route("news/weekly")]
        [HttpGet]
        public newsFeaturedData weekly()
        {
            HttpResponse httpRes = HttpContext.Current.Response;
            newsFeaturedData obj = new newsFeaturedData();
            content cont = new content();
            item[] newsPo = new item[maxLength];
            string clientIp = res.GetUserIP();
            HttpContext.Current.Response.AddHeader("ClientIPAddr", clientIp);
            try
            {
                List<newsPost> posts = newsManager.GetAllNews();
                List<sp_postNewsletter_GetPostsByAttribute_Result> Featured = newsManager_admin.GetPostsByAttribute(0, "Featured");
                featuredObj newest = new featuredObj();
                item newest_feature = new item();
                newest_feature.href = (Featured[0].post_guid != null && Featured[0].post_guid != "") ? "http://hypster.com/breaking/details/" + Featured[0].post_guid : null;
                newest_feature.imgsrc = (Featured[0].post_image != null && Featured[0].post_image != "") ? "http://hypster.com/imgs/i_posts/" + Featured[0].post_image : null;
                newest_feature.title = Featured[0].post_title;
                newest.featured = newest_feature;
                int exclude_id = Featured[0].post_id;
                List<sp_postNewsletter_GetPostsByAttribute_Result> Newsletter = newsManager_admin.GetPostsByAttribute(exclude_id, "Newsletter");
                newest.items = new item[Newsletter.Count];
                for (int i = 0; i < Newsletter.Count; i++)
                {
                    item newest_newsletter = new item();
                    newest_newsletter.href = (Newsletter[i].post_guid != null && Newsletter[i].post_guid != "") ? "http://hypster.com/breaking/details/" + Newsletter[i].post_guid : null;
                    newest_newsletter.imgsrc = (Newsletter[i].post_image != null && Newsletter[i].post_image != "") ? "http://hypster.com/imgs/i_posts/" + Newsletter[i].post_image : null;
                    newest_newsletter.title = Newsletter[i].post_title;
                    newest.items[i] = newest_newsletter;
                }
                cont.newest = newest;
                featuredObj playlists = new featuredObj();
                item playlists_feature = new item();
                List<sp_postNewsletter_GetPostsByAttribute_Result> Playlist = newsManager_admin.GetPostsByAttribute(0, "Playlist");
                playlists.items = new item[Playlist.Count-1];
                playlists_feature.href = (Playlist[0].post_guid != null && Playlist[0].post_guid != "") ? "http://hypster.com/breaking/details/" + Playlist[0].post_guid : null;
                playlists_feature.imgsrc = (Playlist[0].post_image != null && Playlist[0].post_image != "") ? "http://hypster.com/imgs/i_posts/" + Playlist[0].post_image : null;
                playlists_feature.title = Playlist[0].post_title;
                playlists.featured = playlists_feature;
                for (int j = 1; j <= Playlist.Count-1; j++)
                {
                    item playlist_item = new item();
                    playlist_item.href = (Playlist[j].post_guid != null && Playlist[j].post_guid != "") ? "http://hypster.com/breaking/details/" + Playlist[j].post_guid : null;
                    playlist_item.imgsrc = (Playlist[j].post_image != null && Playlist[j].post_image != "") ? "http://hypster.com/imgs/i_posts/" + Playlist[j].post_image : null;
                    playlist_item.title = Playlist[j].post_title;
                    playlists.items[j-1] = playlist_item;
                }
                cont.playlists = playlists;
                List<sp_postNewsletter_GetPostsByAttribute_Result> More = newsManager_admin.GetPostsByAttribute(0, "More");
                cont.more = new item[More.Count];
                for (int k = 0; k < More.Count; k++)
                {
                    item more_item = new item();
                    more_item.href = (More[k].post_guid != null && More[k].post_guid != "") ? "http://hypster.com/breaking/details/" + More[k].post_guid : null;
                    more_item.imgsrc = (More[k].post_image != null && More[k].post_image != "") ? "http://hypster.com/imgs/i_posts/" + More[k].post_image : null;
                    more_item.title = More[k].post_title;
                    cont.more[k] = more_item;
                }
                obj = new newsFeaturedData { status = (int)Resources.xhrCode.OK, message = "Success", content = cont };
            }
            catch (Exception e)
            {                
                obj = new newsFeaturedData { status = (int)Resources.xhrCode.ERROR, message = e.ToString() };
                httpRes.Status = "500 Internal Server Error";
                httpRes.StatusCode = 500;
                httpRes.StatusDescription = "Internal Server Error during the process";
                httpRes.Flush();
            }
            return obj;
        }
    }
}
