import { makeAutoObservable, runInAction } from "mobx";
import DiscountService from "../api/services/DiscountService";



class DiscountStore {
  discountList = [];
  selectedDiscount = null;
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
    const response = await DiscountService.getAll(params);
    runInAction(() => {
      this.discountList = response.data.Items;
      this.pagedMeta = {
        pageNumber: response.data.PageNumber,
        pageSize: response.data.PageSize,
        totalCount: response.data.TotalCount,
        totalPages: response.data.TotalPages
      };
    });
  }


  async fetchById(id) {
    const response = await DiscountService.getById(id);
    runInAction(() => {
      this.selectedDiscount = response.data;
    });
  }

  async createDiscount(data) {
    await DiscountService.create(data);
    await this.fetchAll();
  }

  async updateDiscount(id, data) {
    await DiscountService.update(id, data);
    await this.fetchAll();
  }

  async deleteDiscount(id) {
    await DiscountService.delete(id);
    await this.fetchAll();
  }
}

export const discountStore = new DiscountStore();
