namespace FLFloppa.Events
{
    /// <summary>
    /// Defines how subscription recipes are constructed for event topics.
    /// </summary>
    public interface ISubscriptionConfigurator
    {
        /// <summary>
        /// Creates a subscription recipe for the specified event type.
        /// </summary>
        /// <typeparam name="TEvent">Event payload type associated with the subscription.</typeparam>
        /// <returns>A configured <see cref="SubscriptionRecipe{TEvent}"/>.</returns>
        SubscriptionRecipe<TEvent> Create<TEvent>() where TEvent : struct;
        /// <summary>
        /// Collects the bindings representing configured subscription features, typically for editor use.
        /// </summary>
        /// <param name="bindings">Collection that receives feature bindings.</param>
        void CollectFeatures(System.Collections.Generic.ICollection<SubscriptionFeatureBinding> bindings);
    }
}
