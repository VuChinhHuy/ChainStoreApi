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
using System.Text.Json;

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

    [HttpPost("/registercalendar")]
    // REGISTER CALENDAR
    public async Task<ActionResult> registerCalendarWork([FromBody] Dictionary<String,dynamic> data){
        // staff
        
        // Staff staff = data["staff"].ToObject<Staff>();
        Staff staff = JsonSerializer.Deserialize<Staff>(data["staff"]);
        var timework = JsonSerializer.Deserialize<List<dynamic>>(data["calendarRegister"]);
        
        var calendar = await _calendarWorkService.GetCalendarWorkAsync(staff.storeId);
        
        if(calendar is null ){
            // create new Calendar 
            CalendarWork calendarWorkNew = new CalendarWork();
            List<YearWork> listYear = new List<YearWork>();
            List<MonthWork> listMonth = new List<MonthWork>();
            List<DayWork> listDay = new List<DayWork>();
            for (int i = 0; i < timework.Count; i++ )
            {
                var timeRegister = JsonSerializer.Deserialize<dynamic>(timework[i]);
                Console.Write(timeRegister);
                // String time = JsonSerializer.Deserialize<string>(timeRegister["time"]);
                List<TimeShift> timeShift = JsonSerializer.Deserialize<List<TimeShift>>(timeRegister["timeShift"]);
                
                // DateTime time = DateTime.Parse( JsonSerializer.Deserialize<string>(timework[i]["time"]));
                // String day = time.Day.ToString();
                // String month = time.Month.ToString();
                // String year = time.Year.ToString();


            }
            // foreach(var item in timework ){
            //     DateTime time = DateTime.Parse(item["time"]);
            //     String day = time.Day.ToString();
            //     String month = time.Month.ToString();
            //     String year = time.Year.ToString();
            //     List<checkWork> check = new List<checkWork>();
                
            //     foreach(var work in item["timeShift"]){
            //         checkWork checkwork = new checkWork();
            //         checkwork.staff = staff;
            //         checkwork.timeShift = work;
            //         check.Add(checkwork);
            //     }
            //     if(listDay.IsNullOrEmpty()){
            //         DayWork dayWork = new DayWork();
            //         dayWork.check = check;
            //         dayWork.day = day;
            //         listDay.Add(dayWork);
            //     }
            //     // else
            //     // {
            //     //     var searchDay = listDay.Find(x => x.day == day);
            //     //     if( searchDay != null)
            //     //     {
            //     //         // searchDay.check.Add();
            //     //     }
            //     // }
            //     if(listMonth.IsNullOrEmpty()){
            //         MonthWork monthWork = new MonthWork();
            //         monthWork.month = month;
            //         monthWork.day = listDay;
            //         listMonth.Add(monthWork);
            //     }
            //     // else
            //     // {

            //     // }
                
            //     if(listYear.IsNullOrEmpty()){
            //         YearWork yearWork = new YearWork();
            //         yearWork.month = listMonth;
            //         yearWork.year = year;
            //         listYear.Add(yearWork);
            //     }
                
                
            // }
            // calendarWorkNew.idStore = staff.storeId;
            // calendarWorkNew.year = listYear;

            // await _calendarWorkService.CreateCalendarWorkAsync(calendarWorkNew);
            // return CreatedAtAction(nameof(Get),new{calendarWorkNew.idStore}, calendarWorkNew);
            
        } 
        else
        {

        }
        


        return BadRequest("Lỗi");
    }
    
    // GET WORK IN WEEK

    // CHECK IN

    // CHECK OUT




}