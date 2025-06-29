import api from "../BaseApi";

const MealPlanService = {
  getAll: (params) => api.get("/mealplan",  params ),
  getById: (id) => api.get("/mealplan/" + id),
  create: (data) => api.post("/mealplan", data),
  update: (id, data) => api.put("/mealplan/" + id, data),
  delete: (id) => api.delete("/mealplan/" + id),
};

export default MealPlanService;
