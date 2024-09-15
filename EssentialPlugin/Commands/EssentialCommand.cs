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
        public void Give(EssentialPlayer player, ItemTypeEnum itemName, int amount)
        {
            if (!player.IsOp())
            {
                player.SendMessage("You don't have the permission to use this command!");
                return;
            }
            var item = ItemFactory.GetItem(itemName.Value, 0, amount);
            player.Inventory.AddItem(item, true);
            player.SendMessage($"You received {item.Id}");
        }

        [Command(Name = "op", Description = "Op a Player")]
        [Authorize(Permission = 4)]
        public void Op(EssentialPlayer sender, Target target)
        {
            if (target.Players is null)
            {
                sender.SendMessage("Player not found!");
                return;
            }

            foreach (var minetPlayer in target.Players)
            {
                var player = sender.EServer.GetPlayer(minetPlayer);
                if (player is null)
                {
                    sender.SendMessage("Player could not be found!");
                    continue;
                }
                player.SetOp(true);
                player.EServer.BroadcastMessage($"§8{player.Username} is now Operator!", true);
            }
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
