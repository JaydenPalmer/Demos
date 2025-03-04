const _apiUrl = "/api/instrument";

export const getAllInstruments = () => {
  return fetch(_apiUrl).then((res) => res.json());
};
