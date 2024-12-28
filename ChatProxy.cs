using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using static LeaguePatchCollection.App;

namespace LeaguePatchCollection;

public class XMPPProxy
{
    private const int Port = 29152; // Local server listening port (insecure)
    private const int XMPPPort = 5223; // Real XMPP server secure port

    public async Task RunAsync()
    {
        var listener = new TcpListener(IPAddress.Any, Port);
        listener.Start();
        //Console.WriteLine($"[XMPP] Proxy started on port {Port}");

        while (true)
        {
            var client = await listener.AcceptTcpClientAsync();
            _ = Task.Run(() => HandleClient(client));
        }
    }

    private async Task HandleClient(TcpClient client)
    {
        try
        {
            using (var networkStream = client.GetStream())
            {
                var chatHost = SharedChatHost.Get();
                if (string.IsNullOrEmpty(chatHost))
                {
                    Console.WriteLine("[XMPP] No valid chat.host found, continuing without setting it.");
                }

                using (var tcpClient = new TcpClient(chatHost ?? throw new InvalidOperationException("chat.host is null or empty"), XMPPPort))
                using (var sslStream = new SslStream(tcpClient.GetStream(), false, ValidateServerCertificate))
                {
                    var sslOptions = new SslClientAuthenticationOptions
                    {
                        TargetHost = chatHost,
                        EnabledSslProtocols = SslProtocols.Tls12
                    };

                    await sslStream.AuthenticateAsClientAsync(sslOptions);
                    //Console.WriteLine($"[XMPP] TLS Connection established to {xmppTarget}");

                    var clientToServerTask = ForwardDataAsync(networkStream, sslStream, "Client -> XMPP Server");
                    var serverToClientTask = ForwardDataAsync(sslStream, networkStream, "XMPP Server -> Client");

                    await Task.WhenAny(clientToServerTask, serverToClientTask);
                }
            }

            client.Close();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[XMPP] Error handling client: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
        }
    }

    private async Task ForwardDataAsync(Stream source, Stream destination, string direction)
    {
        var buffer = new byte[8192];
        int bytesRead;

        try
        {
            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                if (direction == "Client -> XMPP Server")
                {
                    message = Regex.Replace(message, @"<show>chat</show>", "<show>offline</show>");
                    message = Regex.Replace(message, @"<st>chat</st>", "<st>offline</st>");
                    message = Regex.Replace(message, "<league_of_legends>.*?</league_of_legends>", string.Empty);
                    message = Regex.Replace(message, "<valorant>.*?</valorant>", string.Empty);
                    message = Regex.Replace(message, "<bacon>.*?</bacon>", string.Empty);
                }

                //Console.WriteLine($"{direction} Message: {message}");

                var modifiedBuffer = Encoding.UTF8.GetBytes(message);
                await destination.WriteAsync(modifiedBuffer, 0, modifiedBuffer.Length);
                await destination.FlushAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[XMPP] Error during {direction}: {ex.Message}");
        }
    }

    private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
    {
        if (sslPolicyErrors == SslPolicyErrors.None)
        {
            return true;
        }

        Console.WriteLine($"[XMPP] Certificate error: {sslPolicyErrors}");
        return true; // Allow self-signed certificates
    }
}
