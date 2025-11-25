import React from "react";
import { IconImageInPicture, IconPencil, IconTrash } from "@tabler/icons-react";
import { Link } from "react-router-dom";

const Show = ({ product, index, onEdit, onDelete }) => {
  return (
    <tr>
      <td data-label="IDProducto">
        <div className="d-flex py-1 align-items-center">
          <span>{index}</span>
        </div>
      </td>
      <td data-label="NombreProducto">
        <div className="font-weight-medium">{product.productName}</div>
      </td>
      <td className="text-secondary" data-label="Role">
        {product.description}
      </td>
      <td className="text-secondary" data-label="Role">
        $ {product.price}
      </td>
      <td className="text-secondary" data-label="Role">
        {product.stock}
      </td>
      <td className="text-secondary" data-label="Role">
        {product.categoryName}
      </td>
      <td className="text-secondary" data-label="Role">
        {product.providerName}
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
              <Link
                className="dropdown-item text-orange"
                to={`/inventario/${product.productId}/imgs`}
              >
                <IconImageInPicture />
                Imágenes
              </Link>
              <button
                className="dropdown-item text-yellow"
                onClick={() => onEdit(product)}
              >
                <IconPencil />
                Editar
              </button>
              <button
                className="dropdown-item text-danger"
                onClick={() => onDelete(product.productId)}
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
