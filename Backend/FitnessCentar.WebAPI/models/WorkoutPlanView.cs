using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.Models
{
    public class WorkoutPlanView
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; }
        public string Desc {  get; set; }

    }
}