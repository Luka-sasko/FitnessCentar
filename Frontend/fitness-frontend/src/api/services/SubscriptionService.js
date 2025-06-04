import baseApi from '../api/baseApi';

const SubscriptionService = {
  getAll: () => baseApi.get('/subscription'),
  getById: (id) => baseApi.get(`/subscription/${id}`),
  create: (data) => baseApi.post('/subscription', data),
  update: (id, data) => baseApi.put(`/subscription/${id}`, data),
  delete: (id) => baseApi.delete(`/subscription/${id}`),
};

export default SubscriptionService;
