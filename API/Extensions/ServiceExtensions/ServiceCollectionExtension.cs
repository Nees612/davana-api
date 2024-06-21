using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using Amazon.SimpleNotificationService;
using API.Data;
using API.Data.Repositories;
using API.Data.Repositories.Interfaces;
using API.Services.Authentication;
using API.Services.Authentication.Models;
using API.Services.SimpleNotification;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using Serilog.Formatting.Json;

namespace API.Extensions.ServiceExtensions
{
    public static class ServiceCollectionExtension
    {
        public static void InitializeLogger(this IServiceCollection services)
        {
            var loggerConf = new LoggerConfiguration().MinimumLevel.Debug().WriteTo.Console(new JsonFormatter()).Enrich.FromLogContext();
            Log.Logger = loggerConf.CreateLogger();
            services.AddLogging(conf => conf.AddSerilog(Log.Logger));
        }

        public static void InitializeCorsPolicy(this IServiceCollection services, ConfigurationManager conf)
        {
            var myCorsPolicy = conf.GetSection("CorsPolicyName").Get<string>() ?? "default";

            services.AddCors(options =>
            {
                options.AddPolicy(myCorsPolicy, policy =>
                {
                    policy.WithOrigins("http://localhost:4200");
                    policy.AllowAnyHeader();
                    policy.AllowAnyMethod();
                });
            });
        }

        public static void InitializePolicyValidation(this IServiceCollection services, ConfigurationManager conf)
        {
            services.AddAuthentication()
                .AddJwtBearer("Bearer",
                        options => options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = conf.GetSection("Authentication:Schemes:Bearer:ValidIssuer").Get<string>(),
                            ValidAudiences = conf.GetSection("Authentication:Schemes:Bearer:ValidAudiences").Get<List<string>>(),
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthSettings.PrivateKey))
                        });

            services.AddAuthorizationBuilder()
                .AddPolicy("coach",
                policy => policy.RequireRole("coach")
                                .RequireClaim("scope", "timetable"))
                .AddPolicy("user",
                policy => policy.RequireRole("user")
                                .RequireClaim("scope", "booking"));
        }

        public static void InitializeAWSOptions(this IServiceCollection services, ConfigurationManager conf)
        {
            AWSOptions awsOptions = conf.GetAWSOptions();
            services.AddDefaultAWSOptions(awsOptions);
        }

        public static void InitializeDynamoDBContext(this IServiceCollection services, ConfigurationManager conf)
        {

            var dynamoDbConfig = conf.GetSection("DynamoDb");
            var runLocalDynamoDb = dynamoDbConfig.GetValue<bool>("LocalMode");

            if (runLocalDynamoDb)
            {
                services.AddSingleton<IAmazonDynamoDB>(sp =>
                {
                    var clientConfig = new AmazonDynamoDBConfig { ServiceURL = dynamoDbConfig.GetValue<string>("LocalServiceUrl") };
                    return new AmazonDynamoDBClient(clientConfig);
                });
            }
            else
            {
                services.AddAWSService<IAmazonDynamoDB>();
            }

            services.AddSingleton<IDavanaDynamoDBContext, DavanaDynamoDBContext>((serviceProvider) =>
            {
                IAmazonDynamoDB amazonDynamoDBClient = serviceProvider.GetRequiredService<IAmazonDynamoDB>();
                DynamoDBContextConfig dynamoDBContextConfig = new DynamoDBContextConfig
                {
                    TableNamePrefix = dynamoDbConfig.GetValue<string>("TableNamePrefix")
                };
                return new DavanaDynamoDBContext(amazonDynamoDBClient, dynamoDBContextConfig);
            });

        }

        public static void InitializeReposytories(this IServiceCollection services)
        {
            services.AddScoped<IAppointmentDynamoRepository, AppointmentDynamoRepository>();
            services.AddScoped<ICoachesDynamoRepository, CoachesDynamoRepository>();
            services.AddScoped<IUsersDynamoRepository, UsersDynamoRepository>();
        }

        public static void InitializeAuthentication(this IServiceCollection services)
        {
            services.AddScoped<CoachAuthenticationService>();
            services.AddScoped<UserAuthenticationService>();
        }

        public static void InitializeMessageDispatcherSns(this IServiceCollection services)
        {
            services.AddScoped<IAmazonSimpleNotificationService, AmazonSimpleNotificationServiceClient>();
            services.AddScoped<IMessageDispatcherSns, MessageDispatcherSns>();
        }
    }
}