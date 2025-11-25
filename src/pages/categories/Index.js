import React, { useEffect, useState } from "react";
import Show from "./Show";
import { CategoryModal } from "../../components/modals/CategoryModal";
import { IconPlus } from "@tabler/icons-react";
import { DeleteModal } from "../../components/modals/DeleteModal";
import { Header } from "../../assets/Header";
import PaginationControl from "../../assets/PaginationControl";
import { EmptyState } from "../../components/EmptyState";
import { useApi } from "../../API/apiService";

export const Index = () => {
  const [categories, setCategories] = useState([]);
  const [currentPage, setCurrentPage] = useState(1);
  const rowsPerPage = 5;

  const { apiServiceGet } = useApi();
  const fetchData = async () => {
    const cat = await apiServiceGet("category");
    setCategories(cat);
  };

  useEffect(() => {
    fetchData();
  }, []);

  const totalPages = Math.ceil(categories.length / rowsPerPage);

  const visibleData = categories.slice(
    (currentPage - 1) * rowsPerPage,
    currentPage * rowsPerPage
  );

  // Modal y lógica
  const [showModal, setShowModal] = useState(false);
  const [category, setCategory] = useState({
    id: 0,
    name: "",
    description: "",
  });
  const [edit, setEdit] = useState(false);
  const closeModal = () => {
    setCategory({ id: 0, name: "", description: "" });
    setShowModal(false);
  };
  const onEdit = (categoryEdit) => {
    setShowModal(true);
    setEdit(true);
    setCategory(categoryEdit);
  };
  const handleModal = () => {
    setShowModal(true);
    setEdit(false);
  };

  const [showModalDelete, setShowModalDelete] = useState(false);
  const [categoryDelete, setCategoryDelete] = useState(0);
  const closeModalDelete = () => {
    setShowModalDelete(false);
    setCategoryDelete(0);
  };
  const onDelete = (categoryId) => {
    setShowModalDelete(true);
    setCategoryDelete(categoryId);
  };

  return (
    <div className="page-wrapper">
      <div className="container-xl">
        <div className="row row-cards">
          <Header title="Categorías" subtitle="Clasifica tus productos">
            <button
              className="btn btn-primary d-inline-block"
              onClick={handleModal}
            >
              <IconPlus className="me-3" />
              Agregar categoría
            </button>
          </Header>

          <div className="col-12">
            <div className="card">
              <div className="table-responsive">
                <table
                  className="table table-vcenter card-table table-striped"
                  id="table-categories"
                >
                  <thead>
                    <tr>
                      <th>No.</th>
                      <th>Nombre</th>
                      <th>Descripción</th>
                      <th className="w-1"></th>
                    </tr>
                  </thead>
                  <tbody>
                    {visibleData.length <= 0 ? (
                      <tr>
                        <td colSpan="100%" className="text-center py-4">
                          <EmptyState text="No hay categorías disponibles." />
                        </td>
                      </tr>
                    ) : (
                      visibleData.map((category, index) => (
                        <Show
                          key={category.id}
                          index={index + 1}
                          category={category}
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
      <CategoryModal
        show={showModal}
        closeModal={closeModal}
        isEdit={edit}
        category={category}
        fetchData={fetchData}
      />
      <DeleteModal
        show={showModalDelete}
        closeModal={closeModalDelete}
        id={categoryDelete}
        endpoint="category/category/delete/"
        fetchData={fetchData}
      />
    </div>
  );
};
