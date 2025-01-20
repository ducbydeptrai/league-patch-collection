using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using static LeaguePatchCollection.App;
using System.Text.RegularExpressions;

namespace LeaguePatchCollection
{
    public class RMSProxy
    {
        private const int Port = 29155;
        private const int RMSPort = 443;

        public async Task RunAsync()
        {
            var listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine($"[RMS] Waiting for client on port {Port}...");

            try
            {
                while (true)
                {
                    var client = await listener.AcceptTcpClientAsync();
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

        private async Task HandleClient(TcpClient client)
        {
            try
            {
                var rmsHost = SharedRmsHost.Get();

                rmsHost = rmsHost.Replace("wss://", "");

                using var tcpClient = new TcpClient(rmsHost, RMSPort);
                Stream serverStream = tcpClient.GetStream();

                if (RequiresTLS())
                {
                    var sslStream = new SslStream(serverStream, false, ValidateServerCertificate);
                    await sslStream.AuthenticateAsClientAsync(rmsHost);
                    serverStream = sslStream;
                    Console.WriteLine("[RMS] Connection to server established.");
                }

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

        private async Task HandleWebSocketHandshakeAsync(Stream clientStream, Stream serverStream)
        {
            var clientReader = new StreamReader(clientStream, Encoding.ASCII);
            var clientWriter = new StreamWriter(clientStream, Encoding.ASCII) { AutoFlush = true };
            var serverWriter = new StreamWriter(serverStream, Encoding.ASCII) { AutoFlush = true };

            string requestLine = await clientReader.ReadLineAsync();
            await serverWriter.WriteLineAsync(requestLine); // Write the request line to the server

            string header;
            while (!string.IsNullOrEmpty(header = await clientReader.ReadLineAsync()))
            {
                if (header.StartsWith("Host: "))
                {
                    header = $"Host: {SharedRmsHost.Get().Replace("wss://", "")}"; // Correct Host header
                }
                else if (header.StartsWith("Origin: "))
                {
                    header = $"Origin: {SharedRmsHost.Get().Replace("wss://", "")}"; // Correct Origin header
                }

                await serverWriter.WriteLineAsync(header);
            }
            await serverWriter.WriteLineAsync();  

            var serverReader = new StreamReader(serverStream, Encoding.ASCII);
            string responseLine = await serverReader.ReadLineAsync();
            Console.WriteLine($"[RMS] {responseLine}");
            await clientWriter.WriteLineAsync(responseLine);

            while (!string.IsNullOrEmpty(header = await serverReader.ReadLineAsync()))
            {
                await clientWriter.WriteLineAsync(header);
            }
            await clientWriter.WriteLineAsync();
        }


        private async Task ForwardDataAsync(Stream source, Stream destination, string direction)
        {
            var buffer = new byte[8192];
            int bytesRead;

            while ((bytesRead = await source.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                string decodedMessage;

                using (var memoryStream = new MemoryStream(buffer, 0, bytesRead))
                using (var outputStream = new MemoryStream())
                {
                    while (memoryStream.Position < memoryStream.Length)
                    {
                        if (IsGzipHeader(memoryStream))
                        {
                            using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Decompress, leaveOpen: true))
                            {
                                gzipStream.CopyTo(outputStream);
                            }
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

                if (Regex.IsMatch(decodedMessage, "parties/v1/notifications"))
                {
                decodedMessage = Regex.Replace(decodedMessage, "\"activityLocked\":true", "\"activityLocked\":false");
                }

                if (Regex.IsMatch(decodedMessage, @"RANKED_RESTRICTION"))
                {
                    continue; // Skip sending this message to the client to block popup about ranked restriction
                }

                if (Regex.IsMatch(decodedMessage, @"gaps-session-service"))
                {
                    //Console.WriteLine($"[RMS] Blocking frame containing 'gaps-session-service': {decodedMessage}");
                    continue; // hawolt ban bypass exploit
                }

                if (direction == "RMS Server -> Client")
                {
                    //Console.ForegroundColor = ConsoleColor.Cyan;
                    //Console.WriteLine($"[RMS] {direction}: Decoded message: {decodedMessage}");
                    //Console.ResetColor();
                }

                await destination.WriteAsync(buffer, 0, bytesRead);
            }
            Console.WriteLine($"[RMS] {direction}: Connection closed.");
        }

        private bool IsGzipHeader(Stream stream)
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


        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None || true; // Allow self-signed certificates
        }

        private static bool RequiresTLS()
        {
            return true;
        }
    }
}