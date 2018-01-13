using gamesdatabasetwo.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace gamesdatabasetwo
{
    public class Startup
    {
        string _offline = null;
        string _online = null;

        public Startup(IHostingEnvironment env)
        {

            var builder = new ConfigurationBuilder();

            builder.AddUserSecrets<Startup>();    

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            _offline = Configuration["online"];
            _online = Configuration["offline"];

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(_offline));

            services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(_online));


            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();
            

            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            var DefaultFile = new DefaultFilesOptions();
            DefaultFile.DefaultFileNames.Clear();
            DefaultFile.DefaultFileNames.Add("index.html");

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvc();

        }
    }
}
