using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Service.Common
{
    public interface IFoodService
    {
        Task<PagedList<IFood>> GetAllFoodAsync(FoodFilter filter, Sorting sorting, Paging paging);
        Task<IFood> GetFoodById(Guid id);
        Task<string> DeleteFoodAsync(Guid foodId);
        Task<string> CreateFoodAsync(IFood newFood);
        Task<string> UpdateFoodAsync(IFood updatedFood);
    }
}
