namespace ERP.HRM.API.Configuration
{
    /// <summary>
    /// CORS configuration extension
    /// </summary>
    public static class CorsConfiguration
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
                ?? new[] { "http://localhost:3000", "http://localhost:4200" };

            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigins", builder =>
                {
                    builder
                        .WithOrigins(allowedOrigins)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                        .WithExposedHeaders("X-Total-Count", "X-Page-Number", "X-Page-Size");
                });

                options.AddPolicy("AllowAll", builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseCustomCors(this IApplicationBuilder app, IWebHostEnvironment env)
        {
            var policyName = env.IsDevelopment() ? "AllowAll" : "AllowSpecificOrigins";
            app.UseCors(policyName);
            return app;
        }
    }
}
