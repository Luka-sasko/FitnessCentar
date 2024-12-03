using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Common
{
    public class WorkoutPlanFilter
    {
        public Guid? UserId { get; set; }
        public string SearchQuery { get; set; }
    }
}
