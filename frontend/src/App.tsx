import { useEffect } from "react";
import { Navigate, Route, Routes } from "react-router-dom";
import { useTranslation } from "react-i18next";

import TopStrip from "./components/layout/TopStrip";
import Header from "./components/layout/Header";
import Footer from "./components/layout/Footer";
import SeoManager from "./components/SeoManager";

import ProtectedRoute from "./components/ProtectedRoute";
import AdminRoute from "./components/AdminRoute";

import HeroSection from "./components/sections/HeroSection";
import PromotionSection from "./components/sections/PromotionSection";
import ServicesSection from "./components/sections/ServicesSection";
import AboutSection from "./components/sections/AboutSection";
import ProcessSection from "./components/sections/ProcessSection";
import WhySection from "./components/sections/WhySection";
import CoverageSection from "./components/sections/CoverageSection";
import TestimonialsSection from "./components/sections/TestimonialsSection";
import FaqSection from "./components/sections/FaqSection";
import CtaSection from "./components/sections/CtaSection";
import ContactSection from "./components/sections/ContactSection";

import AuthPage from "./pages/AuthPage";
import BookingPage from "./pages/BookingPage";
import MyBookingsPage from "./pages/MyBookingsPage";
import ServiceDetailsPage from "./pages/ServiceDetailsPage";
import AdminDashboardPage from "./pages/AdminDashboardPage";

function HomePage() {
  return (
    <div className="site-shell">
      <TopStrip />
      <Header />
      <main>
        <HeroSection />
        <PromotionSection />
        <ServicesSection />
        <AboutSection />
        <ProcessSection />
        <WhySection />
        <CoverageSection />
        <TestimonialsSection />
        <FaqSection />
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
    <>
      <SeoManager />
      <>
      <Routes>
      <Route path="/" element={<HomePage />} />
      <Route path="/services/:id" element={<ServiceDetailsPage />} />
      <Route path="/login" element={<AuthPage mode="login" />} />
      <Route path="/register" element={<AuthPage mode="register" />} />

      <Route element={<ProtectedRoute />}>
        <Route path="/booking" element={<BookingPage />} />
        <Route path="/my-bookings" element={<MyBookingsPage />} />
      </Route>

      <Route element={<AdminRoute />}>
        <Route path="/admin" element={<AdminDashboardPage />} />
      </Route>

      <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
    </>
    </>
  );
}

export default App;



