using UserAnalysisAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Configura serviços normais aqui
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

// Cria uma instância do UserService
var userService = new UserService();

// Aqui você define o caminho:
await userService.LoadUsersAsync("./Data/usuarios_1000.json");

// Registra o UserService para injeção de dependência (DI)
builder.Services.AddSingleton(userService);

var app = builder.Build();

// Configura o pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
