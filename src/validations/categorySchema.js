import * as Yup from "yup";

//VALIDACIONES DE CATEGORIA
export const categorySchema = Yup.object().shape({
  name: Yup.string()
    .required("requerido")
    .min(6, "min 6 caracteres")
    .max(50, "max 50 caracteres")
    .matches(
      /^(?![\W_]+$)[A-Za-zÁÉÍÓÚáéíóúñÑ0-9\s,\.¡!¿?:;]+$/,
      "sin caracteres especiales"
    ),
  description: Yup.string()
    .required("requerido")
    .min(20, "min 20 caracteres")
    .max(200, "max 200 caracteres")
    .matches(
      /^(?![\W_]+$)[A-Za-zÁÉÍÓÚáéíóúñÑ0-9\s,\.¡!¿?:;]+$/,
      "sin caracteres especiales"
    ),
  // isActive: Yup.boolean()
  //   .transform((value, originalValue) => {
  //     if (originalValue === "true") return true;
  //     if (originalValue === "false") return false;
  //     return null;
  //   })
  //   .required("requerido"),
});

export const categoryValidations = {
  nameValidation: {
    id: "name",
    label: "Nombre",
    type: "text",
    name: "name",
    placeholder: "Categoría 01",
  },

  // isActiveValidation: {
  //   id: "isActive",
  //   label: "Estado",
  //   type: "select",
  //   options: [
  //     { id: true, name: "Activo" },
  //     { id: false, name: "Inactivo" },
  //   ],
  //   name: "isActive",
  // },

  descriptionValidation: {
    id: "description",
    label: "Descripción",
    type: "textarea",
    name: "description",
    placeholder: "Descripción de Categoría 01",
  },
};
