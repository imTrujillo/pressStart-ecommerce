import React, { useEffect, useState } from "react";
import { IconPlus } from "@tabler/icons-react";
import "@progress/kendo-theme-bootstrap/dist/all.css";
import { toast } from "react-toastify";
import { FormProvider, useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { Input } from "../Input";
import { useAuth } from "../../pages/session/AuthProvider";
import { orderSchema, orderValidations } from "../../validations/orderSchema"; // VALIDACIONES CON YUP
import { useApi } from "../../API/apiService";

export const OrderModal = ({
  show,
  closeModal,
  isEdit,
  order,
  products,
  fetchData,
}) => {
  const [selectedProductId, setSelectedProductId] = useState("");
  const [orderProducts, setOrderProducts] = useState([]);

  //Utilizar yup para validar formulario
  const { token } = useAuth();
  const methods = useForm({
    resolver: yupResolver(orderSchema),
    defaultValues: {
      userId: token.user?.id ?? 0,
      orderStatus: 3,
      address: "",
      details: [],
    },
  });

  useEffect(() => {
    if (order?.details) {
      setOrderProducts(
        order.details.map((p) => {
          const product = products.find(
            (prod) => prod.productName === p.productName
          );
          return {
            quantity: p.quantity,
            price: p.unitPrice,
            productName: p.productName,
            productId: product.productId,
            stock: product.stock + p.quantity,
          };
        })
      );

      methods.reset({
        orderId: order?.orderId ?? 0,
        userId: token.user?.id ?? 0,
        orderStatus: order.status ?? 3,
        address: "",
        details: orderProducts ?? [],
      });
    }
  }, [order]);

  const handleAddProduct = (e) => {
    e.preventDefault();
    if (isNaN(selectedProductId)) return;

    const prod = products?.find(
      (p) => p.productId === Number(selectedProductId)
    );

    if (prod) {
      if (orderProducts.some((p) => p.productId === prod.productId)) {
        toast("El producto ya fue agregado.");
        return;
      } else if (prod.stock <= 0) {
        toast("Producto agotado.");
        return;
      }
      setOrderProducts((prev) => [
        ...prev,
        {
          productId: prod.productId,
          quantity: 1,
          price: prod.price,
          productName: prod.productName,
          stock: prod.stock,
        },
      ]);

      setSelectedProductId("");
    }
  };

  const handleChangeCantidad = (index, value) => {
    const updated = [...orderProducts];
    const product = updated[index];

    product.quantity = Math.max(Math.min(value, product.stock), 1);

    setOrderProducts(updated);
  };

  const handleDeleteProduct = (index) => {
    const updated = orderProducts.filter((_, i) => i !== index);
    setOrderProducts(updated);
  };

  //Envío de los datos a la API
  const { apiServicePost, apiServiceUpdate } = useApi();
  const onSubmit = methods.handleSubmit(async (data) => {
    try {
      if (orderProducts.length <= 0) {
        toast.error("No hay productos guardados.");
        return;
      }

      const orderData = {
        orderId: data.orderId,
        userId: data.userId ?? 0,
        orderStatus: Number(data.orderStatus),
        address: data.address ?? "",
        details: orderProducts.map(({ stock, productName, ...rest }) => rest),
      };
      console.log(orderData);
      let orderResponse;
      if (!isEdit) {
        orderResponse = await apiServicePost("orders", orderData, true);
      } else {
        orderResponse = await apiServiceUpdate(
          `orders/update/${data.orderId}`,
          orderData,
          true
        );
      }
      if (orderResponse) {
        toast.success(
          isEdit
            ? "¡Pedido actualizado con éxito!"
            : "¡Pedido registrado con éxito!"
        );
        closeModal();
        fetchData();
      }
    } catch (err) {
      console.error("Error al guardar el pedido:", err);
      toast.error("Error al guardar el pedido. Intenta de nuevo.");
    }
  });

  if (!show) return null;

  return (
    <div
      className="modal d-block position-fixed overflow-y-scroll pb-5 show fade modal-blur"
      tabIndex="-1"
      role="dialog"
    >
      <div className="modal-dialog modal-lg">
        <FormProvider {...methods}>
          <form noValidate onSubmit={onSubmit}>
            <div className="modal-content">
              <div className="modal-header">
                <h5 className="modal-title">
                  {isEdit ? "Editar Pedido" : "Agregar Pedido"}
                </h5>
                <button
                  type="button"
                  className="btn-close"
                  onClick={closeModal}
                ></button>
              </div>
              <div className="modal-body">
                {/*  Estado */}
                <div className="row mb-3">
                  <Input {...orderValidations.orderStatusValidation} />
                </div>

                {/* Dirección */}
                <div className="row mb-3">
                  <Input {...orderValidations.addressValidation} />
                </div>

                {/* Productos */}
                <div className="row mb-3">
                  <h6 className="mt-2 mb-4 fs-2">Lista de Productos</h6>
                  <div className="row mb-3">
                    <div className="col-9">
                      <select
                        name="productId"
                        className="form-select"
                        value={selectedProductId}
                        onChange={(e) => setSelectedProductId(e.target.value)}
                      >
                        <option value="">Selecciona un producto</option>
                        {products?.map((p) => (
                          <option key={p.productId} value={p.productId}>
                            {p.productName}
                          </option>
                        ))}
                      </select>
                    </div>
                    <div className="col-3">
                      <button
                        onClick={handleAddProduct}
                        className="btn btn-primary w-100"
                      >
                        <IconPlus className="me-2" /> Agregar
                      </button>
                    </div>
                  </div>
                  <div style={{ maxHeight: "200px", overflowY: "auto" }}>
                    {orderProducts.length > 0 && (
                      <table className="table table-striped responsive">
                        <thead className="sticky-top">
                          <tr>
                            <th>#</th>
                            <th>Producto</th>
                            <th>Cantidad</th>
                            <th>Subtotal</th>
                            <th>Acción</th>
                          </tr>
                        </thead>
                        <tbody>
                          {orderProducts?.map((prod, idx) => (
                            <tr key={idx} className="align-middle">
                              <td>{idx + 1}</td>
                              <td>{prod.productName}</td>
                              <td style={{ maxWidth: "60px" }}>
                                <input
                                  type="number"
                                  className="form-control"
                                  value={prod.quantity}
                                  onChange={(e) =>
                                    handleChangeCantidad(idx, e.target.value)
                                  }
                                  min="1"
                                />
                              </td>
                              <td>
                                $ {(prod.quantity * prod.price).toFixed(2)}
                              </td>
                              <td>
                                <button
                                  type="button"
                                  className="btn btn-danger"
                                  onClick={() => handleDeleteProduct(idx)}
                                >
                                  X
                                </button>
                              </td>
                            </tr>
                          ))}
                        </tbody>
                      </table>
                    )}
                    <div className="text-end fs-4 p-4">
                      <strong>
                        Total: ${" "}
                        {orderProducts
                          .reduce((acc, p) => acc + p.quantity * p.price, 0)
                          .toFixed(2)}
                      </strong>
                    </div>
                  </div>
                </div>
              </div>

              <div className="modal-footer">
                <button type="button" className="btn" onClick={closeModal}>
                  Cancelar
                </button>
                <button type="submit" className="btn btn-primary">
                  {isEdit ? "Editar" : "Guardar"}
                </button>
              </div>
            </div>
          </form>
        </FormProvider>
      </div>
    </div>
  );
};
