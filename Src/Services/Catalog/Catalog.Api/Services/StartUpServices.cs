namespace Catalog.Api.Services
{
    public static class CustomExtensionMethods
    {
        public static IServiceCollection AddCustomMVC(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers()
                .AddJsonOptions(options => { options.JsonSerializerOptions.WriteIndented = true; });

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder
                        .SetIsOriginAllowed((host) => true)
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            return services;


        }
        public static IServiceCollection AddSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "eShop - Catalog HTTP API",
                    Version = "v1",
                    Description = "The Catalog Microservice HTTP API. This is a Data-Driven/CRUD microservice sample"
                });
            });

            return services;

        }
        public static IServiceCollection AddCustomOptions(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.ClientErrorMapping[404].Title = " Not Found Resouce Or Api ";
                options.ClientErrorMapping[403].Title = "Forbidden";
                options.InvalidModelStateResponseFactory = context =>
                {
                    var values = context.ModelState.Values.Where(state => state.Errors.Count != 0)
                        .Select(state => new { ErrorMessage = state.Errors.Select(p => p.ErrorMessage) });

                    string ErrorDetials = JsonConvert.SerializeObject(values) ?? "Please refer to the errors property for additional details.";

                    var problemDetails = new ValidationProblemDetails(context.ModelState)
                    {
                        Instance = context.HttpContext.Request.Path,
                        Status = StatusCodes.Status400BadRequest,
                        Detail = ErrorDetials// "Please refer to the errors property for additional details."
                    };

                    return new BadRequestObjectResult(problemDetails)
                    {
                        ContentTypes = { "application/problem+json", "application/problem+xml" }
                    };
                };
            });


            return services;
        }

    }


}
