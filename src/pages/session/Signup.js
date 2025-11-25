import style from "../../css/Auth.module.css";
import { Link } from "react-router-dom";
import { useAuth } from "./AuthProvider";
import { toast } from "react-toastify";
import { FormProvider, useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { Input } from "../../components/Input";
import {
  customerSchema,
  customerValidations,
} from "../../validations/customerSchema"; // VALIDACIONES CON YUP
import { IconLogin2 } from "@tabler/icons-react";

export const SignUp = () => {
  //Utilizar yup para validar formulario
  const methods = useForm({
    resolver: yupResolver(customerSchema),
    defaultValues: {
      username: "",
      dateOfBirth: "",
      fullName: "",
      email: "",
      dui: "",
      phoneNumber: "",
      address: "",
      password: "",
      confirmPassword: "",
    },
  });

  //Envío de los datos a la API
  const auth = useAuth();
  const onSubmit = methods.handleSubmit(async (data) => {
    try {
      const response = await auth.signup(data, "customer");
      if (response.status >= 200 && response.status < 300) {
        auth.login({
          username: response.data.username,
          password: response.data.password,
        });
      }
    } catch (err) {
      console.error("Error en register:", err);
      toast.error("Error al registrarse. Intenta de nuevo.");
    }
  });

  return (
    <div className={style.loginContainer}>
      <div className={`${style.loginCard} ${style.loginCardExpand}`}>
        <div className={style.loginHeader}>
          <IconLogin2 className={style.loginIcon} />
          <h2>Crea tu cuenta</h2>
          <p>Únete a nuestra comunidad y empieza a comprar</p>
        </div>

        <FormProvider {...methods}>
          <form
            className={`${style.createAccountForm} container`}
            onSubmit={onSubmit}
            noValidate
          >
            {/* Fecha de nacimiento */}
            <div className="row ">
              <Input {...customerValidations.DOBValidation} />
            </div>

            {/*  Nombre y DUI */}
            <div className="row">
              <div className="col-md-6">
                <Input {...customerValidations.fullNameValidation} />
              </div>

              <div className="col-md-6">
                <Input {...customerValidations.duiValidation} />
              </div>
            </div>

            <div className="row">
              <Input {...customerValidations.usernameValidation} />
            </div>

            {/*  Correo y Teléfono */}
            <div className="row">
              <div className="col-md-6">
                <Input {...customerValidations.emailValidation} />
              </div>

              <div className="col-md-6">
                <Input {...customerValidations.phoneValidation} />
              </div>
            </div>

            {/* Dirección */}
            <div className="row">
              <div className="col-12">
                <Input {...customerValidations.addressValidation} />
              </div>
            </div>

            {/* Contraseña y Confirmar */}
            <div className="row ">
              <div className="col-md-6">
                <Input {...customerValidations.passwordValidation} />
              </div>

              <div className="col-md-6">
                <Input {...customerValidations.confirmPasswordValidation} />
              </div>
            </div>

            {/* Botón */}
            <div className="text-center">
              <button type="submit" className={style.loginButton}>
                Registrarse
              </button>
            </div>
          </form>
        </FormProvider>

        <div className={style.signupLink}>
          <p className="d-flex justify-content-center gap-2">
            ¿Ya tienes una cuenta?
            <Link to="/login">Inicia sesión aquí</Link>
          </p>
        </div>
      </div>
    </div>
  );
};
