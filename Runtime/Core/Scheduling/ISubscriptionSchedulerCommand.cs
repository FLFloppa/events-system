namespace FLFloppa.Events
{
    /// <summary>
    /// Describes a command that can mutate scheduler state for a subscriber.
    /// </summary>
    public interface ISubscriptionSchedulerCommand<TEvent> where TEvent : struct
    {
        /// <summary>
        /// Attempts to apply the command to the provided scheduler context.
        /// </summary>
        /// <param name="context">Scheduler context obtained during subscription.</param>
        /// <returns><c>true</c> when the command was accepted; otherwise <c>false</c>.</returns>
        bool TryApply(ISubscriptionSchedulerContext<TEvent> context);
    }
}
