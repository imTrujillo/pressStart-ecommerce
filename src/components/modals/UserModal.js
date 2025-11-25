import { toast } from "react-toastify";
import { useAuth } from "../../pages/session/AuthProvider";
import { FormProvider, useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { useEffect, useMemo, useState } from "react";
import {
  EmployeeInputs,
  employeeSchema,
} from "../../validations/employeeSchema"; // VALIDACIONES CON YUP
import {
  CustomerInputs,
  customerSchema,
} from "../../validations/customerSchema";

export const UserModal = ({ show, closeModal, isEdit, user, fetchData }) => {
  //Asignar formulario según el rol de usuario
  const [role, setRole] = useState("");

  const formConfig = useMemo(() => {
    switch (role) {
      case "employee":
        return {
          resolver: yupResolver(employeeSchema),
          defaultValues: {
            username: "",
            dateOfBirth: "",
            fullName: "",
            email: "",
            dui: "",
            phoneNumber: "",
            address: "",
            password: "",
            nit: "",
            hireDate: "",
            salary: 0,
          },
        };
      case "customer":
        return {
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
        };
      default:
        return {
          resolver: undefined,
          defaultValues: {},
        };
    }
  }, [role]);

  const methods = useForm(formConfig);

  //Asignar los datos para editar un usuario según su rol
  useEffect(() => {
    if (!user) return;

    if (role === "employee") {
      methods.reset({
        username: user.username || "",
        dateOfBirth: user.dateOfBirth || "",
        fullName: user.fullName || "",
        email: user.email || "",
        dui: user.dui || "",
        phoneNumber: user.phoneNumber || "",
        address: user.address || "",
        password: "",
        nit: user.nit || "",
        hireDate: user.hireDate || "",
        salary: user.salary || 0,
      });
    } else if (role === "customer") {
      methods.reset({
        username: user.username || "",
        dateOfBirth: user.dateOfBirth || "",
        fullName: user.fullName || "",
        email: user.email || "",
        dui: user.dui || "",
        phoneNumber: user.phoneNumber || "",
        address: user.address || "",
        password: "",
        confirmPassword: "",
      });
    }
  }, [user, role, methods]);

  //Envío de los datos a la API
  const auth = useAuth();
  const onSubmit = methods.handleSubmit(async (data) => {
    if (!role) {
      return toast.error("Debes seleccionar un rol");
    }

    try {
      if (isEdit) {
        //const dataToSend = {
        //...formData,
        //id: Number(formData.id),
        // await apiServiceUpdate(
        //   `Clientes/cliente/update/${dataToSend.id}`,
        //   dataToSend
        // );
      } else {
        await auth.signup(data, role);
      }
      closeModal();
      fetchData();
    } catch (err) {
      console.error(`Error al guardar ${role}:`, err);
      toast.error(`Error al guardar ${role}. Intenta de nuevo.`);
    }
  });

  if (!show) return null;

  return (
    <div
      className="modal d-block position-fixed overflow-y-scroll pb-5 show fade modal-blur"
      tabIndex="-1"
      role="dialog"
    >
      <div className="modal-dialog modal-lg">
        <FormProvider {...methods}>
          <form noValidate onSubmit={onSubmit}>
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">
                  {isEdit ? "Editar usuario" : "Agregar usuario"}
                </h5>
                <button
                  type="button"
                  className="btn-close"
                  onClick={closeModal}
                ></button>
              </div>

              <div className="modal-body">
                {!isEdit && (
                  <div className="row mb-3">
                    <label className="form-label required">Rol</label>
                    <select
                      defaultValue=""
                      className="form-control"
                      value={role}
                      onChange={(e) => setRole(e.target.value)}
                    >
                      <option value="">Selecciona un rol</option>
                      <option value="employee">Empleado</option>
                      <option value="customer">Cliente</option>
                    </select>
                  </div>
                )}

                {/* Renderizar inputs de acuerdo al rol */}
                {renderInputs(role)}
              </div>
              <div className="modal-footer">
                <button type="button" className="btn" onClick={closeModal}>
                  Cancelar
                </button>
                <button type="submit" className="btn btn-primary">
                  {isEdit ? "Editar" : "Guardar"}
                </button>
              </div>
            </div>
          </form>
        </FormProvider>
      </div>
    </div>
  );
};

const renderInputs = (role) => {
  switch (role) {
    case "employee":
      return <EmployeeInputs />;
    case "customer":
      return <CustomerInputs />;
    default:
      return "";
  }
};
