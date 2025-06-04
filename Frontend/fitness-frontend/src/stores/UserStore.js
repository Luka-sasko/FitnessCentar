import { makeAutoObservable, runInAction } from "mobx";
import UserService from "../services/UserService";

class UserStore {
  users = [];
  selectedUser = null;
  currentUser = null;
  loading = false;
  error = null;

  constructor() {
    makeAutoObservable(this);
    this.currentUser = JSON.parse(localStorage.getItem("user")) || null;
  }

  async login(data) {
    try {
      const response = await UserService.login(data);
      runInAction(() => {
        this.currentUser = response.data;
        localStorage.setItem("user", JSON.stringify(response.data));
        localStorage.setItem("token", response.data.token);
      });
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }

  logout() {
    this.currentUser = null;
    localStorage.removeItem("user");
    localStorage.removeItem("token");
  }

  async fetchAll() {
    this.loading = true;
    try {
      const response = await UserService.getAll();
      runInAction(() => {
        this.users = response.data;
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
      const response = await UserService.getById(id);
      runInAction(() => {
        this.selectedUser = response.data;
        this.loading = false;
      });
    } catch (err) {
      runInAction(() => {
        this.error = err;
        this.loading = false;
      });
    }
  }

  async update(id, data) {
    try {
      await UserService.update(id, data);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }

  async delete(id) {
    try {
      await UserService.delete(id);
      this.fetchAll();
    } catch (err) {
      runInAction(() => {
        this.error = err;
      });
    }
  }
}

export const userStore = new UserStore();
