import { useTranslation } from "react-i18next";
import { ArrowUpLeft, ArrowUpRight, Check, HeartPulse } from "lucide-react";

export default function AboutSection() {
  const { t, i18n } = useTranslation();
  const isArabic = i18n.language.startsWith("ar");
  const Arrow = isArabic ? ArrowUpLeft : ArrowUpRight;

  return (
    <section className="section about-section" id="about">
      <div className="container about-editorial-grid">
        <div className="about-editorial-visual">
          <div className="about-editorial-main-image">
            <img
              src="https://images.unsplash.com/photo-1576091160550-2173dba999ef?auto=format&fit=crop&w=1200&q=85"
              alt="Healthcare professional providing attentive care"
              loading="lazy"
            />
          </div>

          <div className="about-editorial-secondary-image">
            <img
              src="https://images.unsplash.com/photo-1559757175-0eb30cd8c063?auto=format&fit=crop&w=700&q=85"
              alt=""
              loading="lazy"
            />
          </div>

          <div className="about-editorial-label">
            <span className="about-editorial-label-icon">
              <HeartPulse size={23} />
            </span>
            <span>{t("about.imageLabel")}</span>
          </div>

          <span className="about-editorial-decoration" />
        </div>

        <div className="about-editorial-content">
          <div className="eyebrow">
            <span className="eyebrow-line" />
            {t("about.eyebrow")}
          </div>

          <h2>{t("about.title")}</h2>

          <p className="section-description">
            {t("about.description")}
          </p>

          <div className="about-editorial-points">
            {["point1", "point2", "point3"].map((point) => (
              <div className="about-editorial-point" key={point}>
                <span className="about-editorial-check">
                  <Check size={16} />
                </span>
                <span>{t(`about.${point}`)}</span>
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

