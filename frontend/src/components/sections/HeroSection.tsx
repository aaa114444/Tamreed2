import { useTranslation } from "react-i18next";
import {
  ArrowUpLeft,
  ArrowUpRight,
  CheckCircle2,
  Heart,
  HeartPulse,
  Home,
  ShieldCheck,
  Users,
} from "lucide-react";

export default function HeroSection() {
  const { t, i18n } = useTranslation();
  const Arrow = i18n.language === "ar" ? ArrowUpLeft : ArrowUpRight;

  return (
    <section className="hero" id="home">
      <div className="hero-decoration decoration-one" />
      <div className="hero-decoration decoration-two" />

      <div className="container hero-grid">
        <div className="hero-content">
          <div className="eyebrow">
            <span className="eyebrow-line" />
            {t("hero.eyebrow")}
          </div>

          <h1>
            {t("hero.title")}
            <br />
            <span>{t("hero.highlight")}</span>
          </h1>

          <p className="hero-description">
            {t("hero.description")}
          </p>

          <div className="hero-actions">
            <a href="#services" className="button button-primary">
              {t("hero.primary")}
              <Arrow size={18} />
            </a>
            <a href="#about" className="text-link">
              {t("hero.secondary")}
              <span><Arrow size={16} /></span>
            </a>
          </div>

          <div className="hero-trust">
            <div className="trust-avatars">
              <span><Users size={16} /></span>
              <span><Heart size={16} /></span>
              <span><ShieldCheck size={16} /></span>
            </div>
            <div>
              <strong>{t("hero.stat")}</strong>
              <small>{t("hero.note")}</small>
            </div>
          </div>
        </div>

        <div className="hero-visual">
          <div className="hero-image-wrap">
            <img
              src="https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?auto=format&fit=crop&w=1100&q=85"
              alt="Healthcare professional providing compassionate care"
            />
            <div className="image-overlay" />
          </div>

          <div className="floating-card care-card">
            <span className="floating-icon"><HeartPulse size={21} /></span>
            <div>
              <strong>{t("hero.badge")}</strong>
              <small>{t("hero.stat")}</small>
            </div>
            <CheckCircle2 className="floating-check" size={20} />
          </div>

          <div className="floating-card comfort-card">
            <span className="comfort-icon"><Home size={20} /></span>
            <span>{t("trust.two")}</span>
          </div>

          <div className="hero-image-caption">
            <span className="caption-dot" />
            <span>{t("about.imageLabel")}</span>
          </div>
        </div>
      </div>

      <div className="hero-bottom">
        <div className="container hero-bottom-inner">
          <span><ShieldCheck size={19} /> {t("trust.one")}</span>
          <span><Home size={19} /> {t("trust.two")}</span>
          <span><Heart size={19} /> {t("trust.three")}</span>
        </div>
      </div>
    </section>
  );
}