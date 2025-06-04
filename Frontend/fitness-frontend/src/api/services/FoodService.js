import baseApi from '../api/baseApi';

const FoodService = {
  getAll: () => baseApi.get('/food'),
  getById: (id) => baseApi.get(`/food/${id}`),
  create: (data) => baseApi.post('/food', data),
  update: (id, data) => baseApi.put(`/food/${id}`, data),
  delete: (id) => baseApi.delete(`/food/${id}`),
};

export default FoodService;
