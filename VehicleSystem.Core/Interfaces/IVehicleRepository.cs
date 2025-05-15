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

    // ��ɾ������
    Task DeleteAsync(int id);

    // �Ӳֿ��ȡ��ҳ����
    Task<(IEnumerable<Vehicle> items, int totalCount)> GetPagedListAsync(int pageNumber, int pageSize, Expression<Func<Vehicle, bool>> predicate);
}