import { IconDeviceImacCheck, IconMenu2, IconTrash } from "@tabler/icons-react";

export const Show = ({ img, savedTheme, handleMainPhoto, handleDelete }) => {
  return (
    <div className="col-md-6">
      <div
        className={`card card-sm ${
          img.isMain ? "border-blue border-4 rounded-0" : ""
        }`}
      >
        <div className="ratio ratio-4x3 overflow-hidden">
          {savedTheme === "dark" && (
            <div className="img-filter position-absolute top-0 start-0 w-100 h-100 opacity-75 " />
          )}
          <img
            src={img.imageUrl}
            alt="galería_img"
            className="w-100 h-100 object-fit-cover"
          />
        </div>
        <div
          className="dropdown position-absolute top-0 end-0 m-2"
          style={{ zIndex: 10 }}
        >
          <button className="bg-transparent border-0" data-bs-toggle="dropdown">
            <IconMenu2 />
          </button>
          <div className="dropdown-menu dropdown-menu-end">
            {!img.isMain && (
              <button
                className="dropdown-item text-blue"
                onClick={() => handleMainPhoto(img.imageId)}
              >
                <IconDeviceImacCheck />
                Usar de portada
              </button>
            )}
            <button
              className="dropdown-item text-danger"
              onClick={() => handleDelete(img.imageId)}
            >
              <IconTrash />
              Eliminar
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};
