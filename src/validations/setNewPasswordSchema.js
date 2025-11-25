import { IconId, IconLockFilled, IconUserFilled } from "@tabler/icons-react";
import style from "../css/Auth.module.css";
import * as Yup from "yup";

export const setNewPasswordSchema = Yup.object().shape({
  username: Yup.string()
    .required("requerido")
    .min(5, "min 5 caracteres")
    .max(50, "max 50 caracteres")
    .matches(/^(?![\W_]+$)[A-Za-zÁÉÍÓÚáéíóúñÑ\s]+$/, "solo letras y números"),
  dui: Yup.string()
    .matches(/^\d{8}-\d{1}$/, "formato: 12345678-9")
    .required("requerido"),
  newPassword: Yup.string()
    .min(6, "min 8 caracteres.")
    .matches(/[a-z]/, "falta letra minúscula")
    .matches(/[A-Z]/, "falta letra mayúscula")
    .matches(/\d/, "falta un número")
    .matches(/[!@#$%^&*(),.?":{}|<>]/, "falta carácter especial")
    .required("requerido"),
  confirmPassword: Yup.string()
    .oneOf([Yup.ref("newPassword"), null], "las contraseñas no coinciden")
    .required("requerida"),
});

export const setNewPasswordValidations = {
  usernameValidation: {
    icon: <IconUserFilled className={style.icon} />,
    type: "text",
    name: "username",
    placeholder: "Nombre de usuario",
    isAuthInput: true,
  },

  duiValidation: {
    icon: <IconId className={style.icon} />,
    type: "text",
    name: "dui",
    placeholder: "DUI",
    isAuthInput: true,
  },

  newPasswordValidation: {
    icon: <IconLockFilled className={style.icon} />,
    type: "password",
    name: "newPassword",
    placeholder: "Contraseña",
    isAuthInput: true,
  },

  confirmPasswordValidation: {
    icon: <IconLockFilled className={style.icon} />,
    type: "password",
    name: "confirmPassword",
    placeholder: "Confirmar contraseña",
    isAuthInput: true,
  },
};
