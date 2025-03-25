using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.Diagnostics;

namespace LeaguePatchCollection;
internal static class LcuWatcher
{
    private static readonly HttpClient _Client = new(new HttpClientHandler
    {
        UseCookies = false,
        UseProxy = false,
        Proxy = null,
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
        CheckCertificateRevocationList = false
    });

    public static async Task<string?> GetLockfileContent()
    {
        if (!IsLeagueClientRunning())
        {
            Trace.WriteLine("[ERROR] Riot Client is not running.");
            return null;
        }

        string? gameDir = GetProductInstallPath();
        if (string.IsNullOrEmpty(gameDir))
        {
            Trace.WriteLine("[ERROR] Could not determine game directory.");
            return null;
        }

        string lockfilePath = Path.Combine(gameDir, "lockfile");

        if (!File.Exists(lockfilePath))
        {
            Trace.WriteLine("[ERROR] Lockfile does not exist.");
            return null;
        }

        try
        {
            return OperatingSystem.IsMacOS()
                ? await ReadFileMac(lockfilePath)
                : await ReadFileWindows(lockfilePath);
        }
        catch (IOException ex)
        {
            Trace.WriteLine($"[ERROR] Failed to access lockfile: {ex.Message}");
            return null;
        }
    }

    private static async Task<string> ReadFileWindows(string filePath)
    {
        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(fileStream);
        return await reader.ReadToEndAsync();
    }

    private static async Task<string> ReadFileMac(string filePath)
    {
        ProcessStartInfo psi = new()
        {
            FileName = "/bin/cat",
            Arguments = $"\"{filePath}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        using Process process = Process.Start(psi);
        using StreamReader reader = process.StandardOutput;
        string content = await reader.ReadToEndAsync();
        process.WaitForExit();

        return content;
    }

    public static async Task<HttpResponseMessage?> SendLcuRequest(string endpoint, HttpMethod method, HttpContent? content = null)
    {
        string? lockfileContent = await GetLockfileContent();
        if (lockfileContent == null)
        {
            return null;
        }

        var lockfileParts = lockfileContent.Split(':');
        if (lockfileParts.Length != 5)
        {
            Trace.WriteLine("[ERROR] Lockfile format is incorrect.");
            return null;
        }

        string port = lockfileParts[2];
        string password = lockfileParts[3];
        string authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"riot:{password}"));

        _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);
        string url = $"https://127.0.0.1:{port}{endpoint}";

        try
        {
            return method switch
            {
                HttpMethod m when m == HttpMethod.Get => await _Client.GetAsync(url),
                HttpMethod m when m == HttpMethod.Post => await _Client.PostAsync(url, content),
                HttpMethod m when m == HttpMethod.Put => await _Client.PutAsync(url, content),
                HttpMethod m when m == HttpMethod.Delete => await _Client.DeleteAsync(url),
                _ => throw new NotSupportedException($"HTTP method {method} is not supported.")
            };
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[ERROR] LCU request failed: {ex.Message}");
            return null;
        }
    }

    private static bool IsLeagueClientRunning()
    {
        return Process.GetProcessesByName("LeagueClient").Length != 0 || Process.GetProcessesByName("League of Legends").Length != 0;
    }
    private static string? GetProductInstallPath()
    {
        string yamlFilePath = GetYamlFilePath();
        if (!File.Exists(yamlFilePath))
        {
            Trace.WriteLine("[WARN] YAML file not found. Falling back to default paths.");
            return GetHardcodedGamePath();
        }

        try
        {
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();

            using var reader = new StreamReader(yamlFilePath);
            var yamlContent = deserializer.Deserialize<dynamic>(reader);
            return OperatingSystem.IsMacOS()
                ? Path.Combine(yamlContent["product_install_full_path"], "Contents", "LoL")
                : yamlContent["product_install_full_path"];
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[ERROR] Failed to read YAML: {ex.Message}");
            return GetHardcodedGamePath();
        }
    }

    private static string GetYamlFilePath() =>
        OperatingSystem.IsMacOS()
            ? "/Users/Shared/Riot Games/Metadata/league_of_legends.live/league_of_legends.live.product_settings.yaml"
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "Riot Games", "Metadata", "league_of_legends.live", "league_of_legends.live.product_settings.yaml");

    private static string? GetHardcodedGamePath() =>
        OperatingSystem.IsMacOS()
            ? "/Applications/League of Legends.app/Contents/LoL/"
            : "C:\\Riot Games\\League of Legends";
}
internal static class RcsWatcher
{
    private static readonly HttpClient _Client = new(new HttpClientHandler
    {
        UseCookies = false,
        UseProxy = false,
        Proxy = null,
        AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true,
        CheckCertificateRevocationList = false
    });

