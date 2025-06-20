using HTU_FinalProject.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.SemanticKernel;
using ResumeAI.Data;
using ResumeAI.Interfaces;
using ResumeAI.Models.Person;
using ResumeAI.MyService;

namespace ResumeAI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<Person>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();
            builder.Services.AddScoped<IResume, ResumeService>();
            builder.Services.AddScoped<IPortfolioService, PortfolioService>();
            builder.Services.AddScoped<ICreateEmail, EmailService>();
            builder.Services.AddScoped<ICoverLetter, CoverLetterService>();

            var key = builder.Configuration["OpenAI:Key"];
            builder.Services.AddSingleton<Kernel>(sp =>
            {
                var kernelBuilder = Kernel.CreateBuilder();
                kernelBuilder.AddOpenAIChatCompletion("gpt-4", key);
                return kernelBuilder.Build();
            });
            builder.Services.AddScoped<IResumeParser, ResumeParser>();
            builder.Services.AddScoped<IPortfolioParser, PortFolioParser>();
            builder.Services.AddScoped<ICreateEmailParser, EmailParser>();
            builder.Services.AddScoped<ICoverLetterParser, CoverLetterParser>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
            app.MapRazorPages();

            app.Run();
        }
    }
}
