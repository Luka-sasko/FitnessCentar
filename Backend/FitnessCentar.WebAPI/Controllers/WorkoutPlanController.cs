using AutoMapper;
using FitnessCentar.Common;
using FitnessCentar.Model;
using FitnessCentar.Model.Common;
using FitnessCentar.Service.Common;
using FitnessCentar.WebAPI.Models;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Mvc.Async;

namespace FitnessCentar.WebAPI.Controllers
{
    public class WorkoutPlanController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IWorkoutPlanService _workoutPlanService;

        public WorkoutPlanController(IWorkoutPlanService workoutPlanService,IMapper mapper)
        {
            _mapper = mapper;
            _workoutPlanService = workoutPlanService;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<HttpResponseMessage> GetAllAsync
            (
            Guid? userId=null,
            string searchQuery=null,
            int pageNumber=1,
            int pageSize=10,
            string sortBy="Name",
            string sortOrder="ASC"
            )
        {
            Paging paging = new Paging() {PageNumber=pageNumber,PageSize=pageSize };
            Sorting sorting= new Sorting() {SortBy=sortBy,SortOrder=sortOrder };
            WorkoutPlanFilter workoutPlanFilter = new WorkoutPlanFilter() {SearchQuery=searchQuery,UserId=userId };

            try
            {
                var workoutPlans = await _workoutPlanService.GetAllAsync(workoutPlanFilter, sorting, paging);
                if (workoutPlans == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                var plWorkoutPlans = _mapper.Map<PagedList<WorkoutPlan>>(workoutPlans);
                var plWorkoutPlansView = _mapper.Map<PagedList<WorkoutPlanView>>(plWorkoutPlans);

                return Request.CreateResponse(HttpStatusCode.Found, plWorkoutPlansView);
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
            var workoutPlan=await _workoutPlanService.GetByIdAsync(id);
            var workoutPlanView=_mapper.Map<WorkoutPlanView>(workoutPlan);

            if (workoutPlanView == null) {  return Request.CreateResponse(HttpStatusCode.NotFound);}

            try
            {
                return Request.CreateResponse(HttpStatusCode.Found, workoutPlanView);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, User")]
        public async Task<HttpResponseMessage> DeleteAsync([FromUri] Guid Id)
        {
            try
            {
                
                var workoutPlanDeleted = await _workoutPlanService.DeleteAsync(Id);
                if(workoutPlanDeleted == null) { return Request.CreateResponse(HttpStatusCode.BadRequest); }
                return Request.CreateResponse(HttpStatusCode.OK, workoutPlanDeleted);
            }
            catch (Exception ex) { return Request.CreateResponse(HttpStatusCode.InternalServerError, ex); }
        }

        [HttpPost]
        [Authorize(Roles = "Admin, User")]

        public async Task<HttpResponseMessage> CreateAsync([FromBody] WorkoutPlanCreate workoutPlanCreate)
        {
            var workoutPlan=_mapper.Map<WorkoutPlan>(workoutPlanCreate);

            try
            {
                string workoutPlanCreated = await _workoutPlanService.CreateAsync(workoutPlan);
                if(workoutPlanCreated == null) { return Request.CreateResponse(HttpStatusCode.BadRequest); }

                return Request.CreateResponse(HttpStatusCode.Created, workoutPlanCreated);
            }
            catch(Exception ex) { return Request.CreateResponse(HttpStatusCode.BadRequest, ex); }

        }

        [HttpPut]
        [Authorize(Roles = "Admin, User")]
        public async Task<HttpResponseMessage> UpdateAsync([FromUri] Guid id, [FromBody] WorkoutPlanUpdate workoutPlanUpdate)
        {
            var workoutPlan=_mapper.Map<WorkoutPlan>(workoutPlanUpdate);
            workoutPlan.Id = id;
            try
            {
                string workoutPlanUpdated = await _workoutPlanService.UpdateAsync(workoutPlan);
                if (workoutPlanUpdated == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                return Request.CreateResponse(HttpStatusCode.Created, workoutPlanUpdated);
            }
            catch( Exception ex) { return Request.CreateResponse(HttpStatusCode.BadRequest,ex); }

        }


    }
}
