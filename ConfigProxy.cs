using EmbedIO;
using System.Text.Json;
using System.Text.Json.Nodes;


namespace LeaguePatchCollection;

class App
{
    static (string, object)[] VanguardConfigFlags = {
        ("anticheat.vanguard.backgroundInstall", false),
        ("anticheat.vanguard.enabled", false),
        ("keystone.client.feature_flags.restart_required.disabled", true),
        ("keystone.client.feature_flags.vanguardLaunch.disabled", true),
        ("lol.client_settings.vanguard.enabled", false),
        ("lol.client_settings.vanguard.enabled_mac", false),
        ("lol.client_settings.vanguard.url", "")
    };
    static (string, object)[] OptimizeClientConfigPublic = {
        ("keystone.age_restriction.enabled", false),
        ("keystone.client.feature_flags.lifecycle.backgroundRunning.enabled", false),
        ("keystone.client.feature_flags.arcane_event.enabled", false),
        ("keystone.client.feature_flags.arcane_event_live.enabled", false),
        ("keystone.client.feature_flags.arcane_event_prelaunch.enabled", false),
        ("keystone.client.feature_flags.arcane_event_premier.enabled", false),
        ("keystone.client.feature_flags.arcane_theme.enabled", false),
        ("keystone.client.feature_flags.autoPatch.disabled", true),
        ("keystone.client.feature_flags.background_mode_patching.enabled", false),
        ("keystone.client.feature_flags.cpu_memory_warning_report.enabled", false),
        ("keystone.client.feature_flags.dismissible_name_change_modal.enabled", true),
        ("keystone.client.feature_flags.eula.use_patch_downloader.enabled", false),
        ("keystone.client.feature_flags.flaggedNameModal.disabled", true),
        ("keystone.client.feature_flags.keystone_login_splash_video.enabled", false),
        ("keystone.client.feature_flags.launch_on_computer_start.enabled", false),
        ("keystone.client.feature_flags.mfa_notification.enabled", false),
        ("keystone.client.feature_flags.open_telemetry_sender.enabled", false),
        ("keystone.client.feature_flags.penaltyNotifications.enabled", false),
        ("keystone.client.feature_flags.pending_consent_modal.enabled", false),
        ("keystone.client.feature_flags.pending_forget_modal.enabled", false),
        ("keystone.client.feature_flags.playerReportingMailboxIntegration.enabled", false),
        ("keystone.client.feature_flags.playerReportingPasIntegration.enabled", false),
        ("keystone.client.feature_flags.playerReportingReporterFeedback.enabled", false),
        ("keystone.client.feature_flags.privacyPolicy.enabled", false),
        ("keystone.client.feature_flags.product_update_scanner.enabled", false),
        ("keystone.client.feature_flags.qrcode_modal.enabled", false),
        ("keystone.client.feature_flags.quick_actions.enabled", true),
        ("keystone.client.feature_flags.regionlessLoginInfoTooltip.enabled", false),
        ("keystone.client.feature_flags.riot_id_required_modal.enabled", false),
        ("keystone.client.feature_flags.riot_mobile_special_event.enabled", false),
        ("keystone.client.feature_flags.self_update_in_background.enabled", false),
        ("keystone.client.feature_flags.username_required_modal.enabled", false),
        ("keystone.client_config.diagnostics_enabled", false),
        ("games_library.special_events.enabled", false),
        //("keystone.player-affinity.playerAffinityServiceURL", "http://127.0.0.1:29150"),
        ("keystone.telemetry.heartbeat_custom_metrics", false),
        ("keystone.riotgamesapi.telemetry.heartbeat_products", false),
        ("keystone.riotgamesapi.telemetry.heartbeat_voice_chat_metrics", false),
        ("keystone.riotgamesapi.telemetry.newrelic_events_v2_enabled", false),
        ("keystone.riotgamesapi.telemetry.newrelic_metrics_v1_enabled", false),
        ("keystone.riotgamesapi.telemetry.newrelic_schemaless_events_v2_enabled", false),
        ("keystone.riotgamesapi.telemetry.opentelemetry_events_enabled", false),
        ("keystone.riotgamesapi.telemetry.opentelemetry_uri_events", ""),
        ("keystone.riotgamesapi.telemetry.singular_v1_enabled", false),
        ("keystone.rso-mobile-ui.accountCreationTosAgreement", false),
        ("keystone.telemetry.heartbeat_products", false),
        ("keystone.telemetry.heartbeat_voice_chat_metrics", false),
        ("keystone.telemetry.send_error_telemetry_metrics", false),
        ("keystone.telemetry.send_product_session_start_metrics", false),
        ("keystone.telemetry.singular_v1_enabled", false),
        ("lol.client_settings.clash.eosCelebrationEnabled", false),
        ("lol.client_settings.missions.upsell_opens_event_hub", false),
        ("lol.client_settings.client_navigability.info_hub_disabled", true),
        ("lol.client_settings.paw.enableRPTopUp", false),
        ("lol.client_settings.remedy.is_verbal_abuse_remedy_modal_enabled", false),
        ("lol.client_settings.startup.should_show_progress_bar_text", false),
        ("lol.client_settings.startup.should_wait_for_home_hubs", false),
        ("lol.client_settings.store.hidePurchaseModalQuantityControl", true),
        ("lol.game_client_settings.app_config.singular_enabled", false),
        ("lol.game_client_settings.low_memory_reporting_enabled", false),
        ("lol.game_client_settings.missions.enabled", false),
        ("patcher.scd.service_enabled", false),
        ("lol.game_client_settings.cap_orders_metrics_enabled", false),
        ("lol.game_client_settings.platform_stats_enabled", false),
        ("lol.game_client_settings.telemetry.standalone.long_frame_cooldown", (object)999),
        ("lol.game_client_settings.telemetry.standalone.long_frame_min_time", (object)99999),
        ("lol.game_client_settings.telemetry.standalone.nr_sample_rate", (object)0),
        ("lol.game_client_settings.telemetry.standalone.sample_rate", (object)0),
        ("riot.eula.agreementBaseURI", ""),
        ("rms.allow_bad_cert.enabled", true)
    };

