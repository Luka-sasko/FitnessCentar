using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using FitnessCentar.Repository.Common;
using FitnessCentar.Service.Common;
using Microsoft.AspNet.Identity;
using System;
using System.Threading.Tasks;
using System.Web;

namespace FitnessCentar.Service
{
    public class WorkoutPlanService : IWorkoutPlanService
    {
        private readonly IWorkoutPlanRepository _workoutPlanRepository;

        public WorkoutPlanService(IWorkoutPlanRepository workoutPlanRepository)
        {
            _workoutPlanRepository = workoutPlanRepository;
            
        }

        public async Task<string> DeleteAsync(Guid workoutPlanId)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            return await _workoutPlanRepository.DeleteAsync(workoutPlanId,userId,DateTime.UtcNow);
        }

        public async Task<PagedList<IWorkoutPlan>> GetAllAsync(WorkoutPlanFilter filter, Sorting sorting, Paging paging)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            filter.UserId = userId;
            return await _workoutPlanRepository.GetAllAsync(filter, sorting, paging);
        }

        public async Task<IWorkoutPlan> GetByIdAsync(Guid id)
        {
            return await _workoutPlanRepository.GetByIdAsync(id);
        }

        public async Task<string> CreateAsync(IWorkoutPlan workoutPlan)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            workoutPlan=FillUserAndDateInfoOnCreate(workoutPlan, userId);
            return await _workoutPlanRepository.CreateAsync(workoutPlan);
        }

        public async Task<string> UpdateAsync(IWorkoutPlan updatedWorkoutPlan)
        {
            var userId=Guid.Parse(HttpContext.Current.User.Identity.GetUserId()) ;
            updatedWorkoutPlan=FillUserAndDateInfoOnUpdate(updatedWorkoutPlan, userId);
            return await _workoutPlanRepository.UpdateAsync(updatedWorkoutPlan);
            
        }
        private IWorkoutPlan FillUserAndDateInfoOnCreate(IWorkoutPlan newWorkoutPlan,Guid userId)
        {
            newWorkoutPlan.CreatedBy = userId;
            newWorkoutPlan.UpdatedBy = userId;
            newWorkoutPlan.DatedUpdated = DateTime.UtcNow;
            newWorkoutPlan.DateCreated= DateTime.UtcNow;
            newWorkoutPlan.IsActive = true;
            newWorkoutPlan.Id=Guid.NewGuid();
            newWorkoutPlan.UserId = userId;
            return newWorkoutPlan;
        }

        private IWorkoutPlan FillUserAndDateInfoOnUpdate(IWorkoutPlan updatedWorkoutPlan, Guid userId)
        {
            updatedWorkoutPlan.UpdatedBy = userId;
            updatedWorkoutPlan.DatedUpdated = DateTime.UtcNow;
            return updatedWorkoutPlan;
        }


    }
}
