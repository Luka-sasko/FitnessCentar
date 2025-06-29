import baseApi from '../BaseApi';

const UserService = {
  register: (data) => baseApi.post('/user/', data),
  get: () => baseApi.get('/user'),
  update: (data) => baseApi.put(`/user`, data),
  delete: (id) => baseApi.delete(`/user/${id}`),
  resetPassword: (data) => baseApi.updatePassword('/updatePassword',  data)
};

export default UserService;
