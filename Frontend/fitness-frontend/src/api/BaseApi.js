import axios from 'axios';

const BASE_URL = 'https://localhost:44366/api';

const axiosInstance = axios.create({
  baseURL: BASE_URL,
});

axiosInstance.interceptors.request.use((config) => {
  try {
    const token = localStorage.getItem("token");
    if (token) {
      config.headers.Authorization = `Bearer ${token}`;
    }
  } catch (err) {
    console.warn("❌ Invalid token in localStorage");
  }
  return config;
});

const baseApi = {
  getAll: async (url,  params ) => {
    try {
      const token = localStorage.getItem("token");
      console.log(params)
      const queryString = new URLSearchParams(params).toString();
      const fullUrl = `${BASE_URL}${url}?${queryString}`;
      console.log(`📡 GET ${fullUrl}`);
      const response = await axios.get(fullUrl, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      return response;
    } catch (err) {
      console.error("❌ API getAll error:", err);
      throw err;
    }
  },
  
  get: (url, config) => axiosInstance.get(url, config),
  post: (url, data) => axiosInstance.post(url, data),
  put: (url, data) => axiosInstance.put(url, data),
  delete: (url) => axiosInstance.delete(url),
  updatePassword: (url, data) => {
    return axios.put(`https://localhost:44366/${url}`, data, {
      headers: {
        Authorization: `Bearer ${localStorage.getItem("token")}`
      }
    });
  }
};

export default baseApi;
