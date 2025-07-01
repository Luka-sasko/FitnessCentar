import { observable, action, makeObservable, runInAction } from "mobx";
import SubscriptionService from "../api/services/SubscriptionService";

class SubscriptionStore {
  subscriptionList = [];
  pagedMeta = {
    pageNumber: 1,
    pageSize: 10,
    totalCount: 0,
    totalPages: 0
  };

  constructor() {
    makeObservable(this, {
      subscriptionList: observable,
      pagedMeta: observable,
      fetchAll: action
    });
  }

  async fetchAll(params = {}) {
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
}

export const subscriptionStore = new SubscriptionStore();
