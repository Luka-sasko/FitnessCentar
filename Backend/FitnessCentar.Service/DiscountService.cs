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
    public class DiscountService : IDiscountService
    {

        private readonly IDiscountRepository _discountRepository;

        public DiscountService (IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        

        public Task<string> CreateDiscountAsync(IDiscount newDiscount)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());

            IDiscount Discount = FillUserAndDateInformationsOnCreate(newDiscount,userId);

            return _discountRepository.CreateDiscountAsync(Discount);
        }

        

        public Task<string> DeleteDiscountAsync(Guid discountId)
        {
            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());

            return _discountRepository.DeleteDiscountAsync(discountId,userId);
        }

        public Task<PagedList<IDiscount>> GetAllDiscountsAsync(DiscountFilter filter, Sorting sorting, Paging paging)
        {
            return _discountRepository.GetAllDiscountsAsync(filter, sorting, paging);

        }

        public Task<IDiscount> GetDiscountByIdAsync(Guid id)
        {
            return _discountRepository.GetDiscountByIdAsync(id);
        }

        public async Task<string> UpdateDiscountAsync(Guid id, IDiscount discountUpdated)
        {

            var userId = Guid.Parse(HttpContext.Current.User.Identity.GetUserId());

            return await _discountRepository.UpdateDiscountAsync(FillUserAndDateInformationOnUpdate(discountUpdated,userId));
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
