using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace apiHypster.Models
{
    public class Resources
    {
        public enum xhrCode { OK = 1, ERROR, AUTH, INVALID, DUPLICATE, EXPIRED, NOT_FOUND, INCOMPLETE, HUMAN };
        public string[] allowIPAddress = { "66.117.119.138", "23.253.156.54", "127.0.0.1","10.0.0.114" };
        public List<string> properties = (from t in typeof(hypster_tv_DAL.Member).GetProperties() select t.Name).ToList();
        public string GetUserIP()
        {
            string strIP = String.Empty;
            HttpRequest httpReq = HttpContext.Current.Request;

            //test for non-standard proxy server designations of client's IP
            if (httpReq.ServerVariables["HTTP_CLIENT_IP"] != null)
                strIP = httpReq.ServerVariables["HTTP_CLIENT_IP"].ToString();
            else if (httpReq.ServerVariables["HTTP_X_FORWARDED_FOR"] != null)
                strIP = httpReq.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            //test for host address reported by the server: if exists and if not localhost IPV6 or localhost name
            else if ((httpReq.UserHostAddress.Length != 0) && ((httpReq.UserHostAddress != "::1") || (httpReq.UserHostAddress != "localhost")))
                strIP = httpReq.UserHostAddress;
            //finally, if all else fails, get the IP from a web scrape of another server: Check IP: This means No IP Address was Found.
            else
            {
                WebRequest request = WebRequest.Create("http://checkip.thismeansnoipaddfound.com/");
                using (WebResponse response = request.GetResponse())
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                    strIP = sr.ReadToEnd();
                //scrape ip from the html
                int i1 = strIP.IndexOf("Address: ") + 9;
                int i2 = strIP.LastIndexOf("</body>");
                strIP = strIP.Substring(i1, i2 - i1);
            }
            return strIP;
        }

        public bool checkIpAddr(string ip)
        {
            bool result = false;
            foreach (string allow in allowIPAddress)
            {
                if (ip == allow)
                {
                    result = true;
                    break;
                }
                else
                    continue;
            }
            return result;
        }

        public string getQueryStream(hypster_tv_DAL.Member member, memberUser user)
        {
            string querystream = "";
            if (member.username != user.username) querystream += "username='" + user.username + "'";
            if (member.password != user.password) querystream += ((querystream!="")?",":"") + "password='" + user.password + "'";
            if (member.email != user.email) querystream += ((querystream != "") ? "," : "") + "email='" + user.email + "'";
            if (member.name != user.name) querystream += ((querystream != "") ? "," : "") + "name='" + user.name + "'";
            if (member.country != user.country) querystream += ((querystream != "") ? "," : "") + "country='" + user.country + "'";
            if (member.city != user.city) querystream += ((querystream != "") ? "," : "") + "city='" + user.city + "'";
            if (member.zipcode != user.zipcode) querystream += ((querystream != "") ? "," : "") + "zipcode='" + user.zipcode + "'";
            if (Convert.ToDateTime(member.birth).ToString("MM/dd/yyyy") != Convert.ToDateTime(user.birth).ToString("MM/dd/yyyy")) querystream += ((querystream != "") ? "," : "") + "birth='" + user.birth + "'";
            if (member.introduce != user.introduce) querystream += ((querystream != "") ? "," : "") + "introduce='" + user.introduce + "'";
            if (member.sex != user.sex) querystream += ((querystream != "") ? "," : "") + "sex=" + user.sex;
            if (member.trackcount != user.trackcount) querystream += ((querystream != "") ? "," : "") + "trackcount=" + user.trackcount;
            if (member.israndomplay != user.israndomplay) querystream += ((querystream != "") ? "," : "") + "israndomplay=" + user.israndomplay;
            if (member.active_playlist != user.active_playlist) querystream += ((querystream != "") ? "," : "") + "active_playlist=" + user.active_playlist;
            if (member.profile_pic_id != user.profile_pic_id) querystream += ((querystream != "") ? "," : "") + "profile_pic_id=" + user.profile_pic_id;
            if (member.is_new_artist != user.is_new_artist) querystream += ((querystream != "") ? "," : "") + "is_new_artist=" + user.is_new_artist;
            if (member.autoplay != user.autoplay) querystream += ((querystream != "") ? "," : "") + "autoplay=" + user.autoplay;
            if (member.email_confirmed != user.email_confirmed) querystream += ((querystream != "") ? "," : "") + "email_confirmed=" + user.email_confirmed;
            if (member.email_optout != user.email_optout) querystream += ((querystream != "") ? "," : "") + "email_optout=" + user.email_optout;
            if (member.widget_views != user.widget_views) querystream += ((querystream != "") ? "," : "") + "widget_views=" + user.widget_views;
            if (Convert.ToInt16(member.CoregProcessed) != user.coreg_processed) querystream += ((querystream != "") ? "," : "") + "CoregProcessed=" + user.coreg_processed;
            if (member.StatusId != user.status_id) querystream += ((querystream != "") ? "," : "") + "StatusId=" + user.status_id;
            if (Convert.ToInt16(member.AutoshareEnabled) != user.autoshare_enabled) querystream += ((querystream != "") ? "," : "") + "AutoshareEnabled=" + user.autoshare_enabled;
            if (member.ArtistLevel != user.artist_level) querystream += ((querystream != "") ? "," : "") + "ArtistLevel=" + user.artist_level;
            if (Convert.ToInt16(member.IsOverEighteen) != user.is_over_eighteen) querystream += ((querystream != "") ? "," : "") + "IsOverEighteen=" + user.is_over_eighteen;
            if (member.user_interest != user.user_interest) querystream += ((querystream != "") ? "," : "") + "user_interest=" + user.user_interest;
            if (member.eCampD != user.e_camp_d) querystream += ((querystream != "") ? "," : "") + "eCampD=" + user.e_camp_d;
            if (member.adminLevel != user.adminlevel) querystream += ((querystream != "") ? "," : "") + "adminLevel=" + user.adminlevel;
            if (Convert.ToInt16(member.IsSuspended) != user.is_suspended) querystream += ((querystream != "") ? "," : "") + "IsSuspended=" + user.is_suspended;
            return querystream;
        }

        public memberUser padData(hypster_tv_DAL.Member member, memberUser user)
        {
            memberUser mem = new memberUser();
            mem.id = ((user.id == 0) ? member.id : user.id);
            mem.username = (user.username != null ? user.username : member.username);
            mem.password = (user.password != null ? user.password : member.password);
            mem.email = (user.email != null ? user.email : member.email);
            mem.name = (user.name != null ? user.name : member.name);
            mem.country = (user.country != null ? user.country : member.country);
            mem.city = (user.city != null ? user.city : member.city);
            mem.zipcode = (user.zipcode != null ? user.zipcode : member.zipcode);
            mem.birth = (user.birth != null ? Convert.ToDateTime(user.birth).ToString("MM/dd/yyyy") : Convert.ToDateTime(member.birth).ToString("MM/dd/yyyy"));
            mem.introduce = (user.introduce != null ? user.introduce : member.introduce);
            mem.sex = (user.sex != null ? Convert.ToSByte(user.sex) : Convert.ToSByte(member.sex));
            mem.trackcount = ((user.trackcount != null) ? user.trackcount : member.trackcount);
            mem.israndomplay = ((user.israndomplay != null) ? user.israndomplay : member.israndomplay);
            mem.active_playlist = ((user.active_playlist != null) ? user.active_playlist : member.active_playlist);
            mem.profile_pic_id = ((user.profile_pic_id != null) ? user.profile_pic_id : member.profile_pic_id);
            mem.is_new_artist = ((user.is_new_artist != null) ? user.is_new_artist : member.is_new_artist);
            mem.autoplay = ((user.autoplay != null) ? user.autoplay : member.autoplay);
            mem.email_confirmed = ((user.email_confirmed != null) ? user.email_confirmed : member.email_confirmed);
            mem.email_optout = ((user.email_optout != null) ? user.email_optout : member.email_optout);
            mem.widget_views = ((user.widget_views != null) ? user.widget_views : member.widget_views);
            mem.coreg_processed = ((user.coreg_processed != null) ? user.coreg_processed : Convert.ToInt16(member.CoregProcessed));
            mem.status_id = ((user.status_id != null) ? user.status_id : member.StatusId);
            mem.autoshare_enabled = ((user.autoshare_enabled != null) ? user.autoshare_enabled : Convert.ToInt16(member.AutoshareEnabled));
            mem.artist_level = ((user.artist_level != null) ? user.artist_level : member.ArtistLevel); 
            mem.is_over_eighteen = ((user.is_over_eighteen != null) ? user.is_over_eighteen : Convert.ToInt16(member.IsOverEighteen)); 
            mem.user_interest = ((user.user_interest != null) ? user.user_interest : member.user_interest);
            mem.e_camp_d = ((user.e_camp_d != null) ? user.e_camp_d : member.eCampD);
            mem.adminlevel = ((user.adminlevel != null) ? user.adminlevel : member.adminLevel);
            mem.is_suspended = (user.is_suspended != null ? user.is_suspended : Convert.ToInt16(member.IsSuspended));
            return mem;
        }
    }
}