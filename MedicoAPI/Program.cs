using Microsoft.EntityFrameworkCore;
using MedicoAPI.Data;
using MedicoAPI.Controllers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Amazon.Scheduler;
using Amazon.CognitoIdentityProvider;

var builder = WebApplication.CreateBuilder(args);

var connectionString = Environment.GetEnvironmentVariable("MedicoAPIContext");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Connection string 'MedicoAPIContext' not found.");
}
builder.Services.AddDbContext<MedicoAPIContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
    options.AddPolicy("AllowSpecificOrigin",
            builder => builder.WithOrigins("http://127.0.0.1:5500", "http://localhost:5173/")
                              .AllowAnyMethod()
                              .AllowAnyHeader()));
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder => builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader());
});

var awsOptions = builder.Configuration.GetAWSOptions();

builder.Services.AddDefaultAWSOptions(awsOptions);
builder.Services.AddAWSService<IAmazonScheduler>();
builder.Services.AddAWSService<IAmazonCognitoIdentityProvider>();
builder.Services.AddScoped<AppointmentService>();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddScoped<AgoraTokenService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.Authority = "https://cognito-idp.ca-central-1.amazonaws.com/ca-central-1_AiLYmagKl";
    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
    {
        ValidateAudience = false,
        ValidAudience = "",
        ValidateIssuer = true,
        ValidIssuer = "https://cognito-idp.ca-central-1.amazonaws.com/ca-central-1_AiLYmagKl",
        ValidateLifetime = true,
        RoleClaimType = "cognito:groups",
    };
});

var googleApiKey = Environment.GetEnvironmentVariable("GoogleAPIKey");
if (string.IsNullOrEmpty(googleApiKey))
{
    throw new InvalidOperationException("Google API Key not found.");
}
builder.Configuration["GoogleAPIKey"] = googleApiKey;

builder.Services.AddAuthorization();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<MedicoAPIContext>();
    dbContext.Database.Migrate();
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseCors("AllowSpecificOrigin");
app.UseCors("AllowAllOrigins");


app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
