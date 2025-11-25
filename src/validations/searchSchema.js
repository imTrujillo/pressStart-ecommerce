export const searchValidations = {
  searchValidation: {
    id: "search",
    type: "text",
    name: "search",
    placeholder: "Buscar producto...",
  },

  categoryValidation: (categories) => ({
    id: "categoryId",
    type: "select",
    defaultSelect: "Categoría...",
    name: "categoryId",
    options: categories,
  }),

  providerValidation: (suppliers) => ({
    id: "providerId",
    type: "select",
    defaultSelect: "Proveedor...",
    name: "providerId",
    options: suppliers,
  }),
};
