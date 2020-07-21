using EssentialPlugin.Commands;
using log4net;
using MiNET;
using MiNET.Net;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using System;
using System.Collections.Generic;

namespace EssentialPlugin
{
    [Plugin(Author = "Palente", Description = "Essential things for a server", PluginName = "Essential", PluginVersion = "0.0.1")]
    public class EssentialPlugin : Plugin, IStartup
    {
		public readonly ILog Log = LogManager.GetLogger(typeof(EssentialPlugin));
        private EventListener eventListener;
		public override void OnDisable()
        {
            base.OnDisable();
        }
        protected override void OnEnable()
        {
            eventListener = new EventListener(this);
            Context.Server.PlayerFactory.PlayerCreated += (sender, args) =>
			{
				Player player = args.Player;
				player.PlayerJoin += (o, eventArgs) => eventListener.OnJoin(player);
				player.PlayerLeave += (o, eventArgs) => eventListener.OnLeave(player);
			};
            Context.PluginManager.LoadCommands(new EssentialCommand());
		}
		/// <summary>
		/// Configure Own Classes.
		/// </summary>
		/// <param name="server"></param>
		public void Configure(MiNetServer server)
		{
			server.PlayerFactory = new EssentialPlayerFactory();
			server.ServerManager = new EssentialServerManager(server);
			Log.Warn("Executed startup successfully. Replaced identity managment.");
		}

    }
	
    class Command : ICommandFilter
    {
        public void OnCommandExecuted()
        {
            
        }

        public void OnCommandExecuting(Player player)
        {
            
        }
    }
    
}
