import { useTranslation } from "react-i18next";
import { HeartPulse, ArrowUpRight, ArrowUpLeft } from "lucide-react";
import { Link } from "react-router-dom";

export default function Footer() {
  const { t, i18n } = useTranslation();
  const isArabic = i18n.language.startsWith("ar");
  const Arrow = isArabic ? ArrowUpLeft : ArrowUpRight;

  return (
    <footer className="footer footer-editorial">
      <div className="container">
        <div className="footer-editorial-main">
          <div className="footer-editorial-brand">
            <Link to="/#home" className="brand brand-footer">
              <span className="brand-logo">
                <img src="/care-plus-logo.png" alt="Care Plus" />
              </span>
              <span className="brand-copy">
                <strong>{t("brand")}</strong>
                <small>{t("brandTagline")}</small>
              </span>
            </Link>

            <p>{t("footer.description")}</p>

            <Link to="/#contact" className="footer-contact-link">
              {isArabic ? "تواصل مع فريقنا" : "Get in touch"}
              <Arrow size={16} />
            </Link>
          </div>

          <div className="footer-editorial-column">
            <h4>{t("footer.links")}</h4>
            <Link to="/#home">{t("nav.home")}</Link>
            <Link to="/#services">{t("nav.services")}</Link>
            <Link to="/about">{t("nav.about")}</Link>
            <Link to="/#process">{t("process.title")}</Link>
            <Link to="/#contact">{t("nav.contact")}</Link>
          </div>

          <div className="footer-editorial-column">
            <h4>{isArabic ? "خدماتنا" : "Our services"}</h4>
            <Link to="/#services">{t("services.home.title")}</Link>
            <Link to="/#services">{t("services.physio.title")}</Link>
            <Link to="/#services">{t("services.medical.title")}</Link>
            <Link to="/#services">{t("services.all")}</Link>
          </div>

          <div className="footer-editorial-note">
            <span className="footer-editorial-icon">
              <HeartPulse size={27} />
            </span>
            <h3>{t("footer.tagline")}</h3>
            <p>{t("hero.note")}</p>
            <Link to="/login" className="footer-login-link">
              {isArabic ? "تسجيل الدخول" : "Sign in"}
              <Arrow size={16} />
            </Link>
          </div>
        </div>

        <div className="footer-editorial-bottom">
          <span>
            © {new Date().getFullYear()} Care Plus. {t("footer.rights")}
          </span>
          <Link to="/#home">
            {isArabic ? "العودة إلى الأعلى ↑" : "Back to top ↑"}
          </Link>
        </div>
      </div>
    </footer>
  );
}