    static (string, object)[] ClientConfigPlayer = {
        ("chat.allow_bad_cert.enabled", true),
        ("chat.host", "127.0.0.1"),
        ("chat.port", (object)29152),
        ("chat.use_tls.enabled", false),
        ("chat.disable_chat_restriction_muted_system_message", true),
        ("chat.force_filter.enabled", false),
        ("keystone.client.feature_flags.chrome_devtools.enabled", true),
        ("keystone.client.feature_flags.campaign-hub.enabled", false),
        ("keystone.riotgamesapi.telemetry.endpoint.send_deprecated", false),
        ("keystone.riotgamesapi.telemetry.endpoint.send_failure", false),
        ("keystone.riotgamesapi.telemetry.endpoint.send_success", false),
        ("keystone.client.feature_flags.home_page_route.enabled", false),
        ("keystone.telemetry.metrics_enabled", false),
        ("keystone.telemetry.newrelic_events_v2_enabled", false),
        ("keystone.telemetry.newrelic_metrics_v1_enabled", false),
        ("keystone.telemetry.newrelic_schemaless_events_v2_enabled", false),
        //("lol.client_settings.league_edge.url", "http://127.0.0.1:29151"),
        ("lol.client_settings.metrics.enabled", false),
        ("lol.client_settings.player_behavior.display_v1_ban_notifications", true),
        ("lol.client_settings.player_behavior.use_reform_card_v2", false),
        ("lol.game_client_settings.logging.enable_http_public_logs", false),
        ("lol.game_client_settings.logging.enable_rms_public_logs", false)
    };

    public static async Task Main(string[] args)
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("=========================================");
        Console.WriteLine("  Welcome to League Patch Collection");
        Console.WriteLine("    Made with <3 by Cat Bot");
        Console.WriteLine("=========================================");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Important: IF YOU PAID FOR THIS, YOU GOT SCAMMED!");
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("--------------------------------------------");
        Console.WriteLine("Contact me:");
        Console.WriteLine(" Discord : c4t_bot");
        Console.WriteLine(" Reddit  : u/Cat_Bot4");
        Console.WriteLine("--------------------------------------------");
        Console.ResetColor();

        bool usevgk = args.Contains("--usevgk");
        bool legacyhonor = args.Contains("--legacyhonor");

