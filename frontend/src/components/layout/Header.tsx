import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
  ArrowUpLeft,
  ArrowUpRight,
  CalendarDays,
  ClipboardList,
  LayoutDashboard,
  Languages,
  LogOut,
  Menu,
  UserRound,
  X,
} from "lucide-react";

import { useAuth } from "../../context/AuthContext";

export default function Header() {
  const { t, i18n } = useTranslation();
  const { auth, isAuthenticated, logoutUser } = useAuth();
  const navigate = useNavigate();
  const [menuOpen, setMenuOpen] = useState(false);

  const isArabic = i18n.language === "ar";
  const Arrow = isArabic ? ArrowUpLeft : ArrowUpRight;
  const isAdmin = auth?.role?.toLowerCase() === "admin";

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

  const adminLabel = isArabic ? "لوحة التحكم" : "Admin Dashboard";
  const languageLabel = isArabic ? "English" : "العربية";

  return (
    <header className="navbar">
      <div className="container nav-inner">
        <Link to="/" className="brand" onClick={closeMenu}>
          <span className="brand-logo">
            <img src="/care-plus-logo.png" alt="Care Plus" />
          </span>
          <span className="brand-copy">
            <strong>{t("brand")}</strong>
            <small>{t("brandTagline")}</small>
          </span>
        </Link>

        <nav className={`nav-links ${menuOpen ? "nav-open" : ""}`}>
          <a href="/#home" onClick={closeMenu}>{t("nav.home")}</a>
          <a href="/#services" onClick={closeMenu}>{t("nav.services")}</a>
          <Link to="/about" onClick={closeMenu}>{t("nav.about")}</Link>
          <a href="/#contact" onClick={closeMenu}>{t("nav.contact")}</a>

          {isAuthenticated ? (
            <>
              {isAdmin && (
                <Link to="/admin" onClick={closeMenu}>
                  <LayoutDashboard size={15} />
                  {adminLabel}
                </Link>
              )}

              <Link to="/booking" onClick={closeMenu}>
                <CalendarDays size={15} />
                {t("booking.title")}
              </Link>

              <Link to="/my-bookings" onClick={closeMenu}>
                <ClipboardList size={15} />
                {t("myBookings.title")}
              </Link>

              <span className="mobile-customer-name">
                <UserRound size={15} />
                {auth?.firstName}
              </span>

              <button className="mobile-auth-link" onClick={handleLogout}>
                <LogOut size={15} />
                {t("auth.logout")}
              </button>
            </>
          ) : (
            <>
              <Link to="/login" onClick={closeMenu}>{t("auth.login")}</Link>
              <Link to="/register" onClick={closeMenu}>{t("auth.register")}</Link>
            </>
          )}

          <button className="mobile-language" onClick={toggleLanguage}>
            <Languages size={15} />
            {languageLabel}
          </button>
        </nav>

        <div className="nav-actions">
          <button className="language-button" onClick={toggleLanguage}>
            <Languages size={16} />
            <span>{languageLabel}</span>
          </button>

          {isAuthenticated ? (
            <div className="nav-authenticated">
              {isAdmin && (
                <Link to="/admin" className="nav-customer-link">
                  <LayoutDashboard size={15} />
                  {adminLabel}
                </Link>
              )}

              <Link to="/booking" className="nav-customer-link">
                <CalendarDays size={15} />
                {t("booking.title")}
              </Link>

              <Link to="/my-bookings" className="nav-customer-link">
                <ClipboardList size={15} />
                {t("myBookings.title")}
              </Link>

              <span className="nav-customer-name">
                <UserRound size={15} />
                {auth?.firstName}
              </span>

              <button className="button button-dark nav-cta" onClick={handleLogout}>
                {t("auth.logout")}
                <LogOut size={15} />
              </button>
            </div>
          ) : (
            <div className="nav-auth-links">
              <Link to="/login" className="nav-login-link">{t("auth.login")}</Link>
              <Link to="/register" className="button button-dark nav-cta">
                {t("auth.register")}
                <Arrow size={15} />
              </Link>
            </div>
          )}
        </div>

        <button
          className="menu-button"
          onClick={() => setMenuOpen((open) => !open)}
          aria-label="Toggle navigation"
          aria-expanded={menuOpen}
        >
          {menuOpen ? <X size={21} /> : <Menu size={21} />}
        </button>
      </div>
    </header>
  );
}

