import { makeObservable, observable, runInAction, action } from "mobx";
import { BasePagedStore } from "./BasePagedStore";
import FoodService from "../api/services/FoodService";

class FoodStore extends BasePagedStore {
  selectedFood = null;
  dialogOpen = false;
  constructor() {
    super(FoodService.getAll);
    makeObservable(this, {
      selectedFood: observable,
      dialogOpen: observable,
      setDialogOpen: action
    });
  }
  setDialogOpen(value) {
    this.dialogOpen = value;
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
