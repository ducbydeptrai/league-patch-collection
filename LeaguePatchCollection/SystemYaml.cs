using System.Diagnostics;
using System.Text.RegularExpressions;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;

namespace LeaguePatchCollection;

public static partial class SystemYamlLive
{
    private static string? _gamePath;
    public static string? RtmpServer { get; private set; }

    public static string LoadProductInstallPath()
    {
        _gamePath = GetProductInstallPath() ?? GetDefaultRiotGamesPath();
        string configPath = Path.Combine(_gamePath, "Config", "system.yaml");
        CopySystemYaml(_gamePath, configPath);
        return _gamePath;
    }

    private static string? GetProductInstallPath()
    {
        try
        {
            string yamlFilePath = GetYamlFilePath();
            if (!File.Exists(yamlFilePath))
            {
                Console.WriteLine("Product settings YAML file not found.");
                return null;
            }

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            using var reader = new StreamReader(yamlFilePath);
            var yamlContent = deserializer.Deserialize<dynamic>(reader);
            string productInstallFullPath = yamlContent["product_install_full_path"];

            return OperatingSystem.IsMacOS()
                ? Path.Combine(productInstallFullPath, "Contents", "LoL")
                : productInstallFullPath;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading or parsing the file: {ex.Message}");
            return null;
        }
    }

    private static string GetYamlFilePath() => OperatingSystem.IsMacOS()
        ? "/Users/Shared/Riot Games/Metadata/league_of_legends.live/league_of_legends.live.product_settings.yaml"
        : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Riot Games", "Metadata", "league_of_legends.live", "league_of_legends.live.product_settings.yaml");

    private static string GetDefaultRiotGamesPath()
    {
        if (OperatingSystem.IsMacOS()) return "/Applications/League of Legends.app/Contents/LoL/";
        return Path.Combine(Environment.GetEnvironmentVariable("SYSTEMDRIVE") ?? "C:", "Riot Games", "League of Legends");
    }

    public static void CopySystemYaml(string sourcePath, string destinationPath)
    {
        var directoryPath = Path.GetDirectoryName(destinationPath);
        if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath))
        {
            Directory.CreateDirectory(directoryPath);
        }

        string sourceFile = Path.Combine(sourcePath, "system.yaml");
        if (File.Exists(sourceFile))
        {
            File.Copy(sourceFile, destinationPath, overwrite: true);
            ModifySystemYaml(destinationPath);
        }
        else
        {
            Trace.WriteLine($"[WARN] Source system.yaml not found at {sourceFile}");
        }
    }

    public static void ModifySystemYaml(string configFilePath)
    {
        try
        {
            string yamlContent = File.ReadAllText(configFilePath);

            var modifications = new Dictionary<Regex, string>
        {
            { rmsHost(), $"ws://127.0.0.1:{LeagueProxy.RmsPort}" },
            { lcdsHost(), "127.0.0.1" },
            { lcdsPort(), $"{LeagueProxy.RtmpPort}" },
            { lcdsTls(), "false" },
            { ledgeUrl(), $"http://127.0.0.1:{LeagueProxy.LedgePort}" },
        };

            foreach (var modification in modifications)
            {
                yamlContent = modification.Key.Replace(yamlContent, modification.Value);
            }

            var match = defaultRegion().Match(yamlContent);
            if (match.Success)
            {
                string region = match.Value.Trim().ToUpper(); // Normalize to uppercase and trim spaces
                Trace.WriteLine($"[INFO] rtmp default region found: {region}");

                RtmpServer = region switch
                {
                    "BR" => "feapp.br1.lol.pvp.net",
                    "EUNE" => "feapp.eun1.lol.pvp.net",
                    "EUW" => "feapp.euw1.lol.pvp.net",
                    "JP" => "feapp.jp1.lol.pvp.net",
                    "LA1" => "feapp.la1.lol.pvp.net",
                    "LA2" => "feapp.la2.lol.pvp.net",
                    "ME1" => "feapp.me1.lol.pvp.net",
                    "NA" => "feapp.na1.lol.pvp.net",
                    "OC1" => "feapp.oc1.lol.pvp.net",
                    "RU" => "feapp.ru.lol.pvp.net",
                    "TR" => "tr1.chat.si.riotgames.com",
                    _ => null,
                };

                if (RtmpServer != null)
                {
                    Trace.WriteLine($"[INFO] RtmpServer set to: {RtmpServer}");
                }
                else
                {
                    Trace.WriteLine("[WARN] No matching RtmpServer for the found region.");
                }
            }
            else
            {
                Trace.WriteLine("[WARN] default_region not found in system.yaml.");
            }

            File.WriteAllText(configFilePath, yamlContent);
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[ERROR] Error modifying system.yaml: {ex.Message}");
        }
    }

    [GeneratedRegex(@"(?<=rms_url\s*:\s*)\S+")]
    private static partial Regex rmsHost();
    [GeneratedRegex(@"(?<=lcds_host\s*:\s*)\S+")]
    private static partial Regex lcdsHost();

    [GeneratedRegex(@"(?<=lcds_port\s*:\s*)\d+")]
    private static partial Regex lcdsPort();

    [GeneratedRegex(@"(?<=use_tls\s*:\s*)\btrue\b|\bfalse\b")]
    private static partial Regex lcdsTls();

    [GeneratedRegex(@"(?<=league_edge_url\s*:\s*)\S+")]
    private static partial Regex ledgeUrl();
    [GeneratedRegex(@"(?<=default_region\s*:\s*)\S+")]
    private static partial Regex defaultRegion();
}
