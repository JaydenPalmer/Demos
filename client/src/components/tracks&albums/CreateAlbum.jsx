import { Form, Input, FormGroup, Label, Button } from "reactstrap";
import { useEffect, useState, useRef } from "react";
import { getAllInstruments } from "../../managers/instrumentManager";
import { getAllInstrumentCategories } from "../../managers/instrumentCategoryManager";
import { uploadToCloudinary } from "../../managers/cloudinaryManager";
import { useNavigate } from "react-router-dom";
import { createAlbumWithTracks } from "../../managers/albumManager";

export default function CreateAlbum({ loggedInUser }) {
  const [instruments, setInstruments] = useState([]);
  const [instrumentCategories, setInstrumentCategories] = useState([]);
  const [albumData, setAlbumData] = useState({
    title: "untitled project",
    description: "description",
    coverArtUrl: "https://picsum.photos/seed/default/500/500",
    isComplete: false,
    creatorId: loggedInUser?.id,
  });
  const [tracks, setTracks] = useState([]);
  const [selectedInstrumentCategory, setSelectedInstrumentCategory] =
    useState(0);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [uploadProgress, setUploadProgress] = useState(0);
  const [currentUploadingTrack, setCurrentUploadingTrack] = useState(null);
  const [isDraggingOver, setIsDraggingOver] = useState(false);
  const [allTracksComplete, setAllTracksComplete] = useState(false);
  const [currentGradient, setCurrentGradient] = useState("");

  const isSubmittingRef = useRef(false);
  const canvasRef = useRef(null);
  const navigate = useNavigate();

  useEffect(() => {
    getAllInstruments().then(setInstruments);
    getAllInstrumentCategories().then(setInstrumentCategories);

    const gradient = generateRandomGradient();
    setCurrentGradient(gradient);

    const canvas = document.createElement("canvas");
    canvas.width = 500;
    canvas.height = 500;
    canvas.style.display = "none";
    document.body.appendChild(canvas);
    canvasRef.current = canvas;

    return () => {
      if (canvasRef.current) {
        document.body.removeChild(canvasRef.current);
      }
    };
  }, []);

  useEffect(() => {
    if (tracks.length > 0) {
      const allComplete = tracks.every((track) => track.isComplete);
      setAllTracksComplete(allComplete);

      if (allComplete !== albumData.isComplete) {
        setAlbumData({
          ...albumData,
          isComplete: allComplete,
        });
      }
    }
  }, [tracks]);

  const handleAlbumChange = (e) => {
    const { name, value, type, checked } = e.target;
    setAlbumData({
      ...albumData,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const handleFileChange = (e) => {
    const files = e.target.files;
    handleFiles(files);
  };

  const handleDragOver = (e) => {
    e.preventDefault();
    setIsDraggingOver(true);
  };

  const handleDragLeave = () => {
    setIsDraggingOver(false);
  };

  const handleDrop = (e) => {
    e.preventDefault();
    setIsDraggingOver(false);
    const files = e.dataTransfer.files;
    handleFiles(files);
  };

  const handleFiles = (files) => {
    if (files && files.length > 0) {
      const newTracks = [...tracks];

      for (let i = 0; i < files.length; i++) {
        const file = files[i];
        if (file.type.startsWith("audio/")) {
          // Create a track name from file name
          const fileName = file.name.replace(/\.[^/.]+$/, ""); // Remove extension

          newTracks.push({
            title: fileName,
            percentageDone: "0",
            isComplete: false,
            deadline: "",
            description: "",
            coverArtUrl: "",
            audioFile: file,
            instrumentIds: [],
            creatorId: loggedInUser?.id,
          });
        }
      }

      setTracks(newTracks);
    }
  };

  const removeTrack = (index) => {
    setTracks(tracks.filter((_, i) => i !== index));
  };

  const updateTrackTitle = (index, title) => {
    const updatedTracks = [...tracks];
    updatedTracks[index].title = title;
    setTracks(updatedTracks);
  };

  const generateRandomGradient = () => {
    // Generate a gradient similar to the pink one in the screenshot
    const colors = [
      ["#FF61D2", "#FE9090"],
      ["#4158D0", "#C850C0"],
      ["#43CBFF", "#9708CC"],
      ["#8EC5FC", "#E0C3FC"],
      ["#D9AFD9", "#97D9E1"],
      ["#00DBDE", "#FC00FF"],
    ];

    const randomPair = colors[Math.floor(Math.random() * colors.length)];
    return `linear-gradient(135deg, ${randomPair[0]} 0%, ${randomPair[1]} 100%)`;
  };

  // Convert gradient to data URL
  const getGradientImageURL = (gradientStr) => {
    try {
      if (!canvasRef.current) return albumData.coverArtUrl;

      const canvas = canvasRef.current;
      const ctx = canvas.getContext("2d");

      // Parse the gradient string to extract colors
      const matches = gradientStr.match(/#[a-fA-F0-9]{6}/g);
      if (!matches || matches.length < 2) {
        return albumData.coverArtUrl;
      }

      const startColor = matches[0];
      const endColor = matches[1];

      // Create gradient
      const gradient = ctx.createLinearGradient(0, 0, 500, 500);
      gradient.addColorStop(0, startColor);
      gradient.addColorStop(1, endColor);

      // Fill with gradient
      ctx.fillStyle = gradient;
      ctx.fillRect(0, 0, 500, 500);

      // Convert to data URL
      return canvas.toDataURL("image/png");
    } catch (error) {
      console.error("Error converting gradient to image:", error);
      return albumData.coverArtUrl;
    }
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    submitAlbumData();
  };

  const submitAlbumData = async () => {
    try {
      // Validation
      if (!albumData.title || albumData.title === "untitled project") {
        window.alert("Please enter an album title");
        return;
      }

      if (tracks.length === 0) {
        window.alert("Please add at least one track");
        return;
      }

      for (let i = 0; i < tracks.length; i++) {
        const track = tracks[i];
        if (!track.title) {
          window.alert(`Please enter a title for Track #${i + 1}`);
          return;
        }
        if (!track.audioFile) {
          window.alert(`Please upload an audio file for Track #${i + 1}`);
          return;
        }
      }

      // Start submission process
      setIsSubmitting(true);
      setUploadProgress(0);

      const coverArtUrl = getGradientImageURL(currentGradient);

      // Parallel uploads with progress tracking
      const processedTracks = await Promise.all(
        tracks.map(async (track, index) => {
          try {
            // Update current uploading track
            setCurrentUploadingTrack({
              index: index + 1,
              title: track.title,
            });

            // Upload audio file
            const audioUrl = await uploadToCloudinary(track.audioFile);

            // Update progress
            setUploadProgress(((index + 1) / tracks.length) * 100);

            return {
              title: track.title,
              percentageDone: track.percentageDone || 0,
              description: track.description || "No description provided",
              deadline: track.deadline ? new Date(track.deadline) : null,
              coverArtUrl: track.coverArtUrl || coverArtUrl,
              audioUrl: audioUrl,
              creatorId: loggedInUser.id,
              instrumentIds: track.instrumentIds || [],
              isComplete: track.isComplete || false,
            };
          } catch (uploadError) {
            console.error(`Error uploading track ${track.title}:`, uploadError);
            throw uploadError;
          }
        })
      );

      // Prepare album data
      const albumWithTracksData = {
        album: {
          title: albumData.title,
          coverArtUrl: coverArtUrl,
          description: albumData.description,
          isComplete: albumData.isComplete,
          creatorId: loggedInUser.id,
        },
        tracks: processedTracks,
      };

      // Send to API
      await createAlbumWithTracks(albumWithTracksData);

      // Navigate on success
      navigate("/");
    } catch (error) {
      console.error("Error creating album:", error);
      window.alert(`Error creating album: ${error.message}`);
    } finally {
      // Reset submission state
      setIsSubmitting(false);
      setCurrentUploadingTrack(null);
      setUploadProgress(0);
    }
  };

  return (
    <div
      style={{
        backgroundColor: "#121212",
        color: "white",
        minHeight: "100vh",
        padding: "40px 20px",
      }}
    >
      <div className="container" style={{ maxWidth: "1000px" }}>
        <div className="row">
          {/* Album Cover and Title */}
          <div className="col-md-5 mb-4">
            <div
              style={{
                width: "100%",
                aspectRatio: "1/1",
                borderRadius: "12px",
                overflow: "hidden",
                marginBottom: "20px",
                background: currentGradient,
                position: "relative",
              }}
            >
              <Button
                color="light"
                size="sm"
                className="position-absolute m-3"
                onClick={() => {
                  const newGradient = generateRandomGradient();
                  setCurrentGradient(newGradient);
                }}
              >
                New Color
              </Button>
            </div>

            <FormGroup>
              <Input
                type="text"
                id="albumTitle"
                name="title"
                value={albumData.title}
                onChange={handleAlbumChange}
                className="form-control-lg border-0"
                style={{
                  backgroundColor: "transparent",
                  color: "white",
                  fontSize: "24px",
                  padding: "0",
                  outline: "none",
                  boxShadow: "none",
                }}
              />
              <div className="d-flex align-items-center mt-2">
                <small className="text-white-50">
                  {loggedInUser?.userName} • {tracks.length} tracks
                  {tracks.length > 0 && (
                    <>
                      {" "}
                      •{" "}
                      <span
                        className={
                          allTracksComplete ? "text-success" : "text-warning"
                        }
                      >
                        {allTracksComplete ? "Complete" : "In progress"}
                      </span>
                    </>
                  )}
                </small>
              </div>
            </FormGroup>
          </div>

          {/* Tracks Upload Area */}
          <div className="col-md-7">
            <div
              className={`upload-area p-4 ${isDraggingOver ? "active" : ""}`}
              style={{
                border: `2px dashed ${isDraggingOver ? "#ffffff" : "#666"}`,
                borderRadius: "12px",
                textAlign: "center",
                padding: "40px",
                marginBottom: "30px",
                backgroundColor: "#1a1a1a",
              }}
              onDragOver={handleDragOver}
              onDragLeave={handleDragLeave}
              onDrop={handleDrop}
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
                <h5>Upload or drop audio files</h5>
                <p className="text-white-50">WAV, MP3, AIFF and more</p>
              </div>
              <label
                className="btn btn-outline-light"
                style={{ cursor: "pointer" }}
              >
                Add tracks
                <Input
                  type="file"
                  id="audioFiles"
                  name="audioFiles"
                  accept="audio/*"
                  onChange={handleFileChange}
                  multiple
                  style={{ display: "none" }}
                />
              </label>
            </div>

            {/* Tracks List */}
            {tracks.length > 0 && (
              <div className="tracks-list mb-4">
                <h5 className="mb-3">Tracks</h5>
                {tracks.map((track, index) => (
                  <div
                    key={index}
                    className="track-item d-flex align-items-center p-3 mb-2 rounded"
                    style={{
                      backgroundColor: "#1a1a1a",
                    }}
                  >
                    <div
                      className="me-3 text-white-50"
                      style={{ width: "24px" }}
                    >
                      <span>{index + 1}</span>
                    </div>
                    <div className="flex-grow-1">
                      <Input
                        type="text"
                        value={track.title}
                        onChange={(e) =>
                          updateTrackTitle(index, e.target.value)
                        }
                        className="bg-transparent border-0 text-white"
                        style={{ outline: "none", boxShadow: "none" }}
                      />
                      <div className="d-flex align-items-center">
                        <div className="flex-grow-1 me-3">
                          <input
                            type="range"
                            min="0"
                            max="100"
                            value={track.percentageDone || 0}
                            onChange={(e) => {
                              const percentageDone = e.target.value;
                              const updatedTracks = [...tracks];
                              updatedTracks[index].percentageDone =
                                percentageDone;
                              updatedTracks[index].isComplete =
                                percentageDone === "100";
                              setTracks(updatedTracks);
                            }}
                            className="form-range"
                            style={{ height: "8px" }}
                          />
                        </div>
                        <span
                          className="text-white-50 small"
                          style={{ minWidth: "40px" }}
                        >
                          {track.percentageDone || 0}%
                        </span>
                      </div>
                      <p className="mb-0 text-white-50 small">
                        {track.audioFile?.name || "Audio file"} •{" "}
                        {track.isComplete ? "Complete" : "In progress"}
                      </p>
                    </div>
                    <button
                      className="btn btn-link text-white-50 p-1"
                      onClick={() => removeTrack(index)}
                      style={{ textDecoration: "none" }}
                      type="button"
                    >
                      <span>×</span>
                    </button>
                  </div>
                ))}
              </div>
            )}

            {/* Completion Status and Submit Buttons */}
            <div className="d-flex justify-content-between align-items-center mt-4">
              {tracks.length > 0 && (
                <div>
                  <div className="d-flex align-items-center">
                    <div
                      className="me-2"
                      style={{
                        width: "16px",
                        height: "16px",
                        borderRadius: "50%",
                        backgroundColor: allTracksComplete
                          ? "#28a745"
                          : "#ffc107",
                      }}
                    ></div>
                    <span>
                      {allTracksComplete
                        ? "All tracks complete"
                        : "Some tracks still in progress"}
                    </span>
                  </div>
                </div>
              )}

              <div className="d-flex gap-2">
                <Button
                  color="secondary"
                  outline
                  onClick={() => navigate("/")}
                  disabled={isSubmitting}
                  style={{ borderColor: "#666" }}
                  type="button"
                >
                  Cancel
                </Button>
                <Button
                  color="light"
                  onClick={handleSubmit}
                  disabled={isSubmitting}
                  type="button"
                >
                  {isSubmitting ? "Creating Album..." : "Create Album"}
                </Button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}
