const _apiUrl = "/api/instrumentCategory";

export const getAllInstrumentCategories = () => {
  return fetch(_apiUrl).then((res) => res.json());
};
