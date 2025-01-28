![image](https://github.com/user-attachments/assets/fd5dc899-3a23-4445-9452-3deeb4e5c348)
 
# League Patch Collection – Vanguard Disabler & QOL Optimizer for League Client

**League Patch Collection** is a lightweight C# application designed for Windows to enhance your League of Legends experience. This tool introduces several quality-of-life features and optimizations League Client experience. See command line arguments additional options.

>  Unlike most third party tools, this app does NOT use the Riot Client or LCU api. Instead it hooks into backend protocals like XMPP (chat), Websocket (RMS) and https (Ledge).

## Features

> [!WARNING]
> As of January 9th this play without vanguard bypass has been **partially patched**. If you try to play without Vanguard, **you may get kicked (Vanguard Event) and get leaver busted**.

By default, this app provides the following enhancements:
- :white_check_mark: **Vanguard Disabler**: Access full functionality of the League Client and use Kbot without risk of being banned. NOTE: You may not be able to actually play matches without VGK, you may get kicked (Vanguard Event) and get leaver busted.
- :white_check_mark: **Show Legacy Patch Numbers**: Reverts the version format change in season 15. For example, instead seeing 25.S1.1 youll see 15.1 and so on.
- :white_check_mark: **Legacy Honor**: Restores the old post game honor screen pre patch 14.19 where you can only only one teammate. Honoring enemies is cringe.
- :white_check_mark: **Log Cleaner**: Clean all client and account logs with the click of a button.
- :white_check_mark: **Appear offline**: Option to mask your status to appear offline to your friends list. They cannot invite you, however, you can still invite them. You can also mask your status to show as Riot Mobile as well.
- :white_check_mark: **Hawolt ban bypass**: Proof of concept exploit that may or may not actually work, basically this will block RMS session notifications delaying the amount of time it takes for session to expire. [More info](https://web.archive.org/web/20230628125118/https://twitter.com/hawolt/status/1674029547363217410) about this exploit.
- :white_check_mark: **Removal of Bloatware**: Removes the Legends of Runeterra (LoR) button, Info Hub and suppresses some behavior warnings (ranked restrictions and so on).
- :white_check_mark: **Streamlined Interface**: Eliminates promotions and other unnecessary clutter from the client.
- :white_check_mark: **Ban Reason Fix**: Resolves issues where the ban reason doesn't display on certain accounts, fixing the infinite loading/unknown player bug.
- :white_check_mark: **Enhanced Privacy**: Disables all tracking and telemetry services, including Sentry, to reduce tracking and prevent unnecessary background activity.
- :white_check_mark: **Home Hub Fix**: Fixes home hubs taking longer than usual to load issue.
- :information_source: **[Coming Soon] Lobby Revealer**: A feature to reveal names in champ select.

##  Usage

Before running the application, ensure that you have the [.NET 8.0 SDK](https://dotnet.microsoft.com/en-us/download) installed.

You can either use the precompiled executable available in the releases section or clone this repository using Visual Studio. To manually build the project, run the following command in the terminal:
```bash   
dotnet publish "C:\path\to\league-patch-collection.csproj" -c Release -r win-x64 --self-contained false /p:PublishSingleFile=true
```
Replace C:\path\to\LeaguePatchCollection.csproj with the actual path to the project file.

## Pull requests needed

Pull requests are always welcome. Also, if anyone knows a library or easy way to decode Riot's rtmp (they use action message format) please contact me **c4t_bot** on Discord. I cannot find any c# libraries for decoding AMF0, AMF3 which is needed to proxy RTMP for lobby revealer. For reference here is a [good article](https://web-xbaank.vercel.app/blog/Reversing-engineering-lol) that describes how a lobby revealer works.
