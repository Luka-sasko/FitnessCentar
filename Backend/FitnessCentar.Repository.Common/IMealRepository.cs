using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Repository.Common
{
    public interface IMealRepository
    {
        Task<PagedList<IMeal>> GetAllAsync(MealFilter filter, Sorting sorting, Paging paging);
        Task<IMeal> GetByIdAsync(Guid id);
        Task<string> DeleteAsync(Guid mealId, Guid userId);
        Task<string> CreateAsync(IMeal newMeal);
        Task<string> UpdateAsync(IMeal updatedMeal);
        Task<PagedList<IMeal>> GetMealsForUserAsync(Guid userId, MealFilter filter, Sorting sorting, Paging paging);
        
         

    }
}
