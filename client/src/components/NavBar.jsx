import { useState } from "react";
import { NavLink as RRNavLink } from "react-router-dom";
import {
  Button,
  Collapse,
  Nav,
  NavLink,
  NavItem,
  Navbar,
  NavbarBrand,
  NavbarToggler,
} from "reactstrap";
import { logout } from "../managers/authManager";
import "./styles/navbar.css";

export default function NavBar({ loggedInUser, setLoggedInUser }) {
  const [open, setOpen] = useState(false);

  const toggleNavbar = () => setOpen(!open);

  return (
    <div className="navbar-wrapper mb-5">
      <div className="navbar-content">
        <div className="navbar-brand">
          <NavLink tag={RRNavLink} to="/">
            {"[demos]"}
          </NavLink>
        </div>

        <div className="navbar-controls">
          {loggedInUser ? (
            <>
              <button className="menu-dots-button" onClick={toggleNavbar}>
                ...
              </button>

              {open && (
                <div className="logout-dropdown">
                  <Button
                    color="primary"
                    onClick={(e) => {
                      e.preventDefault();
                      setOpen(false);
                      logout().then(() => {
                        setLoggedInUser(null);
                      });
                    }}
                  >
                    Logout
                  </Button>
                </div>
              )}
            </>
          ) : (
            ""
          )}
        </div>
      </div>
    </div>
  );
}
