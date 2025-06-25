import { makeObservable, observable, runInAction, action } from "mobx";
import { BasePagedStore } from "./BasePagedStore";
import FoodService from "../api/services/FoodService";

class FoodStore {
  items = [];
  dialogOpen = false;

  constructor() {
    makeObservable(this, {
      items: observable,
      dialogOpen: observable,
      fetchAll: action,
      setDialogOpen: action,
      createFood: action,
    });
  }

  async fetchAll(params = {}) {
    try {
      const response = await FoodService.getAll(params);
      runInAction(() => {
        this.items = response.data.Items || [];
      });
    } catch (error) {
      runInAction(() => {
        this.items = [];
      });
    }
  }

  setDialogOpen(val) {
    this.dialogOpen = val;
  }

  async createFood(data) {
    await FoodService.create(data);
    // Optionally: refresh list after creation
  }
}

export const foodStore = new FoodStore();
