using AutoMapper;
using FitnessCentar.Common;
using FitnessCentar.Model;
using FitnessCentar.Service.Common;
using FitnessCentar.WebAPI.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNet.Identity;

namespace FitnessCentar.WebAPI.Controllers
{
    public class ExerciseController : ApiController
    {
        private readonly IExerciseService _exerciseService;
        private readonly IMapper _mapper;

        public ExerciseController(IExerciseService exerciseService, IMapper mapper)
        {
            _exerciseService = exerciseService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAllExercises(
            [FromUri] int pageNumber = 1,
            [FromUri] int pageSize = 10,
            [FromUri] string sortBy = "Name",
            [FromUri] string sortOrder = "ASC",
            [FromUri] string searchQuery = null)
        {
            Paging paging = new Paging() { PageNumber = pageNumber, PageSize = pageSize };
            Sorting sorting = new Sorting() { SortBy = sortBy, SortOrder = sortOrder.ToUpper() };
            ExerciseFilter filter = new ExerciseFilter() { SearchQuery = searchQuery };


            try
            {
                var allExercises = await _exerciseService.GetAllExercisesAsync(filter, sorting, paging);
                if (allExercises == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }
                var exerciseViews = allExercises.Items.Select(e => new ExerciseView
                {
                    Id = e.Id,
                    Name = e.Name,
                    Desc = e.Desc,
                    Reps=e.Reps,
                    Sets=e.Sets,
                    RestPeriod=e.RestPeriod,

                }).ToList();

                var plExerciseViews = new PagedList<ExerciseView>(
                    exerciseViews, allExercises.PageNumber, allExercises.PageSize, allExercises.TotalCount);
                if (plExerciseViews != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, plExerciseViews);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetExerciseById([FromUri] Guid id)
        {
            try
            {
                var exercise = await _exerciseService.GetExerciseById(id);
                var exerciseView = _mapper.Map<ExerciseView>(exercise);
                if (exerciseView == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }
                return Request.CreateResponse(HttpStatusCode.OK, exerciseView);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        public async Task<HttpResponseMessage> DeleteExerciseAsync([FromUri] Guid id)
        {
            string deletedExercise = await _exerciseService.DeleteExerciseAsync(id);
            try
            {
                if (deletedExercise != null) { return Request.CreateResponse(HttpStatusCode.OK, deletedExercise); }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        public async Task<HttpResponseMessage> PostExercise([FromBody] ExerciseCreate exerciseCreate)
        {
            var newExercise = _mapper.Map<Exercise>(exerciseCreate);

            try
            {
                Guid createdExerciseId = await _exerciseService.CreateExerciseAsync(newExercise);
                if (createdExerciseId != null)
                {
                    return Request.CreateResponse(HttpStatusCode.Created, createdExerciseId);
                }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        public async Task<HttpResponseMessage> PutExercise([FromUri] Guid id, [FromBody] ExerciseUpdate exerciseUpdate)
        {
            var updateExercise = _mapper.Map<Exercise>(exerciseUpdate);
            updateExercise.Id = id;
            try
            {
                string updatedExercise = await _exerciseService.UpdateExerciseAsync(updateExercise);
                if (updatedExercise != null) { return Request.CreateResponse(HttpStatusCode.OK, updatedExercise); }
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
