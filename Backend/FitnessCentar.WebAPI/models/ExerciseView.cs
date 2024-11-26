using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.Models
{
    public class ExerciseView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
    }
}