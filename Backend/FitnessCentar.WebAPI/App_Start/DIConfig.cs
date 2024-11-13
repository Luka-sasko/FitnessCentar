using Autofac;
using Autofac.Integration.WebApi;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using FitnessCentar.Repository;
using FitnessCentar.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;

namespace FitnessCentar.WebAPI.App_Start
{
    public class DIConfig
    {

        public static void Register(HttpConfiguration config)
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule( new ServiceModule());
            builder.RegisterModule( new RepositoryModule());

            builder.AddAutoMapper(Assembly.GetExecutingAssembly());

            var contrainer = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(contrainer);

        }
    }
}