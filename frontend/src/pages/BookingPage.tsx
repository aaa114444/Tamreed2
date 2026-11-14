import { useEffect, useState, type FormEvent } from "react";
import { Link, useSearchParams } from "react-router-dom";
import { ArrowLeft, ArrowRight, HeartPulse } from "lucide-react";
import { useTranslation } from "react-i18next";

import { ApiError } from "../services/apiClient";
import { createBooking } from "../services/bookingService";
import { getServices } from "../services/serviceService";

import type { ServiceResponse } from "../types/service";
import type { CreateBookingRequest } from "../types/booking";

function getTodayDate(): string {
  const today = new Date();
  const year = today.getFullYear();
  const month = String(today.getMonth() + 1).padStart(2, "0");
  const day = String(today.getDate()).padStart(2, "0");

  return `${year}-${month}-${day}`;
}

export default function BookingPage() {
  const { t, i18n } = useTranslation();
  const [searchParams] = useSearchParams();

  const isArabic = i18n.language === "ar";
  const BackArrow = isArabic ? ArrowRight : ArrowLeft;

  const [services, setServices] = useState<ServiceResponse[]>([]);
  const [serviceId, setServiceId] = useState(
    searchParams.get("serviceId") ?? ""
  );

  const [date, setDate] = useState("");
  const [time, setTime] = useState("");
  const [city, setCity] = useState("");
  const [district, setDistrict] = useState("");
  const [street, setStreet] = useState("");
  const [buildingNumber, setBuildingNumber] = useState("");
  const [apartmentNumber, setApartmentNumber] = useState("");
  const [notes, setNotes] = useState("");

  const [loadingServices, setLoadingServices] = useState(true);
  const [submitting, setSubmitting] = useState(false);
  const [error, setError] = useState("");
  const [success, setSuccess] = useState(false);

  useEffect(() => {
    let isMounted = true;

    async function loadServices() {
      try {
        const response = await getServices();

        if (isMounted) {
          const activeServices = response.filter(
            (service) => service.status.toLowerCase() === "active"
          );

          setServices(activeServices);

          const requestedServiceId = searchParams.get("serviceId");

          if (
            requestedServiceId &&
            activeServices.some((service) => service.id === requestedServiceId)
          ) {
            setServiceId(requestedServiceId);
          }
        }
      } catch (err) {
        if (isMounted) {
          setError(
            err instanceof Error
              ? err.message
              : t("booking.error")
          );
        }
      } finally {
        if (isMounted) {
          setLoadingServices(false);
        }
      }
    }

    loadServices();

    return () => {
      isMounted = false;
    };
  }, [searchParams, t]);

  const selectedService = services.find(
    (service) => service.id === serviceId
  );

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError("");
    setSuccess(false);

    if (!serviceId || !date || !time || !city.trim() ||
        !district.trim() || !street.trim() || !buildingNumber.trim()) {
      setError(t("booking.required"));
      return;
    }

    const scheduledDate = new Date(`${date}T${time}`);

    if (Number.isNaN(scheduledDate.getTime()) ||
        scheduledDate.getTime() <= Date.now()) {
      setError(
        isArabic
          ? "يرجى اختيار تاريخ ووقت في المستقبل."
          : "Please choose a future date and time."
      );
      return;
    }

    const request: CreateBookingRequest = {
      serviceId,
      scheduledAtUtc: scheduledDate.toISOString(),
      city: city.trim(),
      district: district.trim(),
      street: street.trim(),
      buildingNumber: buildingNumber.trim(),
      apartmentNumber: apartmentNumber.trim() || null,
      notes: notes.trim() || null,
    };

    setSubmitting(true);

    try {
      await createBooking(request);
      setSuccess(true);
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else if (err instanceof Error) {
        setError(err.message);
      } else {
        setError(t("booking.error"));
      }
    } finally {
      setSubmitting(false);
    }
  }

  return (
    <section className="auth-section">
      <div className="auth-card booking-card">
        <Link to="/" className="auth-back-link">
          <BackArrow size={16} />
          {t("booking.back")}
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
            TAMREED
          </span>
          <h1>{t("booking.title")}</h1>
          <p>{t("booking.description")}</p>
        </div>

        {error && (
          <div className="auth-error" role="alert">
            {error}
          </div>
        )}

        {success ? (
          <div className="booking-success" role="status">
            <h3>{t("booking.success")}</h3>
            <Link to="/my-bookings" className="button button-primary auth-submit">
              {t("myBookings.title")}
            </Link>
          </div>
        ) : (
          <form className="auth-form" onSubmit={handleSubmit}>
            <div className="auth-field">
              <label htmlFor="booking-service">
                {t("booking.service")}
              </label>
              <select
                id="booking-service"
                value={serviceId}
                onChange={(event) => setServiceId(event.target.value)}
                required
                disabled={loadingServices || services.length === 0}
              >
                <option value="">
                  {loadingServices
                    ? t("myBookings.loading")
                    : t("booking.selectService")}
                </option>
                {services.map((service) => (
                  <option key={service.id} value={service.id}>
                    {service.name}
                  </option>
                ))}
              </select>
              {selectedService && (
                <small className="auth-hint">
                  {t("booking.price")}:{" "}
                  {new Intl.NumberFormat(isArabic ? "ar-EG" : "en-EG", {
                    style: "currency",
                    currency: "EGP",
                    maximumFractionDigits: 0,
                  }).format(selectedService.price)}
                </small>
              )}
            </div>

            <div className="booking-grid">
              <div className="auth-field">
                <label htmlFor="booking-date">{t("booking.date")}</label>
                <input
                  id="booking-date"
                  type="date"
                  min={getTodayDate()}
                  value={date}
                  onChange={(event) => setDate(event.target.value)}
                  required
                />
              </div>

              <div className="auth-field">
                <label htmlFor="booking-time">{t("booking.time")}</label>
                <input
                  id="booking-time"
                  type="time"
                  value={time}
                  onChange={(event) => setTime(event.target.value)}
                  required
                />
              </div>

              <div className="auth-field">
                <label htmlFor="booking-city">{t("booking.city")}</label>
                <input
                  id="booking-city"
                  value={city}
                  onChange={(event) => setCity(event.target.value)}
                  maxLength={100}
                  required
                />
              </div>

              <div className="auth-field">
                <label htmlFor="booking-district">{t("booking.district")}</label>
                <input
                  id="booking-district"
                  value={district}
                  onChange={(event) => setDistrict(event.target.value)}
                  maxLength={100}
                  required
                />
              </div>

              <div className="auth-field">
                <label htmlFor="booking-street">{t("booking.street")}</label>
                <input
                  id="booking-street"
                  value={street}
                  onChange={(event) => setStreet(event.target.value)}
                  maxLength={200}
                  required
                />
              </div>

              <div className="auth-field">
                <label htmlFor="booking-building">
                  {t("booking.buildingNumber")}
                </label>
                <input
                  id="booking-building"
                  value={buildingNumber}
                  onChange={(event) => setBuildingNumber(event.target.value)}
                  maxLength={50}
                  required
                />
              </div>
            </div>

            <div className="auth-field">
              <label htmlFor="booking-apartment">
                {t("booking.apartmentNumber")}
              </label>
              <input
                id="booking-apartment"
                value={apartmentNumber}
                onChange={(event) => setApartmentNumber(event.target.value)}
                maxLength={50}
              />
            </div>

            <div className="auth-field">
              <label htmlFor="booking-notes">{t("booking.notes")}</label>
              <textarea
                id="booking-notes"
                value={notes}
                onChange={(event) => setNotes(event.target.value)}
                rows={4}
                maxLength={1000}
              />
            </div>

            <button
              type="submit"
              className="button button-primary auth-submit"
              disabled={submitting || loadingServices || services.length === 0}
            >
              {submitting ? t("booking.submitting") : t("booking.submit")}
            </button>
          </form>
        )}
      </div>
    </section>
  );
}
