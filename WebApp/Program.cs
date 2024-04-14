using Microsoft.OpenApi.Models;
using WebApp.Installers;

var webApplicationOptions = new WebApplicationOptions
{
    Args = args,
    ApplicationName = typeof(Program).Assembly.FullName,
    ContentRootPath = Directory.GetCurrentDirectory(),
    WebRootPath = "wwwroot/browser"
};

var builder = WebApplication.CreateBuilder(webApplicationOptions);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(context =>
{
    context.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthenticationExample.WebApp", Version = "v1" });
    context.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });
    context.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddJwt(builder.Configuration).AddAuthorization();
builder.Services.AddApi();
builder.Services.AddCore();
builder.Services.AddInfrastructure();
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseRouting();
app.UseAuthorization();
app.UseApi();

app.MapFallbackToFile("index.html");

app.Run();
