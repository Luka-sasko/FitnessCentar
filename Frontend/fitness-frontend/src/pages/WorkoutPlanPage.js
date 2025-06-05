import React, { useEffect } from "react";
import { toJS } from "mobx";
import { workoutPlanStore } from "../stores/WorkOutPlanStore";

const WorkoutPlanPage = () => {
  useEffect(() => {
    const fetchData = async () => {
      await workoutPlanStore.fetchAll();
      console.log("Fetched WorkoutPlan list:", toJS(workoutPlanStore.workoutPlanList));
      console.log("PagedMeta:", toJS(workoutPlanStore.pagedMeta));

    };
    fetchData();
  }, []);

  return (
    <div>
      <h2>WorkoutPlan Page</h2>
    </div>
  );
};

export default WorkoutPlanPage;
