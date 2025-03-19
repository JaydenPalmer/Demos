import {
  Col,
  Form,
  Input,
  Row,
  FormGroup,
  Label,
  Button,
  CardBody,
  Card,
} from "reactstrap";
import { useEffect, useState } from "react";
// import { createTrack } from "../../managers/trackManager";
// import { createAlbum } from "../../managers/albumManager";
import { getAllInstruments } from "../../managers/instrumentManager";
import { getAllInstrumentCategories } from "../../managers/instrumentCategoryManager";
import { uploadToCloudinary } from "../../managers/cloudinaryManager";
import { useNavigate } from "react-router-dom";

export default function CreateAlbum({ loggedInUser }) {
  const [instruments, setInstruments] = useState([]);
  const [instrumentCategories, setInstrumentCategories] = useState([]);
  const [albumData, setAlbumData] = useState({
    title: "",
    coverArtUrl: "https://picsum.photos/seed/default/300/300",
    isComplete: false,
  });
  const [tracks, setTracks] = useState([
    {
      title: "Track #1",
      description: "",
      percentageDone: "",
      deadline: "",
      coverArtUrl: "",
      audioFile: null,
      instrumentIds: [],
    },
  ]);
  const [selectedInstrumentCategory, setSelectedInstrumentCategory] =
    useState(0);
  const [activeTrackIndex, setActiveTrackIndex] = useState(0);

  const navigate = useNavigate();

  useEffect(() => {
    getAllInstruments().then(setInstruments);
    getAllInstrumentCategories().then(setInstrumentCategories);
  }, [loggedInUser]);

  const handleAlbumChange = (e) => {
    const { name, value, type, checked } = e.target;
    setAlbumData({
      ...albumData,
      [name]: type === "checkbox" ? checked : value,
    });
  };

  const handleTrackChange = (e) => {
    const { name, value } = e.target;
    const updatedTracks = [...tracks];
    updatedTracks[activeTrackIndex] = {
      ...updatedTracks[activeTrackIndex],
      [name]: value,
    };
    setTracks(updatedTracks);
  };

  const handleInstrumentClick = (instrumentId) => {
    const updatedTracks = [...tracks];
    const currentTrack = updatedTracks[activeTrackIndex];

    if (currentTrack.instrumentIds.includes(instrumentId)) {
      currentTrack.instrumentIds = currentTrack.instrumentIds.filter(
        (id) => id !== instrumentId
      );
    } else {
      currentTrack.instrumentIds.push(instrumentId);
    }

    setTracks(updatedTracks);
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      const updatedTracks = [...tracks];
      updatedTracks[activeTrackIndex].audioFile = file;
      setTracks(updatedTracks);
    }
  };

  const addTrack = () => {
    setTracks([
      ...tracks,
      {
        title: `Track #${tracks.length + 1}`,
        description: "",
        percentageDone: "",
        deadline: "",
        coverArtUrl: "",
        audioFile: null,
        instrumentIds: [],
      },
    ]);
    setActiveTrackIndex(tracks.length);

    const fileInput = document.getElementById("audioFile");
    if (fileInput) {
      fileInput.value = "";
    }
  };

  const removeTrack = (index) => {
    if (tracks.length > 1) {
      const newTracks = [];
      for (let i = 0; i < tracks.length; i++) {
        if (i !== index) {
          newTracks.push(tracks[i]);
        }
      }

      setTracks(newTracks);

      if (activeTrackIndex === index) {
        if (index > 0) {
          setActiveTrackIndex(index - 1);
        } else {
          setActiveTrackIndex(0);
        }
      } else if (activeTrackIndex > index) {
        setActiveTrackIndex(activeTrackIndex - 1);
      }
    }
  };

  // Filter instruments by the selected category
  const filteredInstruments = selectedInstrumentCategory
    ? instruments.filter(
        (instrument) => instrument.categoryId === selectedInstrumentCategory
      )
    : [];

  const handleSubmit = async (e) => {
    e.preventDefault();

    // Validate album data
    if (!albumData.title) {
      window.alert("Please enter an album title");
      return;
    }

    // Validate track data
    for (let i = 0; i < tracks.length; i++) {
      const track = tracks[i];
      if (!track.title) {
        window.alert(`Please enter a title for Track #${i + 1}`);
        setActiveTrackIndex(i);
        return;
      }
      if (!track.audioFile) {
        window.alert(`Please upload an audio file for Track #${i + 1}`);
        setActiveTrackIndex(i);
        return;
      }
    }

    try {
      // First create the album
      const album = await createAlbum({
        title: albumData.title,
        coverArtUrl: albumData.coverArtUrl,
        isComplete: albumData.isComplete,
      });

      // Then create all tracks
      for (let i = 0; i < tracks.length; i++) {
        const track = tracks[i];

        // Upload audio file to Cloudinary
        const audioUrl = await uploadToCloudinary(track.audioFile);

        // Create track with album reference
        await createTrack({
          title: track.title,
          description: track.description,
          percentageDone: track.percentageDone,
          deadline: track.deadline ? new Date(track.deadline) : null,
          coverArtUrl: track.coverArtUrl || albumData.coverArtUrl,
          audioUrl: audioUrl,
          creatorId: loggedInUser.id,
          instrumentIds: track.instrumentIds,
          albumId: album.id,
          trackOrder: i + 1,
        });
      }

      // Navigate back to the home page after successful creation
      navigate("/");
    } catch (error) {
      console.error("Error creating album:", error);
    }
  };

  return (
    <div
      className="container py-5"
      style={{ maxWidth: "900px", paddingBottom: "100px" }}
    >
      <h2 className="mb-4">Create New Album</h2>

      {/* Album Information Form */}
      <div className="mb-4">
        <h4 className="mb-3">Album Details</h4>
        <Row>
          <Col md={4} className="mb-4 mb-md-0">
            <div className="position-relative" style={{ aspectRatio: "1/1" }}>
              <img
                src={
                  albumData.coverArtUrl ||
                  "https://picsum.photos/seed/default/300/300"
                }
                alt="Album cover art"
                style={{
                  width: "100%",
                  height: "100%",
                  objectFit: "cover",
                  borderRadius: "8px",
                  boxShadow: "0 4px 12px rgba(0,0,0,0.1)",
                }}
              />
              <Button
                color="primary"
                size="sm"
                className="position-absolute bottom-0 end-0 m-2"
                style={{ opacity: 0.9 }}
                onClick={() => {
                  const url = prompt(
                    "Enter cover art URL:",
                    albumData.coverArtUrl
                  );
                  if (url) {
                    setAlbumData({ ...albumData, coverArtUrl: url });
                  }
                }}
              >
                Change Cover
              </Button>
            </div>
          </Col>
          <Col md={8}>
            <FormGroup>
              <Label for="albumTitle" className="fw-bold">
                Album Title
              </Label>
              <Input
                type="text"
                id="albumTitle"
                name="title"
                placeholder="Enter album title"
                value={albumData.title}
                onChange={handleAlbumChange}
                className="mb-3"
              />
            </FormGroup>
            <FormGroup check className="mb-3">
              <Input
                type="checkbox"
                id="isComplete"
                name="isComplete"
                checked={albumData.isComplete}
                onChange={handleAlbumChange}
              />
              <Label for="isComplete" check>
                Mark album as complete
              </Label>
            </FormGroup>
          </Col>
        </Row>
      </div>

      {/* Track Management */}
      <div className="mb-4">
        <div className="d-flex justify-content-between align-items-center mb-3">
          <h4>Tracks ({tracks.length})</h4>
          <Button color="primary" onClick={addTrack}>
            Add Track
          </Button>
        </div>

        {/* Track Selector */}
        <div className="d-flex flex-wrap gap-2 mb-4">
          {tracks.map((track, index) => (
            <Card
              key={index}
              className="p-0"
              style={{
                cursor: "pointer",
                backgroundColor:
                  activeTrackIndex === index ? "#6c757d" : "#e9ecef",
                color: activeTrackIndex === index ? "white" : "black",
                position: "relative",
              }}
              onClick={() => setActiveTrackIndex(index)}
            >
              <CardBody className="py-2 px-3">
                {track.title || `Track #${index + 1}`}
                {tracks.length > 1 && (
                  <Button
                    close
                    size="sm"
                    className="ms-2"
                    onClick={(e) => {
                      e.stopPropagation();
                      removeTrack(index);
                    }}
                  />
                )}
              </CardBody>
            </Card>
          ))}
        </div>

        {/* Active Track Form */}
        {tracks.length > 0 && (
          <Form onSubmit={handleSubmit}>
            <Row className="mb-4">
              <Col md={4} className="mb-4 mb-md-0">
                <div
                  className="position-relative"
                  style={{ aspectRatio: "1/1" }}
                >
                  <img
                    src={
                      tracks[activeTrackIndex].coverArtUrl ||
                      albumData.coverArtUrl ||
                      "https://picsum.photos/seed/default/300/300"
                    }
                    alt={`Cover art for ${tracks[activeTrackIndex].title}`}
                    style={{
                      width: "100%",
                      height: "100%",
                      objectFit: "cover",
                      borderRadius: "8px",
                      boxShadow: "0 4px 12px rgba(0,0,0,0.1)",
                    }}
                  />
                  <Button
                    color="primary"
                    size="sm"
                    className="position-absolute bottom-0 end-0 m-2"
                    style={{ opacity: 0.9 }}
                    onClick={() => {
                      const url = prompt(
                        "Enter track cover art URL (leave empty to use album cover):",
                        tracks[activeTrackIndex].coverArtUrl
                      );
                      const updatedTracks = [...tracks];
                      updatedTracks[activeTrackIndex].coverArtUrl = url;
                      setTracks(updatedTracks);
                    }}
                  >
                    Change Cover
                  </Button>
                </div>
                <FormGroup>
                  <Label for="audioFile" className="fw-bold mt-2">
                    Upload Audio
                  </Label>
                  <Input
                    type="file"
                    id="audioFile"
                    name="audioFile"
                    accept="audio/*"
                    onChange={handleFileChange}
                    className="mb-3"
                  />
                </FormGroup>
                {tracks[activeTrackIndex].audioFile && (
                  <div className="mb-3">
                    <audio
                      controls
                      src={URL.createObjectURL(
                        tracks[activeTrackIndex].audioFile
                      )}
                      className="w-100"
                    />
                  </div>
                )}
                <div className="mt-4">
                  <h5 className="mb-2">Instrument Categories</h5>
                  <div className="d-flex flex-wrap gap-1">
                    {instrumentCategories.map((category) => (
                      <Card
                        className={`text-center tag-choice p-0 ${
                          category.id === selectedInstrumentCategory &&
                          "tagChosen"
                        }`}
                        onClick={() => {
                          if (selectedInstrumentCategory === category.id) {
                            setSelectedInstrumentCategory(0);
                          } else {
                            setSelectedInstrumentCategory(category.id);
                          }
                        }}
                        key={category.id}
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
                    <div className="mt-3">
                      <h6 className="mb-2">Available Instruments</h6>
                      <div className="d-flex flex-wrap gap-1">
                        {filteredInstruments.length > 0 ? (
                          filteredInstruments.map((instrument) => (
                            <Card
                              className="text-center p-0"
                              onClick={() =>
                                handleInstrumentClick(instrument.id)
                              }
                              key={instrument.id}
                              style={{
                                cursor: "pointer",
                                maxWidth: "fit-content",
                                backgroundColor: tracks[
                                  activeTrackIndex
                                ].instrumentIds.includes(instrument.id)
                                  ? "#6c757d"
                                  : "#e9ecef",
                                color: tracks[
                                  activeTrackIndex
                                ].instrumentIds.includes(instrument.id)
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
                </div>
              </Col>
              {/* Track Details Input */}
              <Col md={8}>
                <FormGroup>
                  <Label for="title" className="fw-bold">
                    Track Title
                  </Label>
                  <Input
                    type="text"
                    id="title"
                    name="title"
                    placeholder="Enter track title"
                    value={tracks[activeTrackIndex].title}
                    onChange={handleTrackChange}
                    className="mb-3"
                  />
                </FormGroup>
                <FormGroup>
                  <Label for="description" className="fw-bold">
                    Description
                  </Label>
                  <Input
                    type="textarea"
                    id="description"
                    name="description"
                    placeholder="Enter track description"
                    value={tracks[activeTrackIndex].description}
                    onChange={handleTrackChange}
                    className="mb-3"
                  />
                </FormGroup>
                <FormGroup>
                  <Label for="percentageDone" className="fw-bold">
                    Completion (%)
                  </Label>
                  <Input
                    type="number"
                    id="percentageDone"
                    name="percentageDone"
                    placeholder="Enter percentage complete"
                    value={tracks[activeTrackIndex].percentageDone}
                    onChange={handleTrackChange}
                    min="0"
                    max="100"
                    className="mb-3"
                  />
                </FormGroup>
                <FormGroup>
                  <Label for="deadline" className="fw-bold">
                    Deadline
                  </Label>
                  <Input
                    type="date"
                    id="deadline"
                    name="deadline"
                    value={tracks[activeTrackIndex].deadline}
                    onChange={handleTrackChange}
                    className="mb-3"
                  />
                </FormGroup>
              </Col>
            </Row>
            {/* Cancel and Create Btns */}
            <div className="d-flex justify-content-end gap-2 mt-4">
              <Button color="secondary" outline onClick={() => navigate("/")}>
                Cancel
              </Button>
              <Button color="primary" type="submit">
                Create Album
              </Button>
            </div>
          </Form>
        )}
      </div>
    </div>
  );
}
