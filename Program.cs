using AIWellness.Chat;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.MapPost("/message", async (Request request) =>
{
	string response = Chat.Message(request);

	request.Messages.Add(request.Message);
	request.Messages.Add(response);

	return request.Messages;
});

app.Run();

