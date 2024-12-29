using System.Net.Sockets;
using System.Net;

public class RTMPProxy
{
    private const int ListenPort = 29153; // Local server listening port
    private const string ForwardHost = "feapp.na1.lol.pvp.net"; // Forwarding host
    private const int ForwardPort = 2099; // Forwarding port

    public async Task RunAsync()
    {
        try
        {
            var listener = new TcpListener(IPAddress.Any, ListenPort);
            listener.Start();
            Console.WriteLine($"[RTMP] Proxy listening on port {ListenPort} and forwarding to {ForwardHost}:{ForwardPort}");

            while (true)
            {
                var client = await listener.AcceptTcpClientAsync();
                Console.WriteLine("[RTMP] Client connected.");
                _ = HandleClient(client);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[RTMP] Error starting proxy: {ex.Message}");
        }
    }

    private async Task HandleClient(TcpClient client)
    {
        try
        {
            using (var clientStream = client.GetStream())
            using (var forwardClient = new TcpClient(ForwardHost, ForwardPort)) // Connect to forwarding server
            using (var forwardStream = forwardClient.GetStream())
            {
                Console.WriteLine($"[RTMP] Connected to forwarding server at {ForwardHost}:{ForwardPort}");

                var clientToServerTask = ForwardDataAsync(clientStream, forwardStream, "Client -> Server");
                var serverToClientTask = ForwardDataAsync(forwardStream, clientStream, "Server -> Client");

                await Task.WhenAny(clientToServerTask, serverToClientTask);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[RTMP] Error handling client: {ex.Message}");
        }
        finally
        {
            client.Close();
            Console.WriteLine("[RTMP] Client disconnected.");
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
                Console.WriteLine($"[RTMP] {direction}: {bytesRead} bytes");
                await destination.WriteAsync(buffer, 0, bytesRead);
                await destination.FlushAsync();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[RTMP] Error during {direction}: {ex.Message}");
        }
    }
}
