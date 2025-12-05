using BuddyTech.API.Infra;
using BuddyTech.API.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(Program).Assembly);

// 1. Configuração do DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// 2. Configuração dos Services (Injeção de Dependência)
// Services principais (Lógica de Negócio e Gamificação)
builder.Services.AddScoped<ILeadService, LeadService>();
builder.Services.AddScoped<IMissionService, MissionService>(); // Serviço de Gamificação
builder.Services.AddScoped<ISellerService, SellerService>();

// Integração com a IA (Serviço Python no repositório buddytech-ai-service)
// O AddHttpClient registra o IScoringService e injeta um HttpClient configurado.
builder.Services.AddHttpClient<IScoringService, ScoringService>(client =>
{
    // A URL base é lida do appsettings.json
    client.BaseAddress = new Uri(builder.Configuration["AIServiceUrl"] ?? "http://localhost:8000");
});

// 3. Configuração do HTTP Pipeline
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure o pipeline de requisições HTTP.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();