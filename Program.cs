using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using WebsiteBlazor.Infrastructure;
using WebsiteBlazor.Services;
using Blazored.Toast;
using Blazorise;
using Blazorise.Bootstrap;
using Blazorise.Icons.FontAwesome;
using StackExchange.Redis;
using System.Net;
using WebsiteBlazor;

var builder = WebApplication.CreateBuilder(args);

var initialScopes = builder.Configuration["DownstreamApi:Scopes"]?.Split(' ') ?? builder.Configuration["MicrosoftGraph:Scopes"]?.Split(' ');

// Add services to the container.
builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
    .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAd"))
        .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
            .AddMicrosoftGraph(builder.Configuration.GetSection("MicrosoftGraph"))
            .AddInMemoryTokenCaches();
builder.Services.AddControllersWithViews()
    .AddMicrosoftIdentityUI();


builder.Services.AddAuthorization(options =>
{
    // By default, all incoming requests will be authorized according to the default policy
    options.FallbackPolicy = options.DefaultPolicy;
});

builder.Services.AddAuthorization(config =>
{
    config.AddPolicy(Policies.Administrator, policy => policy.RequireClaim("role", Policies.Administrator));
    config.AddPolicy(Policies.CompanyOwner, policy => policy.RequireClaim("role", Policies.CompanyOwner));
    config.AddPolicy(Policies.AccountSpecialist, policy => policy.RequireClaim("role", Policies.AccountSpecialist));
    config.AddPolicy("Guest", policy => policy.RequireClaim("role", Policies.Guest));
});


builder.Services.AddBootstrapComponents();
//services.AddBootstrapCss();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor().AddCircuitOptions(options => { options.DetailedErrors = true; })
    .AddMicrosoftIdentityConsentHandler();

builder.Services
  .AddBlazorise(options =>
  {

  })
  .AddBootstrapProviders()
  .AddFontAwesomeIcons();


builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor()
    .AddMicrosoftIdentityConsentHandler();


//Add connection to Redis
var redisConfig = new ConfigurationOptions()
{
    KeepAlive = 0,
    AllowAdmin = true,
    EndPoints = { new IPEndPoint(IPAddress.Parse(builder.Configuration["RainMaker:Redis"]), 6379) },
    ConnectTimeout = 5000,
    ConnectRetry = 5,
    SyncTimeout = 5000,
    AbortOnConnectFail = false,
};

builder.Services.AddSingleton<IConnectionMultiplexer>(x =>
ConnectionMultiplexer.Connect(redisConfig));
builder.Services.AddSingleton<ICacheService, RedisCacheService>();


//Add services to DI
builder.Services.AddTransient<CurrencyService>();
builder.Services.AddTransient<TraderService>();
builder.Services.AddTransient<CompanyService>();
builder.Services.AddTransient<EngineService>();
builder.Services.AddTransient<UserService>();
builder.Services.AddTransient<BrokerService>();
builder.Services.AddTransient<SignalrService>();
builder.Services.AddTransient<SystemService>();
builder.Services.AddSingleton<AlertService>();
builder.Services.AddSingleton<AppConfig>();
builder.Services.AddScoped<IClaimsTransformation, AddRolesClaimsTransformation>();
builder.Services.AddBlazoredToast();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}



//MiddleWare will be needed at some point - here is an example
//app.UseMiddleware<SampleMiddleWare>();

//Get Application Specific Configuration
var config = app.Services.GetRequiredService<AppConfig>();

config.DataSericeURL = builder.Configuration["RainMaker:DataServiceUrl"];
config.RmApiKey = builder.Configuration["RainMaker:RmApiKey"];
config.Redis = builder.Configuration["RainMaker:Redis"];

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();




