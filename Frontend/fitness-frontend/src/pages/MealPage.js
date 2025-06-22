import React, { useEffect } from "react";
import { mealStore } from "../stores/MealStore";
import GenericTable from "../components/Common/GenericTable";

const MealPage = () => {
  useEffect(() => {
    const fetchData = async () => {
      await mealStore.fetchByUser();
    };
    fetchData();
  }, []);

  return (
    <div>
      <h2>MY MEALS</h2>
      <GenericTable
        store={{
          items: mealStore.mealList,
          sortBy: "",
          sortOrder: "asc",
          setSort: () => { },
          currentPage: 1,
          totalPages: 1,
          setCurrentPage: () => { }
        }}

      />
    </div>
  );
};

export default MealPage;
