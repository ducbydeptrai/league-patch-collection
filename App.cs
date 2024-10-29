using LeagueProxyLib;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;

class App
{

    static Tuple<string, bool>[] PubliConfigValues = {
        CreateConfigValue("anticheat.vanguard.backgroundInstall", false),
        CreateConfigValue("anticheat.vanguard.enabled", false),
        CreateConfigValue("keystone.age_restriction.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.lifecycle.backgroundRunning.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.arcane_event.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.arcane_event_live.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.arcane_event_prelaunch.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.arcane_event_premier.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.arcane_theme.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.autoPatch.disabled", true),
        CreateConfigValue("keystone.client.feature_flags.background_mode_patching.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.campaign-hub.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.cpu_memory_warning_report.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.dismissible_name_change_modal.enabled", true),
        CreateConfigValue("keystone.client.feature_flags.eula.use_patch_downloader.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.flaggedNameModal.disabled", true),
        CreateConfigValue("keystone.client.feature_flags.keystone_login_splash_video.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.launch_on_computer_start.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.mfa_notification.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.open_telemetry_sender.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.pending_consent_modal.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.pending_forget_modal.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.privacyPolicy.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.product_update_scanner.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.qrcode_modal.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.quick_actions.enabled", true),
        CreateConfigValue("keystone.client.feature_flags.regionlessLoginInfoTooltip.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.restart_required.disabled", true),
        CreateConfigValue("keystone.client.feature_flags.riot_id_required_modal.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.riot_mobile_special_event.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.self_update_in_background.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.terminate_riot_client_on_product_launch.enabled", true),
        CreateConfigValue("keystone.client.feature_flags.username_required_modal.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.vanguardLaunch.disabled", true),
        CreateConfigValue("keystone.riotgamesapi.telemetry.heartbeat_products", false),
        CreateConfigValue("keystone.riotgamesapi.telemetry.heartbeat_voice_chat_metrics", false),
        CreateConfigValue("keystone.riotgamesapi.telemetry.newrelic_events_v2_enabled", false),
        CreateConfigValue("keystone.riotgamesapi.telemetry.newrelic_metrics_v1_enabled", false),
        CreateConfigValue("keystone.riotgamesapi.telemetry.newrelic_schemaless_events_v2_enabled", false),
        CreateConfigValue("keystone.riotgamesapi.telemetry.opentelemetry_events_enabled", false),
        CreateConfigValue("keystone.riotgamesapi.telemetry.singular_v1_enabled", false),
        CreateConfigValue("keystone.rso-mobile-ui.accountCreationTosAgreement", false),
        CreateConfigValue("keystone.telemetry.heartbeat_products", false),
        CreateConfigValue("keystone.telemetry.heartbeat_voice_chat_metrics", false),
        CreateConfigValue("keystone.telemetry.singular_v1_enabled", false),
        CreateConfigValue("lol.client_settings.clash.eosCelebrationEnabled", false),
        CreateConfigValue("lol.client_settings.missions.upsell_opens_event_hub", false),
        CreateConfigValue("lol.client_settings.paw.enableRPTopUp", false),
        CreateConfigValue("lol.client_settings.remedy.is_verbal_abuse_remedy_modal_enabled", false),
        CreateConfigValue("lol.client_settings.startup.should_show_progress_bar_text", false),
        CreateConfigValue("lol.client_settings.startup.should_wait_for_home_hubs", false),
        CreateConfigValue("lol.client_settings.store.hidePurchaseModalQuantityControl", false),
        CreateConfigValue("lol.client_settings.vanguard.enabled", false),
        CreateConfigValue("lol.game_client_settings.app_config.singular_enabled", false),
        CreateConfigValue("lol.game_client_settings.low_memory_reporting_enabled", false),
        CreateConfigValue("lol.game_client_settings.missions.enabled", false),
        CreateConfigValue("lol.game_client_settings.platform_stats_enabled", false),
        CreateConfigValue("rms.allow_bad_cert.enabled", true)
    };

