import { makeAutoObservable, runInAction } from "mobx";
import WorkoutPlanService from "../services/WorkoutPlanService";

class WorkoutPlanStore {
  workoutplans = [];
  selectedWorkoutPlan = null;
  loading = false;
  error = null;

  constructor() {
    makeAutoObservable(this);
  }

  async fetchAll() {
    this.loading = true;
    try {
      const response = await WorkoutPlanService.getAll();
      runInAction(() => {
        this.workoutplans = response.data;
        this.loading = false;
      });
    } catch (err) {
      runInAction(() => {
        this.error = err;
        this.loading = false;
      });
    }
  }

  async fetchById(id) {
    this.loading = true;
    try {
      const response = await WorkoutPlanService.getById(id);
      runInAction(() => {
        this.selectedWorkoutPlan = response.data;
        this.loading = false;
      });
    } catch (err) {
      runInAction(() => {
        this.error = err;
        this.loading = false;
      });
    }
  }

  async create(data) {
    try {
      await WorkoutPlanService.create(data);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }

  async update(id, data) {
    try {
      await WorkoutPlanService.update(id, data);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }

  async delete(id) {
    try {
      await WorkoutPlanService.delete(id);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }
}

export const workoutPlanStore = new WorkoutPlanStore();
