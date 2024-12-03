﻿using System;
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
            builder.RegisterType<SubscriptionRepository>().As<ISubscriptionRepository>();
            builder.RegisterType<FoodRepository>().As<IFoodRepository>();
            builder.RegisterType<MealRepository>().As<IMealRepository>();
            builder.RegisterType<MealPlanRepository>().As<IMealPlanRepository>();
            builder.RegisterType<MealPlanMealRepository>().As<IMealPlanMealRepository>();
            builder.RegisterType<UserRepository>().As<IUserRepository>();
            builder.RegisterType<RoleTypeRepository>().As<IRoleTypeRepository>();
            builder.RegisterType<ExerciseRepository>().As<IExerciseRepository>();
            builder.RegisterType<WorkoutPlanRepository>().As<IWorkoutPlanRepository>();
            builder.RegisterType<WorkoutPlanExerciseRepository>().As<IWorkoutPlanExerciseRepository>();
        }
    }
}
