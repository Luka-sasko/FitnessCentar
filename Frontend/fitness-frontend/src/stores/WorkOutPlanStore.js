import { makeObservable, observable, action, runInAction } from "mobx";
import WorkoutPlanService from "../api/services/WorkoutPlanService";
import { BasePagedStore } from "./BasePagedStore";

class WorkoutPlanStore extends BasePagedStore {
  selectedWorkoutPlan = null;

  constructor() {
    super(WorkoutPlanService.getAll);
    makeObservable(this, {
      selectedWorkoutPlan: observable,
      fetchById: action,
      createWorkoutPlan: action,
      updateWorkoutPlan: action,
      deleteWorkoutPlan: action,
    });
  }

  async fetchById(id) {
    const response = await WorkoutPlanService.getById(id);
    runInAction(() => {
      this.selectedWorkoutPlan = response.data;
    });
  }

  async createWorkoutPlan(data) {
    await WorkoutPlanService.create(data);
    await this.fetchAll();
  }

  async updateWorkoutPlan(id, data) {
    await WorkoutPlanService.update(id, data);
    await this.fetchAll();
  }

  async deleteWorkoutPlan(id) {
    await WorkoutPlanService.delete(id);
    await this.fetchAll();
  }
}

export const workoutPlanStore = new WorkoutPlanStore();
