using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// ScriptableObject asset that produces a no-op diagnostics provider instance.
    /// </summary>
    [CreateAssetMenu(
        fileName = "NullDiagnosticsProvider",
        menuName = "FLFloppa/Events/Diagnostics Providers/Null")]
    public sealed class NullDiagnosticsProviderAsset : DiagnosticsProviderAsset
    {
        /// <inheritdoc />
        public override IEventDiagnosticsProvider Build()
        {
            return NullEventDiagnosticsProvider.Instance;
        }
    }
}
