using Microsoft.EntityFrameworkCore;
using LaundryManagement.Infrastructure.Data;
using LaundryManagement.API.Services;
using LaundryManagement.API.Interfaces;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<LaundryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register custom services
builder.Services.AddScoped<IMachineService, MachineService>();
builder.Services.AddScoped<ICycleService, CycleService>();
builder.Services.AddScoped<IIncomeService, IncomeService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();