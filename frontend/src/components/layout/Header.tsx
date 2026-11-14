import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
  ArrowUpLeft,
  ArrowUpRight,
  CalendarPlus,
  ChevronDown,
  ClipboardList,
  HeartPulse,
  Menu,
  X,
  LogOut,
  UserRound,
} from "lucide-react";

import { useAuth } from "../../context/AuthContext";

export default function Header() {
  const { t, i18n } = useTranslation();
  const { auth, isAuthenticated, logoutUser } = useAuth();
  const navigate = useNavigate();

  const [menuOpen, setMenuOpen] = useState(false);

  const isArabic = i18n.language === "ar";
  const Arrow = isArabic ? ArrowUpLeft : ArrowUpRight;

  const closeMenu = () => setMenuOpen(false);

  const toggleLanguage = () => {
    i18n.changeLanguage(isArabic ? "en" : "ar");
    closeMenu();
  };

  const handleLogout = () => {
    logoutUser();
    closeMenu();
    navigate("/", { replace: true });
  };

  const customerName = auth
    ? `${auth.firstName} ${auth.lastName}`
    : "";

  return (
    <header className="navbar">
      <div className="container nav-inner">
        <Link to="/" className="brand" onClick={closeMenu}>
          <span className="brand-symbol">
            <HeartPulse size={25} strokeWidth={2.2} />
          </span>
          <span className="brand-copy">
            <strong>{t("brand")}</strong>
            <small>{t("brandTagline")}</small>
          </span>
        </Link>

        <nav className={`nav-links ${menuOpen ? "nav-open" : ""}`}>
          <a href="/#home" onClick={closeMenu}>
            {t("nav.home")}
          </a>
          <a href="/#services" onClick={closeMenu}>
            {t("nav.services")}
          </a>
          <a href="/#about" onClick={closeMenu}>
            {t("nav.about")}
          </a>
          <a href="/#contact" onClick={closeMenu}>
            {t("nav.contact")}
          </a>

          {isAuthenticated ? (
            <>
              <Link to="/booking" onClick={closeMenu}>
                <CalendarPlus size={16} />
                {t("booking.title")}
              </Link>

              <Link to="/my-bookings" onClick={closeMenu}>
                <ClipboardList size={16} />
                {t("myBookings.title")}
              </Link>

              <span className="mobile-customer-name">
                <UserRound size={16} />
                {customerName}
              </span>

              <button
                className="mobile-auth-link"
                onClick={handleLogout}
              >
                <LogOut size={16} />
                {t("auth.logout")}
              </button>
            </>
          ) : (
            <>
              <Link to="/login" onClick={closeMenu}>
                {t("auth.login")}
              </Link>
              <Link to="/register" onClick={closeMenu}>
                {t("auth.register")}
              </Link>
            </>
          )}

          <button className="mobile-language" onClick={toggleLanguage}>
            {isArabic ? "English" : "العربية"}
          </button>
        </nav>

        <div className="nav-actions">
          <button className="language-button" onClick={toggleLanguage}>
            <span>?</span>
            {t("language")}
            <ChevronDown size={14} />
          </button>

          {isAuthenticated ? (
            <div className="nav-authenticated">
              <Link
                to="/booking"
                className="nav-customer-link"
              >
                <CalendarPlus size={16} />
                {t("booking.title")}
              </Link>

              <Link
                to="/my-bookings"
                className="nav-customer-link"
              >
                <ClipboardList size={16} />
                {t("myBookings.title")}
              </Link>

              <span className="nav-customer-name">
                <UserRound size={16} />
                {auth?.firstName}
              </span>

              <button
                className="button button-dark nav-cta"
                onClick={handleLogout}
              >
                {t("auth.logout")}
                <LogOut size={16} />
              </button>
            </div>
          ) : (
            <div className="nav-auth-links">
              <Link to="/login" className="nav-login-link">
                {t("auth.login")}
              </Link>

              <Link
                to="/register"
                className="button button-dark nav-cta"
              >
                {t("auth.register")}
                <Arrow size={16} />
              </Link>
            </div>
          )}
        </div>

        <button
          className="menu-button"
          onClick={() => setMenuOpen(!menuOpen)}
          aria-label="Toggle navigation"
          aria-expanded={menuOpen}
        >
          {menuOpen ? <X /> : <Menu />}
        </button>
      </div>
    </header>
  );
}
