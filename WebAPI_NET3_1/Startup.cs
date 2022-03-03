using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPI_NET3_1.Models;

namespace WebAPI_NET3_1
{
    public class Startup
    {

        private const string SECRET_KEY = "TQvgjeABMPOwCycOqah5EQu5yyVjpmVGTQvgjeABMPOwCycOqah5EQu5yyVjpmVGTQvgjeABMPOwCycOqah5EQu5yyVjpmVG";
        public static readonly SymmetricSecurityKey SIGNING_KEY = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SECRET_KEY));

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<MyContext>(options => options.UseInMemoryDatabase("Catalogue"));
            services.AddControllers();

            // Configure the JWT Authentication Service
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "JwtBearer";
                options.DefaultChallengeScheme = "JwtBearer";
            })
                    .AddJwtBearer("JwtBearer", jwtOptions =>
                    {
                        jwtOptions.TokenValidationParameters = new TokenValidationParameters()
                        {
                            // The SigningKey is defined in the TokenController class
                            IssuerSigningKey = SIGNING_KEY,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidIssuer = "https://localhost:44333",
                            ValidAudience = "https://localhost:44333",
                            ValidateLifetime = true
                        };
                    });
       } 

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
