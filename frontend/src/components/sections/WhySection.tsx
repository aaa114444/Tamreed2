import { useTranslation } from "react-i18next";
import { Users, Home, Heart, ArrowUpRight } from "lucide-react";

export default function WhySection() {
  const { t, i18n } = useTranslation();
  const isArabic = i18n.language.startsWith("ar");

  const benefits = [
    { icon: Users, title: t("why.one"), description: t("why.oneDesc") },
    { icon: Home, title: t("why.two"), description: t("why.twoDesc") },
    { icon: Heart, title: t("why.three"), description: t("why.threeDesc") },
  ];

  return (
    <section className="section why-section why-editorial">
      <div className="container">
        <div className="why-editorial-panel">
          <div className="why-editorial-heading">
            <div>
              <div className="eyebrow">
                <span className="eyebrow-line" />
                {t("why.eyebrow")}
              </div>
              <h2>{t("why.title")}</h2>
            </div>
            <p>{t("why.description")}</p>
          </div>

          <div className="why-editorial-grid">
            {benefits.map((benefit, index) => {
              const Icon = benefit.icon;

              return (
                <article className="why-editorial-card" key={index}>
                  <div className="why-editorial-card-top">
                    <span className="why-editorial-number">
                      0{index + 1}
                    </span>
                    <ArrowUpRight size={19} />
                  </div>

                  <div className="why-editorial-icon">
                    <Icon size={27} strokeWidth={1.5} />
                  </div>

                  <h3>{benefit.title}</h3>
                  <p>{benefit.description}</p>
                </article>
              );
            })}
          </div>

          <div className="why-editorial-footer">
            <span>{isArabic ? "تمريض ورعاية باهتمام" : "THOUGHTFUL CARE. RIGHT AT HOME."}</span>
            <Heart size={17} />
          </div>
        </div>
      </div>
    </section>
  );
}
