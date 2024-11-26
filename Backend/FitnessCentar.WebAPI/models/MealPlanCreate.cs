using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.Models
{
    public class MealPlanCreate
    {
        public Guid UserId { get; set; }
        public string Name { get; set; }
    }
}