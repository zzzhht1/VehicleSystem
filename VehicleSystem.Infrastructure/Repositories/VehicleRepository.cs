// 引入必要的命名空间
using VehicleSystem.Core.Entities;   // 包含领域实体类（如Vehicle）
using VehicleSystem.Core.Interfaces; // 定义仓储接口（如IVehicleRepository）
using Microsoft.EntityFrameworkCore;  // EF Core核心功能
using System.Linq.Expressions;       // 表达式树用于动态查询条件
using VehicleSystem.Infrastructure.Data; // 数据库上下文
using System.Collections.Generic;    // 集合类支持
using System.Threading.Tasks;
using System.Linq;
using System;        // 异步编程支持

namespace VehicleSystem.Infrastructure.Repositories;

/// <summary>
/// 车辆仓储实现类，负责与数据库进行车辆数据的交互
/// 实现IVehicleRepository接口，遵循仓储模式
/// </summary>
public class VehicleRepository : IVehicleRepository
{
    // 数据库上下文实例，通过依赖注入获取
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// 构造函数，接收数据库上下文依赖
    /// </summary>
    /// <param name="context">已配置的数据库上下文</param>
    public VehicleRepository(ApplicationDbContext context)
    {
        _context = context; // 依赖注入赋值
    }

    /// <summary>
    /// 根据ID获取单个车辆实体（未删除状态）
    /// </summary>
    /// <param name="id">车辆ID</param>
    /// <returns>找到的车辆实体或null</returns>
    public async Task<Vehicle> GetByIdAsync(int id)
    {
        return await _context.Vehicles
            .FirstOrDefaultAsync(v => v.Id == id && !v.IsDeleted); // 组合条件：ID匹配且未删除
    }

    /// <summary>
    /// 获取所有未删除的车辆列表
    /// （使用无跟踪查询优化只读场景性能）
    /// </summary>
    /// <returns>车辆实体集合</returns>
    public async Task<IEnumerable<Vehicle>> GetAllAsync(string searchTerm = null)
    {
        var query = _context.Vehicles
            .Where(v => string.IsNullOrEmpty(searchTerm) || v.PlateNumber.Contains(searchTerm));

        return await query
            .Where(v => !v.IsDeleted)        // 过滤已删除记录
            .AsNoTracking()                 // 禁用变更跟踪，提升查询性能
            .ToListAsync();                 // 异步转换为列表
    }

    /// <summary>
    /// 根据动态条件查询未删除的车辆
    /// </summary>
    /// <param name="predicate">Lambda表达式条件</param>
    /// <returns>符合条件的结果集合</returns>
    public async Task<IEnumerable<Vehicle>> FindAsync(
        Expression<Func<Vehicle, bool>> predicate)
    {
        return await _context.Vehicles
            .Where(v => !v.IsDeleted)       // 先过滤已删除记录
            .Where(predicate)                // 追加传入的查询条件
            .AsNoTracking()                 // 无跟踪查询
            .ToListAsync();
    }

    /// <summary>
    /// 添加新车辆到数据库
    /// （立即保存更改，适用于需要即时持久化的场景）
    /// </summary>
    /// <param name="entity">要添加的车辆实体</param>
    public async Task AddAsync(Vehicle entity)
    {
        await _context.Vehicles.AddAsync(entity); // 异步添加实体到DbSet
        await _context.SaveChangesAsync();        // 立即提交到数据库
    }

    /// <summary>
    /// 更新车辆实体
    /// （通过修改实体状态触发更新，替代显式Update方法）
    /// </summary>
    /// <param name="entity">要更新的车辆实体</param>
    public async Task UpdateAsync(Vehicle entity)
    {
        // 显式标记实体为已修改状态（即使未修改字段也会更新）
        _context.Entry(entity).State = EntityState.Modified; 
        await _context.SaveChangesAsync(); // 提交变更
    }

    /// <summary>
    /// 软删除车辆（标记IsDeleted为true）
    /// </summary>
    /// <param name="id">要删除的车辆ID</param>
    public async Task DeleteAsync(int id)
    {
        var vehicle = await GetByIdAsync(id);  // 先获取实体
        if (vehicle != null)
        {
            vehicle.IsDeleted = true;         // 修改删除标记
            await _context.SaveChangesAsync(); // 提交软删除操作
        }
    }

    /// <summary>
    /// 分页查询方法（包含总数统计）
    /// </summary>
    /// <param name="pageNumber">页码（从1开始）</param>
    /// <param name="pageSize">每页记录数</param>
    /// <param name="predicate">可选查询条件</param>
    /// <returns>
    /// 元组包含：
    /// - 当前页数据集合
    /// - 总记录数（用于前端分页控件）
    /// </returns>
    public async Task<(IEnumerable<Vehicle> items, int totalCount)> 
        GetPagedListAsync(int pageNumber, int pageSize, 
        Expression<Func<Vehicle, bool>> predicate = null)
    {
        // 参数验证前置
        if (pageNumber < 1) throw new ArgumentException("页码不能小于1", nameof(pageNumber));
        if (pageSize < 1 || pageSize > 100) throw new ArgumentException("每页数量需在1-100之间", nameof(pageSize));

        // 基础查询：过滤已删除记录
        var query = _context.Vehicles
            .Where(v => !v.IsDeleted)
            .AsQueryable(); // 转换为可查询对象

        // 追加动态查询条件
        if (predicate != null)
        {
            query = query.Where(predicate); // 组合查询条件
        }

        // 异步获取总记录数（用于分页统计）
        var totalCount = await query.CountAsync();

        // 执行分页查询
        var items = await query
            .OrderBy(v => v.Id) // 必须指定排序规则（EF Core分页强制要求）
            .Skip((pageNumber - 1) * pageSize) // 计算跳过的记录数
            .Take(pageSize)                    // 获取指定页大小的数据
            .AsNoTracking()                   // 无跟踪查询
            .ToListAsync();

        return (items, totalCount); // 返回分页结果元组
    }
}