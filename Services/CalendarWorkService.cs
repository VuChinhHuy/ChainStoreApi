using ChainStoreApi.Data;
using ChainStoreApi.Models;
using Microsoft.Extensions.Options;
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

        public async Task<CalendarWork> GetCalendarWorkAsync(string idStore) => await _calendarworkCollection.Find(x=>x.idStore == idStore).FirstAsync();
        // create
        public async Task CreateCalendarWorkAsync(CalendarWork calendarWork) => await _calendarworkCollection.InsertOneAsync(calendarWork);

        // update

        // remove

        //  staff check in 

        // staff check out

        // public async Task<TimeWork> GetTimeWorkAsync 
    
    }