    public static async Task<string?> GetLockfileContent()
    {
        if (!IsRiotClientRunning())
        {
            Trace.WriteLine("[ERROR] Riot Client is not running.");
            return null;
        }

        string gameDir = OperatingSystem.IsMacOS()
            ? Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Library", "Application Support", "Riot Games", "Riot Client", "Config")
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Riot Games", "Riot Client", "Config");

        string lockfilePath = Path.Combine(gameDir, "lockfile");

        if (!File.Exists(lockfilePath))
        {
            Trace.WriteLine("[ERROR] RCS lockfile does not exist.");
            return null;
        }

        try
        {
            return OperatingSystem.IsMacOS()
                ? await ReadFileMac(lockfilePath)
                : await ReadFileWindows(lockfilePath);
        }
        catch (IOException ex)
        {
            Trace.WriteLine($"[ERROR] Failed to access RCS lockfile: {ex.Message}");
            return null;
        }
    }

    private static bool IsRiotClientRunning()
    {
        return Process.GetProcessesByName("RiotClientServices").Length != 0 || Process.GetProcessesByName("Riot Client").Length != 0;
    }

    private static async Task<string> ReadFileWindows(string filePath)
    {
        using var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        using var reader = new StreamReader(fileStream);
        return await reader.ReadToEndAsync();
    }

    private static async Task<string> ReadFileMac(string filePath)
    {
        ProcessStartInfo psi = new()
        {
            FileName = "/bin/cat",
            Arguments = $"\"{filePath}\"",
            RedirectStandardOutput = true,
            UseShellExecute = false
        };

        using Process process = Process.Start(psi);
        using StreamReader reader = process.StandardOutput;
        string content = await reader.ReadToEndAsync();
        process.WaitForExit();

        return content;
    }

    public static async Task<HttpResponseMessage?> SendRcsRequest(string endpoint, HttpMethod method, HttpContent? content = null)
    {
        string? lockfileContent = await GetLockfileContent();
        if (lockfileContent == null)
        {
            return null;
        }

        var lockfileParts = lockfileContent.Split(':');
        if (lockfileParts.Length != 5)
        {
            Trace.WriteLine("[ERROR] RCS lockfile format is incorrect.");
            return null;
        }

        string port = lockfileParts[2];
        string password = lockfileParts[3];
        string authValue = Convert.ToBase64String(Encoding.UTF8.GetBytes($"riot:{password}"));

        _Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", authValue);
        string url = $"https://127.0.0.1:{port}{endpoint}";

        try
        {
            return method switch
            {
                HttpMethod m when m == HttpMethod.Get => await _Client.GetAsync(url),
                HttpMethod m when m == HttpMethod.Post => await _Client.PostAsync(url, content),
                HttpMethod m when m == HttpMethod.Put => await _Client.PutAsync(url, content),
                HttpMethod m when m == HttpMethod.Delete => await _Client.DeleteAsync(url),
                _ => throw new NotSupportedException($"HTTP method {method} is not supported.")
            };
        }
        catch (Exception ex)
        {
            Trace.WriteLine($"[ERROR] RCS request failed: {ex.Message}");
            return null;
        }
    }
}

