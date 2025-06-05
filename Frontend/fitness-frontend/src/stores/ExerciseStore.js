import { makeAutoObservable, runInAction } from "mobx";
import ExerciseService from "../api/services/ExerciseService";

class ExerciseStore {
  exerciseList = [];
  selectedExercise = null;
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
    const response = await ExerciseService.getAll(params);
    runInAction(() => {
      this.exerciseList = response.data.Items;
      this.pagedMeta = {
        pageNumber: response.data.PageNumber,
        pageSize: response.data.PageSize,
        totalCount: response.data.TotalCount,
        totalPages: response.data.TotalPages
      };
    });
  }

  async fetchById(id) {
    const response = await ExerciseService.getById(id);
    runInAction(() => {
      this.selectedExercise = response.data;
    });
  }

  async createExercise(data) {
    await ExerciseService.create(data);
    await this.fetchAll();
  }

  async updateExercise(id, data) {
    await ExerciseService.update(id, data);
    await this.fetchAll();
  }

  async deleteExercise(id) {
    await ExerciseService.delete(id);
    await this.fetchAll();
  }
}

export const exerciseStore = new ExerciseStore();
