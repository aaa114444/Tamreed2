import { useTranslation } from "react-i18next";
import { HeartPulse } from "lucide-react";

export default function Footer() {
  const { t } = useTranslation();

  return (
    <footer className="footer">
      <div className="container footer-main">
        <div className="footer-brand">
          <a href="#home" className="brand brand-footer">
            <span className="brand-symbol">
              <HeartPulse size={25} />
            </span>
            <span className="brand-copy">
              <strong>{t("brand")}</strong>
              <small>{t("brandTagline")}</small>
            </span>
          </a>
          <p>{t("footer.description")}</p>
        </div>

        <div className="footer-column">
          <h4>{t("footer.links")}</h4>
          <a href="#home">{t("nav.home")}</a>
          <a href="#services">{t("nav.services")}</a>
          <a href="#about">{t("nav.about")}</a>
          <a href="#contact">{t("nav.contact")}</a>
        </div>

        <div className="footer-column">
          <h4>{t("footer.follow")}</h4>
          <a href="#contact">Contact Tamreed</a>
          <a href="#services">{t("services.all")}</a>
          <a href="#about">{t("about.button")}</a>
        </div>

        <div className="footer-note">
          <div className="footer-heart">
            <HeartPulse size={27} />
          </div>
          <strong>{t("footer.tagline")}</strong>
          <p>{t("hero.note")}</p>
        </div>
      </div>

      <div className="container footer-bottom">
        <span>© {new Date().getFullYear()} Tamreed. {t("footer.rights")}</span>
        <span>{t("footer.tagline")}</span>
      </div>
    </footer>
  );
}
