using personal_finance.Application.Interfaces;
using personal_finance.Application.UseCases.CancelTransaction;
using personal_finance.Application.UseCases.ConfirmTransaction;
using personal_finance.Application.UseCases.CreateTransaction;
using personal_finance.Infrastructure.Persistence.InMemory;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Repository (InMemory)
builder.Services.AddSingleton<ITransactionRepository, InMemoryTransactionRepository>();

// Use cases (handlers)
builder.Services.AddScoped<CreateTransactionHandler>();
builder.Services.AddScoped<ConfirmTransactionHandler>();
builder.Services.AddScoped<CancelTransactionHandler>();

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
