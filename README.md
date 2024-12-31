# League Patch Collection – Vanguard Disabler & QOL Optimizer for League Client

**League Patch Collection** is a lightweight C# application designed for macOS and Windows to enhance your League of Legends experience. This tool introduces several quality-of-life features and optimizations League Client experience. See command line arguments additional options.

## Features

By default, this app provides the following enhancements:
- :white_check_mark: **Vanguard Bypass**: Access full functionality of the League Client and use Kbot without risk of being banned. You may also play matches without getting kicked (Vanguard Event), ensuring your privacy remains protected. To disable this feature, run the app with the --usevgk argument.
- :white_check_mark: **Appear offline**: Option to mask your status to appear offline to your friends list. They cannot invite you, however, you can still invite them. Just type `0` in console to appear offline and `1` to appear online again.
- :white_check_mark: **Removal of Bloatware**: Removes the Legends of Runeterra (LoR) button and Info Hub.
- :white_check_mark: **Streamlined Interface**: Eliminates promotions and other unnecessary clutter from the client.
- :white_check_mark: **Ban Reason Fix**: Resolves issues where the ban reason doesn't display on certain accounts, fixing the infinite loading/unknown player bug.
- :white_check_mark: **Enhanced Privacy**: Disables all tracking and telemetry services, including Sentry, to reduce tracking and prevent unnecessary background activity.
- :white_check_mark: **Home Hub Fix**: Fixes home hubs taking longer than usual to load issue.
- :information_source: **[Coming Soon] Lobby Revealer**: A feature to reveal names in champ select.

## Command-Line Arguments

You can enhance the functionality further by using command-line arguments when running the app. Here's how to use them:

### Arguments
- `--usevgk`:  Disables vanguard bypass feature.
- `--legacyhonor`: Reverts the honor system to its pre-patch 14.9 state, removing the option to honor enemies.

### Running with Arguments

#### Windows:
You can use a shortcut to easily run the app with arguments:

1. Right-click the `league-patch-collection.exe` file and select **Create Shortcut**.
2. Right-click the newly created shortcut and select **Properties**.
3. In the **Target** field, add the desired argument(s) at the end. For example: "C:\Path\To\league-patch-collection.exe" --usevgk --legacyhonor
4. Click **OK** to save the changes.
5. Use this shortcut to run the app with your specified arguments.

#### macOS:
1. Open Terminal.
2. Navigate to the folder where the app is located:
    ```bash
    cd ~/Downloads
    ```
3. Run the app with the desired argument. For example:
    ```bash
    ./mac-league-patch-collection --legacyhonor
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
## Pull requests needed

Pull requests are always welcome. Also, if anyone knows a library or easy way to decode Riot's rtmp (they use action message format) please contact me **c4t_bot** on Discord. I cannot find any c# libraries for decoding AMF0, AMF3 which is needed to proxy RTMP for lobby revealer. For reference here is a [good article](https://web-xbaank.vercel.app/blog/Reversing-engineering-lol) that describes how a lobby revealer works.
