import React, { useEffect } from "react";
import { toJS } from "mobx";
import { exerciseStore } from "../stores/ExerciseStore";

const ExercisePage = () => {
  useEffect(() => {
    const fetchData = async () => {
      await exerciseStore.fetchAll();
      console.log("Fetched Exercise list:", toJS(exerciseStore.exerciseList));
      console.log("PagedMeta:", toJS(exerciseStore.pagedMeta));

    };
    fetchData();
  }, []);

  return (
    <div>
      <h2>Exercise Page</h2>
    </div>
  );
};

export default ExercisePage;
