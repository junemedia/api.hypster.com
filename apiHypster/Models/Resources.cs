using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace apiHypster.Models
{
    public class Resources
    {
        public enum xhrCode { OK = 1, ERROR = 2, AUTH = 3, INVALID = 4, DUPLICATE = 5, EXPIRED = 6, NOT_FOUND = 7, INCOMPLETE = 8, HUMAN = 9 };

        public string[] allowIPAddress = { "66.117.119.138", "23.253.156.54" };
    }
}