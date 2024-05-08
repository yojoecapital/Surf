using CliFramework;
using System.Diagnostics;

namespace surf.Managers
{
    internal class CommandManager
    {
        public CommandManager(SurfCoreFileManager fileManager) =>
           this.fileManager = fileManager;

        private readonly SurfCoreFileManager fileManager;

        public void Match(string[] args)
        {
            var settings = fileManager.Settings;
            var value = fileManager.MatchUrl(args);
            if (settings.exePath == null)
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = value,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                catch
                {
                    PrettyConsole.PrintError($"Could not open to \"{value}\".");
                }
            }
            else
            {
                try
                {
                    var psi = new ProcessStartInfo
                    {
                        FileName = settings.exePath,
                        Arguments = value,
                        UseShellExecute = true
                    };
                    Process.Start(psi);
                }
                catch
                {
                    PrettyConsole.PrintError($"Could not open to \"{value}\" using \"{settings.exePath}\".");
                }
            }
        }

        public static void OpenSettings(string[] _)
        {
            var path = SurfCoreFileManager.SettingsFilePath;
            ProcessStartInfo psi = new()
            {
                FileName = path,
                UseShellExecute = true
            };
            Process.Start(psi);
            Console.WriteLine(path);
        }

        public void OpenUrls(string[] _)
        {
            var path = fileManager.UrlsFilePath;
            ProcessStartInfo psi = new()
            {
                FileName = path,
                UseShellExecute = true
            };
            Process.Start(psi);
            Console.WriteLine(path);
        }
    }
}
