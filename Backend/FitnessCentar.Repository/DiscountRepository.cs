using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using FitnessCentar.Repository.Common;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Repository
{
    public class DiscountRepository : IDiscountRepository
    {

        private readonly string _connectionString = ConfigurationManager.ConnectionStrings["connectionString"].ConnectionString;

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
