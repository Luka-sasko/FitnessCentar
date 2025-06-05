using AutoMapper;
using FitnessCentar.Common;
using FitnessCentar.Model;
using FitnessCentar.Model.Common;
using FitnessCentar.Service.Common;
using FitnessCentar.WebAPI.Models;
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
    public class FoodController : ApiController
    {
        private readonly IFoodService _foodService;
        private readonly IMapper _mapper;

        public  FoodController(IFoodService foodService,IMapper mapper)
        {
            _foodService = foodService;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<HttpResponseMessage> GetAllFood([FromUri]
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "Name",
            string sortOrder = "ASC",
            string searchQuery = null,
            Guid? mealId = null        )
        {
            Paging paging = new Paging() { PageNumber = pageNumber, PageSize=pageSize};
            Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder=sortOrder.ToUpper()};
            FoodFilter filter = new FoodFilter() { SearchQuery = searchQuery, MealId=mealId};

            try
            {
                var allFood = await _foodService.GetAllFoodAsync(filter, sorting, paging);
                if(allFood == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }


                var plFood = _mapper.Map<PagedList<Food>>(allFood);
                var plFoodViews = _mapper.Map<PagedList<FoodView>>(plFood);
                if (plFoodViews != null)
                    return Request.CreateResponse(HttpStatusCode.OK, plFoodViews);

                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetFoodById([FromUri] Guid id)
        {
            try
            {
                var food = await _foodService.GetFoodById(id);
                var foodView = _mapper.Map<FoodView>(food);
                if (foodView == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }
                return Request.CreateResponse(HttpStatusCode.OK, foodView);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostFood([FromBody] FoodCreate foodCreate)
        {
            var newFood = _mapper.Map<Food>(foodCreate);
            
            try
            {
                string createdFood = await _foodService.CreateFoodAsync(newFood);
                if (createdFood != null)
                    return Request.CreateResponse(HttpStatusCode.Created, createdFood);
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

       [HttpPut]
       public async Task<HttpResponseMessage> PutFood([FromUri] Guid id, [FromBody] FoodUpdate foodUpdate)
        {
            var updateFood= _mapper.Map<Food>(foodUpdate);
            updateFood.Id=id;
            try
            {
                string updatedFood = await _foodService.UpdateFoodAsync(updateFood);
                if (updatedFood != null) return Request.CreateResponse(HttpStatusCode.OK, updatedFood);
                return Request.CreateResponse(HttpStatusCode.NotFound);

            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteFood([FromUri] Guid id)
        {
            string deletedFood = await _foodService.DeleteFoodAsync(id);
            try
            {
                if (deletedFood != null) return Request.CreateResponse(HttpStatusCode.OK, deletedFood);
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

    }
}
