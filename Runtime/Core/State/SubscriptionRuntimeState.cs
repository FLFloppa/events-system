namespace FLFloppa.Events
{
    /// <summary>
    /// Runtime state object passed to subscription features during lifecycle operations.
    /// </summary>
    public readonly struct SubscriptionRuntimeState<TEvent> where TEvent : struct
    {
        private readonly ISubscriptionSchedulerContext<TEvent>? _schedulerContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptionRuntimeState{TEvent}"/> struct.
        /// </summary>
        /// <param name="topicId">Identifier for the associated topic.</param>
        /// <param name="handle">Handle referencing the subscription.</param>
        /// <param name="schedulerContext">Optional scheduler context supplied by the scheduler.</param>
        internal SubscriptionRuntimeState(ushort topicId, SubscriptionHandle<TEvent> handle, ISubscriptionSchedulerContext<TEvent>? schedulerContext)
        {
            TopicId = topicId;
            Handle = handle;
            _schedulerContext = schedulerContext;
        }

        /// <summary>
        /// Gets the identifier of the topic this state belongs to.
        /// </summary>
        public ushort TopicId { get; }
        /// <summary>
        /// Gets the handle referencing the subscription.
        /// </summary>
        public SubscriptionHandle<TEvent> Handle { get; }

        /// <summary>
        /// Attempts to apply a scheduler command if the scheduler supplied a context.
        /// </summary>
        /// <param name="command">Command issued by a feature to mutate scheduler state.</param>
        /// <returns><c>true</c> when the command was accepted; otherwise <c>false</c>.</returns>
        public bool TryApplySchedulerCommand(ISubscriptionSchedulerCommand<TEvent> command)
        {
            if (_schedulerContext == null || command == null)
            {
                return false;
            }

            return command.TryApply(_schedulerContext);
        }
    }
}
