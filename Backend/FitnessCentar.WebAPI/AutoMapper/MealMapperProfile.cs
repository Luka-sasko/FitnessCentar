using AutoMapper;
using FitnessCentar.Model;
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
        }
    }
}