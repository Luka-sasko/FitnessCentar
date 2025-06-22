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
    public class MealService : IMealService
    {
        private readonly IMealRepository _mealRepository;
        public MealService(IMealRepository mealRepository)
        {
            _mealRepository = mealRepository;
        }

        public async Task<string> CreateAsync(IMeal newMeal)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
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
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            return await _mealRepository.DeleteAsync(mealId,userId);
        }

        public async Task<PagedList<IMeal>> GetAllsync(MealFilter filter, Sorting sorting, Paging paging)
        {
            return await _mealRepository.GetAllAsync(filter, sorting, paging);  
        }
        public async Task<PagedList<IMeal>> GetMealsForUserAsync(MealFilter filter, Sorting sorting, Paging paging)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            return await _mealRepository.GetMealsForUserAsync(userId, filter, sorting, paging);
        }


        public async Task<IMeal> GetByIdAsync(Guid id)
        {
            return await _mealRepository.GetByIdAsync(id);
        }

        public async Task<string> UpdateAsync(IMeal updatedMeal)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
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
