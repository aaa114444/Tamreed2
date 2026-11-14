import { API_BASE_URL } from "../config/api";
import { getAuthSession } from "./authStorage";

export class ApiError extends Error {
  public readonly status: number;
  public readonly details?: unknown;

  constructor(message: string, status: number, details?: unknown) {
    super(message);
    this.name = "ApiError";
    this.status = status;
    this.details = details;
  }
}

export async function apiRequest<T>(
  path: string,
  options: RequestInit = {}
): Promise<T> {
  if (!API_BASE_URL) {
    throw new Error(
      "API URL is not configured. Set VITE_API_BASE_URL in .env.local."
    );
  }

  const normalizedPath = path.replace(/^\/+/, "");
  const url = `${API_BASE_URL}/${normalizedPath}`;

  const headers = new Headers(options.headers);
  headers.set("Accept", "application/json");

  if (options.body && !headers.has("Content-Type")) {
    headers.set("Content-Type", "application/json");
  }

  const auth = getAuthSession();

  if (auth?.accessToken && !headers.has("Authorization")) {
    headers.set("Authorization", `Bearer ${auth.accessToken}`);
  }

  let response: Response;

  try {
    response = await fetch(url, { ...options, headers });
  } catch (error) {
    throw new ApiError(
      "Unable to connect to the server. Please try again later.",
      0,
      error
    );
  }

  if (response.status === 204) {
    return undefined as T;
  }

  const responseText = await response.text();
  let data: unknown;

  try {
    data = responseText ? JSON.parse(responseText) : undefined;
  } catch {
    data = responseText;
  }

  if (!response.ok) {
    const errorData =
      typeof data === "object" && data !== null
        ? (data as Record<string, unknown>)
        : undefined;

    const message =
      (typeof errorData?.detail === "string" && errorData.detail) ||
      (typeof errorData?.title === "string" && errorData.title) ||
      (typeof errorData?.message === "string" && errorData.message) ||
      `API request failed with status ${response.status}.`;

    throw new ApiError(message, response.status, data);
  }

  return data as T;
}
