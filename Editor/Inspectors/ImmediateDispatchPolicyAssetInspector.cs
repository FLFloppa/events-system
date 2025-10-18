using FLFloppa.EditorHelpers;
using UnityEditor;
using UnityEngine.UIElements;

namespace FLFloppa.Events.Editor
{
    [CustomEditor(typeof(ImmediateDispatchPolicyAsset))]
    public sealed class ImmediateDispatchPolicyAssetInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = InspectorUi.Layout.CreateRoot();

            root.Add(InspectorUi.Layout.CreateHeader(
                "Immediate Dispatch Policy",
                "Dispatches events synchronously on the publishing thread without queuing."));

            root.Add(InspectorUi.Cards.Create("Behaviour", out var behaviourContent, null));
            InspectorUi.Controls.AddHelpBox(
                behaviourContent,
                "Use this policy when event handlers are fast and you require immediate delivery.",
                HelpBoxMessageType.Info);

            InspectorUi.Controls.AddHelpBox(
                behaviourContent,
                "Consider pairing with a scheduler that guards against long-running subscribers if latency is critical.",
                HelpBoxMessageType.Warning);

            return root;
        }
    }
}
