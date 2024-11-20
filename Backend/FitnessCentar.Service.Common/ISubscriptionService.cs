using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Service.Common
{
    public interface ISubscriptionService
    {
        Task<PagedList<ISubscription>> GetAlISubscriptionAsync(SubscriptionFilter filter, Sorting sorting, Paging paging);
        Task<ISubscription> GetSubscriptionByIdAsync(Guid id);
        Task<string> DeleteSubscriptionAsync(Guid id);
        Task<string> CreateSubscriptionAsync(ISubscription newSubscription);
        Task<string> UpdateSubscriptionAsync(Guid id, ISubscription updatedSubscription);
    }
}
