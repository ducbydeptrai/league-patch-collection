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

namespace LeaguePatchCollection
{
    public class XMPPProxy
    {
        private const int Port = 29153;
        private const int XMPPPort = 5223;
        private static bool enableOffline = false;
        private bool _WelcomeMessageSent = false;

        public async Task RunAsync()
        {
            _ = Task.Run(() => HandleConsoleInput());

            var listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();
            Console.WriteLine($"[XMPP] Waiting for client on port {Port}...");

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
                Console.WriteLine("[XMPP] Client connected.");

                var chatHost = SharedChatHost.Get();
                if (string.IsNullOrEmpty(chatHost))
                {
                    Console.WriteLine("[XMPP] No valid chat host found. Disconnecting client.");
                    return;
                }

                using var tcpClient = new TcpClient(chatHost, XMPPPort);
                using var sslStream = new SslStream(tcpClient.GetStream(), false, ValidateServerCertificate);

                var sslOptions = new SslClientAuthenticationOptions
                {
                    TargetHost = chatHost,
                    EnabledSslProtocols = SslProtocols.Tls12
                };
                await sslStream.AuthenticateAsClientAsync(sslOptions);

                Console.WriteLine("[XMPP] Connection to server established.");

                var clientToServerTask = ForwardDataAsync(networkStream, sslStream, "Client -> XMPP Server");
                var serverToClientTask = ForwardDataAsync(sslStream, networkStream, "XMPP Server -> Client");

                await Task.WhenAny(clientToServerTask, serverToClientTask);
            }
            catch (Exception ex) when (ex is IOException || ex is ObjectDisposedException)
            {
                Console.WriteLine($"[XMPP] Client disconnected or connection error: {ex.Message}");
            }
            finally
            {
                Console.WriteLine("[XMPP] Client disconnected.");
                client?.Close();
                networkStream?.Dispose();
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

                    const string rosterTag = "<query xmlns='jabber:iq:riotgames:roster'>";

                    if (direction == "XMPP Server -> Client" && message.Contains(rosterTag))
                    {
                        string fakePlayer =
                            "<item jid='00000000-0000-0000-0000-000000000000@na1.pvp.net' name='League Patch Collection' subscription='both' puuid='00000000-0000-0000-0000-000000000000'>" +
                            "<note>This is an automated service by League Patch Collection - The fan-favorite League Client mod menu and Vanguard bypass.</note>" +
                            "<group priority='9999'>Third Party</group>" +
                            "<state>online</state>" +
                            "<id name='League Patch Collection' tagline='Free'/>" +
                            "<platforms><riot name='League Patch Collection' tagline='Free'/></platforms>" +
                            "<lol name='League Patch Collection'/>" +
                            "</item>";

                        message = message.Insert(message.IndexOf(rosterTag, StringComparison.Ordinal) + rosterTag.Length, fakePlayer);

                        _ = Task.Run(() => SendCustomPacket(destination));
                    }

                    if (direction == "Client -> XMPP Server" && enableOffline)
                    {
                        message = Regex.Replace(message, @"<show>chat</show>", "<show>offline</show>");
                        message = Regex.Replace(message, @"<st>chat</st>", "<st>offline</st>");
                        message = Regex.Replace(message, @"<show>away</show>", "<show>offline</show>");
                        message = Regex.Replace(message, @"<st>away</st>", "<st>offline</st>");
                        message = Regex.Replace(message, "<league_of_legends>.*?</league_of_legends>", string.Empty);
                        message = Regex.Replace(message, "<valorant>.*?</valorant>", string.Empty);
                        message = Regex.Replace(message, "<bacon>.*?</bacon>", string.Empty);
                    }

                    if (direction == "Client -> XMPP Server")
                    {
                        if (message.Contains("00000000-0000-0000-0000-000000000000@na1.pvp.net"))
                        {
                            continue;
                        }
                    }

                    var modifiedBufferFinal = Encoding.UTF8.GetBytes(message);
                    await destination.WriteAsync(modifiedBufferFinal, 0, modifiedBufferFinal.Length);
                    await destination.FlushAsync();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[XMPP] Error during {direction}: {ex.Message}");
            }
        }
        private async Task SendCustomPacket(Stream destination)
        {
            var randomStanzaId = Guid.NewGuid();
            var unixTimeMilliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            var presenceMessage =
                $"<presence from='00000000-0000-0000-0000-000000000000@na1.pvp.net/RC-LeaguePatchCollection' id='presence_{randomStanzaId}'>" +
                "<games>" +
                $"<keystone><st>chat</st><s.t>{unixTimeMilliseconds}</s.t><s.p>keystone</s.p></keystone>" +
                $"<league_of_legends><st>chat</st><s.t>{unixTimeMilliseconds}</s.t><s.p>league_of_legends</s.p><s.c>live</s.c><m>Active and Working</m>" +
                $"<p>{{\"championId\":\"\",\"gameQueueType\":\"\",\"gameStatus\":\"outOfGame\",\"legendaryMasteryScore\":\"\",\"level\":\"\",\"mapId\":\"\",\"profileIcon\":\"-1\",\"puuid\":\"\",\"rankedLeagueDivision\":\"\",\"rankedLeagueQueue\":\"\",\"rankedLeagueTier\":\"\",\"rankedLosses\":\"\",\"rankedPrevSeasonDivision\":\"\",\"rankedPrevSeasonTier\":\"\",\"rankedSplitRewardLevel\":\"\",\"rankedWins\":\"\",\"regalia\":\"\",\"skinVariant\":\"\",\"skinname\":\"\"}}</p>" +
                "</league_of_legends>" +
                $"<valorant><st>away</st><s.t>{unixTimeMilliseconds}</s.t><s.p>valorant</s.p><s.r>PC</s.r><m>Active and Working</m>" +
                $"<p>eyJpc1ZhbGlkIjp0cnVlLCJwYXJ0eUlkIjoiMDAwMDAwMDAtMDAwMC0wMDAwLTAwMDAtMDAwMDAwMDAwMDAwIiwicGFydHlDbGllbnRWZXJzaW9uIjoidW5rbm93biIsImFjY291bnRMZXZlbCI6OTk5fQ==</p>" +
                "</valorant>" +
                $"<bacon><st>away</st><s.t>{unixTimeMilliseconds}</s.t><s.l>bacon_availability_online</s.l><s.p>bacon</s.p></bacon>" +
                "</games>" +
                "<show>chat</show>" +
                "<platform>riot</platform>" +
                "<status></status>" +
                "</presence>";

            var presenceBytes = Encoding.UTF8.GetBytes(presenceMessage);

            await destination.WriteAsync(presenceBytes, 0, presenceBytes.Length);
            if (!_WelcomeMessageSent)
            {
                _WelcomeMessageSent = true;
                _ = Task.Run(() => SendFirstMessage(destination));
            }
        }

