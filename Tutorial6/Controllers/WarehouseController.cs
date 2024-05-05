using Microsoft.AspNetCore.Mvc;
using Tutorial6.Models;
using Tutorial6.Repositories;

namespace Tutorial6.Controllers;

[ApiController]
[Route ("api/[controller]")]
public class WarehouseController : ControllerBase
{
    private IWarehouseRepository _warehouseRepository;

    public WarehouseController(IWarehouseRepository warehouseRepository)
    {
        _warehouseRepository = warehouseRepository;
    }

    [HttpPost]
    public async Task<IActionResult> Add(ProductWarehouse productWarehouse)
    {
        if (!await _warehouseRepository.DoesProductExist(productWarehouse.IdProduct))
            return NotFound($"Product with id: {productWarehouse.IdProduct} does not exist.");

        if (!await _warehouseRepository.DoesWarehouseExist(productWarehouse.IdProduct))
            return NotFound($"Warehouse with id {productWarehouse.IdWarehouse} does not exist");
        
        if (productWarehouse.Amount <= 0)
            return BadRequest($"Invalid amount. Amount must be greater than 0");
        if (!await _warehouseRepository.DoesProductOrderExist(productWarehouse.IdProduct, productWarehouse.Amount,
                productWarehouse.CreatedAt))
            return NotFound($"Order for product with id: {productWarehouse.IdProduct} does not exist for amount: {productWarehouse.Amount}");
        if (await _warehouseRepository.IsOrderCompleted(productWarehouse.IdOrder))
            return BadRequest($"Order with id: {productWarehouse.IdOrder} already exists");
        
        await _warehouseRepository.UpdateFullFilledAt(productWarehouse.IdOrder);
        var result = await _warehouseRepository.AddProduct(productWarehouse);
        return Ok(result);
    }
}