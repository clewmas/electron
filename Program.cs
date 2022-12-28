using ElectronNET.API;
using elec.Configuration;
using elec.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var l4nLogger = new elec.Logging.Log4Net<Program>();
l4nLogger.Info($"Starting Application elec!");

builder.WebHost.UseElectron(args); //This line is required
l4nLogger.Info($"WebHost use electron!");

builder.Services.AddWebServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    //app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

if (HybridSupport.IsElectronActive)
{
    l4nLogger.Info($"Electron is active!");

    var menuService = app.Services.GetService<elec.Services.IMenuService>();
    if(menuService == null)
        throw new Exception("Unable to register menu service");
    menuService.CreateMenu();

    CreateElectronWindow();
}
else{
    l4nLogger.Info($"Electron is no-active!");
}

l4nLogger.Info($"Application elec started!");
app.Run();



async void CreateElectronWindow()
{
    var window = await Electron.WindowManager.CreateWindowAsync();
    window.OnClosed += () => Electron.App.Quit();
}