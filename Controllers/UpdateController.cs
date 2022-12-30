﻿using Microsoft.AspNetCore.Mvc;
using elec.Services;
using ElectronNET.API;

namespace elec.Controllers
{
    public class UpdateController : BaseController<UpdateController>
    {
        public UpdateController(
        elec.Logging.ILogger<UpdateController> logger,
        IControllerExceptionHandler exceptionHandler)
        : base(logger, exceptionHandler)
        {
        }
        public IActionResult Index()
        {
            try
            {
                if (HybridSupport.IsElectronActive)
                {
                    Electron.AutoUpdater.OnError += (message) => Electron.Dialog.ShowErrorBox("Error", message);
                    Electron.AutoUpdater.OnCheckingForUpdate += async () => await Electron.Dialog.ShowMessageBoxAsync("Checking for Update");
                    Electron.AutoUpdater.OnUpdateNotAvailable += async (info) => await Electron.Dialog.ShowMessageBoxAsync("Update not available");
                    Electron.AutoUpdater.OnUpdateAvailable += async (info) => await Electron.Dialog.ShowMessageBoxAsync("Update available" + info.Version);
                    Electron.AutoUpdater.OnDownloadProgress += (info) =>
                    {
                        var message1 = "Download speed: " + info.BytesPerSecond + "\n<br/>";
                        var message2 = "Downloaded " + info.Percent + "%" + "\n<br/>";
                        var message3 = $"({info.Transferred}/{info.Total})" + "\n<br/>";
                        var message4 = "Progress: " + info.Progress + "\n<br/>";
                        var information = message1 + message2 + message3 + message4;

                        var mainWindow = Electron.WindowManager.BrowserWindows.First();
                        Electron.IpcMain.Send(mainWindow, "auto-update-reply", information);
                    };
                    Electron.AutoUpdater.OnUpdateDownloaded += async (info) => await Electron.Dialog.ShowMessageBoxAsync("Update complete!" + info.Version);

                    Electron.IpcMain.On("auto-update", async (args) =>
                    {
                        // Electron.NET CLI Command for deploy:
                        // electronize build /target win /electron-params --publish=always

                        var currentVersion = await Electron.App.GetVersionAsync();
                        _logger.Info($"Electron app current version: {currentVersion}");

                        Electron.AutoUpdater.AutoDownload = false;
                        _logger.Info($"Electron app update - false");

                        #if DEBUG
                        var updateCheckResult = await Electron.AutoUpdater.CheckForUpdatesAsync();
                        #else
                        var updateCheckResult = await Electron.AutoUpdater.CheckForUpdatesAndNotifyAsync();
                        #endif

                        var availableVersion = updateCheckResult.UpdateInfo.Version;
                        _logger.Info($"Available version: {availableVersion}");

                        string information = $"Current version: {currentVersion} - available version: {availableVersion}";
                        _logger.Info(information);

                        var mainWindow = Electron.WindowManager.BrowserWindows.First();
                        Electron.IpcMain.Send(mainWindow, "auto-update-reply", information);
                    });
                }

                return View();
            }
            catch (Exception ex)
            {
                return HandleException(ex);
            }
        }
    }
}
