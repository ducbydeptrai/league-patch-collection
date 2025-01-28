using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Threading;
using System.Threading.Tasks;

namespace LeaguePatchCollection;

public class RTMPProxy
{
    private const int Port = 29154;
    private const int RTMPPort = 2099;
    private const string RTMPServerAddress = "feapp.na1.lol.pvp.net";
    private CancellationTokenSource _cancellationTokenSource;

    public async Task RunAsync()
    {
        _cancellationTokenSource = new CancellationTokenSource();
        var listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();
        Console.WriteLine($"[RTMP] Proxy is not implemented/available yet...");

        try
        {
            while (!_cancellationTokenSource.Token.IsCancellationRequested)
            {
                if (listener.Pending())
                {
                    var client = await listener.AcceptTcpClientAsync();
                    _ = HandleClient(client, _cancellationTokenSource.Token);
                }
                await Task.Delay(100, _cancellationTokenSource.Token); // Small delay to prevent tight loop
            }
        }
        catch (OperationCanceledException)
        {
            Console.WriteLine("[RTMP] Stopping proxy...");
        }
        finally
        {
            listener.Stop();
            _cancellationTokenSource.Dispose();
            Console.WriteLine("[RTMP] Proxy sucessfully stopped.");
        }
    }

    private async Task HandleClient(TcpClient client, CancellationToken cancellationToken)
    {
        NetworkStream? networkStream = null;
        try
        {
            networkStream = client.GetStream();
            Console.WriteLine("[RTMP] Client connected.");

            using var tcpClient = new TcpClient(RTMPServerAddress, RTMPPort);
            Stream serverStream = tcpClient.GetStream();

            if (RequiresTLS())
            {
                var sslStream = new SslStream(serverStream, false, ValidateServerCertificate);
                var sslOptions = new SslClientAuthenticationOptions
                {
                    TargetHost = RTMPServerAddress,
                    EnabledSslProtocols = SslProtocols.Tls12
                };
                await sslStream.AuthenticateAsClientAsync(sslOptions, cancellationToken);
                serverStream = sslStream;
                Console.WriteLine("[RTMP] Connection to server established.");
            }

            var clientToServerTask = ForwardDataAsync(networkStream, serverStream, "Client -> RTMP Server", cancellationToken);
            var serverToClientTask = ForwardDataAsync(serverStream, networkStream, "RTMP Server -> Client", cancellationToken);

            await Task.WhenAny(clientToServerTask, serverToClientTask);
        }
        catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException)
        {
            Console.WriteLine($"[RTMP] Client disconnected or connection error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("[RTMP] Client disconnected.");
            client?.Close();
            networkStream?.Dispose();
        }
    }

    private async Task ForwardDataAsync(Stream source, Stream destination, string direction, CancellationToken cancellationToken)
    {
        var buffer = new byte[128];
        int bytesRead;

        try
        {
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length, cancellationToken)) > 0)
            {
                await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken);
                await destination.FlushAsync(cancellationToken);
            }

            Console.WriteLine($"[RTMP] {direction}: Connection closed.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[RTMP] Error during {direction}: {ex.Message}");
        }
    }

    private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        return sslPolicyErrors == SslPolicyErrors.None || true; // Allow self-signed certificates
    }

    private static bool RequiresTLS()
    {
        return true;
    }

    public void Stop()
    {
        _cancellationTokenSource?.Cancel();
    }
}