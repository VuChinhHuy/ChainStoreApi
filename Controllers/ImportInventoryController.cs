using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using ChainStoreApi.Handler;
using System.Text;


namespace ChainStoreApi.Controllers;
[ApiController]
[Route("[controller]")]
public class ImportInventoryController : ControllerBase
{
    private readonly ImportInventoryService _importInventoryService;

    public ImportInventoryController(ImportInventoryService importInventoryService) => _importInventoryService = importInventoryService;

    [HttpGet("{idStore:length(24)}")]
    public async Task<List<ImportInventory>> GetWithStore(string idStore)
    {
        return await _importInventoryService.getImportInventoryStore(idStore);
    }

    [HttpPost]
    public async Task Post([FromBody] ImportInventory importInventory)
    {
        await _importInventoryService.createImportInventory(importInventory);
        return;
        
    }
    [HttpGet("inventory/{idStore:length(24)}")]
    public async Task<InventoryManager?> GetProductInStore(string idStore)
    {
        return await _importInventoryService!.getProductInStore(idStore);
    }

}