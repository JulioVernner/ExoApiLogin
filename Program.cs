using Exo.WebApi.Contexts;
using Exo.WebApi.Repositories;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<ExoContext, ExoContext>();
builder.Services.AddControllers();

 builder.Services.AddAuthentication(options => 
 {
    options.DefaultAuthenticateScheme = "JwtBearer";
    options.DefaultChallengeScheme = "JwtBearer";
 })
 //parametrto de validao do token
 .AddJwtBearer("JwtBearer", options => 
 {
    options.TokenValidationParameters = new TokenValidationParameters
    {
        //valida quem esta solicitando
        ValidateIssuer = true,
        //valida quem ta recebendo
        ValidateAudience = true,
        //define se o tempo de expericao sera validado
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes("EXO-API-ATENTICACAO")),
        //ValidationFailure o tempo de expiracao do token
        ClockSkew = TimeSpan.FromMinutes(30),
        //nome do issuer, da origem
        ValidIssuer = "exoapi.webapi",
        ValidAudience = "exoapi.webapi",
    };
 });

builder.Services.AddTransient<ProjetoRepository, ProjetoRepository>();
builder.Services.AddTransient<UsuarioRepository, UsuarioRepository>();

var app = builder.Build(); 

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();
