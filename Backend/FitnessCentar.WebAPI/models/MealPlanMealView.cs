using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.Models
{
    public class MealPlanMealView
    {
        public Guid Id { get; set; }
        public Guid MealId { get; set; }
        public Guid MealPlanId { get; set; }
    }
}