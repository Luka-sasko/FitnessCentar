using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using FitnessCentar.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Service
{
    public class SubscriptionService : ISubscriptionService
    {
        public Task<string> CreateSubscriptionAsync(ISubscription newSubscription)
        {
            throw new NotImplementedException();
        }

        public Task<string> DeleteSubscriptionAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<PagedList<ISubscription>> GetAlISubscriptionAsync(SubscriptionFilter filter, Sorting sorting, Paging paging)
        {
            throw new NotImplementedException();
        }

        public Task<ISubscription> GetSubscriptionByIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<string> UpdateSubscriptionAsync(Guid id, ISubscription updatedSubscription)
        {
            throw new NotImplementedException();
        }
    }
}
