using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using FitnessCentar.Common;
using FitnessCentar.Model;
using FitnessCentar.Model.Common;
using FitnessCentar.WebAPI.Models;

namespace FitnessCentar.WebAPI.AutoMapper
{
    public class MealPlanMealMapperProfile : Profile
    {
        public MealPlanMealMapperProfile()
        {
            CreateMap<MealPlanMealCreate, MealPlanMeal>();
            CreateMap<MealPlanMeal, MealPlanMealView>();
            CreateMap<MealPlanMealUpdate, MealPlanMeal>();

            CreateMap<PagedList<IMealPlanMeal>, PagedList<MealPlanMeal>>();
            CreateMap<PagedList<MealPlanMeal>, PagedList<MealPlanMealView>>();
        }
    }
}