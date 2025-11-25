import React, { useEffect, useState } from "react";
import ShopProducts from "../../components/shopping/ShopProducts";
import { EmptyState } from "../../components/EmptyState";
import { Header } from "../../assets/Header";
import PaginationControl from "../../assets/PaginationControl";
import { IconSearch } from "@tabler/icons-react";
import { Input } from "../../components/Input";
import { searchValidations } from "../../validations/searchSchema";
import { FormProvider, useForm } from "react-hook-form";
import { toast } from "react-toastify";
import { CartCanvas } from "../../components/shopping/CartCanvas";
import { useCart } from "../../components/shopping/CartProvider";
import { useApi } from "../../API/apiService";

export const Index = () => {
  //OBTENER DEL CARRITO DE COMPRA
  const { productsCart, setProductsCart } = useCart();

  //OBTENER PRODUCTOS, CATEGORIAS, PROVEEDORES
  const [products, setProducts] = useState([]);
  const [categories, setCategories] = useState([]);
  const [providers, setProviders] = useState([]);
  const { apiServiceGet } = useApi();
  const fetchData = async () => {
    const query = new URLSearchParams(window.location.search).toString();
    const prods = await apiServiceGet(`product?${query}`);
    const cats = await apiServiceGet("category");
    const prov = await apiServiceGet("provider");
    setProducts(prods.products);
    setCategories(cats);
    setProviders(prov);
  };
  useEffect(() => {
    fetchData();
  }, []);

  //Paginación
  const [currentPage, setCurrentPage] = useState(1);
  const rowsPerPage = 5;
  const totalPages = Math.ceil(products.length / rowsPerPage);
  const visibleData = products.slice(
    (currentPage - 1) * rowsPerPage,
    currentPage * rowsPerPage
  );

  //Preparar datos a enviar como parámetros de la url
  const methods = useForm({
    defaultValues: {
      search: "",
      categoryId: "",
      providerId: "",
    },
  });

  //Envío de los datos a la API
  const onSubmit = methods.handleSubmit(async (data) => {
    try {
      const params = new URLSearchParams();

      if (data.search) params.append("search", data.search);
      if (data.categoryId) params.append("categoryId", data.categoryId);
      if (data.providerId) params.append("providerId", data.providerId);

      window.history.replaceState(null, "", `?${params.toString()}`);

      const prods = await apiServiceGet(`product?${params.toString()}`);
      setProducts(prods.products);
    } catch (err) {
      toast.error("Error de búsqueda. Intenta de nuevo.");
    }
  });

  return (
    <div className="page-wrapper">
      <div className="container py-4 d-flex flex-column flex-md-row gap-5 h-100">
        <div className="card p-3">
          <Header
            title="Búsqueda avanzada"
            subtitle="Filtrar por categoría y proveedor"
          />

          <FormProvider {...methods}>
            <form noValidate onSubmit={onSubmit} className="mt-5">
              <Input {...searchValidations.searchValidation} />
              <Input {...searchValidations.categoryValidation(categories)} />
              <Input {...searchValidations.providerValidation(providers)} />
              <button type="submit" className="btn w-100 cursor-pointer">
                <IconSearch className="me-2" />
                Filtrar
              </button>
            </form>
          </FormProvider>
        </div>

        <div className="row w-100">
          {visibleData?.length <= 0 ? (
            <EmptyState text="No hay productos disponibles." />
          ) : (
            visibleData?.map((p) => (
              <div key={p.productId} className="col-12 col-md-6 col-lg-4">
                <ShopProducts product={p} />
              </div>
            ))
          )}

          <div className="col-12 d-flex justify-content-center mt-4">
            <PaginationControl
              count={totalPages}
              page={currentPage}
              onChange={(event, value) => setCurrentPage(value)}
            />
          </div>
        </div>
      </div>

      {/* CARRITO DE COMPRAS */}
      <CartCanvas
        productsCart={productsCart}
        setProductsCart={setProductsCart}
        fetchData={fetchData}
      />
    </div>
  );
};
