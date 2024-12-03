using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Common
{
    public class WorkoutPlanExerciseFilter
    {
        public Guid? WorkoutPlanId { get; set; }
        public Guid? ExerciseId { get; set; }
        public int ExerciseNumber { get; set; }

    }
}
