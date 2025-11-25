import React from "react";
import { toast } from "react-toastify";
import { useNavigate } from "react-router-dom";
import { useApi } from "../../API/apiService";

export const RefreshTokenModal = ({ show, closeModal, token, setToken }) => {
  const { apiServicePost } = useApi();
  const handleRefresh = async (e) => {
    e.preventDefault();
    try {
      const response = await apiServicePost(
        "auth/refresh-token",
        {
          refreshToken: token.refreshToken,
          accessToken: token.accessToken,
        },
        true
      );

      const newToken = {
        ...token,
        refreshToken: response.data.refreshToken,
        accessToken: response.data.accessToken,
        expiresAt: response.data.expiresAt,
      };

      setToken(newToken);
      sessionStorage.setItem("token", JSON.stringify(newToken));

      toast.success("¡Su sesión ha sido renovada!");
      closeModal();
    } catch (error) {
      console.error("Error al refrescar el token:", error);
    }
  };

  const navigate = useNavigate();
  const deleteToken = () => {
    sessionStorage.removeItem("token");
    setToken(null);
    navigate("/");
    closeModal();
  };

  if (!show) return null;

  return (
    <div
      className="modal d-block position-fixed overflow-y-scroll pb-5 show fade modal-blur"
      tabIndex="-1"
      role="dialog"
    >
      <div
        className="modal-dialog modal-sm modal-dialog-centered"
        role="document"
      >
        <div className="modal-content">
          <div className="modal-status bg-success"></div>
          <div className="modal-body py-4 text-center">
            <i className="ti ti-alert-triangle text-success display-2"></i>
            <h3>Su sesión ha expirado </h3>
            <div className="text-secondary">¿Desea renovarla?</div>
          </div>
          <div className="modal-footer">
            <div className="w-100">
              <div className="row">
                <div className="col">
                  <button
                    onClick={deleteToken}
                    className="btn btn-secondary w-100 d-flex items-center gap-2"
                  >
                    Cancelar
                  </button>
                </div>
                <div className="col">
                  <form className="delete-form" onSubmit={handleRefresh}>
                    <button
                      type="submit"
                      className="btn btn-success w-100 flex items-center gap-2"
                    >
                      Sí, mantener activa
                    </button>
                  </form>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
