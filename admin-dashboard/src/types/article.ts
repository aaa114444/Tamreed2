export interface ArticleResponse {
  id: string;
  title: string;
  titleAr: string;
  slug: string;
  excerpt: string;
  excerptAr: string;
  content: string;
  contentAr: string;
  coverImageUrl: string;
  seoTitle: string;
  seoTitleAr: string;
  seoDescription: string;
  seoDescriptionAr: string;
  status: "Draft" | "Published" | string;
  createdAtUtc: string;
  updatedAtUtc: string;
  publishedAtUtc: string | null;
}

export interface SaveArticleRequest {
  title: string;
  titleAr: string;
  slug: string;
  excerpt: string;
  excerptAr: string;
  content: string;
  contentAr: string;
  coverImageUrl: string | null;
  seoTitle: string | null;
  seoTitleAr: string | null;
  seoDescription: string | null;
  seoDescriptionAr: string | null;
}
