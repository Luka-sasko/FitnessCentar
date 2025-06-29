import { makeObservable, observable, action, runInAction } from "mobx";
import DiscountService from "../api/services/DiscountService";
import { BasePagedStore } from "./BasePagedStore";

class DiscountStore extends BasePagedStore {
  selectedDiscount = null;

  constructor() {
    super(DiscountService.getAll);

    makeObservable(this, {
      selectedDiscount: observable,
      fetchById: action,
      createDiscount: action,
      updateDiscount: action,
      deleteDiscount: action,
    });
  }


  async fetchById(id) {
    const response = await DiscountService.getById(id);
    runInAction(() => {
      this.selectedDiscount = response.data;
    });
  }
  async fetchAll() {
    try {
      const params = {
        pageNumber: this.currentPage,
        pageSize: this.itemsPerPage,
        sortBy: this.sortBy,
        sortOrder: this.sortOrder,
      };

      const response = await this.fetchMethod(params);
      console.log(response.data);

      runInAction(() => {
        this.items = response.data.Items.map((item) => ({
          ...item,
          DateStart: item.StartDate
            ? new Date(item.StartDate).toLocaleString("hr-HR", {
              day: "2-digit",
              month: "2-digit",
              year: "numeric",
              hour: "2-digit",
              minute: "2-digit",
            })
            : "",
          DateEnd: item.EndDate
            ? new Date(item.EndDate).toLocaleString("hr-HR", {
              day: "2-digit",
              month: "2-digit",
              year: "numeric",
              hour: "2-digit",
              minute: "2-digit",
            })
            : "",
          isActiveDisplay:
            item.IsActive === true
              ? "✔️"
              : item.IsActive === false
                ? "❌"
                : "",
        }));

        this.totalCount = response.data.TotalCount;
        this.currentPage = response.data.PageNumber;
        this.itemsPerPage = response.data.PageSize;
      });
    } catch (err) {
      console.error("❌ Fetch error:", err);
      runInAction(() => {
        this.items = [];
        this.totalCount = 0;
      });
    }
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
