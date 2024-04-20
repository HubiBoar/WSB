using ToDoApp;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

BoolConverter.Register(builder.Services);
Todos.Register(builder.Services);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

Methods.RegisterRender(app);

Methods.RegisterAdd(app);
Methods.RegisterUpdate(app);
Methods.RegisterDelete(app);

app.Run();
