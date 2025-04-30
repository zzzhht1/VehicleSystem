using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VehicleSystem.Core.Entities;
using Microsoft.AspNetCore.Identity;
using VehicleSystem.Infrastructure.Identity; // ApplicationUser所在命名空间

namespace VehicleSystem.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : base(options)
    {
    }

    public DbSet<Vehicle> Vehicles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        // 配置车辆唯一车牌号
        modelBuilder.Entity<Vehicle>(entity => 
        {
            entity.HasIndex(v => v.PlateNumber)
                  .IsUnique();
                  
            // 添加软删除过滤（如果实体有IsDeleted字段）
            entity.HasQueryFilter(v => !v.IsDeleted);
        });
    }
}