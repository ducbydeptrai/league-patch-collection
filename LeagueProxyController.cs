using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using System.Diagnostics;
using EmbedIO.Utilities;

namespace LeaguePatchCollection;

public sealed class LeagueProxyEvents
{
    public delegate string ProcessBasicEndpoint(string content);

    public event ProcessBasicEndpoint? OnProcessConfigPublic;
    public event ProcessBasicEndpoint? OnProcessConfigPlayer;
    public event ProcessBasicEndpoint? OnProcessLedge;

    private static LeagueProxyEvents? _Instance = null;

    internal static LeagueProxyEvents Instance
    {
        get
        {
            _Instance ??= new LeagueProxyEvents();
            return _Instance;
        }
    }

    private LeagueProxyEvents()
    {
        OnProcessConfigPublic = null;
        OnProcessConfigPlayer = null;
        OnProcessLedge = null;
    }

    private string InvokeProcessBasicEndpoint(ProcessBasicEndpoint? @event, string content)
    {
        if (@event is null)
            return content;

        foreach (var i in @event.GetInvocationList())
        {
            var result = i.DynamicInvoke(content);
            if (result is not string resultString)
                throw new Exception("Return value of an event is not string!");

            content = resultString;
        }

        return content;
    }

    internal string InvokeProcessConfigPublic(string content) => InvokeProcessBasicEndpoint(OnProcessConfigPublic, content);
    internal string InvokeProcessConfigPlayer(string content) => InvokeProcessBasicEndpoint(OnProcessConfigPlayer, content);
    internal string InvokeProcessLedge(string content) => InvokeProcessBasicEndpoint(OnProcessLedge, content);

}

internal sealed class ConfigController : WebApiController
{
    private static HttpClient _Client = new HttpClient();
    private const string BASE_URL = "https://clientconfig.rpg.riotgames.com";

    private static LeagueProxyEvents _Events => LeagueProxyEvents.Instance;

    [Route(HttpVerbs.Get, "/api/v1/config/public")]
    public async Task GetConfigPublic()
    {
        var response = await ClientConfig(HttpContext.Request);
        var content = await response.Content.ReadAsStringAsync();

        content = _Events.InvokeProcessConfigPublic(content);

        await SendResponse(response, content);
    }

    [Route(HttpVerbs.Get, "/api/v1/config/player")]
    public async Task GetConfigPlayer()
    {
        var response = await ClientConfig(HttpContext.Request);
        var content = await response.Content.ReadAsStringAsync();

        content = _Events.InvokeProcessConfigPlayer(content);

        await SendResponse(response, content);
    }

    private async Task<HttpResponseMessage> ClientConfig(IHttpRequest request)
    {
        var url = BASE_URL + request.RawUrl;

        using var message = new HttpRequestMessage(HttpMethod.Get, url);
        message.Headers.TryAddWithoutValidation("User-Agent", request.Headers["user-agent"]);

        if (request.Headers["x-riot-entitlements-jwt"] is not null)
            message.Headers.TryAddWithoutValidation("X-Riot-Entitlements-JWT", request.Headers["x-riot-entitlements-jwt"]);

        if (request.Headers["authorization"] is not null)
            message.Headers.TryAddWithoutValidation("Authorization", request.Headers["authorization"]);

        return await _Client.SendAsync(message);
    }

    private async Task SendResponse(HttpResponseMessage response, string content)
    {
        var responseBuffer = Encoding.UTF8.GetBytes(content);

        HttpContext.Response.SendChunked = false;
        HttpContext.Response.ContentType = "application/json";
        HttpContext.Response.ContentLength64 = responseBuffer.Length;
        HttpContext.Response.StatusCode = (int)response.StatusCode;

        await HttpContext.Response.OutputStream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
        HttpContext.Response.OutputStream.Close();
    }
}

internal sealed class LedgeController : WebApiController
{
    private static HttpClient _Client = new HttpClient();
    private const string LEDGE_URL = "https://na-red.lol.sgp.pvp.net";

    private static LeagueProxyEvents _Events => LeagueProxyEvents.Instance;

    [Route(HttpVerbs.Get, "/", true)]
    public async Task GetLedge()
    {
        var response = await GetLedge(HttpContext.Request);
        var content = await response.Content.ReadAsStringAsync();

        content = _Events.InvokeProcessLedge(content);

        await SendResponse(response, content);
    }

    [Route(HttpVerbs.Post, "/", true)]
    public async Task PostLedge()
    {
        var response = await PostLedge(HttpContext.Request);
        var content = await response.Content.ReadAsStringAsync();

        content = _Events.InvokeProcessLedge(content);

        await SendResponse(response, content);
    }
    private async Task<HttpResponseMessage> PostLedge(IHttpRequest request)
    {
        var url = LEDGE_URL + request.RawUrl;

        using var message = new HttpRequestMessage(HttpMethod.Post, url);

        message.Headers.TryAddWithoutValidation("User-Agent", request.Headers["user-agent"]);
        message.Headers.TryAddWithoutValidation("Accept", "application/json");

        if (request.Headers["x-riot-entitlements-jwt"] is not null)
            message.Headers.TryAddWithoutValidation("X-Riot-Entitlements-JWT", request.Headers["x-riot-entitlements-jwt"]);

        if (request.Headers["authorization"] is not null)
            message.Headers.TryAddWithoutValidation("Authorization", request.Headers["authorization"]);

        if (request.Headers.ContainsKey("Content-Length"))
        {
            message.Headers.TryAddWithoutValidation("Content-Length", request.Headers["Content-Length"]);
        }

        if (request.Headers.ContainsKey("payload"))
        {
            message.Headers.TryAddWithoutValidation("payload", request.Headers["payload"]);
        }

        return await _Client.SendAsync(message);
    }

