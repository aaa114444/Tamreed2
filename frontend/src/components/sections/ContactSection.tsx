import { useEffect, useState, type FormEvent } from "react";
import { useTranslation } from "react-i18next";
import {
  ArrowUpLeft,
  ArrowUpRight,
  Mail,
  MapPin,
  MessageCircle,
  Send,
  CheckCircle2,
} from "lucide-react";

import { getServices } from "../../services/serviceService";
import { createCallbackRequest } from "../../services/callbackRequestService";
import type { ServiceResponse } from "../../types/service";

export default function ContactSection() {
  const { t, i18n } = useTranslation();
  const isArabic = i18n.language.startsWith("ar");
  const Arrow = isArabic ? ArrowUpLeft : ArrowUpRight;

  const [services, setServices] = useState<ServiceResponse[]>([]);
  const [name, setName] = useState("");
  const [mobileNumber, setMobileNumber] = useState("");
  const [selectedServices, setSelectedServices] = useState<string[]>([]);
  const [message, setMessage] = useState("");
  const [website, setWebsite] = useState("");
  const [submitting, setSubmitting] = useState(false);
  const [success, setSuccess] = useState("");
  const [error, setError] = useState("");

  useEffect(() => {
    let mounted = true;

    getServices()
      .then((data) => {
        if (mounted) {
          setServices(data.filter((service) =>
            service.status.toLowerCase() === "active"
          ));
        }
      })
      .catch(() => {
        if (mounted) {
          setError(
            isArabic
              ? "تعذر تحميل الخدمات حالياً. يرجى تحديث الصفحة والمحاولة مرة أخرى."
              : "Unable to load services right now. Please refresh the page and try again."
          );
        }
      });

    return () => {
      mounted = false;
    };
  }, [isArabic]);

  function toggleService(serviceName: string) {
    setSelectedServices((current) =>
      current.includes(serviceName)
        ? current.filter((item) => item !== serviceName)
        : [...current, serviceName]
    );
  }

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError("");
    setSuccess("");

    if (selectedServices.length === 0) {
      setError(
        isArabic
          ? "يرجى اختيار خدمة واحدة على الأقل."
          : "Please select at least one service."
      );
      return;
    }

    setSubmitting(true);

    try {
      const response = await createCallbackRequest({
        name: name.trim(),
        mobileNumber: mobileNumber.trim(),
        servicesNeeded: selectedServices,
        message: message.trim() || undefined,
        website,
      });

      setSuccess(
        isArabic
          ? "تم استلام طلبك بنجاح. سيتواصل معك فريقنا قريباً."
          : response.message
      );

      setName("");
      setMobileNumber("");
      setSelectedServices([]);
      setMessage("");
      setWebsite("");
    } catch (err) {
      setError(
        err instanceof Error
          ? err.message
          : isArabic
            ? "تعذر إرسال طلبك. يرجى المحاولة مرة أخرى."
            : "We couldn't submit your request. Please try again."
      );
    } finally {
      setSubmitting(false);
    }
  }

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
                  {isArabic ? "اكتشف خدماتنا أولاً" : "Explore our services"}
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

        <div className="callback-form-section">
          <div className="callback-form-heading">
            <span className="eyebrow">
              <span className="eyebrow-line" />
              {isArabic ? "طلب معاودة الاتصال" : "REQUEST A CALLBACK"}
            </span>
            <h3>
              {isArabic
                ? "دعنا نتواصل معك"
                : "Let us get in touch with you"}
            </h3>
            <p>
              {isArabic
                ? "أرسل بياناتك والخدمات التي تحتاجها، وسيتواصل معك فريقنا لمساعدتك."
                : "Share your details and the services you need. Our team will contact you to help."}
            </p>
          </div>

          <form
            className="callback-form"
            onSubmit={(event) => void handleSubmit(event)}
            dir={isArabic ? "rtl" : "ltr"}
          >
            <div className="callback-form-grid">
              <label className="callback-field">
                <span>{isArabic ? "الاسم بالكامل" : "Full name"} *</span>
                <input
                  type="text"
                  value={name}
                  onChange={(event) => setName(event.target.value)}
                  placeholder={isArabic ? "أدخل اسمك" : "Enter your name"}
                  maxLength={120}
                  autoComplete="name"
                  required
                />
              </label>

              <label className="callback-field">
                <span>{isArabic ? "رقم الهاتف" : "Mobile number"} *</span>
                <input
                  type="tel"
                  value={mobileNumber}
                  onChange={(event) => setMobileNumber(event.target.value)}
                  placeholder={isArabic ? "مثال: 01050058580" : "e.g. 01050058580"}
                  maxLength={30}
                  autoComplete="tel"
                  required
                />
              </label>
            </div>

            <fieldset className="callback-services">
              <legend>
                {isArabic ? "الخدمات التي تحتاجها" : "Services needed"} *
              </legend>

              {services.length === 0 ? (
                <p className="callback-hint">
                  {isArabic
                    ? "لا توجد خدمات متاحة للاختيار حالياً."
                    : "No services are currently available to select."}
                </p>
              ) : (
                <div className="callback-service-options">
                  {services.map((service) => {
                    const serviceName = service.name;

                    return (
                      <label className="callback-service-option" key={service.id}>
                        <input
                          type="checkbox"
                          checked={selectedServices.includes(serviceName)}
                          onChange={() => toggleService(serviceName)}
                        />
                        <span>
                          {isArabic
                            ? service.nameAr || service.name
                            : service.name}
                        </span>
                      </label>
                    );
                  })}
                </div>
              )}
            </fieldset>

            <label className="callback-field">
              <span>{isArabic ? "رسالة إضافية (اختياري)" : "Message (optional)"}</span>
              <textarea
                value={message}
                onChange={(event) => setMessage(event.target.value)}
                placeholder={
                  isArabic
                    ? "أخبرنا كيف يمكننا مساعدتك..."
                    : "Tell us how we can help..."
                }
                maxLength={1000}
                rows={4}
              />
            </label>

            <div className="callback-honeypot" aria-hidden="true">
              <label>
                Website
                <input
                  type="text"
                  name="website"
                  value={website}
                  onChange={(event) => setWebsite(event.target.value)}
                  tabIndex={-1}
                  autoComplete="off"
                />
              </label>
            </div>

            {error && (
              <p className="callback-message callback-error" role="alert">
                {error}
              </p>
            )}

            {success && (
              <p className="callback-message callback-success" role="status">
                <CheckCircle2 size={19} />
                {success}
              </p>
            )}

            <p className="callback-disclaimer">
              {isArabic
                ? "هذا الطلب مخصص لمعاودة الاتصال بك فقط، ولا يُعد تأكيداً لحجز موعد. سيتواصل معك فريقنا لتأكيد التفاصيل."
                : "This is a callback request only, not an appointment confirmation. Our team will contact you to discuss the details."}
            </p>

            <button
              type="submit"
              className="button button-dark callback-submit"
              disabled={submitting || services.length === 0}
            >
              {submitting
                ? isArabic ? "جارٍ الإرسال..." : "Sending..."
                : isArabic ? "إرسال الطلب" : "Request a callback"}
              <Send size={17} />
            </button>
          </form>
        </div>
      </div>
    </section>
  );
}
