import { useTranslation } from "react-i18next";
import {
  ArrowUpLeft,
  ArrowUpRight,
  Mail,
  MapPin,
  MessageCircle,
} from "lucide-react";

export default function ContactSection() {
  const { t, i18n } = useTranslation();
  const isArabic = i18n.language.startsWith("ar");
  const Arrow = isArabic ? ArrowUpLeft : ArrowUpRight;

  return (
    <section className="section contact-section contact-editorial" id="contact">
      <div className="container">
        <div className="contact-editorial-header">
          <div>
            <div className="eyebrow">
              <span className="eyebrow-line" />
              {t("contact.eyebrow")}
            </div>
            <h2>{t("contact.title")}</h2>
          </div>
          <p>{t("contact.description")}</p>
        </div>

        <div className="contact-editorial-grid">
          <div className="contact-editorial-main">
            <span className="contact-editorial-kicker">
              {isArabic ? "نحن هنا لمساعدتك" : "WE ARE HERE TO HELP"}
            </span>
            <h3>{t("contact.message")}</h3>
            <p>{t("contact.locationValue")}</p>

            <a href="#services" className="button button-dark">
              {t("contact.button")}
              <Arrow size={17} />
            </a>
          </div>

          <div className="contact-editorial-details">
            <div className="contact-editorial-item">
              <span className="contact-editorial-icon">
                <Mail size={22} />
              </span>
              <div>
                <span>{t("contact.email")}</span>
                <strong>
                  {isArabic ? "راسلنا للاستفسارات" : "Email us for enquiries"}
                </strong>
                <p>
                  {isArabic
                    ? "سيتم إضافة البريد الرسمي هنا"
                    : "Official email address coming soon"}
                </p>
              </div>
            </div>

            <div className="contact-editorial-item">
              <span className="contact-editorial-icon">
                <MapPin size={22} />
              </span>
              <div>
                <span>{t("contact.location")}</span>
                <strong>
                  {isArabic ? "خدمات الرعاية المنزلية" : "Home healthcare"}
                </strong>
                <p>{t("contact.locationValue")}</p>
              </div>
            </div>

            <div className="contact-editorial-item">
              <span className="contact-editorial-icon">
                <MessageCircle size={22} />
              </span>
              <div>
                <span>{isArabic ? "تواصل معنا" : "Get in touch"}</span>
                <strong>
                  {isArabic
                    ? "اكتشف خدماتنا أولاً"
                    : "Explore our services"}
                </strong>
                <p>
                  {isArabic
                    ? "تعرف على خيارات الرعاية المتاحة"
                    : "Find the care that meets your needs"}
                </p>
              </div>
            </div>
          </div>
        </div>
      </div>
    </section>
  );
}
