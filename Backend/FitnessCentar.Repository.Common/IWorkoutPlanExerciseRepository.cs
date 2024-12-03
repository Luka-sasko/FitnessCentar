using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Repository.Common
{
    public interface IWorkoutPlanExerciseRepository
    {
        Task<PagedList<IWorkoutPlanExercise>> GetAllAsync(WorkoutPlanExerciseFilter filter, Sorting sorting,Paging paging);
        Task<IWorkoutPlanExercise> GetByIdAsync(Guid id);
        Task<string> DeleteAsync(Guid workoutPlanExerciseId,Guid userId,DateTime time);
        Task<string> CreateAsync(IWorkoutPlanExercise newworkoutPlanExercise);
        Task<string> UpdateAsync(IWorkoutPlanExercise updatedWorkoutPlanExercise);
    }
}
