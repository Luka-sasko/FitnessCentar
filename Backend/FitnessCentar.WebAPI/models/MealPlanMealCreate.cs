using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.Models
{
    public class MealPlanMealCreate
    {
        public Guid MealId { get; set; }
        public Guid MealPlanId { get; set; }
    }
}