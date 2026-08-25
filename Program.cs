using Microsoft.EntityFrameworkCore;
using Radzen;
using RealEstateProj.Components;
using RealEstateProj.Data;
using RealEstateProj.Data.DTO;
using RealEstateProj.Data.Interfaces;
using RealEstateProj.Data.Service;
using RealEstateProj.Identity;
using Syncfusion.Blazor;



namespace RealEstateProj
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveServerComponents();
        

            builder.Services.AddDbContextFactory<AppDbContext>(options =>
                 options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddScoped<IPropertyService, PropertyService>();
            builder.Services.AddScoped<IPropertyImageService, PropertyImageService>();
            builder.Services.AddScoped<IUserService,UserService>();
            builder.Services.AddScoped<PropertyFilterDTO, PropertyFilterDTO>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddSyncfusionBlazor();
            builder.Services.AddRadzenComponents();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>()
                .AddInteractiveServerRenderMode();

            app.Run();
        }
    }
}

