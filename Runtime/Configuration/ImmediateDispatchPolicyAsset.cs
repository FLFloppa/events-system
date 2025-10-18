using UnityEngine;

namespace FLFloppa.Events
{
    /// <summary>
    /// ScriptableObject that builds the singleton immediate dispatch policy.
    /// </summary>
    [CreateAssetMenu(
        fileName = "ImmediateDispatchPolicy",
        menuName = "FLFloppa/Events/Dispatch Policies/Immediate")]
    public sealed class ImmediateDispatchPolicyAsset : DispatchPolicyAsset
    {
        /// <summary>
        /// Returns the <see cref="ImmediateDispatchPolicy"/> instance.
        /// </summary>
        public override IDispatchPolicy Build()
        {
            return ImmediateDispatchPolicy.Instance;
        }
    }
}
