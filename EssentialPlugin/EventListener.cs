using MiNET;
using System;
using System.Collections.Generic;
using System.Text;

namespace EssentialPlugin
{
    public class EventListener
    {
        private EssentialPlugin plugin;
        public EventListener(EssentialPlugin caller) => plugin = caller;

		public void OnJoin(Player player)
		{
			player = (EssentialPlayer)player;
		}

		public void OnLeave(Player player)
		{
			player = (EssentialPlayer)player;
		}
	}
}
