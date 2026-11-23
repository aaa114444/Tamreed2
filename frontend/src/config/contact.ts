/**
 * TEMPORARY DEVELOPMENT CONTACT DETAILS.
 * Replace these values with Tamreed's verified official number before launch.
 */
export const CONTACT_PHONE = "+200000000000";
export const WHATSAPP_NUMBER = "200000000000";

export const getWhatsAppUrl = (isArabic: boolean) => {
  const message = isArabic
    ? "\u0645\u0631\u062d\u0628\u0627\u064b \u062a\u0645\u0631\u064a\u062f\u060c \u0623\u0648\u062f \u0627\u0644\u0627\u0633\u062a\u0641\u0633\u0627\u0631 \u0639\u0646 \u062e\u062f\u0645\u0627\u062a \u0627\u0644\u0631\u0639\u0627\u064a\u0629 \u0627\u0644\u0645\u0646\u0632\u0644\u064a\u0629."
    : "Hello Tamreed, I would like to ask about your home healthcare services.";

  return `https://wa.me/${WHATSAPP_NUMBER}?text=${encodeURIComponent(message)}`;
};
