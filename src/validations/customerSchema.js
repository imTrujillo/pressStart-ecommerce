import style from "../css/Auth.module.css";
import {
  IconCalendar,
  IconId,
  IconLockFilled,
  IconMailFilled,
  IconMapPinFilled,
  IconPhoneFilled,
  IconUser,
  IconUserFilled,
} from "@tabler/icons-react";
import { subYears } from "date-fns";
import * as Yup from "yup";
import { Input } from "../components/Input";

export const customerSchema = Yup.object().shape({
  fullName: Yup.string()
    .required("requerido")
    .min(2, "min 2 caracteres")
    .max(100, "max 100 caracteres")
    .matches(/^[A-Za-zÁÉÍÓÚáéíóúñÑ0-9\s]+$/, "solo letras y espacios"),
  username: Yup.string()
    .required("requerido")
    .min(5, "min 5 caracteres")
    .max(50, "max 50 caracteres")
    .matches(
      /^(?![\W_]+$)[A-Za-zÁÉÍÓÚáéíóúñÑ0-9\s]+$/,
      "solo letras y números"
    ),
  email: Yup.string().email("no válido").required("requerido"),
  phoneNumber: Yup.string().required("requerido"),
  address: Yup.string()
    .required("requerido")
    .min(20, "min 20 caracteres")
    .max(200, "max 200 caracteres"),
  dui: Yup.string()
    .matches(/^\d{8}-\d{1}$/, "formato: 12345678-9")
    .required("requerido"),
  dateOfBirth: Yup.date()
    .nullable()
    .transform((curr, orig) => {
      if (orig === "" || orig === null || orig === undefined) {
        return null;
      }
      const date = new Date(orig);
      return isNaN(date.getTime()) ? null : date;
    })
    .required("requerido")
    .min(new Date(1930, 0, 1), "min 1930")
    .max(subYears(new Date(), 18), "min 18 años"),
  password: Yup.string()
    .min(6, "min 8 caracteres.")
    .matches(/[a-z]/, "falta letra minúscula")
    .matches(/[A-Z]/, "falta letra mayúscula")
    .matches(/\d/, "falta un número")
    .matches(/[!@#$%^&*(),.?":{}|<>]/, "falta caracter especial")
    .required("requerido"),
  confirmPassword: Yup.string()
    .oneOf([Yup.ref("password"), null], "las contraseñas no coinciden")
    .required("requerida"),
});

export const customerValidations = {
  DOBValidation: {
    icon: <IconCalendar className={style.icon} />,
    label: "Fecha de nacimiento:",
    type: "date",
    name: "dateOfBirth",
    isAuthInput: true,
  },

  fullNameValidation: {
    icon: <IconUserFilled className={style.icon} />,
    type: "text",
    name: "fullName",
    placeholder: "Nombre completo",
    isAuthInput: true,
  },

  duiValidation: {
    icon: <IconId className={style.icon} />,
    type: "text",
    name: "dui",
    placeholder: "DUI",
    isAuthInput: true,
  },

  usernameValidation: {
    icon: <IconUser className={style.icon} />,
    type: "text",
    name: "username",
    placeholder: "Nombre de usuario",
    isAuthInput: true,
  },

  emailValidation: {
    icon: <IconMailFilled className={style.icon} />,
    type: "email",
    name: "email",
    placeholder: "Correo electrónico",
    isAuthInput: true,
  },

  phoneValidation: {
    icon: <IconPhoneFilled className={style.icon} />,
    type: "text",
    name: "phoneNumber",
    placeholder: "Teléfono",
    isAuthInput: true,
  },

  addressValidation: {
    icon: <IconMapPinFilled className={style.icon} />,
    type: "text",
    name: "address",
    placeholder: "Dirección",
    isAuthInput: true,
  },

  passwordValidation: {
    icon: <IconLockFilled className={style.icon} />,
    type: "password",
    name: "password",
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

export const CustomerInputs = () => {
  return (
    <>
      <div className="row mb-3">
        <div className="col-6">
          <Input
            {...customerValidations.fullNameValidation}
            label="Nombre completo"
            isAuthInput={false}
            icon={null}
          />
        </div>
        <div className="col-6">
          <Input
            {...customerValidations.duiValidation}
            label="DUI"
            isAuthInput={false}
            icon={null}
          />
        </div>
      </div>

      <div className="row mb-3">
        <div className="col-5">
          <Input
            {...customerValidations.emailValidation}
            label="Correo"
            isAuthInput={false}
            placeholder="Juan Pérez"
            icon={null}
          />
        </div>
        <div className="col-7">
          <Input
            {...customerValidations.phoneValidation}
            label="Teléfono"
            isAuthInput={false}
            placeholder="1234-5678"
            icon={null}
          />
        </div>
      </div>
      <div className="row mb-3">
        <div className="col-7">
          <Input
            {...customerValidations.usernameValidation}
            label="Nombre de usuario"
            isAuthInput={false}
            icon={null}
          />
        </div>
        <div className="col-5">
          <Input
            {...customerValidations.DOBValidation}
            isAuthInput={false}
            icon={null}
          />
        </div>
      </div>

      <div className="row mb-3">
        <div className="col-6">
          <Input
            {...customerValidations.passwordValidation}
            label="Contraseña"
            isAuthInput={false}
            placeholder="TuMascota123!"
            icon={null}
          />
        </div>
        <div className="col-6">
          <Input
            {...customerValidations.confirmPasswordValidation}
            label="Confirmar contraseña"
            isAuthInput={false}
            placeholder="Anótala..."
            icon={null}
          />
        </div>
      </div>

      <div className="row mb-3">
        <Input
          {...customerValidations.addressValidation}
          label="Dirección"
          isAuthInput={false}
          placeholder="Av. Bernal"
          icon={null}
        />
      </div>
    </>
  );
};
