import baseApi from '../api/baseApi';

const MealService = {
  getAll: () => baseApi.get('/meal'),
  getById: (id) => baseApi.get(`/meal/${id}`),
  create: (data) => baseApi.post('/meal', data),
  update: (id, data) => baseApi.put(`/meal/${id}`, data),
  delete: (id) => baseApi.delete(`/meal/${id}`),
};

export default MealService;
