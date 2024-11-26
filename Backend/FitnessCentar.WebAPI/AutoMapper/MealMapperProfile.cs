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
    public class MealMapperProfile : Profile
    {
        public MealMapperProfile() 
        {
            CreateMap<MealCreate, Meal>();
            CreateMap<Meal, MealView>();
            CreateMap<MealUpdate, Meal>();

            CreateMap<PagedList<IMeal>, PagedList<Meal>>();
            CreateMap<PagedList<Meal>, PagedList<MealView>>();
        }
    }
}