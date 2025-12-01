using Shop.Domain.Entities.ShoppingEntities;

namespace Shop.Application.Interfaces.Repositories;

public interface IDetailsRepository : IBaseRepository<OrderDetail>
{
    //Obtener Detalles por Pedido
    Task<IEnumerable<OrderDetail>> GetDetailsByOrderAsync(int orderId);
    //Obtener un Detalle por Pedido y Producto
    Task<OrderDetail> GetDetail(int orderId, int productId);
    //Agregar Producto a los Detalles
    Task<OrderDetail> AddProductToDetail(int orderId, int productId, int amount, float price);
    //Actualizar Detalles de la Orden
    Task<OrderDetail> UpdateOrderDetail(int orderId, int productId, int amount, float price);
    //Eliminar Detalle de un Producto
    Task<bool> DeleteProductFromDetail(int orderId, int productId);
    //Eliminar Todos Detalles de un Pedido
    Task<bool> DeleteAllDetailsAsync(int orderId);
    Task<float> CalculateOrderTotal(int orderId);
    Task<IEnumerable<OrderDetail>> GetOrdersWithProducts(int productId);
}