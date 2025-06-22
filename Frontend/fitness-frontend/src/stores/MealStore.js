import { makeAutoObservable, runInAction } from "mobx";
import MealService from "../api/services/MealService";

class MealStore {
  mealList = [];
  selectedMeal = null;
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
    const response = await MealService.getAll(params);
    runInAction(() => {
      this.mealList = response.data.Items;
      this.pagedMeta = {
        pageNumber: response.data.PageNumber,
        pageSize: response.data.PageSize,
        totalCount: response.data.TotalCount,
        totalPages: response.data.TotalPages
      };
    });
  }

  async fetchById(id) {
    const response = await MealService.getById(id);
    runInAction(() => {
      this.selectedMeal = response.data;
    });
  }
  async fetchByUser() {
    const response = await MealService.getByUser();
    runInAction(() => {
      this.mealList = response.data;
      this.pagedMeta = {
        pageNumber: response.data.PageNumber,
        pageSize: response.data.length,
        totalCount: response.data.length,
        totalPages: response.data.TotalPages
      };
    });
  }

  async createMeal(data) {
    await MealService.create(data);
    await this.fetchAll();
  }

  async updateMeal(id, data) {
    await MealService.update(id, data);
    await this.fetchAll();
  }

  async deleteMeal(id) {
    await MealService.delete(id);
    await this.fetchAll();
  }
}

export const mealStore = new MealStore();
