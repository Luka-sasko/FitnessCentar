import { makeAutoObservable, runInAction } from "mobx";
import FoodService from "../api/services/FoodService";

class FoodStore {
  foodList = [];
  selectedFood = null;
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
    const response = await FoodService.getAll(params);
    runInAction(() => {
      this.foodList = response.data.Items;
      this.pagedMeta = {
        pageNumber: response.data.PageNumber,
        pageSize: response.data.PageSize,
        totalCount: response.data.TotalCount,
        totalPages: response.data.TotalPages
      };
    });
  }

  async fetchById(id) {
    const response = await FoodService.getById(id);
    runInAction(() => {
      this.selectedFood = response.data;
    });
  }

  async createFood(data) {
    await FoodService.create(data);
    await this.fetchAll();
  }

  async updateFood(id, data) {
    await FoodService.update(id, data);
    await this.fetchAll();
  }

  async deleteFood(id) {
    await FoodService.delete(id);
    await this.fetchAll();
  }
}

export const foodStore = new FoodStore();
