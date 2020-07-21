using MiNET;
using MiNET.Items;
using MiNET.Plugins;
using MiNET.Plugins.Attributes;
using MiNET.Worlds;
using System;
using System.Collections.Generic;
using System.Text;

namespace EssentialPlugin.Commands
{
    class EssentialCommand : Command
    {
        [Command(Name = "essential", Description = "Essential Command")]
        public void Essential(Player player)
        {
            player.SendMessage((((EssentialPlayer)player).IsOp() ? "Op":"Random"));
        }

        [Command(Name = "gamemode", Description = "Change Your GameMode")]
        public void GameMode(EssentialPlayer player, GameMode gameMode)
        {
            if (!player.IsOp())
            {
                player.SendMessage("You don't have the permission to use this command!");
                return;
            }
            player.SetGameMode(gameMode);
            player.SendMessage("Changed Successfully your gamemode !");
        }

        /*[Command]
         * Don't Work as excepted!
        public void GameMode(EssentialPlayer author, EssentialPlayer player, GameMode gameMode)
        {
            if (!author.IsOp())
            {
                author.SendMessage("You don't have the permission to use this command!");
                return;
            }
            player.SetGameMode(gameMode);
            player.SendMessage("Changed Successfully your gamemode to " + gameMode.ToString());
            author.SendMessage($"Changed the gamemode of {player.Username} to {gameMode.ToString()}");
        }*/

        [Command(Name = "give", Description = "Give Item")]
        public void Give(EssentialPlayer player, int id, int amount)
        {
            if (!player.IsOp())
            {
                player.SendMessage("You don't have the permission to use this command!");
                return;
            }
            var item = ItemFactory.GetItem((short)id, 0, amount);
            player.Inventory.AddItem(item, true);
            player.SendMessage($"You received {item.Name}");
        }

        [Command(Name = "op", Description = "Op a Player")]
        public void Op(EssentialPlayer sender, string playername)
        {
            var player = sender.EServer.GetPlayer(playername);
            if (!sender.IsOp())
            {
                player.SendMessage("You don't have the permission to use this command!");
                return;
            }
            player.SetOp(true);
            player.EServer.BroadcastMessage($"§8{player.Username} is now Operator!", true);
        }
        [Command(Name = "deop", Description = "Deop a Player")]
        public void DeOp(EssentialPlayer sender, string playername)
        {
            var player = sender.EServer.GetPlayer(playername);
            if (!sender.IsOp())
            {
                sender.SendMessage("You don't have the permission to use this command!");
                return;
            }
            player.SetOp(false);
            player.EServer.BroadcastMessage($"§8{player.Username} is not anymore Operator!", true);
        }

    }
}
