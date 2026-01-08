using personal_finance.API.Middleware;
using personal_finance.Application.Interfaces;
using personal_finance.Application.UseCases.CancelTransaction;
using personal_finance.Application.UseCases.ConfirmTransaction;
using personal_finance.Application.UseCases.CreateTransaction;
using personal_finance.Infrastructure.Persistence.InMemory;
using personal_finance.Application.Queries.Transactions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

// Swagger com grupos de endpoints
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("Commands", new() { Title = "Personal Finance API - Commands", Version = "v1" });
    c.SwaggerDoc("Queries", new() { Title = "Personal Finance API - Queries", Version = "v1" });

    // Mostra só endpoints do grupo correto em cada doc
    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        // Se não tiver GroupName, cai no default (opcional)
        var groupName = apiDesc.GroupName ?? "Commands";
        return string.Equals(groupName, docName, StringComparison.OrdinalIgnoreCase);
    });
});

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
builder.Services.AddScoped<GetTransactionByIdHandler>();


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/Commands/swagger.json", "Commands");
        c.SwaggerEndpoint("/swagger/Queries/swagger.json", "Queries");
    });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
