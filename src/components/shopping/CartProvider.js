import React, { createContext, useContext, useEffect, useState } from "react";

//CREAR EL CARRITO DE COMPRAS
const CartContext = createContext();
export const CartProvider = ({ children }) => {
  // CARGAR CARRITO DESDE SESSIONSTORAGE
  const [productsCart, setProductsCart] = useState(() => {
    const storedCart = sessionStorage.getItem("productsCart");
    return storedCart ? JSON.parse(storedCart) : [];
  });

  //CARGAR DETALLES DEL PEDIDO
  const [orderDetails, setOrderDetails] = useState(() => {
    const stored = sessionStorage.getItem("orderDetails");
    return stored ? JSON.parse(stored) : {};
  });

  //REALIZAR EL PROCESO DE PAGO UNA SOLA VEZ
  const [payments, setPayments] = useState(() => {
    const stored = sessionStorage.getItem("payments");
    return stored ? JSON.parse(stored) : {};
  });

  // GUARDAR CARRITO EN SESSIONSTORAGE CADA VEZ QUE CAMBIA
  useEffect(() => {
    sessionStorage.setItem("productsCart", JSON.stringify(productsCart));
  }, [productsCart]);

  return (
    <CartContext.Provider
      value={{
        productsCart,
        setProductsCart,
        orderDetails,
        setOrderDetails,
        payments,
        setPayments,
      }}
    >
      {children}
    </CartContext.Provider>
  );
};

export const useCart = () => useContext(CartContext);
