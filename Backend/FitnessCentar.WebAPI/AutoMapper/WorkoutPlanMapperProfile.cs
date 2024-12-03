using AutoMapper;
using FitnessCentar.Common;
using FitnessCentar.Model;
using FitnessCentar.Model.Common;
using FitnessCentar.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FitnessCentar.WebAPI.AutoMapper
{
    public class WorkoutPlanMapperProfile:Profile
    {
        public WorkoutPlanMapperProfile()
        {
            CreateMap<WorkoutPlan, WorkoutPlanView>();
            CreateMap<PagedList<IWorkoutPlan>, PagedList<WorkoutPlan>>();
            CreateMap<PagedList<WorkoutPlan>, PagedList<WorkoutPlanView>>();
            CreateMap<WorkoutPlanCreate, WorkoutPlan>();
            CreateMap<WorkoutPlanUpdate,WorkoutPlan>();
        }
    }
}