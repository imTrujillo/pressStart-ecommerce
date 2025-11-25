import {
  IconBrandShopee,
  IconCaretLeft,
  IconReceiptFilled,
} from "@tabler/icons-react";
import { useAuth } from "../../../session/AuthProvider";
import { useCart } from "../../../../components/shopping/CartProvider";
import dayjs from "dayjs";
import { Link, useNavigate } from "react-router-dom";
import { useEffect } from "react";
import { toast } from "react-toastify";
import axios from "axios";

export const Index = () => {
  const { token } = useAuth();
  const { payments, setPayments, orderDetails } = useCart();
  const navigate = useNavigate();

  const downloadInvoice = async (fileName) => {
    try {
      const response = await axios.get(
        `https://localhost:7084/api/invoice?orderId=${orderDetails?.orderId}`,
        { responseType: "blob" }
      );

      const url = window.URL.createObjectURL(new Blob([response.data]));

      const link = document.createElement("a");
      link.href = url;
      link.download = fileName;

      document.body.appendChild(link);

      link.click();

      link.parentNode.removeChild(link);
    } catch (error) {
      console.error("Error al generar la facturación:", error);
      toast.error("Ocurrió un error en la factura.");
    }
  };

  useEffect(() => {
    if (!orderDetails?.orderId) {
      return navigate("/");
    }

    const currentStatus = payments[orderDetails.orderId];
    switch (currentStatus) {
      case "pending":
        const updated = { ...payments, [orderDetails.orderId]: "confirmed" };
        setPayments(updated);
        sessionStorage.setItem("payments", JSON.stringify(updated));
        break;
      case "confirmed":
        toast.success("¡Pago ya confirmado!");
        break;
      case "failed":
        navigate("/cancel");
        break;
      default:
        navigate("/");
    }
  }, [orderDetails]);

  return (
    <div className="page-wrapper">
      <div className="container-xl">
        <div className="d-flex align-items-center gap-4">
          <Link className="btn btn-outline py-0" to="/pedidos">
            <IconCaretLeft />
            Regresar
          </Link>
          <h1 className="badge bg-body-secondary fs-3 my-3">
            ¡Pago exitoso realizado por "{token?.user?.username}"!
          </h1>
        </div>
        <div className="d-flex flex-column flex-md-row justify-content-center align-items-center gap-5">
          <IconBrandShopee size={70} className="mb-5" />
          <div className="card bg-transparent my-4 border-4  w-auto">
            <div className="card-body">
              <p className="text-secondary">Detalles del pedido</p>

              <h3
                className="card-title overflow-x-auto overflow-y-hidden"
                style={{ maxHeight: "30px" }}
              >
                {dayjs(orderDetails?.date).format(" MMMM D, YYYY. h:mm a") ??
                  "Fecha no disponible"}
              </h3>
              <div
                className="text-secondary overflow-y-auto overflow-x-hidden"
                style={{ maxHeight: "420px" }}
              >
                <p className="text-secondary fs-3 fw-bold mb-2">
                  Lista de productos
                </p>
                <div className="d-flex justify-content-between">
                  <h5 className="fs-4">Producto</h5>
                  <h5 className="fs-4"> Subtotal</h5>
                </div>
                {orderDetails?.details?.map((p) => (
                  <div className="d-flex justify-content-between">
                    <h5 className="fw-normal" style={{ maxWidth: "140px" }}>
                      {p.productName} | {p.quantity}{" "}
                      {p.quantity === 1 ? "unidad" : "unidades"}
                    </h5>
                    <h5 className="fw-bold"> $ {p.subTotal}</h5>
                  </div>
                ))}
              </div>
              <h5 className="fs-2 mt-4">
                Total: $ {orderDetails?.total ?? "Precio no disponible"}
              </h5>
            </div>
            <div className="card-footer">
              <button
                onClick={() => downloadInvoice("Factura.pdf")}
                className="btn btn-outline mx-auto d-flex gap-3"
              >
                Descargar comprobante <IconReceiptFilled />
              </button>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};
