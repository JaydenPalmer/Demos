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
    navigate("/createAlbum");
    setIsExpanded(false);
  };

  return (
    <>
      <div className="text-white" style={{ minHeight: "100vh" }}>
        <div
          className="container py-4"
          style={{ maxWidth: "900px", paddingBottom: "100px" }}
        >
          <div className="row g-4">
            {music.map((item) => (
              <div
                key={item.id}
                className="col-12 col-sm-6 col-md-4 col-xl-3 mb-4"
              >
                <div
                  className="card bg-transparent border-0"
                  style={{ padding: "10px" }}
                >
                  <div
                    style={{
                      position: "relative",
                      borderRadius: "12px",
                      overflow: "hidden",
                      marginBottom: "10px",
                    }}
                  >
                    <img
                      src={
                        item.coverArtUrl ||
                        "https://picsum.photos/seed/default/300/300"
                      }
                      alt={`Cover art for ${item.title}`}
                      className="card-img-top"
                      style={{
                        aspectRatio: "1/1",
                        objectFit: "cover",
                        cursor: "pointer",
                        width: "100%",
                      }}
                      onClick={() => {
                        if (item.album) {
                          navigate(`/album/${item.album.id}`);
                        } else {
                          navigate(`/track/${item.id}`);
                        }
                      }}
                    />
                    {/* <button
                      className="btn btn-success position-absolute d-flex align-items-center justify-content-center"
                      style={{
                        width: "40px",
                        height: "40px",
                        borderRadius: "50%",
                        bottom: "10px",
                        right: "10px",
                      }}
                      onClick={(e) => {
                        e.stopPropagation();
                        console.log(`Play ${item.title}`);
                      }}
                    >
                      <i
                        className="bi bi-play-fill"
                        style={{ fontSize: "20px" }}
                      ></i>
                    </button> */}
                  </div>

                  <div className="p-0">
                    <div className="d-flex justify-content-between align-items-start">
                      <div style={{ maxWidth: "85%", color: "white" }}>
                        <h5
                          className="mb-1 text-truncate"
                          style={{ fontSize: "16px", fontWeight: "600" }}
                        >
                          {item.album ? item.album.title : item.title}
                        </h5>
                        <p
                          className="mb-0 text-truncate text-light"
                          style={{ fontSize: "14px", opacity: "0.8" }}
                        >
                          {item.creator.userName}
                        </p>
                      </div>
                      <button
                        className="btn btn-sm p-0 text-white bg-transparent"
                        onClick={() => toggleMenu(item.id)}
                        style={{ marginTop: "2px" }}
                      >
                        <i className="bi bi-three-dots-vertical"></i>
                      </button>
                    </div>
                  </div>

                  {openMenuId === item.id && (
                    <div
                      className="position-absolute end-0 mt-1 py-1"
                      style={{
                        top: "70%",
                        right: "10px",
                        zIndex: 100,
                        backgroundColor: "#282828",
                        borderRadius: "8px",
                        boxShadow: "0 4px 12px rgba(0,0,0,0.5)",
                        minWidth: "150px",
                      }}
                    >
                      {item.creator.id === loggedInUser.id ? (
                        <button
                          className="dropdown-item d-flex align-items-center text-danger"
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
                          <i className="bi bi-trash me-2"></i>
                          Delete
                        </button>
                      ) : (
                        <button
                          className="dropdown-item d-flex align-items-center text-danger"
                          onClick={(e) => {
                            e.preventDefault();
                            console.log("leave collab");
                            toggleMenu(item.id);
                          }}
                        >
                          <i className="bi bi-box-arrow-right me-2"></i>
                          Leave Collab
                        </button>
                      )}
                    </div>
                  )}
                </div>
              </div>
            ))}
          </div>
        </div>
      </div>

      <div
        className="fixed-bottom d-flex justify-content-center mb-3"
        style={{ zIndex: 1050 }}
      >
        <div className="position-relative">
          {isExpanded && (
            <div
              className="position-absolute bottom-100 mb-2 d-flex flex-column"
              style={{
                left: "50%",
                transform: "translateX(-50%)",
              }}
            >
              <Button
                className="mb-2 d-flex align-items-center justify-content-center"
                style={{
                  width: "200px",
                  backgroundColor: "grey",
                  border: "none",
                  borderRadius: "24px",
                }}
                onClick={handleTrackNavigation}
              >
                <i className="bi bi-music-note-list me-2"></i> Create Track
              </Button>
              <Button
                className="d-flex align-items-center justify-content-center"
                style={{
                  width: "200px",
                  backgroundColor: "grey",
                  border: "none",
                  borderRadius: "24px",
                }}
                onClick={handleAlbumNavigation}
              >
                <i className="bi bi-collection me-2"></i> Create Album
              </Button>
            </div>
          )}

          <Button
            className="d-flex align-items-center justify-content-center"
            style={{
              width: "48px",
              height: "48px",
              borderRadius: "50%",
              backgroundColor: "white",
              border: "none",
              boxShadow: "0 4px 8px rgba(0,0,0,0.3)",
            }}
            onClick={() => setIsExpanded(!isExpanded)}
          >
            <i
              className={`bi ${isExpanded ? "bi-x-lg" : "bi-plus-lg"}`}
              style={{ fontSize: "1.25rem", color: "white" }}
            />
          </Button>
        </div>
      </div>
    </>
  );
}
