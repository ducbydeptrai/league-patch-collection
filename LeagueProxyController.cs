using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using EmbedIO;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using System.Diagnostics;
using EmbedIO.Utilities;
using System.Text.Json.Nodes;
using System.Text.Json;
using Swan.Logging;
using System.Net;
using System.IO.Compression;
using static LeaguePatchCollection.SystemYamlLive;
using static System.Net.Mime.MediaTypeNames;
using Swan.Formatters;
using System.Net.WebSockets;
using EmbedIO.WebSockets;

namespace LeaguePatchCollection;

public sealed class LeagueProxyEvents
{
    public delegate string ProcessBasicEndpoint(string content, IHttpRequest request);

    public event ProcessBasicEndpoint? OnClientConfigPublic;
    public event ProcessBasicEndpoint? OnClientConfigPlayer;
    public event ProcessBasicEndpoint? OnClientGeopass;
    public event ProcessBasicEndpoint? OnClientLedge;

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
        OnClientConfigPublic = null;
        OnClientConfigPlayer = null;
        OnClientGeopass = null;
        OnClientLedge = null;
    }

    private string InvokeProcessBasicEndpoint(ProcessBasicEndpoint? @event, string content, IHttpRequest? request)
    {
        if (@event is null)
            return content;

        foreach (var i in @event.GetInvocationList())
        {
            var result = i.DynamicInvoke(content, request); // Pass 'content' and 'request'
            if (result is not string resultString)
                throw new Exception("Return value of an event is not string!");

            content = resultString;
        }

        return content;
    }

    internal string InvokeClientConfigPublic(string content, IHttpRequest request) => InvokeProcessBasicEndpoint(OnClientConfigPublic, content, request);
    internal string InvokeClientConfigPlayer(string content, IHttpRequest request) => InvokeProcessBasicEndpoint(OnClientConfigPlayer, content, request);
    internal string InvokeClientGeopass(string content, IHttpRequest request) => InvokeProcessBasicEndpoint(OnClientGeopass, content, request);
    internal string InvokeClientLedge(string content, IHttpRequest request) => InvokeProcessBasicEndpoint(OnClientLedge, content, request);
}

internal sealed class ConfigController : WebApiController
{
    private static HttpClient _Client = new(new HttpClientHandler { UseCookies = false, UseProxy = false, Proxy = null });
    private const string BASE_URL = "https://clientconfig.rpg.riotgames.com";

    private static LeagueProxyEvents _Events => LeagueProxyEvents.Instance;

    [Route(HttpVerbs.Get, "/api/v1/config/public")]
    public async Task GetConfigPublic()
    {
        var response = await ClientConfig(HttpContext.Request);
        var content = await response.Content.ReadAsStringAsync();

        content = _Events.InvokeClientConfigPublic(content, HttpContext.Request);

        await SendResponse(response, content);
    }

    [Route(HttpVerbs.Get, "/api/v1/config/player")]
    public async Task GetConfigPlayer()
    {
        var response = await ClientConfig(HttpContext.Request);
        var content = await response.Content.ReadAsStringAsync();

        content = _Events.InvokeClientConfigPlayer(content, HttpContext.Request);

        await SendResponse(response, content);
    }

