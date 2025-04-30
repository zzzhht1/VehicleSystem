using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VehicleSystem.Core.Interfaces;

namespace VehicleSystem.Web.Pages.Vehicles;

[Authorize(Roles = "Admin")]
public class IndexModel : PageModel
{
    private readonly IVehicleRepository _repository;

    public IndexModel(IVehicleRepository repository)
    {
        _repository = repository;
    }

    public async Task<IActionResult> OnGetAsync(int pageNumber = 1, int pageSize = 10, string search = "")
    {
        Expression<Func<Vehicle, bool>> predicate = v => 
            string.IsNullOrEmpty(search) || 
            v.LicensePlate.Contains(search) ||
            v.VIN.Contains(search);

        var result = await _repository.GetPagedListAsync(pageNumber, pageSize, predicate);
        return new JsonResult(new { result.items, result.totalCount });
    }
}