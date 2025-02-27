const _apiUrl = "/api/track";

export const getTracksByUserId = async (userId) => {
  try {
    const params = new URLSearchParams();

    if (userId !== null && userId > 0) {
      params.append("userId", userId);
    }

    const url = params.toString() ? `${_apiUrl}?${params.toString()}` : _apiUrl;

    const response = await fetch(url);

    if (!response.ok) {
      throw new Error(`HTTP error! Status ${response.status}`);
    }

    return response.json();
  } catch (error) {
    console.error("Error fetching tracks by user:", error);
    return null;
  }
};
