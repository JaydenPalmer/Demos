import { useEffect, useState, useRef } from "react";
import { useParams, useNavigate } from "react-router-dom";
import { Button, Progress } from "reactstrap";
import { getAlbumById } from "../../managers/albumManager";
import { deleteTrack } from "../../managers/trackManager";
import { deleteAlbum } from "../../managers/albumManager";

export default function AlbumDetails({ loggedInUser }) {
  const [album, setAlbum] = useState(null);
  const [currentTrackIndex, setCurrentTrackIndex] = useState(-1);
  const [isPlaying, setIsPlaying] = useState(false);
  const [currentTime, setCurrentTime] = useState(0);
  const [duration, setDuration] = useState(0);
  const [activeMenuTrackId, setActiveMenuTrackId] = useState(null);
  const [isDeleting, setIsDeleting] = useState(false);

  const audioRef = useRef(null);
  const progressRef = useRef(null);
  const { id } = useParams();
  const navigate = useNavigate();

  // Fetch album data
  useEffect(() => {
    getAlbumById(parseInt(id)).then(setAlbum);
  }, [id]);

  // Add click outside listener to close dropdown
  useEffect(() => {
    const handleClickOutside = (event) => {
      if (activeMenuTrackId && !event.target.closest(".track-menu")) {
        setActiveMenuTrackId(null);
      }
    };

    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [activeMenuTrackId]);

  // Play a specific track
  const playTrack = (index) => {
    if (album?.tracks && album.tracks.length > index && index >= 0) {
      setCurrentTrackIndex(index);

      if (audioRef.current) {
        audioRef.current.src = album.tracks[index].audioUrl;
        audioRef.current.play().catch((error) => {
          console.error("Error playing audio:", error);
        });
        setIsPlaying(true);
      }
    }
  };

  // Pause the currently playing track
  const pauseTrack = () => {
    if (audioRef.current && isPlaying) {
      audioRef.current.pause();
      setIsPlaying(false);
    }
  };

  // Resume the currently playing track
  const resumeTrack = () => {
    if (audioRef.current && !isPlaying && currentTrackIndex >= 0) {
      audioRef.current.play().catch((error) => {
        console.error("Error playing audio:", error);
      });
      setIsPlaying(true);
    }
  };

  // Handle audio time update
  const handleTimeUpdate = () => {
    if (audioRef.current) {
      setCurrentTime(audioRef.current.currentTime);
      setDuration(audioRef.current.duration || 0);
    }
  };

  // Handle seeking (scrubbing)
  const handleSeek = (e) => {
    if (audioRef.current && progressRef.current) {
      const progressRect = progressRef.current.getBoundingClientRect();
      const clickPosition =
        (e.clientX - progressRect.left) / progressRect.width;
      const newTime = clickPosition * audioRef.current.duration;

      // Set currentTime directly on the audio element
      audioRef.current.currentTime = newTime;
      setCurrentTime(newTime);
    }
  };

  // Toggle track menu
  const toggleTrackMenu = (e, trackId) => {
    e.stopPropagation(); // Prevent track click
    if (activeMenuTrackId === trackId) {
      setActiveMenuTrackId(null);
    } else {
      setActiveMenuTrackId(trackId);
    }
  };

  // Handle delete track
  const handleDeleteTrack = async (e, trackId) => {
    e.stopPropagation(); // Prevent track click
    if (window.confirm("Are you sure you want to delete this track?")) {
      try {
        await deleteTrack(trackId);
        // Refresh album data
        getAlbumById(parseInt(id)).then(setAlbum);
        // Reset current track if the deleted track was playing
        if (
          currentTrackIndex >= 0 &&
          album.tracks[currentTrackIndex].id === trackId
        ) {
          setCurrentTrackIndex(-1);
          if (audioRef.current) {
            audioRef.current.pause();
          }
          setIsPlaying(false);
        }
      } catch (error) {
        console.error("Error deleting track:", error);
      }
    }
    setActiveMenuTrackId(null);
  };

  // Navigate to edit track
  const navigateToEditTrack = (e, trackId) => {
    e.stopPropagation(); // Prevent track click
    navigate(`/track/${trackId}`);
    setActiveMenuTrackId(null);
  };

  // Format time (e.g., "3:45")
  const formatTime = (timeInSeconds) => {
    if (isNaN(timeInSeconds) || timeInSeconds === 0) return "0:00";

    const minutes = Math.floor(timeInSeconds / 60);
    const seconds = Math.floor(timeInSeconds % 60);
    return `${minutes}:${seconds.toString().padStart(2, "0")}`;
  };

  // Loading state
  if (!album) {
    return (
      <div
        className="d-flex justify-content-center align-items-center"
        style={{ height: "100vh", backgroundColor: "#0a0a0a" }}
      >
        <div className="spinner-border text-light" role="status">
          <span className="visually-hidden">Loading...</span>
        </div>
      </div>
    );
  }

  return (
    <div
      style={{
        backgroundColor: "#0a0a0a",
        color: "white",
        minHeight: "100vh",
        paddingBottom: currentTrackIndex >= 0 ? "80px" : "0",
      }}
    >
      <div className="container-fluid p-3">
        <div className="row">
          {/* Left side - Album artwork and info */}
          <div className="col-md-5 mb-4 text-center">
            <div
              className="position-relative"
              style={{ maxWidth: "520px", margin: "0 auto" }}
            >
              <img
                src={
                  album.coverArtUrl ||
                  "https://picsum.photos/seed/default/500/500"
                }
                alt={`Cover art for ${album.title}`}
                className="img-fluid rounded-3"
                style={{
                  width: "100%",
                  aspectRatio: "1/1",
                  objectFit: "cover",
                }}
              />
            </div>

            <div className="d-md-none mt-3">
              <h1 className="display-5 fw-bold mb-0">{album.title}</h1>
              <p className="text-white-50 mb-1">
                {album.creator?.userName || "Unknown Artist"} •
                <span className="ms-1">{album.tracks?.length || 0} tracks</span>{" "}
                •
                <span className="ms-1">
                  {album.createdDate
                    ? new Date(album.createdDate).getFullYear()
                    : ""}
                </span>
              </p>
            </div>
          </div>

          {/* Right side - Album info and tracks list */}
          <div className="col-md-7">
            {/* Album info - desktop view */}
            <div className="d-none d-md-block mb-4">
              <h1 className="display-5 fw-bold mb-0">{album.title}</h1>
              <p className="text-white-50 mb-1">
                {album.creator?.userName || "Unknown Artist"} •
                <span className="ms-1">{album.tracks?.length || 0} tracks</span>{" "}
                •
                <span className="ms-1">
                  {album.createdDate
                    ? new Date(album.createdDate).getFullYear()
                    : ""}
                </span>
              </p>
            </div>

            {/* Tracks list */}
            <div className="tracks-list mb-4">
              {album.tracks &&
                album.tracks.map((track, index) => (
                  <div
                    key={track.id}
                    className="track-item d-flex align-items-center p-2 mb-2 rounded-2"
                    style={{
                      cursor: "pointer",
                      backgroundColor:
                        currentTrackIndex === index ? "#1a1a1a" : "transparent",
                      transition: "background-color 0.2s",
                    }}
                    onClick={() => playTrack(index)}
                    onMouseEnter={(e) => {
                      if (currentTrackIndex !== index) {
                        e.currentTarget.style.backgroundColor = "#1a1a1a80";
                      }
                    }}
                    onMouseLeave={(e) => {
                      if (currentTrackIndex !== index) {
                        e.currentTarget.style.backgroundColor = "transparent";
                      }
                    }}
                  >
                    {/* Track number or play/pause indicator */}
                    <div
                      className="me-3 text-white-50"
                      style={{ width: "24px" }}
                    >
                      {currentTrackIndex === index ? (
                        <i
                          className={`bi ${
                            isPlaying ? "bi-pause-fill" : "bi-play-fill"
                          }`}
                        ></i>
                      ) : (
                        <span>{index + 1}</span>
                      )}
                    </div>

                    {/* Track info */}
                    <div className="flex-grow-1">
                      <div className="d-flex justify-content-between align-items-start">
                        <div>
                          <p className="mb-0">{track.title}</p>
                          <p className="mb-0 text-white-50 small">
                            {track.isComplete
                              ? "Complete"
                              : `${track.percentageDone}% complete`}
                          </p>
                        </div>
                        <div className="track-menu position-relative">
                          <button
                            className="btn btn-link text-white-50 p-0 border-0"
                            style={{ textDecoration: "none" }}
                            onClick={(e) => toggleTrackMenu(e, track.id)}
                          >
                            <span
                              style={{ fontWeight: "bold", fontSize: "1.2rem" }}
                            >
                              &#8942;
                            </span>
                          </button>

                          {/* Track options menu */}
                          {activeMenuTrackId === track.id && (
                            <div
                              className="position-absolute end-0 mt-1 py-1 bg-dark border border-secondary rounded"
                              style={{
                                minWidth: "150px",
                                zIndex: 1000,
                                boxShadow: "0 2px 10px rgba(0,0,0,0.3)",
                              }}
                            >
                              <button
                                className="dropdown-item text-white py-2 px-3 d-flex align-items-center"
                                onClick={(e) =>
                                  navigateToEditTrack(e, track.id)
                                }
                              >
                                <span className="me-2">✏️</span> Edit Track
                              </button>

                              {loggedInUser &&
                                (loggedInUser.id === track.creator?.id ||
                                  loggedInUser.id === album.creator?.id) && (
                                  <button
                                    className="dropdown-item text-danger py-2 px-3 d-flex align-items-center"
                                    onClick={(e) =>
                                      handleDeleteTrack(e, track.id)
                                    }
                                  >
                                    <span className="me-2">🗑️</span>
                                    Delete Track
                                  </button>
                                )}
                            </div>
                          )}
                        </div>
                      </div>
                    </div>
                  </div>
                ))}
            </div>

            {/* Delete button only - Edit Album button removed */}
            {album.creator?.id === loggedInUser?.id && (
              <div className="d-flex gap-2 mt-4">
                <Button
                  color="danger"
                  outline
                  size="sm"
                  className="flex-grow-1"
                  onClick={() => {
                    if (
                      window.confirm(
                        "Are you sure you want to delete this album?"
                      )
                    ) {
                      setIsDeleting(true); // Add this state variable
                      deleteAlbum(album.id)
                        .then(() => {
                          navigate("/");
                        })
                        .catch((error) => {
                          console.error("Detailed album deletion error:", {
                            message: error.message,
                            stack: error.stack,
                            // Any additional error details
                          });
                          window.alert(
                            `Album deletion failed: ${error.message}`
                          );
                        })
                        .finally(() => {
                          setIsDeleting(false);
                        });
                    }
                  }}
                  disabled={isDeleting} // Add this state variable
                >
                  {isDeleting ? "Deleting..." : "Delete Album"}
                </Button>
              </div>
            )}
          </div>
        </div>
      </div>

      {/* Fixed bottom player with play/pause buttons and scrubbing */}
      {currentTrackIndex >= 0 &&
        album.tracks &&
        album.tracks[currentTrackIndex] && (
          <div
            className="fixed-bottom py-2 px-3"
            style={{
              backgroundColor: "#181818",
              borderTop: "1px solid #333",
              boxShadow: "0 -4px 12px rgba(0,0,0,0.4)",
            }}
          >
            {/* Scrubbing progress bar at top of player */}
            <div
              ref={progressRef}
              className="position-absolute top-0 start-0 end-0"
              style={{ cursor: "pointer", height: "6px" }}
              onClick={handleSeek}
            >
              <Progress
                value={(currentTime / duration) * 100 || 0}
                className="rounded-0"
                style={{
                  height: "6px",
                  backgroundColor: "#333",
                  cursor: "pointer",
                }}
              />
            </div>

            <div className="d-flex align-items-center pt-1 mt-1">
              {/* Track thumbnail */}
              <div
                className="me-3 d-none d-sm-block"
                style={{ width: "50px", height: "50px" }}
              >
                <img
                  src={
                    album.tracks[currentTrackIndex].coverArtUrl ||
                    album.coverArtUrl ||
                    "https://picsum.photos/seed/default/50/50"
                  }
                  alt="Track cover"
                  className="rounded"
                  style={{ width: "100%", height: "100%", objectFit: "cover" }}
                />
              </div>

              {/* Track info */}
              <div className="flex-grow-1">
                <div className="d-flex justify-content-between align-items-center">
                  <div>
                    <p className="mb-0 fw-bold">
                      {album.tracks[currentTrackIndex].title}
                    </p>
                    <p className="mb-0 text-white-50 small">
                      {album.creator?.userName}
                    </p>
                  </div>
                  <div className="text-white-50 small d-none d-md-block">
                    {formatTime(currentTime)} / {formatTime(duration)}
                  </div>
                </div>
              </div>

              {/* Player controls */}
              <div className="ms-3 d-flex align-items-center">
                {/* Play button */}
                {!isPlaying && (
                  <button
                    className="btn btn-light btn-sm rounded-circle d-flex align-items-center justify-content-center me-2"
                    style={{ width: "38px", height: "38px" }}
                    onClick={resumeTrack}
                  >
                    <i className="bi bi-play-fill fs-5"></i>
                  </button>
                )}

                {/* Pause button */}
                {isPlaying && (
                  <button
                    className="btn btn-light btn-sm rounded-circle d-flex align-items-center justify-content-center me-2"
                    style={{ width: "38px", height: "38px" }}
                    onClick={pauseTrack}
                  >
                    <i className="bi bi-pause-fill fs-5"></i>
                  </button>
                )}

                {/* Time display for mobile */}
                <div className="text-white-50 small d-block d-md-none">
                  {formatTime(currentTime)}
                </div>
              </div>
            </div>
          </div>
        )}

      {/* Hidden audio element */}
      <audio
        ref={audioRef}
        className="d-none"
        onTimeUpdate={handleTimeUpdate}
        onEnded={() => setIsPlaying(false)}
        onLoadedMetadata={(e) => setDuration(e.target.duration)}
      />
    </div>
  );
}
