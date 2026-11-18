import { useCallback, useEffect, useMemo, useState } from "react";
import { Link } from "react-router-dom";
import { useTranslation } from "react-i18next";
import {
  Activity,
  ArrowLeft,
  ArrowRight,
  CalendarDays,
  Check,
  CheckCircle2,
  Clock3,
  LayoutDashboard,
  LogOut,
  MapPin,
  Package,
  Plus,
  RefreshCw,
  Save,
  Search,
  Users,
  X,
} from "lucide-react";

import { useAuth } from "../context/AuthContext";
import {
  getAllBookings,
  updateBookingStatus,
  type BookingAction,
} from "../services/bookingService";
import {
  createService,
  getServices,
  updateService,
  type SaveServiceRequest,
} from "../services/serviceService";

import type { BookingResponse } from "../types/booking";
import { getCustomers, setCustomerActiveStatus } from "../services/customerService";
import type { CustomerResponse } from "../types/customer";
import type { ServiceResponse } from "../types/service";

type DashboardTab = "overview" | "bookings" | "services" | "customers";
type BookingFilter = "all" | "pending" | "confirmed" | "inprogress" | "completed" | "cancelled";

const translations = {
  en: {
    dashboard: "Admin dashboard",
    subtitle: "Manage your home healthcare operations",
    overview: "Overview",
    bookings: "Bookings",
    services: "Services",
    customers: "Customers",
    manageCustomers: "Customer management",
    customerSearch: "Search by name or email...",
    noCustomers: "No customers found.",
    active: "Active",
    inactive: "Inactive",
    activateCustomer: "Activate account",
    deactivateCustomer: "Deactivate account",
    customerStatusUpdated: "Customer account status updated successfully.",
    confirmDeactivateCustomer: "Are you sure you want to deactivate this customer account?",
    registered: "Registered",
    lastLogin: "Last login",
    never: "Never logged in",
    customerCount: "Registered customers",
    bookingHistory: "Booking history",
    viewHistory: "View booking history",
    hideHistory: "Hide booking history",
    noCustomerBookings: "This customer has no bookings yet.",
    totalCustomerBookings: "Total bookings",
    completedCustomerBookings: "Completed",
    cancelledCustomerBookings: "Cancelled",
    welcome: "Welcome back",
    totalBookings: "Total bookings",
    pending: "Pending",
    confirmed: "Confirmed",
    completed: "Completed",
    recentBookings: "Recent bookings",
    allBookings: "All bookings",
    manageBookings: "Manage bookings",
    manageServices: "Manage services",
    customer: "Customer ID",
    service: "Service",
    date: "Scheduled date",
    address: "Address",
    status: "Status",
    actions: "Actions",
    confirm: "Confirm",
    start: "Start",
    complete: "Complete",
    cancel: "Cancel",
    all: "All",
    inprogress: "In progress",
    cancelled: "Cancelled",
    noBookings: "No bookings found.",
    noServices: "No services found.",
    refresh: "Refresh",
    search: "Search bookings...",
    serviceManagement: "Service management",
    addService: "Add service",
    editService: "Edit service",
    title: "Title",
    name: "Service name",
    points: "Points",
    addPoint: "Add point",
    removePoint: "Remove",
    description: "Description",
    save: "Save service",
    saving: "Saving...",
    cancelEdit: "Cancel",
    created: "Service created successfully.",
    updated: "Service updated successfully.",
    loading: "Loading dashboard...",
    error: "Something went wrong. Please try again.",
    logout: "Logout",
    backWebsite: "View website",
    noNotes: "No notes",
    bookingUpdated: "Booking updated successfully.",
    confirmCancel: "Are you sure you want to cancel this booking?",
    required: "Please complete all fields.",
    createdAt: "Created",
    actionsUnavailable: "No actions available",
    serviceCount: "Active services",
  },
  ar: {
    dashboard: "لوحة تحكم الإدارة",
    subtitle: "إدارة خدمات الرعاية الصحية المنزلية",
    overview: "نظرة عامة",
    bookings: "الحجوزات",
    services: "الخدمات",
    customers: "العملاء",
    manageCustomers: "إدارة العملاء",
    customerSearch: "ابحث بالاسم أو البريد الإلكتروني...",
    noCustomers: "لا يوجد عملاء.",
    active: "نشط",
    inactive: "غير نشط",
    activateCustomer: "تفعيل الحساب",
    deactivateCustomer: "إلغاء تفعيل الحساب",
    customerStatusUpdated: "تم تحديث حالة حساب العميل بنجاح.",
    confirmDeactivateCustomer: "هل أنت متأكد من إلغاء تفعيل حساب هذا العميل؟",
    registered: "تاريخ التسجيل",
    lastLogin: "آخر تسجيل دخول",
    never: "لم يسجل الدخول",
    customerCount: "العملاء المسجلون",
    bookingHistory: "سجل الحجوزات",
    viewHistory: "عرض سجل الحجوزات",
    hideHistory: "إخفاء سجل الحجوزات",
    noCustomerBookings: "لا توجد حجوزات لهذا العميل حتى الآن.",
    totalCustomerBookings: "إجمالي الحجوزات",
    completedCustomerBookings: "مكتملة",
    cancelledCustomerBookings: "ملغاة",
    welcome: "مرحباً بعودتك",
    totalBookings: "إجمالي الحجوزات",
    pending: "قيد الانتظار",
    confirmed: "تم التأكيد",
    completed: "مكتمل",
    recentBookings: "أحدث الحجوزات",
    allBookings: "كل الحجوزات",
    manageBookings: "إدارة الحجوزات",
    manageServices: "إدارة الخدمات",
    customer: "رقم العميل",
    service: "الخدمة",
    date: "موعد الحجز",
    address: "العنوان",
    status: "الحالة",
    actions: "الإجراءات",
    confirm: "تأكيد",
    start: "بدء",
    complete: "إنهاء",
    cancel: "إلغاء",
    all: "الكل",
    inprogress: "جارٍ التنفيذ",
    cancelled: "ملغي",
    noBookings: "لا توجد حجوزات.",
    noServices: "لا توجد خدمات.",
    refresh: "تحديث",
    search: "ابحث في الحجوزات...",
    serviceManagement: "إدارة الخدمات",
    addService: "إضافة خدمة",
    editService: "تعديل الخدمة",
    title: "العنوان",
    name: "اسم الخدمة",
    points: "النقاط",
    addPoint: "إضافة نقطة",
    removePoint: "حذف",
    description: "الوصف",
    save: "حفظ الخدمة",
    saving: "جارٍ الحفظ...",
    cancelEdit: "إلغاء",
    created: "تمت إضافة الخدمة بنجاح.",
    updated: "تم تحديث الخدمة بنجاح.",
    loading: "جارٍ تحميل لوحة التحكم...",
    error: "حدث خطأ. حاول مرة أخرى.",
    logout: "تسجيل الخروج",
    backWebsite: "عرض الموقع",
    noNotes: "لا توجد ملاحظات",
    bookingUpdated: "تم تحديث الحجز بنجاح.",
    confirmCancel: "هل أنت متأكد من إلغاء هذا الحجز؟",
    required: "يرجى إكمال جميع الحقول.",
    createdAt: "تاريخ الإضافة",
    actionsUnavailable: "لا توجد إجراءات متاحة",
    serviceCount: "الخدمات النشطة",
  },
};

