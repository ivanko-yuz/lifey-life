using Autofac;
using Autofac.Extensions.DependencyInjection;
using LifeyLife.Api;
using LifeyLife.Core;
using LifeyLife.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>((hostBuilderContext, containerBuilder) =>
    {
        containerBuilder.RegisterModule<LifeyLifeApiModule>();
        containerBuilder.RegisterModule<LifeyLifeCoreModule>();
        containerBuilder.RegisterModule(new LifeyLifeDataModule(hostBuilderContext.Configuration));
    });

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");

app.Run();