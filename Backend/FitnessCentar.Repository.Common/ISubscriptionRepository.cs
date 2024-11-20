using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Repository.Common
{
    public interface ISubscriptionRepository
    {
        Task<PagedList<ISubscription>> GetAllSubscriptionAsync(SubscriptionFilter filter, Sorting sorting, Paging paging);
        Task<ISubscription> GetSubscriptionByIdAsync(Guid id);
        Task<string> DeleteSubscriptionAsync(Guid discountId, Guid userId);
        Task<string> CreateSubscriptionAsync(ISubscription newSubscription);
        Task<string> UpdateSubscriptionAsync(ISubscription updatedSubscription);
    }
}
