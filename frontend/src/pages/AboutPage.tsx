import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
  ArrowUpLeft,
  ArrowUpRight,
  HeartHandshake,
  HeartPulse,
  ShieldCheck,
  Sparkles,
} from "lucide-react";

import Header from "../components/layout/Header";
import Footer from "../components/layout/Footer";
import FloatingContactButtons from "../components/layout/FloatingContactButtons";

export default function AboutPage() {
  const { t, i18n } = useTranslation();
  const isArabic = i18n.language.startsWith("ar");
  const Arrow = isArabic ? ArrowUpLeft : ArrowUpRight;

  const values = [
    { key: "valueOne", icon: HeartHandshake },
    { key: "valueTwo", icon: HeartPulse },
    { key: "valueThree", icon: ShieldCheck },
    { key: "valueFour", icon: Sparkles },
  ];

  return (
    <div className="site-shell about-page">
      <Header />

      <main>
        <section className="section about-page-hero">
          <div className="container">
            <span className="eyebrow">{t("aboutPage.eyebrow")}</span>
            <h1>{t("aboutPage.title")}</h1>
            <p className="section-description">
              {t("aboutPage.intro")}
            </p>
          </div>
        </section>

        <section className="section">
          <div className="container about-page-story">
            <div>
              <span className="eyebrow">{t("aboutPage.storyTitle")}</span>
              <h2>{t("aboutPage.storyTitle")}</h2>
              <p className="section-description">
                {t("aboutPage.story")}
              </p>
              <p className="about-page-draft">
                {t("aboutPage.draftNotice")}
              </p>
            </div>

            <div className="about-page-image">
              <img
                src="https://images.unsplash.com/photo-1576091160550-2173dba999ef?auto=format&fit=crop&w=1000&q=85"
                alt="Healthcare professional providing attentive care"
                loading="lazy"
              />
            </div>
          </div>
        </section>

        <section className="section about-page-purpose">
          <div className="container about-page-purpose-grid">
            <article className="about-page-card">
              <HeartPulse size={30} />
              <h2>{t("aboutPage.missionTitle")}</h2>
              <p>{t("aboutPage.mission")}</p>
            </article>

            <article className="about-page-card">
              <Sparkles size={30} />
              <h2>{t("aboutPage.visionTitle")}</h2>
              <p>{t("aboutPage.vision")}</p>
            </article>
          </div>
        </section>

        <section className="section">
          <div className="container">
            <div className="about-page-values-heading">
              <span className="eyebrow">{t("aboutPage.valuesTitle")}</span>
              <h2>{t("aboutPage.valuesTitle")}</h2>
            </div>

            <div className="about-page-values-grid">
              {values.map(({ key, icon: Icon }) => (
                <article className="about-page-value" key={key}>
                  <Icon size={25} />
                  <h3>{t(`aboutPage.${key}`)}</h3>
                </article>
              ))}
            </div>
          </div>
        </section>

        <section className="section about-page-cta">
          <div className="container">
            <h2>{t("cta.title")}</h2>
            <p>{t("cta.description")}</p>
            <Link to="/#services" className="button button-dark">
              {t("aboutPage.cta")}
              <Arrow size={17} />
            </Link>
          </div>
        </section>
      </main>

      <Footer />
      <FloatingContactButtons />
    </div>
  );
}
