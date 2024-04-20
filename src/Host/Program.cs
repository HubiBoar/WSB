using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt => 
{
    opt.CustomSchemaIds(x => x.FullName);
});

var db = DataBase.Sqlite;

Todo.Register(builder.Services, db);
Identity.Register(builder.Services, db);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/Users", (ClaimsPrincipal user) => $"Hello {user.Identity!.Name}")
    .RequireAuthorization();

Todo.Map(app);
Identity.Map(app);

app.Run();
