import { useState, type FormEvent } from "react";
import { useNavigate } from "react-router-dom";
import { useTranslation } from "react-i18next";
import { Activity, LockKeyhole, Mail } from "lucide-react";
import { useAuth } from "../context/AuthContext";
import { ApiError } from "../services/apiClient";

export default function AdminLoginPage() {
  const navigate = useNavigate();
  const { i18n } = useTranslation();
  const { loginUser, logoutUser } = useAuth();

  const isArabic = i18n.language === "ar";

  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError("");
    setLoading(true);

    try {
      const auth = await loginUser({ email: email.trim(), password });

      if (auth.role?.toLowerCase() !== "admin") {
        logoutUser();
        setError(
          isArabic
            ? "هذا الحساب لا يملك صلاحية دخول لوحة الإدارة."
            : "This account does not have administrator access."
        );
        return;
      }

      navigate("/", { replace: true });
    } catch (err) {
      setError(
        err instanceof ApiError
          ? err.message
          : isArabic
            ? "تعذر تسجيل الدخول. حاول مرة أخرى."
            : "Unable to sign in. Please try again."
      );
    } finally {
      setLoading(false);
    }
  }

  return (
    <main className="admin-login-page">
      <section className="admin-login-card">
        <div className="admin-login-brand">
          <span className="admin-login-logo">
            <Activity size={28} />
          </span>
          <div>
            <h1>TAMREED</h1>
            <p>{isArabic ? "لوحة الإدارة" : "ADMINISTRATION"}</p>
          </div>
        </div>

        <div className="admin-login-heading">
          <h2>{isArabic ? "مرحباً بعودتك" : "Welcome back"}</h2>
          <p>
            {isArabic
              ? "سجل الدخول باستخدام حساب المدير للمتابعة."
              : "Sign in with your administrator account to continue."}
          </p>
        </div>

        <form onSubmit={handleSubmit} className="admin-login-form">
          <label htmlFor="admin-email">
            {isArabic ? "البريد الإلكتروني" : "Email address"}
          </label>
          <div className="admin-login-input">
            <Mail size={18} />
            <input
              id="admin-email"
              type="email"
              autoComplete="username"
              placeholder="admin@example.com"
              value={email}
              onChange={(event) => setEmail(event.target.value)}
              required
            />
          </div>

          <label htmlFor="admin-password">
            {isArabic ? "كلمة المرور" : "Password"}
          </label>
          <div className="admin-login-input">
            <LockKeyhole size={18} />
            <input
              id="admin-password"
              type="password"
              autoComplete="current-password"
              placeholder={isArabic ? "أدخل كلمة المرور" : "Enter your password"}
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              required
            />
          </div>

          {error && (
            <div className="admin-login-error" role="alert">
              {error}
            </div>
          )}

          <button type="submit" disabled={loading}>
            {loading
              ? isArabic ? "جارٍ تسجيل الدخول..." : "Signing in..."
              : isArabic ? "تسجيل الدخول" : "Sign in"}
          </button>
        </form>

        <button
          type="button"
          className="admin-login-language"
          onClick={() => void i18n.changeLanguage(isArabic ? "en" : "ar")}
        >
          {isArabic ? "English" : "العربية"}
        </button>
      </section>
    </main>
  );
}
