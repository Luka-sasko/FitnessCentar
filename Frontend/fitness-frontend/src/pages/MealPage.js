import React, { useEffect } from "react";
import { toJS } from "mobx";
import { mealStore } from "../stores/MealStore";

const MealPage = () => {
  useEffect(() => {
    const fetchData = async () => {
      await mealStore.fetchAll();
      console.log("Fetched Meal list:", toJS(mealStore.mealList));
      console.log("PagedMeta:", toJS(mealStore.pagedMeta));

    };
    fetchData();
  }, []);

  return (
    <div>
      <h2>Meal Page</h2>
    </div>
  );
};

export default MealPage;
