using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using TiendaRopaHola.Data.Data;
using TiendaRopaHola.Data.Inicializator;
using TiendaRopaHola.Data.Repositories.IRepository;
using TiendaRopaHola.Data.Repositories.Repository;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TiendaRopaHola API v1",
        Version = "v1",
        Description = "Documentacion TiendaRopaHola",
        Contact = new OpenApiContact
        {
            Name = "TiendaRopaHola",
            Email = "test@test.com",
            Url = new Uri("https://localhost:44334/")
        },
    });
    c.SwaggerDoc("v2", new OpenApiInfo
    {
        Title = "TiendaRopaHola API v2",
        Version = "v2",
        Description = "Documentacion TiendaRopaHola",
        Contact = new OpenApiContact
        {
            Name = "TiendaRopaHola",
            Email = "test@test.com",
            Url = new Uri("https://localhost:44334/")
        },
    });
    c.EnableAnnotations();
});

builder.Services.AddScoped<IUnitWork, UnitWork>();
builder.Services.AddScoped<IDbInicializador, DbInicializador>();

var app = builder.Build();

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

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "TiendaRopa API v1");
    c.SwaggerEndpoint("/swagger/v2/swagger.json", "TiendaRopa API v2");

    c.DefaultModelExpandDepth(2);
    c.DefaultModelRendering(ModelRendering.Model);
    c.DefaultModelsExpandDepth(-1);
    c.DisplayRequestDuration();
    c.DocExpansion(DocExpansion.None);
    c.EnableDeepLinking();
    c.MaxDisplayedTags(5);
    c.ShowExtensions();
    c.ShowCommonExtensions();
    c.EnableValidator();
    c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Head, SubmitMethod.Post, SubmitMethod.Put, SubmitMethod.Patch, SubmitMethod.Delete);
    
    c.DocumentTitle = "TiendaRopa API Documentation";

    c.InjectStylesheet("/css/custom.css"); 
    c.InjectJavascript("/lib/jquery/dist/jquery.min.js");
    c.InjectJavascript("/js/custom.js");
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Producto}/{action=Index}/{id?}");

app.Run();
