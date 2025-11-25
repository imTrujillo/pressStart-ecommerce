import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import { EmptyState } from "../../../components/EmptyState";
import { Header } from "../../../assets/Header";

import {
  IconCheck,
  IconCircleDashedCheck,
  IconReportMoney,
} from "@tabler/icons-react";
import { toast } from "react-toastify";

import { Swiper, SwiperSlide } from "swiper/react";
import { Pagination, Navigation, Zoom } from "swiper/modules";

// Estilos de Swiper
import "swiper/css";
import "swiper/css/pagination";
import "swiper/css/navigation";
import "swiper/css/zoom";

import { useCart } from "../../../components/shopping/CartProvider";
import { CartCanvas } from "../../../components/shopping/CartCanvas";
import { useApi } from "../../../API/apiService";

export const Index = () => {
  //Obtener el product de acuerdo al id
  const { id } = useParams();
  const [product, setProduct] = useState([]);
  const { apiServiceGet } = useApi();
  const fetchData = async () => {
    const prod = await apiServiceGet("product", id);
    setProduct(prod);
  };
  useEffect(() => {
    fetchData();
  }, []);

  //Obtener el carrito de compras
  const { productsCart, setProductsCart } = useCart();

  const handleProduct = (product) => {
    if (checkProduct(product)) {
      toast.warning("Este producto ya está en el carrito");
    } else {
      toast.success("¡Producto agregado al carrito!");
      setProductsCart((prev) => [...prev, product]);
    }
  };

  const checkProduct = (product) => {
    //Agregar un producto al carrito de compras
    const alreadyInCart = productsCart.some((item) => item.id === product.id);
    return alreadyInCart;
  };

  const RenderButton = ({ product }) => {
    if (product.stock <= 0) {
      return (
        <button className="btn btn-primary disabled">
          <IconCircleDashedCheck className="me-3" />
          Producto agotado
        </button>
      );
    }
    if (checkProduct(product)) {
      return (
        <button className="btn btn-primary disabled">
          <IconCheck className="me-3" />
          Agregado al carrito
        </button>
      );
    } else {
      return (
        <button
          className="btn btn-primary"
          onClick={() => handleProduct(product)}
        >
          <IconReportMoney className="me-3" /> Agregar al carrito
        </button>
      );
    }
  };

  return (
    <div className="page-wrapper">
      <div className="container-xl">
        <div className="row row-cards">
          {/* GALERÍA DE IMÁGENES */}
          <div className=" page-wrapper d-flex flex-column-reverse flex-sm-row align-items-start">
            <div className="page-body col-12 col-sm-7 col-lg-8 px-4">
              {product.images?.length > 0 ? (
                <>
                  <Swiper
                    slidesPerView={1}
                    spaceBetween={60}
                    pagination={{
                      clickable: true,
                    }}
                    navigation={product.images?.length > 1}
                    zoom={true}
                    modules={[Pagination, Navigation, Zoom]}
                    className="mySwiper w-100"
                  >
                    {product.images.map((img) => (
                      <SwiperSlide key={img.imageId}>
                        <div className="slide-frame swiper-zoom-container cursor-pointer">
                          <img src={img.imageUrl} alt={img.fileName} />
                        </div>
                      </SwiperSlide>
                    ))}
                  </Swiper>
                </>
              ) : (
                <EmptyState text="No hay imágenes disponibles." />
              )}
            </div>

            <div className="col-12 col-sm-5 col-lg-4">
              {/* ENCABEZADO DE INVENTARIO */}
              <Header title={product?.name} subtitle={product?.provider?.name}>
                <p className="badge bg-info fw-bold fs-3 my-auto text-white">
                  {product?.category?.name}
                </p>
              </Header>

              <div className="card my-2 p-2 d-flex flex-row flex-sm-column flex-md-row justify-content-between gap-3">
                <div>
                  <h5 className="mb-4">{product.description}</h5>
                </div>

                <div>
                  <p className="fw-bold fst-italic text-green">
                    {product.stock === 1
                      ? `${product.stock} unidad disponible`
                      : `${product.stock} unidades disponibles`}
                  </p>
                  <p className="fw-bold">$ {product.price}</p>
                  <RenderButton product={product} />
                </div>
              </div>
            </div>
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