    private async Task<HttpResponseMessage> GetLedge(IHttpRequest request)
    {
        var url = LEDGE_URL + request.RawUrl;

        using var message = new HttpRequestMessage(HttpMethod.Get, url);

        message.Headers.TryAddWithoutValidation("User-Agent", request.Headers["user-agent"]);
        message.Headers.TryAddWithoutValidation("Accept", "application/json");

        if (request.Headers["x-riot-entitlements-jwt"] is not null)
            message.Headers.TryAddWithoutValidation("X-Riot-Entitlements-JWT", request.Headers["x-riot-entitlements-jwt"]);

        if (request.Headers["authorization"] is not null)
            message.Headers.TryAddWithoutValidation("Authorization", request.Headers["authorization"]);

        return await _Client.SendAsync(message);
    }

    private async Task SendResponse(HttpResponseMessage response, string content)
    {
        var responseBuffer = Encoding.UTF8.GetBytes(content);

        HttpContext.Response.SendChunked = false;
        HttpContext.Response.ContentType = "application/json";
        HttpContext.Response.ContentLength64 = responseBuffer.Length;
        HttpContext.Response.StatusCode = (int)response.StatusCode;

        await HttpContext.Response.OutputStream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
        HttpContext.Response.OutputStream.Close();
    }
}

internal sealed class ProxyServer<T> where T : WebApiController, new()
{
    private WebServer _WebServer;
    private int _Port;

    public string Url => $"http://127.0.0.1:{_Port}";

    public ProxyServer(int port)
    {
        _Port = port;

        _WebServer = new WebServer(o => o
                .WithUrlPrefix(Url)
                .WithMode(HttpListenerMode.EmbedIO))
                .WithWebApi("/", m => m
                    .WithController<T>()
                );
    }

    public void Start(CancellationToken cancellationToken = default)
    {
        _WebServer.Start(cancellationToken);
    }

    public Task RunAsync(CancellationToken cancellationToken = default)
    {
        return _WebServer.RunAsync(cancellationToken);
    }
}
public class LeagueProxy
{
    private ProxyServer<ConfigController> _ConfigServer;
    private ProxyServer<LedgeController> _LedgeServer;
    private RiotClient _RiotClient;
    private CancellationTokenSource? _ServerCTS;

    public LeagueProxyEvents Events => LeagueProxyEvents.Instance;

    public LeagueProxy()
    {
        _ConfigServer = new ProxyServer<ConfigController>(29150); // Port for ConfigServer
        _LedgeServer = new ProxyServer<LedgeController>(29151);   // Port for LedgeServer
        _RiotClient = new RiotClient();
        _ServerCTS = null;
    }

    private void TerminateRiotServices()
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
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine($"Stopping {processName}...");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error terminating {processName}, Please contact c4t_bot on Discord if this issue persists.");
                Console.ResetColor();
            }
        }

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Riot processes terminated, restarting now to apply patches.");
        Console.ResetColor();
    }

    public void Start(out string configServerUrl, out string ledgeServerUrl)
    {
        if (_ServerCTS is not null)
            throw new Exception("Proxy servers are already running!");

        TerminateRiotServices();

        _ServerCTS = new CancellationTokenSource();

        _ConfigServer.Start(_ServerCTS.Token);
        configServerUrl = _ConfigServer.Url;
        Console.WriteLine($"Config Server running at: {_ConfigServer.Url}");

        _LedgeServer.Start(_ServerCTS.Token);
        ledgeServerUrl = _LedgeServer.Url;
        Console.WriteLine($"Ledge Server running at: {_LedgeServer.Url}");
    }

    public void Stop()
    {
        if (_ServerCTS is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            throw new Exception("Failed to stop proxy service, service not running.");
        }

        _ServerCTS.Cancel();
        _ServerCTS = null;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Proxy service successfully stopped.");
        Console.ResetColor();
    }

    public Process? LaunchRCS(IEnumerable<string>? args = null)
    {
        if (_ServerCTS is null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            throw new Exception("Error starting patcher. Please contact c4t_bot on Discord if this issue persists.");
        }

        return _RiotClient.Launch(_ConfigServer.Url, args);
    }

    public Process? StartAndLaunchRCS(IEnumerable<string>? args = null)
    {
        if (_ServerCTS is not null)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            throw new Exception("Another instance of League Patch Collection is already running, please close that first before running.");
        }

        Start(out _, out _);
        return LaunchRCS(args);
    }
}