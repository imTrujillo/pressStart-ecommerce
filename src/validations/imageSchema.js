import * as Yup from "yup";

export const imgSchema = Yup.object().shape({
  images: Yup.array()
    .min(1, "requerida")
    .test("fileType", "Solo JPG, JPEG, PNG, WEBP", (files) => {
      return files.every((file) =>
        ["image/jpeg", "image/jpg", "image/png", "image/webp"].includes(
          file.type
        )
      );
    })
    .test("fileSize", "MAX 5MB", (files) => {
      return files.every((file) => file.size <= 5 * 1024 * 1024);
    }),
});

export const imgValidations = {
  imageValidation: {
    id: "images",
    label: "Agrega varias imágenes",
    type: "file",
    name: "images",
    required: false,
  },
};
