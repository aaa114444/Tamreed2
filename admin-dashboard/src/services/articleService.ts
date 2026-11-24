import { apiRequest } from "./apiClient";
import type { ArticleResponse, SaveArticleRequest } from "../types/article";

export async function getAdminArticles(): Promise<ArticleResponse[]> {
  return apiRequest<ArticleResponse[]>("/api/Articles/admin");
}

export async function createArticle(
  request: SaveArticleRequest
): Promise<{ id: string }> {
  return apiRequest<{ id: string }>("/api/Articles", {
    method: "POST",
    body: JSON.stringify(request),
  });
}

export async function updateArticle(
  id: string,
  request: SaveArticleRequest
): Promise<void> {
  await apiRequest<void>(`/api/Articles/${id}`, {
    method: "PUT",
    body: JSON.stringify(request),
  });
}

export async function deleteArticle(id: string): Promise<void> {
  await apiRequest<void>(`/api/Articles/${id}`, {
    method: "DELETE",
  });
}

export async function publishArticle(id: string): Promise<void> {
  await apiRequest<void>(`/api/Articles/${id}/publish`, {
    method: "POST",
  });
}

export async function unpublishArticle(id: string): Promise<void> {
  await apiRequest<void>(`/api/Articles/${id}/unpublish`, {
    method: "POST",
  });
}
