import { makeAutoObservable, runInAction } from "mobx";
import SubscriptionService from "../api/services/SubscriptionService";

class SubscriptionStore {
  subscriptionList = [];
  selectedSubscription = null;
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
    const response = await SubscriptionService.getAll(params);
    runInAction(() => {
      this.subscriptionList = response.data.Items;
      this.pagedMeta = {
        pageNumber: response.data.PageNumber,
        pageSize: response.data.PageSize,
        totalCount: response.data.TotalCount,
        totalPages: response.data.TotalPages
      };
    });
  }

  async fetchById(id) {
    const response = await SubscriptionService.getById(id);
    runInAction(() => {
      this.selectedSubscription = response.data;
    });
  }

  async createSubscription(data) {
    await SubscriptionService.create(data);
    await this.fetchAll();
  }

  async updateSubscription(id, data) {
    await SubscriptionService.update(id, data);
    await this.fetchAll();
  }

  async deleteSubscription(id) {
    await SubscriptionService.delete(id);
    await this.fetchAll();
  }
}

export const subscriptionStore = new SubscriptionStore();
