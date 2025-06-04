import baseApi from '../api/baseApi';

const UserService = {
  login: (data) => baseApi.post('/user/login', data),
  register: (data) => baseApi.post('/user/register', data),
  getAll: () => baseApi.get('/user'),
  getById: (id) => baseApi.get(`/user/${id}`),
  update: (id, data) => baseApi.put(`/user/${id}`, data),
  delete: (id) => baseApi.delete(`/user/${id}`),
};

export default UserService;
