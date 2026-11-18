import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { ArrowUpLeft, ArrowUpRight } from "lucide-react";

import { serviceDefinitions } from "../../data/services";
import { getServices } from "../../services/serviceService";
import type { ServiceResponse } from "../../types/service";

export default function ServicesSection() {
  const { t, i18n } = useTranslation();

  const [services, setServices] = useState<ServiceResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(false);

  const isArabic = i18n.language === "ar";
  const Arrow = isArabic ? ArrowUpLeft : ArrowUpRight;

  useEffect(() => {
    let isMounted = true;

    async function loadServices() {
      try {
        console.log("Loading services from API...");

        const response = await getServices();

        console.log("Services API response:", response);

        if (isMounted) {
          const activeServices = response.filter(
            (service) =>
              service.status?.toLowerCase() === "active"
          );

          console.log("Active services:", activeServices);

          setServices(activeServices);
          setError(false);
        }
      } catch (error) {
        console.error("Failed to load services:", error);

        if (isMounted) {
          setError(true);
        }
      } finally {
        if (isMounted) {
          setLoading(false);
        }
      }
    }

    loadServices();

    return () => {
      isMounted = false;
    };
  }, []);

  const getServiceImage = (name: string) => {
    const normalizedName = name.toLowerCase();

    if (normalizedName.includes("physio")) {
      return "https://images.unsplash.com/photo-1571019613454-1cb2f99b2d8b?auto=format&fit=crop&w=1000&q=85";
    }

    if (normalizedName.includes("nursing")) {
      return "https://images.unsplash.com/photo-1584515933487-779824d29309?auto=format&fit=crop&w=1000&q=85";
    }

    return "https://images.unsplash.com/photo-1576091160399-112ba8d25d1d?auto=format&fit=crop&w=1000&q=85";
  };

  return (
    <section className="section services-section" id="services">
      <div className="container">
        <div className="section-heading centered">
          <div className="eyebrow">
            <span className="eyebrow-line" />
            {t("services.eyebrow")}
            <span className="eyebrow-line" />
          </div>

          <h2>{t("services.title")}</h2>
          <p>{t("services.description")}</p>
        </div>

        {loading && (
          <p className="services-message">
            {isArabic ? "جارٍ تحميل الخدمات..." : "Loading services..."}
          </p>
        )}

        {error && (
          <p className="services-message">
            {isArabic
              ? "تعذر تحميل الخدمات. يرجى المحاولة مرة أخرى."
              : "Unable to load services. Please try again later."}
          </p>
        )}

        {!loading && !error && services.length === 0 && (
          <p className="services-message">
            {isArabic
              ? "لا توجد خدمات متاحة حالياً."
              : "No services are currently available."}
          </p>
        )}

        {!loading && !error && services.length > 0 && (
          <div className="services-grid">
            {services.map((service, index) => {
              const normalizedName = service.name.toLowerCase();

              const presentation =
                serviceDefinitions.find((definition) => {
                  if (definition.titleKey === "services.physio.title") {
                    return normalizedName.includes("physio");
                  }

                  if (definition.titleKey === "services.home.title") {
                    return (
                      normalizedName.includes("nursing") ||
                      normalizedName.includes("home")
                    );
                  }

                  if (definition.titleKey === "services.medical.title") {
                    return (
                      normalizedName.includes("medical") ||
                      normalizedName.includes("personalized")
                    );
                  }

                  return false;
                }) ?? serviceDefinitions[index % serviceDefinitions.length];

              const Icon = presentation.icon;

              return (
                <article className="service-card" key={service.id}>
                  <div className="service-card-image">
                    <img
                      src={getServiceImage(service.name)}
                      alt={service.name}
                      loading="lazy"
                    />
                    <span className="service-image-overlay" />
                    <span className="service-image-label">
                      {isArabic ? "رعاية تصل إليك" : "CARE THAT COMES HOME"}
                    </span>
                  </div>

                  <div className="service-card-top">
                    <div className={`service-icon ${presentation.color}`}>
                      <Icon size={25} strokeWidth={1.7} />
                    </div>

                    <span className="service-number">
                      {String(index + 1).padStart(2, "0")}
                    </span>
                  </div>

                  <h3>{t(presentation.titleKey)}</h3>
                  <p>{t(presentation.descriptionKey)}</p>

                  <Link
                    to={`/services/${service.id}`}
                    className="service-link"
                  >
                    {isArabic ? "اعرف المزيد" : "Learn More"}
                    <Arrow size={17} />
                  </Link>
                </article>
              );
            })}
          </div>
        )}

        <div className="section-bottom-link">
          <a href="#contact" className="button button-outline">
            {t("services.all")}
            <Arrow size={17} />
          </a>
        </div>
      </div>
    </section>
  );
}
