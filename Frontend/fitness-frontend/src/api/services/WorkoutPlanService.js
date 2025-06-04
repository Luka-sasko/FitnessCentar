import baseApi from '../api/baseApi';

const WorkoutPlanService = {
  getAll: () => baseApi.get('/workoutplan'),
  getById: (id) => baseApi.get(`/workoutplan/${id}`),
  create: (data) => baseApi.post('/workoutplan', data),
  update: (id, data) => baseApi.put(`/workoutplan/${id}`, data),
  delete: (id) => baseApi.delete(`/workoutplan/${id}`),
};

export default WorkoutPlanService;