    static Tuple<string, bool>[] PlayerConfigValues = {
        CreateConfigValue("chat.allow_bad_cert.enabled", true),
        CreateConfigValue("chat.disable_chat_restriction_muted_system_message", true),
        CreateConfigValue("chat.force_filter.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.chrome_devtools.enabled", true),
        CreateConfigValue("keystone.client.feature_flags.fist_animation.enabled", false),
        CreateConfigValue("keystone.client.feature_flags.playerBehaviorToken.enabled", true),
        CreateConfigValue("keystone.riotgamesapi.telemetry.endpoint.send_deprecated", false),
        CreateConfigValue("keystone.riotgamesapi.telemetry.endpoint.send_failure", false),
        CreateConfigValue("keystone.riotgamesapi.telemetry.endpoint.send_success", false),
        CreateConfigValue("keystone.telemetry.metrics_enabled", false),
        CreateConfigValue("keystone.telemetry.newrelic_events_v2_enabled", false),
        CreateConfigValue("keystone.telemetry.newrelic_metrics_v1_enabled", false),
        CreateConfigValue("keystone.telemetry.newrelic_schemaless_events_v2_enabled", false),
        CreateConfigValue("lol.client_settings.metrics.enabled", false),
        CreateConfigValue("lol.client_settings.player_behavior.display_v1_ban_notifications", true),
        CreateConfigValue("lol.client_settings.player_behavior.use_reform_card_v2", false),
        CreateConfigValue("lol.game_client_settings.logging.enable_http_public_logs", false),
        CreateConfigValue("lol.game_client_settings.logging.enable_rms_public_logs", false)
    };

    private static Tuple<string, bool> CreateConfigValue(string key, bool value)
    {
        return new Tuple<string, bool>(key, value);
    }

    public static async Task Main(string[] args)
    {
        KillRiotServices();
        var leagueProxy = new LeagueProxy();

        leagueProxy.Events.OnProcessConfigPublic += (string content) => {
            var configObject = JsonSerializer.Deserialize<JsonNode>(content);

            SetConfigValues(configObject, PubliConfigValues);

            SetConfig(configObject, "lol.client_settings.honor", "CeremonyV3Enabled", false);
            SetConfig(configObject, "lol.client_settings.honor", "Enabled", true);
            SetConfig(configObject, "lol.client_settings.honor", "HonorEndpointsV2Enabled", false);
            SetConfig(configObject, "lol.client_settings.honor", "HonorSuggestionsEnabled", true);
            SetConfig(configObject, "lol.client_settings.honor", "HonorVisibilityEnabled", false);
            SetConfig(configObject, "lol.client_settings.honor", "SecondsToVote", 90);
            SetConfig(configObject, "lol.client_settings.sentry_config", "isEnabled", false);
            SetConfig(configObject, "lol.client_settings.sentry_config", "sampleRate", 0);
            SetConfig(configObject, "lol.client_settings.sentry_config", "dsn", "");

            RemoveVanguardDependencies(configObject, "keystone.products.league_of_legends.patchlines.live");
            RemoveVanguardDependencies(configObject, "keystone.products.league_of_legends.patchlines.pbe");
            RemoveVanguardDependencies(configObject, "keystone.products.valorant.patchlines.live");

            return JsonSerializer.Serialize(configObject);
        };

        leagueProxy.Events.OnProcessConfigPlayer += (string content) => {
            var configObject = JsonSerializer.Deserialize<JsonNode>(content);

            SetConfigValues(configObject, PlayerConfigValues);

            SetConfig(configObject, "lol.client_settings.deepLinks", "launchLorEnabled", false);

            return JsonSerializer.Serialize(configObject);
        };

        var process = leagueProxy.StartAndLaunchRCS(args);
        if (process is null)
        {
            Console.WriteLine("Failed to create RCS process!");
            leagueProxy.Stop();
            return;
        }

        await process.WaitForExitAsync();
        leagueProxy.Stop();


    }

    private static void SetConfigValues(JsonNode? configObject, Tuple<string, bool>[] configValues)
    {
        foreach (var configValue in configValues)
        {
            SetConfig(configObject, configValue.Item1, configValue.Item2);
        }
    }


    static void SetConfig(JsonNode? configObject, string key, bool value)
    {
        if (configObject?[key] is not null)
        {
            configObject[key] = value;
        }
    }

    static void SetConfig(JsonNode? configObject, string key, string value)
    {
        if (configObject?[key] is not null)
        {
            configObject[key] = value;
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

    static void SetConfig(JsonNode? configObject, string parentKey, string childKey, string value)
    {
        if (configObject == null) return;

        if (configObject[parentKey] is JsonNode parentNode)
        {
            parentNode[childKey] = value;
        }
    }

    static void SetConfig(JsonNode? configObject, string parentKey, string childKey, double value)
    {
        if (configObject == null) return;

        if (configObject[parentKey] is JsonNode parentNode)
        {
            parentNode[childKey] = value;
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

    static readonly string[] RiotServiceNames = { "Riot Client", "RiotClientServices", "LeagueClient", "League of Legends" };

    static void KillRiotServices()
    {
        foreach (var serviceName in RiotServiceNames)
        {
            var processes = Process.GetProcessesByName(serviceName);
            foreach (var process in processes)
            {
                try
                {
                    process.Kill();
                }
                catch
                {

                }
            }
        }
    }
}


