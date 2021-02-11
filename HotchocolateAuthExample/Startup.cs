using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace HotchocolateAuthExample
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options => 
                    options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = "hctest.com",
                    ValidAudience = "hctest.com",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("testkeytestkeytestkeytestkeytestkeytestkeytestkeytestkeytestkeytestkey"))
                });

            services
                .AddAuthorization()
                .AddHttpContextAccessor()
                .AddGraphQLServer()
                .AddAuthorization()
                .AddQueryType<Query>();

            Console.WriteLine("JWT Token:");
            Console.WriteLine(GenerateJsonWebToken());
        }

        public static string GenerateJsonWebToken()
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("testkeytestkeytestkeytestkeytestkeytestkeytestkeytestkeytestkeytestkey")); 

            var token = new JwtSecurityToken(
                "hctest.com",
                "hctest.com",
                Array.Empty<Claim>(),
                expires: DateTime.MaxValue,
                signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGraphQL();
            });
        }
    }
}
