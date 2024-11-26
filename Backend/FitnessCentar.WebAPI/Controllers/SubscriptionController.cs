using AutoMapper;
using FitnessCentar.Common;
using FitnessCentar.Model;
using FitnessCentar.Model.Common;
using FitnessCentar.Service;
using FitnessCentar.Service.Common;
using FitnessCentar.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace FitnessCentar.WebAPI.Controllers
{
    public class SubscriptionController : ApiController

    {

        private readonly ISubscriptionService _subscriptionService;
        private readonly IMapper _mapper;
        public SubscriptionController(ISubscriptionService subscriptionService, IMapper mapper)
        {
            _subscriptionService = subscriptionService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAllAsync(
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "StartDate",
            string sortOrder = "ASC",
            string searchQuery = null,
            DateTime? startDate = null,
            decimal? price = null,
            int? duration = null
            )
        { 
            Paging paging = new Paging() {PageNumber=pageNumber, PageSize = pageSize };
            Sorting sorting = new Sorting() {SortBy=sortBy, SortOrder = sortOrder };
            SubscriptionFilter filter = new SubscriptionFilter() { SearchQuery = searchQuery, Duration = duration, Price = price, StartDate = startDate };

            try
            {

                var subscriptions = await _subscriptionService.GetAlSubscriptionAsync(filter, sorting, paging);
                if (subscriptions == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }

                var plSubscriptions = _mapper.Map<PagedList<Subscription>>(subscriptions);
                var plSubscriptionsView = _mapper.Map<PagedList<SubscriptionView>>(plSubscriptions);

                if (plSubscriptionsView != null)
                    return Request.CreateResponse(HttpStatusCode.OK, plSubscriptionsView);

                return Request.CreateResponse(HttpStatusCode.NotFound);
            
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetByIdAsync([FromUri] Guid id)
        { 
            try
            {
                var subscription = await _subscriptionService.GetSubscriptionByIdAsync(id);
                if(subscription == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }

                var subscriptionView = _mapper.Map<SubscriptionView>(subscription);
                return Request.CreateResponse(HttpStatusCode.OK, subscriptionView);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync([FromBody] SubscriptionCreate newSubscription)
        {
            if (newSubscription == null) { return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request: Null object!"); }

            var subscription = _mapper.Map<Subscription>(newSubscription);
            try
            {
                string createdSubscription= await _subscriptionService.CreateSubscriptionAsync(subscription);
                if (createdSubscription != null)
                    return Request.CreateResponse(HttpStatusCode.OK, createdSubscription);

                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<HttpResponseMessage> PutAsync([FromUri] Guid id, [FromBody] SubscriptionUpdate subscriptionUpdate) 
        {
            if (subscriptionUpdate == null) { return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request: Null object!"); }

            if (subscriptionUpdate.DiscountId == null) { subscriptionUpdate.DiscountId = Guid.Parse("22222222-2222-2222-2222-222222222222"); }

            var subscription = _mapper.Map<Subscription>(subscriptionUpdate);


            try
            { 
                string updatedSubscription = await _subscriptionService.UpdateSubscriptionAsync(id, subscription);
                if (updatedSubscription != null)
                    return Request.CreateResponse(HttpStatusCode.OK, updatedSubscription);

                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteAsync([FromUri] Guid id)
        {
            try
            { 
                var deleteSubscription = await _subscriptionService.DeleteSubscriptionAsync(id);
                if (deleteSubscription != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, deleteSubscription);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }


        
    }
}
