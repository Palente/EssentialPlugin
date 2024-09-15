using log4net;
using MiNET;
using MiNET.Net;
using MiNET.UI;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using EssentialPlugin.Events;
using MiNET.Inventory;

namespace EssentialPlugin
{
    public class EssentialPlayer : Player
    {
        public MiNetServer Server { get; private set; }
        public readonly EssentialServer EServer;
        public IPEndPoint EndPoint { get; private set; }
        private static readonly ILog Log = LogManager.GetLogger(typeof(EssentialPlayer));
        public EssentialPlayer(MiNetServer server, IPEndPoint endPoint) : base(server, endPoint)
        {
            Server = server;
            EndPoint = endPoint;

            Inventory = new PlayerInventory(this);
            HungerManager = new HungerManager(this);
            ExperienceManager = new ExperienceManager(this);
            ItemStackInventoryManager = new ItemStackInventoryManager(this);

            IsSpawned = false;
            IsConnected = endPoint != null; // Can't connect if there is no endpoint

            Width = 0.6f;
            Length = Width;
            Height = 1.80;

            HideNameTag = false;
            IsAlwaysShowName = true;
            CanClimb = true;
            HasCollision = true;
            IsAffectedByGravity = true;
            NoAi = false;
            EServer = (EssentialServer) server.ServerManager.GetServer();
        }
        public bool IsOp() => EServer.IsOp(Username);
        public void SetOp(bool op)
        {
            //Don't work!
            this.ActionPermissions = op ? ActionPermissions.Operator : ActionPermissions.Default;
            this.CommandPermission = op ? 4 : 0;
            this.PermissionLevel = op ? PermissionLevel.Operator : PermissionLevel.Member;
            this.SendAdventureSettings();
            EServer.SetOp(Username, op);
        }
        public override void HandleMcpeLogin(McpeLogin message)
        {
            base.HandleMcpeLogin(message);
        }

        public override Form GetServerSettingsForm()
        {
            return base.GetServerSettingsForm();
        }

        public delegate void PlayerMessage(object sender, MessageReceivedEventArgs eventArgs);
        public event PlayerMessage OnMessageReceived;
        //Proof of concept for chat event
        public override void HandleMcpeText(McpeText message)
        {
            var text = message.message;
            if (string.IsNullOrEmpty(text))
                return;
            var eventArgs = new MessageReceivedEventArgs(this, text);
            OnMessageReceived?.Invoke(this, eventArgs);
            if(eventArgs.Cancelled)
                return;
            EServer.BroadcastMessage(eventArgs.FormattedMessage);
        }

    }
    public class EssentialPlayerFactory : PlayerFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(typeof(EssentialPlayerFactory));
        public static EssentialPlayerFactory instance { get; private set; } = null;
        public EssentialPlayerFactory()
        {
            instance = this;
        }
        public virtual Player CreatePlayer(MiNetServer server, IPEndPoint endPoint, PlayerInfo playerInfo)
        {
            var player = new EssentialPlayer(server, endPoint);
            /*player.MaxViewDistance = Config.GetProperty("MaxViewDistance", 22);
            player.MoveRenderDistance = Config.GetProperty("MoveRenderDistance", 1);*/
            player.MaxViewDistance = 22;
            player.MoveRenderDistance = 1;
            Log.Warn("Player Created With Class EssentialPlayer");
            OnPlayerCreated(new PlayerEventArgs(player));
            return player;
        }
    }
}
