import api from "../BaseApi";

const FoodService = {
  getAll: (params) => api.getAll('/food', params),
  getById: (id) => api.get(`/food/${id}`),
  create: (data) => api.post('/food', data),
  update: (id, data) => api.put(`/food/${id}`, data),
  delete: (id) => api.delete(`/food/${id}`)
};

export default FoodService;
