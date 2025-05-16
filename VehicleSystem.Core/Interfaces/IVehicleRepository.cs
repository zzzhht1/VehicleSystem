using VehicleSystem.Core.Entities;
using System.Linq.Expressions;
using VehicleSystem.Core.Enums;

namespace VehicleSystem.Core.Interfaces;

// �����ִ��ӿڣ����峵�����ݷ��ʷ���
public interface IVehicleRepository
{

    // ����ID��ȡ����
    Task<Vehicle> GetByIdAsync(int id);

    // ��ȡ���г���
    Task<IEnumerable<Vehicle>> GetAllAsync(string searchTerm = null);

    // ���ݶ�̬������ѯ����
    Task<IEnumerable<Vehicle>> FindAsync(Expression<Func<Vehicle, bool>> predicate);

    // ��ӳ���
    Task AddAsync(Vehicle entity);

    // ���³�����Ϣ
    Task UpdateAsync(Vehicle entity);

    // ��ɾ������
    Task<SoftDeleteResult> DeleteAsync(int id);

    // �Ӳֿ��ȡ��ҳ����
    Task<(IEnumerable<Vehicle> items, int totalCount)> GetPagedListAsync(int pageNumber, int pageSize, Expression<Func<Vehicle, bool>> predicate);
}