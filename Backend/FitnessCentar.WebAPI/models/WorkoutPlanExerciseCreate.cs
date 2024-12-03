using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.Models
{
    public class WorkoutPlanExerciseCreate
    {
        public Guid WorkoutPlanId { get; set; }
        public Guid ExerciseId { get; set; }
        public int ExerciseNumber { get; set; }
    }
}