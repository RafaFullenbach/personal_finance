using Microsoft.EntityFrameworkCore;
using personal_finance.API.Middleware;
using personal_finance.Application.Interfaces.Accounts;
using personal_finance.Application.Interfaces.Budgets;
using personal_finance.Application.Interfaces.Categories;
using personal_finance.Application.Interfaces.CloseMonth;
using personal_finance.Application.Interfaces.Recurring;
using personal_finance.Application.Interfaces.Reports;
using personal_finance.Application.Interfaces.Transactions;
using personal_finance.Application.Queries.Accounts;
using personal_finance.Application.Queries.Budgets;
using personal_finance.Application.Queries.Categories;
using personal_finance.Application.Queries.Recurring;
using personal_finance.Application.Queries.Reports.Accounts;
using personal_finance.Application.Queries.Reports.Balance;
using personal_finance.Application.Queries.Reports.Budgets;
using personal_finance.Application.Queries.Reports.CategorySummary;
using personal_finance.Application.Queries.Reports.MonthlySummary;
using personal_finance.Application.Queries.Transactions;
using personal_finance.Application.Services.Guards;
using personal_finance.Application.UseCases.Accounts.ActivateAccount;
using personal_finance.Application.UseCases.Accounts.CreateAccount;
using personal_finance.Application.UseCases.Accounts.DeactivateAccount;
using personal_finance.Application.UseCases.Accounts.UpdateAccount;
using personal_finance.Application.UseCases.Budgets.ActivateBudget;
using personal_finance.Application.UseCases.Budgets.DeactivateBudget;
using personal_finance.Application.UseCases.Budgets.UpsertBudget;
using personal_finance.Application.UseCases.Categories.ActivateCategory;
using personal_finance.Application.UseCases.Categories.CreateCategory;
using personal_finance.Application.UseCases.Categories.DeactivateCategory;
using personal_finance.Application.UseCases.Categories.UpdateCategory;
using personal_finance.Application.UseCases.CloseMonth;
using personal_finance.Application.UseCases.Recurring.CreateRecurringTemplate;
using personal_finance.Application.UseCases.Recurring.GenerateRecurringTransactions;
using personal_finance.Application.UseCases.Transactions.CancelTransaction;
using personal_finance.Application.UseCases.Transactions.ConfirmTransaction;
using personal_finance.Application.UseCases.Transactions.CreateTransaction;
using personal_finance.Application.UseCases.Transactions.UpdateTransaction;
using personal_finance.Application.UseCases.Transfers.CreateTransfer;
using personal_finance.Infrastructure.Persistence;
using personal_finance.Infrastructure.Persistence.Repositories.Accounts;
using personal_finance.Infrastructure.Persistence.Repositories.Budgets;
using personal_finance.Infrastructure.Persistence.Repositories.Categories;
using personal_finance.Infrastructure.Persistence.Repositories.CloseMonth;
using personal_finance.Infrastructure.Persistence.Repositories.Recurring;
using personal_finance.Infrastructure.Persistence.Repositories.Reports;
using personal_finance.Infrastructure.Persistence.Repositories.Transactions;

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
builder.Services.AddScoped<ActivateCategoryHandler>();
builder.Services.AddScoped<DeactivateCategoryHandler>();
builder.Services.AddScoped<ActivateBudgetHandler>();
builder.Services.AddScoped<DeactivateBudgetHandler>();

// Query Handlers
builder.Services.AddScoped<GetAllTransactionsHandler>();
builder.Services.AddScoped<GetTransactionByIdHandler>();
builder.Services.AddScoped<GetAllAccountsHandler>();
builder.Services.AddScoped<GetAllCategoriesHandler>();
builder.Services.AddScoped<GetBudgetsByMonthHandler>();
builder.Services.AddScoped<GetAllRecurringTemplatesHandler>();
builder.Services.AddScoped<GetAccountByIdHandler>();
builder.Services.AddScoped<GetCategoryByIdHandler>();
builder.Services.AddScoped<GetBudgetByIdHandler>();

// Reports Query Handlers
builder.Services.AddScoped<GetMonthlySummaryHandler>();
builder.Services.AddScoped<GetBalanceHandler>();
builder.Services.AddScoped<GetAccountBalanceHandler>();
builder.Services.AddScoped<GetCategorySummaryHandler>();
builder.Services.AddScoped<GetBudgetVsActualHandler>();

// Application Services
builder.Services.AddScoped<MonthCloseGuard>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

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

public partial class Program { }
