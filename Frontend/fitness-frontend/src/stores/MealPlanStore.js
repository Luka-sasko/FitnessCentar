import { makeAutoObservable, runInAction } from "mobx";
import MealPlanService from "../api/services/MealPlanService";

class MealPlanStore {
  mealPlanList = [];
  selectedMealPlan = null;
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
    const response = await MealPlanService.getAll(params);
    runInAction(() => {
      this.mealPlanList = response.data.Items;
      this.pagedMeta = {
        pageNumber: response.data.PageNumber,
        pageSize: response.data.PageSize,
        totalCount: response.data.TotalCount,
        totalPages: response.data.TotalPages
      };
    });
  }

  async fetchById(id) {
    const response = await MealPlanService.getById(id);
    runInAction(() => {
      this.selectedMealPlan = response.data;
    });
  }

  async createMealPlan(data) {
    await MealPlanService.create(data);
    await this.fetchAll();
  }

  async updateMealPlan(id, data) {
    await MealPlanService.update(id, data);
    await this.fetchAll();
  }

  async deleteMealPlan(id) {
    await MealPlanService.delete(id);
    await this.fetchAll();
  }
}

export const mealPlanStore = new MealPlanStore();