export default function AdminDashboardPage() {
  const { i18n } = useTranslation();
  const { auth, logoutUser } = useAuth();
  const isArabic = i18n.language === "ar";
  const t = isArabic ? translations.ar : translations.en;
  const BackArrow = isArabic ? ArrowRight : ArrowLeft;

  const [activeTab, setActiveTab] = useState<DashboardTab>("overview");
  const [bookings, setBookings] = useState<BookingResponse[]>([]);
  const [services, setServices] = useState<ServiceResponse[]>([]);
  const [customers, setCustomers] = useState<CustomerResponse[]>([]);
  const [customerSearch, setCustomerSearch] = useState("");
  const [expandedCustomerId, setExpandedCustomerId] = useState<string | null>(null);
  const [loading, setLoading] = useState(true);
  const [refreshing, setRefreshing] = useState(false);
  const [error, setError] = useState("");
  const [notice, setNotice] = useState("");
  const [filter, setFilter] = useState<BookingFilter>("all");
  const [search, setSearch] = useState("");
  const [saving, setSaving] = useState(false);
  const [editingService, setEditingService] = useState<ServiceResponse | null>(null);
  const [showServiceForm, setShowServiceForm] = useState(false);
  const [serviceForm, setServiceForm] = useState<SaveServiceRequest>({ title: "", name: "", description: "", points: [] });

  const loadDashboard = useCallback(async (refresh = false) => {
    if (refresh) setRefreshing(true);
    else setLoading(true);

    try {
      const [bookingData, serviceData, customerData] = await Promise.all([
        getAllBookings(),
        getServices(),
        getCustomers(),
      ]);
      setBookings(bookingData);
      setServices(serviceData);
      setCustomers(customerData);
      setError("");
    } catch (err) {
      setError(err instanceof Error ? err.message : t.error);
    } finally {
      setLoading(false);
      setRefreshing(false);
    }
  }, [t.error]);

  useEffect(() => {
    void loadDashboard();
  }, [loadDashboard]);

  const counts = useMemo(() => ({
    total: bookings.length,
    pending: bookings.filter((b) => b.status.toLowerCase() === "pending").length,
    confirmed: bookings.filter((b) => b.status.toLowerCase() === "confirmed").length,
    completed: bookings.filter((b) => b.status.toLowerCase() === "completed").length,
  }), [bookings]);

  const filteredBookings = useMemo(() => {
    return bookings.filter((booking) => {
      const status = booking.status.toLowerCase();
      const matchesFilter = filter === "all" || status === filter;
      const serviceName = services.find((s) => s.id === booking.serviceId)?.name ?? "";
      const haystack = [
        booking.customerId,
        serviceName,
        booking.city,
        booking.district,
        booking.street,
        booking.status,
      ].join(" ").toLowerCase();

      return matchesFilter && haystack.includes(search.toLowerCase());
    });
  }, [bookings, filter, search, services]);

  const filteredCustomers = useMemo(() => {
    const term = customerSearch.trim().toLowerCase();
    if (!term) return customers;

    return customers.filter((customer) =>
      `${customer.firstName} ${customer.lastName} ${customer.email}`
        .toLowerCase()
        .includes(term)
    );
  }, [customers, customerSearch]);

  function serviceName(id: string) {
    return services.find((service) => service.id === id)?.name ??
      (isArabic ? "خدمة غير معروفة" : "Unknown service");
  }

  function dateLabel(value: string) {
    const date = new Date(value);
    if (Number.isNaN(date.getTime())) return value;
    return new Intl.DateTimeFormat(isArabic ? "ar-EG" : "en-EG", {
      dateStyle: "medium",
      timeStyle: "short",
    }).format(date);
  }

  function statusLabel(status: string) {
    const labels: Record<string, { en: string; ar: string }> = {
      pending: { en: "Pending", ar: "قيد الانتظار" },
      confirmed: { en: "Confirmed", ar: "تم التأكيد" },
      inprogress: { en: "In progress", ar: "جارٍ التنفيذ" },
      completed: { en: "Completed", ar: "مكتمل" },
      cancelled: { en: "Cancelled", ar: "ملغي" },
    };
    const item = labels[status.toLowerCase()];
    return item ? (isArabic ? item.ar : item.en) : status;
  }

  function statusClass(status: string) {
    const normalized = status.toLowerCase();
    return ["pending", "confirmed", "inprogress", "completed", "cancelled"].includes(normalized)
      ? normalized
      : "pending";
  }


  async function handleCustomerStatus(customer: CustomerResponse) {
    const nextStatus = !customer.isActive;

    if (
      !nextStatus &&
      !window.confirm(t.confirmDeactivateCustomer)
    ) {
      return;
    }

    setNotice("");
    setError("");

    try {
      await setCustomerActiveStatus(customer.id, nextStatus);

      setCustomers((current) =>
        current.map((item) =>
          item.id === customer.id
            ? { ...item, isActive: nextStatus }
            : item
        )
      );

      setNotice(t.customerStatusUpdated);
    } catch (err) {
      setError(err instanceof Error ? err.message : t.error);
    }
  }
  async function handleBookingAction(id: string, action: BookingAction) {
    if (action === "cancel" && !window.confirm(t.confirmCancel)) return;

    setNotice("");
    setError("");

    try {
      await updateBookingStatus(id, action);
      setNotice(t.bookingUpdated);
      await loadDashboard(true);
    } catch (err) {
      setError(err instanceof Error ? err.message : t.error);
    }
  }

  function openNewService() {
    setEditingService(null);
    setServiceForm({ title: "", name: "", description: "", points: [] });
    setShowServiceForm(true);
    setNotice("");
    setError("");
  }

  function openEditService(service: ServiceResponse) {
    setEditingService(service);
    setServiceForm({ title: service.title, name: service.name, description: service.description, points: [...service.points] });
    setShowServiceForm(true);
    setNotice("");
    setError("");
  }

  async function handleSaveService(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if (!serviceForm.title.trim() || !serviceForm.name.trim() || !serviceForm.description.trim()) { setError(t.required); return; }

    setSaving(true);
    setError("");
    setNotice("");

    try {
      if (editingService) {
        await updateService(editingService.id, serviceForm);
        setNotice(t.updated);
      } else {
        await createService(serviceForm);
        setNotice(t.created);
      }

      setShowServiceForm(false);
      setEditingService(null);
      await loadDashboard(true);
    } catch (err) {
      setError(err instanceof Error ? err.message : t.error);
    } finally {
      setSaving(false);
    }
  }

  function bookingActions(booking: BookingResponse) {
    const status = booking.status.toLowerCase();

    if (status === "pending") {
      return (
        <>
          <button className="admin-action admin-action-confirm" onClick={() => void handleBookingAction(booking.id, "confirm")}>
            <Check size={14} /> {t.confirm}
          </button>
          <button className="admin-action admin-action-cancel" onClick={() => void handleBookingAction(booking.id, "cancel")}>
            <X size={14} /> {t.cancel}
          </button>
        </>
      );
    }

    if (status === "confirmed") {
      return (
        <>
          <button className="admin-action admin-action-start" onClick={() => void handleBookingAction(booking.id, "start")}>
            <Activity size={14} /> {t.start}
          </button>
          <button className="admin-action admin-action-cancel" onClick={() => void handleBookingAction(booking.id, "cancel")}>
            <X size={14} /> {t.cancel}
          </button>
        </>
      );
    }

    if (status === "inprogress") {
      return (
        <button className="admin-action admin-action-confirm" onClick={() => void handleBookingAction(booking.id, "complete")}>
          <CheckCircle2 size={14} /> {t.complete}
        </button>
      );
    }

    return <span className="admin-no-action">{t.actionsUnavailable}</span>;
  }

  const activeServices = services.filter((service) => service.status.toLowerCase() === "active");

  return (
    <div className="admin-shell">
      <aside className="admin-sidebar">
        <Link to="/" className="admin-brand">
          <span className="admin-brand-icon"><Activity size={23} /></span>
          <span><strong>CARE PLUS</strong><small>{t.dashboard}</small></span>
        </Link>

        <div className="admin-sidebar-label">{isArabic ? "القائمة الرئيسية" : "MAIN MENU"}</div>

        <nav className="admin-nav">
          <button className={activeTab === "overview" ? "active" : ""} onClick={() => setActiveTab("overview")}>
            <LayoutDashboard size={18} /> {t.overview}
          </button>
          <button className={activeTab === "bookings" ? "active" : ""} onClick={() => setActiveTab("bookings")}>
            <CalendarDays size={18} /> {t.bookings}
            <span className="admin-nav-count">{counts.pending}</span>
          </button>
          <button className={activeTab === "services" ? "active" : ""} onClick={() => setActiveTab("services")}>
            <Package size={18} /> {t.services}
          </button>
          <button className={activeTab === "customers" ? "active" : ""} onClick={() => setActiveTab("customers")}>
            <Users size={18} /> {t.customers}
            <span className="admin-nav-count">{customers.length}</span>
          </button>
        </nav>

        <div className="admin-sidebar-bottom">
          <Link to="/"><ArrowLeft size={16} /> {t.backWebsite}</Link>
          <button onClick={logoutUser}><LogOut size={16} /> {t.logout}</button>
        </div>
      </aside>

      <main className="admin-main">
        <header className="admin-topbar">
          <div>
            <span className="admin-eyebrow">CARE PLUS / ADMIN</span>
            <h1>{activeTab === "overview" ? t.overview : activeTab === "bookings" ? t.manageBookings : activeTab === "services" ? t.serviceManagement : t.manageCustomers}</h1>
            <p>{t.subtitle}</p>
          </div>

          <div className="admin-topbar-actions">
            <button className="admin-language-button" onClick={() => void i18n.changeLanguage(isArabic ? "en" : "ar")}>
              {isArabic ? "English" : "العربية"}
            </button>
            <div className="admin-user">
              <span>{auth?.firstName?.charAt(0)?.toUpperCase() ?? "A"}</span>
              <div><strong>{auth?.firstName} {auth?.lastName}</strong><small>Administrator</small></div>
            </div>
            <button className="admin-refresh" onClick={() => void loadDashboard(true)} disabled={refreshing}>
              <RefreshCw size={16} className={refreshing ? "refresh-spinning" : ""} />
              {t.refresh}
            </button>
          </div>
        </header>

        {error && <div className="admin-alert admin-alert-error" role="alert">{error}</div>}
        {notice && <div className="admin-alert admin-alert-success" role="status">{notice}</div>}

        {loading ? (
          <div className="admin-loading"><RefreshCw className="refresh-spinning" /> {t.loading}</div>
        ) : (
          <>
            {activeTab === "overview" && (
              <section className="admin-content">
                <div className="admin-welcome">
                  <div>
                    <span>{t.welcome}, {auth?.firstName ?? "Admin"}</span>
                    <h2>{isArabic ? "إليك ملخص نشاط تمريض" : "Here's your Care Plus activity at a glance."}</h2>
                    <p>{t.subtitle}</p>
                  </div>
                  <div className="admin-welcome-mark"><Activity size={48} /></div>
                </div>

                <div className="admin-stats-grid">
                  <article className="admin-stat-card">
                    <span className="admin-stat-icon stat-purple"><CalendarDays size={20} /></span>
                    <span>{t.totalBookings}</span>
                    <strong>{counts.total}</strong>
                  </article>
                  <article className="admin-stat-card">
                    <span className="admin-stat-icon stat-amber"><Clock3 size={20} /></span>
                    <span>{t.pending}</span>
                    <strong>{counts.pending}</strong>
                  </article>
                  <article className="admin-stat-card">
                    <span className="admin-stat-icon stat-blue"><Check size={20} /></span>
                    <span>{t.confirmed}</span>
                    <strong>{counts.confirmed}</strong>
                  </article>
                  <article className="admin-stat-card">
                    <span className="admin-stat-icon stat-green"><CheckCircle2 size={20} /></span>
                    <span>{t.completed}</span>
                    <strong>{counts.completed}</strong>
                  </article>
                  <article className="admin-stat-card">
                    <span className="admin-stat-icon stat-teal"><Package size={20} /></span>
                    <span>{t.serviceCount}</span>
                    <strong>{activeServices.length}</strong>
                  </article>
                  <article className="admin-stat-card">
                    <span className="admin-stat-icon stat-blue"><Users size={20} /></span>
                    <span>{t.customerCount}</span>
                    <strong>{customers.length}</strong>
                  </article>
                </div>

                <section className="admin-panel">
                  <div className="admin-panel-heading">
                    <div><span className="admin-eyebrow">LATEST ACTIVITY</span><h2>{t.recentBookings}</h2></div>
                    <button className="admin-text-link" onClick={() => setActiveTab("bookings")}>{t.allBookings} <BackArrow size={15} /></button>
                  </div>
                  {bookings.length === 0 ? (
                    <div className="admin-empty">{t.noBookings}</div>
                  ) : (
                    <div className="admin-table-wrap">
                      <table className="admin-table">
                        <thead><tr><th>{t.service}</th><th>{t.date}</th><th>{t.address}</th><th>{t.status}</th></tr></thead>
                        <tbody>
                          {[...bookings].sort((a, b) => new Date(b.createdAtUtc).getTime() - new Date(a.createdAtUtc).getTime()).slice(0, 5).map((booking) => (
                            <tr key={booking.id}>
                              <td><strong>{serviceName(booking.serviceId)}</strong><small>{booking.customerId.slice(0, 8)}...</small></td>
                              <td>{dateLabel(booking.scheduledAtUtc)}</td>
                              <td>{booking.city}, {booking.district}</td>
                              <td><span className={`admin-status status-${statusClass(booking.status)}`}>{statusLabel(booking.status)}</span></td>
                            </tr>
                          ))}
                        </tbody>
                      </table>
                    </div>
                  )}
                </section>
              </section>
            )}

            {activeTab === "bookings" && (
              <section className="admin-content">
                <div className="admin-section-heading">
                  <div><span className="admin-eyebrow">OPERATIONS</span><h2>{t.allBookings}</h2><p>{bookings.length} {t.bookings.toLowerCase()}</p></div>
                </div>

                <div className="admin-booking-tools">
                  <div className="admin-search"><Search size={17} /><input value={search} onChange={(event) => setSearch(event.target.value)} placeholder={t.search} /></div>
                  <div className="admin-filters">
                    {(["all", "pending", "confirmed", "inprogress", "completed", "cancelled"] as BookingFilter[]).map((item) => (
                      <button key={item} className={filter === item ? "active" : ""} onClick={() => setFilter(item)}>{t[item]}</button>
                    ))}
                  </div>
                </div>

                {filteredBookings.length === 0 ? <div className="admin-empty">{t.noBookings}</div> : (
                  <div className="admin-booking-grid">
                    {filteredBookings.map((booking) => (
                      <article className="admin-booking-card" key={booking.id}>
                        <div className="admin-booking-card-top">
                          <div><span className="admin-eyebrow">{booking.id.slice(0, 8).toUpperCase()}</span><h3>{serviceName(booking.serviceId)}</h3></div>
                          <span className={`admin-status status-${statusClass(booking.status)}`}>{statusLabel(booking.status)}</span>
                        </div>
                        <div className="admin-booking-info">
                          <div><CalendarDays size={16} /><span>{dateLabel(booking.scheduledAtUtc)}</span></div>
                          <div><MapPin size={16} /><span>{[booking.street, booking.buildingNumber, booking.apartmentNumber, booking.district, booking.city].filter(Boolean).join(", ")}</span></div>
                          <div><span className="admin-info-label">{t.customer}</span><span className="admin-customer-id">{booking.customerId}</span></div>
                          {booking.notes && <div className="admin-booking-notes"><span className="admin-info-label">{isArabic ? "ملاحظات" : "Notes"}</span><p>{booking.notes}</p></div>}
                        </div>
                        <div className="admin-booking-actions">{bookingActions(booking)}</div>
                      </article>
                    ))}
                  </div>
                )}
              </section>
            )}

            {activeTab === "services" && (
              <section className="admin-content">
                <div className="admin-section-heading">
                  <div><span className="admin-eyebrow">CATALOGUE</span><h2>{t.serviceManagement}</h2><p>{services.length} {t.services.toLowerCase()}</p></div>
                  <button className="admin-primary-button" onClick={openNewService}><Plus size={17} /> {t.addService}</button>
                </div>

                {showServiceForm && (
                  <form className="admin-service-form" onSubmit={(event) => void handleSaveService(event)}>
                    <div className="admin-panel-heading">
                      <h2>{editingService ? t.editService : t.addService}</h2>
                      <button type="button" className="admin-icon-button" onClick={() => setShowServiceForm(false)}><X size={19} /></button>
                    </div>
                                        <label>{t.title}<input value={serviceForm.title} onChange={(event) => setServiceForm({ ...serviceForm, title: event.target.value })} required maxLength={200} /></label>
                    <label>{t.name}<input value={serviceForm.name} onChange={(event) => setServiceForm({ ...serviceForm, name: event.target.value })} required maxLength={150} /></label>
                    <label>{t.description}<textarea value={serviceForm.description} onChange={(event) => setServiceForm({ ...serviceForm, description: event.target.value })} required maxLength={1000} rows={4} /></label>
                    <div className="admin-points-field">
                      <span>{t.points}</span>
                      {serviceForm.points.map((point, index) => (
                        <div key={index} className="admin-point-input">
                          <input value={point} maxLength={200} placeholder={`${t.points} ${index + 1}`} onChange={(event) => setServiceForm({ ...serviceForm, points: serviceForm.points.map((item, i) => i === index ? event.target.value : item) })} />
                          <button type="button" className="admin-secondary-button" onClick={() => setServiceForm({ ...serviceForm, points: serviceForm.points.filter((_, i) => i !== index) })}>{t.removePoint}</button>
                        </div>
                      ))}
                      <button type="button" className="admin-secondary-button" disabled={serviceForm.points.length >= 20} onClick={() => setServiceForm({ ...serviceForm, points: [...serviceForm.points, ""] })}>+ {t.addPoint}</button>
                    </div>
                    <div className="admin-form-actions">
                      <button type="button" className="admin-secondary-button" onClick={() => setShowServiceForm(false)}>{t.cancelEdit}</button>
                      <button type="submit" className="admin-primary-button" disabled={saving}><Save size={16} /> {saving ? t.saving : t.save}</button>
                    </div>
                  </form>
                )}

                {services.length === 0 ? <div className="admin-empty">{t.noServices}</div> : (
                  <div className="admin-services-grid">
                    {services.map((service) => (
                      <article className="admin-service-card" key={service.id}>
                        <div className="admin-service-card-icon"><Package size={22} /></div>
                        <span className={`admin-status status-${service.status.toLowerCase() === "active" ? "confirmed" : "cancelled"}`}>{service.status}</span>
                        <h3>{service.name}</h3>
                        <p>{service.description}</p>
                        <div className="admin-service-card-bottom">
                          <button className="admin-secondary-button" onClick={() => openEditService(service)}>{isArabic ? "تعديل" : "Edit"}</button>
                        </div>
                      </article>
                    ))}
                  </div>
                )}
              </section>
            )}
            {activeTab === "customers" && (
              <section className="admin-content">
                <div className="admin-section-heading">
                  <div>
                    <span className="admin-eyebrow">CUSTOMER RELATIONS</span>
                    <h2>{t.manageCustomers}</h2>
                    <p>{customers.length} {t.customers.toLowerCase()}</p>
                  </div>
                </div>

                <div className="admin-booking-tools">
                  <div className="admin-search">
                    <Search size={17} />
                    <input
                      value={customerSearch}
                      onChange={(event) => setCustomerSearch(event.target.value)}
                      placeholder={t.customerSearch}
                    />
                  </div>
                </div>

                {filteredCustomers.length === 0 ? (
                  <div className="admin-empty">{t.noCustomers}</div>
                ) : (
                  <div className="admin-services-grid">
                    {filteredCustomers.map((customer) => (
                      <article className="admin-service-card" key={customer.id}>
                        <div className="admin-service-card-icon">
                          <Users size={22} />
                        </div>
                        <span className={`admin-status status-${customer.isActive ? "confirmed" : "cancelled"}`}>
                          {customer.isActive ? t.active : t.inactive}
                        </span>
                        <button
                          className="admin-secondary-button"
                          onClick={() => void handleCustomerStatus(customer)}
                        >
                          {customer.isActive ? t.deactivateCustomer : t.activateCustomer}
                        </button>
                        <h3>{customer.firstName} {customer.lastName}</h3>
                        <p>{customer.email}</p>

                        <div className="admin-customer-stats">
                          <div>
                            <span>{t.totalCustomerBookings}</span>
                            <strong>{bookings.filter((b) => b.customerId === customer.id).length}</strong>
                          </div>
                          <div>
                            <span>{t.completedCustomerBookings}</span>
                            <strong>{bookings.filter((b) => b.customerId === customer.id && b.status.toLowerCase() === "completed").length}</strong>
                          </div>
                          <div>
                            <span>{t.cancelledCustomerBookings}</span>
                            <strong>{bookings.filter((b) => b.customerId === customer.id && b.status.toLowerCase() === "cancelled").length}</strong>
                          </div>
                        </div>

                        <button
                          className="admin-secondary-button"
                          onClick={() => setExpandedCustomerId(
                            expandedCustomerId === customer.id ? null : customer.id
                          )}
                        >
                          <CalendarDays size={15} />
                          {expandedCustomerId === customer.id ? t.hideHistory : t.viewHistory}
                          ({bookings.filter((booking) => booking.customerId === customer.id).length})
                        </button>

                        {expandedCustomerId === customer.id && (
                          <div className="admin-customer-history">
                            <h4>{t.bookingHistory}</h4>
                            {bookings
                              .filter((booking) => booking.customerId === customer.id)
                              .sort((a, b) => new Date(b.createdAtUtc).getTime() - new Date(a.createdAtUtc).getTime())
                              .map((booking) => (
                                <div className="admin-customer-history-item" key={booking.id}>
                                  <strong>{serviceName(booking.serviceId)}</strong>
                                  <span className={`admin-status status-${statusClass(booking.status)}`}>
                                    {statusLabel(booking.status)}
                                  </span>
                                  <p>{dateLabel(booking.scheduledAtUtc)}</p>
                                  <p>{[booking.street, booking.buildingNumber, booking.district, booking.city].filter(Boolean).join(", ")}</p>
                                </div>
                              ))}
                            {bookings.filter((booking) => booking.customerId === customer.id).length === 0 && (
                              <p>{t.noCustomerBookings}</p>
                            )}
                          </div>
                        )}
                        <div className="admin-booking-info">
                          <div>
                            <span className="admin-info-label">{t.registered}</span>
                            <span>{dateLabel(customer.createdAtUtc)}</span>
                          </div>
                          <div>
                            <span className="admin-info-label">{t.lastLogin}</span>
                            <span>{customer.lastLoginAtUtc ? dateLabel(customer.lastLoginAtUtc) : t.never}</span>
                          </div>
                          <div>
                            <span className="admin-info-label">ID</span>
                            <span className="admin-customer-id">{customer.id}</span>
                          </div>
                        </div>
                      </article>
                    ))}
                  </div>
                )}
              </section>
            )}
          </>
        )}
      </main>
    </div>
  );
}







