import { toast } from "react-toastify";
import { FormProvider, useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import { Input } from "../Input";
import { useAuth } from "../../pages/session/AuthProvider";
import { orderSchema, orderValidations } from "../../validations/orderSchema"; // VALIDACIONES CON YUP
import { useApi } from "../../API/apiService";

export const BuyModal = ({
  show,
  closeModal,
  productsCart,
  setProductsCart,
  fetchData,
}) => {
  //Utilizar yup para validar formulario
  const { token } = useAuth();
  const methods = useForm({
    resolver: yupResolver(orderSchema),
    defaultValues: {
      userId: token?.user?.id ?? 0,
      orderStatus: 3,
      address: "",
      details: [],
    },
  });

  const { apiServicePost } = useApi();
  const onSubmit = methods.handleSubmit(async (data) => {
    //Guardar los detalles del pedido
    const details = productsCart.map((product) => ({
      quantity: product.quantity ?? 1,
      price: product.price,
      productId: product.id,
    }));
    //Confirmar que el carrito no este vació
    if (details.length <= 0) {
      toast.error("No hay productos en el carrito.");
      return;
    }

    const payload = {
      userId: data.userId ?? 0,
      orderStatus: data.orderStatus ?? 3,
      address: data.address ?? "",
      details,
    };

    try {
      await apiServicePost("orders", payload, true);
      toast.success("¡Pedido registrado con éxito!");
      sessionStorage.removeItem("productsCart");
      setProductsCart([]);
      closeModal();
      fetchData();
    } catch (err) {
      console.error("Error al guardar el pedido:", err, payload);
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
                <h5 className="modal-title">Agregar Pedido</h5>
                <button
                  type="button"
                  className="btn-close"
                  onClick={closeModal}
                ></button>
              </div>

              <div className="modal-body">
                <div className="row mb-3">
                  <Input {...orderValidations.addressValidation} />
                </div>
              </div>

              <div className="modal-footer">
                <button type="button" className="btn" onClick={closeModal}>
                  Cancelar
                </button>
                <button type="submit" className="btn btn-primary">
                  Guardar
                </button>
              </div>
            </div>
          </form>
        </FormProvider>
      </div>
    </div>
  );
};
