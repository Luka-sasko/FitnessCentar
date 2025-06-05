using AutoMapper;
using FitnessCentar.Common;
using FitnessCentar.Model;
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
    public class MealPlanController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IMealPlanService _mealPlanService;

        public MealPlanController(IMapper mapper, IMealPlanService mealPlanService)
        {
            _mapper = mapper;
            _mealPlanService = mealPlanService;
        }

        [HttpGet]
       // [Authorize(Roles = "Admin, User")]
        public async Task<HttpResponseMessage> GetByIdAsync([FromUri] Guid id)
        { 
            var mealPlan = await _mealPlanService.GetByIdAsync(id);
            var mealPlanView = _mapper.Map<MealPlanView>(mealPlan);
            if(mealPlanView == null)
                return Request.CreateResponse(HttpStatusCode.NotFound);
            try 
            {
                return Request.CreateResponse(HttpStatusCode.OK,mealPlanView);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        
        [HttpGet]
       // [Authorize(Roles = "Admin, User")]
        public async Task<HttpResponseMessage> GetAllAsync
            (
            Guid? userId = null,
            string searchQuery = null,
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "Name",
            string sortOrder = "ASC"
            )
        {
            Paging paging = new Paging() { PageNumber = pageNumber, PageSize = pageSize };
            Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = sortOrder };
            MealPlanFilter mealPlanFilter = new MealPlanFilter() { SearchQuery = searchQuery , UserId = userId};
            try 
            {
                var mealPlans = await _mealPlanService.GetAllsync(mealPlanFilter, sorting, paging);
                if (mealPlans == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                var plMealPlans = _mapper.Map<PagedList<MealPlan>>(mealPlans);
                var plMealPlansView = _mapper.Map<PagedList<MealPlanView>>(plMealPlans);

                return Request.CreateResponse(HttpStatusCode.OK,plMealPlansView);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }


        [HttpDelete]
       // [Authorize(Roles ="Admin, User")]
        public async Task<HttpResponseMessage> DeleteAsync([FromUri] Guid id)
        {
            try
            {
                var mealPlanDeleted = await _mealPlanService.DeleteAsync(id);
                if (mealPlanDeleted != null) { return Request.CreateResponse(HttpStatusCode.OK,mealPlanDeleted); }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex) { return Request.CreateResponse(HttpStatusCode.InternalServerError, ex); }
        }



        [HttpPost]
       // [Authorize( Roles = "User, Admin")]
        public async Task<HttpResponseMessage> PostAsync([FromBody] MealPlanCreate mealPlanCreate)
        {
            var mealPlan = _mapper.Map<MealPlan>(mealPlanCreate);

            try
            {
                string mealPlanCreated = await _mealPlanService.CreateAsync(mealPlan);
                if (mealPlanCreated == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.Created,mealPlanCreated);
               
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPut]
       // [Authorize(Roles = "User,Admin")]
        public async Task<HttpResponseMessage> PutAsync([FromUri] Guid id, [FromBody] MealPlanUpdate mealPlanUpdate)
        {
            var mealPlan = _mapper.Map<MealPlan>(mealPlanUpdate);
            mealPlan.Id= id;
            try
            {
                string mealPlanUpdated = await _mealPlanService.UpdateAsync(mealPlan);
                if (mealPlanUpdated == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                return Request.CreateResponse(HttpStatusCode.Created, mealPlanUpdated);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ex);
            }

        }
    }
}
