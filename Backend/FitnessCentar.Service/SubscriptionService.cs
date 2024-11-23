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
    public class SubscriptionService : ISubscriptionService
    {
        private readonly ISubscriptionRepository _subscriptionRepository;
        private readonly Guid _userId = Guid.NewGuid();

        public SubscriptionService(ISubscriptionRepository subscriptionRepository)
        {
            _subscriptionRepository = subscriptionRepository;
        }
        public async Task<string> CreateSubscriptionAsync(ISubscription newSubscription)
        {
            ISubscription subscription = FillDateAndUserInfoForCreate(newSubscription,_userId);
            return await _subscriptionRepository.CreateSubscriptionAsync(subscription);
        }

        private ISubscription FillDateAndUserInfoForCreate(ISubscription newSubscription, Guid userId)
        {
            newSubscription.Id=Guid.NewGuid();
            newSubscription.DateCreated = DateTime.UtcNow;
            newSubscription.DateUpdated = DateTime.UtcNow;
            newSubscription.CreatedBy = userId;
            newSubscription.UpdatedBy = userId;
            newSubscription.IsActive = true;
            newSubscription.DiscountId = Guid.Parse("22222222-2222-2222-2222-222222222222");

            return newSubscription;
        }

        public async Task<string> DeleteSubscriptionAsync(Guid id)
        {
            return await _subscriptionRepository.DeleteSubscriptionAsync(id,_userId);
        }

        public async Task<PagedList<ISubscription>> GetAlSubscriptionAsync(SubscriptionFilter filter, Sorting sorting, Paging paging)
        {
            return await _subscriptionRepository.GetAllSubscriptionAsync(filter, sorting, paging);
        }

        public Task<ISubscription> GetSubscriptionByIdAsync(Guid id)
        {
           return _subscriptionRepository.GetSubscriptionByIdAsync(id);
        }

        public async Task<string> UpdateSubscriptionAsync(Guid id, ISubscription updatedSubscription)
        {
            ISubscription subscription = FillDateAndUserInfoForUpdate(updatedSubscription, _userId);
            subscription.Id = id;
            return await _subscriptionRepository.UpdateSubscriptionAsync(subscription);
        }

        private ISubscription FillDateAndUserInfoForUpdate(ISubscription updatedSubscription, Guid userId)
        {
            updatedSubscription.DateUpdated = DateTime.UtcNow;
            updatedSubscription.UpdatedBy = userId;
            return updatedSubscription;
        }
    }
}
