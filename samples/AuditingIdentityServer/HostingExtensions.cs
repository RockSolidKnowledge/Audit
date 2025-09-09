using Duende.IdentityServer;
using AuditingIdentityServer.Data;
using AuditingIdentityServer.Models;
using Duende.IdentityServer.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RSK.Audit.EF;
using Rsk.DuendeIdentityServer.AuditEventSink;
using Serilog;

namespace AuditingIdentityServer;

internal static class HostingExtensions
{
public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddRazorPages();
        
        string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlite(connectionString));

        builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services
            .AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = optionsBuilder =>
                    {
                        optionsBuilder.UseSqlite(connectionString,
                            sql => sql.MigrationsAssembly(typeof(HostingExtensions).Assembly.FullName));
                    };
                })
            .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = optionsBuilder =>
                    {
                        optionsBuilder.UseSqlite(connectionString,
                            sql => sql.MigrationsAssembly(typeof(HostingExtensions).Assembly.FullName));
                    };
                })
            .AddAspNetIdentity<ApplicationUser>();
        
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<AuditDatabaseContext>();

        RSK.Audit.AuditProviderFactory auditFactory = new AuditProviderFactory(dbContextOptionsBuilder.UseSqlite(connectionString).Options);

        var auditRecorder = auditFactory.CreateAuditSource("IdentityServer");

        builder.Services.AddSingleton<IEventSink>(provider => new AuditSink(auditRecorder));

        builder.Services.AddAuthentication();

        return builder.Build();
    }

    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseStaticFiles();
        app.UseRouting();
        app.UseIdentityServer();
        app.UseAuthorization();

        app.MapRazorPages()
            .RequireAuthorization();

        return app;
    }
}