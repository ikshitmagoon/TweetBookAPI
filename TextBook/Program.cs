using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TweetBook.Data;
using TweetBook.Installer;
using TweetBook.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.InstallServicesInAssembly(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();

   
}
else
{
    app.UseHsts();
}

var swaggerOptions = new SwaggerOptions();
builder.Configuration.GetSection(nameof(swaggerOptions)).Bind(swaggerOptions);
app.UseSwagger(options =>
{
    options.RouteTemplate = swaggerOptions.JsonRoute;
});
app.UseSwaggerUI(options => {
    options.SwaggerEndpoint(swaggerOptions.UIEndpoint, swaggerOptions.Description);

});
app.UseHttpsRedirection();
app.UseRouting();


app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Test}/{action=api/user}/{id?}")
    .WithStaticAssets();

app.MapRazorPages()
   .WithStaticAssets();

app.Run();
