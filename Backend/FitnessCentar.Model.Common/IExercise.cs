using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Model.Common
{
    public interface IExercise
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Desc { get; set; }
        int Reps { get; set; }
        int Sets { get; set; }
        int RestPeriod { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DatedUpdated { get; set; }
        bool? IsActive { get; set; }

    }
}
