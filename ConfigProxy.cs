using EmbedIO;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace LeaguePatchCollection;

class App
{
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

        leagueProxy.Events.OnClientConfigPublic += (string content, IHttpRequest request) =>
        {
            var configObject = JsonSerializer.Deserialize<JsonNode>(content);

            var GeopassUrlNode = configObject?["keystone.player-affinity.playerAffinityServiceURL"];
            if (GeopassUrlNode != null)
            {
                SharedGeopassUrl.Set(GeopassUrlNode.ToString());
            }

            if (!usevgk)
            {
                DisableVanguard(configObject);
            }
            if (legacyhonor)
            {
                LegacyHonor(configObject);
            }

            SetKey(configObject, "keystone.age_restriction.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.lifecycle.backgroundRunning.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.arcane_event.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.arcane_event_live.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.arcane_event_prelaunch.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.arcane_event_premier.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.arcane_theme.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.autoPatch.disabled", true);
            SetKey(configObject, "keystone.client.feature_flags.background_mode_patching.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.cpu_memory_warning_report.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.dismissible_name_change_modal.enabled", true);
            SetKey(configObject, "keystone.client.feature_flags.eula.use_patch_downloader.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.flaggedNameModal.disabled", true);
            SetKey(configObject, "keystone.client.feature_flags.keystone_login_splash_video.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.launch_on_computer_start.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.mfa_notification.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.open_telemetry_sender.enabled", false); 
            SetKey(configObject, "keystone.client.feature_flags.pcbang_vanguard_restart_bypass.disabled", true);
            SetKey(configObject, "keystone.client.feature_flags.penaltyNotifications.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.pending_consent_modal.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.pending_forget_modal.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.playerReportingMailboxIntegration.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.playerReportingPasIntegration.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.playerReportingReporterFeedback.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.privacyPolicy.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.product_update_scanner.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.qrcode_modal.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.quick_actions.enabled", true);
            SetKey(configObject, "keystone.client.feature_flags.regionlessLoginInfoTooltip.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.riot_id_required_modal.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.riot_mobile_special_event.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.self_update_in_background.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.username_required_modal.enabled", false);
            SetKey(configObject, "keystone.client_config.diagnostics_enabled", false);
            SetKey(configObject, "games_library.special_events.enabled", false);
            SetKey(configObject, "keystone.player-affinity.playerAffinityServiceURL", "http://127.0.0.1:29151");
            SetKey(configObject, "keystone.telemetry.heartbeat_custom_metrics", false);
            SetKey(configObject, "keystone.riotgamesapi.telemetry.heartbeat_products", false);
            SetKey(configObject, "keystone.riotgamesapi.telemetry.heartbeat_voice_chat_metrics", false);
            SetKey(configObject, "keystone.riotgamesapi.telemetry.newrelic_events_v2_enabled", false);
            SetKey(configObject, "keystone.riotgamesapi.telemetry.newrelic_metrics_v1_enabled", false);
            SetKey(configObject, "keystone.riotgamesapi.telemetry.newrelic_schemaless_events_v2_enabled", false);
            SetKey(configObject, "keystone.riotgamesapi.telemetry.opentelemetry_events_enabled", false);
            SetKey(configObject, "keystone.riotgamesapi.telemetry.opentelemetry_uri_events", "");
            SetKey(configObject, "keystone.riotgamesapi.telemetry.singular_v1_enabled", false);
            SetKey(configObject, "keystone.rso-mobile-ui.accountCreationTosAgreement", false);
            SetKey(configObject, "keystone.telemetry.heartbeat_products", false);
            SetKey(configObject, "keystone.telemetry.heartbeat_voice_chat_metrics", false);
            SetKey(configObject, "keystone.telemetry.send_error_telemetry_metrics", false);
            SetKey(configObject, "keystone.telemetry.send_product_session_start_metrics", false);
            SetKey(configObject, "keystone.telemetry.singular_v1_enabled", false);
            SetKey(configObject, "lol.client_settings.clash.eosCelebrationEnabled", false);
            SetKey(configObject, "lol.client_settings.missions.upsell_opens_event_hub", false);
            SetKey(configObject, "lol.client_settings.client_navigability.info_hub_disabled", true);
            SetKey(configObject, "lol.client_settings.paw.enableRPTopUp", false);
            SetKey(configObject, "lol.client_settings.remedy.is_verbal_abuse_remedy_modal_enabled", false);
            SetKey(configObject, "lol.client_settings.startup.should_show_progress_bar_text", false);
            SetKey(configObject, "lol.client_settings.startup.should_wait_for_home_hubs", false);
            SetKey(configObject, "lol.client_settings.store.hidePurchaseModalQuantityControl", true);
            SetKey(configObject, "lol.game_client_settings.app_config.singular_enabled", false);
            SetKey(configObject, "lol.game_client_settings.low_memory_reporting_enabled", false);
            SetKey(configObject, "lol.game_client_settings.missions.enabled", false);
            SetKey(configObject, "patcher.scd.service_enabled", false);
            SetKey(configObject, "lol.game_client_settings.cap_orders_metrics_enabled", false);
            SetKey(configObject, "lol.game_client_settings.platform_stats_enabled", false);
            SetKey(configObject, "lol.game_client_settings.telemetry.standalone.long_frame_cooldown", 999);
            SetKey(configObject, "lol.game_client_settings.telemetry.standalone.long_frame_min_time", 99999);
            SetKey(configObject, "lol.game_client_settings.telemetry.standalone.nr_sample_rate", 0);
            SetKey(configObject, "lol.game_client_settings.telemetry.standalone.sample_rate", 0);
            SetKey(configObject, "riot.eula.agreementBaseURI", "");
            SetKey(configObject, "rms.host", "ws://127.0.0.1");
            SetKey(configObject, "rms.port", 29155);
            SetKey(configObject, "rms.allow_bad_cert.enabled", true);

            SetNestedKeys(configObject, "lol.client_settings.datadog_rum_config", "applicationID", "");
            SetNestedKeys(configObject, "lol.client_settings.datadog_rum_config", "clientToken", "");
            SetNestedKeys(configObject, "lol.client_settings.datadog_rum_config", "isEnabled", false);
            SetNestedKeys(configObject, "lol.client_settings.datadog_rum_config", "service", "");
            SetNestedKeys(configObject, "lol.client_settings.datadog_rum_config", "sessionReplaySampleRate", 0);
            SetNestedKeys(configObject, "lol.client_settings.datadog_rum_config", "sessionSampleRate", 0);
            SetNestedKeys(configObject, "lol.client_settings.datadog_rum_config", "site", "");
            SetNestedKeys(configObject, "lol.client_settings.datadog_rum_config", "telemetrySampleRate", 0);
            SetNestedKeys(configObject, "lol.client_settings.datadog_rum_config", "traceSampleRate", 0);
            SetNestedKeys(configObject, "lol.client_settings.datadog_rum_config", "trackLongTasks", false);
            SetNestedKeys(configObject, "lol.client_settings.datadog_rum_config", "trackResources", false);
            SetNestedKeys(configObject, "lol.client_settings.datadog_rum_config", "trackUserInteractions", false);
            SetNestedKeys(configObject, "lol.client_settings.sentry_config", "isEnabled", false);
            SetNestedKeys(configObject, "lol.client_settings.sentry_config", "sampleRate", 0);
            SetNestedKeys(configObject, "lol.client_settings.sentry_config", "dsn", "");

            //AppendLauncherArgumentsWin(configObject, "keystone.products.league_of_legends.patchlines.live");
            //AppendLauncherArgumentsWin(configObject, "keystone.products.league_of_legends.patchlines.pbe");
            //AppendLauncherArgumentsMac(configObject, "keystone.products.league_of_legends.patchlines.live");
            //AppendLauncherArgumentsMac(configObject, "keystone.products.league_of_legends.patchlines.pbe");

            return JsonSerializer.Serialize(configObject);
        };

