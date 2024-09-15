using MiNET;
using System;
using System.Collections.Generic;
using System.Text;
using EssentialPlugin.Events;

namespace EssentialPlugin
{
    public class EventListener
    {
        private EssentialPlugin plugin;
        private static EventListener _instance;
        public EventListener(EssentialPlugin caller)
        { 
	        plugin = caller;
	        _instance = this;
        }

		public void OnJoin(Player player)
		{
			player = (EssentialPlayer)player;
		}
		
		public void OnLeave(Player player)
		{
			player = (EssentialPlayer)player;
		}
		public void OnChatReceived(object sender, MessageReceivedEventArgs eventArgs)
		{
			var message = eventArgs.Message;
			var player = eventArgs.Player;
			if (message.Contains("ff"))
			{
				//never ff
				eventArgs.Cancelled = true;
				eventArgs.Player.SendMessage("§cYou can't say that!");
			}
			eventArgs.FormattedMessage = $"§8{player.Username}§7: §e{message}";
		}


		public static EventListener getInstance()
		{
			return _instance;
		}
	}
}
