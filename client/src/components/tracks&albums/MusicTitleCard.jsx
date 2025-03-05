import { useEffect, useState } from "react";
import { deleteTrack, getTracksByUserId } from "../../managers/trackManager";
import { Button } from "reactstrap";
import { useNavigate } from "react-router-dom";

export default function MusicTitleCard({ loggedInUser }) {
  const [music, setMusic] = useState([]);
  const [openMenuId, setOpenMenuId] = useState(null);
  const [isExpanded, setIsExpanded] = useState(false);

  const navigate = useNavigate();

  const toggleMenu = (id) => {
    if (openMenuId === id) {
      setOpenMenuId(null);
    } else {
      setOpenMenuId(id);
    }
  };

  useEffect(() => {
    getTracksByUserId(loggedInUser.id).then(setMusic);
  }, [loggedInUser]);

  const handleTrackNavigation = () => {
    navigate("/createTrack");
    setIsExpanded(false);
  };

  const handleAlbumNavigation = () => {
    window.alert("this has not been set up yet");
    setIsExpanded(false);
  };

  return (
    <>
      <div
        className="container"
        style={{ maxWidth: "900px", paddingBottom: "100px" }}
      >
        <div className="row">
          {music.map((item) => (
            <div
              key={item.id}
              className="col-12 col-sm-6 col-md-4 col-xl-3 mb-3"
            >
              <div
                className="border-none h-100"
                style={{ position: "relative" }}
              >
                <div>
                  <img
                    src={
                      item.coverArtUrl ||
                      "https://picsum.photos/seed/default/300/300"
                    }
                    alt={`Cover art for ${item.title}`}
                    style={{
                      width: "100%",
                      height: "100%",
                      objectFit: "cover",
                      borderRadius: "4px",
                    }}
                    onClick={() => {
                      if (item.album) {
                        navigate(`/album/${item.album.id}`);
                      } else {
                        navigate(`/track/${item.id}`);
                      }
                    }}
                  />
                </div>
                <h4 className="mt-2">
                  {item.album ? item.album.title : item.title}
                </h4>
                <h5>{item.creator.userName}</h5>
                <button
                  className="menu-dots-button"
                  onClick={() => toggleMenu(item.id)}
                  style={{
                    position: "absolute",
                    bottom: "10px",
                    right: "10px",
                    border: "none",
                    background: "transparent",
                    cursor: "pointer",
                  }}
                >
                  ...
                </button>
                {openMenuId === item.id && (
                  <div
                    style={{
                      position: "absolute",
                      top: "100%",
                      right: "10px",
                      zIndex: 100,
                      borderRadius: "4px",
                      boxShadow: "0 2px 5px rgba(0,0,0,0.15)",
                      marginTop: "5px",
                    }}
                  >
                    {item.creator.id === loggedInUser.id ? (
                      <Button
                        color="danger"
                        onClick={(e) => {
                          e.preventDefault();
                          if (
                            window.confirm(
                              "Are you sure you want to delete this track?"
                            )
                          ) {
                            deleteTrack(item.id)
                              .then(() => {
                                toggleMenu(item.id);
                                getTracksByUserId(loggedInUser.id).then(
                                  setMusic
                                );
                              })
                              .catch((error) => {
                                console.error("Error deleting track:", error);
                              });
                          }
                        }}
                      >
                        Delete
                      </Button>
                    ) : (
                      <Button
                        color="danger"
                        onClick={(e) => {
                          e.preventDefault();
                          console.log("leave collab");
                          toggleMenu(item.id);
                        }}
                      >
                        Leave Collab
                      </Button>
                    )}
                  </div>
                )}
              </div>
            </div>
          ))}
        </div>
      </div>

      <div
        className="fixed-bottom d-flex justify-content-center mb-3"
        style={{ zIndex: 1050 }}
      >
        <div className="position-relative">
          {isExpanded && (
            <div className="position-absolute bottom-100 mb-2 d-flex flex-column align-items-center">
              <Button
                color="primary"
                className="mb-2 d-flex align-items-center"
                style={{ width: "200px" }}
                onClick={handleTrackNavigation}
              >
                <i className="bi bi-music-note-list mr-2"></i> Create Track
              </Button>
              <Button
                color="secondary"
                className="d-flex align-items-center"
                style={{ width: "200px" }}
                onClick={handleAlbumNavigation}
              >
                <i className="bi bi-collection mr-2"></i> Create Album
              </Button>
            </div>
          )}

          <Button
            color="light"
            className="d-flex align-items-center justify-content-center"
            style={{
              width: "120px",
              height: "48px",
              position: "relative",
              zIndex: 10,
              boxShadow: "0 2px 5px rgba(0,0,0,0.2)",
            }}
            onClick={() => setIsExpanded(!isExpanded)}
          >
            <i
              className={`bi ${
                isExpanded ? "bi-x-lg mr-2" : "bi-plus-lg mr-2"
              }`}
              style={{
                fontSize: "1rem",
                transition: "transform 0.3s ease",
              }}
            />
            ADD
          </Button>
        </div>
      </div>
    </>
  );
}
