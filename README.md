# Surf

This is a command for Windows Systems to quickly open your browser and browse the web from the command line.

## Usage

- Use `surf` to open to [Google](https://www.google.com/).
- Use `surf what time is it` to google ["what time is it"](https://www.google.com/search?q=what+time+is+it).
- To add more functionality or change the default, web surfing behavior look at [advanced usage](#advanced-usage).

### Advanced Usage

#### Settings JSON

- You can use `surf open` to open to the settings JSON.
- The settings JSON has 2 properties:
  1. `exePath`: This is the path to your browser's executable. If it is set to `null` or is undefined, it will use the system's default browser.
  2. `urlsPath`: This is the path to the URLs JSON. If it is set to `null` or is undefined, it will point to the wherever `surf.exe` is saved.

#### URLs JSON

- You can use `surf open urls` to open the URLs JSON.
- The URLs JSON is a list of objects with these 2 properties:
  1. `key`: This will match for a specific URL you'd like to go to.
  2. `value`: This is the specific URL you'd like to go to.
- The list is in priority of most specific to least.
- For example, if your URLs JSON includes:

```json
[
    {
        "key": "yt",
        "value": "https://www.youtube.com/results?search_query=<*>"
    },
    {
        "key": "yt",
        "value": "https://www.youtube.com/"
    }
]
```

- then using `surf yt` will open up to [YouTube](https://www.youtube.com/) whereas using `surf yt cat videos` will search for ["cat videos"](https://www.youtube.com/results?search_query=cat+videos) on YouTube.
- You can also add in multiple parameters in the `value`. For example, if your URLs JSON includes:

```json
{
	"key": "gm",
	"value": "https://mail.google.com/mail/u/<0>/#<1>/<*>"
}
```

- then using `surf gm 1 search pizzahut deals ` will search for ["pizzahut deals"](https://mail.google.com/mail/u/1/#search/pizzahut+deals) on profile 1 in Gmail.

## Building

1. Clone the repository: `git clone https://github.com/yojoecapital/Surf.git`
2. Restore the NuGet Packages using the NuGet CLI: `nuget restore`
3. Build the application using the .NET CLI: `dotnet build`
4. Run the executable located in `SurfCore/bin`

## Contact

For any inquiries or feedback, contact me at [yousefsuleiman10@gmail.com](mailto:yousefsuleiman10@gmail.com).