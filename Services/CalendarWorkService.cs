using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;


namespace ChainStoreApi.Services;

    public class CalendarWorkService
    {
        private readonly IMongoCollection<CalendarWork> _calendarworkCollection;
        public CalendarWorkService(
            IOptions<DatabaseSetting> databaseSetting)
        {
            var mongoClient = new MongoClient(
                databaseSetting.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(databaseSetting.Value.DatabaseName);

            _calendarworkCollection = mongoDatabase.GetCollection<CalendarWork>(databaseSetting.Value.CalendarWorkCollectionName);
        }

        
        public async Task<CalendarWork> GetCalendarWorkAsync(string idStore) => await _calendarworkCollection.Find(x=>x.idStore == idStore).FirstOrDefaultAsync();
        // create
        public async Task CreateCalendarWorkAsync(CalendarWork calendarWork) => await _calendarworkCollection.InsertOneAsync(calendarWork);

        // update
        public async Task updateCalendarWorkAsync(CalendarWork calendarWork) => await _calendarworkCollection.ReplaceOneAsync(x => x.id == calendarWork.id, calendarWork);
        
        // public async Task checkin(String idStore, TimeShift timeShift, time time){
        //     DateTime date = DateTime.Now;
        //     String year = date.Year.ToString();
        //     String month = date.Month.ToString();
        //     String day = date.Day.ToString();
        //     var reuslt =  _calendarworkCollection.FindOneAndUpdateAsync(
        //         Builders<CalendarWork>.Filter.Where(x  => x.idStore == idStore),
        //         Builders<CalendarWork>.Update
        //         .Set(rec => rec.year.Find(x=> x.year == year)!.month.Find(x=>x.month == month)!.
        //         day.Find(x=>x.day == day)!.check.Find(x=>x.timeShift == timeShift)!.checkStart, time),
        //         options: new FindOneAndUpdateOptions<CalendarWork> {
        //             ReturnDocument = ReturnDocument.After
        //         }
        //     );
        // }
        
    }
