import type { AuthResponse } from "../types/auth";

const AUTH_STORAGE_KEY = "tamreed-auth";

export function saveAuthSession(auth: AuthResponse): void {
  sessionStorage.setItem(AUTH_STORAGE_KEY, JSON.stringify(auth));
}

export function getAuthSession(): AuthResponse | null {
  const storedAuth = sessionStorage.getItem(AUTH_STORAGE_KEY);

  if (!storedAuth) {
    return null;
  }

  try {
    return JSON.parse(storedAuth) as AuthResponse;
  } catch {
    sessionStorage.removeItem(AUTH_STORAGE_KEY);
    return null;
  }
}

export function clearAuthSession(): void {
  sessionStorage.removeItem(AUTH_STORAGE_KEY);
}
