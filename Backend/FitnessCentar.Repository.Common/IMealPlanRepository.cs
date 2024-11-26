using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Repository.Common
{
    public interface IMealPlanRepository
    {
        Task<PagedList<IMealPlan>> GetAllAsync(MealPlanFilter filter, Sorting sorting, Paging paging);
        Task<IMealPlan> GetByIdAsync(Guid id);
        Task<string> DeleteAsync(Guid mealPlanId, Guid userId, DateTime time);
        Task<string> CreateAsync(IMealPlan newMealPlan);
        Task<string> UpdateAsync(IMealPlan updatedMealPlan);
    }
}
