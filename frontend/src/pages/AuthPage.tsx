import { useState, type FormEvent } from "react";
import { Link, useLocation, useNavigate } from "react-router-dom";
import { HeartPulse, ArrowLeft, ArrowRight } from "lucide-react";
import { useTranslation } from "react-i18next";
import { ApiError } from "../services/apiClient";
import { useAuth } from "../context/AuthContext";

interface AuthPageProps {
  mode: "login" | "register";
}

interface RedirectLocation {
  pathname?: string;
  search?: string;
  hash?: string;
}

interface AuthLocationState {
  from?: RedirectLocation;
}

export default function AuthPage({ mode }: AuthPageProps) {
  const { t, i18n } = useTranslation();
  const navigate = useNavigate();
  const location = useLocation();
  const { loginUser, registerUser } = useAuth();

  const isArabic = i18n.language === "ar";
  const isRegister = mode === "register";
  const Arrow = isArabic ? ArrowRight : ArrowLeft;

  const locationState = location.state as AuthLocationState | null;
  const from = locationState?.from;

  const redirectTo = from
    ? `${from.pathname ?? "/"}${from.search ?? ""}${from.hash ?? ""}`
    : "/";

  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState("");
  const [isLoading, setIsLoading] = useState(false);

  async function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault();
    setError("");
    setIsLoading(true);

    try {
      if (isRegister) {
        await registerUser({
          firstName: firstName.trim(),
          lastName: lastName.trim(),
          email: email.trim(),
          password,
        });
      } else {
        await loginUser({
          email: email.trim(),
          password,
        });
      }

      navigate(redirectTo, { replace: true });
    } catch (err) {
      if (err instanceof ApiError) {
        setError(err.message);
      } else if (err instanceof Error) {
        setError(err.message);
      } else {
        setError(t("auth.genericError"));
      }
    } finally {
      setIsLoading(false);
    }
  }

  return (
    <section className="auth-section">
      <div className="auth-card">
        <Link to="/" className="auth-back-link">
          <Arrow size={16} />
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
          <h1>
            {isRegister
              ? t("auth.registerTitle")
              : t("auth.loginTitle")}
          </h1>
          <p>
            {isRegister
              ? t("auth.registerDescription")
              : t("auth.loginDescription")}
          </p>
        </div>

        {error && (
          <div className="auth-error" role="alert">
            {error}
          </div>
        )}

        <form className="auth-form" onSubmit={handleSubmit}>
          {isRegister && (
            <>
              <div className="auth-field">
                <label htmlFor="firstName">
                  {t("auth.firstName")}
                </label>
                <input
                  id="firstName"
                  type="text"
                  autoComplete="given-name"
                  value={firstName}
                  onChange={(event) => setFirstName(event.target.value)}
                  required
                  maxLength={100}
                />
              </div>

              <div className="auth-field">
                <label htmlFor="lastName">
                  {t("auth.lastName")}
                </label>
                <input
                  id="lastName"
                  type="text"
                  autoComplete="family-name"
                  value={lastName}
                  onChange={(event) => setLastName(event.target.value)}
                  required
                  maxLength={100}
                />
              </div>
            </>
          )}

          <div className="auth-field">
            <label htmlFor="email">{t("auth.email")}</label>
            <input
              id="email"
              type="email"
              autoComplete="email"
              value={email}
              onChange={(event) => setEmail(event.target.value)}
              required
            />
          </div>

          <div className="auth-field">
            <label htmlFor="password">{t("auth.password")}</label>
            <input
              id="password"
              type="password"
              autoComplete={
                isRegister ? "new-password" : "current-password"
              }
              value={password}
              onChange={(event) => setPassword(event.target.value)}
              required
              minLength={isRegister ? 8 : undefined}
            />
            {isRegister && (
              <small className="auth-hint">
                {t("auth.passwordHint")}
              </small>
            )}
          </div>

          <button
            type="submit"
            className="button button-primary auth-submit"
            disabled={isLoading}
          >
            {isLoading
              ? t("auth.loading")
              : isRegister
                ? t("auth.register")
                : t("auth.login")}
          </button>
        </form>

        <p className="auth-switch">
          {isRegister
            ? t("auth.haveAccount")
            : t("auth.noAccount")}{" "}
          <Link
            to={isRegister ? "/login" : "/register"}
            state={location.state}
          >
            {isRegister
              ? t("auth.loginLink")
              : t("auth.createAccount")}
          </Link>
        </p>
      </div>
    </section>
  );
}

