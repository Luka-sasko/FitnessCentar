using AutoMapper;
using FitnessCentar.Model;
using FitnessCentar.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.AutoMapper
{
    public class ExerciseMapperProfile:Profile
    {
        public ExerciseMapperProfile()
        {
            CreateMap<Exercise, ExerciseView>();
            CreateMap<ExerciseCreate,Exercise> ();
            CreateMap<ExerciseUpdate, Exercise> ();
        }
    }
}