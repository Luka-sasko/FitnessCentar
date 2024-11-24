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
    public class FoodService : IFoodService
    {
        private readonly IFoodRepository _foodRepository;
        private readonly Guid _userId= Guid.NewGuid();
        public FoodService(IFoodRepository foodRepository) 
        { 
            _foodRepository = foodRepository;
        }

        public async Task<string> CreateFoodAsync(IFood newFood)
        {
            newFood = FillDateAndUserInfoOnCreate(newFood, _userId);
            return await _foodRepository.CreateFoodAsync(newFood);
        }

        private IFood FillDateAndUserInfoOnCreate(IFood newFood, Guid userId)
        {
            newFood.Id = Guid.NewGuid();
            newFood.DateCreated = DateTime.UtcNow;
            newFood.DateUpdated = DateTime.UtcNow;
            newFood.CreatedBy = _userId;
            newFood.UpdatedBy = _userId;
            newFood.IsActive = true;
            return newFood;
        }

        public async Task<string> DeleteFoodAsync(Guid foodId)
        {
            return await _foodRepository.DeleteFoodAsync(foodId, _userId);
        }

        public async Task<PagedList<IFood>> GetAllFoodAsync(FoodFilter filter, Sorting sorting, Paging paging)
        {
            return await _foodRepository.GetAllFoodAsync(filter, sorting, paging);
        }

        public async Task<IFood> GetFoodById(Guid id)
        {
            return await _foodRepository.GetFoodById(id);
        }

        public async Task<string> UpdateFoodAsync(IFood updatedFood)
        {
            updatedFood=FillDateAndUserInfoOnUpdate(updatedFood, _userId);
            return await _foodRepository.UpdateFoodAsync(updatedFood);
        }

        private IFood FillDateAndUserInfoOnUpdate(IFood updatedFood, Guid userId)
        {
            updatedFood.UpdatedBy= _userId;
            updatedFood.DateUpdated= DateTime.UtcNow;
            return updatedFood;
        }
    }
}
