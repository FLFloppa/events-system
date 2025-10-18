using FLFloppa.EditorHelpers;
using UnityEditor;
using UnityEngine.UIElements;

namespace FLFloppa.Events.Editor
{
    [CustomEditor(typeof(NullDiagnosticsProviderAsset))]
    public sealed class NullDiagnosticsProviderAssetInspector : UnityEditor.Editor
    {
        public override VisualElement CreateInspectorGUI()
        {
            var root = InspectorUi.Layout.CreateRoot();

            root.Add(InspectorUi.Layout.CreateHeader(
                "Null Diagnostics Provider",
                "Disables diagnostics emission by supplying a no-op provider."));

            root.Add(InspectorUi.Cards.Create("Behaviour", out var behaviourContent, null));
            InspectorUi.Controls.AddHelpBox(
                behaviourContent,
                "Ideal for production builds or scenarios where diagnostics overhead must be eliminated.",
                HelpBoxMessageType.Info);

            InspectorUi.Controls.AddHelpBox(
                behaviourContent,
                "Combine with diagnostics-aware configs so you can swap back to instrumented providers when required.",
                HelpBoxMessageType.Warning);

            return root;
        }
    }
}
