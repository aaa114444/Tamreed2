import { apiRequest } from "./apiClient";
import {
  clearAuthSession,
  getAuthSession,
  saveAuthSession,
} from "./authStorage";

import type {
  AuthResponse,
  LoginRequest,
  RegisterRequest,
} from "../types/auth";

export async function register(
  request: RegisterRequest
): Promise<AuthResponse> {
  const response = await apiRequest<AuthResponse>(
    "/api/Auth/register",
    {
      method: "POST",
      body: JSON.stringify(request),
    }
  );

  saveAuthSession(response);
  return response;
}

export async function login(
  request: LoginRequest
): Promise<AuthResponse> {
  const response = await apiRequest<AuthResponse>(
    "/api/Auth/login",
    {
      method: "POST",
      body: JSON.stringify(request),
    }
  );

  saveAuthSession(response);
  return response;
}

export { getAuthSession };

export function logout(): void {
  clearAuthSession();
}
