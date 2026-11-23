import { useTranslation } from "react-i18next";
import { MapPin, ArrowRight } from "lucide-react";

const serviceAreas = [
  { en: "New Cairo", ar: "القاهرة الجديدة" },
  { en: "Fifth Settlement", ar: "التجمع الخامس" },
  { en: "Nasr City", ar: "مدينة نصر" },
  { en: "Heliopolis", ar: "مصر الجديدة" },
  { en: "Madinaty", ar: "مدينتي" },
  { en: "Rehab", ar: "الرحاب" },
];

export default function CoverageSection() {
  const { i18n } = useTranslation();
  const isArabic = i18n.language.startsWith("ar");

  return (
    <section className="section coverage-section">
      <div className="container">
        <div className="coverage-content">
          <div className="coverage-icon">
            <MapPin size={30} />
          </div>

          <div className="coverage-details">
            <div className="eyebrow">
              <span className="eyebrow-line" />
              {isArabic ? "مناطق الخدمة" : "OUR SERVICE AREAS"}
            </div>

            <h2>
              {isArabic
                ? "رعاية منزلية أقرب إليك"
                : "Quality care, closer to home."}
            </h2>

            <p>
              {isArabic
                ? "نقدم خدمات الرعاية المنزلية في المناطق التالية:"
                : "Our home healthcare services are available in:"}
            </p>

            <div className="coverage-areas-list">
              {serviceAreas.map((area) => (
                <span className="coverage-area" key={area.en}>
                  <MapPin size={15} />
                  {isArabic ? area.ar : area.en}
                </span>
              ))}
            </div>

            <a href="#contact" className="coverage-button">
              {isArabic ? "تواصل معنا" : "Check Availability"}
              <ArrowRight size={18} />
            </a>
          </div>
        </div>
      </div>
    </section>
  );
}
