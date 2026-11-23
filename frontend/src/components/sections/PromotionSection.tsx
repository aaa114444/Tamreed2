import { useTranslation } from "react-i18next";
import { ArrowUpLeft, ArrowUpRight, Megaphone } from "lucide-react";

export default function PromotionSection() {
  const { i18n } = useTranslation();
  const isArabic = i18n.language.startsWith("ar");
  const Arrow = isArabic ? ArrowUpLeft : ArrowUpRight;

  return (
    <section className="promotion-section" aria-label={isArabic ? "العروض والإعلانات" : "Promotions and announcements"}>
      <div className="container">
        <div className="promotion-banner">
          <div className="promotion-icon">
            <Megaphone size={25} />
          </div>

          <div className="promotion-content">
            <span className="promotion-eyebrow">
              {isArabic ? "ابقَ على اطلاع" : "STAY IN THE KNOW"}
            </span>

            <h2>
              {isArabic
                ? "رعايتك تبدأ بخطوة بسيطة"
                : "Your care starts with one simple step"}
            </h2>

            <p>
              {isArabic
                ? "اكتشف خدمات الرعاية المنزلية وتعرّف على الخيارات المناسبة لك ولعائلتك."
                : "Explore our home healthcare services and find the right care for you and your family."}
            </p>
          </div>

          <a href="#services" className="promotion-link">
            {isArabic ? "اكتشف خدماتنا" : "Explore services"}
            <Arrow size={17} />
          </a>
        </div>
      </div>
    </section>
  );
}
