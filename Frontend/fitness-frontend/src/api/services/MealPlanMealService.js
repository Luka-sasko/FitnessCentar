import baseApi from '../api/baseApi';

const MealPlanMealService = {
  getAll: () => baseApi.get('/mealplanmeal'),
  getById: (id) => baseApi.get(`/mealplanmeal/${id}`),
  create: (data) => baseApi.post('/mealplanmeal', data),
  update: (id, data) => baseApi.put(`/mealplanmeal/${id}`),
  delete: (id) => baseApi.delete(`/mealplanmeal/${id}`),
};

export default MealPlanMealService;
