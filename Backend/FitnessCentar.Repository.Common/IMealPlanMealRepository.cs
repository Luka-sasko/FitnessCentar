using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Repository.Common
{
    public interface IMealPlanMealRepository
    {
        Task<PagedList<IMealPlanMeal>> GetAllAsync(MealPlanMealFilter filter, Sorting sorting, Paging paging);
        Task<IMealPlanMeal> GetByIdAsync(Guid id);
        Task<string> DeleteAsync(Guid mealPlanMealId, Guid userId, DateTime time);
        Task<string> CreateAsync(IMealPlanMeal newMealPlanMeal);
        Task<string> UpdateAsync(IMealPlanMeal updatedMealPlanMeal);
    }
}
