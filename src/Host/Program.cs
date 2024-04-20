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

Methods.Render.Register(app);
Methods.Add.Register(app);
Methods.Update.Register(app);
Methods.Delete.Register(app);

app.Run();
