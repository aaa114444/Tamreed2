import { useTranslation } from "react-i18next";
import { ArrowUpLeft, ArrowUpRight, Check, HeartPulse } from "lucide-react";

export default function AboutSection() {
  const { t, i18n } = useTranslation();
  const isArabic = i18n.language === "ar";
  const Arrow = isArabic ? ArrowUpLeft : ArrowUpRight;

  return (
    <section className="section about-section" id="about">
      <div className="container about-grid">
        <div className="about-visual">
          <div className="about-image-main">
            <img
              src="https://images.unsplash.com/photo-1584515933487-779824d29309?auto=format&fit=crop&w=1000&q=85"
              alt="Healthcare professional caring for a patient"
              loading="lazy"
            />
          </div>
          <div className="about-image-accent" />
          <div className="about-sticker">
            <HeartPulse size={28} />
            <span>{t("about.imageLabel")}</span>
          </div>
        </div>

        <div className="about-content">
          <div className="eyebrow">
            <span className="eyebrow-line" />
            {t("about.eyebrow")}
          </div>
          <h2>{t("about.title")}</h2>
          <p className="section-description">{t("about.description")}</p>

          <div className="about-points">
            {["point1", "point2", "point3"].map((point) => (
              <div className="about-point" key={point}>
                <span><Check size={15} /></span>
                {t(`about.${point}`)}
              </div>
            ))}
          </div>

          <a href="#contact" className="button button-dark">
            {t("about.button")}
            <Arrow size={17} />
          </a>
        </div>
      </div>
    </section>
  );
}
