import baseApi from '../api/baseApi';

const DiscountService = {
  getAll: () => baseApi.get('/discount'),
  getById: (id) => baseApi.get(`/discount/${id}`),
  create: (data) => baseApi.post('/discount', data),
  update: (id, data) => baseApi.put(`/discount/${id}`, data),
  delete: (id) => baseApi.delete(`/discount/${id}`),
};

export default DiscountService;
