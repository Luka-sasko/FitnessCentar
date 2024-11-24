using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.Models
{
    public class FoodCreate
    {
        public Guid MealId { get; set; }
        public string Name { get; set; }
        public decimal Weight { get; set; }

    }
}