        private async Task SendFirstMessage(Stream destination)
        {
            await Task.Delay(1000);
            var randomStanzaId = Guid.NewGuid();
            var stamp = DateTime.UtcNow.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss.fff");

            var FirstMessage =
                $"<message from='00000000-0000-0000-0000-000000000000@na1.pvp.net/RC-LeaguePatchCollection' stamp='{stamp}' id='fake-{stamp}' type='chat'><body>Welcome to League Patch Collection, created by Cat Bot. This tool is free and open-source at https://github.com/Cat1Bot/league-patch-collection - IF YOU PAID FOR THIS, YOU GOT SCAMMED.</body></message>";

            var messageBytes = Encoding.UTF8.GetBytes(FirstMessage);

            await destination.WriteAsync(messageBytes, 0, messageBytes.Length);
            _ = Task.Run(() => SendSecondMessage(destination));
        }
        private async Task SendSecondMessage(Stream destination)
        {
            await Task.Delay(1000);
            var randomStanzaId = Guid.NewGuid();
            var stamp = DateTime.UtcNow.AddSeconds(1).ToString("yyyy-MM-dd HH:mm:ss.fff");

            var SecondMessage =
                $"<message from='00000000-0000-0000-0000-000000000000@na1.pvp.net/RC-LeaguePatchCollection' stamp='{stamp}' id='fake-{stamp}' type='chat'><body>Contact || Discord: c4t_bot , Reddit: u/Cat_Bot4 || Donate || Venmo: @Cat_Bot</body></message>";

            var SecondmessageBytes = Encoding.UTF8.GetBytes(SecondMessage);

            await destination.WriteAsync(SecondmessageBytes, 0, SecondmessageBytes.Length);
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            if (sslPolicyErrors == SslPolicyErrors.None)
            {
                return true;
            }

            return true; // Allow self-signed certificates
        }

        private static void HandleConsoleInput()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("--------------------------------------------");
            Console.WriteLine("[XMPP] Type 0 to activate offline and 1 to disable it. NOTE: there is about a 1 minute delay on the backend when updating status");
            Console.WriteLine("--------------------------------------------");
            Console.ResetColor();

            while (true)
            {
                var input = Console.ReadLine();
                if (input == "0")
                {
                    enableOffline = true;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("[XMPP] Offline mode activated. Despite what League client may be showing, you are appearing offline and your friends cannot invite you, however, you can still invite them");
                    Console.WriteLine("--------------------------------------------");
                    Console.ResetColor();
                }
                else if (input == "1")
                {
                    enableOffline = false;
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("--------------------------------------------");
                    Console.WriteLine("[XMPP] Online mode activated.");
                    Console.WriteLine("--------------------------------------------");
                    Console.ResetColor();
                }
                else
                {
                    Console.WriteLine("[XMPP] Invalid input. Type 0 to activate offline mode or 1 to disable it.");
                }
            }
        }
    }
}