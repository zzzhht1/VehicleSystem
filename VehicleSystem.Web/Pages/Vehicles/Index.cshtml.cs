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
    public async Task<JsonResult> OnGetAsyncData(int pageNumber = 1, int pageSize = 10)
    {
        Expression<Func<Vehicle, bool>> predicate = v =>
            string.IsNullOrEmpty(SearchTerm) ||
            v.PlateNumber.Contains(SearchTerm);

        var result = await _repository.GetPagedListAsync(pageNumber, pageSize, predicate);
        // 转换数据
        var convertedItems = result.items.Select(v => new
        {
            v.PlateNumber,
            Status = v.Status.ToString(), // 转换为 "在库"、"已出租" 等
            v.IsDeleted
        });
        return new JsonResult(new
        {
            items = convertedItems,  // ✅ 返回转换后的数据
            totalCount = result.totalCount
        });
    }
}