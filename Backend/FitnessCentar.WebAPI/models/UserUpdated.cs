using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.Models
{
    public class UserUpdated
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Contact { get; set; }
        public DateTime? Birthdate { get; set; }
        public double Weight { get; set; }
        public double Height { get; set; }
    }

}