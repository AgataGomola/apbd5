using System.Collections;
using Tutorial6.Models;

namespace Tutorial6.Repositories;

public interface IWarehouseRepository
{
    public Task<bool> DoesProductExist(int idProduct);
    public Task<bool> DoesWarehouseExist(int id);
    public Task<bool> DoesProductOrderExist(int idProduct, int amount, DateTime createdAt);
    public Task<bool> IsOrderCompleted(int idOrder);
    public Task<int> UpdateFullFilledAt(int idOrder);
    public Task<int> AddProduct(ProductWarehouse productWarehouse);
}