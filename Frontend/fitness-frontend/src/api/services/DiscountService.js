import api from "../BaseApi";

const DiscountService = {
  getAll: (params) => api.get("/discount", { params }),
  getById: (id) => api.get("/discount", { params: { id } }),
  create: (data) => api.post("/discount", data),
  update: (id, data) => api.put("/discount?id=" + id, data),
  delete: (id) => api.delete("/discount?id=" + id),
};

export default DiscountService;
