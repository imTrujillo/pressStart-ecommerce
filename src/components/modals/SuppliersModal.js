import React, { useEffect, useState } from "react";
import { toast } from "react-toastify";
import { FormProvider, useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { Input } from "../Input";
import {
  supplierSchema,
  supplierValidations,
} from "../../validations/supplierSchema"; // VALIDACIONES CON YUP
import { useApi } from "../../API/apiService";

export const SuppliersModal = ({
  show,
  closeModal,
  isEdit,
  supplier,
  fetchData,
}) => {
  //Utilizar yup para validar formulario
  const methods = useForm({
    resolver: yupResolver(supplierSchema),
    defaultValues: {
      id: 0,
      name: "",
      phoneNumber: "",
      email: "",
    },
  });

  useEffect(() => {
    if (supplier) {
      methods.reset({
        id: supplier.id || 0,
        name: supplier.name || "",
        phoneNumber: supplier.phoneNumber || "",
        email: supplier.email || "",
      });
    }
  }, [supplier]);

  const { apiServicePost, apiServiceUpdate } = useApi();
  const onSubmit = methods.handleSubmit(async (data) => {
    try {
      if (isEdit) {
        await apiServiceUpdate(
          `provider/provider/update/${data.id}`,
          data,
          true
        );
      } else {
        await apiServicePost("provider", data, true);
      }
      closeModal();
      fetchData();
      toast.success(
        isEdit ? "¡Proveedor actualizado!" : "¡Proveedor agregado!"
      );
      console.log(data);
    } catch (err) {
      console.error("Error al guardar el proveedor:", err);
      toast.error("Error al guardar el proveedor. Intenta de nuevo.");
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
                  {isEdit ? "Editar Proveedor" : "Agregar Proveedor"}
                </h5>
                <button
                  type="button"
                  className="btn-close"
                  onClick={closeModal}
                ></button>
              </div>

              <div className="modal-body">
                <div className="row mb-3">
                  <Input {...supplierValidations.nameValidation} />
                </div>
                <div className="row mb-3">
                  <Input {...supplierValidations.emailValidation} />
                </div>
                <div className="row mb-3">
                  <Input {...supplierValidations.phoneNumberValidation} />
                </div>
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
