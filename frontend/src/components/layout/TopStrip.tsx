import { useTranslation } from "react-i18next";
import { Heart } from "lucide-react";

export default function TopStrip() {
  const { t } = useTranslation();

  return (
    <div className="top-strip">
      <div className="container top-strip-inner">
        <span>
          <Heart size={14} />
          {t("hero.note")}
        </span>
        <span className="top-strip-right">
          <span className="availability-dot" />
          {t("hero.badge")}
        </span>
      </div>
    </div>
  );
}