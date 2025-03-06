import { useState } from "react";
import { register } from "../../managers/authManager";
import { Link, useNavigate } from "react-router-dom";
import {
  Button,
  FormFeedback,
  FormGroup,
  Input,
  Label,
  Alert,
} from "reactstrap";

export default function Register({ setLoggedInUser }) {
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [userName, setUserName] = useState("");
  const [email, setEmail] = useState("");
  const [address, setAddress] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [isArtist, setIsArtist] = useState(false);
  const [profileImage, setProfileImage] = useState(
    "https://example.com/default-profile.jpg"
  );

  const [passwordMismatch, setPasswordMismatch] = useState(false);
  const [error, setError] = useState("");
  const [isSubmitting, setIsSubmitting] = useState(false);

  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");

    // Validate form
    if (
      !firstName ||
      !lastName ||
      !userName ||
      !email ||
      !password ||
      !confirmPassword
    ) {
      setError("All fields are required");
      return;
    }

    if (password !== confirmPassword) {
      setPasswordMismatch(true);
      setError("Passwords do not match");
      return;
    }

    // Construct user object with PascalCase property names to match C# DTO
    const newUser = {
      FirstName: firstName,
      LastName: lastName,
      UserName: userName,
      Email: email,
      Password: password,
      IsArtist: isArtist,
      ProfileImage: profileImage,
      Address: address,
    };

    try {
      setIsSubmitting(true);
      const user = await register(newUser);
      if (user) {
        setLoggedInUser(user);
        navigate("/");
      } else {
        setError("Registration failed for an unknown reason");
      }
    } catch (err) {
      console.error("Registration error:", err);
      setError(err.message || "Failed to register. Please try again.");
    } finally {
      setIsSubmitting(false);
    }
  };

  return (
    <div className="container" style={{ maxWidth: "500px" }}>
      <h3>Sign Up</h3>

      {error && <Alert color="danger">{error}</Alert>}

      <form onSubmit={handleSubmit}>
        <FormGroup>
          <Label>First Name</Label>
          <Input
            type="text"
            value={firstName}
            onChange={(e) => setFirstName(e.target.value)}
            invalid={error && !firstName}
            required
          />
          {error && !firstName && (
            <FormFeedback>First name is required</FormFeedback>
          )}
        </FormGroup>

        <FormGroup>
          <Label>Last Name</Label>
          <Input
            type="text"
            value={lastName}
            onChange={(e) => setLastName(e.target.value)}
            invalid={error && !lastName}
            required
          />
          {error && !lastName && (
            <FormFeedback>Last name is required</FormFeedback>
          )}
        </FormGroup>

        <FormGroup>
          <Label>Email</Label>
          <Input
            type="email"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            invalid={error && !email}
            required
          />
          {error && !email && <FormFeedback>Email is required</FormFeedback>}
        </FormGroup>

        <FormGroup>
          <Label>User Name</Label>
          <Input
            type="text"
            value={userName}
            onChange={(e) => setUserName(e.target.value)}
            invalid={error && !userName}
            required
          />
          {error && !userName && (
            <FormFeedback>Username is required</FormFeedback>
          )}
        </FormGroup>

        <FormGroup>
          <Label>Address</Label>
          <Input
            type="text"
            value={address}
            onChange={(e) => setAddress(e.target.value)}
          />
        </FormGroup>

        <FormGroup check className="mb-3">
          <Input
            type="checkbox"
            id="isArtist"
            checked={isArtist}
            onChange={(e) => setIsArtist(e.target.checked)}
          />
          <Label for="isArtist" check>
            Register as Artist
          </Label>
        </FormGroup>

        <FormGroup>
          <Label>Profile Image URL</Label>
          <Input
            type="text"
            value={profileImage}
            onChange={(e) => setProfileImage(e.target.value)}
            required
          />
        </FormGroup>

        <FormGroup>
          <Label>Password</Label>
          <Input
            type="password"
            value={password}
            onChange={(e) => {
              setPassword(e.target.value);
              setPasswordMismatch(false);
            }}
            invalid={passwordMismatch || (error && !password)}
            required
          />
          {error && !password && (
            <FormFeedback>Password is required</FormFeedback>
          )}
        </FormGroup>

        <FormGroup>
          <Label>Confirm Password</Label>
          <Input
            type="password"
            value={confirmPassword}
            onChange={(e) => {
              setConfirmPassword(e.target.value);
              setPasswordMismatch(false);
            }}
            invalid={passwordMismatch || (error && !confirmPassword)}
            required
          />
          {passwordMismatch && (
            <FormFeedback>Passwords do not match!</FormFeedback>
          )}
          {error && !confirmPassword && !passwordMismatch && (
            <FormFeedback>Please confirm your password</FormFeedback>
          )}
        </FormGroup>

        <Button
          color="primary"
          type="submit"
          disabled={isSubmitting || passwordMismatch}
        >
          {isSubmitting ? "Registering..." : "Register"}
        </Button>
      </form>

      <p className="mt-3">
        Already signed up? Log in <Link to="/login">here</Link>
      </p>
    </div>
  );
}
