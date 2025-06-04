import baseApi from '../api/baseApi';

const ExerciseService = {
  getAll: () => baseApi.get('/exercise'),
  getById: (id) => baseApi.get(`/exercise/${id}`),
  create: (data) => baseApi.post('/exercise', data),
  update: (id, data) => baseApi.put(`/exercise/${id}`, data),
  delete: (id) => baseApi.delete(`/exercise/${id}`),
};

export default ExerciseService;
