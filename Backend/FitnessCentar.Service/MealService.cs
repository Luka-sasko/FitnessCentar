using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using FitnessCentar.Repository.Common;
using FitnessCentar.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Service
{
    public class MealService : IMealService
    {
        private readonly Guid _userId = Guid.NewGuid();
        private readonly IMealRepository _mealRepository;
        public MealService(IMealRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<string> CreateAsync(IMeal newMeal)
        {
            var userId = _userId;

            newMeal = FillUserAndDateInfoOnCreate(newMeal, userId);
            return await _mealRepository.CreateAsync(newMeal);
        }

        private IMeal FillUserAndDateInfoOnCreate(IMeal newMeal, Guid userId)
        {
            newMeal.CreatedBy = userId;
            newMeal.UpdatedBy = userId;
            newMeal.DateUpdated = DateTime.UtcNow;
            newMeal.DateCreated = DateTime.UtcNow;
            newMeal.Id = Guid.NewGuid();
            newMeal.IsActive = true;
            return newMeal;
        }

        public async Task<string> DeleteAsync(Guid mealId)
        {
            var userId = _userId;
            return await _mealRepository.DeleteAsync(mealId,userId);
        }

        public async Task<PagedList<IMeal>> GetAllsync(MealFilter filter, Sorting sorting, Paging paging)
        {
            return await _mealRepository.GetAllAsync(filter, sorting, paging);  
        }

        public async Task<IMeal> GetByIdAsync(Guid id)
        {
            return await _mealRepository.GetByIdAsync(id);
        }

        public async Task<string> UpdateAsync(IMeal updatedMeal)
        {
            var userId = _userId;
            updatedMeal = FillUserAndDateInfoOnUpdate(userId, updatedMeal);
            return await _mealRepository.UpdateAsync(updatedMeal);
        }

        private IMeal FillUserAndDateInfoOnUpdate(Guid userId, IMeal updatedMeal)
        {
            updatedMeal.UpdatedBy = userId;
            updatedMeal.DateUpdated= DateTime.UtcNow;
            return updatedMeal;
        }
    }
}
