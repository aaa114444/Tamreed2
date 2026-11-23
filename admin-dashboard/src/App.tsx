import { useEffect } from "react";
import {
  BrowserRouter,
  Navigate,
  Route,
  Routes,
} from "react-router-dom";

import { AuthProvider, useAuth } from "./context/AuthContext";
import AdminLoginPage from "./pages/AdminLoginPage";
import AdminDashboardPage from "./pages/AdminDashboardPage";

function AdminRoute() {
  const { auth, logoutUser } = useAuth();

  useEffect(() => {
    if (auth && auth.role?.toLowerCase() !== "admin") {
      logoutUser();
    }
  }, [auth, logoutUser]);

  if (!auth || auth.role?.toLowerCase() !== "admin") {
    return <Navigate to="/login" replace />;
  }

  return <AdminDashboardPage />;
}

function AppRoutes() {
  return (
    <Routes>
      <Route path="/login" element={<AdminLoginPage />} />
      <Route path="/" element={<AdminRoute />} />
      <Route path="*" element={<Navigate to="/" replace />} />
    </Routes>
  );
}

export default function App() {
  return (
    <AuthProvider>
      <BrowserRouter>
        <AppRoutes />
      </BrowserRouter>
    </AuthProvider>
  );
}
