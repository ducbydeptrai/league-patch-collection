# League Patch Collection - A Quality-of-Life Tweaker for macOS and Windows

**League Patch Collection** is a lightweight C# application designed for macOS and Windows to enhance your League of Legends experience. This tool introduces several quality-of-life features and optimizations for a smoother and less intrusive gaming experience.

## Features

By default, this app provides the following enhancements:
- **Removal of Bloatware**: Removes the Legends of Runeterra (LoR) button and Info Hub.
- **Streamlined Interface**: Eliminates promotions and other unnecessary clutter from the client.
- **Ban Reason Fix**: Resolves issues where the ban reason doesn't display on certain accounts, fixing the infinite loading/unknown player bug.
- **Enhanced Privacy**: Disables all tracking and telemetry services, including Sentry, to reduce tracking and prevent unnecessary background activity.
- **Home Hub Fix**: Fixes home hubs taking longer than usual to load issue.
- **[Coming Soon] Lobby Revealer**: A feature to reveal names in champ select.

## Command-Line Arguments

Enhance functionality further by using the following command-line arguments:

- `--novgk`: Disables Vanguard anti-cheat enforcement, making the Riot Client believe that League of Legends/Valorant does not require it. Also prevents vgc.exe from initializing.
- `--legacyhonor`: Reverts the honor system to its pre-patch 14.9 state, removing the option to honor enemies.
- `--appearoffline`: Masks your chat status to appear offline to your friends list. *(Note: While the League client may still show you as online, your status will appear offline to others — this is a client-side visual bug.)*

## Usage

Before running the application, make sure you have the [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download) installed. If not, download and install it from the link.

### Windows
1. Simply run the `league-patch-collection.exe` file.

### macOS
macOS can be pretty restrictive with apps outside of the App Store, so follow these steps to get the app running:

1. Open Terminal and navigate to the folder where you downloaded the app:
    ```bash
    cd ~/Downloads
    ```
2. Make the file executable:
    ```bash
    chmod +x mac-league-patch-collection
    ```
3. Because macOS treats anything not from the App Store like it's a virus (thanks, Gatekeeper), you need to disable Gatekeeper:
    ```bash
    sudo spctl --master-disable
    ```
4. Once Gatekeeper is disabled, you can run the app:
    ```bash
    ./mac-league-patch-collection
    ```

If you want, you can turn Gatekeeper back on with a simple command:
```bash
sudo spctl --master-enable
```
## See also

If you enjoy this project, you might also like my [League Client Debloater](https://github.com/Cat1Bot/LeagueClientDebloater). It works well alongside this tool to provide an even cleaner and more optimized League Client experience by:

- **Removing unnecessary bloat** from the League Client.
- **Improving overall performance** and responsiveness.
