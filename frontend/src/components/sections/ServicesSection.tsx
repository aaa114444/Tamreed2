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
        const response = await getServices();

        if (isMounted) {
          setServices(
            response.filter(
              (service) => service.status.toLowerCase() === "active"
            )
          );
          setError(false);
        }
      } catch {
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
              const presentation =
                serviceDefinitions.find((definition) =>
                  service.name.toLowerCase().includes("physio")
                    ? definition.titleKey === "services.physio.title"
                    : definition.titleKey === "services.home.title"
                ) ?? serviceDefinitions[index % serviceDefinitions.length];

              const Icon = presentation.icon;

              return (
                <article className="service-card" key={service.id}>
                  <div className="service-card-top">
                    <div className={`service-icon ${presentation.color}`}>
                      <Icon size={27} strokeWidth={1.7} />
                    </div>

                    <span className="service-number">
                      {String(index + 1).padStart(2, "0")}
                    </span>
                  </div>

                  <h3>{service.name}</h3>
                  <p>{service.description}</p>

                  <p className="service-price">
                    {new Intl.NumberFormat(isArabic ? "ar-EG" : "en-EG", {
                      style: "currency",
                      currency: "EGP",
                      maximumFractionDigits: 0,
                    }).format(service.price)}
                  </p>

                  <Link
                    to={`/booking?serviceId=${encodeURIComponent(service.id)}`}
                    className="service-link"
                  >
                    {t("booking.title")}
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
