import { ToastContainer } from "react-toastify";
import style from "../css/Auth.module.css";
import { useEffect, useState } from "react";

export default function Guest({ children }) {
  useEffect(() => {
    document.body.setAttribute("data-bs-theme", "dark");
    localStorage.setItem("theme", "light");

    // Limpieza opcional si se cambia de componente
    return () => {
      document.body.removeAttribute("data-bs-theme");
    };
  }, []);

  return (
    <>
      <main className={style.guestContainer}>{children}</main>
      <ToastContainer
        id="toast-popup"
        position="bottom-right"
        autoClose={5000}
        hideProgressBar={false}
        newestOnTop={false}
        closeOnClick={false}
        rtl={false}
        pauseOnFocusLoss
        draggable
        pauseOnHover
        theme="dark"
      />
    </>
  );
}
