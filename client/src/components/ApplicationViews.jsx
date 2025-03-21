import { Route, Routes } from "react-router-dom";
import { AuthorizedRoute } from "./auth/AuthorizedRoute";
import Login from "./auth/Login";
import Register from "./auth/Register";
import MusicTitleCard from "./tracks&albums/MusicTitleCard";
import CreateTrack from "./tracks&albums/CreateTrack";
import TrackDetailsView from "./tracks&albums/TrackDetailsView";
import CreateAlbum from "./tracks&albums/CreateAlbum";
import AlbumDetails from "./tracks&albums/AlbumDetailsView";

export default function ApplicationViews({ loggedInUser, setLoggedInUser }) {
  return (
    <Routes>
      <Route path="/">
        <Route
          index
          element={
            <AuthorizedRoute loggedInUser={loggedInUser}>
              {<MusicTitleCard loggedInUser={loggedInUser} />}
            </AuthorizedRoute>
          }
        />
        <Route
          path="createTrack"
          element={
            <AuthorizedRoute loggedInUser={loggedInUser}>
              {<CreateTrack loggedInUser={loggedInUser} />}
            </AuthorizedRoute>
          }
        />
        <Route
          path="createAlbum"
          element={
            <AuthorizedRoute loggedInUser={loggedInUser}>
              {<CreateAlbum loggedInUser={loggedInUser} />}
            </AuthorizedRoute>
          }
        />
        <Route path="track/:id">
          <Route
            index
            element={
              <AuthorizedRoute loggedInUser={loggedInUser}>
                {<TrackDetailsView loggedInUser={loggedInUser} />}
              </AuthorizedRoute>
            }
          />
        </Route>
        <Route path="album/:id">
          <Route
            index
            element={
              <AuthorizedRoute loggedInUser={loggedInUser}>
                {<AlbumDetails loggedInUser={loggedInUser} />}
              </AuthorizedRoute>
            }
          />
        </Route>
        <Route
          path="employees"
          element={
            <AuthorizedRoute roles={["Admin"]} loggedInUser={loggedInUser}>
              <p>Employees</p>
            </AuthorizedRoute>
          }
        />
        <Route
          path="login"
          element={<Login setLoggedInUser={setLoggedInUser} />}
        />
        <Route
          path="register"
          element={<Register setLoggedInUser={setLoggedInUser} />}
        />
      </Route>
      <Route path="*" element={<p>Whoops, nothing here...</p>} />
    </Routes>
  );
}
