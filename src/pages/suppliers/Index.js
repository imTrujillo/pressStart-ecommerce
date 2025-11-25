import React, { useEffect, useState } from "react";
import Show from "./Show";
import { SuppliersModal } from "../../components/modals/SuppliersModal";
import { IconPlus } from "@tabler/icons-react";
import { DeleteModal } from "../../components/modals/DeleteModal";
import { Header } from "../../assets/Header";
import PaginationControl from "../../assets/PaginationControl";
import { EmptyState } from "../../components/EmptyState";
import { useApi } from "../../API/apiService";

export const Index = () => {
  const [currentPage, setCurrentPage] = useState(1);
  const rowsPerPage = 5;

  //LLAMAR LA API
  const [Proveedores, setProveedores] = useState([]);
  const { apiServiceGet } = useApi();
  const fetchData = async () => {
    try {
      const sups = await apiServiceGet("provider");
      setProveedores(sups);
    } catch (error) {
      console.error("Error al obtener proveedores:", error);
    }
  };
  useEffect(() => {
    fetchData();
  }, []);

  const totalPages = Math.ceil(Proveedores.length / rowsPerPage);

  const visibleData = Proveedores.slice(
    (currentPage - 1) * rowsPerPage,
    currentPage * rowsPerPage
  );

  //CREAR/EDITAR UN PROVEEDOR
  const [showModal, setShowModal] = useState(false);
  const closeModal = () => {
    setSupplier({
      id: 0,
      name: "",
      phoneNumber: "",
      email: "",
    });
    setShowModal(false);
  };
  const [supplier, setSupplier] = useState({
    id: 0,
    name: "",
    phoneNumber: "",
    email: "",
  });
  const [edit, setEdit] = useState(false);
  const onEdit = (supplierEdit) => {
    setShowModal(true);
    setEdit(true);
    setSupplier(supplierEdit);
  };

  //ELIMINAR UN PROVEEDOR
  const [showModalDelete, setShowModalDelete] = useState(false);
  const closeModalDelete = () => {
    setSupplier({
      id: 0,
      name: "",
      phoneNumber: "",
      email: "",
    });
    setShowModalDelete(false);
  };
  const onDelete = (supplierDelete) => {
    setSupplier(supplierDelete);
    setShowModalDelete(true);
  };

  return (
    <div className="page-wrapper">
      <div className="container-xl">
        <div className="row row-cards">
          {/* ENCABEZADO DE PROVEEDOR */}
          <Header
            title="Proveedores"
            subtitle="Anota detalles sobre proveedores"
          >
            <button
              className="btn btn-primary d-inline-block"
              onClick={() => {
                setEdit(false);
                closeModal();
                setShowModal(true);
              }}
            >
              <IconPlus className="me-3" />
              Agregar proveedor
            </button>
          </Header>
          {/* TABLA DE PROVEEDORES */}
          <div className="col-12">
            <div className="card">
              <div className="table-responsive">
                <table
                  className="table table-vcenter card-table table-striped"
                  id="table-Proveedores"
                >
                  <thead>
                    <tr>
                      <th>No.</th>
                      <th>Nombre</th>
                      <th>Teléfono</th>
                      <th>Email</th>
                      <th className="w-1"></th>
                    </tr>
                  </thead>
                  <tbody>
                    {visibleData.length === 0 ? (
                      <tr>
                        <td colSpan="100%" className="text-center py-4">
                          <EmptyState text="No hay proveedores disponibles." />
                        </td>
                      </tr>
                    ) : (
                      visibleData.map((supplier, index) => (
                        <Show
                          key={supplier.id}
                          index={index + 1}
                          supplier={supplier}
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
      <SuppliersModal
        show={showModal}
        closeModal={closeModal}
        isEdit={edit}
        supplier={supplier}
        fetchData={fetchData}
      />
      <DeleteModal
        show={showModalDelete}
        closeModal={closeModalDelete}
        id={supplier.id}
        endpoint="Provider/provider/delete/"
        fetchData={fetchData}
      />
    </div>
  );
};
