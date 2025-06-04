import { makeAutoObservable, runInAction } from "mobx";
import MealPlanMealService from "../services/MealPlanMealService";

class MealPlanMealStore {
  mealplanmeals = [];
  selectedMealPlanMeal = null;
  loading = false;
  error = null;

  constructor() {
    makeAutoObservable(this);
  }

  async fetchAll() {
    this.loading = true;
    try {
      const response = await MealPlanMealService.getAll();
      runInAction(() => {
        this.mealplanmeals = response.data;
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
      const response = await MealPlanMealService.getById(id);
      runInAction(() => {
        this.selectedMealPlanMeal = response.data;
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
      await MealPlanMealService.create(data);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }

  async update(id, data) {
    try {
      await MealPlanMealService.update(id, data);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }

  async delete(id) {
    try {
      await MealPlanMealService.delete(id);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }
}

export const mealPlanMealStore = new MealPlanMealStore();
