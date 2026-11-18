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
      ? "كير بلس | رعاية صحية منزلية"
      : "Care Plus | Home Healthcare";

    let description = isArabic
      ? "كير بلس تقدم خدمات الرعاية الصحية المنزلية باهتمام واحترافية، لتصلك الرعاية حتى باب بيتك."
      : "Care Plus brings compassionate, professional home healthcare services to your doorstep.";

    if (pathname === "/login") {
      title = isArabic ? "تسجيل الدخول | كير بلس" : "Login | Care Plus";
    } else if (pathname === "/register") {
      title = isArabic ? "إنشاء حساب | كير بلس" : "Create Account | Care Plus";
    } else if (pathname === "/booking") {
      title = isArabic ? "حجز خدمة | كير بلس" : "Book a Service | Care Plus";
    } else if (pathname === "/my-bookings") {
      title = isArabic ? "حجوزاتي | كير بلس" : "My Bookings | Care Plus";
    } else if (pathname === "/admin") {
      title = isArabic ? "لوحة الإدارة | كير بلس" : "Admin Dashboard | Care Plus";
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
