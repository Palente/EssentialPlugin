using System;

namespace EssentialPlugin.Events;

public class MessageReceivedEventArgs : EventArgs
{
    public string Message { get; set; }
    public EssentialPlayer Player { get; set; }
    public string FormattedMessage { get; set; }
    public bool Cancelled { get; set; }

    public MessageReceivedEventArgs(EssentialPlayer player, string message)
    {
        this.Message = message;
        this.FormattedMessage = $"§8{player.Username}§7: {message}";
        this.Player = player;
        this.Cancelled = false;
    }
}