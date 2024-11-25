using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.Models
{
    public class PasswordUpdateModel
    {
        public string PasswordOld { get; set; }
        public string PasswordNew { get; set; }
    }
}