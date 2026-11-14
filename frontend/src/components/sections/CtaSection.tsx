import { useTranslation } from "react-i18next";
import { ArrowUpLeft, ArrowUpRight, HeartPulse } from "lucide-react";

export default function CtaSection() {
  const { t, i18n } = useTranslation();

  const Arrow = i18n.language === "ar" ? ArrowUpLeft : ArrowUpRight;

  return (
    <section className="cta-section">
      <div className="container">
        <div className="cta-card">
          <div className="cta-decoration" />

          <div className="cta-content">
            <div className="eyebrow eyebrow-light">
              <span className="eyebrow-line" />
              {t("cta.eyebrow")}
            </div>

            <h2>{t("cta.title")}</h2>
            <p>{t("cta.description")}</p>

            <a href="#services" className="button button-white">
              {t("cta.button")}
              <Arrow size={17} />
            </a>
          </div>

          <div className="cta-symbol">
            <HeartPulse size={150} strokeWidth={0.8} />
          </div>
        </div>
      </div>
    </section>
  );
}
