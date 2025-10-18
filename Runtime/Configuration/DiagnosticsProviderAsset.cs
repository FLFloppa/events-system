using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// Base ScriptableObject asset that builds <see cref="IEventDiagnosticsProvider"/> instances for runtime use.
    /// </summary>
    public abstract class DiagnosticsProviderAsset : ScriptableObject, IBuildable<IEventDiagnosticsProvider>
    {
        /// <summary>
        /// Creates a diagnostics provider instance.
        /// </summary>
        public abstract IEventDiagnosticsProvider Build();
    }
}
