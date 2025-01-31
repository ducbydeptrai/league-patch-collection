using System.Diagnostics;
using System.IO;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace LeaguePatchCollection;

public sealed class RiotClient
{
    public RiotClient()
    {

    }

    public static Process? Launch(string configServerUrl, IEnumerable<string>? args = null)
    {
        var path = GetPath();
        if (path is null)
        {
            Trace.WriteLine("[ERROR] Unable to find Riot Client installation path.");
            return null;
        }

        Trace.WriteLine($"[INFO] Found RCS at {path}");

        IEnumerable<string> allArgs = [$"--client-config-url={configServerUrl}", .. args ?? []];
        Trace.WriteLine($"[INFO] Launching RCS with arguments: {string.Join(" ", allArgs)}");

        return Process.Start(path, allArgs);
    }

    private static string? GetPath()
    {
        string installPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                                           "Riot Games/RiotClientInstalls.json");

        if (File.Exists(installPath))
        {
            try
            {
                var data = JsonSerializer.Deserialize<JsonNode>(File.ReadAllText(installPath));
                var rcPaths = new List<string?>
            {
                data?["rc_default"]?.ToString(),
                data?["rc_live"]?.ToString(),
                data?["rc_beta"]?.ToString()
            };

                var validPath = rcPaths.FirstOrDefault(File.Exists);
                if (validPath != null)
                    return validPath;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"[WARN] An error occurred while processing the install path, using fallback path: {ex.Message}");
            }
        }

        foreach (var drive in DriveInfo.GetDrives().Where(d => d.IsReady && d.DriveType == DriveType.Fixed))
        {
            var potentialPath = Path.Combine(drive.RootDirectory.FullName, "Riot Games", "Riot Client", "RiotClientServices.exe");
            if (File.Exists(potentialPath))
            {
                Trace.WriteLine($"[INFO] Found RiotClient fallback path found at {drive}{potentialPath}");
                return potentialPath;
            }
        }

        Trace.WriteLine("[ERROR] Failed to locate Riot Client installation path from both fallback method and RiotClientInstalls.");
        return null;
    }
}
public class Utility
{
    public static void TerminateRiotServices()
    {
        string[] riotProcesses = { "RiotClientServices", "LeagueClient" };

        foreach (var processName in riotProcesses)
        {
            try
            {
                var processes = Process.GetProcessesByName(processName);

                foreach (var process in processes)
                {
                    Trace.WriteLine($"[INFO] Attemping to stop {processName}.");
                    process.Kill();
                    process.WaitForExit();
                    Trace.WriteLine($"[INFO] {processName} stopped successfully.");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"[ERROR] Failed to stop {processName}. Exception: {ex.Message}");
            }
        }
    }
    public static void RestartUX()
    {
        string[] riotProcesses = { "LeagueClientUxRender", };

        foreach (var processName in riotProcesses)
        {
            try
            {
                var processes = Process.GetProcessesByName(processName);

                foreach (var process in processes)
                {
                    Trace.WriteLine($"[INFO] Attemping to restart {processName}.");
                    process.Kill();
                    process.WaitForExit();
                    Trace.WriteLine($"[INFO] {processName} restarted successfully.");
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"[ERROR] Failed to restart {processName}. Exception: {ex.Message}");
            }
        }
    }
    private string GetCommandLine(Process process)
    {
        using (var searcher = new System.Management.ManagementObjectSearcher($"SELECT CommandLine FROM Win32_Process WHERE ProcessId = {process.Id}"))
        {
            var query = searcher.Get().Cast<System.Management.ManagementObject>().FirstOrDefault();
            return query?["CommandLine"]?.ToString() ?? string.Empty;
        }
    }
}