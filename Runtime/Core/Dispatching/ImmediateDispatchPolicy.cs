using System;

namespace FLFloppa.Events
{
    /// <summary>
    /// Dispatch policy that immediately executes subscribers using the provided executor.
    /// </summary>
    public sealed class ImmediateDispatchPolicy : IDispatchPolicy
    {
        /// <summary>
        /// Singleton instance of the immediate dispatch policy.
        /// </summary>
        public static IDispatchPolicy Instance { get; } = new ImmediateDispatchPolicy();

        private ImmediateDispatchPolicy()
        {
        }

        /// <inheritdoc />
        public void Dispatch<TEvent>(Topic<TEvent> topic, in TEvent payload, DispatchRecipe<TEvent> recipe, IExecutorStrategy executor) where TEvent : struct
        {
            if (executor is null) throw new ArgumentNullException(nameof(executor));
            executor.Execute(topic, payload, recipe.Features);
        }
    }
}
