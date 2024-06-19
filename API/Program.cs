using System.Text;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.DataModel;
using Amazon.Extensions.NETCore.Setup;
using API.Data;
using API.Data.Repositories;
using API.Data.Repositories.Interfaces;
using API.Entities;
using API.Extensions.DynamoDbExtensions;
using API.Services.Authentication;
using API.Services.Authentication.Interfaces;
using API.Services.Authentication.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

var conf = builder.Configuration;
var myCorsPolicy = "_davanaCorsPolicy";

builder.Services.AddCors(options =>
{
    options.AddPolicy(myCorsPolicy, policy =>
    {
        policy.WithOrigins("http://localhost:4200");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

builder.Services.AddAuthentication()
                .AddJwtBearer("Bearer",
                        options => options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidIssuer = conf.GetSection("Authentication:Schemes:Bearer:ValidIssuer").Get<string>(),
                            ValidAudiences = conf.GetSection("Authentication:Schemes:Bearer:ValidAudiences").Get<List<string>>(),
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(AuthSettings.PrivateKey))
                        });

builder.Services.AddAuthorizationBuilder()
    .AddPolicy("coach",
    policy => policy.RequireRole("coach")
                    .RequireClaim("scope", "timetable"))
    .AddPolicy("user",
    policy => policy.RequireRole("user")
                    .RequireClaim("scope", "booking"));


// Get the AWS profile information from configuration providers
AWSOptions awsOptions = builder.Configuration.GetAWSOptions();

// Configure AWS service clients to use these credentials
builder.Services.AddDefaultAWSOptions(awsOptions);

var dynamoDbConfig = builder.Configuration.GetSection("DynamoDb");
var runLocalDynamoDb = dynamoDbConfig.GetValue<bool>("LocalMode");

#region DynamoDB setup
if (runLocalDynamoDb)
{
    builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
    {
        var clientConfig = new AmazonDynamoDBConfig { ServiceURL = dynamoDbConfig.GetValue<string>("LocalServiceUrl") };
        return new AmazonDynamoDBClient(clientConfig);
    });
}
else
{
    builder.Services.AddAWSService<IAmazonDynamoDB>();
}

builder.Services.AddSingleton<IDynamoDBContext, DavanaDynamoDBContext>((serviceProvider) =>
{
    IAmazonDynamoDB amazonDynamoDBClient = serviceProvider.GetRequiredService<IAmazonDynamoDB>();
    DynamoDBContextConfig dynamoDBContextConfig = new DynamoDBContextConfig
    {
        TableNamePrefix = dynamoDbConfig.GetValue<string>("TableNamePrefix")
    };
    return new DavanaDynamoDBContext(amazonDynamoDBClient, dynamoDBContextConfig);
});

#endregion


builder.Services.AddScoped<IAppointmentRepository, AppointmentRepository>();
builder.Services.AddScoped<ICoachRepository, CoachRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<IAppointmentDynamoRepository, AppointmentDynamoRepository>();
builder.Services.AddScoped<ICoachesDynamoRepository, CoachesDynamoRepository>();
builder.Services.AddScoped<IUsersDynamoRepository, UsersDynamoRepository>();

builder.Services.AddScoped<CoachAuthenticationService>();
builder.Services.AddScoped<UserAuthenticationService>();


builder.Services.AddDbContext<DavanaContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    // New Option required for using DynamoDB
});


// DynamoDbHelper.CreateSchema();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(myCorsPolicy);
app.MapControllers();
app.Run();
