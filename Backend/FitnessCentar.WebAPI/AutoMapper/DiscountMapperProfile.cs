using AutoMapper;
using FitnessCentar.Common;
using FitnessCentar.Model;
using FitnessCentar.Model.Common;
using FitnessCentar.WebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace FitnessCentar.WebAPI.AutoMapper
{
    public class DiscountMapperProfile : Profile
    {
        public DiscountMapperProfile() {
            CreateMap<DiscountCreate, Discount>();
            CreateMap<DiscountUpdate, Discount>();
            CreateMap<Discount, DiscountView>();
            CreateMap<PagedList<Discount>, PagedList<DiscountView>>();

            CreateMap<PagedList<IDiscount>, PagedList<Discount>>();
            CreateMap<PagedList<Discount>, PagedList<DiscountView>>();

        }

    }
}