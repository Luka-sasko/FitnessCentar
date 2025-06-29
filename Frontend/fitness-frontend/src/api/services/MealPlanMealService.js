import api from "../BaseApi";

const MealPlanMealService = {
  getAll: (params) => api.getAll("/mealplanmeal",params ),
  getById: (id) => api.get("/mealplanmeal/" + id),
  create: (data) => api.post("/mealplanmeal", data),
  update: (id, data) => api.put("/mealplanmeal/" + id, data),
  delete: (id) => api.delete("/mealplanmeal/" + id),
};

export default MealPlanMealService;
