using AIWellness.claude;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddCors(options =>
{
	options.AddPolicy(name: "_allowAny", policy =>
		policy.AllowAnyOrigin()
			.AllowAnyMethod()
			.AllowAnyHeader());
});
var app = builder.Build();

app.UseCors("_allowAny");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
}

app.UseHttpsRedirection();
    app.MapOpenApi();


app.UseHttpsRedirection();


app.MapPost("/message", async (HttpContext httpContext) =>
{
	var request = await httpContext.Request.ReadFromJsonAsync<Request>();
	if (request == null)
	{
		return Results.BadRequest("Invalid request body");
	}

	string? response = await Chat.AWSChat(request);

	if (response == null)
	{
		return Results.Ok(string.Empty);
	}

	request.Messages.Add(request.Message);
	request.Messages.Add(response);

	return Results.Ok(request.Messages);
});

app.Run();

