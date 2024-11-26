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
    public class MealPlanMapperProfile : Profile
    {
        public MealPlanMapperProfile() 
        {
            CreateMap<MealPlanCreate, MealPlan>();
            CreateMap<MealPlan, MealPlanView>();
            CreateMap<MealPlanUpdate, MealPlan>();

            CreateMap<PagedList<IMealPlan>, PagedList<MealPlan>>();
            CreateMap<PagedList<MealPlan>, PagedList<MealPlanView>>();
        }
    }
}