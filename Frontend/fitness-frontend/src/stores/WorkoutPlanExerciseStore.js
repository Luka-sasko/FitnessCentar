import { makeAutoObservable, runInAction } from "mobx";
import WorkoutPlanExerciseService from "../api/services/WorkoutPlanExerciseService";

class WorkoutPlanExerciseStore {
  workoutPlanExerciseList = [];
  selectedWorkoutPlanExercise = null;
  pagedMeta = {
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
    totalPages: 0
  };

  constructor() {
    makeAutoObservable(this);
  }

  async fetchAll(params) {
    const response = await WorkoutPlanExerciseService.getAll(params);
    runInAction(() => {
      this.workoutPlanExerciseList = response.data.Items;
      this.pagedMeta = {
        pageNumber: response.data.PageNumber,
        pageSize: response.data.PageSize,
        totalCount: response.data.TotalCount,
        totalPages: response.data.TotalPages
      };
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
    await this.fetchAll();
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