    private async Task<HttpResponseMessage> ClientConfig(IHttpRequest request)
    {
        var url = BASE_URL + request.RawUrl;

        using var message = new HttpRequestMessage(HttpMethod.Get, url);

        if (request.Headers["accept-encoding"] is not null)
            message.Headers.TryAddWithoutValidation("Accept-Encoding", request.Headers["accept-encoding"]);

        message.Headers.TryAddWithoutValidation("user-agent", request.Headers["user-agent"]);

        if (request.Headers["x-riot-entitlements-jwt"] is not null)
            message.Headers.TryAddWithoutValidation("X-Riot-Entitlements-JWT", request.Headers["x-riot-entitlements-jwt"]);

        if (request.Headers["authorization"] is not null)
            message.Headers.TryAddWithoutValidation("Authorization", request.Headers["authorization"]);

        if (request.Headers["x-riot-rso-identity-jwt"] is not null)
            message.Headers.TryAddWithoutValidation("X-Riot-RSO-Identity-JWT", request.Headers["x-riot-rso-identity-jwt"]);

        if (request.Headers["baggage"] is not null)
            message.Headers.TryAddWithoutValidation("baggage", request.Headers["baggage"]);

        if (request.Headers["traceparent"] is not null)
            message.Headers.TryAddWithoutValidation("traceparent", request.Headers["traceparent"]);

        message.Headers.TryAddWithoutValidation("Accept", "application/json");

        var response = await _Client.SendAsync(message);

        if (response.Content.Headers.ContentEncoding.Contains("gzip"))
        {
            var originalContent = await response.Content.ReadAsStreamAsync();
            response.Content = new StreamContent(new GZipStream(originalContent, CompressionMode.Decompress));
        }

        return response;
    }

    private async Task SendResponse(HttpResponseMessage response, string content)
    {
        var responseBuffer = Encoding.UTF8.GetBytes(content);

        HttpContext.Response.SendChunked = false;
        HttpContext.Response.ContentType = "application/json";
        HttpContext.Response.ContentLength64 = responseBuffer.Length;
        HttpContext.Response.StatusCode = (int)response.StatusCode;

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Client config request Cloudflare blocked (403), please open issue on GitHub or contact c4t_bot on Discord");
            Console.ResetColor();
        }

        await HttpContext.Response.OutputStream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
        HttpContext.Response.OutputStream.Close();
    }
}
internal sealed class LedgeController : WebApiController
{
    private static HttpClient _Client = new(new HttpClientHandler { UseCookies = false, UseProxy = false, Proxy = null });

    private static string LEDGE_URL => EnsureLedgeUrlIsSet();

    private static LeagueProxyEvents _Events => LeagueProxyEvents.Instance;

    [Route(HttpVerbs.Get, "/", true)]
    public async Task GetLedge()
    {
        if (HttpContext.Request.Url.LocalPath == "/leagues-ledge/v2/notifications")
        {
            return;
        }

        string requestBody;
        using (var reader = new StreamReader(HttpContext.OpenRequestStream()))
        {
            requestBody = await reader.ReadToEndAsync();
        }

        var response = await GetLedge(HttpContext.Request);
        var content = await response.Content.ReadAsStringAsync();

        content = _Events.InvokeClientLedge(content, HttpContext.Request);

        await SendResponse(response, content);
    }
    [Route(HttpVerbs.Post, "/", true)]
    public async Task PostLedge()
    {
        string requestBody;
        using (var reader = new StreamReader(HttpContext.OpenRequestStream()))
        {
            requestBody = await reader.ReadToEndAsync();
        }

        var response = await PostLedge(HttpContext.Request, requestBody);
        var content = await response.Content.ReadAsStringAsync();

        content = _Events.InvokeClientLedge(content, HttpContext.Request);

        await SendResponse(response, content);
    }
    [Route(HttpVerbs.Put, "/", true)]
    public async Task PutLedge()
    {
        string requestBody;
        using (var reader = new StreamReader(HttpContext.OpenRequestStream()))
        {
            requestBody = await reader.ReadToEndAsync();
        }

        var response = await PutLedge(HttpContext.Request, requestBody);
        var content = await response.Content.ReadAsStringAsync();

        content = _Events.InvokeClientLedge(content, HttpContext.Request);

        await SendResponse(response, content);
    }
    private static string EnsureLedgeUrlIsSet()
    {
        var ledgeUrl = App.SharedLeagueEdgeUrl.Get();

        if (string.IsNullOrEmpty(ledgeUrl))
        {
            throw new InvalidOperationException("Ledge URL is not set.");
        }

        return ledgeUrl;
    }

