const _apiUrl = "/api/auth";

export const login = (email, password) => {
  return fetch(_apiUrl + "/login", {
    method: "POST",
    credentials: "same-origin",
    headers: {
      Authorization: `Basic ${btoa(`${email}:${password}`)}`,
    },
  }).then((res) => {
    if (res.status !== 200) {
      return Promise.resolve(null);
    } else {
      return tryGetLoggedInUser();
    }
  });
};

export const logout = () => {
  return fetch(_apiUrl + "/logout");
};

export const tryGetLoggedInUser = () => {
  return fetch(_apiUrl + "/me").then((res) => {
    return res.status === 401 ? Promise.resolve(null) : res.json();
  });
};

export const register = (userProfile) => {
  // Log what we're working with
  console.log("Before encoding:", userProfile);

  // Use correct capitalization
  userProfile.Password = btoa(userProfile.Password);

  // Log what we're sending
  console.log("After encoding:", userProfile);

  return fetch(_apiUrl + "/register", {
    credentials: "same-origin",
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(userProfile),
  }).then((response) => {
    if (!response.ok) {
      // Try to get and log error details
      return response.text().then((text) => {
        console.error("Registration failed:", text);
        throw new Error(`Registration failed: ${text}`);
      });
    }
    return tryGetLoggedInUser();
  });
};
