using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using FitnessCentar.Repository.Common;
using FitnessCentar.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace FitnessCentar.Service
{
    public class MealPlanService : IMealPlanService
    {
        private readonly IMealPlanRepository _mealPlanRepository;
        public MealPlanService( IMealPlanRepository mealPlanRepository )
        {
            _mealPlanRepository = mealPlanRepository;
        }

        public async Task<string> CreateAsync(IMealPlan newMealPlan)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            newMealPlan = FillUserAndDateInfoOnCreate(newMealPlan, userId);
            return await _mealPlanRepository.CreateAsync(newMealPlan);
        }

        private IMealPlan FillUserAndDateInfoOnCreate(IMealPlan newMealPlan, Guid userId)
        {
            newMealPlan.CreatedBy = userId;
            newMealPlan.UpdatedBy = userId;
            newMealPlan.DateUpdated = DateTime.UtcNow;
            newMealPlan.DateCreated = DateTime.UtcNow;
            newMealPlan.IsActive= true;
            newMealPlan.Id= Guid.NewGuid();
            return newMealPlan;
        }

        public async Task<string> DeleteAsync(Guid mealPlanId)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            var time = DateTime.UtcNow;
            return await _mealPlanRepository.DeleteAsync(mealPlanId,userId,time);
        }

        public async Task<PagedList<IMealPlan>> GetAllsync(MealPlanFilter filter, Sorting sorting, Paging paging)
        {
            return await _mealPlanRepository.GetAllAsync(filter, sorting, paging);
        }

        public async Task<IMealPlan> GetByIdAsync(Guid id)
        {
            return await _mealPlanRepository.GetByIdAsync(id);
        }

        public async Task<string> UpdateAsync(IMealPlan updatedMealPlan)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());
            updatedMealPlan = FillUserAndDateInfoOnUpdate(updatedMealPlan, userId);
            return await _mealPlanRepository.UpdateAsync(updatedMealPlan);
        }

        private IMealPlan FillUserAndDateInfoOnUpdate(IMealPlan updatedMealPlan, Guid userId)
        {
            updatedMealPlan.UpdatedBy = userId;
            updatedMealPlan.DateUpdated = DateTime.UtcNow;
            return updatedMealPlan;
        }
    }
}
