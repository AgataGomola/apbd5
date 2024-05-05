using System.Data.SqlClient;
using Tutorial6.Models;

namespace Tutorial6.Repositories;

public class WarehouseRepository : IWarehouseRepository
{
    private IConfiguration _configuration;

    public WarehouseRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> DoesProductExist(int idProduct)
    {
        var query = "SELECT 1 FROM PRODUCT WHERE IdProduct = @idProduct";
        await using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var command = new SqlCommand();
        
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@idProduct", idProduct);
        
        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return result is not null;
    }

    public async Task<bool> DoesWarehouseExist(int id)
    {
        var query = "SELECT 1 FROM WAREHOUSE WHERE IdWarehouse = @id";
        await using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", id);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return result is not null;
    }

    public async Task<bool> DoesProductOrderExist(int idProduct, int amount, DateTime orderTime)
    {
        var query = "SELECT CreatedAt FROM [ORDER] WHERE IdProduct = @id AND Amount >= @amount";
        await using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", idProduct);
        command.Parameters.AddWithValue("@amount", amount);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return result is not null && (DateTime) result < orderTime;
    }

    public async Task<bool> IsOrderCompleted(int idOrder)
    {
        var query = "SELECT 1 FROM Product_Warehouse WHERE IdOrder = @id";
        await using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", idOrder);
        await connection.OpenAsync();

        var result = await command.ExecuteScalarAsync();
        return result is not null;
    }

    public async Task<int> UpdateFullFilledAt(int idOrder)
    {
        var quer = "UPDATE [ORDER] SET FulfilledAt = @date WHERE IdOrder = @id";
        await using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var command = new SqlCommand();

        command.Connection = connection;
        command.CommandText = quer;
        command.Parameters.AddWithValue("@date", DateTime.Now);
        command.Parameters.AddWithValue("@id", idOrder);

        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }

    public async Task<int> AddProduct(ProductWarehouse productWarehouse)
    {
        var query =
            "INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) " +
            "OUTPUT INSERTED.IdProductWarehouse " +
            "VALUES (@IdWarehouse, @IdProduct, @IdOrder, @Amount, @Price, @CreatedAt)";
    
        await using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@IdWarehouse", productWarehouse.IdWarehouse);
        command.Parameters.AddWithValue("@IdProduct", productWarehouse.IdProduct);
        command.Parameters.AddWithValue("@IdOrder", await getOrderId(productWarehouse.IdProduct));
        command.Parameters.AddWithValue("@Amount", productWarehouse.Amount);
        command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
        command.Parameters.AddWithValue("@Price", await GetProductPrice(productWarehouse.IdProduct) * productWarehouse.Amount);
    
        await connection.OpenAsync();
        var insertedId = await command.ExecuteScalarAsync();
        return Convert.ToInt32(insertedId);
    }


    public async Task<double> GetProductPrice(int IdProduct)
    {
        var query = "SELECT Price FROM Product_Warehouse WHERE IdOrder = @id";
        await using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        await using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", IdProduct);
        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return Convert.ToDouble(result);
    }

    public async Task<int> getOrderId(int idProduct)
    {
        var query = "SELECT IdOrder FROM [ORDER] WHERE IdProduct = @id";
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DEfault"));
        await using var command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = query;
        command.Parameters.AddWithValue("@id", idProduct);
        await connection.OpenAsync();
        var result = await command.ExecuteScalarAsync();
        return Convert.ToInt32(result);
    }
}