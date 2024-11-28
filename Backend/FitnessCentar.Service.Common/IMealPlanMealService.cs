using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Service.Common
{
    public interface IMealPlanMealService
    {
        Task<PagedList<IMealPlanMeal>> GetAllsync(MealPlanMealFilter filter, Sorting sorting, Paging paging);
        Task<IMealPlanMeal> GetByIdAsync(Guid id);
        Task<string> DeleteAsync(Guid mealPlanMealId);
        Task<string> CreateAsync(IMealPlanMeal newMealPlanMeal);
        Task<string> UpdateAsync(IMealPlanMeal updatedMealPlanMeal);
    }
}
