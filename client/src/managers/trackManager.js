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

export const getTrackByTrackId = async (trackId) => {
  try {
    const params = new URLSearchParams();

    if (trackId !== null && trackId > 0) {
      params.append("trackId", trackId);
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

//create the track

export const createTrack = async (trackData) => {
  const response = await fetch(_apiUrl, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(trackData),
  });

  if (!response.ok) {
    const errorData = await response.text();
    throw new Error(`Error creating track: ${errorData}`);
  }

  return await response.json();
};

//delete the track

export const deleteTrack = async (trackId) => {
  const response = await fetch(`${_apiUrl}/${trackId}`, {
    method: "DELETE",
  });
  if (!response.ok) {
    throw new Error(`HTTP Error! Status ${response.status}`);
  }
};

//edit the track

export const updateTrack = async (trackData) => {
  try {
    console.log("Sending update with data:", trackData); // Log what you're sending

    const response = await fetch(`/api/Track/${trackData.id}`, {
      method: "PUT",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(trackData),
    });

    if (!response.ok) {
      const errorText = await response.text();
      console.error("Error response:", errorText); // Log the error response
      throw new Error(
        `Failed to update track (${response.status}): ${errorText}`
      );
    }

    return await getTrackByTrackId(trackData.id);
  } catch (error) {
    console.error("Error in updateTrack:", error);
    throw error;
  }
};
