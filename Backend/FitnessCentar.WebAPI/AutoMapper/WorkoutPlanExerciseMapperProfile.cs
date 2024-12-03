using AutoMapper;
using FitnessCentar.Common;
using FitnessCentar.Model.Common;
using FitnessCentar.Model;
using FitnessCentar.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.AutoMapper
{
    public class WorkoutPlanExerciseMapperProfile:Profile
    {
        public WorkoutPlanExerciseMapperProfile()
        {
            CreateMap<WorkoutPlanExercise, WorkoutPlanExerciseView>();

            CreateMap<PagedList<IWorkoutPlanExercise>, PagedList<WorkoutPlanExercise>>();
            CreateMap<PagedList<WorkoutPlanExercise>, PagedList<WorkoutPlanExerciseView>>();
            CreateMap<WorkoutPlanExerciseCreate, WorkoutPlanExercise>();
            CreateMap<WorkoutPlanExerciseUpdate, WorkoutPlanExercise>();
        }
    }
}