        var leagueProxy = new LeagueProxy();

        if (!usevgk)
        {
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("=========================================");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Vanguard bypass is active. You can now use Kbot or other blacklisted apps without risk of being banned. If you havent already, uninstall vgk");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("IMPORTANT");
            Console.WriteLine("To avoid getting kicked in game (Vanguard Event), use Kbot or other tool to clear logs every 1-3 games and sign back in.");
            Console.WriteLine("Doing this will reset your limit on the backend for how many games the server lets you play without Vanguard.");
            Console.WriteLine("Rinse and Repeat!");
            Console.ResetColor();
            Console.WriteLine("--------------------------------------------");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("launch this app with --usevgk to NOT use the Vanguard bypass.");
            Console.ResetColor();
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("There are other features in this app like showing offline,");
            Console.WriteLine("using the old honor system, and bloatware removal that you'll benefit from.");
            Console.WriteLine("=========================================");
            Console.ResetColor();
        }
        else if (usevgk)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Vanguard enforcement is enabled. Just remember: privacy is a myth.");
            Console.WriteLine("Looks like you’ve volunteered to be spied on. Hope they enjoy your data!");
            Console.WriteLine("---------------------------------------");
            Console.ResetColor();
        }
        if (!legacyhonor)
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Use --legacyhonor to opt out of the cringe new honor system introduced in patch 14.9.");
            Console.WriteLine("--------------------------------------------");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("Congratulations! You’ve kept your sanity and opted out of honoring enemies.");
            Console.WriteLine("--------------------------------------------");
            Console.ResetColor();
        }

        leagueProxy.Events.OnProcessConfigPublic += (string content, IHttpRequest request) =>
        {
            var configObject = JsonSerializer.Deserialize<JsonNode>(content);

            if (!usevgk)
            {
                DisableVanguard(configObject);
            }
            if (legacyhonor)
            {
                LegacyHonor(configObject);
            }
            PublicConfig(configObject);

            return JsonSerializer.Serialize(configObject);
        };

        leagueProxy.Events.OnProcessConfigPlayer += (string content, IHttpRequest request) =>
        {
            var configObject = JsonSerializer.Deserialize<JsonNode>(content);


            var leagueEdgeUrlNode = configObject?["lol.client_settings.league_edge.url"];
            if (leagueEdgeUrlNode != null)
            {
                SharedLeagueEdgeUrl.Set(leagueEdgeUrlNode.ToString());
            }

            var chatHostNode = configObject?["chat.host"];
            if (chatHostNode != null)
            {
                SharedChatHost.Set(chatHostNode.ToString());
            }

            PlayerConfig(configObject);

            return JsonSerializer.Serialize(configObject);
        };

        leagueProxy.Events.OnProcessLedge += (string content, IHttpRequest request) =>
        {

            if (request.Url.LocalPath == "/leaverbuster-ledge/restrictionInfo")
            {

                var configObject = JsonSerializer.Deserialize<JsonNode>(content);

                if (configObject["rankedRestrictionEntryDto"] != null)
                {
                    configObject["rankedRestrictionEntryDto"]["rankedRestrictionAckNeeded"] = false;
                }

                content = JsonSerializer.Serialize(configObject);

                Console.WriteLine("Modified content: " + content);
            }

            return content;
        };

        var process = leagueProxy.StartAndLaunchRCS();
        if (process is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failed to start Riot Client. This may be due to a change on Riot's end. Please ensure you're using the latest version by checking https://github.com/Cat1Bot/league-patch-collection/releases. If the issue persists even with the latest version, contact c4t_bot on Discord for further assistance.");
            Console.ResetColor();
            leagueProxy.Stop();
            return;
        }

        var ChatProxy = new XMPPProxy();
        var RtmpProxy = new RTMPProxy();

        var proxyTasks = Task.WhenAll(ChatProxy.RunAsync(), RtmpProxy.RunAsync());

        await Task.WhenAny(process.WaitForExitAsync(), proxyTasks);
        leagueProxy.Stop();
    }

    private static void SetEmptyArrayForConfig(JsonNode? configObject, string configKey)
    {
        if (configObject?[configKey] is JsonArray)
        {
            configObject[configKey] = new JsonArray();  // Set to empty array
        }
        else if (configObject?[configKey] is JsonObject jsonObject)
        {
            jsonObject[configKey] = new JsonArray(); 
        }
    }

    static void SetConfig(JsonNode? configObject, string parentKey, string childKey, bool value)
    {
        if (configObject == null) return;

        if (configObject[parentKey] is JsonNode parentNode)
        {
            parentNode[childKey] = value;
        }
    }
    private static void SetConfigValues(JsonNode? configObject, (string, object)[] configValues)
    {
        foreach (var (key, value) in configValues)
        {
            SetConfig(configObject, key, value);
        }
    }
    private static void SetConfig(JsonNode? configObject, string key, object value)
    {
        if (configObject?[key] is not null)
        {
            switch (value)
            {
                case bool boolValue:
                    configObject[key] = boolValue;
                    break;
                case string stringValue:
                    configObject[key] = stringValue;
                    break;
                case double doubleValue:
                    configObject[key] = doubleValue;
                    break;
                case int intValue:
                    configObject[key] = intValue;
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported type: {value.GetType()}");
            }
        }
    }
    private static void SetConfig(JsonNode? configObject, string parentKey, string childKey, object value)
    {
        if (configObject?[parentKey] is JsonNode parentNode)
        {
            switch (value)
            {
                case bool boolValue:
                    parentNode[childKey] = boolValue;
                    break;
                case string stringValue:
                    parentNode[childKey] = stringValue;
                    break;
                case double doubleValue:
                    parentNode[childKey] = doubleValue;
                    break;
                case int intValue:
                    parentNode[childKey] = intValue;
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported type: {value.GetType()}");
            }
        }
    }
    public static void DisableVanguard(JsonNode? configObject)
    {
        SetConfigValues(configObject, VanguardConfigFlags);
        RemoveVanguardDependencies(configObject, "keystone.products.league_of_legends.patchlines.live");
        RemoveVanguardDependencies(configObject, "keystone.products.league_of_legends.patchlines.pbe");
        RemoveVanguardDependencies(configObject, "keystone.products.valorant.patchlines.live");
    }
    public static void LegacyHonor(JsonNode? configObject)
    {
        SetConfig(configObject, "lol.client_settings.honor", "CeremonyV3Enabled", false);
        SetConfig(configObject, "lol.client_settings.honor", "Enabled", true);
        SetConfig(configObject, "lol.client_settings.honor", "HonorEndpointsV2Enabled", false);
        SetConfig(configObject, "lol.client_settings.honor", "HonorSuggestionsEnabled", true);
        SetConfig(configObject, "lol.client_settings.honor", "HonorVisibilityEnabled", true);
        SetConfig(configObject, "lol.client_settings.honor", "SecondsToVote", 90);
    }
    public static void PublicConfig(JsonNode? configObject)
    {
        SetConfigValues(configObject, OptimizeClientConfigPublic);
        //AppendLauncherArgumentsWin(configObject, "keystone.products.league_of_legends.patchlines.live");
        //AppendLauncherArgumentsWin(configObject, "keystone.products.league_of_legends.patchlines.pbe");
        //AppendLauncherArgumentsMac(configObject, "keystone.products.league_of_legends.patchlines.live");
        //AppendLauncherArgumentsMac(configObject, "keystone.products.league_of_legends.patchlines.pbe");
        SetConfig(configObject, "lol.client_settings.datadog_rum_config", "applicationID", "");
        SetConfig(configObject, "lol.client_settings.datadog_rum_config", "clientToken", "");
        SetConfig(configObject, "lol.client_settings.datadog_rum_config", "isEnabled", false);
        SetConfig(configObject, "lol.client_settings.datadog_rum_config", "service", "");
        SetConfig(configObject, "lol.client_settings.datadog_rum_config", "sessionReplaySampleRate", 0);
        SetConfig(configObject, "lol.client_settings.datadog_rum_config", "sessionSampleRate", 0);
        SetConfig(configObject, "lol.client_settings.datadog_rum_config", "site", "");
        SetConfig(configObject, "lol.client_settings.datadog_rum_config", "telemetrySampleRate", 0);
        SetConfig(configObject, "lol.client_settings.datadog_rum_config", "traceSampleRate", 0);
        SetConfig(configObject, "lol.client_settings.datadog_rum_config", "trackLongTasks", false);
        SetConfig(configObject, "lol.client_settings.datadog_rum_config", "trackResources", false);
        SetConfig(configObject, "lol.client_settings.datadog_rum_config", "trackUserInteractions", false);
        SetConfig(configObject, "lol.client_settings.sentry_config", "isEnabled", false);
        SetConfig(configObject, "lol.client_settings.sentry_config", "sampleRate", 0);
        SetConfig(configObject, "lol.client_settings.sentry_config", "dsn", "");
    }
    public static void PlayerConfig(JsonNode? configObject)
    {
        SetConfigValues(configObject, ClientConfigPlayer);
        chatAffinity(configObject);
        NoLoyalty(configObject);
        SetConfig(configObject, "lol.client_settings.deepLinks", "launchLorEnabled", false);
        SetEmptyArrayForConfig(configObject, "chat.xmpp_stanza_response_telemetry_allowed_codes");
        SetEmptyArrayForConfig(configObject, "chat.xmpp_stanza_response_telemetry_allowed_iqids");
    }


    public static class SharedLeagueEdgeUrl
    {
        public static string? _leagueEdgeUrl;

        public static string? Get()
        {
            return _leagueEdgeUrl;
        }

        public static void Set(string url)
        {
            _leagueEdgeUrl = url;
        }
    }
    public static class SharedChatHost
    {
        public static string? _chatHost;

        public static string? Get()
        {
            return _chatHost;
        }

        public static void Set(string host)
        {
            _chatHost = host;
        }
    }

    public static class SharedChatHostUrl
    {
        public static string? _chatHostUrl;

        public static string? Get()
        {
            return _chatHostUrl;
        }

        public static void Set(string url)
        {
            _chatHostUrl = url;
        }
    }

    static void chatAffinity(JsonNode? configObject)
    {
        if (configObject?["chat.affinities"] is JsonObject affinities)
        {
            var keys = affinities.Select(entry => entry.Key).ToArray();
            foreach (var key in keys)
            {
                affinities[key] = "127.0.0.1";
            }
        }
    }

    static void NoLoyalty(JsonNode? configObject)
    {
        if (configObject?["keystone.loyalty.config"] is JsonObject loyaltyConfig)
        {
            foreach (var region in loyaltyConfig)
            {
                if (region.Value is JsonObject regionConfig && regionConfig.ContainsKey("enabled"))
                {
                    regionConfig["enabled"] = false;
                }
            }
        }
    }

    static void AppendLauncherArgumentsWin(JsonNode configObject, string patchline)
    {
        var productNode = configObject?[patchline];
        if (productNode is not null)
        {
            var configs = productNode["platforms"]?["win"]?["configurations"]?.AsArray();
            if (configs != null)
            {
                foreach (var config in configs)
                {
                    var launcherArray = config["launcher"]?["arguments"]?.AsArray();
                    if (launcherArray is not null)
                    {
                        launcherArray.Add("--system-yaml-override=\"Config/system.yaml\"");
                    }
                }
            }
        }
    }

    static void AppendLauncherArgumentsMac(JsonNode configObject, string patchline)
    {
        var productNode = configObject?[patchline];
        if (productNode is not null)
        {
            var configs = productNode["platforms"]?["mac"]?["configurations"]?.AsArray();
            if (configs != null)
            {
                foreach (var config in configs)
                {
                    var launcherArray = config["launcher"]?["arguments"]?.AsArray();
                    if (launcherArray is not null)
                    {
                        launcherArray.Add("--system-yaml-override=\"Config/system.yaml\"");
                    }
                }
            }
        }
    }

    static void RemoveVanguardDependencies(JsonNode configObject, string path)
    {
        var productNode = configObject?[path];
        if (productNode is not null)
        {
            var configs = productNode["platforms"]?["win"]?["configurations"]?.AsArray();
            if (configs is not null)
            {
                foreach (var config in configs)
                {
                    var dependencies = config["dependencies"]?.AsArray();
                    var vanguard = dependencies?.FirstOrDefault(x => x!["id"]!.GetValue<string>() == "vanguard");
                    if (vanguard is not null)
                    {
                        dependencies.Remove(vanguard);
                    }
                }
            }
        }
    }
}
