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


    public static void Start(out string configServerUrl, out string ledgeServerUrl, out string geopassServerUrl)
    {
        if (_ServerCTS is not null)
        {
            Trace.WriteLine("[INFO] Proxy is already running. Attempting to restart.");
            Stop();
        }

        SystemYamlLive.LoadProductInstallPath();
        _ServerCTS = new CancellationTokenSource();

        _ConfigServer.Start(_ServerCTS.Token);
        configServerUrl = _ConfigServer.Url;
        Trace.WriteLine($"[INFO] Config Proxy started on {configServerUrl}");

        _LedgeServer.Start(_ServerCTS.Token);
        ledgeServerUrl = _LedgeServer.Url;
        Trace.WriteLine($"[INFO] Ledge Proxy started on {ledgeServerUrl}");

        _GeopassServer.Start(_ServerCTS.Token);
        geopassServerUrl = _GeopassServer.Url;
        Trace.WriteLine($"[INFO] Geopass Proxy started on {geopassServerUrl}");

        var chatProxyTask = _ChatProxy.RunAsync(_ServerCTS.Token);
        Trace.WriteLine("[INFO] Chat Proxy started.");

        var rmsProxyTask = _RmsProxy.RunAsync(_ServerCTS.Token);
        Trace.WriteLine("[INFO] RMS Proxy started.");
    }

    public static void Stop()
    {
        if (_ServerCTS is null)
        {
            Trace.WriteLine("[WARN] Unable to stop proxy service: Service is not running.");
        }

        _ServerCTS?.Cancel();

        _ChatProxy.Stop();
        _RmsProxy.Stop();

        _ConfigServer.Dispose();
        _GeopassServer.Dispose();
        _LedgeServer.Dispose();

        _ServerCTS?.Dispose();
        _ServerCTS = null;

        Trace.WriteLine("[INFO] Proxy services successfully stopped.");
    }
    public static Process? LaunchRCS(IEnumerable<string>? args = null)
    {
        if (_ServerCTS is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Trace.WriteLine("[ERROR] RCS launch failed: Proxies were not started due to an error.");
        }
        return RiotClient.Launch(_ConfigServer.Url, args);
    }
}