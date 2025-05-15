using VehicleSystem.Core.Entities;
using System.Linq.Expressions;

namespace VehicleSystem.Core.Interfaces;

public interface IVehicleRepository
{
    Task<Vehicle> GetByIdAsync(int id);
    Task<IEnumerable<Vehicle>> GetAllAsync(string searchTerm = null);
    Task<IEnumerable<Vehicle>> FindAsync(Expression<Func<Vehicle, bool>> predicate);
    Task AddAsync(Vehicle entity);
    Task UpdateAsync(Vehicle entity);

    // 软删除车辆
    Task DeleteAsync(int id);

    // 从仓库获取分页数据
    Task<(IEnumerable<Vehicle> items, int totalCount)> GetPagedListAsync(int pageNumber, int pageSize, Expression<Func<Vehicle, bool>> predicate);
}