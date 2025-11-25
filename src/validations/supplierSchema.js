import * as Yup from "yup";

export const supplierSchema = Yup.object().shape({
  name: Yup.string()
    .min(6, "min 50 caracteres")
    .required("requerido")
    .matches(
      /^(?![\W_]+$)[A-Za-zÁÉÍÓÚáéíóúñÑ0-9\s,\.¡!¿?:;]+$/,
      "sin caracteres especiales"
    ),
  phoneNumber: Yup.string()
    .required("requerido")
    .matches(/^[0-9]+$/, "solo números")
    .length(8, "len: 8 dígitos"),
  email: Yup.string().email("no válido").required("requerido"),
  // isActive: Yup.boolean()
  //   .transform((value, originalValue) => {
  //     if (originalValue === "true") return true;
  //     if (originalValue === "false") return false;
  //     return null;
  //   })
  //   .required("requerido"),
});

export const supplierValidations = {
  nameValidation: {
    id: "name",
    label: "Nombre",
    type: "text",
    name: "name",
    placeholder: "Proveedor 01",
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

  phoneNumberValidation: {
    id: "phoneNumber",
    label: "Teléfono",
    type: "text",
    name: "phoneNumber",
    placeholder: "12345678",
  },

  emailValidation: {
    id: "email",
    label: "Correo",
    type: "email",
    name: "email",
    placeholder: "proveedor@proveedor.com",
  },
};
