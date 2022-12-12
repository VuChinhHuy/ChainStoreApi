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
        try
        {

        
        // staff
        Staff staff = JsonSerializer.Deserialize<Staff>(data["staff"]);
        List<CalendarWorkFromClient> timework = JsonSerializer.Deserialize<List<CalendarWorkFromClient>>(data["calendarRegister"]);
        
        var calendar = await _calendarWorkService.GetCalendarWorkAsync(staff.storeId);
        
        if(calendar is null ){
            // create new Calendar 
            CalendarWork calendarWorkNew = new CalendarWork();
            List<YearWork> listYear = new List<YearWork>();
            
            


            foreach(var item in timework.ToList())
            {
                DateTime time = (DateTime)item.time!;
                String day = time.Day.ToString();
                String month = time.Month.ToString();
                String year = time.Year.ToString();  
                if(item == timework.First()){
                List<checkWork> check = new List<checkWork>();

                    foreach(var work in item.timeShift!.ToList()){
                        checkWork checkwork = new checkWork();
                        checkwork.staff = staff;
                        checkwork.timeShift = work;
                        check.Add(checkwork);
                    }
                    List<MonthWork> listMonth = new List<MonthWork>();
                    List<DayWork> listDay = new List<DayWork>();

                    DayWork dayWork = new DayWork();
                    dayWork.check = check;
                    dayWork.day = day;
                    listDay.Add(dayWork);

                    MonthWork monthWork = new MonthWork();
                    monthWork.month = month;
                    monthWork.day = listDay;
                    listMonth.Add(monthWork);

                    YearWork yearWork = new YearWork();
                    yearWork.month = listMonth;
                    yearWork.year = year;
                    listYear.Add(yearWork);
                }
                else
                {
                    if(listYear.Find(x=>x.year == year) != null)
                    {
                        if(listYear.Find(x=>x.year == year)!.month.Find(x=>x.month == month) != null)
                        {
                            if(listYear.Find(x=>x.year == year)!.month.Find(x=>x.month == month)!.day.Find(x=>x.day == day) != null)
                            {
                                List<checkWork> check = new List<checkWork>();
                                foreach(var work in item.timeShift!.ToList()){
                                    checkWork checkwork = new checkWork();
                                    checkwork.staff = staff;
                                    checkwork.timeShift = work;
                                    check.Add(checkwork);
                                }
                                listYear.Find(x=>x.year == year)!.month.Find(x=>x.month == month)!.day.Find(x=>x.day == day)!.check.AddRange(check);

                            }
                            else
                            {
                                List<checkWork> check = new List<checkWork>();
                                foreach(var work in item.timeShift!.ToList()){
                                    checkWork checkwork = new checkWork();
                                    checkwork.staff = staff;
                                    checkwork.timeShift = work;
                                    check.Add(checkwork);
                                }
                                DayWork dayWork = new DayWork();
                                dayWork.check = check;
                                dayWork.day = day;
                                listYear.Find(x=>x.year == year)!.month.Find(x=>x.month == month)!.day.Add(dayWork);

                            }

                        }
                        else
                        {
                            List<checkWork> check = new List<checkWork>();
                            foreach(var work in item.timeShift!.ToList()){
                                checkWork checkwork = new checkWork();
                                checkwork.staff = staff;
                                checkwork.timeShift = work;
                                check.Add(checkwork);
                            }
                            List<MonthWork> listMonth = new List<MonthWork>();
                            List<DayWork> listDay = new List<DayWork>();

                            DayWork dayWork = new DayWork();
                            dayWork.check = check;
                            dayWork.day = day;
                            listDay.Add(dayWork);

                            MonthWork monthWork = new MonthWork();
                            monthWork.month = month;
                            monthWork.day = listDay;
                            listMonth.Add(monthWork);

                            listYear.Find(x=>x.year==year)!.month.AddRange(listMonth);

                        }
                    }
                    else
                    {
                            List<checkWork> check = new List<checkWork>();
                            foreach(var work in item.timeShift!.ToList()){
                                checkWork checkwork = new checkWork();
                                checkwork.staff = staff;
                                checkwork.timeShift = work;
                                check.Add(checkwork);
                            }
                            List<MonthWork> listMonth = new List<MonthWork>();
                            List<DayWork> listDay = new List<DayWork>();

                            DayWork dayWork = new DayWork();
                            dayWork.check = check;
                            dayWork.day = day;
                            listDay.Add(dayWork);

                            MonthWork monthWork = new MonthWork();
                            monthWork.month = month;
                            monthWork.day = listDay;
                            listMonth.Add(monthWork);

                            YearWork yearWork = new YearWork();
                            yearWork.month = listMonth;
                            yearWork.year = year;
                            listYear.Add(yearWork);
                    }
                }
            }

            calendarWorkNew.idStore = staff.storeId;
            calendarWorkNew.year = listYear;
            
            await _calendarWorkService.CreateCalendarWorkAsync(calendarWorkNew);


            Console.WriteLine(calendar!.id);

            return Content("Thành công");
            
        } 
        else
        {
            CalendarWork calendarWorkNew = new CalendarWork();
            List<YearWork> listYear = calendar.year;

            foreach(var item in timework.ToList())
            {
                DateTime time = (DateTime)item.time!;
                String day = time.Day.ToString();
                String month = time.Month.ToString();
                String year = time.Year.ToString();  
                
                    if(listYear.Find(x=>x.year == year) != null)
                    {
                        if(listYear.Find(x=>x.year == year)!.month.Find(x=>x.month == month) != null)
                        {
                            if(listYear.Find(x=>x.year == year)!.month.Find(x=>x.month == month)!.day.Find(x=>x.day == day) != null)
                            {
                                List<checkWork> check = new List<checkWork>();
                                foreach(var work in item.timeShift!.ToList()){
                                    checkWork checkwork = new checkWork();
                                    checkwork.staff = staff;
                                    checkwork.timeShift = work;
                                    check.Add(checkwork);
                                }
                                listYear.Find(x=>x.year == year)!.month.Find(x=>x.month == month)!.day.Find(x=>x.day == day)!.check.AddRange(check);

                            }
                            else
                            {
                                List<checkWork> check = new List<checkWork>();
                                foreach(var work in item.timeShift!.ToList()){
                                    checkWork checkwork = new checkWork();
                                    checkwork.staff = staff;
                                    checkwork.timeShift = work;
                                    check.Add(checkwork);
                                }
                                DayWork dayWork = new DayWork();
                                dayWork.check = check;
                                dayWork.day = day;
                                listYear.Find(x=>x.year == year)!.month.Find(x=>x.month == month)!.day.Add(dayWork);

                            }

                        }
                        else
                        {
                            List<checkWork> check = new List<checkWork>();
                            foreach(var work in item.timeShift!.ToList()){
                                checkWork checkwork = new checkWork();
                                checkwork.staff = staff;
                                checkwork.timeShift = work;
                                check.Add(checkwork);
                            }
                            List<MonthWork> listMonth = new List<MonthWork>();
                            List<DayWork> listDay = new List<DayWork>();

                            DayWork dayWork = new DayWork();
                            dayWork.check = check;
                            dayWork.day = day;
                            listDay.Add(dayWork);

                            MonthWork monthWork = new MonthWork();
                            monthWork.month = month;
                            monthWork.day = listDay;
                            listMonth.Add(monthWork);

                            listYear.Find(x=>x.year==year)!.month.AddRange(listMonth);

                        }
                    }
                    else
                    {
                            List<checkWork> check = new List<checkWork>();
                            foreach(var work in item.timeShift!.ToList()){
                                checkWork checkwork = new checkWork();
                                checkwork.staff = staff;
                                checkwork.timeShift = work;
                                check.Add(checkwork);
                            }
                            List<MonthWork> listMonth = new List<MonthWork>();
                            List<DayWork> listDay = new List<DayWork>();

                            DayWork dayWork = new DayWork();
                            dayWork.check = check;
                            dayWork.day = day;
                            listDay.Add(dayWork);

                            MonthWork monthWork = new MonthWork();
                            monthWork.month = month;
                            monthWork.day = listDay;
                            listMonth.Add(monthWork);

                            YearWork yearWork = new YearWork();
                            yearWork.month = listMonth;
                            yearWork.year = year;
                            listYear.Add(yearWork);
                    }
                }
            

            calendarWorkNew.idStore = staff.storeId;
            calendarWorkNew.year = listYear;
            calendarWorkNew.id = calendar!.id;
            await _calendarWorkService.updateCalendarWorkAsync(calendarWorkNew);



            return Content("Thành công");

        }
        
        }
        catch (Exception)
        {
            return BadRequest("Lỗi");
        }

    }
    
    // GET WORK IN WEEK
    [HttpGet("calanderinweek/{idStore:length(24)}")]
    public async Task<ActionResult> getCalendarInWeek(String idStore)
    {
        
        var calendar = await _calendarWorkService.GetCalendarWorkAsync(idStore);
        var listDayInWeek = DatetimeConvert.getWeekNow();
        foreach(var date in listDayInWeek.ToList())
        {
            String year = date.Year.ToString();
            String month = date.Month.ToString();
            String day = date.Day.ToString();

            

        }

        return BadRequest("Lỗi");
    }

    // CHECK IN

    // CHECK OUT




}