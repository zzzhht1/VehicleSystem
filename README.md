Vehicle System
车辆管理系统
## 任务背景
开发一个B/S架构的车辆管理系统，要求使用C# + ASP.NET Core 6.0框架，集成Entity Framework Core和SQL Server数据库。系统需包含以下核心模块：车辆信息管理、用户权限控制、数据统计报表，并遵循分层架构设计。

## 技术要求
1. **技术栈**
   - 前端：Razor Pages + Bootstrap 5 + Chart.js（用于饼图统计）
   - 后端：ASP.NET Core Web API + EF Core + SQL Server LocalDB
   - 安全：ASP.NET Core Identity实现角色权限（Admin/User）
   - 部署：IIS服务器部署，支持JWT令牌认证[6,9](@ref)

2. **功能需求**
   - **车辆管理模块**：
     - CRUD操作（逻辑删除），支持车牌号唯一性校验[6](@ref)
     - 搜索/分页功能（AJAX异步加载）
     - 字段：车牌号（必填）、品牌、型号、颜色、购买日期、状态（可用/维修/报废）[1](@ref)
   - **统计模块**：
     - 按状态统计车辆数量的饼图（Chart.js实现）[1](@ref)
   - **用户权限**：
     - Admin可管理所有数据，User仅查看[6,9](@ref)

## 代码生成要求
1. **项目结构生成**
   - 使用`dotnet new`创建解决方案：
     ```bash
     dotnet new sln -n VehicleSystem
     dotnet new webapp -n VehicleSystem.Web --no-https
     dotnet new classlib -n VehicleSystem.Core
     dotnet new classlib -n VehicleSystem.Infrastructure
     ```
   - 按DDD分层架构组织项目[9](@ref)

2. **数据库建模**
   - 生成以下EF Core实体及DbContext：
     ```csharp
     // Vehicle.cs
     public class Vehicle {
         public int Id { get; set; }
         [Required, MaxLength(20)] 
         public string PlateNumber { get; set; }
         public string Brand { get; set; }
         public DateTime PurchaseDate { get; set; }
         public VehicleStatus Status { get; set; }
     }
     
     // AppDbContext配置
     modelBuilder.Entity<Vehicle>().HasQueryFilter(v => !v.IsDeleted); // 逻辑删除
     ```

3. **API与页面生成**
   - 使用Razor Pages生成车辆列表页（`Pages/Vehicles/Index.cshtml`），包含：
     - 分页控件（每页10条）
     - 搜索框（车牌号模糊查询）
     - 使用Tag Helpers实现表单验证[1](@ref)
   - 生成RESTful API控制器（`VehiclesController.cs`），包含：
     ```csharp
     [Authorize(Roles = "Admin")]
     [HttpDelete("{id}")]
     public async Task<IActionResult> SoftDelete(int id) {
         var vehicle = await _context.Vehicles.FindAsync(id);
         vehicle.IsDeleted = true; // 逻辑删除标记
         await _context.SaveChangesAsync();
         return NoContent();
     }
     ```

4. **权限集成**
   - 生成ASP.NET Identity的扩展：
     ```csharp
     // Startup.cs配置
     services.AddDefaultIdentity<IdentityUser>()
             .AddRoles<IdentityRole>()
             .AddEntityFrameworkStores<AppDbContext>();
     ```

## 优化与扩展指令
1. **性能优化**
   - 为EF Core查询添加`AsNoTracking()`[1](@ref)
   - 使用缓存机制存储常用数据（如车辆状态枚举）

2. **错误处理**
   - 生成全局异常过滤器（`GlobalExceptionFilter.cs`）记录日志[6](@ref)
   - 返回标准化的API错误响应格式

3. **扩展建议**
   - 后续可集成实时监控功能（如GPS追踪，参考网页5的AI数据分析方案）
   - 添加维修记录模块（关联车辆ID）[1](@ref)

## 交互策略
1. **分阶段生成**：
   - 首轮生成项目骨架和核心实体
   - 第二轮补充业务逻辑和服务层
   - 第三轮实现前端交互和权限控制

2. **错误修复指令**：
   - 当出现编译错误时，使用：
     ```markdown
     [问题描述] 运行`dotnet build`时出现CS0246未找到类型错误
     [已尝试] 检查命名空间引用
     [需求] 请修正缺失的using语句或NuGet包引用
     ```