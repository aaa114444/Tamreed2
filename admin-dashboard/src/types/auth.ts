export interface AuthResponse {
  userId: string;
  email: string;
  firstName: string;
  lastName: string;
  role: string;
  accessToken: string;
  refreshToken: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  firstName: string;
  lastName: string;
}

export interface LoginRequest {
  email: string;
  password: string;
}
