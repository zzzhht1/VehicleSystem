using VehicleSystem.Core.Entities;
using System.Linq.Expressions;
using VehicleSystem.Core.Enums;

namespace VehicleSystem.Core.Interfaces;

// 车辆仓储接口，定义车辆数据访问方法
public interface IVehicleRepository
{

    // 根据ID获取车辆
    Task<Vehicle> GetByIdAsync(int id);

    // 获取所有车辆
    Task<IEnumerable<Vehicle>> GetAllAsync(string searchTerm = null);

    // 根据动态条件查询车辆
    Task<IEnumerable<Vehicle>> FindAsync(Expression<Func<Vehicle, bool>> predicate);

    // 添加车辆
    Task AddAsync(Vehicle entity);

    // 更新车辆信息
    Task UpdateAsync(Vehicle entity);

    // 软删除车辆
    Task<SoftDeleteResult> DeleteAsync(int id);

    // 从仓库获取分页数据
    Task<(IEnumerable<Vehicle> items, int totalCount)> GetPagedListAsync(int pageNumber, int pageSize, Expression<Func<Vehicle, bool>> predicate);
}