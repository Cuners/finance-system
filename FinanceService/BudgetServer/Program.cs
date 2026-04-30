using Azure.Core;
using Finance.Application.DependencyInjection;
using Finance.Application.DTO;
using Finance.Application.UseCases;
using Finance.Infrastructure.DependencyInjection;
using Finance.Infrastructure.Persistence;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using RabbitMQ.Client;
using StackExchange.Redis;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson(options =>
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})

.AddJwtBearer(options =>
{
    var jwtSettings = builder.Configuration.GetSection("jwtSettings");
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var accessToken = context.Request.Cookies["access_token"];
            if (!string.IsNullOrEmpty(accessToken))
            {
                context.Token = accessToken;
            }
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization();
builder.Services.AddScoped<IUnitOfWork>(provider => (IUnitOfWork)provider.GetRequiredService<BudgetDbContext>());

builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var redisConnectionString = builder.Configuration.GetConnectionString("Redis");
    if (string.IsNullOrWhiteSpace(redisConnectionString))
    {
        throw new InvalidOperationException("Redis connection string is not configured");
    }
    var config = ConfigurationOptions.Parse(redisConnectionString);
    return ConnectionMultiplexer.Connect(config);
});
builder.Services.AddMassTransit(x =>
{
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(builder.Configuration["RabbitMQ:Host"], h =>
        {
            h.Username(builder.Configuration["RabbitMQ:Username"]);
            h.Password(builder.Configuration["RabbitMQ:Password"]);
        });
        cfg.Message<TransactionCreatedEvent>(x => x.SetEntityName("TransactionCreated"));
        cfg.ConfigureEndpoints(context);
    });
});
//var rabbitMQFactory = new ConnectionFactory
//{
//    HostName = builder.Configuration["RabbitMQ:Host"],
//    UserName = builder.Configuration["RabbitMQ:Username"],
//    Password = builder.Configuration["RabbitMQ:Password"],
//    AutomaticRecoveryEnabled = true,
//    NetworkRecoveryInterval = TimeSpan.FromSeconds(5),
//    RequestedConnectionTimeout = TimeSpan.FromSeconds(30),
//    SocketReadTimeout = TimeSpan.FromSeconds(30),
//    SocketWriteTimeout = TimeSpan.FromSeconds(30),
//};
//IConnection rabbitMQConnection = await rabbitMQFactory.CreateConnectionAsync();
//builder.Services.AddSingleton<IConnection>(rabbitMQConnection);

builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");

});
builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy",
        builder =>
        {
            builder.AllowCredentials().AllowAnyMethod().AllowAnyHeader().WithOrigins("http://localhost:5173", "http://localhost");
        });
}
);

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();
app.UseCors("CORSPolicy");
app.UseAuthentication();
app.UseAuthorization();
//app.MapGet("get", () =>
//{
//    return Results.Ok("ok");
//});
app.MapControllers();

app.Run();
