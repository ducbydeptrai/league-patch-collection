using LeaguePatchCollection;
using System.Text.Json;
using System.Text.Json.Nodes;

class App
{
    static (string, object)[] PublicConfigValues = {
        ("anticheat.vanguard.backgroundInstall", false),
        ("anticheat.vanguard.enabled", false),
        ("keystone.client.feature_flags.restart_required.disabled", true),
        ("keystone.client.feature_flags.vanguardLaunch.disabled", true),
        ("lol.client_settings.vanguard.enabled", false),
        ("lol.client_settings.vanguard.enabled_mac", false),
        ("lol.client_settings.vanguard.url", ""),
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

    static (string, object)[] PlayerConfigValues = {
        ("chat.allow_bad_cert.enabled", true),
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
        ("lol.game_client_settings.logging.enable_http_public_logs", false)
    };

    public static async Task Main(string[] args)
    {
        var leagueProxy = new LeagueProxy();

        leagueProxy.Events.OnProcessConfigPublic += (string content) =>
        {
            var configObject = JsonSerializer.Deserialize<JsonNode>(content);

            SetConfigValues(configObject, PublicConfigValues);

            SetConfig(configObject, "lol.client_settings.honor", "CeremonyV3Enabled", false);
            SetConfig(configObject, "lol.client_settings.honor", "Enabled", true);
            SetConfig(configObject, "lol.client_settings.honor", "HonorEndpointsV2Enabled", false);
            SetConfig(configObject, "lol.client_settings.honor", "HonorSuggestionsEnabled", true);
            SetConfig(configObject, "lol.client_settings.honor", "HonorVisibilityEnabled", true);
            SetConfig(configObject, "lol.client_settings.honor", "SecondsToVote", 90);
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

            RemoveVanguardDependencies(configObject, "keystone.products.league_of_legends.patchlines.live");
            RemoveVanguardDependencies(configObject, "keystone.products.league_of_legends.patchlines.pbe");
            RemoveVanguardDependencies(configObject, "keystone.products.valorant.patchlines.live");

            return JsonSerializer.Serialize(configObject);
        };

        leagueProxy.Events.OnProcessConfigPlayer += (string content) =>
        {
            var configObject = JsonSerializer.Deserialize<JsonNode>(content);

            SetConfigValues(configObject, PlayerConfigValues);

            NoLoyalty(configObject);
            SetConfig(configObject, "lol.client_settings.deepLinks", "launchLorEnabled", false);

            return JsonSerializer.Serialize(configObject);
        };

        leagueProxy.Events.OnProcessLedge += (string content) =>
        {
            var configObject = JsonSerializer.Deserialize<JsonNode>(content);

            return JsonSerializer.Serialize(configObject);
        };

        var process = leagueProxy.StartAndLaunchRCS(args);
        if (process is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Failed to start Riot Client. This may be due to a change on Riot's end. Please ensure you're using the latest version by checking https://github.com/Cat1Bot/league-patch-collection/releases. If the issue persists even with the latest version, contact c4t_bot on Discord for further assistance.");
            Console.ResetColor();
            leagueProxy.Stop();
            return;
        }

        await process.WaitForExitAsync();
        leagueProxy.Stop();
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