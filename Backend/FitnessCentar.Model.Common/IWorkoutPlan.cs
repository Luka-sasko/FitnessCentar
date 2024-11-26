using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Model.Common
{
    public interface IWorkoutPlan
    {
        Guid Id { get; set; }
        string Name { get; set; }
        string Desc { get; set; }
        Guid UserId { get; set; }
        Guid CreatedBy { get; set; }
        Guid UpdatedBy { get; set; }
        DateTime DateCreated { get; set; }
        DateTime DatedUpdated { get; set; }
        bool? IsActive { get; set; }
    }
}