    private async Task<HttpResponseMessage> PutLedge(IHttpRequest request, string body)
    {
        var url = LEDGE_URL + request.RawUrl;

        using var message = new HttpRequestMessage(HttpMethod.Put, url);

        if (request.Headers["accept-encoding"] is not null)
            message.Headers.TryAddWithoutValidation("Accept-Encoding", request.Headers["accept-encoding"]);

        message.Headers.TryAddWithoutValidation("user-agent", request.Headers["user-agent"]);

        if (request.Headers["authorization"] is not null)
            message.Headers.TryAddWithoutValidation("Authorization", request.Headers["authorization"]);

        message.Headers.TryAddWithoutValidation("Content-type", "application/json");

        message.Headers.TryAddWithoutValidation("Accept", "application/json");

        if (!string.IsNullOrEmpty(body))
            message.Content = new StringContent(body, Encoding.UTF8, "application/json");

        if (request.Headers["content-length"] is not null)
        {
            if (long.TryParse(request.Headers["content-length"], out var contentLength))
                message.Content.Headers.ContentLength = contentLength;
        }

        var response = await _Client.SendAsync(message);

        if (response.Content.Headers.ContentEncoding.Contains("gzip"))
        {
            var originalContent = await response.Content.ReadAsStreamAsync();
            response.Content = new StreamContent(new GZipStream(originalContent, CompressionMode.Decompress));
        }

        return response;
    }

    private async Task<HttpResponseMessage> PostLedge(IHttpRequest request, string body)
    {
        var url = LEDGE_URL + request.RawUrl;

        using var message = new HttpRequestMessage(HttpMethod.Post, url);

        if (request.Headers["accept-encoding"] is not null)
            message.Headers.TryAddWithoutValidation("Accept-Encoding", request.Headers["accept-encoding"]);

        message.Headers.TryAddWithoutValidation("user-agent", request.Headers["user-agent"]);

        if (request.Headers["authorization"] is not null)
            message.Headers.TryAddWithoutValidation("Authorization", request.Headers["authorization"]);

        if (request.Headers["content-type"] is not null)
        {
            message.Content = new StringContent(body, null, request.Headers["content-type"]);
        }

        message.Headers.TryAddWithoutValidation("Accept", "application/json");

        if (request.Headers["content-length"] is not null)
        {
            if (long.TryParse(request.Headers["content-length"], out var contentLength))
                message.Content.Headers.ContentLength = contentLength;
        }

        var response = await _Client.SendAsync(message);

        if (response.Content.Headers.ContentEncoding.Contains("gzip"))
        {
            var originalContent = await response.Content.ReadAsStreamAsync();
            response.Content = new StreamContent(new GZipStream(originalContent, CompressionMode.Decompress));
        }

        return response;
    }

    private async Task<HttpResponseMessage> GetLedge(IHttpRequest request)
    {
        var url = LEDGE_URL + request.RawUrl;

        using var message = new HttpRequestMessage(HttpMethod.Get, url);

        if (request.Headers["accept-encoding"] is not null)
            message.Headers.TryAddWithoutValidation("Accept-Encoding", request.Headers["accept-encoding"]);

        message.Headers.TryAddWithoutValidation("user-agent", request.Headers["user-agent"]);

        if (request.Headers["authorization"] is not null)
            message.Headers.TryAddWithoutValidation("Authorization", request.Headers["authorization"]);

        if (request.Headers["Content-type"] is not null)
            message.Headers.TryAddWithoutValidation("Content-Type", request.Headers["Content-type"]);

        if (request.Headers["accept"] is not null)
            message.Headers.TryAddWithoutValidation("Accept", request.Headers["accept"]);

        var response = await _Client.SendAsync(message);

        if (response.Content.Headers.ContentEncoding.Contains("gzip"))
        {
            var originalContent = await response.Content.ReadAsStreamAsync();
            response.Content = new StreamContent(new GZipStream(originalContent, CompressionMode.Decompress));
        }

        return response; 
    }

    private async Task SendResponse(HttpResponseMessage response, string content)
    {
        var responseBuffer = Encoding.UTF8.GetBytes(content);

        HttpContext.Response.SendChunked = false;
        HttpContext.Response.ContentType = "application/json";
        HttpContext.Response.ContentLength64 = responseBuffer.Length;
        HttpContext.Response.StatusCode = (int)response.StatusCode;

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Ledge request Cloudflare blocked (403), please open issue on GitHub or contact c4t_bot on Discord");
            Console.ResetColor();
        }

