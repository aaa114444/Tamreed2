import { useCallback, useEffect, useMemo, useState } from "react";
import {
  BookOpen,
  FileText,
  Pencil,
  Plus,
  RefreshCw,
  Search,
  Trash2,
  X,
  CheckCircle2,
  AlertCircle,
  Globe,
} from "lucide-react";
import {
  createArticle,
  deleteArticle,
  getAdminArticles,
  publishArticle,
  unpublishArticle,
  updateArticle,
} from "../../services/articleService";
import type {
  ArticleResponse,
  SaveArticleRequest,
} from "../../types/article";

type Language = "en" | "ar";

const emptyForm: SaveArticleRequest = {
  title: "",
  titleAr: "",
  slug: "",
  excerpt: "",
  excerptAr: "",
  content: "",
  contentAr: "",
  coverImageUrl: null,
  seoTitle: null,
  seoTitleAr: null,
  seoDescription: null,
  seoDescriptionAr: null,
};

function countWords(value: string): number {
  return value.trim().split(/\s+/).filter(Boolean).length;
}

function analyzeSeo(
  form: SaveArticleRequest,
  language: Language,
  keyword: string,
) {
  const title = language === "en" ? form.title : form.titleAr;
  const seoTitle =
    (language === "en" ? form.seoTitle : form.seoTitleAr) || title;
  const description =
    (language === "en"
      ? form.seoDescription
      : form.seoDescriptionAr) || "";
  const content =
    language === "en" ? form.content : form.contentAr;
  const normalizedKeyword = keyword.trim().toLowerCase();
  const slug = form.slug.toLowerCase();
  const plainText = content.replace(/[#*_[\]()>`-]/g, " ");
  const words = countWords(plainText);
  const headings = content.match(/^#{1,6}\s+.+$/gm) || [];
  const links = content.match(/\[[^\]]+\]\([^)]+\)/g) || [];
  const images = content.match(/!\[([^\]]*)\]\([^)]+\)/g) || [];
  const checks = [
    {
      label: "SEO title length (50�60 characters)",
      passed: seoTitle.length >= 50 && seoTitle.length <= 60,
    },
    {
      label: "Meta description length (120�160 characters)",
      passed: description.length >= 120 && description.length <= 160,
    },
    {
      label: "Focus keyword in title",
      passed:
        !!normalizedKeyword &&
        title.toLowerCase().includes(normalizedKeyword),
    },
    {
      label: "Focus keyword in meta description",
      passed:
        !!normalizedKeyword &&
        description.toLowerCase().includes(normalizedKeyword),
    },
    {
      label: "Focus keyword in URL slug",
      passed:
        !!normalizedKeyword &&
        slug.includes(normalizedKeyword.replace(/\s+/g, "-")),
    },
    {
      label: "Content has at least 300 words",
      passed: words >= 300,
    },
    {
      label: "Content includes a heading",
      passed: headings.length > 0,
    },
    {
      label: "Content includes an H2 heading",
      passed: /^##\s+.+$/m.test(content),
    },
    {
      label: "Cover image URL is provided",
      passed: !!form.coverImageUrl?.trim(),
    },
    {
      label: "Images in Markdown have alt text",
      passed: images.length > 0 && images.every((image) => {
        const alt = image.match(/^!\[([^\]]*)\]/)?.[1] || "";
        return alt.trim().length > 0;
      }),
    },
    {
      label: "Content includes at least one link",
      passed: links.length > 0,
    },
  ];
  const passed = checks.filter((check) => check.passed).length;
  return {
    checks,
    words,
    score: Math.round((passed / checks.length) * 100),
  };
}

