using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.Models
{
    public class SubscriptionCreate
    {
        public decimal Price { set; get; }
        public int Duration { set; get; }
        public string Description { set; get; }
        public string Name { set; get; }
        public DateTime StartDate { set; get; }
        public Guid? DiscountId { set; get; }
    }
}