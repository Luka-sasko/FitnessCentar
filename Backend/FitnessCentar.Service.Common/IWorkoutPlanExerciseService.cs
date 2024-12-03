using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Service.Common
{
    public interface IWorkoutPlanExerciseService
    {
        Task<PagedList<IWorkoutPlanExercise>> GetAllAsync(WorkoutPlanExerciseFilter filter, Sorting sorting, Paging paging);
        Task<IWorkoutPlanExercise> GetByIdAsync(Guid id);
        Task<string> DeleteAsync(Guid workoutPlanExerciseId);
        Task<string> CreateAsync(IWorkoutPlanExercise workoutPlanExercise);
        Task<string> UpdateAsync(IWorkoutPlanExercise udpatedWorkoutPlanExercise);
    }
}
