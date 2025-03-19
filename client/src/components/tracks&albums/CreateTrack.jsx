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
import { createTrack, getTracksByUserId } from "../../managers/trackManager";
import { getAllInstruments } from "../../managers/instrumentManager";
import { getAllInstrumentCategories } from "../../managers/instrumentCategoryManager";
import { uploadToCloudinary } from "../../managers/cloudinaryManager";
import { useNavigate } from "react-router-dom";

export default function CreateTrack({ loggedInUser }) {
  const [instruments, setInstruments] = useState([]);
  const [instrumentCategories, setInstrumentCategories] = useState([]);
  const [formData, setFormData] = useState({
    title: "",
    percentageDone: "",
    description: "",
    deadline: "",
    coverArtUrl: "",
  });
  const [selectedInstrumentCategory, setSelectedInstrumentCategory] =
    useState(0);
  const [selectedInstruments, setSelectedInstruments] = useState([]);

  const navigate = useNavigate();

  useEffect(() => {
    getTracksByUserId(loggedInUser.id).then((t) => {
      const trackNumber = `Track #${t.length}`;

      const baseFormData = { ...formData };

      baseFormData.title = trackNumber;
      baseFormData.coverArtUrl = "https://picsum.photos/seed/default/300/300";

      setFormData(baseFormData);

      getAllInstruments().then(setInstruments);
      getAllInstrumentCategories().then(setInstrumentCategories);
    });
  }, [loggedInUser]);

  const handleChange = (e) => {
    const fieldName = e.target.name;
    const newValue = e.target.value;

    const updatedFormData = { ...formData };

    if (fieldName === "title") {
      updatedFormData.title = newValue;
    } else if (fieldName === "percentageDone") {
      updatedFormData.percentageDone = newValue;
    } else if (fieldName === "description") {
      updatedFormData.description = newValue;
    } else if (fieldName === "deadline") {
      updatedFormData.deadline = newValue;
    }

    setFormData(updatedFormData);
  };

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

  // Filter instruments by the selected category
  const filteredInstruments = selectedInstrumentCategory
    ? instruments.filter(
        (instrument) => instrument.categoryId === selectedInstrumentCategory
      )
    : [];

  const handleSubmit = async (e) => {
    e.preventDefault();

    // required fields
    if (!formData.title) {
      window.alert("Please enter a track title");
      return;
    }

    if (!formData.audioFile) {
      window.alert("Please upload an audio file");
      return;
    }

    try {
      // Upload audio file if one exists
      let audioUrl = null;
      if (formData.audioFile) {
        audioUrl = await uploadToCloudinary(formData.audioFile);
      }

      // creating data object for backend with the audio url from cloudinary
      const trackData = {
        title: formData.title,
        description: formData.description,
        percentageDone: formData.percentageDone,
        deadline: formData.deadline ? new Date(formData.deadline) : null,
        coverArtUrl: formData.coverArtUrl,
        audioUrl: audioUrl,
        creatorId: loggedInUser.id,
        instrumentIds: selectedInstruments,
      };

      // Send to API
      await createTrack(trackData).then(() => {
        console.log("Track created with data:", trackData);
        navigate("/");
      });
    } catch (error) {
      console.error("Error creating track:", error);
    }
  };

  const handleFileChange = (e) => {
    const file = e.target.files[0];
    if (file) {
      // storing the audio so there can be a preview and can be sent to cloudinary
      const updatedFormData = { ...formData };
      updatedFormData.audioFile = file;
      setFormData(updatedFormData);
    }
  };

  return (
    <div
      className="container py-5"
      style={{ maxWidth: "900px", paddingBottom: "100px" }}
    >
      <Form onSubmit={handleSubmit}>
        <Row className="mb-4">
          <Col md={4} className="mb-4 mb-md-0">
            <div className="position-relative" style={{ aspectRatio: "1/1" }}>
              <img
                src={"https://picsum.photos/seed/default/300/300"}
                alt={`Cover art for new track`}
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
            {formData.audioFile && (
              <div className="mb-3">
                <audio
                  controls
                  src={URL.createObjectURL(formData.audioFile)}
                  className="w-100"
                />
              </div>
            )}
            <div className="mt-4">
              <h5 className="mb-2">Instrument Categories</h5>
              <div className="d-flex flex-wrap gap-1">
                {instrumentCategories.map((t) => (
                  <Card
                    className={`text-center tag-choice p-0 ${
                      t.id === selectedInstrumentCategory && "tagChosen"
                    }`}
                    onClick={() => {
                      if (selectedInstrumentCategory === t.id) {
                        setSelectedInstrumentCategory(0);
                      } else {
                        setSelectedInstrumentCategory(t.id);
                      }
                    }}
                    key={t.id}
                    style={{
                      cursor: "pointer",
                      maxWidth: "fit-content",
                      backgroundColor:
                        t.id === selectedInstrumentCategory ? "#6c757d" : "",
                      color: t.id === selectedInstrumentCategory ? "white" : "",
                    }}
                  >
                    <CardBody
                      className="py-1 px-2"
                      style={{ fontSize: "0.8rem" }}
                    >
                      {t.name}
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
                          onClick={() => handleInstrumentClick(instrument.id)}
                          key={instrument.id}
                          style={{
                            cursor: "pointer",
                            maxWidth: "fit-content",
                            backgroundColor: selectedInstruments.includes(
                              instrument.id
                            )
                              ? "#6c757d"
                              : "#e9ecef",
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
            </div>
          </Col>
          {/* Track Title Input */}
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
                value={formData.title}
                onChange={handleChange}
                className="mb-3"
              />
            </FormGroup>
            {/* Completion Percentage Input */}
            <FormGroup>
              <Label for="percentageDone" className="fw-bold">
                Completion (%)
              </Label>
              <Input
                type="number"
                id="percentageDone"
                name="percentageDone"
                placeholder="Enter percentage complete"
                value={formData.percentageDone}
                onChange={handleChange}
                min="0"
                max="100"
                className="mb-3"
              />
            </FormGroup>
            {/* Deadline DATE Input */}
            <FormGroup>
              <Label for="deadline" className="fw-bold">
                Deadline
              </Label>
              <Input
                type="date"
                id="deadline"
                name="deadline"
                value={formData.deadline}
                onChange={handleChange}
                className="mb-3"
              />
            </FormGroup>
          </Col>
        </Row>
        {/* Cancel and Create Btns */}
        <div className="d-flex justify-content-end gap-2">
          <Button color="secondary" outline>
            Cancel
          </Button>
          <Button color="primary" type="submit">
            Create Track
          </Button>
        </div>
      </Form>
    </div>
  );
}
