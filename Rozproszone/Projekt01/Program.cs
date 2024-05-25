var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

Identity.Register(builder.Services, DataBase.Get(builder.Configuration, "Identity"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapGet("/elo", () =>
{
    
})
.WithName("ELO")
.WithOpenApi()
.AllowAnonymous();

Identity.Map(app);

app.Run();

//2g2f1VLELG7RVWTPrA1CgnkwfLt_5Z3iPgDaXBdcoFpzBAtGn 