using Microsoft.EntityFrameworkCore;
using TeacherPlanApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// === 1. ЧИТАЕМ ПЕРЕМЕННЫЕ ОКРУЖЕНИЯ (Для Docker) ===
// Если переменных нет (запуск локально), берем значения по умолчанию
var host = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var port = Environment.GetEnvironmentVariable("DB_PORT") ?? "5432";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "TeacherPlanDb";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "123"; 

var connectionString = $"Host={host};Port={port};Database={dbName};Username={dbUser};Password={dbPassword}";

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(connectionString));

// === 2. ДОБАВЛЯЕМ ВСТРОЕННЫЙ HEALTHCHECK ===
builder.Services.AddHealthChecks(); 
// ===========================================

var app = builder.Build();

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

// Включаем маршрут для проверки здоровья (Docker будет дергать эту ссылку)
app.MapHealthChecks("/health");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

// === 3. АВТОМАТИЧЕСКОЕ СОЗДАНИЕ ТАБЛИЦ В DOCKER ===
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate(); // Эта команда сама накатит миграции при запуске!
}
// ==================================================

app.Run();

// Это нужно для того, чтобы интеграционные тесты "видели" класс Program
public partial class Program { }