        await HttpContext.Response.OutputStream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
        HttpContext.Response.OutputStream.Close();
    }
}
internal sealed class GeopassController : WebApiController
{
    private static HttpClient _Client = new(new HttpClientHandler { UseCookies = false, UseProxy = false, Proxy = null });
    private static string GEOPASS_URL => EnsureGeopassUrlIsSet();
    private static LeagueProxyEvents _Events => LeagueProxyEvents.Instance;
    [Route(HttpVerbs.Get, "/", true)]
    public async Task GetGeopass()
    {

        var response = await GetGeopass(HttpContext.Request);
        var content = await response.Content.ReadAsStringAsync();

        content = _Events.InvokeClientGeopass(content, HttpContext.Request);

        await SendResponse(response, content);
    }
    [Route(HttpVerbs.Put, "/", true)]
    public async Task PutGeopass()
    {
        string requestBody;
        using (var reader = new StreamReader(HttpContext.OpenRequestStream()))
        {
            requestBody = await reader.ReadToEndAsync();
        }

        var response = await PutGeopass(HttpContext.Request, requestBody);
        var content = await response.Content.ReadAsStringAsync();

        content = _Events.InvokeClientGeopass(content, HttpContext.Request);

        await SendResponse(response, content);
    }
    private static string EnsureGeopassUrlIsSet()
    {
        var GeopassUrl = App.SharedGeopassUrl.Get();

        if (string.IsNullOrEmpty(GeopassUrl))
        {
            throw new InvalidOperationException("Geopass URL is not set.");
        }

        return GeopassUrl;
    }
    private async Task<HttpResponseMessage> GetGeopass(IHttpRequest request)
    {
        var url = GEOPASS_URL + request.RawUrl;

        using var message = new HttpRequestMessage(HttpMethod.Get, url);

        if (request.Headers["accept-encoding"] is not null)
            message.Headers.TryAddWithoutValidation("Accept-Encoding", request.Headers["accept-encoding"]);

        message.Headers.TryAddWithoutValidation("user-agent", HttpContext.Request.Headers["user-agent"]);

        if (request.Headers["authorization"] is not null)
            message.Headers.TryAddWithoutValidation("Authorization", request.Headers["authorization"]);

        if (request.Headers["x-pas-affinity-hint"] is not null)
            message.Headers.TryAddWithoutValidation("X-PAS-affinity-hint", request.Headers["x-pas-affinity-hint"]);

        if (request.Headers["baggage"] is not null)
            message.Headers.TryAddWithoutValidation("baggage", request.Headers["baggage"]);

        if (request.Headers["traceparent"] is not null)
            message.Headers.TryAddWithoutValidation("traceparent", request.Headers["traceparent"]);

        message.Headers.TryAddWithoutValidation("Accept", "application/json");

        var response = await _Client.SendAsync(message);

        if (response.Content.Headers.ContentEncoding.Contains("gzip"))
        {
            var originalContent = await response.Content.ReadAsStreamAsync();
            response.Content = new StreamContent(new GZipStream(originalContent, CompressionMode.Decompress));
        }

        return response;
    }
    private async Task<HttpResponseMessage> PutGeopass(IHttpRequest request, string body)
    {
        var url = GEOPASS_URL + request.RawUrl;

        using var message = new HttpRequestMessage(HttpMethod.Put, url);

        if (request.Headers["accept-encoding"] is not null)
            message.Headers.TryAddWithoutValidation("Accept-Encoding", request.Headers["accept-encoding"]);

        message.Headers.TryAddWithoutValidation("user-agent", HttpContext.Request.Headers["user-agent"]);

        if (request.Headers["authorization"] is not null)
            message.Headers.TryAddWithoutValidation("Authorization", request.Headers["authorization"]);

        if (request.Headers["baggage"] is not null)
            message.Headers.TryAddWithoutValidation("baggage", request.Headers["baggage"]);

        if (request.Headers["traceparent"] is not null)
            message.Headers.TryAddWithoutValidation("traceparent", request.Headers["traceparent"]);

        if (request.Headers["accept"] is not null)
            message.Headers.TryAddWithoutValidation("Accept", request.Headers["accept"]);

        if (request.Headers["content-length"] is not null)
            message.Headers.TryAddWithoutValidation("Content-Length", request.Headers["content-length"]);

        if (request.Headers["content-type"] is not null)
            message.Headers.TryAddWithoutValidation("Content-Type", request.Headers["content-type"]);

        string requestBody;
        using (var reader = new StreamReader(HttpContext.OpenRequestStream()))
        {
            requestBody = await reader.ReadToEndAsync();
        }

        if (body != null)
        {
            message.Content = new StringContent(body, Encoding.UTF8, request.Headers["content-type"]);
        }
        var response = await _Client.SendAsync(message);

        if (response.Content.Headers.ContentEncoding.Contains("gzip"))
        {
            var originalContent = await response.Content.ReadAsStreamAsync();
            response.Content = new StreamContent(new GZipStream(originalContent, CompressionMode.Decompress));
        }

        return response;
    }
    private async Task SendResponse(HttpResponseMessage response, string content)
    {
        var responseBuffer = Encoding.UTF8.GetBytes(content);

        HttpContext.Response.SendChunked = false;
        HttpContext.Response.ContentType = "application/json";
        HttpContext.Response.ContentLength64 = responseBuffer.Length;
        HttpContext.Response.StatusCode = (int)response.StatusCode;

        if (response.StatusCode == HttpStatusCode.Forbidden)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Geopass request Cloudflare blocked (403), please open issue on GitHub or contact c4t_bot on Discord");
            Console.ResetColor();
        }

