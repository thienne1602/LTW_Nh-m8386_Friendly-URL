using LTW_Nhóm8386_FriendlyURL.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Thêm dịch vụ Controller và Views
builder.Services.AddControllersWithViews();

// Cấu hình kết nối đến SQL Server (đã thêm TrustServerCertificate nếu cần)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Cấu hình middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// Cấu hình routing mặc định: Controller=URLs, Action=Index
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=URLs}/{action=Index}/{id?}");

app.Run();
