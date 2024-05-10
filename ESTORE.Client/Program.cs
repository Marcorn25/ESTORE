using ESTORE.Client;
using ESTORE.Client.Services.AuthServices;
using ESTORE.Client.Services.CartServices;
using ESTORE.Client.Services.CategoryServices;
using ESTORE.Client.Services.ChatMessageServices;
using ESTORE.Client.Services.DashboardServices;
using ESTORE.Client.Services.FileUploadServices;
using ESTORE.Client.Services.OrderService;
using ESTORE.Client.Services.ProductServices;
using ESTORE.Client.Services.UserService;
using ESTORE.Client.States.Order;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddMudServices();

//AuthenticationStateProvider
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

//Client Services
builder.Services.AddScoped<IClientAuthService, ClientAuthService>();
builder.Services.AddScoped<IClientUserService, ClientUserService>();
builder.Services.AddScoped<IClientProductService, ClientProductService>();
builder.Services.AddScoped<IClientCategoryService, ClientCategoryService>();
builder.Services.AddScoped<IClientFileUploadService, ClientFileUploadService>();
builder.Services.AddScoped<IClientChatMessageService, ClientChatMessageService>();
builder.Services.AddScoped<IClientCartService, ClientCartService>();
builder.Services.AddScoped<IClientOrderService, ClientOrderService>();
builder.Services.AddScoped<IClientDashboardService, ClientDashboardService>();
builder.Services.AddSingleton<OrderState>();

await builder.Build().RunAsync();
