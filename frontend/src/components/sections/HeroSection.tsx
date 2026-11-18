import { useTranslation } from "react-i18next";
import {
  ArrowUpLeft,
  ArrowUpRight,
  ArrowUpRight as ExternalArrow,
  HeartPulse,
  ShieldCheck,
  CheckCircle2,
  Clock3,
  Home,
} from "lucide-react";

export default function HeroSection() {
  const { t, i18n } = useTranslation();
  const isArabic = i18n.language === "ar";
  const Arrow = isArabic ? ArrowUpLeft : ArrowUpRight;

  return (
    <section className="tamreed-hero" id="home">
      <div className="tamreed-hero-grain" />

      <div className="container tamreed-hero-inner">
        <div className="tamreed-hero-copy">
          <div className="tamreed-hero-kicker">
            <span className="tamreed-kicker-mark">
              <HeartPulse size={15} />
            </span>
            <span>{t("hero.eyebrow")}</span>
          </div>

          <h1>
            {t("hero.title")}
            <span>{t("hero.highlight")}</span>
          </h1>

          <p className="tamreed-hero-description">
            {t("hero.description")}
          </p>

          <div className="tamreed-hero-actions">
            <a href="#services" className="tamreed-hero-primary">
              {t("hero.primary")}
              <Arrow size={18} />
            </a>

            <a href="#about" className="tamreed-hero-secondary">
              {t("hero.secondary")}
              <ExternalArrow size={16} />
            </a>
          </div>

          <div className="tamreed-hero-assurance">
            <span className="tamreed-assurance-icon">
              <ShieldCheck size={20} />
            </span>
            <div>
              <strong>{t("hero.badge")}</strong>
              <span>{t("hero.note")}</span>
            </div>
          </div>
        </div>

        <div className="tamreed-hero-art">
          <div className="tamreed-hero-image">
            <img
              src="https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?auto=format&fit=crop&w=1200&q=85"
              alt="Healthcare professional providing compassionate care"
            />
            <div className="tamreed-image-shade" />
            <div className="tamreed-image-caption">
              <span className="tamreed-caption-dot" />
              <span>{t("about.imageLabel")}</span>
            </div>
          </div>

          <div className="tamreed-floating-note tamreed-note-top">
            <span className="tamreed-note-icon">
              <CheckCircle2 size={21} />
            </span>
            <div>
              <strong>{t("hero.badge")}</strong>
              <small>{t("hero.stat")}</small>
            </div>
          </div>

          <div className="tamreed-floating-note tamreed-note-bottom">
            <span className="tamreed-note-icon home">
              <Home size={21} />
            </span>
            <div>
              <strong>{t("trust.two")}</strong>
              <small>{t("trust.three")}</small>
            </div>
          </div>

          <div className="tamreed-hero-stamp">
            <Clock3 size={17} />
            <span>{t("hero.note")}</span>
          </div>
        </div>
      </div>

      <div className="tamreed-hero-footer">
        <div className="container tamreed-hero-footer-inner">
          <span><ShieldCheck size={18} /> {t("trust.one")}</span>
          <span><Home size={18} /> {t("trust.two")}</span>
          <span><HeartPulse size={18} /> {t("trust.three")}</span>
        </div>
      </div>
    </section>
  );
}
