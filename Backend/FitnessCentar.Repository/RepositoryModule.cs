using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using FitnessCentar.Repository.Common;

namespace FitnessCentar.Repository
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder) {

            //builder.RegisterType<Class1>().As<IRClass1>();
            builder.RegisterType<DiscountRepository>().As<IDiscountRepository>();
        }
    }
}
