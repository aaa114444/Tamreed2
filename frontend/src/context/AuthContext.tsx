import {
  createContext,
  useContext,
  useState,
  type ReactNode,
} from "react";

import type { AuthResponse } from "../types/auth";
import {
  getAuthSession,
  login,
  logout,
  register,
} from "../services/authService";

import type {
  LoginRequest,
  RegisterRequest,
} from "../types/auth";

interface AuthContextType {
  auth: AuthResponse | null;
  isAuthenticated: boolean;
  loginUser: (request: LoginRequest) => Promise<AuthResponse>;
  registerUser: (request: RegisterRequest) => Promise<AuthResponse>;
  logoutUser: () => void;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

interface AuthProviderProps {
  children: ReactNode;
}

export function AuthProvider({ children }: AuthProviderProps) {
  const [auth, setAuth] = useState<AuthResponse | null>(() => getAuthSession());

  async function loginUser(request: LoginRequest): Promise<AuthResponse> {
    const response = await login(request);
    setAuth(response);
    return response;
  }

  async function registerUser(
    request: RegisterRequest
  ): Promise<AuthResponse> {
    const response = await register(request);
    setAuth(response);
    return response;
  }

  function logoutUser(): void {
    logout();
    setAuth(null);
  }

  return (
    <AuthContext.Provider
      value={{
        auth,
        isAuthenticated: auth !== null,
        loginUser,
        registerUser,
        logoutUser,
      }}
    >
      {children}
    </AuthContext.Provider>
  );
}

export function useAuth(): AuthContextType {
  const context = useContext(AuthContext);

  if (!context) {
    throw new Error("useAuth must be used inside an AuthProvider.");
  }

  return context;
}
