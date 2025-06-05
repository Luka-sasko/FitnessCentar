import api from "../BaseApi";

const WorkoutPlanService = {
  getAll: (params) => api.get("/workoutplan", { params }),
  getById: (id) => api.get("/workoutplan/" + id),
  create: (data) => api.post("/workoutplan", data),
  update: (id, data) => api.put("/workoutplan/" + id, data),
  delete: (id) => api.delete("/workoutplan/" + id),
};

export default WorkoutPlanService;
