import style from "../css/Auth.module.css";
import { useFormContext } from "react-hook-form";
import { AnimatePresence, motion } from "framer-motion";
import { IconExclamationCircleFilled } from "@tabler/icons-react";
import { findInputError } from "../utils/findInputError";
import { isFormInvalid } from "../utils/isFormInvalid";
import Dropzone from "dropzone";
import { useEffect, useRef } from "react";

export const Input = ({
  label = "",
  type = "text",
  id = "",
  name = "",
  placeholder = "",
  icon = null,
  isAuthInput = false,
  options = [],
  defaultSelect = null,
  required = true,
  step = 1,
  min = "",
}) => {
  const {
    register,
    formState: { errors },
    setValue,
    getValues,
  } = useFormContext();

  const inputError = findInputError(errors, name);
  const isInvalid = isFormInvalid(inputError);

  //Crear un array para guardar las imagenes
  const dropzoneRef = useRef(null);
  useEffect(() => {
    if (
      type === "file" &&
      dropzoneRef.current &&
      !dropzoneRef.current.dropzone
    ) {
      const dz = new Dropzone(dropzoneRef.current, {
        url: "#",
        paramName: "file",
        autoProcessQueue: false,
        maxFilesize: 5,
        acceptedFiles: "image/jpeg,image/jpg,image/webp,image/png",
        addRemoveLinks: true,
        dictDefaultMessage: "Arrastra tus imágenes aquí o haz clic",
      });

      dz.on("addedfile", (file) => {
        if (file.previewElement) {
          const progress = file.previewElement.querySelector(".dz-progress");
          if (progress) progress.style.display = "none";
        }

        const current = Array.isArray(getValues(name)) ? getValues(name) : [];
        setValue(name, [...current, file], { shouldValidate: true });
      });

      dz.on("removedfile", (file) => {
        const current = Array.isArray(getValues(name)) ? getValues(name) : [];
        setValue(
          name,
          current.filter((f) => f !== file),
          { shouldValidate: true }
        );
      });

      return () => {
        dz.destroy();
      };
    }
  }, [type, name, setValue, getValues]);

  //Se selecciona un input, select o textarea según "type"
  const renderInput = () => {
    if (type === "select") {
      return (
        <select
          id={id}
          name={name}
          {...register(name)}
          className="form-control"
        >
          <option value="">
            {defaultSelect ? defaultSelect : "Selecciona una opción"}
          </option>
          {options.map((option, index) =>
            typeof option === "object" ? (
              <option key={option.id || index} value={option.id}>
                {option.name}
              </option>
            ) : (
              <option key={index} value={option}>
                {option}
              </option>
            )
          )}
        </select>
      );
    }

    if (type === "textarea") {
      return (
        <textarea
          id={id}
          name={name}
          className="form-control"
          placeholder={placeholder}
          {...register(name)}
        />
      );
    }

    if (type === "file") {
      return (
        <div className="dropzone border-info-subtle" id={id} ref={dropzoneRef}>
          <div className="fallback">
            <input {...register(name)} id={id} name={name} type="file" />
          </div>
          <div className="dz-message">
            <h3 className="dropzone-msg-title">{label}</h3>
            <span className="dropzone-msg-desc">
              Haz click aquí o arrastra una imagen...
            </span>
          </div>
        </div>
      );
    }

    return (
      <input
        id={id}
        type={type}
        className="form-control"
        name={name}
        placeholder={placeholder}
        step={step}
        min={min}
        {...register(name)}
      />
    );
  };

  return (
    <>
      {/* Verificar si hay un label */}
      {label ? (
        isAuthInput ? (
          <label htmlFor={id}>
            <small className="text-secondary">{label}</small>
          </label>
        ) : (
          <label className={`form-label ${required ? "required" : ""}`}>
            {label}
          </label>
        )
      ) : (
        ""
      )}

      {/* Renderizar el input */}
      <div className="w-100 mb-3">
        <div className={isAuthInput ? style.inputGroup : ""}>
          {icon}
          {renderInput()}
        </div>
        <AnimatePresence mode="wait" initial={false}>
          {isInvalid && (
            <InputError
              message={inputError.error.message}
              key={inputError.error.message}
            />
          )}
        </AnimatePresence>
      </div>
    </>
  );
};

// Mostrar el mensaje de error abajo del input
const InputError = ({ message }) => (
  <motion.p
    className="d-flex align-items-center text-start gap-1 px-2 fw-semibold text-danger rounded-4 mt-2"
    {...framer_error}
  >
    <IconExclamationCircleFilled />
    {message}
  </motion.p>
);

const framer_error = {
  initial: { opacity: 0, y: 10 },
  animate: { opacity: 1, y: 0 },
  exit: { opacity: 0, y: 10 },
  transition: { duration: 0.2 },
};
