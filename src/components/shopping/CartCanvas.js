import React, { useState } from "react";
import { BuyModal } from "../modals/BuyModal";
import { ShopCart } from "./ShopCart";
import { useAuth } from "../../pages/session/AuthProvider";
import { useNavigate } from "react-router-dom";
import { toast } from "react-toastify";

export const CartCanvas = ({ productsCart, setProductsCart, fetchData }) => {
  //MODAL PARA EL PROCESO DE COMPRA
  const [showModal, setShowModal] = useState(false);

  //COMPROBAR SI EL USUARIO ESTÁ LOGEADO
  const { token } = useAuth();
  const navigate = useNavigate();
  const handleModal = () => {
    if (!token?.user) {
      return navigate("/sign-up");
    }
    return setShowModal(true);
  };
  const closeModal = () => setShowModal(false);

  return (
    <>
      <div
        className="offcanvas offcanvas-end"
        tabIndex="-1"
        id="offcanvasEnd"
        aria-labelledby="offcanvasEndLabel"
      >
        <div className="offcanvas-header">
          <h2 className="offcanvas-title" id="offcanvasEndLabel">
            Tu Pedido
          </h2>
          <button
            type="button"
            className="btn-close text-reset"
            data-bs-dismiss="offcanvas"
            aria-label="Close"
          ></button>
        </div>
        <div className="offcanvas-body">
          {productsCart.length <= 0 ? (
            <div className="col-12 text-center mt-3">¡Carrito Vacío!</div>
          ) : (
            <>
              {productsCart.map((productBuy, id) => (
                <ShopCart
                  productBuy={productBuy}
                  key={id}
                  setProductsCart={setProductsCart}
                  productsCart={productsCart}
                />
              ))}

              {/* TOTAL DEL CARRITO */}
              <div className="p-3 text-end">
                <h4>
                  Total: ${" "}
                  {productsCart
                    .reduce((acc, item) => {
                      const quantity = item.quantity ?? 1;
                      return acc + item.price * quantity;
                    }, 0)
                    .toFixed(2)}
                </h4>
              </div>

              {/* HACER PEDIDO */}
              <div className="mt-3">
                <button
                  className="btn btn-primary w-100"
                  type="button"
                  data-bs-dismiss="offcanvas"
                  onClick={() => handleModal()}
                >
                  Realizar pedido
                </button>
              </div>
            </>
          )}
        </div>
      </div>
      <BuyModal
        show={showModal}
        closeModal={closeModal}
        productsCart={productsCart}
        setProductsCart={setProductsCart}
        fetchData={fetchData}
      />
    </>
  );
};
