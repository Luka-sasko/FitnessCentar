using Autofac;
using FitnessCentar.Service.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnessCentar.Service
{
    public class ServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
           //builder.RegisterType<Class1>().As<IClass1>();
           builder.RegisterType<DiscountService>().As<IDiscountService>();
           builder.RegisterType<SubscriptionService>().As<ISubscriptionService>();
           builder.RegisterType<FoodService>().As<IFoodService>();

        }
    }
}
