import { Link } from "react-router-dom";
import { EmptyState } from "../EmptyState";

const ShopProducts = ({ product }) => {
  const mainPhoto =
    product.images.find((i) => i.isMain === true) ?? product.images[0];

  return (
    <Link
      className="card product-card bg-transparent my-4 border-4 border-bottom-0 w-100 cursor-pointer"
      to={`/producto/${product.productId}`}
    >
      <div className="ratio ratio-4x3 overflow-hidden">
        {mainPhoto ? (
          <img
            src={mainPhoto.imageUrl}
            alt={product.productName}
            loading="lazy"
            className="w-100 object-fit-cover"
          />
        ) : (
          <EmptyState text="Imagen no disponible." />
        )}
      </div>

      <div className="card-body">
        <p className="text-secondary">{product.providerName}</p>

        <h3
          className="card-title overflow-x-auto overflow-y-hidden"
          style={{ maxHeight: "30px" }}
        >
          {product.productName}
        </h3>
        <p
          className="text-secondary overflow-y-auto overflow-x-hidden"
          style={{ maxHeight: "120px" }}
        >
          {product.description}
        </p>
        <h5>$ {product.price.toFixed(2)}</h5>
      </div>
    </Link>
  );
};

export default ShopProducts;
