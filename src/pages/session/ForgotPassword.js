import style from "../../css/Auth.module.css";
import { Link } from "react-router-dom";

import { IconShieldLockFilled } from "@tabler/icons-react";
import { useAuth } from "./AuthProvider";
import { toast } from "react-toastify";
import { FormProvider, useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { Input } from "../../components/Input";
import {
  setNewPasswordSchema,
  setNewPasswordValidations,
} from "../../validations/setNewPasswordSchema";

export const ForgotPassword = () => {
  //Utilizar yup para validar formulario
  const methods = useForm({
    resolver: yupResolver(setNewPasswordSchema),
    defaultValues: {
      username: "",
      dui: "",
      newPassword: "",
      confirmPassword: "",
    },
  });

  //Envío de los datos a la API
  const auth = useAuth();
  const onSubmit = methods.handleSubmit(async (data) => {
    try {
      console.log(data);
      await auth.setNewPassword(data);
    } catch (err) {
      console.error("Error de actualización de contraseña:", err);
      toast.error("Error al actualizar la contraseña. Intenta de nuevo.");
    }
  });

  return (
    <div className={style.loginContainer}>
      <div className={style.loginCard}>
        <div className={style.loginHeader}>
          <IconShieldLockFilled className={style.loginIcon} />
          <h2>Verifica tu identidad</h2>
          <p>Ingresa tus datos para restablecer tu contraseña.</p>
        </div>
        <FormProvider {...methods}>
          <form className="forgot-password-form" onSubmit={onSubmit} noValidate>
            <Input {...setNewPasswordValidations.usernameValidation} />
            <Input {...setNewPasswordValidations.duiValidation} />
            <Input {...setNewPasswordValidations.newPasswordValidation} />
            <Input {...setNewPasswordValidations.confirmPasswordValidation} />
            <button type="submit" className={style.loginButton}>
              Restablecer Contraseña
            </button>
          </form>
        </FormProvider>

        <div className={style.signupLink}>
          <p className="d-flex justify-content-center gap-2">
            ¿Recordaste tu contraseña?
            <Link to="/login">Volver al inicio de sesión</Link>
          </p>
        </div>
      </div>
    </div>
  );
};
