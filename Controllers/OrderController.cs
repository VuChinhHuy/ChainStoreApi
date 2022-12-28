using ChainStoreApi.EnumExtension;
using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace ChainStoreApi.Controllers;

[ApiController]
[Route("[controller]")]
public class orderController : ControllerBase
{
    private readonly OrderService _orderService;

    
    public orderController(OrderService orderService) { 
        _orderService = orderService;
        
    }

    // lấy ra các trạng thái của hoá đơn bên enum list
    [HttpGet("GetBillStatus")]
    public IActionResult GetBillStatus()
    {
        List<EnumValue> enums = ((BillStatus[])Enum.GetValues(typeof(BillStatus)))
        .Select(x => new EnumValue()
        {
            Value = (int)x,
            Name = x.GetDescription()
        }).ToList();
        return new OkObjectResult(enums);
    }
    // lấy ra các định dạng của phương thức thanh toán từ enum list
    [HttpGet("GetPaymentMethos")]
    public IActionResult GetPaymentMethos()
    {
        List<EnumValue> enums = ((PaymentMethos[])Enum.GetValues(typeof(PaymentMethos)))
        .Select(x => new EnumValue()
        {
            Value = (int)x,
            Name = x.GetDescription()
        }).ToList();
        return new OkObjectResult(enums);
    }
    // tìm kiếm thông tin hoá đơn dựa vào keyword, ngày tạo
    [HttpGet("Keyword")]
    public async Task<ActionResult<Order>> searchkeyword(string? startDate, string? endDate, string? keyword)
    {
        var order = await _orderService.SearchKeyword(startDate, endDate, keyword);
        return Ok(order);
    }

    // lấy ra tất cả hoá đơn
    [HttpGet]
    public async Task<List<Order>> Get() => await _orderService.GetOrderAsync();

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<Order>> Get(string id)
    {
        var order = await _orderService.GetOrderAsync(id);
        if (order is null)
        {
            return NotFound();
        }
        return order;
    }

    [HttpPost]
    public async Task<IActionResult> Createorder(Order order)
    {
                
        await _orderService.CreateOrderAsync(order);
        var result = CreatedAtAction(nameof(Get), new { id = order.id }, order);

        

        return result;
    }

    [HttpPut("{id:length(24)}")]
    public async Task<IActionResult> UpdateOrder(string id, Order orderUpdate)
    {
        var order = await _orderService.GetOrderAsync(id);

        if (order is null)
        {
            return NotFound();
        }

        await _orderService.UpdateOrderAsync(id, orderUpdate);

        return NoContent();
    }

    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> DeleteOrder(string id)
    {
        var order = await _orderService.GetOrderAsync(id);

        if (order is null)
        {
            return NotFound();
        }

        await _orderService.RemoveOrderAsync(id);

        return NoContent();
    }

}