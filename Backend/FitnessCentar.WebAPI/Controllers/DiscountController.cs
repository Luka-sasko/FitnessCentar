using FitnessCentar.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using FitnessCentar.Model.Common;
using FitnessCentar.WebAPI.Models;
using AutoMapper;
using FitnessCentar.Model;
using FitnessCentar.Common;

namespace FitnessCentar.WebAPI.Controllers
{
    public class DiscountController : ApiController
    {
        private readonly IDiscountService _discountService;
        private readonly IMapper _mapper;

        public DiscountController(IDiscountService discountService,IMapper mapper)
        {
            _discountService = discountService;
            _mapper = mapper;
        }

        [HttpPut]
        //PUT api/Discount/id
        public async Task<HttpResponseMessage> PutAsync([FromUri] Guid id,[FromBody] DiscountUpdate editedDiscount)
        {

            if (!ValidateInputDate(editedDiscount.StartDate, editedDiscount.EndDate)) 
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Wrong date input!");

            }

            IDiscount discount = _mapper.Map<Discount>(editedDiscount);
            discount.Id = id;

            try 
            {

                string updatedDiscount = await _discountService.UpdateDiscountAsync(id, discount);

                if (updatedDiscount != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, updatedDiscount);
                }

                return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request!");
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        //[Authorize(Roles = "User, Admin")]
        //POST api/Discount
        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync([FromBody] DiscountCreate newDiscount)
        {

            if (!ValidateInputDate(newDiscount.StartDate, newDiscount.EndDate)) 
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Wrong date input!");

            }

            IDiscount discount = _mapper.Map<Discount>(newDiscount);

            try
            {
                string createdDiscount = await _discountService.CreateDiscountAsync(discount);
                if (createdDiscount != null)
                    return Request.CreateResponse(HttpStatusCode.OK, createdDiscount);

                return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request!");
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteAsync([FromUri] Guid id) 
        { 
            if(id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request!");

            }

            try 
            {
                string deletedDiscount = await _discountService.DeleteDiscountAsync(id);
                if (deletedDiscount != null)
                    return Request.CreateResponse(HttpStatusCode.OK, deletedDiscount);

                return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request!");
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetById([FromUri] Guid id)
        {
            if (id == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request!");

            }
            try
            {
                IDiscount discountByID = await _discountService.GetDiscountByIdAsync(id);
                DiscountView discountView = _mapper.Map<DiscountView>(discountByID);
                if (discountByID != null)
                    return Request.CreateResponse(HttpStatusCode.OK, discountView);

                return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request!");
            }

            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAll
            (
            int pageNumber = 1,
            int pageSize = 10,
            string sortBy = "StartDate",
            string sortOrder = "ASC",
            string searchQuery = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? amount = null
            )
        {
            Paging paging = new Paging() { PageNumber=pageNumber, PageSize=pageSize};
            Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = sortOrder.ToUpper() };
            DiscountFilter filter = new DiscountFilter() {SearchQuery=searchQuery, StartDate=startDate, EndDate=endDate, Amount=amount };

            var discounts = await _discountService.GetAllDiscountsAsync(filter, sorting, paging);
            if(discounts == null)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request!");

            }

            try
            {
                var plDiscounts = _mapper.Map<PagedList<Discount>>(discounts);
                var plDiscountsView = _mapper.Map<PagedList<DiscountView>>(plDiscounts);
                if (plDiscountsView != null)
                    return Request.CreateResponse(HttpStatusCode.OK, plDiscountsView);

                return Request.CreateResponse(HttpStatusCode.BadRequest, "Bad request!");
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }

        }
            

        private bool ValidateInputDate(DateTime startDate, DateTime endDate)
        {
            if ((startDate >= DateTime.Today || endDate >= DateTime.Today) && (startDate < endDate) && (startDate!=DateTime.MinValue && endDate!=DateTime.MinValue))
            {
                return true;
            }
            return false;
            
        }
    }
}