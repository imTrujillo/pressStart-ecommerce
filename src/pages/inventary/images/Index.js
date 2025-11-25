import React, { useEffect, useState } from "react";

import { Header } from "../../../assets/Header";
import PaginationControl from "../../../assets/PaginationControl";
import { useParams } from "react-router-dom";
import { Input } from "../../../components/Input";

import { FormProvider, useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { toast } from "react-toastify";
import { EmptyState } from "../../../components/EmptyState";
import { Show } from "./Show";
import { imgSchema, imgValidations } from "../../../validations/imageSchema";
import { useApi } from "../../../API/apiService";

export const Index = () => {
  //Obtener id del product según la url
  const { id } = useParams();
  const { apiServiceGet, apiServicePost, apiServiceUpdate, apiServiceDelete } =
    useApi();
  //Eliminar el filtro de la imagen en el modo claro
  const savedTheme = localStorage.getItem("theme");

  const [currentPage, setCurrentPage] = useState(1);
  const rowsPerPage = 5;

  //LLAMAR LA API

  const [imgs, setImgs] = useState([]);
  const fetchData = async () => {
    const images = await apiServiceGet(`product/${id}/images`);
    setImgs(images);
  };

  useEffect(() => {
    fetchData();
  });

  const totalPages = Math.ceil(imgs.length / rowsPerPage);

  const visibleData = imgs.slice(
    (currentPage - 1) * rowsPerPage,
    currentPage * rowsPerPage
  );

  //Utilizar yup para validar formulario
  const methods = useForm({
    resolver: yupResolver(imgSchema),
    defaultValues: {
      images: [],
    },
  });

  //AGREGAR IMAGENES EN LA BASE DE DATOS
  const onSubmit = methods.handleSubmit(async (data) => {
    try {
      const formData = new FormData();

      data.images.forEach((file) => {
        formData.append("images", file);
      });

      await apiServicePost(`product/${id}/images/multiple`, formData, true);

      setImgs([]);
      fetchData();
      toast.success("¡Lista de imágenes agregada!");
    } catch (err) {
      console.error("Error al guardar la lista:", err);
      toast.error("Error al guardar la lista. Intenta de nuevo.");
    }
  });

  //ESTABLECER COMO FOTO PRINCIPAL
  const handleMainPhoto = async (imageId) => {
    try {
      await apiServiceUpdate(
        `product/${id}/images/${imageId}/principal`,
        null,
        true
      );

      fetchData();
      toast.success("¡Portada actualizada!");
    } catch (err) {
      console.error("Error al actualizar la portada:", err);
      toast.error("Error al actualizar la portada. Intenta de nuevo.");
    }
  };

  //ELIMINAR UN PRODUCTO
  const handleDelete = async (imageId) => {
    try {
      await apiServiceDelete(`product/${id}/images/${imageId}`, true);

      fetchData();
      toast.success("¡Imagen eliminada!");
    } catch (err) {
      console.error("Error al eliminar la imagen:", err);
      toast.error("Error al eliminar la imagen. Intenta de nuevo.");
    }
  };

  return (
    <div className="page-wrapper">
      <div className="container-xl">
        <div className="row row-cards">
          {/* ENCABEZADO DE INVENTARIO */}
          <Header title={`Galería`} subtitle="Organiza fotos por producto" />

          {/* GALERÍA DE IMÁGENES */}
          <div className=" page-wrapper d-flex flex-column-reverse flex-md-row align-items-start">
            <div className="page-body col-12 col-md-8">
              {visibleData.length ? (
                <>
                  <div className="container-xl align-items-center">
                    <div className="row row-cards">
                      {visibleData.map((img) => (
                        <Show
                          key={img.imageId}
                          img={img}
                          savedTheme={savedTheme}
                          handleMainPhoto={handleMainPhoto}
                          handleDelete={handleDelete}
                        />
                      ))}
                    </div>

                    <div className="d-flex justify-content-center mt-3">
                      <PaginationControl
                        count={totalPages}
                        page={currentPage}
                        onChange={(event, value) => setCurrentPage(value)}
                      />
                    </div>
                  </div>
                </>
              ) : (
                <EmptyState text="No hay imágenes disponibles." />
              )}
            </div>

            <FormProvider {...methods}>
              <form noValidate onSubmit={onSubmit} className="col-12 col-md-4">
                <div className="row mb-3">
                  <Input {...imgValidations.imageValidation} />
                </div>
                <button
                  className="btn btn-primary d-inline-block"
                  type="submit"
                >
                  Agregar imagen
                </button>
              </form>
            </FormProvider>
          </div>
        </div>
      </div>
    </div>
  );
};
