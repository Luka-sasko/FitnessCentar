import { observable, action, makeObservable, runInAction } from "mobx";
import WorkoutPlanExerciseService from "../api/services/WorkoutPlanExerciseService";
import { BasePagedStore } from "./BasePagedStore";

class WorkoutPlanExerciseStore extends BasePagedStore {
  workoutPlanExerciseList = [];
  selectedWorkoutPlanExercise = null;

  constructor() {
    super(WorkoutPlanExerciseService.getAll);
    makeObservable(this, {
      workoutPlanExerciseList: observable,
      selectedWorkoutPlanExercise: observable,
      fetchById: action,
      createWorkoutPlanExercise: action,
      updateWorkoutPlanExercise: action,
      deleteWorkoutPlanExercise: action
    });
  }


  async fetchById(id) {
    const response = await WorkoutPlanExerciseService.getById(id);
    runInAction(() => {
      this.selectedWorkoutPlanExercise = response.data;
    });
  }

  async createWorkoutPlanExercise(data) {
    await WorkoutPlanExerciseService.create(data);
  }

  async updateWorkoutPlanExercise(id, data) {
    await WorkoutPlanExerciseService.update(id, data);
    await this.fetchAll();
  }

  async deleteWorkoutPlanExercise(id) {
    await WorkoutPlanExerciseService.delete(id);
    await this.fetchAll();
  }
}

export const workoutPlanExerciseStore = new WorkoutPlanExerciseStore();
