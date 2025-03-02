using LTW_Nhóm8386_FriendlyURL.Models;
using LTW_Nhóm8386_FriendlyURL.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Cấu hình Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Đăng ký dịch vụ
builder.Services.AddScoped<UrlShortenerService>();

// Thêm MVC
builder.Services.AddControllersWithViews();
builder.Services.AddLogging();
builder.Services.AddScoped<UrlShortenerService>();

var app = builder.Build();

// Cấu hình pipeline middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Cấu hình route mặc định
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
