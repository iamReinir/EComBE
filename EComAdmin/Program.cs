using ECom;
using EComAdmin.Data;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace EComAdmin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Configuration.AddJsonFile("appsettings.json");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("Default"),
                 new MySqlServerVersion(new Version(8, 0, 39))));
            builder.Services.AddDbContext<EComContext>(options =>
               options.UseMySql(builder.Configuration.GetConnectionString("Default"),
                new MySqlServerVersion(new Version(8, 0, 39))));

            builder.Services.AddTransient(service =>
            {
                var client = new HttpClient();
                client.BaseAddress = new Uri(builder.Configuration["BE_endpoint"]);
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                return client;
            });

            // builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services
                .AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddRazorPages();
            builder.Services.AddSession();
            builder.Services.AddTransient<UserService, UserService>();
            builder.Services.AddTransient(services 
                => GrpcChannel.ForAddress(builder.Configuration["GrpcConnection"]));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();
            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
