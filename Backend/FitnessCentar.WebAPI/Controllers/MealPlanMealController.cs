using AutoMapper;
using FitnessCentar.Common;
using FitnessCentar.Model;
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
    public class MealPlanMealController : ApiController
    {
        private readonly IMealPlanMealService _mealPlanMealService;
        private readonly IMapper _mapper;
        public MealPlanMealController(IMealPlanMealService mealPlanMealService, IMapper mapper)
        {
            _mealPlanMealService = mealPlanMealService;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<HttpResponseMessage> GetAllAsync
            (
                Guid? mealId = null,
                Guid? mealPlanId = null,
                string sortBy = "Id",
                string sortOrder = "ASC",
                int pageNumber = 1,
                int pageSize = 10
            )
        {
            Paging paging = new Paging() { PageNumber = pageNumber, PageSize = pageSize };
            Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = sortOrder };
            MealPlanMealFilter filter = new MealPlanMealFilter() { MealId = mealId, MealPlanId = mealPlanId };
            try
            {
                var mealPlanMeals = await _mealPlanMealService.GetAllsync(filter,sorting,paging);
                var plMealPlanMeals = _mapper.Map<PagedList<MealPlanMeal>>(mealPlanMeals);
                var plMealPlanMealView = _mapper.Map<PagedList<MealPlanMealView>>(plMealPlanMeals);
                if (plMealPlanMealView == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                return Request.CreateResponse(HttpStatusCode.Found, plMealPlanMealView);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<HttpResponseMessage> GetByIdAsync([FromUri] Guid id)
        {
            try
            {
                var mealPlanMeal = await _mealPlanMealService.GetByIdAsync(id);
                var mealPlanMealView = _mapper.Map<MealPlanMealView>(mealPlanMeal);
                if (mealPlanMealView == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                return Request.CreateResponse(HttpStatusCode.Found, mealPlanMealView);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, User")]
        public async Task<HttpResponseMessage> DeleteAsync ([FromUri] Guid id)
        {
            try
            {
                var mealPlanMealDeleted = await _mealPlanMealService.DeleteAsync(id);
                if (mealPlanMealDeleted == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                return Request.CreateResponse(HttpStatusCode.OK, mealPlanMealDeleted);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPut]
        [Authorize(Roles = "Admin, User")]
        public async Task<HttpResponseMessage> PutAsync([FromUri] Guid id, [FromBody]  MealPlanMealUpdate mealPlanMealUpdate)
        {
            var mealPlanMeal = _mapper.Map<MealPlanMeal>(mealPlanMealUpdate);
            mealPlanMeal.Id = id;
            try
            {
                string mealPlanMealUpdated = await _mealPlanMealService.UpdateAsync(mealPlanMeal);
                if (mealPlanMealUpdated == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                return Request.CreateResponse(HttpStatusCode.OK, mealPlanMealUpdated);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<HttpResponseMessage> PostAsync([FromBody] MealPlanMealCreate mealPlanMeal)
        {
            var newMealPlanMeal = _mapper.Map<MealPlanMeal>(mealPlanMeal);
            try
            {
                string mealPlanMealCreated = await _mealPlanMealService.CreateAsync(newMealPlanMeal);
                if (mealPlanMealCreated == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                return Request.CreateResponse(HttpStatusCode.OK, mealPlanMealCreated);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }
    }
}
