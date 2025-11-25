import React, { useState } from "react";
import PaginationControl from "../../assets/PaginationControl";

export const ProductsOrderModal = ({
  show,
  closeProductsModal,
  orderProducts,
}) => {
  const [currentPage, setCurrentPage] = useState(1);
  const rowsPerPage = 5;

  const totalPages = Math.ceil(orderProducts.length / rowsPerPage);

  const visibleData = orderProducts.slice(
    (currentPage - 1) * rowsPerPage,
    currentPage * rowsPerPage
  );

  if (!show) return null;

  return (
    <div
      className="modal d-block position-fixed overflow-y-scroll pb-5 show fade modal-blur"
      tabIndex="-1"
      role="dialog"
    >
      <div className="modal-dialog modal-lg">
        {/* MODAL PARA MOSTRAR PRODUCTOS DE UN PEDIDO */}
        <div className="modal-content">
          <div className="modal-header">
            <h5 className="modal-title">Lista de Productos</h5>
            <button
              type="button"
              className="btn-close"
              onClick={closeProductsModal}
            ></button>
          </div>
          <div className="modal-body">
            <div className="table-responsive">
              <table className="table table-vcenter card-table">
                <thead>
                  <tr>
                    <th>Producto</th>
                    <th>Cantidad</th>
                    <th>Precio Unitario</th>
                  </tr>
                </thead>
                <tbody>
                  {visibleData.map((detail) => {
                    return (
                      <tr key={detail.id}>
                        <td>
                          {detail.productName ?? "Producto no encontrado"}
                        </td>
                        <td className="text-secondary">
                          {detail.quantity}{" "}
                          {detail.quantity === 1 ? "unidad" : "unidades"}
                        </td>
                        <td className="text-secondary">${detail.unitPrice}</td>
                      </tr>
                    );
                  })}
                </tbody>
              </table>
              <PaginationControl
                count={totalPages}
                page={currentPage}
                onChange={(event, value) => setCurrentPage(value)}
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
