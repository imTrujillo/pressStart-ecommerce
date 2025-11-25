import React, { useEffect, useState } from "react";
import Navbar from "./../assets/Navbar";
import { ToastContainer } from "react-toastify";
import { useSearchParams } from "react-router-dom";
import { RefreshTokenModal } from "../components/modals/RefreshTokenModal";
import { useAuth } from "../pages/session/AuthProvider";

export default function App({ children }) {
  //Definir el modo oscuro o claro en el localStorage
  const [theme, setTheme] = useState(() => {
    const savedTheme = localStorage.getItem("theme");
    return savedTheme
      ? savedTheme === "dark"
      : window.matchMedia("(prefers-color-scheme: dark)").matches;
  });

  //Asignar el modo oscuro en la url
  const [searchParams, setSearchParams] = useSearchParams();
  useEffect(() => {
    const themeParam = searchParams.get("theme");
    if (themeParam === "light" || themeParam === "dark") {
      const newTheme = themeParam === "dark";
      if (newTheme !== theme) {
        setTheme(newTheme);
      }
    }

    localStorage.setItem("theme", theme ? "dark" : "light");
  }, [theme, searchParams]);

  //Cambiar el modo ya sea claro u oscuro
  const handleTheme = () => {
    const newTheme = !theme;
    localStorage.setItem("theme", newTheme ? "dark" : "light");
    window.location.href = newTheme ? "?theme=dark" : "?theme=light";
  };
  const toastTheme = theme ? "dark" : "light";

  return (
    <>
      <Navbar theme={theme} handleTheme={handleTheme} />

      <main>{children}</main>

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
        theme={toastTheme}
      />
    </>
  );
}
