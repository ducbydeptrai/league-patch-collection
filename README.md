# League Patch Collection - A Quality-of-Life Tweaker for macOS and Windows

**League Patch Collection** is a lightweight C# application designed for macOS and Windows to enhance your League of Legends experience. This tool introduces several quality-of-life features and optimizations League Client experience.

## Features

By default, this app provides the following enhancements:
- **Removal of Bloatware**: Removes the Legends of Runeterra (LoR) button and Info Hub.
- **Streamlined Interface**: Eliminates promotions and other unnecessary clutter from the client.
- **Ban Reason Fix**: Resolves issues where the ban reason doesn't display on certain accounts, fixing the infinite loading/unknown player bug.
- **Enhanced Privacy**: Disables all tracking and telemetry services, including Sentry, to reduce tracking and prevent unnecessary background activity.
- **Home Hub Fix**: Fixes home hubs taking longer than usual to load issue.
- **[Coming Soon] Lobby Revealer**: A feature to reveal names in champ select.

## Command-Line Arguments

You can enhance the functionality further by using command-line arguments when running the app. Here's how to use them:

### Arguments
- `--novgk`:  Disables Vanguard enforcement/VAN Errors and stops `vgc.exe` from initializing, which can be useful if you want to use blacklisted tools like Kbot Ext without getting banned. **Note**: This is not a Vanguard bypass; you can fully stop Vanguard by running `sc stop vgk` command or uninstalling Vanguard but you will still be kicked from matches if Vanguard is required to play by Riot’s backend.
- `--legacyhonor`: Reverts the honor system to its pre-patch 14.9 state, removing the option to honor enemies.
- `--appearoffline`: Masks your chat status to appear offline to your friends list. *(Note: While the League client may still show you as online, your status will appear offline to others — this is a client-side visual bug.)*

### Running with Arguments

#### Windows:
You can use a shortcut to easily run the app with arguments:

1. Right-click the `league-patch-collection.exe` file and select **Create Shortcut**.
2. Right-click the newly created shortcut and select **Properties**.
3. In the **Target** field, add the desired argument(s) at the end. For example: "C:\Path\To\league-patch-collection.exe" --novgk --legacyhonor
4. Click **OK** to save the changes.
5. Use this shortcut to run the app with your specified arguments.

#### macOS:
1. Open Terminal.
2. Navigate to the folder where the app is located:
    ```bash
    cd ~/Downloads
    ```
3.Run the app with the desired argument. For example:
    ```bash
    ./mac-league-patch-collection --appearoffline
    ```

## Installation and Usage

Before running the application, make sure you have the [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download) installed. If not, download and install it from the link.

### Windows
Run the league-patch-collection.exe file directly, or use the shortcut method (as described above) to apply arguments

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
