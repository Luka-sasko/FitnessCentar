import baseApi from '../api/baseApi';

const MealPlanService = {
  getAll: () => baseApi.get('/mealplan'),
  getById: (id) => baseApi.get(`/mealplan/${id}`),
  create: (data) => baseApi.post('/mealplan', data),
  update: (id, data) => baseApi.put(`/mealplan/${id}`, data),
  delete: (id) => baseApi.delete(`/mealplan/${id}`),
};

export default MealPlanService;
