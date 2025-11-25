import React from "react";
import { IconPencil, IconTrash } from "@tabler/icons-react";

const Show = ({ user, index, onEdit, onDelete }) => {
  return (
    <tr>
      <td data-label="IDProducto">
        <div className="d-flex py-1 align-items-center">
          <span>{index}</span>
        </div>
      </td>
      <td data-label="nombreProducto">
        <div className="font-weight-medium">{user.nombreCompleto}</div>
      </td>
      <td className="text-secondary" data-label="Role">
        {user.nombreUsuario}
      </td>
      <td className="text-secondary" data-label="Role">
        {user.email}
      </td>
      <td className="text-secondary" data-label="Role">
        {user.fechaNacimiento}
      </td>
      <td className="text-secondary" data-label="Role">
        {user.fechaContratacion}
      </td>
      <td className="text-secondary" data-label="Role">
        {user.nit}
      </td>
      <td className="text-secondary" data-label="Role">
        {user.direccion}
      </td>
      <td className="text-secondary" data-label="Role">
        ${user.salario}
      </td>
      <td>
        <div className="btn-list flex-nowrap">
          <div className="dropdown">
            <button
              className="btn dropdown-toggle align-text-top"
              data-bs-toggle="dropdown"
            >
              Acciones
            </button>
            <div className="dropdown-menu dropdown-menu-end">
              <button
                className="dropdown-item text-yellow"
                onClick={() => onEdit(user)}
              >
                <IconPencil />
                Editar
              </button>
              <button
                className="dropdown-item text-danger"
                onClick={() => onDelete(user)}
              >
                <IconTrash />
                Eliminar
              </button>
            </div>
          </div>
        </div>
      </td>
    </tr>
  );
};

export default Show;
