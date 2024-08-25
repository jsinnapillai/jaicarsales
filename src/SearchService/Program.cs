using System.Net;
using MassTransit;
using Polly;
using Polly.Extensions.Http;
using SearchService;
using SearchService.Data;
using SearchService.services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
// builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddHttpClient<AuctionServiceHttpClient>();//.AddPolicyHandler(GetPolicy());

// Configured masstransit
builder.Services.AddMassTransit(x =>{
  x.AddConsumersFromNamespaceContaining<AuctionCreatedConsumer>();

  x.SetEndpointNameFormatter(new KebabCaseEndpointNameFormatter("search",false));

  
  x.UsingRabbitMq((context,cfg) => {

    cfg.ReceiveEndpoint("search-auction-created",e => {
      e.UseMessageRetry(r=> r.Interval(5,5));
      e.ConfigureConsumer<AuctionCreatedConsumer>(context);
    });

    // Configuring Rabbitmq from the other machine or anywhere
    cfg.Host(builder.Configuration["RabbitMQ:Host"],"/", host => {
      host.Username(builder.Configuration.GetValue("RabbitMQ:Username","guest"));
      host.Password(builder.Configuration.GetValue("RabbitMQ:Password","guest"));

    });    

    cfg.ConfigureEndpoints(context);
  });
});


// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
//     app.UseSwagger();
//     app.UseSwaggerUI();
// }

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Lifetime.ApplicationStarted.Register(async () =>
{
    try
{
   await  DbInitializer.InitDb(app);
}
catch (System.Exception e)
{
    
   Console.WriteLine(e);
}

});



app.Run();


// static IAsyncPolicy<HttpResponseMessage> GetPolicy()
// => HttpPolicyExtensions
//     .HandleTransientHttpError()
//     .OrResult(msg => msg.StatusCode == HttpStatusCode.NotFound)
//     .WaitAndRetryForeverAsync(_ => TimeSpan.FromSeconds(3));