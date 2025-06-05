using AutoMapper;
using AutoMapper.Mappers;
using FitnessCentar.Common;
using FitnessCentar.Model;
using FitnessCentar.Repository.Common;
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
    public class WorkoutPlanExerciseController : ApiController
    {
        private readonly IMapper _mapper;
        private readonly IWorkoutPlanExerciseService _workoutPlanExerciseService;
        public WorkoutPlanExerciseController(IWorkoutPlanExerciseService workoutPlanExerciseService,IMapper mapper)
        {
            _mapper = mapper;
            _workoutPlanExerciseService = workoutPlanExerciseService;
        }
        [HttpGet]
       // [Authorize(Roles = "Admin, User")]
        public async Task<HttpResponseMessage> GetAllAsync
            (
            Guid? workoutPlanId=null,
            Guid? exerciseId=null,
            int exerciseNumber=1,
            string sortBy = "Id",
            string sortOrder = "ASC",
            int pageNumber = 1,
            int pageSize = 10
            )
        {
            Paging paging = new Paging() { PageNumber = pageNumber, PageSize = pageSize };
            Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = sortOrder };
            WorkoutPlanExerciseFilter filter=new WorkoutPlanExerciseFilter() {WorkoutPlanId=workoutPlanId,ExerciseId=exerciseId,ExerciseNumber=exerciseNumber };
            try
            {
                var workoutPlanExercises = await _workoutPlanExerciseService.GetAllAsync(filter, sorting, paging);
                var plWorkoutPlanExercises = _mapper.Map<PagedList<WorkoutPlanExercise>>(workoutPlanExercises);
                var plWorkoutPlanExerciseView = _mapper.Map<PagedList<WorkoutPlanExerciseView>>(plWorkoutPlanExercises);
                if (plWorkoutPlanExerciseView == null)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                return Request.CreateResponse(HttpStatusCode.OK, plWorkoutPlanExerciseView);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpGet]
       // [Authorize(Roles = "Admin, User")]
        public async Task<HttpResponseMessage> GetById([FromUri] Guid id)
        {
            try
            {
                var workoutPlanExercise = await _workoutPlanExerciseService.GetByIdAsync(id);
                var workoutPlanExerciseView = _mapper.Map<WorkoutPlanExerciseView>(workoutPlanExercise);
                if(workoutPlanExerciseView == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }
                return   Request.CreateResponse(HttpStatusCode.OK, workoutPlanExerciseView);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }
        }

        [HttpDelete]
       // [Authorize(Roles = "Admin, User")]
        public async Task<HttpResponseMessage> DeleteAsync([FromUri] Guid id)
        {
            try
            {
                var workoutPlanExerciseDeleted = await _workoutPlanExerciseService.DeleteAsync(id);
                if (workoutPlanExerciseDeleted == null) { return Request.CreateResponse(HttpStatusCode.BadRequest); }
                return Request.CreateResponse(HttpStatusCode.OK, workoutPlanExerciseDeleted);
            }
            catch(Exception ex) { return Request.CreateResponse(HttpStatusCode.InternalServerError, ex); }


        }

        [HttpPost]
       // [Authorize(Roles = "Admin, User")]
        public async Task<HttpResponseMessage> CreateAsync([FromBody] WorkoutPlanExerciseCreate workoutPlanExerciseCreate)
        {
            var newWorkoutPlanExercise = _mapper.Map<WorkoutPlanExercise>(workoutPlanExerciseCreate);
            try
            {
                string workoutPlanCreated = await _workoutPlanExerciseService.CreateAsync(newWorkoutPlanExercise);
                if (workoutPlanCreated == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                return Request.CreateResponse(HttpStatusCode.OK, workoutPlanCreated);
            }
            catch(Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

        }

        [HttpPut]
      //  [Authorize(Roles = "Admin,User")]
        public async Task<HttpResponseMessage> UpdateAsync([FromUri] Guid id,[FromBody] WorkoutPlanExerciseUpdate workoutPlanExerciseUpdate)
        {
            var workoutPlanExercise = _mapper.Map<WorkoutPlanExercise>(workoutPlanExerciseUpdate);
            workoutPlanExercise.Id = id;

            try
            {
                string workoutPlanExerciseUpdated = await _workoutPlanExerciseService.UpdateAsync(workoutPlanExercise);
                if(workoutPlanExerciseUpdated == null)
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                return Request.CreateResponse(HttpStatusCode.OK, workoutPlanExerciseUpdated);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError, ex);
            }

        }
    }
}
