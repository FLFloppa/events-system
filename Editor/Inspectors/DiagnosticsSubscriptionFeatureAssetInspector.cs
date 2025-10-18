using FLFloppa.EditorHelpers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace FLFloppa.Events.Editor
{
    [CustomEditor(typeof(DiagnosticsSubscriptionFeatureAsset))]
    public sealed class DiagnosticsSubscriptionFeatureAssetInspector : UnityEditor.Editor
    {
        private SerializedProperty _label;
        private SerializedProperty _logBefore;
        private SerializedProperty _logAfter;

        private void OnEnable()
        {
            _label = serializedObject.FindProperty("label");
            _logBefore = serializedObject.FindProperty("logBeforeInvoke");
            _logAfter = serializedObject.FindProperty("logAfterInvoke");
        }

        public override VisualElement CreateInspectorGUI()
        {
            serializedObject.Update();

            var root = InspectorUi.Layout.CreateRoot();

            root.Add(InspectorUi.Layout.CreateHeader(
                "Diagnostics Subscription Feature",
                "Logs before and/or after subscriber invocation for debugging purposes."));

            root.Add(InspectorUi.Cards.Create(
                "Configuration",
                out var configContent,
                "Customize the diagnostics label and which hooks emit log messages."));
            configContent.Add(InspectorUi.Controls.CreatePropertyField(_label, "Diagnostics Label"));
            configContent.Add(InspectorUi.Controls.CreatePropertyField(_logBefore, "Log Before Invoke"));
            configContent.Add(InspectorUi.Controls.CreatePropertyField(_logAfter, "Log After Invoke"));

            root.Add(InspectorUi.Cards.Create("Behaviour", out var behaviourContent, null));
            InspectorUi.Controls.AddHelpBox(
                behaviourContent,
                "When enabled, the feature emits Unity console logs around subscriber execution, helping trace event flow.",
                HelpBoxMessageType.Info);

            return root;
        }
    }
}
