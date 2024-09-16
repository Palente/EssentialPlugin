namespace EssentialPlugin.Events;

/// <summary>
/// If an event is cancellable, it should implement this interface.
/// </summary>
public interface IEventCancellable
{
    public bool Cancelled { get; set; }
}