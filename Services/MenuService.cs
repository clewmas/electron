using ElectronNET.API.Entities;
using ElectronNET.API;

namespace elec.Services;
public class MenuService : IMenuService
{

    public MenuService()
    {}

    public void CreateMenu()
{
    var fileMenu = new MenuItem[]
    {
        new MenuItem { Label = "Home", 
                                Click = () => Electron.WindowManager.BrowserWindows.First().LoadURL($"http://localhost:{BridgeSettings.WebPort}/") },
        new MenuItem { Label = "Privacy", 
                                Click = () => Electron.WindowManager.BrowserWindows.First().LoadURL($"http://localhost:{BridgeSettings.WebPort}/Privacy") },
        new MenuItem { Label = "Update", 
                                Click = () => Electron.WindowManager.BrowserWindows.First().LoadURL($"http://localhost:{BridgeSettings.WebPort}/Update") },
        new MenuItem { Role = MenuRole.help },
        new MenuItem { Type = MenuType.separator },
        new MenuItem { Role = MenuRole.quit }
        
    };

    var viewMenu = new MenuItem[]
    {
        new MenuItem { Role = MenuRole.reload },
        new MenuItem { Role = MenuRole.forcereload },
        new MenuItem { Role = MenuRole.toggledevtools },
        new MenuItem { Type = MenuType.separator },
        new MenuItem { Role = MenuRole.resetzoom },
        new MenuItem { Role = MenuRole.zoomin },
        new MenuItem { Role = MenuRole.zoomout },
        new MenuItem { Type = MenuType.separator },
        new MenuItem { Role = MenuRole.togglefullscreen },
        new MenuItem
                    {
                        Label = "Open Developer Tools",
                        Accelerator = "CmdOrCtrl+I",
                        Click = () => Electron.WindowManager.BrowserWindows.First().WebContents.OpenDevTools()
                    },
    };

    var menu = new MenuItem[] 
    {
        new MenuItem { Label = "File", Type = MenuType.submenu, Submenu = fileMenu },
        new MenuItem { Label = "View", Type = MenuType.submenu, Submenu = viewMenu }
    };

    Electron.Menu.SetApplicationMenu(menu);
}

private void CreateContextMenu()
        {

            Electron.App.Ready += () => CreateContextMenu();

            var menu = new MenuItem[]
            {
                new MenuItem
                {
                    Label = "Hello",
                    Click = async () => await Electron.Dialog.ShowMessageBoxAsync("Electron.NET rocks!")
                },
                new MenuItem { Type = MenuType.separator },
                new MenuItem { Label = "Electron.NET", Type = MenuType.checkbox, Checked = true }
            };

            var mainWindow = Electron.WindowManager.BrowserWindows.FirstOrDefault();
            Electron.Menu.SetContextMenu(mainWindow, menu);

            Electron.IpcMain.On("show-context-menu", (args) =>
            {
                var mainWindow = Electron.WindowManager.BrowserWindows.FirstOrDefault();
                Electron.Menu.ContextMenuPopup(mainWindow);
            });
        }
}