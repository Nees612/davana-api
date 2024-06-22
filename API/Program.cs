using API.Extensions.ServiceExtensions;

var builder = WebApplication.CreateBuilder(args);
var conf = builder.Configuration;

#region builder.Services.Add* and builder.Services.Ini*
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();

//builder.Services.InitializeLogger();

builder.Services.InitializeCorsPolicy(conf);

builder.Services.InitializePolicyValidation(conf);

builder.Services.InitializeAWSOptions(conf);

builder.Services.InitializeMessageDispatcherSns();

builder.Services.InitializeDynamoDBContext(conf);

builder.Services.InitializeReposytories();

#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(conf.GetSection("CorsPolicyName").Get<string>() ?? "default");
app.MapControllers();
app.Run();
