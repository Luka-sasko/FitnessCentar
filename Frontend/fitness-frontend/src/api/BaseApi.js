import axios from 'axios';

const BASE_URL = 'https://localhost:44366/api';

const axiosInstance = axios.create({
  baseURL: BASE_URL,
});

axiosInstance.interceptors.request.use((config) => {
  const userJson = localStorage.getItem("user");
  let token;

  try {
    const user = JSON.parse(userJson);
    token = user?.token;
  } catch (err) {
    console.warn("❌ Neispravan JSON u localStorage.user");
  }

  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }

  return config;
});

const baseApi = {
  get: (url) => axiosInstance.get(url),
  post: (url, data) => axiosInstance.post(url, data),
  put: (url, data) => axiosInstance.put(url, data),
  delete: (url) => axiosInstance.delete(url),
};

export default baseApi;
