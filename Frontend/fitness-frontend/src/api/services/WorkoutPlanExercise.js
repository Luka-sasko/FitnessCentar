import baseApi from '../api/baseApi';

const WorkoutPlanExerciseService = {
  getAll: () => baseApi.get('/workoutplanexercise'),
  getById: (id) => baseApi.get(`/workoutplanexercise/${id}`),
  create: (data) => baseApi.post('/workoutplanexercise', data),
  update: (id, data) => baseApi.put(`/workoutplanexercise/${id}`, data),
  delete: (id) => baseApi.delete(`/workoutplanexercise/${id}`),
};

export default WorkoutPlanExerciseService;
