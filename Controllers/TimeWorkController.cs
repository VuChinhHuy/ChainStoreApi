using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
namespace ChainStoreApi.Controllers;


    [ApiController]
    [Route("[controller]")]
    public class timeworkController : ControllerBase
    {
        
        private readonly TimeWorkService _timeWorkService;
        public timeworkController(TimeWorkService timeWorkService)
        {
            _timeWorkService = timeWorkService;
        }
        // Get time work with store 
        [HttpGet]
        public async Task<ActionResult<TimeWork>> Get(string idStore)
        {
            var timeWork = await _timeWorkService.GetTimeWorkWithStoreAsync(idStore);
            return timeWork!;
        }
        // create time work
        [HttpPost]
        public async Task<IActionResult> Post(TimeWork timeWork)
        {
            await _timeWorkService.CreateTimeWorkAsync(timeWork);
            return CreatedAtAction(nameof(Get), new { id = timeWork.id }, timeWork);

        }
        // update time work 
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, TimeWork timeWorkUpdate)
        {
            var timeWork = await _timeWorkService.GetTimeWorkAsync(id);

            if (timeWork is null)
            {
                return NotFound();
            }

            await _timeWorkService.UpdateTimeWorkAsync(id, timeWorkUpdate);

            return CreatedAtAction(nameof(Get), new { id = timeWork.id }, timeWork);
        }
    }