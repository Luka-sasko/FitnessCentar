import { makeAutoObservable, runInAction } from "mobx";
import DiscountService from "../services/DiscountService";

class DiscountStore {
  discounts = [];
  selectedDiscount = null;
  loading = false;
  error = null;

  constructor() {
    makeAutoObservable(this);
  }

  async fetchAll() {
    this.loading = true;
    try {
      const response = await DiscountService.getAll();
      runInAction(() => {
        this.discounts = response.data;
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
      const response = await DiscountService.getById(id);
      runInAction(() => {
        this.selectedDiscount = response.data;
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
      await DiscountService.create(data);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }

  async update(id, data) {
    try {
      await DiscountService.update(id, data);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }

  async delete(id) {
    try {
      await DiscountService.delete(id);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }
}

export const discountStore = new DiscountStore();
