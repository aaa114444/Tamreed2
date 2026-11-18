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

    const normalizedName = service.name.toLowerCase();
    const isPhysio = normalizedName.includes("physio");
    const isNursing = normalizedName.includes("nursing");

    const pageTitle = isArabic
      ? isPhysio
        ? "العلاج الطبيعي في المنزل"
        : isNursing
          ? "التمريض المنزلي"
          : service.name
      : service.name;

    document.title = `${pageTitle} | Care Plus`;

    const description = isArabic
      ? isPhysio
        ? "خدمات العلاج الطبيعي في المنزل من كير بلس، بخطة رعاية تناسب احتياجاتك وتساعدك على استعادة الحركة."
        : isNursing
          ? "خدمات التمريض المنزلي من كير بلس، رعاية مهنية واهتمام باحتياجاتك في راحة منزلك."
          : service.description
      : service.description ||
        "Discover professional home healthcare services from Care Plus.";

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

  const title = isArabic
    ? isPhysio ? "العلاج الطبيعي في المنزل" : isNursing ? "التمريض المنزلي" : service.name
    : service.name;

  const intro = isArabic
    ? isPhysio
      ? "استعد حركتك وثقتك بخدمة علاج طبيعي مصممة لتناسب احتياجاتك، في راحة منزلك وبما يتوافق مع خطتك العلاجية."
      : isNursing
        ? "احصل على رعاية تمريضية منزلية باهتمام واحترام لاحتياجاتك، مع تجربة أكثر راحة لك ولعائلتك."
        : service.description
    : isPhysio
      ? "Make everyday movement feel more achievable with at-home physiotherapy designed around your needs, comfort and recovery goals."
      : isNursing
        ? "Receive attentive nursing support in the comfort of your home, with care that respects your needs, routine and personal space."
        : service.description;

  const articleSections = isPhysio
    ? {
        about: isArabic
          ? "ما هي خدمة العلاج الطبيعي المنزلي؟"
          : "What is home physiotherapy?",
        aboutText: isArabic
          ? "العلاج الطبيعي المنزلي يتيح لك الحصول على جلسات علاجية في بيئة مألوفة ومريحة. يتم تحديد طبيعة الجلسة والأهداف المناسبة وفق تقييم المختص وحالة كل شخص."
          : "Home physiotherapy brings rehabilitation support into a familiar environment. The session plan and goals should be determined by a qualified professional following an assessment of your individual needs.",
        audience: isArabic
          ? "لمن تناسب هذه الخدمة؟"
          : "Who may benefit from this service?",
        audienceItems: isArabic
          ? ["الأشخاص الذين يحتاجون إلى دعم في الحركة والتنقل", "من يتبعون خطة تأهيل بعد إصابة أو إجراء طبي", "من يفضلون تلقي جلساتهم العلاجية في المنزل"]
          : ["People who need support with movement and mobility", "Those following a rehabilitation plan after an injury or procedure", "People who prefer receiving sessions in their own home"],
        included: isArabic
          ? "ماذا تتضمن الخدمة؟"
          : "What can a session include?",
        includedItems: isArabic
          ? ["مناقشة الاحتياجات والأهداف مع المختص", "تقييم مناسب للحركة وفق طبيعة الخدمة", "تمارين وأنشطة علاجية حسب الخطة", "إرشادات تساعدك على متابعة خطتك"]
          : ["A discussion of your needs and goals", "An appropriate movement assessment", "Exercises and activities based on your plan", "Guidance to help you follow your agreed plan"],
      }
    : {
        about: isArabic
          ? "ما هي خدمة التمريض المنزلي؟"
          : "What is home nursing?",
        aboutText: isArabic
          ? "توفر خدمة التمريض المنزلي دعماً تمريضياً داخل المنزل، بما يساعد المريض على تلقي الرعاية في بيئة مألوفة. يتم تحديد طبيعة الرعاية المطلوبة وفق احتياجات الحالة وتفاصيل الحجز."
          : "Home nursing provides nursing support in the patient's own home. It can make receiving care in a familiar environment more convenient. The specific care required should be discussed and confirmed according to the patient's needs.",
        audience: isArabic
          ? "لمن تناسب هذه الخدمة؟"
          : "Who may benefit from this service?",
        audienceItems: isArabic
          ? ["الأشخاص الذين يحتاجون إلى دعم تمريضي في المنزل", "العائلات التي تبحث عن مساندة في رعاية أحد أفرادها", "من يفضلون تلقي الرعاية في بيئة منزلية"]
          : ["People who need nursing support at home", "Families looking for additional support in caring for a loved one", "Those who prefer receiving care in a familiar home environment"],
        included: isArabic
          ? "ماذا تتضمن الخدمة؟"
          : "What can the service include?",
        includedItems: isArabic
          ? ["مراجعة احتياجات المريض قبل تقديم الخدمة", "مناقشة تفاصيل الرعاية المطلوبة", "تقديم الدعم المتفق عليه وفق نطاق الخدمة", "إرشادات للمريض أو أفراد الأسرة عند الحاجة"]
          : ["Reviewing the patient's needs before the service", "Discussing the required care arrangements", "Providing agreed support within the service scope", "Guidance for the patient or family when appropriate"],
      };

  const steps = isArabic
    ? [
        ["01", "اختر الخدمة", "راجع تفاصيل الخدمة وحدد ما يناسب احتياجاتك."],
        ["02", "أرسل طلب الحجز", "أدخل بياناتك والعنوان والموعد المناسب."],
        ["03", "تأكيد التفاصيل", "سيتواصل معك فريقنا لمراجعة طلبك وتأكيد التفاصيل."],
        ["04", "استقبل الرعاية", "يتم تنسيق تقديم الخدمة وفق التفاصيل المؤكدة."],
      ]
    : [
        ["01", "Choose your service", "Explore the details and select the service that suits your needs."],
        ["02", "Submit a booking", "Share your contact details, address and preferred time."],
        ["03", "Confirm the details", "Our team will contact you to review and confirm your request."],
        ["04", "Receive your care", "Your service is coordinated according to the confirmed arrangements."],
      ];

  const faqs = isArabic
    ? [
        ["كيف يمكنني حجز الخدمة؟", "اضغط على زر احجز الآن، ثم أدخل بيانات الحجز المطلوبة. سيتواصل معك فريقنا لمراجعة التفاصيل."],
        ["هل يتم تقديم الخدمة في المنزل؟", "نعم، هذه الخدمة مخصصة للتقديم المنزلي، مع مراعاة مناطق التغطية المتاحة."],

        ["هل يمكنني الاستفسار قبل الحجز؟", "بالتأكيد. يمكنك الاتصال بفريقنا لمناقشة احتياجاتك والاستفسار عن الخدمة."],
      ]
    : [
        ["How do I book this service?", "Select Book Now and complete the requested booking details. Our team will contact you to review your request."],
        ["Is this an at-home service?", "Yes. This service is intended to be provided at home, subject to available coverage areas."],

        ["Can I ask questions before booking?", "Of course. You can call our team to discuss your needs and ask questions about the service."],
      ];

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

            <section className="service-article-section">
              <span className="service-section-number">01 /</span>
              <h2>{articleSections.about}</h2>
              <p className="service-article-lead">{articleSections.aboutText}</p>
              <p>
                {isArabic
                  ? "في كير بلس، نؤمن أن تجربة الرعاية الجيدة تبدأ بفهم احتياجات كل شخص والتواصل الواضح قبل تقديم الخدمة."
                  : "At Care Plus, we believe a better care experience begins with understanding each person's needs and communicating clearly before the service takes place."}
              </p>
            </section>

            <section className="service-article-section service-tinted-section">
              <span className="service-section-number">02 /</span>
              <h2>{articleSections.audience}</h2>
              <p>
                {isArabic
                  ? "قد تكون الخدمة مناسبة في مواقف مختلفة. من المهم مناقشة احتياجات الحالة وتوقعاتها قبل تأكيد الحجز."
                  : "This service may be suitable in different circumstances. Discuss the individual's needs and expectations with our team before confirming a booking."}
              </p>
              <ul className="service-editorial-list">
                {articleSections.audienceItems.map((item) => (
                  <li key={item}><CheckCircle2 size={19} />{item}</li>
                ))}
              </ul>
            </section>

            <section className="service-article-section">
              <span className="service-section-number">03 /</span>
              <h2>{articleSections.included}</h2>
              <p>
                {isArabic
                  ? "تختلف التفاصيل حسب احتياجات المريض ونوع الخدمة التي يتم الاتفاق عليها."
                  : "The exact details depend on the patient's needs and the specific service arrangements agreed before the visit."}
              </p>
              <div className="service-included-grid">
                {articleSections.includedItems.map((item, index) => (
                  <div className="service-included-item" key={item}>
                    <span>0{index + 1}</span>
                    <p>{item}</p>
                  </div>
                ))}
              </div>
            </section>

            <section className="service-article-section service-process-section">
              <span className="service-section-number">04 /</span>
              <h2>{isArabic ? "كيف تبدأ رحلتك معنا؟" : "How does it work?"}</h2>
              <p>
                {isArabic
                  ? "جعلنا خطوات طلب الخدمة واضحة وبسيطة قدر الإمكان."
                  : "We've made the process of requesting care as clear and straightforward as possible."}
              </p>
              <div className="service-steps">
                {steps.map(([number, heading, description]) => (
                  <div className="service-step" key={number}>
                    <span className="service-step-number">{number}</span>
                    <div>
                      <h3>{heading}</h3>
                      <p>{description}</p>
                    </div>
                  </div>
                ))}
              </div>
            </section>

            <section className="service-article-section service-faq-section">
              <span className="service-section-number">05 /</span>
              <h2>{isArabic ? "أسئلة شائعة" : "Frequently asked questions"}</h2>
              <p>
                {isArabic
                  ? "إليك إجابات عن بعض الأسئلة التي قد تساعدك قبل إرسال طلبك."
                  : "Here are answers to some common questions that may help before you submit your request."}
              </p>
              <div className="service-faq-list">
                {faqs.map(([question, answer]) => (
                  <details className="service-faq-item" key={question}>
                    <summary>
                      {question}
                      <span><ArrowRight size={17} /></span>
                    </summary>
                    <p>{answer}</p>
                  </details>
                ))}
              </div>
            </section>
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







