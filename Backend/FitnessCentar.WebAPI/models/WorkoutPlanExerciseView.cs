using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.Models
{
    public class WorkoutPlanExerciseView
    {
        public Guid Id { get; set; }
        public Guid WorkoutPlanId { get; set; }
        public Guid ExerciseId { get; set; }
        public int ExerciseNumber { get; set; }
    }
}