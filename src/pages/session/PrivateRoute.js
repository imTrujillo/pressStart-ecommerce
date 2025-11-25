import { useAuth } from "./AuthProvider";
import { Navigate, Outlet } from "react-router-dom";

const PrivateRoute = ({ allowedRoles }) => {
  const { token } = useAuth();

  if (!token) {
    return <Navigate to="/" replace={true} />;
  }

  if (allowedRoles && !allowedRoles.includes(token.user.role)) {
    switch (token.user.role) {
      case "Customer":
        return <Navigate to="/pedidos" replace={true} />;
      case "Employee":
        return <Navigate to="/inventario" replace={true} />;
      default:
        return <Navigate to="/login" replace={true} />;
    }
  }

  // Si el usuario tiene token y el rol es permitido, renderiza el componente hijo
  return <Outlet />;
};

export default PrivateRoute;
