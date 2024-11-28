using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Service.Common
{
    public interface IExerciseService
    {
        Task<PagedList<IExercise>> GetAllExercisesAsync(ExerciseFilter filter, Sorting sorting, Paging paging);
        Task<IExercise> GetExerciseById(Guid id);
        Task<string> DeleteExerciseAsync(Guid exerciseId);
    }
}
