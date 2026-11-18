import { useTranslation } from "react-i18next";
import { ArrowUpLeft, ArrowUpRight, HeartPulse, Phone } from "lucide-react";

export default function CtaSection() {
  const { t, i18n } = useTranslation();
  const isArabic = i18n.language.startsWith("ar");
  const Arrow = isArabic ? ArrowUpLeft : ArrowUpRight;

  return (
    <section className="cta-section cta-editorial">
      <div className="container">
        <div className="cta-editorial-card">
          <div className="cta-editorial-decoration cta-decoration-one" />
          <div className="cta-editorial-decoration cta-decoration-two" />

          <div className="cta-editorial-content">
            <div className="eyebrow eyebrow-light">
              <span className="eyebrow-line" />
              {t("cta.eyebrow")}
            </div>

            <h2>{t("cta.title")}</h2>
            <p>{t("cta.description")}</p>

            <div className="cta-editorial-actions">
              <a href="#services" className="button button-white">
                {t("cta.button")}
                <Arrow size={17} />
              </a>

              <a href="#contact" className="cta-contact-link">
                <Phone size={17} />
                {isArabic ? "تواصل معنا" : "Talk to our team"}
              </a>
            </div>
          </div>

          <div className="cta-editorial-visual" aria-hidden="true">
            <div className="cta-editorial-orbit">
              <HeartPulse size={125} strokeWidth={0.8} />
            </div>
            <span className="cta-editorial-caption">
              {isArabic ? "رعايتك تبدأ من هنا" : "YOUR CARE STARTS HERE"}
            </span>
          </div>
        </div>
      </div>
    </section>
  );
}
