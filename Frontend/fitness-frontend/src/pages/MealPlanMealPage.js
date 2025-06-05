import React, { useEffect } from "react";
import { toJS } from "mobx";
import { mealPlanMealStore } from "../stores/MealPlanMealStore";

const MealPlanMealPage = () => {
  useEffect(() => {
    const fetchData = async () => {
      await mealPlanMealStore.fetchAll();
      console.log("Fetched MealPlanMeal list:", toJS(mealPlanMealStore.mealPlanMealList));
      console.log("PagedMeta:", toJS(mealPlanMealStore.pagedMeta));

    };
    fetchData();
  }, []);

  return (
    <div>
      <h2>MealPlanMeal Page</h2>
    </div>
  );
};

export default MealPlanMealPage;
