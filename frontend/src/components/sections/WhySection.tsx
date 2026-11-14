import { useTranslation } from "react-i18next";
import { Users, Home, Heart } from "lucide-react";

export default function WhySection() {
  const { t } = useTranslation();

  const benefits = [
    { icon: Users, title: t("why.one"), description: t("why.oneDesc") },
    { icon: Home, title: t("why.two"), description: t("why.twoDesc") },
    { icon: Heart, title: t("why.three"), description: t("why.threeDesc") },
  ];

  return (
    <section className="section why-section">
      <div className="container">
        <div className="why-header">
          <div>
            <div className="eyebrow">
              <span className="eyebrow-line" />
              {t("why.eyebrow")}
            </div>
            <h2>{t("why.title")}</h2>
          </div>
          <p>{t("why.description")}</p>
        </div>

        <div className="benefits-grid">
          {benefits.map((benefit, index) => {
            const Icon = benefit.icon;

            return (
              <article className="benefit-card" key={index}>
                <div className="benefit-icon">
                  <Icon size={25} />
                </div>
                <h3>{benefit.title}</h3>
                <p>{benefit.description}</p>
              </article>
            );
          })}
        </div>
      </div>
    </section>
  );
}
