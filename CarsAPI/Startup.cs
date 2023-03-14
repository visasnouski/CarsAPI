using System.Text;
using CarsAPI.Interfaces;
using CarsAPI.Internal;
using CarsAPI.Middlewares;
using CarsAPI.Options;
using DataAccess.Data;
using DataAccess.DbAccess;
using DataAccess.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using Swashbuckle.AspNetCore.Filters;
using ExceptionHandlerMiddleware = CarsAPI.Middlewares.ExceptionHandlerMiddleware;

namespace CarsAPI
{
	public class Startup
	{
		private readonly IConfiguration _configuration;

		public Startup(IConfiguration configuration)
		{
			_configuration = configuration;

			Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(configuration)
				.CreateLogger();
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddSingleton<ISqlDataAccess, SqlDataAccess>();
			services.AddSingleton<IUserData, UserData>();
			services.AddSingleton<ICarsData, CarsData>();

			services.AddSingleton<IPasswordManager, PasswordManager>();
			services.AddSingleton<ITokenBuilder, TokenBuilder>();
			services.AddSingleton<IDateTimeFacade, DateTimeFacade>();

			services.AddControllers();

			services.AddEndpointsApiExplorer();

			services.Configure<DbConnectionSettings>(_configuration.GetSection("connectionStrings"));
			services.Configure<JwtSettings>(_configuration.GetSection("jwtConfig"));

			services.AddSwaggerGen(options =>
			{
				options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
				{
					Description = "Standard Authorization header using Bearer token.",
					In = ParameterLocation.Header,
					Name = "Authorization",
					Type = SecuritySchemeType.ApiKey,
				});

				options.OperationFilter<SecurityRequirementsOperationFilter>();
			});

			var secretSettings = new JwtSettings();
			_configuration.Bind("jwtConfig", secretSettings);

			if (secretSettings.Secret.IsNullOrEmpty())
			{
				throw new InvalidOperationException("The secret is not specified in the configuration file.");
			}

			services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
				 .AddJwtBearer(options =>
				 {
					 options.TokenValidationParameters = new TokenValidationParameters()
					 {
						 ValidateIssuerSigningKey = true,
						 IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretSettings.Secret!)),
						 ValidateIssuer = false,
						 ValidateAudience = false,
						 RequireExpirationTime = true
					 };
				 });
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
            app.UseMiddleware<ExceptionHandlerMiddleware>();

			if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseRouting();
			app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseMiddleware<LoggingMiddleware>();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
	}
}
