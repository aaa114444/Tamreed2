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
  deleteService,
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
function createEmptyServiceForm(): SaveServiceRequest {
  return {
    title: "",
    name: "",
    description: "",
    points: [],
    titleAr: "",
    nameAr: "",
    descriptionAr: "",
    pointsAr: [],
    details: {
      aboutTitle: "",
      aboutText: "",
      aboutTitleAr: "",
      aboutTextAr: "",
      audienceTitle: "",
      audienceDescription: "",
      audienceTitleAr: "",
      audienceDescriptionAr: "",
      audienceItems: [],
      audienceItemsAr: [],
      includedTitle: "",
      includedDescription: "",
      includedTitleAr: "",
      includedDescriptionAr: "",
      includedItems: [],
      includedItemsAr: [],
      processTitle: "",
      processDescription: "",
      processTitleAr: "",
      processDescriptionAr: "",
      steps: [],
      faqTitle: "",
      faqDescription: "",
      faqTitleAr: "",
      faqDescriptionAr: "",
      faqs: [],
    },
  };
}

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
    never: "لم يسجل الدخول من قبل",
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
    required: "يرجى إكمال الحقول المطلوبة.",
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
  const [editingService, setEditingService] = useState<ServiceResponse | null>(null);
  const [showServiceForm, setShowServiceForm] = useState(false);
  const [serviceForm, setServiceForm] = useState<SaveServiceRequest>(createEmptyServiceForm());

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
      (isArabic ? "Ã˜Â®Ã˜Â¯Ã™â€¦Ã˜Â© Ã˜ÂºÃ™Å Ã˜Â± Ã™â€¦Ã˜Â¹Ã˜Â±Ã™Ë†Ã™ÂÃ˜Â©" : "Unknown service");
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
    setServiceForm(createEmptyServiceForm());
    setShowServiceForm(true);
    setNotice("");
    setError("");
  }

  function openEditService(service: ServiceResponse) {
    const emptyForm = createEmptyServiceForm();

    setEditingService(service);
    setServiceForm({
      title: service.title ?? "",
      name: service.name ?? "",
      description: service.description ?? "",
      points: [...(service.points ?? [])],
      titleAr: service.titleAr ?? "",
      nameAr: service.nameAr ?? "",
      descriptionAr: service.descriptionAr ?? "",
      pointsAr: [...(service.pointsAr ?? [])],
      details: {
        ...emptyForm.details,
        ...(service.details ?? {}),
        audienceItems: [...(service.details?.audienceItems ?? [])],
        audienceItemsAr: [...(service.details?.audienceItemsAr ?? [])],
        includedItems: [...(service.details?.includedItems ?? [])],
        includedItemsAr: [...(service.details?.includedItemsAr ?? [])],
        steps: [...(service.details?.steps ?? [])],
        faqs: [...(service.details?.faqs ?? [])],
      },
    });

    setShowServiceForm(true);
    setNotice("");
    setError("");
  }

  function updateServiceField<K extends keyof SaveServiceRequest>(
    field: K,
    value: SaveServiceRequest[K]
  ) {
    setServiceForm((current) => ({ ...current, [field]: value }));
  }

  function updateDetailsField<K extends keyof SaveServiceRequest["details"]>(
    field: K,
    value: SaveServiceRequest["details"][K]
  ) {
    setServiceForm((current) => ({
      ...current,
      details: { ...current.details, [field]: value },
    }));
  }

  function updateDetailsList(
    field: "audienceItems" | "audienceItemsAr" | "includedItems" | "includedItemsAr",
    index: number,
    value: string
  ) {
    updateDetailsField(
      field,
      serviceForm.details[field].map((item, i) => i === index ? value : item)
    );
  }

  function updateStep(
    index: number,
    field: "title" | "description" | "titleAr" | "descriptionAr",
    value: string
  ) {
    updateDetailsField(
      "steps",
      serviceForm.details.steps.map((step, i) =>
        i === index ? { ...step, [field]: value } : step
      )
    );
  }

  function updateFaq(
    index: number,
    field: "question" | "answer" | "questionAr" | "answerAr",
    value: string
  ) {
    updateDetailsField(
      "faqs",
      serviceForm.details.faqs.map((faq, i) =>
        i === index ? { ...faq, [field]: value } : faq
      )
    );
  }
  async function handleDeleteService(service: ServiceResponse) {
    const confirmed = window.confirm(
      isArabic
        ? `هل أنت متأكد من حذف خدمة "${service.nameAr || service.name}"؟`
        : `Are you sure you want to delete "${service.name || service.nameAr}"?`
    );

    if (!confirmed) return;

    setError("");
    setNotice("");

    try {
      await deleteService(service.id);
      setNotice(
        isArabic
          ? "تم حذف الخدمة بنجاح."
          : "Service deleted successfully."
      );
      await loadDashboard(true);
    } catch (err) {
      setError(err instanceof Error ? err.message : t.error);
    }
  }
  async function handleSaveService(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();

    if ((!serviceForm.title.trim() && !serviceForm.titleAr.trim()) || (!serviceForm.name.trim() && !serviceForm.nameAr.trim()) || (!serviceForm.description.trim() && !serviceForm.descriptionAr.trim())) { setError(t.required); return; }
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

        <div className="admin-sidebar-label">{isArabic ? "\u0627\u0644\u0642\u0627\u0626\u0645\u0629 \u0627\u0644\u0631\u0626\u064a\u0633\u064a\u0629" : "MAIN MENU"}</div>

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
              {isArabic ? "English" : "\u0627\u0644\u0639\u0631\u0628\u064a\u0629"}
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
                    <h2>{isArabic ? "\u0625\u0644\u064a\u0643 \u0645\u0644\u062e\u0635 \u0646\u0634\u0627\u0637 \u062a\u0645\u0631\u064a\u0636" : "Here's your Care Plus activity at a glance."}</h2>
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
                    <div><span className="admin-eyebrow">{isArabic ? "\u0622\u062e\u0631 \u0627\u0644\u0646\u0634\u0627\u0637" : "LATEST ACTIVITY"}</span><h2>{t.recentBookings}</h2></div>
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
                          {booking.notes && <div className="admin-booking-notes"><span className="admin-info-label">{isArabic ? "Ã™â€¦Ã™â€žÃ˜Â§Ã˜Â­Ã˜Â¸Ã˜Â§Ã˜Âª" : "Notes"}</span><p>{booking.notes}</p></div>}
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
      <button type="button" className="admin-icon-button" onClick={() => setShowServiceForm(false)}>
        <X size={19} />
      </button>
    </div>

    <h3>{isArabic ? "المعلومات الأساسية" : "Basic Information"}</h3>
    <div className="admin-details-grid">
      <label>{isArabic ? "العنوان" : "Title"}
        <input dir={isArabic ? "rtl" : "ltr"} value={isArabic ? serviceForm.titleAr : serviceForm.title}
          onChange={(event) => isArabic ? updateServiceField("titleAr", event.target.value) : updateServiceField("title", event.target.value)}
          maxLength={200} />
      </label>
      <label>{isArabic ? "اسم الخدمة" : "Service name"}
        <input dir={isArabic ? "rtl" : "ltr"} value={isArabic ? serviceForm.nameAr : serviceForm.name}
          onChange={(event) => isArabic ? updateServiceField("nameAr", event.target.value) : updateServiceField("name", event.target.value)}
          maxLength={150} />
      </label>
      <label>{isArabic ? "الوصف" : "Description"}
        <textarea dir={isArabic ? "rtl" : "ltr"} value={isArabic ? serviceForm.descriptionAr : serviceForm.description}
          onChange={(event) => isArabic ? updateServiceField("descriptionAr", event.target.value) : updateServiceField("description", event.target.value)}
          maxLength={1000} rows={3} />
      </label>
    </div>

    <div className="admin-editor-section">
      <h3>{isArabic ? "النقاط الرئيسية" : "Highlights"}</h3>
      <div className="admin-points-field">
        {(isArabic ? serviceForm.pointsAr : serviceForm.points).map((point, index) => (
          <div className="admin-dynamic-row" key={`point-${isArabic ? "ar" : "en"}-${index}`}>
            <input dir={isArabic ? "rtl" : "ltr"} value={point}
              onChange={(event) => {
                if (isArabic) updateServiceField("pointsAr", serviceForm.pointsAr.map((item, i) => i === index ? event.target.value : item));
                else updateServiceField("points", serviceForm.points.map((item, i) => i === index ? event.target.value : item));
              }} />
            <button type="button" className="admin-icon-button" onClick={() => {
              if (isArabic) updateServiceField("pointsAr", serviceForm.pointsAr.filter((_, i) => i !== index));
              else updateServiceField("points", serviceForm.points.filter((_, i) => i !== index));
            }}><X size={16} /></button>
          </div>
        ))}
        <button type="button" className="admin-secondary-button" onClick={() => {
          if (isArabic) updateServiceField("pointsAr", [...serviceForm.pointsAr, ""]);
          else updateServiceField("points", [...serviceForm.points, ""]);
        }}>{isArabic ? "+ إضافة نقطة" : "+ Add point"}</button>
      </div>
    </div>

    <div className="admin-editor-section">
      <h3>{isArabic ? "عن الخدمة" : "About This Service"}</h3>
      <div className="admin-details-grid">
        <label>{isArabic ? "عنوان القسم" : "Section title"}
          <input dir={isArabic ? "rtl" : "ltr"} value={isArabic ? serviceForm.details.aboutTitleAr : serviceForm.details.aboutTitle}
            onChange={(event) => isArabic ? updateDetailsField("aboutTitleAr", event.target.value) : updateDetailsField("aboutTitle", event.target.value)} />
        </label>
        <label>{isArabic ? "المحتوى" : "Content"}
          <textarea dir={isArabic ? "rtl" : "ltr"} rows={4}
            value={isArabic ? serviceForm.details.aboutTextAr : serviceForm.details.aboutText}
            onChange={(event) => isArabic ? updateDetailsField("aboutTextAr", event.target.value) : updateDetailsField("aboutText", event.target.value)} />
        </label>
      </div>
    </div>

    <div className="admin-editor-section">
      <h3>{isArabic ? "لمن هذه الخدمة؟" : "Who Is This For?"}</h3>
      <div className="admin-details-grid">
        <label>{isArabic ? "عنوان القسم" : "Section title"}
          <input dir={isArabic ? "rtl" : "ltr"} value={isArabic ? serviceForm.details.audienceTitleAr : serviceForm.details.audienceTitle}
            onChange={(event) => isArabic ? updateDetailsField("audienceTitleAr", event.target.value) : updateDetailsField("audienceTitle", event.target.value)} />
        </label>
        <label>{isArabic ? "الوصف" : "Description"}
          <textarea dir={isArabic ? "rtl" : "ltr"}
            value={isArabic ? serviceForm.details.audienceDescriptionAr : serviceForm.details.audienceDescription}
            onChange={(event) => isArabic ? updateDetailsField("audienceDescriptionAr", event.target.value) : updateDetailsField("audienceDescription", event.target.value)} />
        </label>
      </div>
      <div className="admin-points-field">
        {(isArabic ? serviceForm.details.audienceItemsAr : serviceForm.details.audienceItems).map((item, index) => (
          <div className="admin-dynamic-row" key={`audience-${isArabic ? "ar" : "en"}-${index}`}>
            <input dir={isArabic ? "rtl" : "ltr"} value={item}
              onChange={(event) => updateDetailsList(isArabic ? "audienceItemsAr" : "audienceItems", index, event.target.value)} />
            <button type="button" className="admin-icon-button" onClick={() => {
              if (isArabic) updateDetailsField("audienceItemsAr", serviceForm.details.audienceItemsAr.filter((_, i) => i !== index));
              else updateDetailsField("audienceItems", serviceForm.details.audienceItems.filter((_, i) => i !== index));
            }}><X size={16} /></button>
          </div>
        ))}
        <button type="button" className="admin-secondary-button" onClick={() => {
          if (isArabic) updateDetailsField("audienceItemsAr", [...serviceForm.details.audienceItemsAr, ""]);
          else updateDetailsField("audienceItems", [...serviceForm.details.audienceItems, ""]);
        }}>{isArabic ? "+ إضافة عنصر" : "+ Add item"}</button>
      </div>
    </div>

    <div className="admin-editor-section">
      <h3>{isArabic ? "ماذا تشمل الخدمة؟" : "What's Included"}</h3>
      <div className="admin-details-grid">
        <label>{isArabic ? "عنوان القسم" : "Section title"}
          <input dir={isArabic ? "rtl" : "ltr"} value={isArabic ? serviceForm.details.includedTitleAr : serviceForm.details.includedTitle}
            onChange={(event) => isArabic ? updateDetailsField("includedTitleAr", event.target.value) : updateDetailsField("includedTitle", event.target.value)} />
        </label>
        <label>{isArabic ? "الوصف" : "Description"}
          <textarea dir={isArabic ? "rtl" : "ltr"}
            value={isArabic ? serviceForm.details.includedDescriptionAr : serviceForm.details.includedDescription}
            onChange={(event) => isArabic ? updateDetailsField("includedDescriptionAr", event.target.value) : updateDetailsField("includedDescription", event.target.value)} />
        </label>
      </div>
      <div className="admin-points-field">
        {(isArabic ? serviceForm.details.includedItemsAr : serviceForm.details.includedItems).map((item, index) => (
          <div className="admin-dynamic-row" key={`included-${isArabic ? "ar" : "en"}-${index}`}>
            <input dir={isArabic ? "rtl" : "ltr"} value={item}
              onChange={(event) => updateDetailsList(isArabic ? "includedItemsAr" : "includedItems", index, event.target.value)} />
            <button type="button" className="admin-icon-button" onClick={() => {
              if (isArabic) updateDetailsField("includedItemsAr", serviceForm.details.includedItemsAr.filter((_, i) => i !== index));
              else updateDetailsField("includedItems", serviceForm.details.includedItems.filter((_, i) => i !== index));
            }}><X size={16} /></button>
          </div>
        ))}
        <button type="button" className="admin-secondary-button" onClick={() => {
          if (isArabic) updateDetailsField("includedItemsAr", [...serviceForm.details.includedItemsAr, ""]);
          else updateDetailsField("includedItems", [...serviceForm.details.includedItems, ""]);
        }}>{isArabic ? "+ إضافة عنصر" : "+ Add item"}</button>
      </div>
    </div>

    <div className="admin-editor-section">
      <h3>{isArabic ? "خطوات تقديم الخدمة" : "How It Works"}</h3>
      <div className="admin-details-grid">
        <label>{isArabic ? "عنوان القسم" : "Section title"}
          <input dir={isArabic ? "rtl" : "ltr"} value={isArabic ? serviceForm.details.processTitleAr : serviceForm.details.processTitle}
            onChange={(event) => isArabic ? updateDetailsField("processTitleAr", event.target.value) : updateDetailsField("processTitle", event.target.value)} />
        </label>
        <label>{isArabic ? "الوصف" : "Description"}
          <textarea dir={isArabic ? "rtl" : "ltr"}
            value={isArabic ? serviceForm.details.processDescriptionAr : serviceForm.details.processDescription}
            onChange={(event) => isArabic ? updateDetailsField("processDescriptionAr", event.target.value) : updateDetailsField("processDescription", event.target.value)} />
        </label>
      </div>
      {serviceForm.details.steps.map((step, index) => (
        <div className="admin-nested-editor" key={`step-${index}`}>
          <div className="admin-panel-heading">
            <strong>{isArabic ? `الخطوة ` : `Step ${index + 1}`}</strong>
            <button type="button" className="admin-icon-button" onClick={() => updateDetailsField("steps", serviceForm.details.steps.filter((_, i) => i !== index))}><X size={16} /></button>
          </div>
          <div className="admin-details-grid">
            <label>{isArabic ? "العنوان" : "Title"}
              <input dir={isArabic ? "rtl" : "ltr"} value={isArabic ? step.titleAr : step.title}
                onChange={(event) => isArabic ? updateStep(index, "titleAr", event.target.value) : updateStep(index, "title", event.target.value)} />
            </label>
            <label>{isArabic ? "الوصف" : "Description"}
              <textarea dir={isArabic ? "rtl" : "ltr"} value={isArabic ? step.descriptionAr : step.description}
                onChange={(event) => isArabic ? updateStep(index, "descriptionAr", event.target.value) : updateStep(index, "description", event.target.value)} />
            </label>
          </div>
        </div>
      ))}
      <button type="button" className="admin-secondary-button" onClick={() => updateDetailsField("steps", [...serviceForm.details.steps, { title: "", description: "", titleAr: "", descriptionAr: "" }])}>
        {isArabic ? "+ إضافة خطوة" : "+ Add step"}
      </button>
    </div>

    <div className="admin-editor-section">
      <h3>{isArabic ? "الأسئلة الشائعة" : "Frequently Asked Questions"}</h3>
      <div className="admin-details-grid">
        <label>{isArabic ? "عنوان القسم" : "Section title"}
          <input dir={isArabic ? "rtl" : "ltr"} value={isArabic ? serviceForm.details.faqTitleAr : serviceForm.details.faqTitle}
            onChange={(event) => isArabic ? updateDetailsField("faqTitleAr", event.target.value) : updateDetailsField("faqTitle", event.target.value)} />
        </label>
        <label>{isArabic ? "الوصف" : "Description"}
          <textarea dir={isArabic ? "rtl" : "ltr"}
            value={isArabic ? serviceForm.details.faqDescriptionAr : serviceForm.details.faqDescription}
            onChange={(event) => isArabic ? updateDetailsField("faqDescriptionAr", event.target.value) : updateDetailsField("faqDescription", event.target.value)} />
        </label>
      </div>
      {serviceForm.details.faqs.map((faq, index) => (
        <div className="admin-nested-editor" key={`faq-${index}`}>
          <div className="admin-panel-heading">
            <strong>{isArabic ? `السؤال ` : `Question ${index + 1}`}</strong>
            <button type="button" className="admin-icon-button" onClick={() => updateDetailsField("faqs", serviceForm.details.faqs.filter((_, i) => i !== index))}><X size={16} /></button>
          </div>
          <div className="admin-details-grid">
            <label>{isArabic ? "السؤال" : "Question"}
              <input dir={isArabic ? "rtl" : "ltr"} value={isArabic ? faq.questionAr : faq.question}
                onChange={(event) => isArabic ? updateFaq(index, "questionAr", event.target.value) : updateFaq(index, "question", event.target.value)} />
            </label>
            <label>{isArabic ? "الإجابة" : "Answer"}
              <textarea dir={isArabic ? "rtl" : "ltr"} value={isArabic ? faq.answerAr : faq.answer}
                onChange={(event) => isArabic ? updateFaq(index, "answerAr", event.target.value) : updateFaq(index, "answer", event.target.value)} />
            </label>
          </div>
        </div>
      ))}
      <button type="button" className="admin-secondary-button" onClick={() => updateDetailsField("faqs", [...serviceForm.details.faqs, { question: "", answer: "", questionAr: "", answerAr: "" }])}>
        {isArabic ? "+ إضافة سؤال" : "+ Add FAQ"}
      </button>
    </div>
    <div className="admin-form-actions">
      <button type="button" className="admin-secondary-button" onClick={() => setShowServiceForm(false)}>{t.cancel}</button>
      <button type="submit" className="admin-primary-button">{t.save}</button>
    </div>
  </form>
)}{services.length === 0 ? <div className="admin-empty">{t.noServices}</div> : (
                  <div className="admin-services-grid">
                    {services.map((service) => (
                      <article className="admin-service-card" key={service.id}>
                        <div className="admin-service-card-icon"><Package size={22} /></div>
                        <span className={`admin-status status-${service.status.toLowerCase() === "active" ? "confirmed" : "cancelled"}`}>{service.status}</span>
                        <h3>{isArabic ? service.nameAr || service.name : service.name || service.nameAr}</h3>
                        <p>{isArabic ? service.descriptionAr || service.description : service.description || service.descriptionAr}</p>
                        <div className="admin-service-card-bottom">
                          <button className="admin-secondary-button" onClick={() => openEditService(service)}>{isArabic ? "تعديل" : "Edit"}</button>
                          <button type="button" className="admin-action admin-action-cancel" onClick={() => void handleDeleteService(service)}>
                            <X size={14} /> {isArabic ? "حذف" : "Delete"}
                          </button>
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
















