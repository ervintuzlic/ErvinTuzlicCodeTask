using IntusCodeTaskErvinTuzlic.Server;
using IntusCodeTaskErvinTuzlic.Server.Data;
using IntusCodeTaskErvinTuzlic.Server.Filters;
using IntusCodeTaskErvinTuzlic.Shared.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(typeof(ExceptionFilter));
});
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

DependencyInjector.Configure(builder.Services);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
    context!.Database.EnsureCreatedAsync().GetAwaiter().GetResult();
}

if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();


app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
