import React from "react";
import photo from "../../assets/images/producto.jpg";
import { EmptyState } from "../EmptyState";

export const ShopCart = ({ productBuy, setProductsCart }) => {
  // BORRAR UN PRODUCTO DEL CARRITO
  const deleteProduct = (productDelete) => {
    setProductsCart((prev) => prev.filter((p) => p.id !== productDelete.id));
  };

  // ACTUALIZAR CANTIDAD DE UN PRODUCTO
  const updateQuantity = (id, value) => {
    if (value >= 1) {
      setProductsCart((prev) =>
        prev.map((p) => {
          if (p.id === id) {
            const newQuantity = Math.min(Math.max(value, 1), p.stock);
            return { ...p, quantity: newQuantity };
          }
          return p;
        })
      );
    }
  };

  // Valor de cantidad (si no está definido, usa 1 por defecto)
  const quantity = productBuy.quantity ?? 1;

  const mainPhoto =
    productBuy.images.find((i) => i.isMain === true) ??
    productBuy.images[0] ??
    "";

  return (
    <div className="card position-relative">
      <div className="row row-0">
        <div className="col-3 my-auto">
          {mainPhoto ? (
            <img src={mainPhoto.imageUrl} alt={productBuy.name} />
          ) : (
            <EmptyState text="Sin imagen" />
          )}
        </div>
        <div className="col">
          <div className="card-body">
            <div>
              <h5>{productBuy.providerName}</h5>
              <h3 className="card-title">{productBuy.name}</h3>
            </div>
            <div className="d-flex align-items-center gap-2 my-2">
              <p className="my-auto">Cantidad: </p>
              <input
                type="number"
                placeholder="1"
                min="1"
                step="1"
                value={quantity}
                onChange={(e) =>
                  updateQuantity(productBuy.id, Number(e.target.value))
                }
                className="w-25"
              />
            </div>

            <strong>$ {(productBuy.price * quantity).toFixed(2)}</strong>
          </div>
        </div>
        <div className="col-2 py-2 d-flex justify-content-end">
          <button
            className="btn btn-danger btn-sm rounded-circle h-25 w-50 mx-2"
            onClick={() => deleteProduct(productBuy)}
          >
            X
          </button>
        </div>
      </div>
    </div>
  );
};
