public  class DatetimeConvert
{
    // public DateTime formatDate(String timePare)
    // {
        
    // } 
    public String getDay(DateTime datetime)
    {
        return datetime.Day.ToString();        
    }
    public String getMonth(DateTime dateTime)
    {
        return dateTime.Month.ToString();
    }
    public String getYear(DateTime dateTime)
    {
        return dateTime.Year.ToString();
    }

    public static List<DateTime> getWeekNow()
    {
        List<DateTime> list = new List<DateTime>();
        var dayOfWeek = (int)DateTime.Today.DayOfWeek;
        var monday = DateTime.Today.AddDays(-dayOfWeek + (int)DayOfWeek.Monday);
        var dayThisWeek = Enumerable.Range(0,7)
        .Select(d=> monday.AddDays(d)).ToList();
        list = dayThisWeek;
        return list;
    }
}