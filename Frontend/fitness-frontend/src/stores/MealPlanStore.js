import { makeObservable, observable, runInAction, action } from "mobx";
import { BasePagedStore } from "./BasePagedStore";
import MealPlanService from "../api/services/MealPlanService";

class MealPlanStore extends BasePagedStore {
  selectedMealPlan = null;
  dialogOpen = false;

  constructor() {
    super(MealPlanService.getAll);
    makeObservable(this, {
      selectedMealPlan: observable,
      dialogOpen: observable,
      setDialogOpen: action
    });
  }

  setDialogOpen(value) {
    this.dialogOpen = value;
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
