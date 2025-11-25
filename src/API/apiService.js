import axios from "axios";

import { toast } from "react-toastify";
import { useAuth } from "../pages/session/AuthProvider";

const baseUrl = "https://localhost:7084/api/";

export function useApi() {
  const { token } = useAuth();

  const apiServiceGet = async (endpoint, id = null, authorized = false) => {
    try {
      const url = id ? baseUrl + endpoint + "/" + id : baseUrl + endpoint;

      const response = await axios.get(
        url,
        authorized
          ? { headers: { Authorization: `Bearer ${token.accessToken}` } }
          : undefined
      );
      if (response.status >= 200 && response.status < 300) {
        return response.data;
      } else {
        // Si el status no es 200, devolver un array vacío o null
        console.warn(
          `La API respondió con estado ${response.status} para ${url}`
        );
        return []; // Devolver un array vacío por defecto para listas
      }
    } catch (error) {
      console.error("Ocurrió un error", error.message);

      return [];
    }
  };

  const apiServicePost = async (endpoint, object, authorized = false) => {
    const config = authorized
      ? { headers: { Authorization: `Bearer ${token.accessToken}` } }
      : undefined;

    const response = await axios.post(baseUrl + endpoint, object, config);

    return response;
  };

  const apiServiceUpdate = async (endpoint, object, authorized = false) => {
    try {
      const response = await axios.put(
        baseUrl + endpoint,
        object,
        authorized
          ? { headers: { Authorization: `Bearer ${token.accessToken}` } }
          : undefined
      );

      if (response.status >= 200 && response.status < 300) {
        // Consideracion de todos los 2xx
        return response.data;
      } else {
        console.warn(
          `La API respondió con estado ${response.status} para PUT ${
            baseUrl + endpoint
          }.`
        );
        return null; // Devuelve null si no es 2xx
      }
    } catch (error) {
      console.error("Ocurrió un error", error.message);

      return [];
    }
  };

  const apiServiceDelete = async (endpoint, authorized = false) => {
    try {
      const response = await axios.delete(
        baseUrl + endpoint,
        authorized
          ? { headers: { Authorization: `Bearer ${token.accessToken}` } }
          : undefined
      );

      if (response.status >= 200 && response.status < 300) {
        toast.success("¡Elemento borrado exitosamente!");
        return response.data;
      }
    } catch (error) {
      console.error("Ocurrió un error", error.message);
      return [];
    }
  };

  return { apiServiceGet, apiServicePost, apiServiceUpdate, apiServiceDelete };
}
