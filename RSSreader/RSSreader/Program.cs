using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RSSreader.Data;
var builder = WebApplication.CreateBuilder(args);

// Add dependency injection for Entity Framework
builder.Services.AddDbContext<RSSreaderContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("RSSreaderContext") ?? throw new InvalidOperationException("Connection string 'RSSreaderContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure database and possibly seed it
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services);
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Feed}/{action=Index}/{id?}");

app.Run();