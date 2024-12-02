using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Repository.Common
{
    public interface IExerciseRepository
    {
        Task<PagedList<IExercise>> GetAllExercisesAsync(ExerciseFilter filter, Sorting sorting, Paging paging);
        Task<IExercise> GetExerciseById(Guid id);
        Task<string> DeleteExerciseAsync(Guid exerciseId, Guid userId);
        Task<string> CreateExerciseAsync(IExercise newExercise);

        Task<string> UpdateExerciseAsync(IExercise updatedExercise);
    }
}