        await HttpContext.Response.OutputStream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
        HttpContext.Response.OutputStream.Close();
    }
}
internal sealed class httpProxyServer<T> where T : WebApiController, new()
{
    private WebServer _WebServer;
    private int _Port;

    public string Url => $"http://127.0.0.1:{_Port}";

    public httpProxyServer(int port)
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
    private httpProxyServer<ConfigController> _ConfigServer;
    private httpProxyServer<GeopassController> _GeopassServer;
    private httpProxyServer<LedgeController> _LedgeServer;
    private RiotClient _RiotClient;
    private CancellationTokenSource? _ServerCTS;


    public LeagueProxyEvents Events => LeagueProxyEvents.Instance;

    public LeagueProxy()
    {
        _ConfigServer = new httpProxyServer<ConfigController>(29150); // Port for ConfigServer
        _GeopassServer = new httpProxyServer<GeopassController>(29151);   // Port for 
        _LedgeServer = new httpProxyServer<LedgeController>(29152);   // Port for ledge
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
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine($"Stopping {processName}...");
                    Console.ResetColor();
                    process.WaitForExit();
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    Console.WriteLine($"Sucessfully stopped {processName}");
                    Console.ResetColor();
                }
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Error stopping {processName}, Please contact c4t_bot on Discord if this issue persists.");
                Console.ResetColor();
            }
        }

        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.WriteLine("Starting now with quality improvements.");
        Console.ResetColor();
    }

    public void Start(out string configServerUrl, out string ledgeServerUrl, out string GeopassServerUrl)
    {
        if (_ServerCTS is not null)
            throw new Exception("Proxy servers are already running!");

        TerminateRiotServices();
        LoadProductInstallPath();
        Logger.UnregisterLogger<ConsoleLogger>();
        _ServerCTS = new CancellationTokenSource();

        _ConfigServer.Start(_ServerCTS.Token);
        configServerUrl = _ConfigServer.Url;

        _LedgeServer.Start(_ServerCTS.Token);
        ledgeServerUrl = _LedgeServer.Url;

        _GeopassServer.Start(_ServerCTS.Token);
        GeopassServerUrl = _GeopassServer.Url;
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
        Console.WriteLine("Proxy services successfully stopped.");
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

        Start(out _, out _, out _);
        return LaunchRCS(args);
    }
}