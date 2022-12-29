using Microsoft.AspNetCore.Mvc;
using ChainStoreApi.Services;
using ChainStoreApi.Models;

namespace ChainStoreApi.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class addressController : ControllerBase
    {
        private readonly AddressService _addressService;
        public addressController(AddressService addresssService) => _addressService = addresssService;

        [HttpGet]
        public async Task<List<Provinces>> Get () => await _addressService.getProvinces();
    }