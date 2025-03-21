import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import {
  deleteTrack,
  getTrackByTrackId,
  updateTrack,
} from "../../managers/trackManager";
import {
  Button,
  Progress,
  Input,
  FormGroup,
  Label,
  Card,
  CardBody,
} from "reactstrap";
import { getAllInstrumentCategories } from "../../managers/instrumentCategoryManager";
import { getAllInstruments } from "../../managers/instrumentManager";
import { uploadToCloudinary } from "../../managers/cloudinaryManager";

export default function TrackDetailsView({ loggedInUser }) {
  const [track, setTrack] = useState(null);
  const [isEditing, setIsEditing] = useState(false);
  const [editForm, setEditForm] = useState({});
  const [instruments, setInstruments] = useState([]);
  const [instrumentCategories, setInstrumentCategories] = useState([]);
  const [selectedInstrumentCategory, setSelectedInstrumentCategory] =
    useState(0);
  const [selectedInstruments, setSelectedInstruments] = useState([]);
  const [audioChange, setAudioChange] = useState(false);

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
        deadline: track.deadline
          ? new Date(track.deadline).toISOString().split("T")[0]
          : "",
        coverArtUrl: track.coverArtUrl,
        audioUrl: track.audioUrl,
      });

      // If the track has instruments, initialize selectedInstruments with their IDs
      if (track.instruments && track.instruments.length > 0) {
        setSelectedInstruments(
          track.instruments.map((instrument) => instrument.id)
        );
      }
    }
  }, [track]);

  // Load instrument categories and instruments when editing starts
  useEffect(() => {
    if (isEditing) {
      getAllInstrumentCategories().then(setInstrumentCategories);
      getAllInstruments().then(setInstruments);
    }
  }, [isEditing]);

  if (!track) {
    return (
      <div className="d-flex justify-content-center mt-5">Track not found</div>
    );
  }

  // Filter instruments by the selected category
  const filteredInstruments = selectedInstrumentCategory
    ? instruments.filter(
        (instrument) => instrument.categoryId === selectedInstrumentCategory
      )
    : [];

  const handleInstrumentClick = (instrumentId) => {
    setSelectedInstruments((prev) => {
      // If instrument is already selected, remove it
      if (prev.includes(instrumentId)) {
        return prev.filter((id) => id !== instrumentId);
      }
      // Otherwise add it to selected instruments
      else {
        return [...prev, instrumentId];
      }
    });
  };

  const formatDate = (dateString) => {
    if (!dateString) return "No deadline";

    // Get just the date part (YYYY-MM-DD) from the string
    const datePart = dateString.split("T")[0];
    const [year, month, day] = datePart.split("-");

    // Format it as MM/DD/YYYY (or however you prefer)
    return `${month}/${day}/${year}`;
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
        deadline: editForm.deadline ? new Date(editForm.deadline) : null,
        coverArtUrl: track.coverArtUrl,
        audioUrl: track.audioUrl,
        instrumentIds: selectedInstruments,
      };

      if (editForm.audioFile) {
        const audioUrl = await uploadToCloudinary(editForm.audioFile);
        updatedTrackData.audioUrl = audioUrl;
      }

      updateTrack(updatedTrackData)
        .then((data) => {
          setTrack(data);
          setIsEditing(false);
        })
        .catch((error) => {
          console.error("Error updating track:", error);
        });
    } catch (error) {
      console.error("Error during file upload:", error);
    }
  };

  return (
    <div
      style={{
        color: "white",
        minHeight: "100vh",
      }}
    >
      <div className="container-fluid">
        <div className="d-flex align-items-center">
          <button
            className="btn btn-link text-white-50 p-0 border-0"
            onClick={() => navigate(-1)}
            style={{ textDecoration: "none" }}
          >
            <i className="bi bi-chevron-left"></i>
          </button>
        </div>
        <div className="d-flex align-items-center">
          <button
            className="btn btn-link text-white-50 p-0 border-0"
            style={{ textDecoration: "none" }}
          >
            <i className="bi bi-link-45deg"></i>
          </button>
          <button
            className="btn btn-link text-white-50 p-0 ms-3 border-0"
            style={{ textDecoration: "none" }}
          >
            <i className="bi bi-search"></i>
          </button>
          <button
            className="btn btn-link text-white-50 p-0 ms-3 border-0"
            style={{ textDecoration: "none" }}
          >
            <i className="bi bi-person"></i>
          </button>
        </div>
      </div>

      <div className="container-fluid p-3">
        <div className="row">
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
              {/* <button
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
                onClick={() => {
                  const audioElement = document.querySelector("audio");
                  if (audioElement) audioElement.play();
                }}
              >
                <i
                  className="bi bi-play-fill text-dark"
                  style={{ fontSize: "20px" }}
                ></i>
              </button> */}
            </div>
          </div>

          {/* Right side - Track details */}
          <div className="col-md-7">
            <div className="mb-4">
              <h1 className="display-5 fw-bold mb-0">
                {isEditing ? (
                  <Input
                    type="text"
                    name="title"
                    value={editForm.title}
                    onChange={handleEditChange}
                    className="form-control bg-transparent text-white border-dark"
                  />
                ) : (
                  track.title
                )}
              </h1>
              <p className="text-white-50 mb-1">
                {track.creator?.userName || "Unknown Artist"} •
                <span className="ms-1">
                  {track.isComplete
                    ? "Completed"
                    : `${track.percentageDone}% complete`}
                </span>{" "}
                •<span className="ms-1">{formatDate(track.uploadDate)}</span>
              </p>
            </div>

            {/* Add these sections only when in edit mode */}
            {isEditing && (
              <>
                <div className="row mb-3">
                  <div className="col-md-6">
                    <div className="form-group">
                      <label className="form-label text-white-50">
                        Completion (%)
                      </label>
                      <Input
                        type="number"
                        name="percentageDone"
                        value={editForm.percentageDone}
                        onChange={handleEditChange}
                        min="0"
                        max="100"
                        className="form-control form-control-sm bg-dark text-white border-dark"
                      />
                    </div>
                  </div>
                  <div className="col-md-6">
                    <div className="form-group">
                      <label className="form-label text-white-50">
                        Deadline
                      </label>
                      <Input
                        type="date"
                        name="deadline"
                        value={editForm.deadline}
                        onChange={handleEditChange}
                        className="form-control form-control-sm bg-dark text-white border-dark"
                      />
                    </div>
                  </div>
                </div>
              </>
            )}

            {/* Audio player */}
            {(track.audioUrl || (editForm.audioFile && audioChange)) && (
              <div className="mb-4">
                <audio
                  controls
                  src={
                    editForm.audioFile && audioChange
                      ? URL.createObjectURL(editForm.audioFile)
                      : track.audioUrl
                  }
                  className="w-100"
                  style={{
                    borderRadius: "4px",
                    backgroundColor: "#1a1a1a",
                  }}
                >
                  Your browser does not support the audio element.
                </audio>

                {isEditing && (
                  <div className="mt-3">
                    <Input
                      type="file"
                      id="audioFile"
                      name="audioFile"
                      accept="audio/*"
                      onChange={(e) => {
                        handleEditChange(e);
                        if (e.target.files && e.target.files[0]) {
                          setAudioChange(true);
                        }
                      }}
                      className="form-control bg-dark text-white border-dark"
                    />
                  </div>
                )}
              </div>
            )}

            {/* Track details that are always visible */}
            {!isEditing && track.deadline && (
              <div className="mb-3">
                <p className="mb-1 text-white-50">
                  <strong>Deadline:</strong> {formatDate(track.deadline)}
                </p>
              </div>
            )}

            {/* Album info */}
            {track.album && (
              <div className="card bg-dark mb-4 border-0">
                <div className="card-body p-2">
                  <div
                    className="d-flex align-items-center"
                    style={{ cursor: "pointer" }}
                    onClick={() => navigate(`/album/${track.album.id}`)}
                  >
                    <div className="me-3">
                      <img
                        src={
                          track.album.coverArtUrl ||
                          "https://picsum.photos/seed/album/100/100"
                        }
                        alt={track.album.title}
                        style={{
                          width: "50px",
                          height: "50px",
                          objectFit: "cover",
                          borderRadius: "4px",
                        }}
                      />
                    </div>
                    <div>
                      <p className="card-title mb-0">{track.album.title}</p>
                      <p className="card-text small text-white-50">
                        Track #{track.album.trackOrder}
                      </p>
                    </div>
                  </div>
                </div>
              </div>
            )}

            {/* Instruments section */}
            <div className="mb-4">
              {isEditing ? (
                <>
                  <p className="text-white-50 mb-2">Select instruments:</p>

                  <div className="mb-3">
                    <div className="d-flex flex-wrap gap-2 mb-2">
                      {instrumentCategories.map((category) => (
                        <button
                          key={category.id}
                          onClick={() => {
                            if (selectedInstrumentCategory === category.id) {
                              setSelectedInstrumentCategory(0);
                            } else {
                              setSelectedInstrumentCategory(category.id);
                            }
                          }}
                          className={`btn btn-sm ${
                            selectedInstrumentCategory === category.id
                              ? "btn-secondary"
                              : "btn-outline-secondary"
                          }`}
                        >
                          {category.name}
                        </button>
                      ))}
                    </div>

                    {selectedInstrumentCategory > 0 && (
                      <div className="d-flex flex-wrap gap-2 mb-3">
                        {filteredInstruments.length > 0 ? (
                          filteredInstruments.map((instrument) => (
                            <button
                              key={instrument.id}
                              onClick={() =>
                                handleInstrumentClick(instrument.id)
                              }
                              className={`btn btn-sm ${
                                selectedInstruments.includes(instrument.id)
                                  ? "btn-light text-dark"
                                  : "btn-outline-light"
                              }`}
                            >
                              {instrument.name}
                            </button>
                          ))
                        ) : (
                          <p className="text-muted small">
                            No instruments found in this category.
                          </p>
                        )}
                      </div>
                    )}
                  </div>

                  <div className="d-flex flex-wrap gap-2">
                    {instruments
                      .filter((i) => selectedInstruments.includes(i.id))
                      .map((instrument) => (
                        <span
                          key={instrument.id}
                          className="badge bg-secondary text-white rounded-pill px-3 py-2"
                        >
                          {instrument.name}
                          <button
                            className="btn-close btn-close-white ms-2"
                            style={{ fontSize: "0.5rem" }}
                            onClick={() => handleInstrumentClick(instrument.id)}
                          ></button>
                        </span>
                      ))}
                  </div>
                </>
              ) : (
                track.instruments &&
                track.instruments.length > 0 && (
                  <>
                    <p className="text-white-50 mb-2">Instruments:</p>
                    <div className="d-flex flex-wrap gap-2">
                      {track.instruments.map((instrument) => (
                        <span
                          key={instrument.id}
                          className="badge bg-dark text-white-50 border border-secondary rounded-pill px-3 py-2"
                        >
                          {instrument.name}
                        </span>
                      ))}
                    </div>
                  </>
                )
              )}
            </div>

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

        <div className="row mt-4">
          <div className="col-12">
            {track.instruments && track.instruments.length > 0 && (
              <div className="mb-3">
                {/* Hidden section for mobile view (appears below) */}
                <div className="d-md-none">
                  <p className="text-white-50 mb-2">Instruments:</p>
                  <div className="d-flex flex-wrap gap-2">
                    {track.instruments.map((instrument) => (
                      <span
                        key={instrument.id}
                        className="badge bg-dark text-white-50 border border-secondary rounded-pill px-3 py-2"
                      >
                        {instrument.name}
                      </span>
                    ))}
                  </div>
                </div>
              </div>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}
