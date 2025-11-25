import React, { useEffect } from "react";
import { toast } from "react-toastify";
import { FormProvider, useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { Input } from "../Input";
import {
  productSchema,
  productValidations,
} from "../../validations/productSchema"; // VALIDACIONES CON YUP
import { useApi } from "../../API/apiService";

export const ProductsModal = ({
  show,
  closeModal,
  isEdit,
  product,
  fetchData,
  suppliers,
  categories,
}) => {
  // VALIDACIONES CON YUP

  //Utilizar yup para validar formulario
  const methods = useForm({
    resolver: yupResolver(productSchema),
    defaultValues: {
      productName: "",
      description: "",
      price: 0.0,
      stock: 0,
      categoryId: "",
      providerId: "",
      images: [],
    },
  });

  useEffect(() => {
    if (product) {
      const supplier = suppliers.find((s) => s.name === product.providerName);
      const category = categories.find((c) => c.name === product.categoryName);
      methods.reset({
        // productId: product.productId || 0,
        productName: product.productName || "",
        description: product.description || "",
        price: product.price || 0,
        stock: product.stock || 0,
        categoryId: category?.id || "",
        providerId: supplier?.id || "",
      });
    }
  }, [product, suppliers, categories, methods]);

  const { apiServicePost, apiServiceUpdate } = useApi();
  const onSubmit = methods.handleSubmit(async (data) => {
    try {
      if (isEdit) {
        await apiServiceUpdate(`product/${product.productId}`, data, true);
      } else {
        const formData = new FormData();

        Object.entries(data).forEach(([key, value]) => {
          if (key !== "images") {
            formData.append(key, value);
          }
        });

        if (data?.images && data.images.length > 0) {
          data.images.forEach((file) => {
            formData.append("images", file);
          });
        }

        await apiServicePost("product", formData, true);
      }
      closeModal();
      fetchData();
      toast.success(isEdit ? "¡Producto actualizado!" : "¡Producto agregado!");
    } catch (err) {
      console.error("Error al guardar el producto:", err);
      toast.error("Error al guardar el producto. Intenta de nuevo.");
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
                  {isEdit ? "Editar Producto" : "Agregar Producto"}
                </h5>
                <button
                  type="button"
                  className="btn-close"
                  onClick={closeModal}
                ></button>
              </div>

              <div className="modal-body">
                <div className="row mb-3">
                  <div className="col-5">
                    <Input {...productValidations.productNameValidation} />
                  </div>
                  <div className="col-7">
                    <Input {...productValidations.descriptionValidation} />
                  </div>
                </div>

                <div className="row mb-3">
                  <div className="col-6">
                    <Input {...productValidations.priceValidation} />
                  </div>
                  <div className="col-6">
                    <Input {...productValidations.stockValidation} />
                  </div>

                  <div className="row mb-3">
                    <div className="col-5">
                      <Input
                        {...productValidations.categoryValidation(categories)}
                      />
                    </div>
                    <div className="col-7">
                      <Input
                        {...productValidations.providerValidation(suppliers)}
                      />
                    </div>
                  </div>

                  {!isEdit && (
                    <div className="row mb-3">
                      <Input {...productValidations.imageValidation} />
                    </div>
                  )}
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
