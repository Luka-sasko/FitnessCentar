
import api from "../BaseApi";

const WorkoutPlanExerciseService = {
  getAll: (params) => api.get("/workoutplanexercise", { params }),
  getById: (id) => api.get("/workoutplanexercise/" + id),
  create: (data) => api.post("/workoutplanexercise", data),
  update: (id, data) => api.put("/workoutplanexercise/" + id, data),
  delete: (id) => api.delete("/workoutplanexercise/" + id),
};

export default WorkoutPlanExerciseService;
