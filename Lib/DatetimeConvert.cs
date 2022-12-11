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
}