const baseUrl = "/api/album";

export const createAlbumWithTracks = (albumWithTracks) => {
  return fetch(`${baseUrl}`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(albumWithTracks),
  }).then((res) => {
    if (!res.ok) {
      return res.text().then((text) => {
        throw new Error(text);
      });
    }
    return res.json();
  });
};

export const getAlbumById = async (albumId) => {
  try {
    const params = new URLSearchParams();
    params.append("albumId", albumId);

    const url = `/api/track?${params.toString()}`;
    const response = await fetch(url);

    if (!response.ok) {
      throw new Error(`HTTP error! Status ${response.status}`);
    }

    return response.json();
  } catch (error) {
    console.error("Error fetching album:", error);
    return null;
  }
};

export const deleteAlbum = (id) => {
  return fetch(`/api/album/${id}`, {
    method: "DELETE",
    credentials: "include", // Ensure cookies are sent
  }).then((res) => {
    if (!res.ok) {
      return res.text().then((errorText) => {
        console.error("Delete album server error:", errorText);
        throw new Error(errorText || "Failed to delete album");
      });
    }
    return;
  });
};
