using Synonims.DataLayer.SynonimRepositories;
using Synonims.Services.Synonims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// IoC
builder.Services.AddSingleton<ISynonimRepository, SynonimMemoryRepository>();
builder.Services.AddScoped<ISynonimService, SynonimService>();

var app = builder.Build();




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
