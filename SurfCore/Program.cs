using CliFramework;
using surf.Managers;

namespace Surf
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Repl repl = new();
            CommandManager commandManager = new(new SurfCoreFileManager());
            repl.AddCommand(
                args => args.Length == 1 && args[0].Equals("open"),
                CommandManager.OpenSettings,
                "open",
                "Open the settings JSON file."
            );
            repl.AddCommand(
                args => args.Length == 2 && args[0].Equals("open") && args[1].Equals("urls"),
                commandManager.OpenUrls,
                "open",
                "Open the URLs JSON file."
            );
            repl.AddCommand(
                _ => true,
                commandManager.Match,
                "[query...]",
                "Search for something."
            );
            repl.Process(args, false);
        }
    }
}