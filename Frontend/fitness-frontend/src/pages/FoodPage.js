import React, { useEffect } from "react";
import { toJS } from "mobx";
import { foodStore } from "../stores/FoodStore";

const FoodPage = () => {
  useEffect(() => {
    const fetchData = async () => {
      await foodStore.fetchAll();
      console.log("Fetched Food list:", toJS(foodStore.foodList));
      console.log("PagedMeta:", toJS(foodStore.pagedMeta));

    };
    fetchData();
  }, []);

  return (
    <div>
      <h2>Food Page</h2>
    </div>
  );
};

export default FoodPage;
