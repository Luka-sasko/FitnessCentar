import { makeAutoObservable, runInAction } from "mobx";
import WorkoutPlanExerciseService from "../services/WorkoutPlanExerciseService";

class WorkoutPlanExerciseStore {
  workoutplanexercises = [];
  selectedWorkoutPlanExercise = null;
  loading = false;
  error = null;

  constructor() {
    makeAutoObservable(this);
  }

  async fetchAll() {
    this.loading = true;
    try {
      const response = await WorkoutPlanExerciseService.getAll();
      runInAction(() => {
        this.workoutplanexercises = response.data;
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
      const response = await WorkoutPlanExerciseService.getById(id);
      runInAction(() => {
        this.selectedWorkoutPlanExercise = response.data;
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
      await WorkoutPlanExerciseService.create(data);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }

  async update(id, data) {
    try {
      await WorkoutPlanExerciseService.update(id, data);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }

  async delete(id) {
    try {
      await WorkoutPlanExerciseService.delete(id);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }
}

export const workoutPlanExerciseStore = new WorkoutPlanExerciseStore();
