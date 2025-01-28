using EmbedIO;
using Microsoft.VisualBasic.Logging;
using System.Diagnostics;
using System.Runtime.InteropServices.Marshalling;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace LeaguePatchCollection;

public class LeagueProxy
{
    private static HttpProxy.httpProxyServer<HttpProxy.ConfigController> _ConfigServer;
    private static HttpProxy.httpProxyServer<HttpProxy.GeopassController> _GeopassServer;
    private static HttpProxy.httpProxyServer<HttpProxy.LedgeController> _LedgeServer;
    private static RiotClient _RiotClient;
    private static CancellationTokenSource? _ServerCTS;
    private static XMPPProxy _ChatProxy;
    private static RTMPProxy _RtmpProxy;
    private static RMSProxy _RmsProxy;

    static LeagueProxy()
    {
        _ConfigServer = new HttpProxy.httpProxyServer<HttpProxy.ConfigController>(29150);
        _GeopassServer = new HttpProxy.httpProxyServer<HttpProxy.GeopassController>(29151);
        _LedgeServer = new HttpProxy.httpProxyServer<HttpProxy.LedgeController>(29152);
        _RiotClient = new RiotClient();
        _ChatProxy = new XMPPProxy();
        _RtmpProxy = new RTMPProxy();
        _RmsProxy = new RMSProxy();
    }

    private static void TerminateRiotServices()
    {
        string[] riotProcesses = { "RiotClientServices", "LeagueClient" };

        foreach (var processName in riotProcesses)
        {
            try
            {
                var processes = Process.GetProcessesByName(processName);

                foreach (var process in processes)
                {
                    process.Kill();
                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Error stopping {processName}, {ex}");
            }
        }
    }

    public static void Start(out string configServerUrl, out string ledgeServerUrl, out string geopassServerUrl)
    {
        if (_ServerCTS is not null)
        {
            Trace.WriteLine("Restarting servers...");
            Stop();
        }

        SystemYamlLive.LoadProductInstallPath();
        _ServerCTS = new CancellationTokenSource();

        _ConfigServer.Start(_ServerCTS.Token);
        configServerUrl = _ConfigServer.Url;

        _LedgeServer.Start(_ServerCTS.Token);
        ledgeServerUrl = _LedgeServer.Url;

        _GeopassServer.Start(_ServerCTS.Token);
        geopassServerUrl = _GeopassServer.Url;

        var chatProxyTask = _ChatProxy.RunAsync(_ServerCTS.Token);
        var rmsProxyTask = RMSProxy.RunAsync(_ServerCTS.Token);
    }

    public static void Stop()
    {
        if (_ServerCTS is null)
        {
            throw new Exception("Failed to stop proxy service, service not running.");
        }

        _ServerCTS.Cancel();

        _ChatProxy.Stop();
        _RmsProxy.Stop();

        _ConfigServer.Dispose();
        _GeopassServer.Dispose();
        _LedgeServer.Dispose();

        _ServerCTS?.Dispose();
        _ServerCTS = null;

        Trace.WriteLine("Proxy services successfully stopped.");
    }

    public static Process? LaunchRCS(IEnumerable<string>? args = null)
    {
        if (_ServerCTS is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            throw new Exception("Error starting proxies. Please contact c4t_bot on Discord if this issue persists.");
        }
        TerminateRiotServices();
        return RiotClient.Launch(_ConfigServer.Url, args);
    }
}