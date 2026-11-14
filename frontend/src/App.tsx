import { useEffect } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import { useTranslation } from "react-i18next";

import TopStrip from "./components/layout/TopStrip";
import Header from "./components/layout/Header";
import Footer from "./components/layout/Footer";
import ProtectedRoute from "./components/ProtectedRoute";

import HeroSection from "./components/sections/HeroSection";
import ServicesSection from "./components/sections/ServicesSection";
import AboutSection from "./components/sections/AboutSection";
import ProcessSection from "./components/sections/ProcessSection";
import WhySection from "./components/sections/WhySection";
import CtaSection from "./components/sections/CtaSection";
import ContactSection from "./components/sections/ContactSection";

import AuthPage from "./pages/AuthPage";
import BookingPage from "./pages/BookingPage";
import MyBookingsPage from "./pages/MyBookingsPage";

function HomePage() {
  return (
    <div className="site-shell">
      <TopStrip />
      <Header />

      <main>
        <HeroSection />
        <ServicesSection />
        <AboutSection />
        <ProcessSection />
        <WhySection />
        <CtaSection />
        <ContactSection />
      </main>

      <Footer />
    </div>
  );
}

function App() {
  const { i18n } = useTranslation();
  const isArabic = i18n.language === "ar";

  useEffect(() => {
    document.documentElement.lang = isArabic ? "ar" : "en";
    document.documentElement.dir = isArabic ? "rtl" : "ltr";
    localStorage.setItem("tamreed-language", isArabic ? "ar" : "en");
  }, [isArabic]);

  return (
    <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/login" element={<AuthPage mode="login" />} />
      <Route path="/register" element={<AuthPage mode="register" />} />

      <Route element={<ProtectedRoute />}>
        <Route path="/booking" element={<BookingPage />} />
        <Route path="/my-bookings" element={<MyBookingsPage />} />
      </Route>

      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}

export default App;
