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
           builder.RegisterType<MealService>().As<IMealService>();
           builder.RegisterType<MealPlanService>().As<IMealPlanService>();
           builder.RegisterType<MealPlanMealService>().As<IMealPlanMealService>();
           builder.RegisterType<UserService>().As<IUserService>();
           builder.RegisterType<ExerciseService>().As<IExerciseService>();
            builder.RegisterType<WorkoutPlanService>().As<IWorkoutPlanService>();
            builder.RegisterType<WorkoutPlanExerciseService>().As<IWorkoutPlanExerciseService>();

        }
    }
}
