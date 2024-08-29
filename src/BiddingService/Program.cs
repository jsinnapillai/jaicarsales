using BiddingService;
using BiddingService.Services;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using MongoDB.Driver;
using MongoDB.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


// Configured masstransit
builder.Services.AddMassTransit(x =>{

x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();
  
  x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("bids",false));


  x.UsingRabbitMq((context,cfg) => {

    
    // Configuring Rabbitmq from the other machine or anywhere
    cfg.Host(builder.Configuration["RabbitMq:Host"],"/", host => {
      host.Username(builder.Configuration.GetValue("RabbitMQ:Username","guest"));
      host.Password(builder.Configuration.GetValue("RabbitMQ:Password","guest"));

    });


    cfg.ConfigureEndpoints(context);
  });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
   .AddJwtBearer(options =>
   {
       options.Authority = builder.Configuration["IdentityServiceUrl"];
       options.RequireHttpsMetadata = false;
       options.TokenValidationParameters.ValidateAudience = false;
       options.TokenValidationParameters.NameClaimType = "username";
   });


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddHostedService<CheckAuctionFinished>();
builder.Services.AddScoped<GrpcAuctionClient>();

var app = builder.Build();

// // Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await DB.InitAsync("BidDb", MongoClientSettings
.FromConnectionString(builder.Configuration.GetConnectionString("BidDbConnection")));

app.Run();
