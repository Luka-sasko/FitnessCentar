using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using FitnessCentar.Repository.Common;
using FitnessCentar.Service.Common;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace FitnessCentar.Service
{
    public class WorkoutPlanExerciseService : IWorkoutPlanExerciseService
    {
        private readonly IWorkoutPlanExerciseRepository _workoutPlanExerciseRepository;
        public WorkoutPlanExerciseService(IWorkoutPlanExerciseRepository workoutPlanExerciseRepository)
        {
            _workoutPlanExerciseRepository = workoutPlanExerciseRepository;
            
        }

        public async Task<string> CreateAsync(IWorkoutPlanExercise newworkoutPlanExercise)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            newworkoutPlanExercise = FillDateAndUserInfoOnCreate(newworkoutPlanExercise, userId);
            return await _workoutPlanExerciseRepository.CreateAsync(newworkoutPlanExercise);
            
        }

        public async Task<string> DeleteAsync(Guid workoutPlanExerciseId)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            var time = DateTime.UtcNow;
            return await _workoutPlanExerciseRepository.DeleteAsync(workoutPlanExerciseId, userId, time);
        }

        public async Task<PagedList<IWorkoutPlanExercise>> GetAllAsync(WorkoutPlanExerciseFilter filter, Sorting sorting, Paging paging)
        {
            return await _workoutPlanExerciseRepository.GetAllAsync(filter, sorting, paging);
            
        }

        public async Task<IWorkoutPlanExercise> GetByIdAsync(Guid id)
        {
            return await _workoutPlanExerciseRepository.GetByIdAsync(id);
        }

        public async Task<string> UpdateAsync(IWorkoutPlanExercise updatedWorkoutPlanExercise)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            updatedWorkoutPlanExercise=FillDateAndUserInfoOnUpdate(updatedWorkoutPlanExercise, userId);
            return await _workoutPlanExerciseRepository.UpdateAsync(updatedWorkoutPlanExercise);
            
        }

        private IWorkoutPlanExercise FillDateAndUserInfoOnCreate(IWorkoutPlanExercise newWorkoutPlanExercise, Guid userId)
        {

            newWorkoutPlanExercise.DateCreated = DateTime.UtcNow;
            newWorkoutPlanExercise.DatedUpdated = DateTime.UtcNow;
            newWorkoutPlanExercise.CreatedBy = userId;
            newWorkoutPlanExercise.UpdatedBy = userId;
            newWorkoutPlanExercise.Id = Guid.NewGuid();
            newWorkoutPlanExercise.IsActive = true;
            return newWorkoutPlanExercise;
        }

        private IWorkoutPlanExercise FillDateAndUserInfoOnUpdate(IWorkoutPlanExercise updatedWorkoutPlanExercise, Guid userId)
        {
            updatedWorkoutPlanExercise.UpdatedBy = userId;
            updatedWorkoutPlanExercise.DatedUpdated = DateTime.UtcNow;
            return updatedWorkoutPlanExercise;
        }
    }
}
