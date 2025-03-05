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
      className="container py-5"
      style={{ maxWidth: "900px", paddingBottom: "100px" }}
    >
      <div className="row">
        <div className="col-md-5">
          <div className="position-relative">
            <img
              src={
                track.coverArtUrl ||
                "https://picsum.photos/seed/default/500/500"
              }
              alt={`Cover art for ${track.title}`}
              style={{
                width: "100%",
                aspectRatio: "1/1",
                objectFit: "cover",
                borderRadius: "8px",
                boxShadow: "0 4px 12px rgba(0,0,0,0.1)",
              }}
            />
          </div>

          {/* Instrument section - Shown in both edit and view modes */}
          {isEditing ? (
            <div className="mt-4">
              <h5 className="mb-2">Instrument Categories</h5>
              <div className="d-flex flex-wrap gap-1 mb-3">
                {instrumentCategories.map((category) => (
                  <Card
                    key={category.id}
                    className={`text-center tag-choice p-0 ${
                      category.id === selectedInstrumentCategory && "tagChosen"
                    }`}
                    onClick={() => {
                      if (selectedInstrumentCategory === category.id) {
                        setSelectedInstrumentCategory(0);
                      } else {
                        setSelectedInstrumentCategory(category.id);
                      }
                    }}
                    style={{
                      cursor: "pointer",
                      maxWidth: "fit-content",
                      backgroundColor:
                        category.id === selectedInstrumentCategory
                          ? "#6c757d"
                          : "",
                      color:
                        category.id === selectedInstrumentCategory
                          ? "white"
                          : "",
                    }}
                  >
                    <CardBody
                      className="py-1 px-2"
                      style={{ fontSize: "0.8rem" }}
                    >
                      {category.name}
                    </CardBody>
                  </Card>
                ))}
              </div>

              {/* Display instruments for the selected category */}
              {selectedInstrumentCategory > 0 && (
                <div>
                  <h6 className="mb-2">Available Instruments</h6>
                  <div className="d-flex flex-wrap gap-1 mb-3">
                    {filteredInstruments.length > 0 ? (
                      filteredInstruments.map((instrument) => (
                        <Card
                          key={instrument.id}
                          className="text-center p-0"
                          onClick={() => handleInstrumentClick(instrument.id)}
                          style={{
                            cursor: "pointer",
                            maxWidth: "fit-content",
                            backgroundColor: selectedInstruments.includes(
                              instrument.id
                            )
                              ? "#198754" // Bootstrap green for selected instruments
                              : "#e9ecef", // Light gray for unselected instruments
                            color: selectedInstruments.includes(instrument.id)
                              ? "white"
                              : "black",
                          }}
                        >
                          <CardBody
                            className="py-1 px-2"
                            style={{ fontSize: "0.8rem" }}
                          >
                            {instrument.name}
                          </CardBody>
                        </Card>
                      ))
                    ) : (
                      <p className="text-muted small">
                        No instruments found in this category.
                      </p>
                    )}
                  </div>
                </div>
              )}

              {/* Selected Instruments Display */}
              <div>
                <h6 className="mb-2">Selected Instruments</h6>
                <div className="d-flex flex-wrap gap-1">
                  {instruments
                    .filter((i) => selectedInstruments.includes(i.id))
                    .map((instrument) => (
                      <span
                        key={instrument.id}
                        className="badge bg-success text-white p-2"
                        style={{ borderRadius: "20px" }}
                      >
                        {instrument.name}
                        <button
                          className="btn-close btn-close-white ms-1"
                          style={{ fontSize: "0.5rem" }}
                          onClick={() => handleInstrumentClick(instrument.id)}
                        ></button>
                      </span>
                    ))}
                  {selectedInstruments.length === 0 && (
                    <p className="text-muted small">No instruments selected</p>
                  )}
                </div>
              </div>
            </div>
          ) : (
            /* Display instruments in view mode */
            track.instruments &&
            track.instruments.length > 0 && (
              <div className="mt-4">
                <h5 className="mb-2">Instruments</h5>
                <div className="d-flex flex-wrap gap-2">
                  {track.instruments.map((instrument) => (
                    <span
                      key={instrument.id}
                      className="badge bg-light text-dark p-2"
                      style={{ borderRadius: "20px" }}
                    >
                      {instrument.name}
                    </span>
                  ))}
                </div>
              </div>
            )
          )}
        </div>

        <div className="col-md-7">
          <div className="d-flex justify-content-between align-items-start">
            <div>
              {isEditing ? (
                <Input
                  type="text"
                  name="title"
                  value={editForm.title}
                  onChange={handleEditChange}
                  className="form-control-lg mb-2"
                  style={{
                    color: "white",
                    fontSize: "2rem",
                    fontWeight: "bold",
                    border: "1px solid transparent",
                    background: "rgba(0,0,0,0.03)",
                    padding: "0.25rem 0.5rem",
                    borderRadius: "4px",
                  }}
                />
              ) : (
                <h1 className="mb-2">{track.title}</h1>
              )}
              <h5 className="text-muted mb-4">
                {track.creator?.userName || "Unknown Artist"}
              </h5>
            </div>
            {track.creator?.id === loggedInUser.id &&
              (isEditing ? (
                <div>
                  <Button
                    color="success"
                    size="sm"
                    className="me-2"
                    onClick={handleSave}
                  >
                    Save
                  </Button>
                  <Button
                    color="secondary"
                    size="sm"
                    onClick={() => setIsEditing(false)}
                  >
                    Cancel
                  </Button>
                </div>
              ) : (
                <Button
                  color="outline-primary"
                  size="sm"
                  onClick={() => setIsEditing(true)}
                >
                  Edit Track
                </Button>
              ))}
          </div>

          <div className="mb-4">
            <div className="d-flex justify-content-between mb-2">
              <span>
                Progress:
                {isEditing ? (
                  <Input
                    type="number"
                    name="percentageDone"
                    value={editForm.percentageDone}
                    onChange={handleEditChange}
                    min="0"
                    max="100"
                    className="d-inline-block ms-2"
                    style={{ width: "70px" }}
                  />
                ) : (
                  ` ${track.percentageDone}%`
                )}
              </span>
              <span>{track.isComplete ? "Completed" : "In Progress"}</span>
            </div>
            <Progress
              value={isEditing ? editForm.percentageDone : track.percentageDone}
              color={track.isComplete ? "success" : "primary"}
              style={{ height: "10px", borderRadius: "5px" }}
            />
          </div>

          <div className="mb-4">
            <h5>Details</h5>
            <div className="row">
              <div className="col-6">
                <p className="mb-1">
                  <strong>Upload Date:</strong>
                </p>
                <p>{new Date(track.uploadDate).toLocaleDateString()}</p>
              </div>
              <div className="col-6">
                <p className="mb-1">
                  <strong>Deadline:</strong>
                </p>
                {isEditing ? (
                  <Input
                    type="date"
                    name="deadline"
                    value={editForm.deadline}
                    onChange={handleEditChange}
                  />
                ) : (
                  <p>{formatDate(track.deadline)}</p>
                )}
              </div>
            </div>
          </div>

          {track.album && (
            <div className="mb-4">
              <h5>Part of Album</h5>
              <div
                className="d-flex align-items-center p-2 border rounded cursor-pointer"
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
                  <h6 className="mb-0">{track.album.title}</h6>
                  <small>Track #{track.album.trackOrder}</small>
                </div>
              </div>
            </div>
          )}

          {isEditing ? (
            <div className="mt-4">
              <h5>Audio</h5>
              <audio
                controls
                src={
                  editForm.audioFile && audioChange
                    ? URL.createObjectURL(editForm.audioFile)
                    : track.audioUrl
                }
                className="w-100"
                style={{ borderRadius: "8px" }}
              >
                Your browser does not support the audio element.
              </audio>

              <FormGroup>
                <Label for="audioFile" className="fw-bold mt-2">
                  Update Audio
                </Label>
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
                  className="mb-3"
                />
              </FormGroup>
            </div>
          ) : (
            track.audioUrl && (
              <div className="mt-4">
                <h5>Audio</h5>
                <audio
                  controls
                  src={track.audioUrl}
                  className="w-100"
                  style={{ borderRadius: "8px" }}
                >
                  Your browser does not support the audio element.
                </audio>
              </div>
            )
          )}
        </div>
      </div>

      <div className="d-flex justify-content-between mt-5">
        <Button color="secondary" outline onClick={() => navigate(-1)}>
          Back
        </Button>

        {track.creator?.id === loggedInUser.id && !isEditing && (
          <Button
            color="danger"
            outline
            onClick={() => {
              if (
                window.confirm("Are you sure you want to delete this track?")
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
        )}
      </div>
    </div>
  );
}
