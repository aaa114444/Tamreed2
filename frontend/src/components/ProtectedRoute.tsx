import { Navigate, Outlet, useLocation } from "react-router-dom";
import { useAuth } from "../context/AuthContext";

export default function ProtectedRoute() {
  const { auth, isAuthenticated } = useAuth();
  const location = useLocation();

  if (!isAuthenticated || !auth) {
    return (
      <Navigate
        to="/login"
        replace
        state={{ from: location }}
      />
    );
  }

  if (auth.role !== "User") {
    return <Navigate to="/" replace />;
  }

  return <Outlet />;
}
