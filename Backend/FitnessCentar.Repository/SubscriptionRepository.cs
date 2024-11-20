using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using FitnessCentar.Repository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Repository
{
    public class SubscriptionRepository : ISubscriptionRepository
    {
        public Task<string> CreateSubscriptionAsync(ISubscription newSubscription)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteSubscriptionAsync(Guid discountId, Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<ISubscription>> GetAllSubscriptionAsync(SubscriptionFilter filter, Sorting sorting, Paging paging)
        {
            throw new NotImplementedException();
        }

        public Task<ISubscription> GetSubscriptionByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateSubscriptionAsync(ISubscription updatedSubscription)
        {
            throw new NotImplementedException();
        }
    }
}
