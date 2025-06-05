import React, { useEffect } from "react";
import { toJS } from "mobx";
import { mealPlanStore } from "../stores/MealPlanStore";

const MealPlanPage = () => {
  useEffect(() => {
    const fetchData = async () => {
      await mealPlanStore.fetchAll();
      console.log("Fetched MealPlan list:", toJS(mealPlanStore.mealPlanList));
      console.log("PagedMeta:", toJS(mealPlanStore.pagedMeta));

    };
    fetchData();
  }, []);

  return (
    <div>
      <h2>MealPlan Page</h2>
    </div>
  );
};

export default MealPlanPage;
