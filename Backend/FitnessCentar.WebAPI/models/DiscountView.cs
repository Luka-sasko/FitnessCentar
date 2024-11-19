using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.Models
{
    public class DiscountView
    {
        public string Name { get; set; }
        public int Amount { get; set; }
        public DateTime EndDate { get; set; }
    }
}