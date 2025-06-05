import { makeAutoObservable, runInAction } from "mobx";
import MealPlanMealService from "../api/services/MealPlanMealService";

class MealPlanMealStore {
  mealPlanMealList = [];
  selectedMealPlanMeal = null;
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
    const response = await MealPlanMealService.getAll(params);
    runInAction(() => {
      this.mealPlanMealList = response.data.Items;
      this.pagedMeta = {
        pageNumber: response.data.PageNumber,
        pageSize: response.data.PageSize,
        totalCount: response.data.TotalCount,
        totalPages: response.data.TotalPages
      };
    });
  }

  async fetchById(id) {
    const response = await MealPlanMealService.getById(id);
    runInAction(() => {
      this.selectedMealPlanMeal = response.data;
    });
  }

  async createMealPlanMeal(data) {
    await MealPlanMealService.create(data);
    await this.fetchAll();
  }

  async updateMealPlanMeal(id, data) {
    await MealPlanMealService.update(id, data);
    await this.fetchAll();
  }

  async deleteMealPlanMeal(id) {
    await MealPlanMealService.delete(id);
    await this.fetchAll();
  }
}

export const mealPlanMealStore = new MealPlanMealStore();
