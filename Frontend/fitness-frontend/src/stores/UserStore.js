import { makeAutoObservable, runInAction } from "mobx";
import axios from "axios";
import UserService from "../api/services/UserService";


class UserStore {
  users = [];
  selectedUser = null;
  currentUser = null;
  userRole = false;
  loading = false;
  error = null;

  constructor() {
    makeAutoObservable(this);
    this.currentUser = JSON.parse(localStorage.getItem("user")) || null;
  }


  async updatePassword(data) {
    try {
      const response = await UserService.resetPassword(data);
      runInAction(() => {
        this.error = "";
      });
      return response;
    } catch (err) {
      runInAction(() => {
        this.error = "Couldn't reset password, try again!";
      });
      throw err;
    }
  }

  async login(username, password) {
    try {
      const response = await axios.post(
        "https://localhost:44366/Login",
        new URLSearchParams({
          grant_type: "password",
          username,
          password
        }),
        {
          headers: { "Content-Type": "application/x-www-form-urlencoded" }
        }
      );

      runInAction(() => {
        const token = response.data.access_token;
        this.currentUser = { username, token };
        localStorage.setItem("user", JSON.stringify(this.currentUser));
        localStorage.setItem("token", token);
        this.error = null;
      });
    } catch (err) {
      runInAction(() => {
        this.error = "Invalid username or password.";
      });
      throw err;
    }
  }
  async fetchUserId() {
    try {
      const token = localStorage.getItem("token");
      const response = await axios.get("https://localhost:44366/api/GetUserId", {
        headers: {
          Authorization: `Bearer ${token}`
        }
      });
      return response.data;
    } catch (error) {
      console.error("Failed to fetch user ID:", error);
      throw error;
    }
  }


  async register(data) {
    try {
      const response = await UserService.register(data);
      return response.data;
    } catch (err) {
      runInAction(() => {
        this.error = "Registration failed.";
      });
      throw err;
    }
  }

  logout() {
    this.currentUser = null;
    localStorage.removeItem("user");
    localStorage.removeItem("token");
  }

  async fetch() {
    const response = await UserService.get();
    this.userRole = response.data.Role;
    return response;
  }

  async update(data) {
    try {
      await UserService.update(data);
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

  get isLoggedIn() {
    return !!this.currentUser;
  }

  get isAdmin(){
    return this.userRole === "Admin" ? true : false;
  }

}

export const userStore = new UserStore();
