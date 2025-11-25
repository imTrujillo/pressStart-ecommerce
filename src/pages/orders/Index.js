import React, { useEffect, useState } from "react";
import Show from "./Show";
import { OrderModal } from "../../components/modals/OrderModal";
import { IconPlus } from "@tabler/icons-react";
import { DeleteModal } from "../../components/modals/DeleteModal";
import { ProductsOrderModal } from "../../components/modals/ProductsOrderModal";
import { Header } from "../../assets/Header";
import PaginationControl from "../../assets/PaginationControl";
import { useAuth } from "../session/AuthProvider";
import { EmptyState } from "../../components/EmptyState";
import { useApi } from "../../API/apiService";

export const Index = () => {
  const [currentPage, setCurrentPage] = useState(1);
  const rowsPerPage = 5;

  const [orders, setOrders] = useState([]);
  const [products, setProducts] = useState([]);

  //Obtener detalles del pedido
  const { token } = useAuth();
  const { apiServiceGet } = useApi();
  const fetchData = async () => {
    if (!token.user?.id) return;
    const ords = await apiServiceGet("orders/user", token.user.id, true);
    const prods = await apiServiceGet("product");
    setOrders(ords);
    setProducts(prods.products);
  };
  useEffect(() => {
    fetchData();
  }, [token.user]);

  const totalPages = Math.ceil(orders.length / rowsPerPage);
  const visibleData = orders.slice(
    (currentPage - 1) * rowsPerPage,
    currentPage * rowsPerPage
  );

  const [showModal, setShowModal] = useState(false);
  const [orderProducts, setOrderProducts] = useState([]);
  const [productsModal, setProductsModal] = useState(false);

  const openProductsModal = (products) => {
    setOrderProducts(products);
    setProductsModal(true);
  };
  const closeProductsModal = () => {
    setOrderProducts([]);
    setProductsModal(false);
  };

  const closeModal = () => {
    setOrder({
      orderId: 0,
      userId: token.user?.id ?? 0,
      orderStatus: 3,
      address: "",
      details: [],
    });
    setEdit(false);
    setShowModal(false);
  };

  const [order, setOrder] = useState({
    orderId: 0,
    userId: token.user?.id ?? 0,
    orderStatus: 3,
    address: "",
    details: [],
  });
  const [edit, setEdit] = useState(false);
  const onEdit = (orderEdit) => {
    setShowModal(true);
    setEdit(true);
    setOrder(orderEdit);
  };

  const [showModalDelete, setShowModalDelete] = useState(false);
  const [orderDelete, setOrderDelete] = useState(0);
  const closeModalDelete = () => {
    setOrder({
      orderId: 0,
      userId: token.user?.id ?? 0,
      orderStatus: 3,
      address: "",
      details: [],
    });
    setOrderDelete(0);
    setShowModalDelete(false);
  };
  const onDelete = (orderId) => {
    setOrderDelete(orderId);
    setShowModalDelete(true);
  };

  return (
    <div className="page-wrapper">
      <div className="container-xl">
        <div className="row row-cards">
          {/* ENCABEZADO DE PEDIDO */}
          <Header
            title="Mis Pedidos"
            subtitle="Edita estados y completa compras"
          >
            <button
              className="btn btn-primary d-inline-block"
              onClick={() => setShowModal(true)}
            >
              <IconPlus className="me-3" />
              Agregar pedido
            </button>
          </Header>

          {/* TABLA DE PEDIDOS */}
          <div className="col-12">
            <div className="card">
              <div className="table-responsive">
                <table
                  className="table table-vcenter card-table table-striped"
                  id="table-orders"
                >
                  <thead>
                    <tr>
                      <th>No.</th>
                      <th>Fecha</th>
                      <th>Estado</th>
                      <th>Productos</th>
                      <th className="w-1"></th>
                    </tr>
                  </thead>
                  <tbody>
                    {visibleData.length === 0 ? (
                      <tr>
                        <td colSpan="100%" className="text-center py-4">
                          <EmptyState text="No hay pedidos disponibles." />
                        </td>
                      </tr>
                    ) : (
                      visibleData.map((order, index) => (
                        <Show
                          key={order.orderId}
                          index={index + 1}
                          openProductsModal={openProductsModal}
                          order={order}
                          onEdit={onEdit}
                          onDelete={onDelete}
                        />
                      ))
                    )}
                  </tbody>
                </table>
              </div>
            </div>
            <PaginationControl
              count={totalPages}
              page={currentPage}
              onChange={(event, value) => setCurrentPage(value)}
            />
          </div>
        </div>
      </div>
      <OrderModal
        show={showModal}
        closeModal={closeModal}
        isEdit={edit}
        order={order}
        products={products}
        fetchData={fetchData}
      />
      <DeleteModal
        show={showModalDelete}
        closeModal={closeModalDelete}
        id={orderDelete}
        endpoint="orders/delete/"
        fetchData={fetchData}
      />
      <ProductsOrderModal
        show={productsModal}
        closeProductsModal={closeProductsModal}
        orderProducts={orderProducts}
      />
    </div>
  );
};
