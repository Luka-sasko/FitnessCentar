using AutoMapper;
using FitnessCentar.Common;
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
    public class ExerciseController : ApiController
    {
        private readonly IExerciseService _exerciseService;
        private readonly IMapper _mapper;

        public ExerciseController(IExerciseService exerciseService,IMapper mapper)
        {
            _exerciseService = exerciseService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<HttpResponseMessage> GetAllExercises([FromUri] 
        int pageNumber=1,
        int pageSize=10,
        string sortBy="Name",
        string sortOrder="ASC",
        string searchQuery = null)

        {
            Paging paging=new Paging() { PageNumber=pageNumber,PageSize=pageSize};
            Sorting sorting=new Sorting() { SortBy=sortBy,SortOrder=sortOrder};
            ExerciseFilter filter=new ExerciseFilter() {SearchQuery=searchQuery };

            try
            {
                var allExercises = await _exerciseService.GetAllExercisesAsync(filter, sorting, paging);
                if (allExercises == null) { return Request.CreateResponse(HttpStatusCode.NotFound); }
                var exerciseViews = allExercises.Items.Select(e => new ExerciseView
                {
                    Id=e.Id,
                    Name=e.Name,
                    Desc=e.Desc

                }).ToList();

                var plExerciseViews = new PagedList<ExerciseView>(

                   exerciseViews, allExercises.PageNumber, allExercises.PageSize, allExercises.TotalCount);
                if (plExerciseViews != null)
                {
                    return Request.CreateResponse(HttpStatusCode.Found, plExerciseViews);
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