        leagueProxy.Events.OnClientConfigPlayer += (string content, IHttpRequest request) =>
        {
            var configObject = JsonSerializer.Deserialize<JsonNode>(content);

            var userRegion = GeopassHandler.GetUserRegion();
            var chatAffinitiesNode = configObject?["chat.affinities"];
            if (chatAffinitiesNode != null && chatAffinitiesNode is JsonObject chatAffinities)
            {
                if (!string.IsNullOrEmpty(userRegion) && chatAffinities[userRegion] is JsonNode tempChatHost)
                {
                    SharedChatHost.Set(tempChatHost.ToString());
                }
                else
                {
                    var chatHostNode = configObject?["chat.host"];
                    if (chatHostNode != null)
                    {
                        SharedChatHost.Set(chatHostNode.ToString());
                    }
                }
            }

            var userRegionRms = GeopassHandlerRms.GetUserRegionRms();
            var RmsAffinitiesNode = configObject?["rms.affinities"];
            if (RmsAffinitiesNode != null && RmsAffinitiesNode is JsonObject rmsAffinities)
            {
                if (!string.IsNullOrEmpty(userRegionRms) && rmsAffinities[userRegionRms] is JsonNode tempRmsHost)
                {
                    SharedRmsHost.Set(tempRmsHost.ToString()); // Use the derived RMS host.
                }
                else
                {
                    SharedRmsHost.Set("wss://unconfigured.edge.rms.si.riotgames.com");
                }
            }


            var leagueEdgeUrlNode = configObject?["lol.client_settings.league_edge.url"];
            if (leagueEdgeUrlNode != null)
            {
                SharedLeagueEdgeUrl.Set(leagueEdgeUrlNode.ToString());
            }

            if (configObject?["chat.affinities"] is JsonObject affinities)
            {
                var keys = affinities.Select(entry => entry.Key).ToArray();
                foreach (var key in keys)
                {
                    affinities[key] = "127.0.0.1";
                }
            }

            if (configObject?["rms.affinities"] is JsonObject RmsAffinities)
            {
                var keys = RmsAffinities.Select(entry => entry.Key).ToArray();
                foreach (var key in keys)
                {
                    RmsAffinities[key] = "ws://127.0.0.1";
                }
            }

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

            SetKey(configObject, "chat.allow_bad_cert.enabled", true);
            SetKey(configObject, "chat.host", "127.0.0.1");
            SetKey(configObject, "chat.port", 29153);
            SetKey(configObject, "chat.use_tls.enabled", false);
            SetKey(configObject, "chat.disable_chat_restriction_muted_system_message", true);
            SetKey(configObject, "chat.force_filter.enabled", false);
            SetKey(configObject, "keystone.client.feature_flags.chrome_devtools.enabled", true);
            SetKey(configObject, "keystone.client.feature_flags.campaign-hub.enabled", false);
            SetKey(configObject, "keystone.riotgamesapi.telemetry.endpoint.send_deprecated", false);
            SetKey(configObject, "keystone.riotgamesapi.telemetry.endpoint.send_failure", false);
            SetKey(configObject, "keystone.riotgamesapi.telemetry.endpoint.send_success", false);
            SetKey(configObject, "keystone.client.feature_flags.home_page_route.enabled", false);
            SetKey(configObject, "keystone.telemetry.metrics_enabled", false);
            SetKey(configObject, "keystone.telemetry.newrelic_events_v2_enabled", false);
            SetKey(configObject, "keystone.telemetry.newrelic_metrics_v1_enabled", false);
            SetKey(configObject, "keystone.telemetry.newrelic_schemaless_events_v2_enabled", false);
            SetKey(configObject, "lol.client_settings.league_edge.url", "http://127.0.0.1:29152");
            SetKey(configObject, "lol.client_settings.metrics.enabled", false);
            SetKey(configObject, "lol.client_settings.player_behavior.display_v1_ban_notifications", true);
            SetKey(configObject, "lol.game_client_settings.logging.enable_http_public_logs", false);
            SetKey(configObject, "lol.game_client_settings.logging.enable_rms_public_logs", false);

            SetNestedKeys(configObject, "lol.client_settings.deepLinks", "launchLorEnabled", false);

            SetEmptyArrayForConfig(configObject, "chat.xmpp_stanza_response_telemetry_allowed_codes");
            SetEmptyArrayForConfig(configObject, "chat.xmpp_stanza_response_telemetry_allowed_iqids");

            return JsonSerializer.Serialize(configObject);
        };

