// Program.cs (CORRIGIDO E COMPLETO)
using BuddyTech.API.Infra;
using BuddyTech.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// REGISTRO DE TODOS OS SERVIÇOS (ESSA PARTE ESTAVA FALTANDO!)
builder.Services.AddScoped<ILeadService, LeadService>();
builder.Services.AddScoped<IMissionService, MissionService>();
builder.Services.AddScoped<ISellerService, SellerService>();
builder.Services.AddScoped<IScoringService, ScoringService>();

// HttpClient para chamar a IA Python
builder.Services.AddHttpClient<IScoringService, ScoringService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration["AIServiceUrl"] ?? "http://localhost:8000");
});

// Controllers + Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();