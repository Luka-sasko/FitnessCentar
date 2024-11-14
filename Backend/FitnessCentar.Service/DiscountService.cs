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

        public DiscountService (IDiscountRepository discountRepository)
        {
            _discountRepository = discountRepository;
        }

        public Task<string> CreateDiscountAsync(IDiscount newDiscount, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteDiscountAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<IDiscount>> GetAllDiscountsAsync(DiscountFilter filter, Sorting sorting, Paging paging)
        {
            throw new NotImplementedException();
        }

        public Task<IDiscount> GetDiscountByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateDiscountAsync(Guid id, IDiscount discountUpdated, Guid userId)
        {
            throw new NotImplementedException();
        }
    }
}
