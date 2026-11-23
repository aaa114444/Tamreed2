import { MessageCircle, Phone } from "lucide-react";
import { useTranslation } from "react-i18next";
import { CONTACT_PHONE, getWhatsAppUrl } from "../../config/contact";

export default function FloatingContactButtons() {
  const { i18n } = useTranslation();
  const isArabic = i18n.language.startsWith("ar");

  const whatsappLabel = isArabic ? "واتساب" : "WhatsApp";
  const callLabel = isArabic ? "اتصل بنا" : "Call us";

  return (
    <div
      className={`floating-contact-actions${isArabic ? " is-rtl" : ""}`}
      aria-label={isArabic ? "طرق التواصل" : "Contact options"}
    >
      <a
        className="floating-contact-button floating-whatsapp"
        href={getWhatsAppUrl(isArabic)}
        target="_blank"
        rel="noopener noreferrer"
        aria-label={whatsappLabel}
        title={whatsappLabel}
      >
        <MessageCircle size={21} />
        <span>{whatsappLabel}</span>
      </a>

      <a
        className="floating-contact-button floating-call"
        href={`tel:${CONTACT_PHONE}`}
        aria-label={callLabel}
        title={callLabel}
      >
        <Phone size={20} />
        <span>{callLabel}</span>
      </a>
    </div>
  );
}
