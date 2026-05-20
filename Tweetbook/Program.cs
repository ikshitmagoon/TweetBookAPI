using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TweetBook.Data;
using TweetBook.Installer;
using TweetBook.Options;

var builder = WebApplication.CreateBuilder(args);

// 1. Add services to the container (Configuration must happen BEFORE Build)
builder.Services.InstallServicesInAssembly(builder.Configuration);

// Bind swagger options early so they are accessible to the pipeline building phase
var swaggerOptions = new SwaggerOptions();
builder.Configuration.GetSection(nameof(SwaggerOptions)).Bind(swaggerOptions);

var app = builder.Build();

// 2. Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseHsts();
}

// 3. Database Auto-Migrations Strategy
using (var serviceScope = app.Services.CreateScope())
{
    var dbContext = serviceScope.ServiceProvider.GetRequiredService<DataContext>();
    await dbContext.Database.MigrateAsync();
}

// 4. Swagger Routing Activation Middlewares
app.UseSwagger(options =>
{
    options.RouteTemplate = swaggerOptions.JsonRoute;
});

app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);
});

// 5. Standard Endpoint Routing
app.UseHttpsRedirection();
app.UseRouting();

app.MapStaticAssets();

app.UseAuthentication(); // Note: Authentication must ALWAYS come before Authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Test}/{action=api/user}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

await app.RunAsync();
