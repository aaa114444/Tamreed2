import { useTranslation } from "react-i18next";
import { ArrowUpLeft, ArrowUpRight, Mail } from "lucide-react";

export default function ContactSection() {
  const { t, i18n } = useTranslation();

  const Arrow = i18n.language === "ar" ? ArrowUpLeft : ArrowUpRight;

  return (
    <section className="section contact-section" id="contact">
      <div className="container contact-grid">
        <div>
          <div className="eyebrow">
            <span className="eyebrow-line" />
            {t("contact.eyebrow")}
          </div>
          <h2>{t("contact.title")}</h2>
          <p className="section-description">{t("contact.description")}</p>
        </div>

        <div className="contact-card">
          <div className="contact-card-icon">
            <Mail size={23} />
          </div>
          <div>
            <span>{t("contact.email")}</span>
            <strong>{t("contact.message")}</strong>
            <p>{t("contact.locationValue")}</p>
          </div>
          <a href="#services" aria-label="Explore services">
            <Arrow size={20} />
          </a>
        </div>
      </div>
    </section>
  );
}
