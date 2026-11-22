import { useState } from "react";
import { useTranslation } from "react-i18next";
import { ChevronDown } from "lucide-react";

export default function FaqSection() {
  const { i18n } = useTranslation();
  const isArabic = i18n.language.startsWith("ar");
  const [openIndex, setOpenIndex] = useState<number | null>(0);

  const questions = isArabic
    ? [
        {
          question: "ما الخدمات التي تقدمها كير بلس؟",
          answer: "نقدم خدمات التمريض المنزلي والعلاج الطبيعي والرعاية الشخصية، ويمكنك استكشاف تفاصيل الخدمات من خلال الموقع.",
        },
        {
          question: "هل يمكنني الحصول على الرعاية في منزلي؟",
          answer: "نعم، نركز على تقديم خدمات الرعاية الصحية المنزلية لتتمكن من الحصول على الدعم في بيئة مألوفة ومريحة.",
        },
        {
          question: "كيف يمكنني طلب خدمة؟",
          answer: "استكشف الخدمات المتاحة، واختر الخدمة المناسبة، ثم انتقل إلى صفحة الحجز لإرسال تفاصيل طلبك.",
        },
        {
          question: "هل تتوفر الخدمات في جميع المناطق؟",
          answer: "تختلف إمكانية تقديم الخدمة حسب المنطقة. تواصل معنا للتحقق من توفر الخدمة في عنوانك.",
        },
        {
          question: "كيف يمكنني متابعة حجوزاتي؟",
          answer: "يمكنك تسجيل الدخول إلى حسابك وفتح صفحة حجوزاتي للاطلاع على طلباتك وحالاتها.",
        },
      ]
    : [
        {
          question: "What services does Care Plus offer?",
          answer: "We offer Home Nursing, Physiotherapy, and Personalized Care. You can explore the service pages to learn more.",
        },
        {
          question: "Can I receive care at home?",
          answer: "Yes. Our focus is on home healthcare, helping you receive support in a familiar and comfortable environment.",
        },
        {
          question: "How can I request a service?",
          answer: "Explore the available services, choose the one that suits your needs, and visit the booking page to submit your request details.",
        },
        {
          question: "Are services available in every area?",
          answer: "Availability depends on your location. Contact us to check whether the service is available at your address.",
        },
        {
          question: "How can I track my bookings?",
          answer: "Sign in to your account and open My Bookings to view your requests and their current statuses.",
        },
      ];

  return (
    <section className="section faq-section" id="faq">
      <div className="container">
        <div className="faq-layout">
          <div className="faq-intro">
            <div className="eyebrow">
              <span className="eyebrow-line" />
              {isArabic ? "نحن هنا للإجابة" : "HERE TO HELP"}
            </div>
            <h2>
              {isArabic
                ? "إجابات تهمك قبل أن تبدأ"
                : "Answers that matter before you start"}
            </h2>
            <p>
              {isArabic
                ? "جمعنا لك إجابات عن الأسئلة الشائعة لمساعدتك على فهم خدماتنا وخطوات طلب الرعاية."
                : "Find helpful answers about our services and how to request the care you need."}
            </p>
            <a href="#contact" className="faq-contact-link">
              {isArabic ? "هل لديك سؤال آخر؟ تواصل معنا" : "Have another question? Contact us"}
            </a>
          </div>

          <div className="faq-list">
            {questions.map((item, index) => {
              const isOpen = openIndex === index;
              const answerId = `faq-answer-${index}`;

              return (
                <article className={`faq-item${isOpen ? " is-open" : ""}`} key={item.question}>
                  <button
                    className="faq-question"
                    type="button"
                    aria-expanded={isOpen}
                    aria-controls={answerId}
                    onClick={() => setOpenIndex(isOpen ? null : index)}
                  >
                    <span>{item.question}</span>
                    <ChevronDown size={19} />
                  </button>

                  {isOpen && (
                    <div className="faq-answer" id={answerId}>
                      <p>{item.answer}</p>
                    </div>
                  )}
                </article>
              );
            })}
          </div>
        </div>
      </div>
    </section>
  );
}