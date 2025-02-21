using System.IO.Compression;
using System.Net.Security;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Text;
using System.Diagnostics;

namespace LeaguePatchCollection
{
    public partial class RMSProxy
    {
        private const int Port = 29155;
        private const int RMSPort = 443;

        private readonly CancellationTokenSource _cts;
        private readonly TcpListener _listener;

        public RMSProxy()
        {
            _cts = new CancellationTokenSource();
            _listener = new TcpListener(IPAddress.Any, Port);
        }

        public async Task RunAsync(CancellationToken cancellationToken)
        {
            var listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine($"[RMS] Waiting for client on port {Port}...");

            try
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    var client = await listener.AcceptTcpClientAsync(cancellationToken);
                    _ = HandleClient(client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[RMS] Proxy error: {ex.Message}");
            }
            finally
            {
                listener.Stop();
                Console.WriteLine("[RMS] Proxy successfully stopped.");
            }
        }

        private static async Task HandleClient(TcpClient client)
        {
            try
            {
                var rmsHost = HttpProxy._rmsHost;
                rmsHost = rmsHost?.Replace("wss://", "");

                using var tcpClient = new TcpClient(rmsHost!, RMSPort);
                Stream serverStream = tcpClient.GetStream();

                var sslStream = new SslStream(serverStream, false, ValidateServerCertificate);
                await sslStream.AuthenticateAsClientAsync(rmsHost!);
                serverStream = sslStream;
                Console.WriteLine("[RMS] Connection to server established.");

                await HandleWebSocketHandshakeAsync(client.GetStream(), serverStream);

                var clientToServerTask = ForwardDataAsync(client.GetStream(), serverStream, "Client -> RMS Server");
                var serverToClientTask = ForwardDataAsync(serverStream, client.GetStream(), "RMS Server -> Client");

                await Task.WhenAny(clientToServerTask, serverToClientTask);
            }
            catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException)
            {
                Console.WriteLine($"[RMS] Client disconnected or connection error: {ex.Message}");
            }
        }

        private static async Task HandleWebSocketHandshakeAsync(Stream clientStream, Stream serverStream)
        {
            var clientReader = new StreamReader(clientStream, Encoding.ASCII);
            var clientWriter = new StreamWriter(clientStream, Encoding.ASCII) { AutoFlush = true };
            var serverWriter = new StreamWriter(serverStream, Encoding.ASCII) { AutoFlush = true };

            string? requestLine = await clientReader.ReadLineAsync();
            await serverWriter.WriteLineAsync(requestLine); // Write the request line to the server

            while (true)
            {
                string? header = await clientReader.ReadLineAsync();
                if (string.IsNullOrEmpty(header)) break;

                if (header.StartsWith("Host: "))
                {
                    header = $"Host: {HttpProxy._rmsHost!.Replace("wss://", "")}";
                }
                else if (header.StartsWith("Origin: "))
                {
                    header = $"Origin: {HttpProxy._rmsHost!.Replace("wss://", "")}";
                }

                await serverWriter.WriteLineAsync(header);
            }
            await serverWriter.WriteLineAsync();

            var serverReader = new StreamReader(serverStream, Encoding.ASCII);

            string? responseLine = await serverReader.ReadLineAsync();
            Console.WriteLine($"[RMS] {responseLine}");
            await clientWriter.WriteLineAsync(responseLine ?? string.Empty);

            while (true)
            {
                string? header = await serverReader.ReadLineAsync();
                if (string.IsNullOrEmpty(header)) break;

                await clientWriter.WriteLineAsync(header);
            }
            await clientWriter.WriteLineAsync();
        }

        private static async Task ForwardDataAsync(Stream source, Stream destination, string direction)
        {
            var buffer = new byte[8192];
            int bytesRead;

            while ((bytesRead = await source.ReadAsync(buffer)) > 0)
            {
                string decodedMessage;

                using (var memoryStream = new MemoryStream(buffer, 0, bytesRead))
                using (var outputStream = new MemoryStream())
                {
                    while (memoryStream.Position < memoryStream.Length)
                    {
                        if (IsGzipHeader(memoryStream))
                        {
                            using var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress, leaveOpen: true);
                            gzipStream.CopyTo(outputStream);
                        }
                        else
                        {
                            int currentByte = memoryStream.ReadByte();
                            if (currentByte == -1) break;
                            outputStream.WriteByte((byte)currentByte);
                        }
                    }

                    decodedMessage = Encoding.UTF8.GetString(outputStream.ToArray());
                }

                if (RankedRestriction().IsMatch(decodedMessage))
                {
                    continue; // Skip sending this message to the client to block popup about ranked restriction
                }

                if (HawoltBypass().IsMatch(decodedMessage))
                {
                    continue; // big mighty hawolt ban bypass
                }

                if (BlockVanguardSessionCheck().IsMatch(decodedMessage))
                {
                    Trace.WriteLine("[INFO] ATTEMPING TO BYPASS GAMEFLOW KICK/BLOCK: BLOCKING MESSAING " + decodedMessage);
                    continue; // Block this message so the client doesnt know gameflow detecting no vanguard session
                }

                await destination.WriteAsync(buffer.AsMemory(0, bytesRead));
            }
            Console.WriteLine($"[RMS] {direction}: Connection closed.");
        }

        private static bool IsGzipHeader(Stream stream)
        {
            if (stream.Length - stream.Position < 2) return false;

            long originalPosition = stream.Position;

            try
            {
                int firstByte = stream.ReadByte();
                int secondByte = stream.ReadByte();

                stream.Position = originalPosition;

                return firstByte == 0x1F && secondByte == 0x8B;
            }
            catch
            {
                stream.Position = originalPosition;
                return false;
            }
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate? certificate, X509Chain? chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }
            return true;
        }

        public void Stop()
        {
            if (_cts == null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("[RMS] Proxy service is not running.");
                Console.ResetColor();
                return;
            }

            _cts.Cancel();
            _listener.Stop();
            Console.WriteLine("[RMS] Proxy has been stopped.");
        }

        [GeneratedRegex(@"RANKED_RESTRICTION")]
        private static partial Regex RankedRestriction();
        [GeneratedRegex(@"gaps-session-service")]
        private static partial Regex HawoltBypass();
        [GeneratedRegex(@"PLAYER_LACKS_VANGUARD_SESSION")]
        private static partial Regex BlockVanguardSessionCheck();

    }
}
