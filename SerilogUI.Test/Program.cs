using SerilogUI;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services to the container.

//builder.Services.AddControllers();
builder.Services.AddMvc();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Logging service
var logPath = config.GetSection("EnvVars").GetValue<string>("Logging:Path") ?? "Logs";
builder.Services.AddSerilogUI(conf => conf.SetLogDirectory(logPath));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogUIDashboard(conf => conf.SetPath("/SerilogUI"));
app.UseAuthorization();

app.MapControllers();

app.Run();
