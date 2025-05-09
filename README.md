Vehicle System
车辆管理系统

## 任务背景
开发一个B/S架构的车辆管理系统，要求使用C# + ASP.NET Core 6.0框架，集成Entity Framework Core和SQL Server数据库。系统需包含以下核心模块：车辆信息管理、用户权限控制、数据统计报表，并遵循分层架构设计。

## 技术要求
1. **技术栈**
   - 前端：Razor Pages + Bootstrap 5 + Chart.js（用于饼图统计）
   - 后端：ASP.NET Core Web API + EF Core + SQL Server
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
4. **权限集成**
   - 生成ASP.NET Identity的扩展：
     ```csharp
     // Startup.cs配置
     services.AddDefaultIdentity<IdentityUser>()
             .AddRoles<IdentityRole>()
             .AddEntityFrameworkStores<AppDbContext>();

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

## 车辆管理系统需求分析：
1. 基础信息管理
  1.1 车辆信息管理
  1.2 驾驶员信息管理
2. 车辆调度管理
  2.1 车辆调度信息管理
  2.2 车辆调度记录查询
3. 车辆维修管理
  3.1 车辆维修记录管理
  3.2 车辆维修记录查询
4. 车辆保险管理
  4.1 车辆保险信息管理
  4.2 车辆保险记录查询
5. 车辆年检管理
  5.1 车辆年检信息管理
  5.2 车辆年检记录查询
6. 车辆违章管理
  6.1 车辆违章信息管理
  6.2 车辆违章记录查询
7. 报表统计管理
  7.1 车辆使用情况统计
  7.2 车辆维修情况统计
  7.3 车辆保险情况统计
  7.4 车辆年检情况统计
  7.5 车辆违章情况统计
8. 系统设置
  8.1 用户权限管理
  8.2 系统参数设置
  8.3 数据备份与恢复

## 优化建议与补充方向:
1. ​​基础信息管理​​
   ​​缺失项​​：
   车辆档案中可补充 ​​新能源车专属字段​​（如电卡信息、充电记录、平均电耗统计）；
   驾驶员档案建议增加 ​​安全绩效指标​​（如事故率、累计行驶里程）。
   ​​改进建议​​：
   在“车辆信息管理”中增加 ​​车辆状态分类​​（可用/出车/维修/报废）.
2. ​​车辆调度管理​​
   ​​缺失项​​：
   未包含 ​​智能调度算法​​（如基于车辆位置、驾驶员排班的自动匹配）；
   缺少 ​​路线规划功能​​（优化行驶路径，降低油耗/电耗）。
   ​​改进建议​​：
   在“调度信息管理”中增加 ​​自动化调度规则配置​​，支持优先级设置（如紧急任务优先）。
3. ​​维修与保险管理​​
   ​​缺失项​​：
   维修模块可增加​保养自动提醒​​（按里程或时间触发）；
   保险管理建议补充到期自动预警​​（提前30天通知）。
4. ​​年检与违章管理​​
   ​​缺失项​​：
   年检模块可加入 ​​代办流程跟踪​​（如年检资料上传、进度查询）；
   违章处理建议支持 ​​在线缴纳罚款​​（对接交管平台）。
5. ​​报表统计管理​​
   ​​缺失项​​：
   缺少 ​​驾驶行为分析报表​​（如超速、急刹车次数统计）；
   未涉及 ​​成本核算报表​​（如单车运营成本、费用趋势预测）。
6. ​​系统设置​​
   ​​缺失项​​：
   权限管理可细化 ​​角色分级​​（如超级管理员、调度员、驾驶员）；
   数据备份建议支持 ​​自动定时备份​​与异地灾备。
   ​​改进建议​​：
   在“参数设置”中增加 ​​预警规则配置​​（如保养周期、保险到期阈值）。

## 关键新增功能推荐：
1. ​智能化扩展​​
   支持驾驶员扫码（如使用手机APP扫码车辆二维码，自动打开系统并选取对应车辆信息）。
2. ​非功能需求补充​​
   ​扩展性​​：预留API接口。


## 项目完善方案
1. 修改数据库表设计
   通过修改 C# 代码（实体类）然后使用 EF Core Migrations 来更新数据库表结构。
   不建议直接在 SQL Server 数据库里修改表设计，因为这会导致你的代码模型和数据库结构不同步，引发各种问题。
2. 

## 数据库设计
1. 车辆信息表
