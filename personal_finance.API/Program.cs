using personal_finance.API.Middleware;
using personal_finance.Application.Interfaces;
using personal_finance.Application.UseCases.CancelTransaction;
using personal_finance.Application.UseCases.ConfirmTransaction;
using personal_finance.Application.UseCases.CreateTransaction;
using personal_finance.Infrastructure.Persistence.InMemory;
using personal_finance.Application.Queries.Transactions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Cria UMA instância compartilhada do InMemoryTransactionRepository
builder.Services.AddSingleton<InMemoryTransactionRepository>();

// Usa a MESMA instância para escrita
builder.Services.AddSingleton<ITransactionRepository>(sp =>
    sp.GetRequiredService<InMemoryTransactionRepository>());

// Usa a MESMA instância para leitura
builder.Services.AddSingleton<ITransactionQueryRepository, InMemoryTransactionQueryRepository>();

// Command Handlers
builder.Services.AddScoped<CreateTransactionHandler>();
builder.Services.AddScoped<ConfirmTransactionHandler>();
builder.Services.AddScoped<CancelTransactionHandler>();

// Query Handlers
builder.Services.AddScoped<GetAllTransactionsHandler>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
