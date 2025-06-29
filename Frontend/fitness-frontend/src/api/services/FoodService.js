import baseApi from "../BaseApi";

const FoodService = {
  getAll: (params) => baseApi.getAll("/food", params ),
  getById: (id) => baseApi.get("/food/" + id),
  create: (data) => baseApi.post("/food", data),
  update: (id, data) => baseApi.put("/food/" + id, data),
  delete: (id) => baseApi.delete("/food/" + id),
};

export default FoodService;
