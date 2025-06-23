using AutoMapper;
using FitnessCentar.Common;
using FitnessCentar.Model;
using FitnessCentar.Model.Common;
using FitnessCentar.Service.Common;
using FitnessCentar.WebAPI.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace FitnessCentar.WebAPI.Controllers
{
    [RoutePrefix("api/meal")]
    public class MealController : ApiController
    {
        private readonly IMealService _mealService;
        private readonly IMapper _mapper;

        public MealController(IMealService mealService, IMapper mapper)
        {
            _mealService = mealService;
            _mapper = mapper;
        }


        [HttpGet]
        public async Task<HttpResponseMessage> GetAllAsync(
            string searchQuery = null,
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "Name",
            string sortOrder = "ASC"
        )
        {
            Paging paging = new Paging() { PageNumber = pageNumber, PageSize=pageSize};
            Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = sortOrder }; 
            MealFilter mealFilter = new MealFilter() { SearchQuery = searchQuery};

            try {
                var meals = await _mealService.GetAllsync(mealFilter, sorting, paging);
                if (meals == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);

                var plMeal = _mapper.Map<PagedList<Meal>>(meals);
                var plMealsView = _mapper.Map<PagedList<MealView>>(plMeal);
                if (plMealsView != null)
                    return Request.CreateResponse(HttpStatusCode.OK, plMealsView);

                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("by-user")]
        public async Task<IHttpActionResult> GetMealsByUser(string searchQuery = null,
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "Name",
            string sortOrder = "ASC")
        {
            Paging paging = new Paging() { PageNumber = pageNumber, PageSize = pageSize };
            Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = sortOrder };
            MealFilter mealFilter = new MealFilter() { SearchQuery = searchQuery };
            var meals = await _mealService.GetMealsForUserAsync(mealFilter, sorting, paging);
            if (meals == null)
                return BadRequest("No meals for user");
            return Ok(meals);
        }



        [HttpGet]
        [Route("{id:guid}")]
        public async Task<HttpResponseMessage> GetByIdAsync(Guid id)
        {
            try
            {
                var meal = await _mealService.GetByIdAsync(id);
                if (meal == null)
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                return Request.CreateResponse(HttpStatusCode.OK,meal);
            }
            catch(Exception ex) { return Request.CreateResponse(HttpStatusCode.InternalServerError, ex); }
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteAsync([FromUri] Guid id)
        {
            try
            {
                string mealDeleted = await _mealService.DeleteAsync(id);
                if (mealDeleted != null)
                    return Request.CreateResponse(HttpStatusCode.OK, mealDeleted);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

        }
        [HttpPut]
        public async Task<HttpResponseMessage> PutAsync([FromUri] Guid id, [FromBody] MealUpdate mealUpdate)
        {
            var meal = _mapper.Map<Meal>(mealUpdate);
            meal.Id = id;
            try
            {
                string mealUpdated = await _mealService.UpdateAsync(meal);
                if (mealUpdated != null) { return Request.CreateResponse(HttpStatusCode.OK, mealUpdated); }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync([FromBody] MealCreate mealCreate)
        {
            var meal = _mapper.Map<Meal>(mealCreate);

            try 
            {
                string mealCreated = await _mealService.CreateAsync(meal);
                if(mealCreated != null) { return Request.CreateResponse(HttpStatusCode.Created,mealCreated); }
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }

            catch (Exception ex) { return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);  }

        }


    }
}
