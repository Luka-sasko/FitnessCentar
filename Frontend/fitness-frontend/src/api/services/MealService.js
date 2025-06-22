import api from "../BaseApi";

const MealService = {
  getAll: (params) => api.get("/meal", { params }),
  getById: (id) => api.get("/meal/" + id),
  delete: (id) => api.delete("/meal/" + id),
  update: (id, data) => api.put("/meal/" + id, data),
  create: (data) => api.post("/meal", data),
  getByUser: () => api.get("/meal/by-user"),
};

export default MealService;