export default function ArticlesManagement() {
  const [articles, setArticles] = useState<ArticleResponse[]>([]);
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState("");
  const [notice, setNotice] = useState("");
  const [search, setSearch] = useState("");
  const [statusFilter, setStatusFilter] = useState("all");
  const [editingId, setEditingId] = useState<string | null>(null);
  const [form, setForm] = useState<SaveArticleRequest>(emptyForm);
  const [language, setLanguage] = useState<Language>("en");
  const [keyword, setKeyword] = useState("");
  const [showForm, setShowForm] = useState(false);

  const loadArticles = useCallback(async () => {
    setLoading(true);
    setError("");
    try {
      setArticles(await getAdminArticles());
    } catch (err) {
      setError(
        err instanceof Error ? err.message : "Unable to load articles.",
      );
    } finally {
      setLoading(false);
    }
  }, []);

  useEffect(() => {
    void loadArticles();
  }, [loadArticles]);

  const filteredArticles = useMemo(
    () =>
      articles.filter((article) => {
        const matchesSearch =
          `${article.title} ${article.titleAr} ${article.slug}`
            .toLowerCase()
            .includes(search.toLowerCase());
        const matchesStatus =
          statusFilter === "all" ||
          article.status.toLowerCase() === statusFilter;
        return matchesSearch && matchesStatus;
      }),
    [articles, search, statusFilter],
  );

  const analysis = useMemo(
    () => analyzeSeo(form, language, keyword),
    [form, language, keyword],
  );

  function updateField<K extends keyof SaveArticleRequest>(
    field: K,
    value: SaveArticleRequest[K],
  ) {
    setForm((current) => ({ ...current, [field]: value }));
  }

  function startCreate() {
    setEditingId(null);
    setForm(emptyForm);
    setKeyword("");
    setLanguage("en");
    setShowForm(true);
    setNotice("");
    setError("");
  }

  function startEdit(article: ArticleResponse) {
    setEditingId(article.id);
    setForm({
      title: article.title,
      titleAr: article.titleAr,
      slug: article.slug,
      excerpt: article.excerpt,
      excerptAr: article.excerptAr,
      content: article.content,
      contentAr: article.contentAr,
      coverImageUrl: article.coverImageUrl || null,
      seoTitle: article.seoTitle || null,
      seoTitleAr: article.seoTitleAr || null,
      seoDescription: article.seoDescription || null,
      seoDescriptionAr: article.seoDescriptionAr || null,
    });
    setKeyword("");
    setLanguage("en");
    setShowForm(true);
    setNotice("");
    setError("");
  }

  function closeForm() {
    setShowForm(false);
    setEditingId(null);
    setForm(emptyForm);
    setKeyword("");
  }

  async function handleSave(event: React.FormEvent<HTMLFormElement>) {
    event.preventDefault();
    if (
      !form.title.trim() ||
      !form.titleAr.trim() ||
      !form.slug.trim() ||
      !form.content.trim() ||
      !form.contentAr.trim()
    ) {
      setError("Complete both language titles, slug, and content.");
      return;
    }

    setSaving(true);
    setError("");
    setNotice("");
    try {
      if (editingId) {
        await updateArticle(editingId, form);
        setNotice("Article updated successfully.");
      } else {
        await createArticle(form);
        setNotice("Article created successfully.");
      }
      closeForm();
      await loadArticles();
    } catch (err) {
      setError(
        err instanceof Error ? err.message : "Unable to save article.",
      );
    } finally {
      setSaving(false);
    }
  }

  async function handleDelete(article: ArticleResponse) {
    if (!window.confirm(`Delete "${article.title}"? This cannot be undone.`)) {
      return;
    }
    setError("");
    setNotice("");
    try {
      await deleteArticle(article.id);
      setNotice("Article deleted successfully.");
      await loadArticles();
    } catch (err) {
      setError(
        err instanceof Error ? err.message : "Unable to delete article.",
      );
    }
  }

  async function handlePublish(article: ArticleResponse) {
    setError("");
    setNotice("");
    try {
      if (article.status.toLowerCase() === "published") {
        await unpublishArticle(article.id);
        setNotice("Article moved to draft.");
      } else {
        await publishArticle(article.id);
        setNotice("Article published successfully.");
      }
      await loadArticles();
    } catch (err) {
      setError(
        err instanceof Error ? err.message : "Unable to update article status.",
      );
    }
  }

  const inputClass =
    "w-full rounded-xl border border-slate-200 bg-white px-4 py-3 text-sm text-slate-800 outline-none transition focus:border-teal-500 focus:ring-2 focus:ring-teal-100";
  const labelClass = "mb-2 block text-sm font-semibold text-slate-700";
  const activeField: {
    title: "title" | "titleAr";
    excerpt: "excerpt" | "excerptAr";
    content: "content" | "contentAr";
    seoTitle: "seoTitle" | "seoTitleAr";
    seoDescription: "seoDescription" | "seoDescriptionAr";
  } =
    language === "en"
      ? {
          title: "title",
          excerpt: "excerpt",
          content: "content",
          seoTitle: "seoTitle",
          seoDescription: "seoDescription",
        }
      : {
          title: "titleAr",
          excerpt: "excerptAr",
          content: "contentAr",
          seoTitle: "seoTitleAr",
          seoDescription: "seoDescriptionAr",
        };

  return (
    <div className="space-y-6">
      <div className="flex flex-wrap items-center justify-between gap-4">
        <div>
          <h2 className="text-2xl font-bold text-slate-900">Articles</h2>
          <p className="mt-1 text-sm text-slate-500">
            Create bilingual articles, manage publication, and review SEO.
          </p>
        </div>
        <div className="flex gap-2">
          <button
            type="button"
            onClick={() => void loadArticles()}
            className="inline-flex items-center gap-2 rounded-xl border border-slate-200 px-4 py-2.5 text-sm font-semibold text-slate-700 hover:bg-slate-50"
          >
            <RefreshCw size={16} /> Refresh
          </button>
          <button
            type="button"
            onClick={startCreate}
            className="inline-flex items-center gap-2 rounded-xl bg-teal-700 px-4 py-2.5 text-sm font-semibold text-white hover:bg-teal-800"
          >
            <Plus size={16} /> New article
          </button>
        </div>
      </div>

      {notice && (
        <div className="rounded-xl border border-emerald-200 bg-emerald-50 px-4 py-3 text-sm text-emerald-800">
          {notice}
        </div>
      )}
      {error && (
        <div className="rounded-xl border border-red-200 bg-red-50 px-4 py-3 text-sm text-red-700">
          {error}
        </div>
      )}

      {showForm ? (
        <form onSubmit={handleSave} className="space-y-6 rounded-2xl border border-slate-200 bg-white p-5 shadow-sm md:p-7">
          <div className="flex flex-wrap items-center justify-between gap-3">
            <h3 className="text-lg font-bold text-slate-900">
              {editingId ? "Edit article" : "Create article"}
            </h3>
            <button
              type="button"
              onClick={closeForm}
              className="rounded-lg p-2 text-slate-500 hover:bg-slate-100"
              aria-label="Close editor"
            >
              <X size={18} />
            </button>
          </div>

          <div className="grid gap-5 md:grid-cols-2">
            <div>
              <label className={labelClass}>URL slug *</label>
              <input
                className={inputClass}
                value={form.slug}
                onChange={(event) => updateField("slug", event.target.value)}
                placeholder="e.g. home-care-guide"
                required
              />
              <p className="mt-1 text-xs text-slate-500">
                Use lowercase words separated by hyphens.
              </p>
            </div>
            <div>
              <label className={labelClass}>Cover image URL</label>
              <input
                className={inputClass}
                type="url"
                value={form.coverImageUrl || ""}
                onChange={(event) =>
                  updateField("coverImageUrl", event.target.value || null)
                }
                placeholder="https://..."
              />
            </div>
          </div>

          <div className="flex flex-wrap gap-2 border-b border-slate-100 pb-3">
            <button
              type="button"
              onClick={() => setLanguage("en")}
              className={`rounded-lg px-4 py-2 text-sm font-semibold ${language === "en" ? "bg-teal-700 text-white" : "bg-slate-100 text-slate-600"}`}
            >
              English
            </button>
            <button
              type="button"
              onClick={() => setLanguage("ar")}
              className={`rounded-lg px-4 py-2 text-sm font-semibold ${language === "ar" ? "bg-teal-700 text-white" : "bg-slate-100 text-slate-600"}`}
            >
              {"\u0627\u0644\u0639\u0631\u0628\u064a\u0629"}
            </button>
          </div>

          <div className="grid gap-5 md:grid-cols-2">
            <div className="md:col-span-2">
              <label className={labelClass}>Article title *</label>
              <input
                className={inputClass}
                dir={language === "ar" ? "rtl" : "ltr"}
                value={form[activeField.title] || ""}
                onChange={(event) =>
                  updateField(activeField.title, event.target.value)
                }
                required
              />
            </div>
            <div className="md:col-span-2">
              <label className={labelClass}>Excerpt</label>
              <textarea
                className={inputClass}
                rows={2}
                dir={language === "ar" ? "rtl" : "ltr"}
                value={form[activeField.excerpt] || ""}
                onChange={(event) =>
                  updateField(activeField.excerpt, event.target.value)
                }
              />
            </div>
            <div className="md:col-span-2">
              <label className={labelClass}>Content (Markdown) *</label>
              <textarea
                className={`${inputClass} font-mono`}
                rows={14}
                dir={language === "ar" ? "rtl" : "ltr"}
                value={form[activeField.content] || ""}
                onChange={(event) =>
                  updateField(activeField.content, event.target.value)
                }
                placeholder={"## Section heading\n\nWrite your article in Markdown..."}
                required
              />
              <p className="mt-1 text-xs text-slate-500">
                Supports Markdown headings, links, and images. Use ![Alt text](image-url) for images.
              </p>
            </div>
          </div>

          <section className="space-y-4 rounded-xl bg-slate-50 p-4">
            <div className="flex items-center gap-2">
              <Globe size={18} className="text-teal-700" />
              <h4 className="font-bold text-slate-900">SEO settings</h4>
            </div>
            <div>
              <label className={labelClass}>SEO title</label>
              <input
                className={inputClass}
                value={form[activeField.seoTitle] || ""}
                onChange={(event) =>
                  updateField(activeField.seoTitle, event.target.value || null)
                }
                maxLength={70}
              />
              <p className="mt-1 text-xs text-slate-500">
                {(form[activeField.seoTitle] || "").length}/60 recommended characters
              </p>
            </div>
            <div>
              <label className={labelClass}>Meta description</label>
              <textarea
                className={inputClass}
                rows={3}
                value={form[activeField.seoDescription] || ""}
                onChange={(event) =>
                  updateField(activeField.seoDescription, event.target.value || null)
                }
                maxLength={180}
              />
              <p className="mt-1 text-xs text-slate-500">
                {(form[activeField.seoDescription] || "").length}/160 recommended characters
              </p>
            </div>
            <div>
              <label className={labelClass}>Focus keyword (analysis only)</label>
              <input
                className={inputClass}
                value={keyword}
                onChange={(event) => setKeyword(event.target.value)}
                placeholder="e.g. home nursing"
              />
            </div>
          </section>

          <section className="space-y-3 rounded-xl border border-slate-200 p-4">
            <div className="flex flex-wrap items-center justify-between gap-3">
              <h4 className="font-bold text-slate-900">
                {language === "en" ? "English" : "Arabic"} SEO analysis
              </h4>
              <span className="rounded-full bg-teal-50 px-3 py-1 text-sm font-bold text-teal-800">
                {analysis.score}/100
              </span>
            </div>
            <div className="h-2 overflow-hidden rounded-full bg-slate-100">
              <div
                className="h-full rounded-full bg-teal-600 transition-all"
                style={{ width: `${analysis.score}%` }}
              />
            </div>
            <p className="text-sm text-slate-500">
              {analysis.words} words detected in this language.
            </p>
            <div className="grid gap-2 md:grid-cols-2">
              {analysis.checks.map((check) => (
                <div key={check.label} className="flex items-start gap-2 text-sm">
                  {check.passed ? (
                    <CheckCircle2 size={16} className="mt-0.5 shrink-0 text-emerald-600" />
                  ) : (
                    <AlertCircle size={16} className="mt-0.5 shrink-0 text-amber-600" />
                  )}
                  <span className={check.passed ? "text-slate-700" : "text-slate-500"}>
                    {check.label}
                  </span>
                </div>
              ))}
            </div>
          </section>

          <div className="flex flex-wrap justify-end gap-3 border-t border-slate-100 pt-4">
            <button
              type="button"
              onClick={closeForm}
              className="rounded-xl border border-slate-200 px-5 py-2.5 text-sm font-semibold text-slate-600 hover:bg-slate-50"
            >
              Cancel
            </button>
            <button
              type="submit"
              disabled={saving}
              className="rounded-xl bg-teal-700 px-5 py-2.5 text-sm font-semibold text-white hover:bg-teal-800 disabled:opacity-60"
            >
              {saving ? "Saving..." : "Save article"}
            </button>
          </div>
        </form>
      ) : (
        <>
          <div className="flex flex-wrap gap-3">
            <div className="relative min-w-[220px] flex-1">
              <Search size={17} className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400" />
              <input
                className={`${inputClass} pl-10`}
                value={search}
                onChange={(event) => setSearch(event.target.value)}
                placeholder="Search articles..."
              />
            </div>
            <select
              className={`${inputClass} w-auto min-w-[150px]`}
              value={statusFilter}
              onChange={(event) => setStatusFilter(event.target.value)}
            >
              <option value="all">All statuses</option>
              <option value="draft">Drafts</option>
              <option value="published">Published</option>
            </select>
          </div>

          {loading ? (
            <div className="rounded-2xl border border-slate-200 bg-white p-10 text-center text-slate-500">
              Loading articles...
            </div>
          ) : filteredArticles.length === 0 ? (
            <div className="rounded-2xl border border-dashed border-slate-300 bg-white p-10 text-center">
              <BookOpen size={34} className="mx-auto mb-3 text-slate-300" />
              <p className="font-semibold text-slate-700">No articles found.</p>
              <p className="mt-1 text-sm text-slate-500">
                Create your first bilingual article to get started.
              </p>
            </div>
          ) : (
            <div className="overflow-hidden rounded-2xl border border-slate-200 bg-white">
              <div className="divide-y divide-slate-100">
                {filteredArticles.map((article) => (
                  <article
                    key={article.id}
                    className="flex flex-col gap-4 p-5 md:flex-row md:items-center md:justify-between"
                  >
                    <div className="flex min-w-0 items-start gap-3">
                      {article.coverImageUrl ? (
                        <img
                          src={article.coverImageUrl}
                          alt=""
                          className="h-16 w-20 shrink-0 rounded-lg object-cover"
                        />
                      ) : (
                        <div className="flex h-16 w-20 shrink-0 items-center justify-center rounded-lg bg-slate-100 text-slate-400">
                          <FileText size={24} />
                        </div>
                      )}
                      <div className="min-w-0">
                        <h3 className="truncate font-bold text-slate-900">
                          {article.title}
                        </h3>
                        <p className="mt-1 truncate text-sm text-slate-500" dir="rtl">
                          {article.titleAr}
                        </p>
                        <p className="mt-1 text-xs text-slate-400">
                          /{article.slug} � {new Date(article.updatedAtUtc).toLocaleDateString()}
                        </p>
                      </div>
                    </div>
                    <div className="flex flex-wrap items-center gap-2">
                      <span className={`rounded-full px-3 py-1 text-xs font-semibold ${article.status.toLowerCase() === "published" ? "bg-emerald-50 text-emerald-700" : "bg-amber-50 text-amber-700"}`}>
                        {article.status}
                      </span>
                      <button
                        type="button"
                        onClick={() => startEdit(article)}
                        className="inline-flex items-center gap-1.5 rounded-lg border border-slate-200 px-3 py-2 text-sm font-semibold text-slate-600 hover:bg-slate-50"
                      >
                        <Pencil size={15} /> Edit
                      </button>
                      <button
                        type="button"
                        onClick={() => void handlePublish(article)}
                        className="rounded-lg border border-teal-200 px-3 py-2 text-sm font-semibold text-teal-700 hover:bg-teal-50"
                      >
                        {article.status.toLowerCase() === "published" ? "Unpublish" : "Publish"}
                      </button>
                      <button
                        type="button"
                        onClick={() => void handleDelete(article)}
                        className="rounded-lg border border-red-200 p-2 text-red-600 hover:bg-red-50"
                        aria-label="Delete article"
                      >
                        <Trash2 size={16} />
                      </button>
                    </div>
                  </article>
                ))}
              </div>
            </div>
          )}
        </>
      )}
    </div>
  );
}
