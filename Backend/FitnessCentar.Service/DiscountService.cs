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
    public class DiscountService : IDiscountService
    {

        private readonly IDiscountRepository _discountRepository;
        private readonly Guid _userId = Guid.NewGuid();

        public DiscountService (IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        

        public Task<string> CreateDiscountAsync(IDiscount newDiscount)
        {

            IDiscount Discount = FillUserAndDateInformationsOnCreate(newDiscount,_userId);

            return _discountRepository.CreateDiscountAsync(Discount);
        }

        

        public Task<string> DeleteDiscountAsync(Guid discountId)
        {
            return _discountRepository.DeleteDiscountAsync(discountId,_userId);
        }

        public Task<PagedList<IDiscount>> GetAllDiscountsAsync(DiscountFilter filter, Sorting sorting, Paging paging)
        {
            throw new NotImplementedException();
        }

        public Task<IDiscount> GetDiscountByIdAsync(Guid id)
        {
            return _discountRepository.GetDiscountByIdAsync(id);
        }

        public async Task<string> UpdateDiscountAsync(Guid id, IDiscount discountUpdated)
        {

            return await _discountRepository.UpdateDiscountAsync(FillUserAndDateInformationOnUpdate(discountUpdated,_userId));
        }




        //--------------------------------------------------------------------------------------------------------------------------------------------
        private IDiscount FillUserAndDateInformationOnUpdate(IDiscount discountUpdated,Guid userId)
        {
            IDiscount updatedDiscount = discountUpdated;
            updatedDiscount.DateUpdated = DateTime.UtcNow;
            updatedDiscount.UpdatedBy = userId;
            return updatedDiscount;
        }
        private IDiscount FillUserAndDateInformationsOnCreate(IDiscount newDiscount, Guid userId)
        {
            IDiscount discount = newDiscount;
            discount.Id = Guid.NewGuid(); 
            DateTime date = DateTime.UtcNow;
            discount.CreatedBy = userId;
            discount.UpdatedBy = userId;
            discount.DateUpdated = date;
            discount.DateCreated = date;
            discount.IsActive = true;

            return discount;
        }
    }
}
