import * as Yup from "yup";

export const orderSchema = Yup.object().shape({
  address: Yup.string()
    .required("requerido")
    .min(20, "min 20 caracteres")
    .max(200, "max 200 caracteres")
    .matches(
      /^(?![\W_]+$)[A-Za-zÁÉÍÓÚáéíóúñÑ0-9\s,\.!?:;]+$/,
      "sin caracteres especiales"
    ),
});

export const orderValidations = {
  addressValidation: {
    id: "address",
    label: "Dirección de Envío",
    type: "textarea",
    name: "address",
    placeholder: "Ubicación de la recepción de pedido",
  },
  orderStatusValidation: {
    id: "orderStatus",
    label: "Estado",
    type: "select",
    options: [
      { id: 1, name: "Entregado" },
      { id: 2, name: "Empaquetado" },
      { id: 3, name: "En proceso..." },
      { id: 4, name: "Cancelado" },
    ],
    name: "orderStatus",
  },
};
