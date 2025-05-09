using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using VehicleSystem.Core.Entities;
using VehicleSystem.Core.Interfaces;
using VehicleSystem.Infrastructure.Data;
using VehicleSystem.Infrastructure.Identity;
using VehicleSystem.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 数据库配置（最低必需）
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("VehicleSystemDb")));
/*
// 身份认证（基础版）
//builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>()
//.AddEntityFrameworkStores<ApplicationDbContext>();

// Program.cs 添加以下配置
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/"; // 当要求登录时默认跳转首页
    options.AccessDeniedPath = "/Error"; // 权限不足时的跳转地址
});
*/
builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();

// Razor Pages（必需）
builder.Services.AddRazorPages();

var app = builder.Build();

// 中间件管道（必需顺序）
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
//app.UseAuthentication();
//app.UseAuthorization();
app.MapRazorPages();

// 自动迁移（同步简化版）
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate(); // 同步迁移
}

app.Run();
