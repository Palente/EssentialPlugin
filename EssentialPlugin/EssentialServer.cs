using log4net;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using MiNET;
using MiNET.Net;
using MiNET.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EssentialPlugin
{
	public class EssentialServer : IServer
	{
		private MiNetServer _server;
		private EssentialPlayerFactory factory;
		private static readonly ILog Log = LogManager.GetLogger(typeof(EssentialServer));
		public IConfiguration config;
		private List<string> OpsList = new();
		private List<EssentialPlayer> playersList = new();
		public EssentialServer(MiNetServer server)
		{
			_server = server;
			if (EssentialPlayerFactory.instance is EssentialPlayerFactory) factory = EssentialPlayerFactory.instance;
			else factory = new EssentialPlayerFactory();
			/*var builder = new ConfigurationBuilder()
				.SetBasePath(AppContext.BaseDirectory)
				.AddJsonFile(path: "config.json");
			this.config = builder.Build();*/

			//OpList
			string opsFilePath = Directory.GetCurrentDirectory() + "/ops.txt";
			String fileContent = string.Empty;
			if (File.Exists(opsFilePath))
			{
				fileContent = File.ReadAllText(opsFilePath);
				foreach (string rawLine in fileContent.Split(new[] { "\r\n", "\n", Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
				{
					string line = rawLine.Trim();
					if (line.StartsWith("#")) continue; //It's a comment or not a key value pair.
					OpsList.Add(line.ToLower());
				}
			}
			else
			{
				Log.Warn("Didn't found ops.txt in " + Directory.GetCurrentDirectory() + "creating ops.txt");
				var file = File.Create(opsFilePath);
			}
		}
		public IMcpeMessageHandler CreatePlayer(INetworkHandler session, PlayerInfo playerInfo)
		{
			var player = (EssentialPlayer) factory.CreatePlayer(_server, session.GetClientEndPoint(), playerInfo);
			player.NetworkHandler = session;
			player.CertificateData = playerInfo.CertificateData;
			player.Username = playerInfo.Username;
			player.ClientUuid = playerInfo.ClientUuid;
			player.ServerAddress = playerInfo.ServerAddress;
			player.ClientId = playerInfo.ClientId;
			player.Skin = playerInfo.Skin;
			player.PlayerInfo = playerInfo;
            player.PlayerJoin += OnPlayerJoin;
            player.PlayerLeave += OnPlayerLeave;
            player.SetOp(player.IsOp());
            
            //Subscribe to all events
            player.OnMessageReceived += (sender, eventArgs) =>
	            EventListener.getInstance().OnChatReceived(sender, eventArgs);
            return player;
		}

        public bool IsOp(string name)
        {
			return OpsList.Contains(name.ToLower());
        }

		public void SetOp(string name, bool isOp=true)
        {
			if (!isOp) OpsList.Remove(name.ToLower());
			else OpsList.Add(name.ToLower());
        }

		private void OnPlayerJoin(object sender, PlayerEventArgs e)
		{
			//Not his job!
			var player = (EssentialPlayer) e.Player;
			playersList.Add(player);
			BroadcastMessage($"§a[+] §e{player.Username} joined the server!");
		}

		private void OnPlayerLeave(object sender, PlayerEventArgs e)
		{
			// Not his job
			var player = (EssentialPlayer)e.Player;
			playersList.Remove(player);
			BroadcastMessage($"§c[-] §e{player.Username} left the server!");
		}
		/// <summary>
		/// Get a Player
		/// </summary>
		/// <param name="name">name of the player</param>
		/// <returns>EssentialPlayer</returns>
		public EssentialPlayer GetPlayer(string name)
			=> playersList.Find(p => p.Username.Equals(name, StringComparison.CurrentCultureIgnoreCase));
		public EssentialPlayer GetPlayer(Player player)
			=> playersList.Find(p => p.Username == player.Username);
		/// <summary>
		/// Broadcast a message to all server!
		/// </summary>
		/// <param name="message">Message to Broadcast</param>
		/// <param name="admin">Broadcast only to OP</param>
		public void BroadcastMessage(string message, bool admin = false)
		{
			if(admin) playersList.ForEach(p => { if (p.IsOp()) p.SendMessage(message); });
			else playersList.ForEach(p => p.SendMessage(message));
		}
	}
	public class EssentialServerManager : IServerManager
	{
		public EssentialServer IServer;
		
		public EssentialServerManager(MiNetServer server)
		{
			IServer = new EssentialServer(server);
		}
		IServer IServerManager.GetServer()
		{
			return IServer;
		}
	}
}
