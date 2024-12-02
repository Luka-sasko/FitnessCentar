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
    public class ExerciseService:IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;

        public ExerciseService(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task<string> CreateExerciseAsync(IExercise newExercise)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            newExercise = FillDateAndUserInfoOnCreate(newExercise, userId);
            return await _exerciseRepository.CreateExerciseAsync(newExercise);
        }

        private IExercise FillDateAndUserInfoOnCreate(IExercise newExercise, Guid userId)
        {
            newExercise.Id = Guid.NewGuid();
            newExercise.DateCreated = DateTime.UtcNow;
            newExercise.DatedUpdated = DateTime.UtcNow;
            newExercise.CreatedBy = userId;
            newExercise.UpdatedBy = userId;
            newExercise.IsActive = true;
            return newExercise;
        }

        public async Task<string> DeleteExerciseAsync(Guid exerciseId)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
           return await _exerciseRepository.DeleteExerciseAsync(exerciseId,userId);
        }

        public async Task<string> UpdateExerciseAsync(IExercise updatedExercise)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            updatedExercise= FillDateAndUserInfoOnUpdate(updatedExercise, userId);
            return await _exerciseRepository.UpdateExerciseAsync(updatedExercise);

        }

        public async Task<PagedList<IExercise>> GetAllExercisesAsync(ExerciseFilter filter, Sorting sorting, Paging paging)
        {
            return await _exerciseRepository.GetAllExercisesAsync(filter, sorting, paging); 
        }

        public async Task<IExercise> GetExerciseById(Guid id)
        {
            return await _exerciseRepository.GetExerciseById(id);
        }

        private IExercise FillDateAndUserInfoOnUpdate(IExercise updatedExercise, Guid userId)
        {
            updatedExercise.UpdatedBy = userId;
            updatedExercise.DatedUpdated = DateTime.UtcNow;
            return updatedExercise;
        }


    }
}
