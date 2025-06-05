import api from "../BaseApi";

const ExerciseService = {
  getAll: (params) => api.get("/exercise", { params }),
  getById: (id) => api.get("/exercise/" + id),
  create: (data) => api.post("/exercise", data),
  update: (id, data) => api.put("/exercise/" + id, data),
  delete: (id) => api.delete("/exercise/" + id),
};

export default ExerciseService;
