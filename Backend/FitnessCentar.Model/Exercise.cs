using FitnessCentar.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Model
{
    public class Exercise : IExercise
    {
        public Guid Id { get; set; }
        public string Name { get; set ; }
        public string Desc { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public int RestPeriod { get; set; }
        public Guid CreatedBy { get; set; }
        public Guid UpdatedBy { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DatedUpdated { get; set; }
        public bool? IsActive { get; set; }
        public Guid? UserId { get; set; }
    }
}
