using ChainStoreApi.Models;
using ChainStoreApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace ChainStoreApi.Controllers;

    [ApiController]
    [Route("[controller]")]
    public class StaffController : ControllerBase
    {
        private readonly StaffService _staffService;
        
        private readonly ProfileStaffService _profileStaffServie;

        private readonly StoreService _storeService;

        //  public StaffController(StaffService stafftService)=> _staffService = stafftService;
        public StaffController(StaffService staffService, ProfileStaffService profileStaffServie, StoreService storeService) 
        {
            _staffService = staffService;
            _profileStaffServie = profileStaffServie;
            _storeService = storeService;
        }
        
        [HttpGet()]
        public async Task<List<Staff>> Get() => await _staffService.GetStaffAsync();

        [HttpGet("idstore/{idstore:length(24)}")]
        public async Task<List<Staff>> GetStaffOfStore(string idstore) => await _staffService.GetStaffInStoreAsync(idstore);


        [HttpGet("profileuser/{id:length(24)}")]
        public async Task<ActionResult<Staff>> GetProfileUser(string id)
    {
        var user = await _staffService.GetStaffWithAccountIdAsync(id);
        if (user is null)
            return BadRequest("Lỗi");
        return user;
    }


    [HttpGet("{id:length(24)}")]
        public async Task<ActionResult<ProfileStaff>> Get(string id)
        {
            
            
            var profile = await _profileStaffServie.GetProfileStaffAsync(id);
            if (profile is null)
            return NotFound();

            return profile;
        }
    [Authorize(Roles ="admin")]
    [HttpGet("manager")]
    public async Task<ActionResult<List<ProfileStaff>>> GetStaff()
    {
        var profileStaff = await _profileStaffServie.GetProfileManagerAsync();
        if (profileStaff is null)
        return BadRequest("Không có manager nào!");
        return profileStaff;
    }

    [HttpPost]
    public async Task<IActionResult> Post(ProfileStaff profile)
    {
        await _staffService.CreateStaffAsync(profile.staff);
        var staffnew = CreatedAtAction(nameof(Get), new { id = profile.staff.id }, profile.staff);
        if(profile.account.role!.ToString().Equals("manager")){
            var store = await _storeService.GetStoreAsync(profile.staff.storeId);
            if(store is null)
             return NotFound();
            else
            {   var staff = await  _staffService.GetStaffWithAccountIdAsync(profile.staff.accountId);
                store.manager = staff?.id;
                await _storeService.UpdateStoreAsync(store.id!, store);
            }
        }
        return CreatedAtAction(nameof(Get), new { id = profile.staff.id }, profile.staff);
    }
    

    [HttpPut("{id:length(24)}")]

    public async Task<IActionResult> Update(string id, Staff staffupdate)
    {
        var staff = await _staffService.GetStaffAsync(id);

        if (staff is null)
        {
            return NotFound();
        }

        staff.id = staffupdate.id;

        await _staffService.UpdateStaffAsync(id, staffupdate);

        return NoContent();
    }

    [Authorize(Roles ="admin, manager")]
    [HttpDelete("{id:length(24)}")]
    public async Task<IActionResult> Delete(string id)
    {
        var staff = await _staffService.GetStaffAsync(id);

        if (staff is null)
        {
            return NotFound();
        }
        var store = await _storeService.GetStoreWithIdManager(staff.id);
        if(store != null)
        {
            store.manager = null;
            await _storeService.UpdateStoreAsync(store.id,store);
        }
        await _staffService.RemoveStaffAsync(id!);

        return NoContent();
    }
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery(Name="staff")] string? staff, [FromQuery(Name="store")] string? store, [FromQuery(Name="page")] int? page)
    {
        return Ok(await _staffService.GetSearchStaff(staff,store,page));
    }
    
    }
