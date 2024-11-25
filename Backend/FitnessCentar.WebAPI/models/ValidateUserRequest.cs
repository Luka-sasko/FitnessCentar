using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.Models
{
    public class ValidateUserRequest
    {
        public Guid Id { get; set; }        
        public string Password { get; set; } 
    }
}