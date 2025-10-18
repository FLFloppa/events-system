using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// Base ScriptableObject that builds an <see cref="IExecutorStrategy"/> instance.
    /// </summary>
    public abstract class ExecutorStrategyAsset : ScriptableObject, IBuildable<IExecutorStrategy>
    {
        /// <summary>
        /// Creates an executor strategy for runtime use.
        /// </summary>
        public abstract IExecutorStrategy Build();
    }
}
