using AuctionService;
using AuctionService.Data;
using AuctionService.RequestHelpers;
using AuctionService.Services;
using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();

// Added for DB context
builder.Services.AddDbContext<AuctionDBContext>(opt => {
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
});

// Added for AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Configured masstransit
builder.Services.AddMassTransit(x =>{

  x.AddEntityFrameworkOutbox<AuctionDBContext>( o=> {
    o.QueryDelay = TimeSpan.FromSeconds(10);
    o.UsePostgres();
    o.UseBusOutbox();
  });

  x.AddConsumersFromNamespaceContaining<AuctionCreatedFaultConsumer>();

  x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("auction",false));


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

builder.Services.AddGrpc();


var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();
app.MapGrpcService<GrpcAuctionService>();

try
{
    DbInitializer.InitDb(app);
}
catch (System.Exception e)
{
    
  Console.WriteLine(e);
}

app.Run();
