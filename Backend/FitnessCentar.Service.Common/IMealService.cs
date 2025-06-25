using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Service.Common
{
    public interface IMealService
    { 
        Task<PagedList<IMeal>> GetAllsync(MealFilter filter, Sorting sorting, Paging paging);
        Task<IMeal> GetByIdAsync(Guid id);
        Task<string> DeleteAsync(Guid mealId);
        Task<Guid> CreateAsync(IMeal newMeal);
        Task<string> UpdateAsync(IMeal updatedMeal);
        Task<PagedList<IMeal>> GetMealsForUserAsync(MealFilter filter, Sorting sorting, Paging paging);

    }
}
