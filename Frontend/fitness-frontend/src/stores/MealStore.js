import { makeObservable, observable, action, runInAction } from "mobx";
import { BasePagedStore } from "./BasePagedStore";
import MealService from "../api/services/MealService";

class MealStore extends BasePagedStore {
  selectedMeal = null;
  dialogOpen = false;

  constructor() {
    super(MealService.getByUser); 
    makeObservable(this, {
      selectedMeal: observable,
      dialogOpen: observable,
      setDialogOpen: action,
    });
  }

  setDialogOpen(value) {
    this.dialogOpen = value;
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
      this.items = response.data.Items;
      this.pagedMeta = {
        pageNumber: response.data.PageNumber ?? 1,
        pageSize: response.data.length ?? 10,
        totalCount: response.data.length ?? 0,
        totalPages: response.data.TotalPages ?? 1,
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
