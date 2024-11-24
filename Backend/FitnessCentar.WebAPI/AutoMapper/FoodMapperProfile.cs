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
    public class FoodMapperProfile : Profile
    {

        public FoodMapperProfile() 
        {
            CreateMap<FoodCreate, Food>();
            CreateMap<Food, FoodView>();
            CreateMap<FoodUpdate, Food>();
        }
    }
}