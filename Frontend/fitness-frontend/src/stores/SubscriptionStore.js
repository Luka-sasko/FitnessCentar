import { makeAutoObservable, runInAction } from "mobx";
import SubscriptionService from "../services/SubscriptionService";

class SubscriptionStore {
  subscriptions = [];
  selectedSubscription = null;
  loading = false;
  error = null;

  constructor() {
    makeAutoObservable(this);
  }

  async fetchAll() {
    this.loading = true;
    try {
      const response = await SubscriptionService.getAll();
      runInAction(() => {
        this.subscriptions = response.data;
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
      const response = await SubscriptionService.getById(id);
      runInAction(() => {
        this.selectedSubscription = response.data;
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
      await SubscriptionService.create(data);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }

  async update(id, data) {
    try {
      await SubscriptionService.update(id, data);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }

  async delete(id) {
    try {
      await SubscriptionService.delete(id);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }
}

export const subscriptionStore = new SubscriptionStore();
