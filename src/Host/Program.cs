var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt => 
{
    opt.CustomSchemaIds(x => x.FullName);
});

Identity.Register(builder.Services, DataBase.Get(builder.Configuration, "Identity"));
Todo.Register(builder.Services, DataBase.Get(builder.Configuration, "Todo"));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

Identity.Map(app);
Todo.Map(app);

app.Run();