import { useCallback, useEffect, useState } from "react";
import { Link } from "react-router-dom";
import {
  ArrowLeft,
  ArrowRight,
  HeartPulse,
  RefreshCw,
  CalendarDays,
  MapPin,
  FileText,
} from "lucide-react";
import { useTranslation } from "react-i18next";

import { getMyBookings } from "../services/bookingService";
import { getServices } from "../services/serviceService";

import type { BookingResponse } from "../types/booking";
import type { ServiceResponse } from "../types/service";

export default function MyBookingsPage() {
  const { t, i18n } = useTranslation();

  const isArabic = i18n.language === "ar";
  const BackArrow = isArabic ? ArrowRight : ArrowLeft;

  const [bookings, setBookings] = useState<BookingResponse[]>([]);
  const [services, setServices] = useState<ServiceResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [error, setError] = useState("");

  const loadBookings = useCallback(async (isRefresh = false) => {
    if (isRefresh) {
      setRefreshing(true);
    } else {
      setLoading(true);
    }

    try {
      const [bookingResponse, serviceResponse] = await Promise.all([
        getMyBookings(),
        getServices(),
      ]);

      setBookings(bookingResponse);
      setServices(serviceResponse);
      setError("");
    } catch (err) {
      setError(
        err instanceof Error ? err.message : t("myBookings.error")
      );
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  }, [t]);

  useEffect(() => {
    void loadBookings();

  }, [loadBookings]);

  function getServiceName(serviceId: string): string {
    return (
      services.find((service) => service.id === serviceId)?.name ??
      (isArabic ? "خدمة غير معروفة" : "Unknown service")
    );
  }

  function formatDate(value: string): string {
    const date = new Date(value);

    if (Number.isNaN(date.getTime())) {
      return value;
    }

    return new Intl.DateTimeFormat(isArabic ? "ar-EG" : "en-EG", {
      dateStyle: "medium",
      timeStyle: "short",
    }).format(date);
  }

  function getStatusLabel(status: string): string {
    const normalized = status.toLowerCase();

    const labels: Record<string, { en: string; ar: string }> = {
      pending: { en: "Pending", ar: "قيد الانتظار" },
      confirmed: { en: "Confirmed", ar: "تم التأكيد" },
      inprogress: { en: "In progress", ar: "جارٍ التنفيذ" },
      completed: { en: "Completed", ar: "مكتمل" },
      cancelled: { en: "Cancelled", ar: "ملغي" },
    };

    const label = labels[normalized];

    return label
      ? isArabic ? label.ar : label.en
      : status;
  }

  function getStatusClass(status: string): string {
    const normalized = status.toLowerCase();

    if (normalized === "confirmed") return "confirmed";
    if (normalized === "completed") return "completed";
    if (normalized === "cancelled") return "cancelled";
    if (normalized === "inprogress") return "in-progress";

    return "pending";
  }

  return (
    <section className="auth-section">
      <div className="auth-card bookings-card">
        <Link to="/" className="auth-back-link">
          <BackArrow size={16} />
          {t("auth.backHome")}
        </Link>

        <div className="auth-brand">
          <span className="brand-symbol">
            <HeartPulse size={26} />
          </span>
          <span className="brand-copy">
            <strong>{t("brand")}</strong>
            <small>{t("brandTagline")}</small>
          </span>
        </div>

        <div className="auth-heading">
          <span className="eyebrow">
            <span className="eyebrow-line" />
            CARE PLUS
          </span>
          <h1>{t("myBookings.title")}</h1>
          <p>{t("myBookings.description")}</p>
        </div>

        <div className="bookings-toolbar">
          <span>
            {isArabic
              ? `${bookings.length} طلب حجز`
              : `${bookings.length} booking${bookings.length === 1 ? "" : "s"}`}
          </span>

          <button
            type="button"
            className="bookings-refresh-button"
            onClick={() => void loadBookings(true)}
            disabled={loading || refreshing}
          >
            <RefreshCw
              size={16}
              className={refreshing ? "refresh-spinning" : ""}
            />
            {isArabic ? "تحديث" : "Refresh"}
          </button>
        </div>

        {loading && (
          <p className="services-message">
            {t("myBookings.loading")}
          </p>
        )}

        {error && (
          <div className="auth-error" role="alert">
            {error}
            <button
              type="button"
              className="bookings-error-retry"
              onClick={() => void loadBookings(true)}
            >
              {isArabic ? "حاول مرة أخرى" : "Try again"}
            </button>
          </div>
        )}

        {!loading && !error && bookings.length === 0 && (
          <div className="booking-empty">
            <CalendarDays size={38} />
            <p>{t("myBookings.empty")}</p>
            <Link
              to="/#services"
              className="button button-primary auth-submit"
            >
              {t("myBookings.viewServices")}
            </Link>
          </div>
        )}

        {!loading && !error && bookings.length > 0 && (
          <div className="bookings-list">
            {bookings.map((booking) => {
              const address = [
                booking.street,
                booking.buildingNumber,
                booking.apartmentNumber,
                booking.district,
                booking.city,
              ]
                .filter(Boolean)
                .join(", ");

              return (
                <article className="booking-item" key={booking.id}>
                  <div className="booking-item-heading">
                    <h3>{getServiceName(booking.serviceId)}</h3>
                    <span className={`booking-status status-${getStatusClass(booking.status)}`}>
                      {getStatusLabel(booking.status)}
                    </span>
                  </div>

                  <div className="booking-detail">
                    <CalendarDays size={16} />
                    <span>{t("myBookings.date")}</span>
                    <strong>{formatDate(booking.scheduledAtUtc)}</strong>
                  </div>

                  <div className="booking-detail">
                    <MapPin size={16} />
                    <span>{t("myBookings.address")}</span>
                    <strong>{address}</strong>
                  </div>

                  {booking.notes && (
                    <div className="booking-detail">
                      <FileText size={16} />
                      <span>{t("myBookings.notes")}</span>
                      <strong>{booking.notes}</strong>
                    </div>
                  )}
                </article>
              );
            })}
          </div>
        )}

        {!loading && (
          <Link
            to="/#services"
            className="bookings-explore-link"
          >
            {t("myBookings.viewServices")}
            <BackArrow size={16} />
          </Link>
        )}
      </div>
    </section>
  );
}

