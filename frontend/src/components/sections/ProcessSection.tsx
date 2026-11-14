import { useTranslation } from "react-i18next";
import { Sparkles, Stethoscope, HeartPulse } from "lucide-react";

export default function ProcessSection() {
  const { t } = useTranslation();

  const steps = [
    { number: "01", icon: Sparkles, title: "step1", desc: "step1desc" },
    { number: "02", icon: Stethoscope, title: "step2", desc: "step2desc" },
    { number: "03", icon: HeartPulse, title: "step3", desc: "step3desc" },
  ];

  return (
    <section className="section process-section">
      <div className="container">
        <div className="section-heading centered">
          <div className="eyebrow">
            <span className="eyebrow-line" />
            {t("process.eyebrow")}
            <span className="eyebrow-line" />
          </div>
          <h2>{t("process.title")}</h2>
          <p>{t("process.description")}</p>
        </div>

        <div className="process-grid">
          {steps.map((step) => {
            const Icon = step.icon;

            return (
              <article className="process-card" key={step.number}>
                <span className="process-number">{step.number}</span>
                <div className="process-icon">
                  <Icon size={26} />
                </div>
                <h3>{t(`process.${step.title}`)}</h3>
                <p>{t(`process.${step.desc}`)}</p>
              </article>
            );
          })}
        </div>
      </div>
    </section>
  );
}
