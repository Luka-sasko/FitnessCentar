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
    public class SubscriptionMapperProfile : Profile
    {
        public SubscriptionMapperProfile()
        {
            CreateMap<SubscriptionCreate, Subscription>();
            CreateMap<Subscription, SubscriptionView>();
            CreateMap<SubscriptionUpdate, Subscription>();

            CreateMap<PagedList<ISubscription>, PagedList<Subscription>>();
            CreateMap<PagedList<Subscription>, PagedList<SubscriptionView>>();
        }

    }
}