using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
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
public class calendarworkController : ControllerBase
{
    private readonly CalendarWorkService _calendarWorkService;
    public calendarworkController(CalendarWorkService calendarWorkService)=> _calendarWorkService = calendarWorkService;

    [HttpGet("{id:length(24)}")]
    public async Task<ActionResult<CalendarWork>> Get(string idStore) => await _calendarWorkService.GetCalendarWorkAsync(idStore);

    [HttpPost]
     public async Task<IActionResult> Post(CalendarWork calendarWork)
    {
        await _calendarWorkService.CreateCalendarWorkAsync(calendarWork);

        return CreatedAtAction(nameof(Get),new{calendarWork.idStore}, calendarWork);
    }
        

}