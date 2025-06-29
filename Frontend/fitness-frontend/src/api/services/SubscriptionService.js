import api from "../BaseApi";

const SubscriptionService = {
  getAll: (params) => api.get("/subscription",  params ),
  getById: (id) => api.get("/subscription/" + id),
  create: (data) => api.post("/subscription", data),
  update: (id, data) => api.put("/subscription/" + id, data),
  delete: (id) => api.delete("/subscription/" + id),
};

export default SubscriptionService;
