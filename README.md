# League Patch Collection - A QoL Tweaker for MacOS and Windows
A small c# app for MacOS and Windows that modifies client config values for LCU backend and Riot Client to remove bloat and improve qol features such as: 
* Disable vanguard enforcement and remove depenency from Riot Client.
* Removes Arcane crap from Riot Client.
* Remove lor button.
* Remove promos and other crap from store.
* Fix issue where ban reason doesnt show on some accounts (fixes infinite loading/unknown player bug).
* Disables sentry and telemtry services.
* Fixes home hubs loading issue.
* [coming soon] Disables RMS (Riot Messaging service) - this will delay account bans since the client wont know the session state has changed so you can keep playing until your JWT expires.

*Addiontially, you can fork the repo and add your own config flags.*

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
## Credit

This app utilizes [unproductive's LeagueProxyLib](https://github.com/user344/LeagueProxyLib), which I forked to add support for macOS. The only modification made was in `RiotClient.cs`, where I adjusted file path checks for macOS compatibility.

You can view and track the related pull request here: [LeagueProxyLib Pull Request #1](https://github.com/User344/LeagueProxyLib/issues/1).

## See also

If you enjoy this project, you might also like my [League Client Debloater](https://github.com/Cat1Bot/LeagueClientDebloater). It works well alongside this tool to provide an even cleaner and more optimized League Client experience by:

- **Removing unnecessary bloat** from the League Client.
- **Improving overall performance** and responsiveness.
