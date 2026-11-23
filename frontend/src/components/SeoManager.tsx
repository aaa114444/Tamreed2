import { useEffect } from "react";
import { useLocation } from "react-router-dom";
import { useTranslation } from "react-i18next";

export default function SeoManager() {
  const { pathname } = useLocation();
  const { i18n } = useTranslation();

  useEffect(() => {
    const isArabic = i18n.language === "ar";
    const privateRoutes = [
      "/login",
      "/register",
      "/booking",
      "/my-bookings",
      "/admin",
    ];

    const isPrivate = privateRoutes.some(
      (route) => pathname === route || pathname.startsWith(`${route}/`)
    );

    const isService = pathname.startsWith("/services/");

    if (isService) return;

    let title = isArabic
      ? "تمريد | رعاية صحية منزلية"
      : "Tamreed | Professional Home Healthcare";

    let description = isArabic
      ? "تمريد تقدم خدمات الرعاية الصحية المنزلية باهتمام واحترافية، لتصلك الرعاية حتى باب بيتك."
      : "Tamreed brings compassionate, professional home healthcare services directly to your doorstep. Discover home nursing, physiotherapy, and personalized care.";

    if (pathname === "/login") {
      title = isArabic ? "تسجيل الدخول | تمريد" : "Login | Tamreed";
    } else if (pathname === "/register") {
      title = isArabic ? "إنشاء حساب | تمريد" : "Create Account | Tamreed";
    } else if (pathname === "/booking") {
      title = isArabic ? "حجز خدمة | تمريد" : "Book a Service | Tamreed";
    } else if (pathname === "/my-bookings") {
      title = isArabic ? "حجوزاتي | تمريد" : "My Bookings | Tamreed";
    } else if (pathname === "/admin") {
      title = isArabic ? "لوحة الإدارة | تمريد" : "Admin Dashboard | Tamreed";
    }

    document.title = title;

    let descriptionTag = document.querySelector(
      'meta[name="description"]'
    ) as HTMLMetaElement | null;

    if (!descriptionTag) {
      descriptionTag = document.createElement("meta");
      descriptionTag.name = "description";
      document.head.appendChild(descriptionTag);
    }

    descriptionTag.content = description;

    let robotsTag = document.querySelector(
      'meta[name="robots"]'
    ) as HTMLMetaElement | null;

    if (!robotsTag) {
      robotsTag = document.createElement("meta");
      robotsTag.name = "robots";
      document.head.appendChild(robotsTag);
    }

    robotsTag.content = isPrivate
      ? "noindex, nofollow"
      : "index, follow";
  }, [pathname, i18n.language]);

  return null;
}
