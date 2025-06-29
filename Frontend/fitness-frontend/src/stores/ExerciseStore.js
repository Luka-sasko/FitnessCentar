import { makeObservable, observable, action, runInAction } from "mobx";
import ExerciseService from "../api/services/ExerciseService";
import { BasePagedStore } from "./BasePagedStore";

class ExerciseStore extends BasePagedStore {
  selectedExercise = null;

  constructor() {
    super(ExerciseService.getAll);

    makeObservable(this, {
      selectedExercise: observable,
      fetchById: action,
      createExercise: action,
      updateExercise: action,
      deleteExercise: action,
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
