using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Service.Common
{
    public interface IWorkoutPlanService
    {
        Task<PagedList<IWorkoutPlan>> GetAllAsync(WorkoutPlanFilter filter,Sorting sorting,Paging paging);
        Task<IWorkoutPlan> GetByIdAsync(Guid id);
        Task<string> DeleteAsync(Guid workoutPlanId);
        Task<string> CreateAsync(IWorkoutPlan workoutPlan);
        Task<string> UpdateAsync(IWorkoutPlan updatedWorkoutPlan);

        
    }
}
