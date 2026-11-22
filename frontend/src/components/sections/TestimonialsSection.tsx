import { useTranslation } from "react-i18next";
import { HeartHandshake, MessageCircle, ShieldCheck } from "lucide-react";

export default function TestimonialsSection() {
  const { i18n } = useTranslation();
  const isArabic = i18n.language.startsWith("ar");

  const items = isArabic
    ? [
        { icon: HeartHandshake, title: "التمريض المنزلي", description: "ستُعرض هنا آراء العملاء بعد جمعها والتحقق منها والحصول على موافقتهم." },
        { icon: ShieldCheck, title: "العلاج الطبيعي", description: "نؤمن بأهمية مشاركة تجارب حقيقية تعكس جودة الرعاية والاهتمام." },
        { icon: MessageCircle, title: "الرعاية الشخصية", description: "نعمل على جمع آراء العملاء لنشارككم تجاربهم بكل شفافية." },
      ]
    : [
        { icon: HeartHandshake, title: "Home Nursing", description: "Customer feedback will appear here once it has been collected, verified, and approved for sharing." },
        { icon: ShieldCheck, title: "Physiotherapy", description: "We believe in sharing genuine experiences that reflect the quality and care we strive to provide." },
        { icon: MessageCircle, title: "Personalized Care", description: "We are collecting customer feedback so we can share real experiences with you transparently." },
      ];

  return (
    <section className="section testimonials-section" id="testimonials">
      <div className="container">
        <div className="testimonials-heading">
          <div>
            <div className="eyebrow">
              <span className="eyebrow-line" />
              {isArabic ? "تجارب تثق بها" : "YOUR TRUST MATTERS"}
            </div>
            <h2>
              {isArabic
                ? "ثقة نفخر بها"
                : "Trust we are proud of"}
            </h2>
          </div>
          <p>
            {isArabic
              ? "ثقتكم مسؤولية نعتز بها. سنشارك هنا آراء وتجارب عملائنا الحقيقية بعد موافقتهم."
              : "Your trust means everything to us. Real customer experiences will be shared here with their permission."}
          </p>
        </div>

        <div className="testimonials-grid">
          {items.map((item) => {
            const Icon = item.icon;

            return (
              <article className="testimonial-card" key={item.title}>
                <div className="testimonial-icon">
                  <Icon size={23} strokeWidth={1.7} />
                </div>
                <span className="testimonial-label">
                  {isArabic ? "قريباً" : "COMING SOON"}
                </span>
                <h3>{item.title}</h3>
                <p>{item.description}</p>
              </article>
            );
          })}
        </div>
      </div>
    </section>
  );
}