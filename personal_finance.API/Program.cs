using Microsoft.EntityFrameworkCore;
using personal_finance.API.Middleware;
using personal_finance.Application.Interfaces;
using personal_finance.Application.Queries.Accounts;
using personal_finance.Application.Queries.Budgets;
using personal_finance.Application.Queries.Categories;
using personal_finance.Application.Queries.Recurring;
using personal_finance.Application.Queries.Reports;
using personal_finance.Application.Queries.Transactions;
using personal_finance.Application.Services.Guards;
using personal_finance.Application.UseCases.ActivateAccount;
using personal_finance.Application.UseCases.CancelTransaction;
using personal_finance.Application.UseCases.CloseMonth;
using personal_finance.Application.UseCases.ConfirmTransaction;
using personal_finance.Application.UseCases.CreateAccount;
using personal_finance.Application.UseCases.CreateCategory;
using personal_finance.Application.UseCases.CreateRecurringTemplate;
using personal_finance.Application.UseCases.CreateTransaction;
using personal_finance.Application.UseCases.CreateTransfer;
using personal_finance.Application.UseCases.DeactivateAccount;
using personal_finance.Application.UseCases.GenerateRecurringTransactions;
using personal_finance.Application.UseCases.UpdateAccount;
using personal_finance.Application.UseCases.UpdateCategory;
using personal_finance.Application.UseCases.UpdateTransaction;
using personal_finance.Application.UseCases.UpsertBudget;
using personal_finance.Infrastructure.Persistence;
using personal_finance.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// CORS (DEV)
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCors", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
        // .AllowCredentials(); // só se no futuro usar cookies/auth
    });
});

// Swagger com grupos de endpoints
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("Commands", new() { Title = "Personal Finance API - Commands", Version = "v1" });
    c.SwaggerDoc("Queries", new() { Title = "Personal Finance API - Queries", Version = "v1" });

    c.DocInclusionPredicate((docName, apiDesc) =>
    {
        var groupName = apiDesc.GroupName ?? "Commands";
        return string.Equals(groupName, docName, StringComparison.OrdinalIgnoreCase);
    });
});

// EF Core + SQLite
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositórios EF
builder.Services.AddScoped<ITransactionRepository, EfTransactionRepository>();
builder.Services.AddScoped<ITransactionQueryRepository, EfTransactionQueryRepository>();

builder.Services.AddScoped<IAccountRepository, EfAccountRepository>();
builder.Services.AddScoped<IAccountQueryRepository, EfAccountQueryRepository>();

builder.Services.AddScoped<IReportsQueryRepository, EfReportsQueryRepository>();

builder.Services.AddScoped<ICategoryRepository, EfCategoryRepository>();
builder.Services.AddScoped<ICategoryQueryRepository, EfCategoryQueryRepository>();

builder.Services.AddScoped<IBudgetRepository, EfBudgetRepository>();
builder.Services.AddScoped<IBudgetQueryRepository, EfBudgetQueryRepository>();

builder.Services.AddScoped<IRecurringTemplateRepository, EfRecurringTemplateRepository>();
builder.Services.AddScoped<IRecurringTemplateQueryRepository, EfRecurringTemplateQueryRepository>();

builder.Services.AddScoped<IMonthClosingRepository, EfMonthClosingRepository>();

builder.Services.AddScoped<ICategoryUsageQueryRepository, EfCategoryUsageQueryRepository>();




// Command Handlers
builder.Services.AddScoped<CreateTransactionHandler>();
builder.Services.AddScoped<ConfirmTransactionHandler>();
builder.Services.AddScoped<CancelTransactionHandler>();
builder.Services.AddScoped<CreateTransferHandler>();
builder.Services.AddScoped<CreateAccountHandler>();
builder.Services.AddScoped<CreateCategoryHandler>();
builder.Services.AddScoped<UpsertBudgetHandler>();
builder.Services.AddScoped<CreateRecurringTemplateHandler>();
builder.Services.AddScoped<GenerateRecurringTransactionsHandler>();
builder.Services.AddScoped<CloseMonthHandler>();
builder.Services.AddScoped<UpdateTransactionHandler>();
builder.Services.AddScoped<UpdateAccountHandler>();
builder.Services.AddScoped<DeactivateAccountHandler>();
builder.Services.AddScoped<ActivateAccountHandler>();
builder.Services.AddScoped<UpdateCategoryHandler>();

// Query Handlers
builder.Services.AddScoped<GetAllTransactionsHandler>();
builder.Services.AddScoped<GetTransactionByIdHandler>();
builder.Services.AddScoped<GetAllAccountsHandler>();
builder.Services.AddScoped<GetAllCategoriesHandler>();
builder.Services.AddScoped<GetBudgetsByMonthHandler>();
builder.Services.AddScoped<GetAllRecurringTemplatesHandler>();
builder.Services.AddScoped<GetAccountByIdHandler>();
builder.Services.AddScoped<GetCategoryByIdHandler>();

// Reports Query Handlers
builder.Services.AddScoped<GetMonthlySummaryHandler>();
builder.Services.AddScoped<GetBalanceHandler>();
builder.Services.AddScoped<GetAccountBalanceHandler>();
builder.Services.AddScoped<GetCategorySummaryHandler>();
builder.Services.AddScoped<GetBudgetVsActualHandler>();

// Application Services
builder.Services.AddScoped<MonthCloseGuard>();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/Commands/swagger.json", "Commands");
        c.SwaggerEndpoint("/swagger/Queries/swagger.json", "Queries");
    });
}

// CORS deve vir antes de Authorization/MapControllers
if (app.Environment.IsDevelopment())
{
    app.UseCors("DevCors");
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
