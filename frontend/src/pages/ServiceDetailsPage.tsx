import { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
  ArrowRight,
  Activity,
  ArrowUpRight,
  CheckCircle2,
  Clock3,
  HeartHandshake,
  HeartPulse,
  MapPin,
  Phone,
  ShieldCheck,
  Sparkles,
} from "lucide-react";

import { getServices } from "../services/serviceService";
import type { ServiceResponse } from "../types/service";

const PHONE_NUMBER = "+201000000000";

export default function ServiceDetailsPage() {
  const { id } = useParams();
  const { i18n } = useTranslation();
  const isArabic = i18n.language === "ar";

  const [service, setService] = useState<ServiceResponse | null>(null);
  const [relatedServices, setRelatedServices] = useState<ServiceResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(false);

  useEffect(() => {
    let active = true;

    async function loadService() {
      try {
        const services = await getServices();
        const activeServices = services.filter(
          (item) => item.status.toLowerCase() === "active"
        );
        const found = activeServices.find((item) => item.id === id);

        if (active) {
          setService(found ?? null);
          setRelatedServices(activeServices.filter((item) => item.id !== id));
          setError(false);
        }
      } catch {
        if (active) setError(true);
      } finally {
        if (active) setLoading(false);
      }
    }

    loadService();
    return () => {
      active = false;
    };
  }, [id]);

  useEffect(() => {
    if (!service) return;

    const pageTitle = isArabic
      ? service.titleAr || service.nameAr || service.title || service.name
      : service.title || service.name;

    document.title = `${pageTitle} | Tamreed`;

    const description = isArabic
      ? service.descriptionAr || service.description
      : service.description;

    let descriptionTag = document.querySelector(
      'meta[name="description"]'
    ) as HTMLMetaElement | null;

    if (!descriptionTag) {
      descriptionTag = document.createElement("meta");
      descriptionTag.name = "description";
      document.head.appendChild(descriptionTag);
    }

    descriptionTag.content = description.slice(0, 155);

    let robotsTag = document.querySelector(
      'meta[name="robots"]'
    ) as HTMLMetaElement | null;

    if (!robotsTag) {
      robotsTag = document.createElement("meta");
      robotsTag.name = "robots";
      document.head.appendChild(robotsTag);
    }

    robotsTag.content = "index, follow";
  }, [service, isArabic]);

  if (loading) {
    return (
      <main className="service-article-page">
        <div className="container service-loading">
          <div className="service-loading-mark"><HeartPulse size={30} /></div>
          <p>{isArabic ? "جارٍ تحميل تفاصيل الخدمة..." : "Preparing your service guide..."}</p>
        </div>
      </main>
    );
  }

  if (error || !service) {
    return (
      <main className="service-article-page">
        <div className="container service-error">
          <HeartPulse size={42} />
          <h1>{isArabic ? "الخدمة غير متاحة" : "Service unavailable"}</h1>
          <p>
            {isArabic
              ? "تعذر العثور على هذه الخدمة أو تحميل تفاصيلها. يرجى المحاولة مرة أخرى."
              : "We couldn't find this service or load its details. Please try again."}
          </p>
          <Link to="/" className="service-primary-button">
            {isArabic ? "العودة إلى الرئيسية" : "Return to homepage"}
          </Link>
        </div>
      </main>
    );
  }

  const normalizedName = service.name.toLowerCase();
  const isPhysio = normalizedName.includes("physio");
  const isNursing = normalizedName.includes("nursing");
  const Icon = isPhysio ? Activity : isNursing ? HeartPulse : HeartHandshake;

  if (loading) {
    return (
      <main className="service-article-page">
        <div className="container service-loading">
          <div className="service-loading-mark"><HeartPulse size={30} /></div>
          <p>{isArabic ? "جارٍ تحميل تفاصيل الخدمة..." : "Preparing your service guide..."}</p>
        </div>
      </main>
    );
  }

  if (error || !service) {
    return (
      <main className="service-article-page">
        <div className="container service-error">
          <HeartPulse size={42} />
          <h1>{isArabic ? "الخدمة غير متاحة" : "Service unavailable"}</h1>
          <p>
            {isArabic
              ? "تعذر العثور على هذه الخدمة أو تحميل تفاصيلها. يرجى المحاولة مرة أخرى."
              : "We couldn't find this service or load its details. Please try again."}
          </p>
          <Link to="/" className="service-primary-button">
            {isArabic ? "العودة إلى الرئيسية" : "Return to homepage"}
          </Link>
        </div>
      </main>
    );
  }
  const details = service.details;
  const title = isArabic
    ? service.titleAr || service.nameAr || service.title || service.name
    : service.title || service.name;

  const intro = isArabic
    ? service.descriptionAr || service.description
    : service.description;

  const articleSections = {
    about: isArabic
      ? details.aboutTitleAr || details.aboutTitle
      : details.aboutTitle,
    aboutText: isArabic
      ? details.aboutTextAr || details.aboutText
      : details.aboutText,
    audience: isArabic
      ? details.audienceTitleAr || details.audienceTitle
      : details.audienceTitle,
    audienceDescription: isArabic
      ? details.audienceDescriptionAr || details.audienceDescription
      : details.audienceDescription,
    audienceItems: isArabic
      ? details.audienceItemsAr?.filter(Boolean) ?? []
      : details.audienceItems?.filter(Boolean) ?? [],
    included: isArabic
      ? details.includedTitleAr || details.includedTitle
      : details.includedTitle,
    includedDescription: isArabic
      ? details.includedDescriptionAr || details.includedDescription
      : details.includedDescription,
    includedItems: isArabic
      ? details.includedItemsAr?.filter(Boolean) ?? []
      : details.includedItems?.filter(Boolean) ?? [],
    process: isArabic
      ? details.processTitleAr || details.processTitle
      : details.processTitle,
    processDescription: isArabic
      ? details.processDescriptionAr || details.processDescription
      : details.processDescription,
    faq: isArabic
      ? details.faqTitleAr || details.faqTitle
      : details.faqTitle,
    faqDescription: isArabic
      ? details.faqDescriptionAr || details.faqDescription
      : details.faqDescription,
  };

  const steps = (details.steps ?? [])
    .map((step, index) => ({
      number: String(index + 1).padStart(2, "0"),
      title: isArabic ? step.titleAr || step.title : step.title,
      description: isArabic
        ? step.descriptionAr || step.description
        : step.description,
    }))
    .filter((step) => step.title || step.description);

  const faqs = (details.faqs ?? [])
    .map((faq) => ({
      question: isArabic ? faq.questionAr || faq.question : faq.question,
      answer: isArabic ? faq.answerAr || faq.answer : faq.answer,
    }))
    .filter((faq) => faq.question || faq.answer);
  return (
    <main className="service-article-page">
      <div className="container">
        <nav className="service-breadcrumb" aria-label="Breadcrumb">
          <Link to="/">{isArabic ? "الرئيسية" : "Home"}</Link>
          <span>/</span>
          <Link to="/#services">{isArabic ? "الخدمات" : "Services"}</Link>
          <span>/</span>
          <span>{title}</span>
        </nav>

        <section className="service-editorial-hero">
          <div className="service-hero-copy">
            <span className="service-kicker">
              <Sparkles size={15} />
              {isArabic ? "رعاية تصل إليك" : "CARE THAT COMES HOME"}
            </span>
            <h1>{title}</h1>
            <p>{intro}</p>
            <div className="service-hero-actions">
              <Link
                to={`/booking?serviceId=${encodeURIComponent(service.id)}`}
                className="service-primary-button"
              >
                {isArabic ? "احجز الخدمة الآن" : "Book this service"}
                <ArrowUpRight size={19} />
              </Link>
              <a href={`tel:${PHONE_NUMBER}`} className="service-phone-link">
                <Phone size={17} />
                {isArabic ? "تحدث مع فريقنا" : "Talk to our team"}
              </a>
            </div>
            <div className="service-hero-trust">
              <span><ShieldCheck size={17} />{isArabic ? "اهتمام باحتياجاتك" : "Care around your needs"}</span>
              <span><HeartHandshake size={17} />{isArabic ? "في راحة منزلك" : "In your own home"}</span>
            </div>
          </div>

          <div className="service-hero-art" aria-hidden="true">
            <div className="service-art-orbit service-art-orbit-one" />
            <div className="service-art-orbit service-art-orbit-two" />
            <div className="service-art-main-icon"><Icon size={76} strokeWidth={1.35} /></div>
            <div className="service-art-note service-art-note-top">
              <span className="service-art-note-icon"><CheckCircle2 size={18} /></span>
              <span>{isArabic ? "رعاية باهتمام" : "Thoughtful care"}</span>
            </div>
            <div className="service-art-note service-art-note-bottom">
              <HeartPulse size={19} />
              <span>{isArabic ? "الرعاية تبدأ من المنزل" : "Care starts at home"}</span>
            </div>
            <div className="service-art-dot service-art-dot-one" />
            <div className="service-art-dot service-art-dot-two" />
          </div>
        </section>

        <div className="service-article-layout">
          <article className="service-article-body">
            <div className="service-article-meta">
              <span>{isArabic ? "دليل الخدمة" : "SERVICE GUIDE"}</span>
              <span className="service-meta-divider" />
              <span>{isArabic ? "اقرأ عن الخدمة" : "YOUR GUIDE TO CARE"}</span>
            </div>

            {(articleSections.about || articleSections.aboutText) && (
              <section className="service-article-section">
                <span className="service-section-number">01 /</span>
                <h2>{articleSections.about || (isArabic ? "عن الخدمة" : "About this service")}</h2>
                {articleSections.aboutText && (
                  <p className="service-article-lead">{articleSections.aboutText}</p>
                )}
              </section>
            )}

            {(articleSections.audience || articleSections.audienceDescription || articleSections.audienceItems.length > 0) && (
              <section className="service-article-section service-tinted-section">
                <span className="service-section-number">02 /</span>
                <h2>{articleSections.audience || (isArabic ? "لمن تناسب هذه الخدمة؟" : "Who is this service for?")}</h2>
                {articleSections.audienceDescription && <p>{articleSections.audienceDescription}</p>}
                {articleSections.audienceItems.length > 0 && (
                  <ul className="service-editorial-list">
                    {articleSections.audienceItems.map((item, index) => (
                      <li key={`${index}-${item}`}>
                        <CheckCircle2 size={19} />
                        {item}
                      </li>
                    ))}
                  </ul>
                )}
              </section>
            )}

            {(articleSections.included || articleSections.includedDescription || articleSections.includedItems.length > 0) && (
              <section className="service-article-section">
                <span className="service-section-number">03 /</span>
                <h2>{articleSections.included || (isArabic ? "ماذا تشمل الخدمة؟" : "What's included?")}</h2>
                {articleSections.includedDescription && <p>{articleSections.includedDescription}</p>}
                {articleSections.includedItems.length > 0 && (
                  <div className="service-included-grid">
                    {articleSections.includedItems.map((item, index) => (
                      <div className="service-included-item" key={`${index}-${item}`}>
                        <span>{String(index + 1).padStart(2, "0")}</span>
                        <p>{item}</p>
                      </div>
                    ))}
                  </div>
                )}
              </section>
            )}

            {(articleSections.process || articleSections.processDescription || steps.length > 0) && (
              <section className="service-article-section service-process-section">
                <span className="service-section-number">04 /</span>
                <h2>{articleSections.process || (isArabic ? "كيف تبدأ رحلتك معنا؟" : "How does it work?")}</h2>
                {articleSections.processDescription && <p>{articleSections.processDescription}</p>}
                {steps.length > 0 && (
                  <div className="service-steps">
                    {steps.map((step) => (
                      <div className="service-step" key={step.number}>
                        <span className="service-step-number">{step.number}</span>
                        <div>
                          {step.title && <h3>{step.title}</h3>}
                          {step.description && <p>{step.description}</p>}
                        </div>
                      </div>
                    ))}
                  </div>
                )}
              </section>
            )}

            {(articleSections.faq || articleSections.faqDescription || faqs.length > 0) && (
              <section className="service-article-section service-faq-section">
                <span className="service-section-number">05 /</span>
                <h2>{articleSections.faq || (isArabic ? "الأسئلة الشائعة" : "Frequently asked questions")}</h2>
                {articleSections.faqDescription && <p>{articleSections.faqDescription}</p>}
                {faqs.length > 0 && (
                  <div className="service-faq-list">
                    {faqs.map((faq, index) => (
                      <details className="service-faq-item" key={`${index}-${faq.question}`}>
                        <summary>
                          {faq.question}
                          <span><ArrowRight size={17} /></span>
                        </summary>
                        <p>{faq.answer}</p>
                      </details>
                    ))}
                  </div>
                )}
              </section>
            )}
          </article>

          <aside className="service-article-sidebar">
            <div className="service-booking-card">
              <span className="service-card-eyebrow">
                {isArabic ? "ابدأ من هنا" : "GET STARTED"}
              </span>
              <div className="service-card-icon"><Icon size={28} /></div>
              <h2>{title}</h2>
              <p>
                {isArabic
                  ? "أرسل طلبك وسيساعدك فريقنا في تأكيد التفاصيل."
                  : "Send your request and our team will help confirm the details."}
              </p>

              <Link
                to={`/booking?serviceId=${encodeURIComponent(service.id)}`}
                className="service-primary-button service-card-button"
              >
                {isArabic ? "احجز الآن" : "Book now"}
                <ArrowUpRight size={18} />
              </Link>
              <a href={`tel:${PHONE_NUMBER}`} className="service-card-call">
                <Phone size={17} />
                {isArabic ? "اتصل للاستفسار" : "Call to enquire"}
              </a>
              <div className="service-card-divider" />
              <div className="service-card-detail">
                <Clock3 size={17} />
                <span>{isArabic ? "المواعيد حسب التوافر" : "Scheduling subject to availability"}</span>
              </div>
              <div className="service-card-detail">
                <MapPin size={17} />
                <span>{isArabic ? "حسب مناطق التغطية" : "Available coverage areas"}</span>
              </div>
              <div className="service-card-detail">
                <ShieldCheck size={17} />
                <span>{isArabic ? "تأكيد التفاصيل قبل الزيارة" : "Details confirmed before the visit"}</span>
              </div>
            </div>

            <div className="service-sidebar-note">
              <HeartHandshake size={23} />
              <h3>{isArabic ? "هل تحتاج إلى مساعدة؟" : "Need some guidance?"}</h3>
              <p>
                {isArabic
                  ? "فريقنا جاهز لمساعدتك في معرفة المزيد عن الخدمة."
                  : "Our team can help you understand the service before you book."}
              </p>
              <a href={`tel:${PHONE_NUMBER}`}>
                {isArabic ? "تواصل معنا" : "Contact our team"}
                <ArrowUpRight size={16} />
              </a>
            </div>
          </aside>
        </div>

        {relatedServices.length > 0 && (
          <section className="service-related-section">
            <div className="service-related-heading">
              <div>
                <span className="service-kicker">
                  {isArabic ? "اكتشف المزيد" : "EXPLORE MORE"}
                </span>
                <h2>{isArabic ? "خدمات أخرى قد تهمك" : "Explore our other services"}</h2>
              </div>
              <Link to="/#services">
                {isArabic ? "كل الخدمات" : "All services"}
                <ArrowUpRight size={17} />
              </Link>
            </div>
            <div className="service-related-grid">
              {relatedServices.slice(0, 3).map((item) => (
                <Link
                  to={`/services/${item.id}`}
                  className="service-related-card"
                  key={item.id}
                >
                  <span className="service-related-icon"><HeartHandshake size={23} /></span>
                  <h3>{item.name}</h3>
                  <p>{item.description}</p>
                  <span className="service-related-link">
                    {isArabic ? "اكتشف الخدمة" : "Explore service"}
                    <ArrowUpRight size={17} />
                  </span>
                </Link>
              ))}
            </div>
          </section>
        )}
      </div>
    </main>
  );
}












