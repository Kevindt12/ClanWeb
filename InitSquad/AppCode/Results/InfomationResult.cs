using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClanWeb.Web.AppCode.Results
{
    public class InfomationResult
    {
        public string Error { get; set; }

        public string[] Errors { get; set; }

        public bool Succeeded { get; set; }



        public InfomationResult()
        {


        }

    }
}