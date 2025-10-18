using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// ScriptableObject that creates diagnostics subscription features with configurable logging behavior.
    /// </summary>
    [CreateAssetMenu(
        fileName = "DiagnosticsSubscriptionFeature",
        menuName = "FLFloppa/Events/Subscription Features/Diagnostics")]
    public sealed class DiagnosticsSubscriptionFeatureAsset : SubscriptionFeatureAsset
    {
        [SerializeField] private string label = "SubscriptionDiagnostics";
        [SerializeField] private bool logBeforeInvoke = true;
        [SerializeField] private bool logAfterInvoke = true;

        /// <summary>
        /// Creates a diagnostics feature factory using the serialized configuration.
        /// </summary>
        public override ISubscriptionFeatureFactory CreateFactory()
        {
            return new DiagnosticsSubscriptionFeatureFactory(label, logBeforeInvoke, logAfterInvoke);
        }
    }
}
