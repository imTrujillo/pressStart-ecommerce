import * as Yup from "yup";

export const productSchema = Yup.object().shape({
  productName: Yup.string()
    .min(3, "min 6 caracteres")
    .max(50, "max 50 caracteres")
    .required("requerido")
    .matches(
      /^(?![\W_]+$)[A-Za-zÁÉÍÓÚáéíóúñÑ0-9\s,\.¡!¿?:;]+$/,
      "sin caracteres especiales"
    ),
  description: Yup.string()
    .min(20, "min 20 caracteres")
    .max(200, "max 200 caracteres")
    .required("requerido")
    .matches(
      /^(?![\W_]+$)[A-Za-zÁÉÍÓÚáéíóúñÑ0-9\s,\.¡!¿?:;]+$/,
      "sin caracteres especiales"
    ),
  price: Yup.number()
    .typeError("debe ser número")
    .positive("min $0.01")
    .required("requerido"),
  stock: Yup.number()
    .typeError("debe ser número")
    .integer("debe ser número entero")
    .min(1, "min 1")
    .required("requerido"),
  categoryId: Yup.string().required("requerido"),
  providerId: Yup.string().required("requerido"),
});

export const productValidations = {
  productNameValidation: {
    id: "productName",
    label: "Nombre",
    type: "text",
    name: "productName",
    placeholder: "Producto 01",
  },

  descriptionValidation: {
    id: "description",
    label: "Descripción",
    type: "textarea",
    name: "description",
    placeholder: "Descripción de Producto 01",
  },

  priceValidation: {
    id: "price",
    label: "Precio",
    type: "number",
    name: "price",
    placeholder: "99.99",
    step: 0.01,
  },

  stockValidation: {
    id: "stock",
    label: "Cantidad",
    type: "number",
    name: "stock",
    placeholder: "99",
    setp: 1,
  },

  categoryValidation: (categories) => ({
    id: "categoryId",
    label: "Categoría",
    type: "select",
    name: "categoryId",
    options: categories,
  }),

  providerValidation: (suppliers) => ({
    id: "providerId",
    label: "Proveedor",
    type: "select",
    name: "providerId",
    options: suppliers,
  }),

  imageValidation: {
    id: "images",
    label: "Agrega una imagen",
    type: "file",
    name: "images",
    required: false,
  },
};
