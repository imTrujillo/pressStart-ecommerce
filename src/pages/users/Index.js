import React, { useEffect, useState } from "react";
import Show from "./Show";
import { UserModal } from "../../components/modals/UserModal";
import { IconPlus } from "@tabler/icons-react";
import { DeleteModal } from "../../components/modals/DeleteModal";
import { Header } from "../../assets/Header";
import PaginationControl from "../../assets/PaginationControl";
import { EmptyState } from "../../components/EmptyState";

// NOTA: LA PAGINA DE EMPLEADOS POR AHORITA ES UNA COPIA DE CLIENTES,
// CUANDO SE AGREGUE EL CRUD DE EMPLEADOS, SE MODIFICARÁ EL INDEX Y EL SHOW

export const Index = () => {
  const [currentPage, setCurrentPage] = useState(1);
  const rowsPerPage = 5;

  //LLAMAR LA API
  const [users, setUsers] = useState([]);
  const fetchData = async () => {
    // const cust = await apiServiceGet("users");
    // setUsers(cust);
  };
  // useEffect(() => {
  //   fetchData();
  // }, []);

  //DATOS ESTATICOS
  useEffect(
    () =>
      setUsers([
        {
          id: 1,
          nombreCompleto: "Juan Pérez",
          nombreUsuario: "juanp",
          email: "juan.perez@example.com",
          fechaNacimiento: "1990-05-15",
          fechaContratacion: "2018-03-20",
          nit: "0614-250395-102-3",
          direccion: "Calle 123, San Salvador",
          salario: 500.0,
        },
        {
          id: 2,
          nombreCompleto: "María López",
          nombreUsuario: "marial",
          email: "maria.lopez@example.com",
          fechaNacimiento: "1985-11-08",
          fechaContratacion: "2015-07-10",
          nit: "0614-080985-203-9",
          direccion: "Avenida Central 456, San Miguel",
          salario: 650.5,
        },
        {
          id: 3,
          nombreCompleto: "Carlos Ramírez",
          nombreUsuario: "carlosr",
          email: "carlos.ramirez@example.com",
          fechaNacimiento: "1992-01-22",
          fechaContratacion: "2019-12-01",
          nit: "0614-220192-309-7",
          direccion: "Boulevard Los Héroes, Santa Ana",
          salario: 480.75,
        },
      ]),

    []
  );

  const totalPages = Math.ceil(users.length / rowsPerPage);

  const visibleData = users.slice(
    (currentPage - 1) * rowsPerPage,
    currentPage * rowsPerPage
  );

  //CREAR/EDITAR UN CLIENTE
  const [showModal, setShowModal] = useState(false);
  const [edit, setEdit] = useState(false);
  const closeModal = () => {
    setUser({
      id: 0,
      nombreCompleto: "",
      nombreUsuario: "",
      email: "",
      fechaNacimiento: "",
      fechaContratacion: "",
      nit: "",
      direccion: "",
      salario: 0,
    });
    setEdit(false);
    setShowModal(false);
  };
  const [user, setUser] = useState({
    id: 0,
    nombreCompleto: "",
    nombreUsuario: "",
    email: "",
    fechaNacimiento: "",
    fechaContratacion: "",
    nit: "",
    direccion: "",
    salario: 0,
  });

  const onEdit = (userEdit) => {
    setShowModal(true);
    setEdit(true);
    setUser(userEdit);
  };

  //ELIMINAR UN CLIENTE
  const [showModalDelete, setShowModalDelete] = useState(false);
  const closeModalDelete = () => {
    setUser({
      id: 0,
      nombreCompleto: "",
      nombreUsuario: "",
      email: "",
      fechaNacimiento: "",
      fechaContratacion: "",
      nit: "",
      direccion: "",
      salario: 0,
    });
    setShowModalDelete(false);
  };
  const onDelete = (userDelete) => {
    setUser(userDelete);
    setShowModalDelete(true);
  };

  return (
    <div className="page-wrapper">
      <div className="container-xl">
        <div className="row row-cards">
          {/* ENCABEZADO DE USUARIOS*/}
          <Header title="Usuarios" subtitle="Agrega y administra usuarios">
            <button
              className="btn btn-primary d-inline-block"
              onClick={() => setShowModal(true)}
            >
              <IconPlus className="me-3" />
              Agregar usuario
            </button>
          </Header>

          {/* TABLA DE CLIENTES */}
          <div className="col-12">
            <div className="card">
              <div className="table-responsive">
                <table
                  className="table table-vcenter card-table table-striped"
                  id="table-users"
                >
                  <thead>
                    <tr>
                      <th>No.</th>
                      <th>nombre completo</th>
                      <th>nombre de usuario</th>
                      <th>email</th>
                      <th>fecha de nacimiento</th>
                      <th>fecha de contratación</th>
                      <th>nit</th>
                      <th>dirección</th>
                      <th>salario</th>
                      <th className="w-1"></th>
                    </tr>
                  </thead>
                  <tbody>
                    {users.length <= 0 ? (
                      <tr>
                        <td colSpan="100%" className="text-center py-4">
                          <EmptyState text="No hay usuarios disponibles." />
                        </td>
                      </tr>
                    ) : (
                      visibleData.map((user, index) => (
                        <Show
                          key={user.id}
                          index={index + 1}
                          user={user}
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
      <UserModal
        show={showModal}
        closeModal={closeModal}
        isEdit={edit}
        user={user}
        fetchData={fetchData}
      />
      <DeleteModal
        show={showModalDelete}
        closeModal={closeModalDelete}
        id={user.id}
        endpoint="Clientes/cliente/delete/"
        fetchData={fetchData}
      />
    </div>
  );
};
