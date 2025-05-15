using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq.Expressions;
using VehicleSystem.Core.Entities;
using VehicleSystem.Core.Interfaces;

namespace VehicleSystem.Web.Pages.Vehicles;

//[Authorize(Roles = "Admin")]
[AllowAnonymous]  // 允许匿名访问
public class IndexModel : PageModel
{
    private readonly IVehicleRepository _repository;

    [BindProperty(SupportsGet = true)]
    public string? SearchTerm { get; set; }

    public int TotalCount { get; set; }

    public IndexModel(IVehicleRepository repository)
    {
        _repository = repository;
    }
    /*
    // 处理页面首次加载
    public async Task OnGetAsync(int pageNumber = 1, int pageSize = 10)
    {
        Expression<Func<Vehicle, bool>> predicate = v =>
            string.IsNullOrEmpty(SearchTerm) ||
            v.PlateNumber.Contains(SearchTerm);

        var result = await _repository.GetPagedListAsync(pageNumber, pageSize, predicate);
        TotalCount = result.totalCount;
    }
    */

    // 处理 AJAX 请求
    // 获取车辆数据
    public async Task<JsonResult> OnGetAsyncData(int pageNumber = 1, int pageSize = 10)
    {
        // 定义筛选条件
        Expression<Func<Vehicle, bool>> predicate = v =>
            string.IsNullOrEmpty(SearchTerm) ||
            v.PlateNumber.Contains(SearchTerm) || v.Brand.Contains(SearchTerm) || 
            v.Type .Contains(SearchTerm) || v.Color.Contains(SearchTerm) ||
            v.FuelType.Contains(SearchTerm);

        // 从仓库获取分页数据
        var result = await _repository.GetPagedListAsync(pageNumber, pageSize, predicate);

        // 转换数据
        var convertedItems = result.items.Select(v => new
        {
            v.Id,
            v.PlateNumber,
            v.Type,
            v.Brand,
            v.Color,
            v.FuelType,
            v.SeatCapacity,
            v.Mileage,
            Statuss = v.Status.ToString(), // 转换为 "在库"、"已出租" 等
            v.OwnerId,
            v.IsDeleted
        });

        // ✅ 返回转换后的数据
        return new JsonResult(new
        {
            items = convertedItems,
            totalCount = result.totalCount
        });
    }

    // 软删除车辆
    public async Task<JsonResult> OnGetDeleteVehicleAsync(int id)
    {
        if (id <= 0)
        {
            return new JsonResult(new { success = false, message = "无效的车辆ID。" });
        }

        try
        {
            var vehicleToMarkAsDeleted = await _repository.GetByIdAsync(id);

            if (vehicleToMarkAsDeleted == null)
            {
                return new JsonResult(new { success = false, message = "未找到要删除的车辆。" });
            }

            if (vehicleToMarkAsDeleted.IsDeleted)
            {
                return new JsonResult(new { success = true, message = "未更改，车辆已经被标记为删除。" });
            }

            vehicleToMarkAsDeleted.IsDeleted = true; // 执行软删除
            await _repository.UpdateAsync(vehicleToMarkAsDeleted); // 保存更改

            return new JsonResult(new { success = true, message = "车辆成功标记为删除。" });
        }

        // 捕获异常
        catch (Exception ex)
        {
            // 控制台日志
            Console.WriteLine($"Error deleting vehicle {id}: {ex.Message}");
            return new JsonResult(new { success = false, message = "删除车辆时发生错误。" });
        }
    }
}