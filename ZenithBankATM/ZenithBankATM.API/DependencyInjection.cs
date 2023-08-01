using Microsoft.EntityFrameworkCore;

using Serilog;

using ZenithBankATM.API.Database.DataContext;
using ZenithBankATM.API.Infrastructures.Filters;
using ZenithBankATM.API.Services.Account;
using ZenithBankATM.API.Services.Onboarding;

namespace ZenithBankATM.API;

public static class DependencyInjection
{
    public static WebApplication InjectServicesAndReturnWebApp(this WebApplicationBuilder builder)
    {
        // configure loggin
        var logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)
            .Enrich.FromLogContext()
            .CreateLogger();
        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog(logger);

        // configure database
        builder.Services.AddDbContext<StorageDataContext>(
        dbContextOptionsBuilder =>
        {
            string connectionString = builder.Configuration.GetConnectionString("AppConnectionString")!;
            dbContextOptionsBuilder.UseSqlServer(connectionString);
        });

        // inject in-app services
        builder.Services.AddScoped<IOnboardingService, OnboardingService>();
        builder.Services.AddScoped<IAccountService, AccountService>();

        builder.Services.AddControllers(options =>
            options.Filters.Add<ApiExceptionFilter>());

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

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

        return app;
    }
}
