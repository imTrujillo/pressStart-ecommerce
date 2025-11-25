import { createContext, useContext, useState, useEffect } from "react"; // Añadido useEffect
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { RefreshTokenModal } from "../../components/modals/RefreshTokenModal";
import axios from "axios";

const AuthContext = createContext();

const AuthProvider = ({ children }) => {
  //Buscar el token guardado en el sessionStorage
  const [token, setToken] = useState(() => {
    const stored = sessionStorage.getItem("token");
    return stored ? JSON.parse(stored) : null;
  });

  const [refreshTokenModal, setRefreshTokenModal] = useState(false);
  useEffect(() => {
    if (!token) return;

    // Función que revisa si el token expiró
    const checkTokenExpiry = () => {
      const now = new Date();
      const expiryDate = new Date(token.expiresAt);
      if (expiryDate <= now) {
        setRefreshTokenModal(true);
      }
    };
    checkTokenExpiry();

    // Revisar cada 1 minuto
    const intervalId = setInterval(checkTokenExpiry, 60000);
    return () => clearInterval(intervalId);
  }, [token]);

  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const timer = setTimeout(() => {
      setIsLoading(false);
    }, 2000);
    return () => clearTimeout(timer);
  }, []);

  const navigate = useNavigate();

  const baseUrl = "https://localhost:7084/api/";

  const signup = async (data, rol) => {
    try {
      const response = await axios.post(
        `${baseUrl}auth/register/${rol}`,
        data,
        {
          headers: { Authorization: `Bearer ${token?.accessToken}` },
        }
      );

      if (response.status >= 200 && response.status < 300) {
        toast.success(
          `¡${rol === "employee" ? "Empleado" : "Cliente"} agregado!`
        );
      }
    } catch (error) {
      if (error.response && Array.isArray(error.response.data)) {
        error.response.data.forEach((err) => {
          toast.error(`${err.property}: ${err.error}`);
        });
      } else if (error.response?.data?.error) {
        toast.error(error.response.data.error);
      } else {
        toast.error("Ocurrió un error al intentar registrarte.");
      }
    }
  };

  const login = async (data) => {
    try {
      const response = await axios.post(`${baseUrl}auth/login`, data);

      if (response.status >= 200 && response.status < 300) {
        const tokenData = response.data;
        setToken(tokenData);
        sessionStorage.setItem("token", JSON.stringify(tokenData));

        switch (tokenData.user.role) {
          case "Employee":
            navigate("/inventario");
            break;
          case "Customer":
            navigate("/pedidos");
            break;
          case "Admin":
            navigate("/usuarios");
            break;
          default:
            navigate("/");
        }

        toast.success("Inicio de sesión exitoso.");
      }
    } catch (error) {
      if (error.response && Array.isArray(error.response.data)) {
        error.response.data.forEach((err) => {
          toast.error(`${err.property}: ${err.error}`);
        });
      } else if (error.response?.data?.error) {
        toast.error(error.response.data.error);
      } else {
        toast.error("Ocurrió un error al iniciar sesión.");
        console.error(error);
      }
    }
  };

  const setNewPassword = async (data) => {
    try {
      const response = await axios.post(`${baseUrl}auth/reset-password`, data);

      if (response && response.status >= 200 && response.status < 300) {
        navigate("/login");
        toast.success("Contraseña actualizada exitosamente.");
      }
    } catch (error) {
      if (error.response && Array.isArray(error.response.data)) {
        error.response.data.forEach((err) => {
          toast.error(`${err.property}: ${err.error}`);
        });
      } else if (error.response?.data?.error) {
        toast.error(error.response.data.error);
      } else {
        toast.error("Ocurrió un error al cambiar la contraseña.");
      }
    }
  };

  const logout = async () => {
    try {
      const body = {
        accessToken: token?.token,
        refreshToken: token?.refreshToken,
      };

      await axios.post(`${baseUrl}auth/logout`, body, {
        headers: { Authorization: `Bearer ${token?.accessToken}` },
      });
    } catch (error) {
      console.error("Error durante el logout en la API:", error);
    } finally {
      setToken(null);
      sessionStorage.removeItem("token");

      navigate("/");
      toast.success("Sesión cerrada.");
    }
  };

  if (isLoading)
    return (
      <div className="progress progress-sm">
        <div className="progress-bar progress-bar-indeterminate"></div>
      </div>
    );

  return (
    //Pasa los valores como un objeto
    <AuthContext.Provider
      value={{ token, signup, setNewPassword, login, logout, setIsLoading }}
    >
      {children}
      <RefreshTokenModal
        show={refreshTokenModal}
        token={token}
        setToken={setToken}
        closeModal={() => setRefreshTokenModal(false)}
      />
    </AuthContext.Provider>
  );
};

export default AuthProvider;

export const useAuth = () => {
  return useContext(AuthContext);
};
