import React, { useEffect } from "react";
import { toJS } from "mobx";
import { workoutPlanExerciseStore } from "../stores/WorkoutPlanExerciseStore";

const WorkoutPlanExercisePage = () => {
  useEffect(() => {
    const fetchData = async () => {
      await workoutPlanExerciseStore.fetchAll();
      console.log("Fetched WorkoutPlanExercise list:", toJS(workoutPlanExerciseStore.workoutPlanExerciseList));
      console.log("PagedMeta:", toJS(workoutPlanExerciseStore.pagedMeta));

    };
    fetchData();
  }, []);

  return (
    <div>
      <h2>WorkoutPlanExercise Page</h2>
    </div>
  );
};

export default WorkoutPlanExercisePage;
