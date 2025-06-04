import { makeAutoObservable, runInAction } from "mobx";
import MealPlanService from "../services/MealPlanService";

class MealPlanStore {
  mealplans = [];
  selectedMealPlan = null;
  loading = false;
  error = null;

  constructor() {
    makeAutoObservable(this);
  }

  async fetchAll() {
    this.loading = true;
    try {
      const response = await MealPlanService.getAll();
      runInAction(() => {
        this.mealplans = response.data;
        this.loading = false;
      });
    } catch (err) {
      runInAction(() => {
        this.error = err;
        this.loading = false;
      });
    }
  }

  async fetchById(id) {
    this.loading = true;
    try {
      const response = await MealPlanService.getById(id);
      runInAction(() => {
        this.selectedMealPlan = response.data;
        this.loading = false;
      });
    } catch (err) {
      runInAction(() => {
        this.error = err;
        this.loading = false;
      });
    }
  }

  async create(data) {
    try {
      await MealPlanService.create(data);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }

  async update(id, data) {
    try {
      await MealPlanService.update(id, data);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }

  async delete(id) {
    try {
      await MealPlanService.delete(id);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }
}

export const mealPlanStore = new MealPlanStore();
