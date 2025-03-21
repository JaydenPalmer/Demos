import { useEffect, useState, useRef } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
  deleteTrack,
  getTrackByTrackId,
  updateTrack,
} from "../../managers/trackManager";
import { Button, Progress, Input, FormGroup, Label } from "reactstrap";
import { uploadToCloudinary } from "../../managers/cloudinaryManager";

export default function TrackDetailsView({ loggedInUser }) {
  const [track, setTrack] = useState(null);
  const [isEditing, setIsEditing] = useState(true);
  const [editForm, setEditForm] = useState({});
  const [audioChange, setAudioChange] = useState(false);

  // Audio player states
  const [isPlaying, setIsPlaying] = useState(false);
  const [currentTime, setCurrentTime] = useState(0);
  const [duration, setDuration] = useState(0);

  const audioRef = useRef(null);
  const progressRef = useRef(null);
  const { id } = useParams();
  const navigate = useNavigate();

  useEffect(() => {
    getTrackByTrackId(id)
      .then((data) => {
        setTrack(data);
      })
      .catch((error) => {
        console.error("Error fetching track:", error);
      });
  }, [id]);

  useEffect(() => {
    if (track) {
      setEditForm({
        title: track.title,
        percentageDone: track.percentageDone,
        audioUrl: track.audioUrl,
      });
    }
  }, [track]);

  // Function to play the audio
  const playTrack = () => {
    if (audioRef.current) {
      audioRef.current.play().catch((error) => {
        console.error("Error playing audio:", error);
      });
      setIsPlaying(true);
    }
  };

  // Function to pause the audio
  const pauseTrack = () => {
    if (audioRef.current && isPlaying) {
      audioRef.current.pause();
      setIsPlaying(false);
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

      audioRef.current.currentTime = newTime;
      setCurrentTime(newTime);
    }
  };

  // Format time (e.g., "3:45")
  const formatTime = (timeInSeconds) => {
    if (isNaN(timeInSeconds) || timeInSeconds === 0) return "0:00";

    const minutes = Math.floor(timeInSeconds / 60);
    const seconds = Math.floor(timeInSeconds % 60);
    return `${minutes}:${seconds.toString().padStart(2, "0")}`;
  };

  const handleEditChange = (e) => {
    const { name, value, type, files } = e.target;

    if (type === "file") {
      if (files && files[0]) {
        setEditForm({
          ...editForm,
          [name]: files[0],
        });
      }
    } else {
      setEditForm({
        ...editForm,
        [name]: value,
      });
    }
  };

  const handleSave = async () => {
    try {
      let updatedTrackData = {
        ...track,
        title: editForm.title,
        percentageDone: parseInt(editForm.percentageDone),
        audioUrl: track.audioUrl,
      };

      if (editForm.audioFile) {
        const audioUrl = await uploadToCloudinary(editForm.audioFile);
        updatedTrackData.audioUrl = audioUrl;
      }

      updateTrack(updatedTrackData)
        .then((data) => {
          setTrack(data);
          setIsEditing(false);
          navigate(-1); // Go back to the album page
        })
        .catch((error) => {
          console.error("Error updating track:", error);
        });
    } catch (error) {
      console.error("Error during file upload:", error);
    }
  };

  if (!track) {
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
        paddingBottom: track.audioUrl ? "80px" : "0",
      }}
    >
      <div className="container-fluid p-3">
        <div className="row">
          {/* Left side - Track artwork */}
          <div className="col-md-5 mb-4 text-center">
            <div
              className="position-relative"
              style={{ maxWidth: "520px", margin: "0 auto" }}
            >
              <img
                src={
                  track.coverArtUrl ||
                  "https://picsum.photos/seed/default/500/500"
                }
                alt={`Cover art for ${track.title}`}
                className="img-fluid rounded-3"
                style={{
                  width: "100%",
                  aspectRatio: "1/1",
                  objectFit: "cover",
                }}
              />
              {!isEditing && (
                <button
                  className="btn d-flex align-items-center justify-content-center"
                  style={{
                    position: "absolute",
                    right: "10px",
                    bottom: "10px",
                    backgroundColor: "white",
                    width: "40px",
                    height: "40px",
                    borderRadius: "50%",
                    padding: "0",
                  }}
                  onClick={playTrack}
                >
                  <i
                    className="bi bi-play-fill text-dark"
                    style={{ fontSize: "20px" }}
                  ></i>
                </button>
              )}
            </div>
          </div>

          {/* Right side - Track details */}
          <div className="col-md-7">
            {isEditing ? (
              <>
                <div className="row mb-3">
                  <div className="col-md-12 mb-3">
                    <FormGroup>
                      <Input
                        type="text"
                        name="title"
                        value={editForm.title}
                        onChange={handleEditChange}
                        className="form-control-lg border-0"
                        style={{
                          backgroundColor: "transparent",
                          color: "white",
                          fontSize: "24px",
                          padding: "0",
                          outline: "none",
                          boxShadow: "none",
                        }}
                        placeholder="Track Title"
                      />
                    </FormGroup>
                  </div>
                </div>

                <div className="row mb-3">
                  <div className="col-12">
                    <div className="mb-3">
                      <Label className="form-label text-white-50">
                        Completion
                      </Label>
                      <div className="d-flex align-items-center">
                        <div className="flex-grow-1 me-3">
                          <input
                            type="range"
                            min="0"
                            max="100"
                            value={editForm.percentageDone}
                            onChange={(e) => {
                              handleEditChange({
                                target: {
                                  name: "percentageDone",
                                  value: e.target.value,
                                },
                              });
                            }}
                            className="form-range"
                            style={{ height: "8px" }}
                          />
                        </div>
                        <span
                          className="text-white-50 small"
                          style={{ minWidth: "40px" }}
                        >
                          {editForm.percentageDone}%
                        </span>
                      </div>
                    </div>

                    {/* Audio Update Section */}
                    <div
                      className="upload-area p-4"
                      style={{
                        border: "2px dashed #666",
                        borderRadius: "12px",
                        textAlign: "center",
                        backgroundColor: "#1a1a1a",
                      }}
                    >
                      <div className="mb-3">
                        <svg
                          xmlns="http://www.w3.org/2000/svg"
                          width="48"
                          height="48"
                          fill="currentColor"
                          className="mb-3 text-white-50"
                          viewBox="0 0 16 16"
                        >
                          <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z" />
                          <path d="M8 4a.5.5 0 0 1 .5.5v3h3a.5.5 0 0 1 0 1h-3v3a.5.5 0 0 1-1 0v-3h-3a.5.5 0 0 1 0-1h3v-3A.5.5 0 0 1 8 4z" />
                        </svg>
                        <h5>Update Audio</h5>
                        <p className="text-white-50">WAV, MP3, AIFF and more</p>
                      </div>

                      <label
                        className="btn btn-outline-light"
                        style={{ cursor: "pointer" }}
                      >
                        Choose File
                        <Input
                          type="file"
                          name="audioFile"
                          accept="audio/*"
                          onChange={(e) => {
                            handleEditChange(e);
                            if (e.target.files && e.target.files[0]) {
                              setAudioChange(true);
                            }
                          }}
                          style={{ display: "none" }}
                        />
                      </label>

                      {editForm.audioFile && (
                        <div className="mt-3">
                          <audio
                            controls
                            src={URL.createObjectURL(editForm.audioFile)}
                            className="w-100"
                            style={{
                              borderRadius: "4px",
                              backgroundColor: "#2a2a2a",
                            }}
                          >
                            Your browser does not support the audio element.
                          </audio>
                        </div>
                      )}
                    </div>
                  </div>
                </div>
              </>
            ) : (
              <>
                <h1 className="display-5 fw-bold mb-0">{track.title}</h1>
                <p className="text-white-50 mb-1">
                  {track.creator?.userName || "Unknown Artist"} •
                  <span className="ms-1">
                    {track.isComplete
                      ? "Completed"
                      : `${track.percentageDone}% complete`}
                  </span>
                </p>
              </>
            )}

            {/* Edit/Save/Delete buttons */}
            <div className="d-flex justify-content-between mt-4">
              {track.creator?.id === loggedInUser.id &&
                (isEditing ? (
                  <div className="d-flex gap-2 w-100">
                    <Button
                      color="secondary"
                      outline
                      className="flex-grow-1"
                      onClick={() => setIsEditing(false)}
                    >
                      Cancel
                    </Button>
                    <Button
                      color="light"
                      className="flex-grow-1"
                      onClick={handleSave}
                    >
                      Save Changes
                    </Button>
                  </div>
                ) : (
                  <div className="d-flex gap-2 w-100">
                    <Button
                      color="outline-light"
                      size="sm"
                      className="flex-grow-1"
                      onClick={() => setIsEditing(true)}
                    >
                      Edit Track
                    </Button>
                    <Button
                      color="danger"
                      outline
                      size="sm"
                      className="flex-grow-1"
                      onClick={() => {
                        if (
                          window.confirm(
                            "Are you sure you want to delete this track?"
                          )
                        ) {
                          deleteTrack(track.id)
                            .then(() => navigate("/"))
                            .catch((error) =>
                              console.error("Error deleting track:", error)
                            );
                        }
                      }}
                    >
                      Delete Track
                    </Button>
                  </div>
                ))}
            </div>
          </div>
        </div>
      </div>

      {/* Fixed bottom player with play/pause buttons and scrubbing */}
      {!isEditing && track.audioUrl && (
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
                  track.coverArtUrl ||
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
                  <p className="mb-0 fw-bold">{track.title}</p>
                  <p className="mb-0 text-white-50 small">
                    {track.creator?.userName}
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
                  onClick={playTrack}
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
        src={track.audioUrl}
        className="d-none"
        onTimeUpdate={handleTimeUpdate}
        onEnded={() => setIsPlaying(false)}
        onLoadedMetadata={(e) => setDuration(e.target.duration)}
      />
    </div>
  );
}
