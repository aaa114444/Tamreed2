import { useTranslation } from "react-i18next";
import { Sparkles, Stethoscope, HeartPulse, ArrowUpRight } from "lucide-react";

export default function ProcessSection() {
  const { t, i18n } = useTranslation();
  const isArabic = i18n.language.startsWith("ar");

  const steps = [
    {
      number: "01",
      icon: Sparkles,
      title: "step1",
      desc: "step1desc",
      label: isArabic ? "ابدأ رحلتك" : "GET STARTED",
    },
    {
      number: "02",
      icon: Stethoscope,
      title: "step2",
      desc: "step2desc",
      label: isArabic ? "نرتب رعايتك" : "YOUR CARE PLAN",
    },
    {
      number: "03",
      icon: HeartPulse,
      title: "step3",
      desc: "step3desc",
      label: isArabic ? "نصل إليك" : "CARE AT HOME",
    },
  ];

  return (
    <section className="section process-section process-editorial" id="process">
      <div className="container">
        <div className="process-editorial-heading">
          <div>
            <div className="eyebrow">
              <span className="eyebrow-line" />
              {t("process.eyebrow")}
            </div>
            <h2>{t("process.title")}</h2>
          </div>
          <p>{t("process.description")}</p>
        </div>

        <div className="process-editorial-grid">
          {steps.map((step) => {
            const Icon = step.icon;

            return (
              <article className="process-editorial-card" key={step.number}>
                <div className="process-editorial-card-top">
                  <span className="process-editorial-number">{step.number}</span>
                  <span className="process-editorial-label">{step.label}</span>
                </div>

                <div className="process-editorial-icon">
                  <Icon size={29} strokeWidth={1.5} />
                </div>

                <h3>{t(`process.${step.title}`)}</h3>
                <p>{t(`process.${step.desc}`)}</p>

                <div className="process-editorial-bottom">
                  <span>{step.number} / 03</span>
                  <ArrowUpRight size={20} />
                </div>
              </article>
            );
          })}
        </div>
      </div>
    </section>
  );
}
