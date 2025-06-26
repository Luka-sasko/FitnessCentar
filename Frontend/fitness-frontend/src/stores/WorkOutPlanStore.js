import { makeAutoObservable, runInAction } from "mobx";
import WorkoutPlanService from "../api/services/WorkoutPlanService";

class WorkoutPlanStore {
  workoutPlanList = [];
  get items() {
    return this.workoutPlanList;
  }
  selectedWorkoutPlan = null;
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
    const response = await WorkoutPlanService.getAll(params);
    runInAction(() => {
      this.workoutPlanList = response.data.Items;
      this.pagedMeta = {
        pageNumber: response.data.PageNumber,
        pageSize: response.data.PageSize,
        totalCount: response.data.TotalCount,
        totalPages: response.data.TotalPages
      };
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
