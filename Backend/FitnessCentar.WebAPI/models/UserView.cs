using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.Models
{
    public class UserView
    {
        public Guid Id { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Role { get; set; }
        public DateTime Birthdate { get; set; }
        public decimal Weight { get; set; }
        public decimal Height { get; set; }
    }
}