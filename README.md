# league-patch-collection
A small c# app for MacOS and Windows that modifies client config values for LCU backend and Riot Client to remove bloat and improve qol features such as: 
* Disable vanguard enforcement and remove depenency from Riot Client.
* Remove lor button.
* Restore classic honor system.
* Remove promos and other crap from store.
* Fix issue where ban reason doesnt show on some accounts (fixes infinite loading/unknown player bug).
* Disables sentry and telemtry services.
* Fixe home hubs loading issue.

Addiontially you can fork the repo and add your own config flags.

# Usage
* Make sure you have net 8.0 SDK installed or it wont work. If not, you can get it here: https://dotnet.microsoft.com/en-us/download
* On Windows just run **league-patch-collection.exe**
* On MacOS, open terminal and cd to the folder you downloaded it to, then run commands in this order:
1. `cd ~/Downloads`
2. `chmod +x mac-league-patch-collection`
3. Because Apple is fucking stupid and says everything is a virus, you have to disable gatekeeper with this command: `sudo spctl --master-disable`
4. `./mac-league-patch-collection`

# Credit
This app uses unproductives League Proxy lib forked to support MacOS. All I changed in that repo was RiotClient.cs to check for Mac file paths, you can track track this pull request here: https://github.com/User344/LeagueProxyLib/issues/1
