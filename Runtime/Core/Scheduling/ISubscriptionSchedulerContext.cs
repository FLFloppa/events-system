namespace FLFloppa.Events
{
    /// <summary>
    /// Marker interface describing scheduler-specific context objects exposed to subscription features.
    /// </summary>
    public interface ISubscriptionSchedulerContext<TEvent> where TEvent : struct
    {
    }
}