        leagueProxy.Events.OnClientGeopass += (string content, IHttpRequest request) =>
        {
            if (request.Url.LocalPath == "/pas/v1/service/chat")
            {
                GeopassHandler.DecodeAndStoreUserRegion(content); // Pass the content to the handler
            }
            if (request.Url.LocalPath == "/pas/v1/service/rms")
            {
                GeopassHandlerRms.DecodeAndStoreUserRegionRms(content); // Pass the content to the handler
            }
            return content;
        };

        leagueProxy.Events.OnClientLedge += (string content, IHttpRequest request) =>
        {

            if (request.Url.LocalPath == "/leaverbuster-ledge/restrictionInfo")
            {
                var configObject = JsonSerializer.Deserialize<JsonNode>(content);

                SetNestedKeys(configObject, "rankedRestrictionEntryDto", "rankedRestrictionAckNeeded", false);
                SetNestedKeys(configObject, "leaverBusterEntryDto", "preLockoutAckNeeded", false);
                SetNestedKeys(configObject, "leaverBusterEntryDto", "onLockoutAckNeeded", false);
                content = JsonSerializer.Serialize(configObject);
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
        var RmsProxy = new RMSProxy();

        var proxyTasks = Task.WhenAll(ChatProxy.RunAsync(), RtmpProxy.RunAsync(), RmsProxy.RunAsync());

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
    static void SetKey(JsonNode? configObject, string key, object value)
    {
        if (configObject == null) return;

        if (configObject[key] != null)
        {
            switch (value)
            {
                case bool boolValue:
                    configObject[key] = boolValue;
                    break;
                case int intValue:
                    configObject[key] = intValue;
                    break;
                case double doubleValue:
                    configObject[key] = doubleValue;
                    break;
                case string stringValue:
                    configObject[key] = stringValue;
                    break;
                default:
                    throw new InvalidOperationException($"Unsupported type: {value.GetType()}");
            }
        }
    }
    private static void SetNestedKeys(JsonNode? configObject, string parentKey, string childKey, object value)
    {
        if (configObject == null) return;
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
        SetKey(configObject, "anticheat.vanguard.backgroundInstall", false);
        SetKey(configObject, "anticheat.vanguard.enabled", false);
        SetKey(configObject, "keystone.client.feature_flags.restart_required.disabled", true);
        SetKey(configObject, "keystone.client.feature_flags.vanguardLaunch.disabled", true);
        SetKey(configObject, "lol.client_settings.vanguard.enabled", false);
        SetKey(configObject, "lol.client_settings.vanguard.enabled_mac", false);
        SetKey(configObject, "lol.client_settings.vanguard.url", "");
        RemoveVanguardDependencies(configObject, "keystone.products.league_of_legends.patchlines.live");
        RemoveVanguardDependencies(configObject, "keystone.products.league_of_legends.patchlines.pbe");
        RemoveVanguardDependencies(configObject, "keystone.products.valorant.patchlines.live");
    }
    public static void LegacyHonor(JsonNode? configObject)
    {
        SetNestedKeys(configObject, "lol.client_settings.honor", "CeremonyV3Enabled", false);
        SetNestedKeys(configObject, "lol.client_settings.honor", "Enabled", true);
        SetNestedKeys(configObject, "lol.client_settings.honor", "HonorEndpointsV2Enabled", false);
        SetNestedKeys(configObject, "lol.client_settings.honor", "HonorSuggestionsEnabled", true);
        SetNestedKeys(configObject, "lol.client_settings.honor", "HonorVisibilityEnabled", true);
        SetNestedKeys(configObject, "lol.client_settings.honor", "SecondsToVote", 90);
    }
    public static class SharedGeopassUrl
    {
        public static string? _GeopassUrl;

        public static string? Get()
        {
            return _GeopassUrl;
        }

        public static void Set(string url)
        {
            _GeopassUrl = url;
        }
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
    public static class GeopassHandler
    {
        private static string _userRegion;

        public static void DecodeAndStoreUserRegion(string content)
        {
            _userRegion = JwtDecoder.DecodeAndGetRegion(content);
        }

        public static string GetUserRegion()
        {
            return _userRegion;
        }
    }
    public static class GeopassHandlerRms
    {
        private static string _userRegionRms;

        public static void DecodeAndStoreUserRegionRms(string content)
        {
            _userRegionRms = JwtDecoderRms.DecodeAndGetRegionRms(content);
        }

        public static string GetUserRegionRms()
        {
            return _userRegionRms;
        }
    }
    public class JwtDecoder
    {
        private static string _storedRegion;

        public static string DecodeAndGetRegion(string jwtToken)
        {
            try
            {

                var pasJwtContent = jwtToken.Split('.')[1];
                var validBase64 = pasJwtContent.PadRight((pasJwtContent.Length / 4 * 4) + (pasJwtContent.Length % 4 == 0 ? 0 : 4), '=');
                var pasJwtString = Encoding.UTF8.GetString(Convert.FromBase64String(validBase64));
                var pasJwtJson = JsonSerializer.Deserialize<JsonNode>(pasJwtString);
                _storedRegion = pasJwtJson?["affinity"]?.GetValue<string>();

                return _storedRegion ?? throw new Exception("JWT payload is malformed or missing 'affinity'.");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetStoredRegion()
        {
            return _storedRegion;
        }
    }
    public class JwtDecoderRms
    {
        private static string _storedRegionRms;

        public static string DecodeAndGetRegionRms(string jwtToken)
        {
            try
            {

                var pasJwtContent = jwtToken.Split('.')[1];
                var validBase64 = pasJwtContent.PadRight((pasJwtContent.Length / 4 * 4) + (pasJwtContent.Length % 4 == 0 ? 0 : 4), '=');
                var pasJwtString = Encoding.UTF8.GetString(Convert.FromBase64String(validBase64));
                var pasJwtJson = JsonSerializer.Deserialize<JsonNode>(pasJwtString);
                _storedRegionRms = pasJwtJson?["affinity"]?.GetValue<string>();

                return _storedRegionRms ?? throw new Exception("JWT payload is malformed or missing 'affinity'.");
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static string GetStoredRegionRms()
        {
            return _storedRegionRms;
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

    public static class SharedRmsHost
    {
        public static string? _RmsHost;

        public static string? Get()
        {
            return _RmsHost;
        }

        public static void Set(string host)
        {
            _RmsHost = host;
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