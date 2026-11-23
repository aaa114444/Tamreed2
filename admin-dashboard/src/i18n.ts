import i18n from "i18next";
import { initReactI18next } from "react-i18next";

const savedLanguage = localStorage.getItem("tamreed-admin-language");

void i18n
  .use(initReactI18next)
  .init({
    resources: {
      en: { translation: {} },
      ar: { translation: {} },
    },
    lng: savedLanguage === "ar" ? "ar" : "en",
    fallbackLng: "en",
    interpolation: {
      escapeValue: false,
    },
  });

i18n.on("languageChanged", (language) => {
  const isArabic = language === "ar";

  document.documentElement.lang = isArabic ? "ar" : "en";
  document.documentElement.dir = isArabic ? "rtl" : "ltr";

  localStorage.setItem("tamreed-admin-language", isArabic ? "ar" : "en");
});

export default i18n;
