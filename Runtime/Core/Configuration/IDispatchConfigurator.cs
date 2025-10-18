using System.Collections.Generic;

namespace FLFloppa.Events
{
    /// <summary>
    /// Defines how dispatch recipes are constructed for event topics.
    /// </summary>
    public interface IDispatchConfigurator
    {
        /// <summary>
        /// Creates a dispatch recipe for the specified event type.
        /// </summary>
        /// <typeparam name="TEvent">Event payload type associated with the dispatch.</typeparam>
        /// <returns>A configured <see cref="DispatchRecipe{TEvent}"/>.</returns>
        DispatchRecipe<TEvent> Create<TEvent>() where TEvent : struct;
        /// <summary>
        /// Collects the bindings representing configured dispatch features, typically for editor use.
        /// </summary>
        /// <param name="bindings">Collection that receives feature bindings.</param>
        void CollectFeatures(ICollection<DispatchFeatureBinding> bindings);
    }
}
