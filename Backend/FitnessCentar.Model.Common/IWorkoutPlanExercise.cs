using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Model.Common
{
    public interface IWorkoutPlanExercise
    {
        Guid Id { get; set; }
        int Exercisenumber { get; set; }
        Guid WorkoutPlanId { get; set; }
        Guid ExerciseId { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DatedUpdated { get; set; }
        bool? IsActive { get; set; }
    }
}
