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
            <a href="#home" className="brand brand-footer">
              <span className="brand-logo">
                <img src="/care-plus-logo.png" alt="Care Plus" />
              </span>
              <span className="brand-copy">
                <strong>{t("brand")}</strong>
                <small>{t("brandTagline")}</small>
              </span>
            </a>

            <p>{t("footer.description")}</p>

            <a href="#contact" className="footer-contact-link">
              {isArabic ? "تواصل مع فريقنا" : "Get in touch"}
              <Arrow size={16} />
            </a>
          </div>

          <div className="footer-editorial-column">
            <h4>{t("footer.links")}</h4>
            <a href="#home">{t("nav.home")}</a>
            <a href="#services">{t("nav.services")}</a>
            <a href="#about">{t("nav.about")}</a>
            <a href="#process">{t("process.title")}</a>
            <a href="#contact">{t("nav.contact")}</a>
          </div>

          <div className="footer-editorial-column">
            <h4>{isArabic ? "خدماتنا" : "Our services"}</h4>
            <a href="#services">{t("services.home.title")}</a>
            <a href="#services">{t("services.physio.title")}</a>
            <a href="#services">{t("services.medical.title")}</a>
            <a href="#services">{t("services.all")}</a>
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
          <a href="#home">
            {isArabic ? "العودة إلى الأعلى ↑" : "Back to top ↑"}
          </a>
        </div>
      </div>
    </footer>
  );
}

