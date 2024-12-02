﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.Models
{
    public class ExerciseCreate
    {
        public string Name { get; set; }
        public string Desc { get; set; }
        public int Reps { get; set; }
        public int Sets { get; set; }
        public int RestPeriod { get; set; }
    }
}