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
        public string[] allowIPAddress = { "66.117.119.138", "23.253.156.54", "127.0.0.1", "10.0.0.114", "162.242.221.47" };//, "::1"

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
    }
}