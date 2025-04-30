using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleSystem.Infrastructure.Data;

namespace VehicleSystem.Web.Controllers;

[Authorize(Roles = "Admin")]
public class VehiclesController : Controller
{
    private readonly ApplicationDbContext _context;

    public VehiclesController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> SoftDelete(int id)
    {
        var vehicle = await _context.Vehicles.FindAsync(id);
        if (vehicle == null) return NotFound();
        
        vehicle.IsDeleted = true;
        await _context.SaveChangesAsync();
        return NoContent();
    }
}