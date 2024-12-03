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
    public class MealPlanMealService : IMealPlanMealService
    {
        private readonly IMealPlanMealRepository _mealPlanMealRepository;
        public MealPlanMealService(IMealPlanMealRepository mealPlanMealRepository)
        {
            _mealPlanMealRepository = mealPlanMealRepository;
        }

        public async  Task<string> CreateAsync(IMealPlanMeal newMealPlanMeal)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            newMealPlanMeal = FillDateAndUserInfoOnCreate(newMealPlanMeal, userId);
            return await _mealPlanMealRepository.CreateAsync(newMealPlanMeal);
        }

        private IMealPlanMeal FillDateAndUserInfoOnCreate(IMealPlanMeal newMealPlanMeal, Guid userId)
        {
            
            newMealPlanMeal.DateCreated = DateTime.UtcNow;
            newMealPlanMeal.DateUpdated = DateTime.UtcNow;
            newMealPlanMeal.CreatedBy=userId;
            newMealPlanMeal.UpdatedBy=userId;
            newMealPlanMeal.Id=Guid.NewGuid();
            newMealPlanMeal.IsActive = true;
            return newMealPlanMeal;
        }

        public async Task<string> DeleteAsync(Guid mealPlanMealId)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            var time = DateTime.UtcNow;
            return await _mealPlanMealRepository.DeleteAsync(mealPlanMealId, userId, time);
        }

        public async Task<PagedList<IMealPlanMeal>> GetAllsync(MealPlanMealFilter filter, Sorting sorting, Paging paging)
        {
            return await _mealPlanMealRepository.GetAllAsync(filter, sorting, paging);
        }

        public async Task<IMealPlanMeal> GetByIdAsync(Guid id)
        {
            return await _mealPlanMealRepository.GetByIdAsync(id); 
        }

        public async Task<string> UpdateAsync(IMealPlanMeal updatedMealPlanMeal)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            updatedMealPlanMeal = FillDateAndUserInfoOnUpdate(updatedMealPlanMeal, userId);
            return await _mealPlanMealRepository.UpdateAsync(updatedMealPlanMeal);
        }

        private IMealPlanMeal FillDateAndUserInfoOnUpdate(IMealPlanMeal updatedMealPlanMeal, Guid userId)
        {
            updatedMealPlanMeal.UpdatedBy = userId;
            updatedMealPlanMeal.DateUpdated = DateTime.UtcNow;
            return updatedMealPlanMeal;
        }
    }
}
