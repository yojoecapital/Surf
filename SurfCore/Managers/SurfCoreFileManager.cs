using CliFramework;
using Newtonsoft.Json;
using surf.Utilities;
using System.Net;
using System.Text.RegularExpressions;

namespace surf.Managers
{
    internal class SurfCoreFileManager : FileManager
    {
        public static string SettingsFilePath
        {
            get => GetDictionaryFilePath("settings.json");
        }

        private Settings settings;
        public Settings Settings
        {
            get
            {
                settings ??= GetObject<Settings>(SettingsFilePath);
                if (settings == null)
                {
                    PrettyConsole.PrintError("Could not parse settings JSON file.");
                    return null;
                }
                else return settings;
            }
            set
            {
                settings = value;
                SetObject(SettingsFilePath, settings, Formatting.Indented);
            }
        }

        public string UrlsFilePath
        {
            get
            {
                var settings = Settings;
                string path;
                if (settings != null && !string.IsNullOrEmpty(settings.urlsPath)) 
                    path = GetFilePath(settings.urlsPath);
                else path = GetFilePath("urls.json");
                if (File.Exists(path)) return path;
                File.WriteAllText(path, "[]");
                return path;
            }
        }

        private List<Url> urls;
        private List<Url> Urls
        {
            get
            {
                urls ??= GetObject<List<Url>>(UrlsFilePath);
                if (urls == null)
                {
                    PrettyConsole.PrintError("Could not parse URLs JSON file.");
                    return null;
                }
                else return urls;
            }
        }

        public string MatchUrl(string[] args)
        {
            var urls = Urls;
            if (args.Length == 0)
            {
                if (FindUrl(urls, new[] { "default" }, out var value1)) return value1;
                else return "https://www.google.com/";
            }
            else if (FindUrl(urls, args, out var value2)) return value2;
            else 
            {    
                if (FindUrl(urls, new[] { "default" }.Concat(args), out var value3)) return value3;
                return "https://www.google.com/search?q=" + WebUtility.UrlEncode(string.Join(" ", args));
            }
        }

        private static bool FindUrl(IEnumerable<Url> urls, IEnumerable<string> args, out string value)
        {
            foreach (var url in urls.Where(url => url.key.ToLower().Equals(args.First().ToLower()))) 
            {
                if (Math.Max(args.Count() - 1, 0) >= GetMinArgs(url.value)) 
                {
                    args = args.Skip(1);
                    value = url.value;
                    string pattern = @"<(\d+)>";
                    var matches = Regex.Matches(url.value, pattern);
                    int largestIndex = -1;
                    foreach (var match in matches.Cast<Match>())
                    {
                        if (int.TryParse(match.Groups[1].Value, out int index))
                        {
                            largestIndex = Math.Max(largestIndex, index);
                            var arg = args.ElementAt(index);
                            value = value.Replace($"<{index}>", arg);
                        }
                    }
                    if (value.Contains("<*>"))
                    {
                        if (largestIndex == args.Count()) continue;
                        var remaining = args.Skip(largestIndex + 1);
                        value = value.Replace("<*>", WebUtility.UrlEncode(string.Join(" ", remaining)));
                    }
                    return true;
                }
            }
            value = null;
            return false;
        }

        private static int GetMinArgs(string value)
        {
            string pattern = @"<(\d+)>";
            var matches = Regex.Matches(value, pattern);
            int largestNumber = 0;
            foreach (var match in matches.Cast<Match>())
            {
                if (int.TryParse(match.Groups[1].Value, out int number))
                    largestNumber = Math.Max(largestNumber, number + 1);
            }
            if (value.Contains("<*>")) largestNumber++;
            return largestNumber;
        }
    }
}
