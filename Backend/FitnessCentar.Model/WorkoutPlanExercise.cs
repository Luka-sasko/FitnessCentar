using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Model
{
    public class WorkoutPlanExercise : IWorkoutPlanExercise
    {
        public Guid Id { get ; set; }
        public int Exercisenumber { get; set; }
        public Guid WorkoutPlanId { get; set; }
        public Guid ExerciseId { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DatedUpdated { get; set; }
        public bool? IsActive { get; set; }
    }
}
