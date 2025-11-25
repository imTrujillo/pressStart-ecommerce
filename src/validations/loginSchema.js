import { IconLockFilled, IconUserFilled } from "@tabler/icons-react";
import style from "../css/Auth.module.css";
import * as Yup from "yup";

export const loginSchema = Yup.object().shape({
  username: Yup.string()
    .required("requerido")
    .min(5, "min 5 caracteres")
    .max(50, "max 50 caracteres")
    .matches(/^(?![\W_]+$)[A-Za-zÁÉÍÓÚáéíóúñÑ\s]+$/, "solo letras y números"),
  password: Yup.string().min(6, "min 8 caracteres.").required("requerido"),
});

export const loginValidations = {
  // Datos de cada input
  userValidation: {
    icon: <IconUserFilled className={style.icon} />,
    type: "text",
    id: "username",
    name: "username",
    placeholder: "Nombre de usuario",
    isAuthInput: true,
  },

  passwordValidation: {
    icon: <IconLockFilled className={style.icon} />,
    type: "password",
    id: "password",
    name: "password",
    placeholder: "Contraseña",
    isAuthInput: true,
  },
};
