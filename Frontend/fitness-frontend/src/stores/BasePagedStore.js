import { makeObservable, observable, computed, action, runInAction } from "mobx";

export class BasePagedStore {
  items = [];
  totalCount = 0;
  currentPage = 1;
  itemsPerPage = 10;
  sortBy = "Name";
  sortOrder = "ASC";

  constructor(fetchMethod) {
    this.fetchMethod = fetchMethod;

    makeObservable(this, {
      items: observable,
      totalCount: observable,
      currentPage: observable,
      itemsPerPage: observable,
      sortBy: observable,
      sortOrder: observable,

      fetchAll: action,
      setCurrentPage: action,
      setItemsPerPage: action,
      setSort: action,

      totalPages: computed,
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
      runInAction(() => {
        this.items = response.data.Items;
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

  async setCurrentPage(page) {
    console.log("⬅️ setCurrentPage called with:", page); 
    this.currentPage = page;
    await this.fetchAll();
  }


  async setItemsPerPage(count) {
    this.itemsPerPage = count;
    this.currentPage = 1;
    await this.fetchAll();
  }

  columnSortMap = {
  DateStart: "StartDate",
  DateEnd: "EndDate",
  Active: "IsActive",

};

  setSort(col, order) {
  const apiCol = this.columnSortMap?.[col] ?? col; 
  this.sortBy = apiCol;
  this.sortOrder = order;
  this.fetchAll();
}



  get totalPages() {
    return Math.ceil(this.totalCount / this.itemsPerPage) || 1;
  }
}
