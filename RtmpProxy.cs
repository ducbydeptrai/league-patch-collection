using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;

namespace LeaguePatchCollection;

public class RTMPProxy
{
    private const int Port = 29153;
    private const int RTMPPort = 2099; 
    private const string RTMPServerAddress = "feapp.na1.lol.pvp.net"; 

    public async Task RunAsync()
    {
        var listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();
        Console.WriteLine($"[RTMP] Waiting for client on port {Port}...");

        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = HandleClient(client);
        }
    }

    private async Task HandleClient(TcpClient client)
    {
        NetworkStream networkStream = null;
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
                await sslStream.AuthenticateAsClientAsync(sslOptions);
                serverStream = sslStream;
                Console.WriteLine("[RTMP] Connection to server established.");
            }

            var clientToServerTask = ForwardDataAsync(networkStream, serverStream, "Client -> RTMP Server");
            var serverToClientTask = ForwardDataAsync(serverStream, networkStream, "RTMP Server -> Client");

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

    private async Task ForwardDataAsync(Stream source, Stream destination, string direction)
    {
        var buffer = new byte[128]; 
        int bytesRead;

        try
        {
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                Console.WriteLine($"[RTMP] {direction}: {bytesRead} bytes read.");

                if (bytesRead >= 1)
                {
                    //Console.WriteLine($"[RTMP] {direction} Command: {buffer[0]}");
                    //Console.WriteLine($"[RTMP] {direction} Raw Data: {BitConverter.ToString(buffer, 0, bytesRead)}");
                }

                await destination.WriteAsync(buffer, 0, bytesRead);
                await destination.FlushAsync();
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
        if (sslPolicyErrors == SslPolicyErrors.None)
        {
            return true;
        }

        return true; // Allow self-signed certificates
    }

    private static bool RequiresTLS()
    {
        return true;
    }
}
