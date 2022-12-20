using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
    public async Task<ActionResult<Object>> getCalendarInWeek(String idStore)
    {
        try
        {

        List<dynamic > result = new List<dynamic>();

        var calendar = await _calendarWorkService.GetCalendarWorkAsync(idStore);
        
        var listDayInWeek = DatetimeConvert.getWeekNow();
        foreach(var date in listDayInWeek.ToList())
        {
            String year = date.Year.ToString();
            String month = date.Month.ToString();
            String day = date.Day.ToString();

            if(calendar.year.Find(x=>x.year == year) != null)
            {
                if(calendar.year.Find(x=>x.year == year)!.month.Find(x=>x.month== month)!= null)
                {
                    if(calendar.year.Find(x=>x.year == year)!.month.Find(x=>x.month== month)!.day.Find(x=>x.day == day) != null)
                    {
                        var item = calendar.year.Find(x=>x.year == year)!.month.Find(x=>x.month== month)!.day.Find(x=>x.day == day)!.check;
                        Dictionary<string, dynamic> itemResult = new Dictionary<string, dynamic>{["time"] = date.ToString("yyyy-MM-dd'T'HH:mm:ss.000"), ["timeshift"] = item};
                        result.Add(itemResult);
                    }
                    else
                    {
                        Dictionary<string, dynamic> itemResult = new Dictionary<string, dynamic>{["time"] = date.ToString("yyyy-MM-dd'T'HH:mm:ss.000"), ["timeshift"] = ""};
                        result.Add(itemResult);
                    }
                }
                else
                {
                    Dictionary<string, dynamic> itemResult = new Dictionary<string, dynamic>{["time"] = date.ToString("yyyy-MM-dd'T'HH:mm:ss.000"), ["timeshift"] = ""};
                    result.Add(itemResult);
                }
            }
            else{
                Dictionary<string, dynamic> itemResult = new Dictionary<string, dynamic>{["time"] = date.ToString("yyyy-MM-dd'T'HH:mm:ss.000"), ["timeshift"] = ""};
                result.Add(itemResult);
            }
        }
        if (result.IsNullOrEmpty())

            return BadRequest("Không có lịch");
        return result.ToList();
        }
        catch (Exception)
        {
            return BadRequest("Lỗi");
        }
    }
    // GET WORK IN WEEK OF STAFF
    [HttpGet("calendarinweek/staff")]
    public async Task<ActionResult<Object>> getCalendarInWeek([FromQuery] String storeId, String idStaff)
    {
        try
        {

        List<dynamic > result = new List<dynamic>();

        var calendar = await _calendarWorkService.GetCalendarWorkAsync(storeId);
        
        var listDayInWeek = DatetimeConvert.getWeekNow();
        foreach(var date in listDayInWeek.ToList())
        {
            String year = date.Year.ToString();
            String month = date.Month.ToString();
            String day = date.Day.ToString();

            if(calendar.year.Find(x=>x.year == year) != null)
            {
                if(calendar.year.Find(x=>x.year == year)!.month.Find(x=>x.month== month)!= null)
                {
                    if(calendar.year.Find(x=>x.year == year)!.month.Find(x=>x.month== month)!.day.Find(x=>x.day == day) != null)
                    {
                        
                        var item = calendar.year.Find(x=>x.year == year)!.month.Find(x=>x.month== month)!.day.Find(x=>x.day == day)!.check;
                        foreach (var check in item.ToList())
                        {
                            List<dynamic> checkItem = new List<dynamic>();
                            if(check.staff.id == idStaff)
                            {   
                                checkItem.Add(check);
                                
                            }
                            Dictionary<string, dynamic> itemResult = new Dictionary<string, dynamic>{["time"] = date.ToString("yyyy-MM-dd'T'HH:mm:ss.000"), ["timeshift"] = checkItem};
                            result.Add(itemResult);
                        }
                    }
                    else
                    {
                        Dictionary<string, dynamic> itemResult = new Dictionary<string, dynamic>{["time"] = date.ToString("yyyy-MM-dd'T'HH:mm:ss.000"), ["timeshift"] = ""};
                        result.Add(itemResult);
                    }
                }
                else
                {
                    Dictionary<string, dynamic> itemResult = new Dictionary<string, dynamic>{["time"] = date.ToString("yyyy-MM-dd'T'HH:mm:ss.000"), ["timeshift"] = ""};
                    result.Add(itemResult);
                }
            }
            else{
                Dictionary<string, dynamic> itemResult = new Dictionary<string, dynamic>{["time"] = date.ToString("yyyy-MM-dd'T'HH:mm:ss.000"), ["timeshift"] = ""};
                result.Add(itemResult);
            }
        }
        if (result.IsNullOrEmpty())

            return BadRequest("Không có lịch");
        return result.ToList();
        }
        catch (Exception)
        {
            return BadRequest("Lỗi");
        } 
    }
    // CHECK IN
    [HttpPost("/checkin")]
    public async Task<ActionResult> checkin([FromBody] Dictionary<String,dynamic> data)
    {
        try
        {
            DateTime now = DateTime.Now;
            String year = now.Year.ToString();
            String month = now.Month.ToString();
            String day = now.Day.ToString();
            Staff staff =  JsonSerializer.Deserialize<Staff>(data["staff"]);
            time time =  JsonSerializer.Deserialize<time>(data["timecheck"]);
            TimeShift timeShift =  JsonSerializer.Deserialize<TimeShift>(data["timeShift"]);
            var calendar = await _calendarWorkService.GetCalendarWorkAsync(staff.storeId);

            if(calendar.year.Find(x=>x.year == year) != null)
            {
                if(calendar.year.Find(x=>x.year == year)!.month.Find(x=>x.month== month)!= null)
                {
                    if(calendar.year.Find(x=>x.year == year)!.month.Find(x=>x.month== month)!.day.Find(x=>x.day == day) != null)
                    {
                        
                        if(calendar.year.Find(x=>x.year == year)!.month.Find(x=>x.month== month)!.day.Find(x=>x.day == day)!.check.Find(x=>x.staff.id == staff.id && x.timeShift!.name == timeShift.name) != null)
                        {
                            if(calendar.year.Find(x=>x.year == year)!.month.Find(x=>x.month== month)!.day.Find(x=>x.day == day)!.check.Find(x=>x.staff.id == staff.id && x.timeShift!.name == timeShift.name)!.checkStart == null) 
                            {
                                calendar.year.Find(x=>x.year == year)!.month.Find(x=>x.month== month)!.day.Find(x=>x.day == day)!.check.Find(x=>x.staff.id == staff.id && x.timeShift!.name == timeShift.name)!.checkStart = time;
                                await _calendarWorkService.updateCalendarWorkAsync(calendar);
                                return Content("OK");
                            }
                            else
                            {
                                return Content("Đã CHECK IN ở ca" + timeShift.name + " rồi!");

                            }
                            
                        }  
                        return Content("Không có ca" + timeShift.name + " làm giờ này!");   

                    }
                    else
                    {
                        return Content("Không có lịch làm!");

                    }
                }
                else
                {
                    return Content("Không có lịch làm!");

                }
            }
            else
            {
                return Content("Không có lịch làm!");

            }

        }
        catch (Exception)
        {
            return BadRequest("Lỗi!");
        }
    }

    // CHECK OUT
    [HttpPost("/checkout")]
    public async Task<ActionResult> checkout([FromBody] Dictionary<String,dynamic> data)
    {
        try
        {
            DateTime now = DateTime.Now;
            String year = now.Year.ToString();
            String month = now.Month.ToString();
            String day = now.Day.ToString();
            Staff staff =  JsonSerializer.Deserialize<Staff>(data["staff"]);
            time time =  JsonSerializer.Deserialize<time>(data["timecheck"]);
            TimeShift timeShift =  JsonSerializer.Deserialize<TimeShift>(data["timeShift"]);
            var calendar = await _calendarWorkService.GetCalendarWorkAsync(staff.storeId);

            if(calendar.year.Find(x=>x.year == year) != null)
            {
                if(calendar.year.Find(x=>x.year == year)!.month.Find(x=>x.month== month)!= null)
                {
                    if(calendar.year.Find(x=>x.year == year)!.month.Find(x=>x.month== month)!.day.Find(x=>x.day == day) != null)
                    {
                        if(calendar.year.Find(x=>x.year == year)!.month.Find(x=>x.month== month)!.day.Find(x=>x.day == day)!.check.Find(x=>x.staff.id == staff.id) != null)   
                        calendar.year.Find(x=>x.year == year)!.month.Find(x=>x.month== month)!.day.Find(x=>x.day == day)!.check.Find(x=>x.staff.id == staff.id && x.timeShift!.name == timeShift.name)!.checkEnd = time;
                            await _calendarWorkService.updateCalendarWorkAsync(calendar);
                        return Content("OK");
                    }
                    else
                    {
                        return Content("Không có lịch làm!");

                    }
                }
                else
                {
                    return Content("Không có lịch làm!");

                }
            }
            else
            {
                return Content("Không có lịch làm!");

            }

        }
        catch (Exception)
        {
            return BadRequest("Lỗi!");
        }
    }


}