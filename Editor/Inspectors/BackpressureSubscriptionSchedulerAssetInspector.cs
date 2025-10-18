using FLFloppa.EditorHelpers;
using UnityEditor;
using UnityEngine.UIElements;

namespace FLFloppa.Events.Editor
{
    [CustomEditor(typeof(BackpressureSubscriptionSchedulerAsset))]
    public sealed class BackpressureSubscriptionSchedulerAssetInspector : UnityEditor.Editor
    {
        private SerializedProperty _defaultBacklogLimit;

        private void OnEnable()
        {
            _defaultBacklogLimit = serializedObject.FindProperty("defaultBacklogLimit");
        }

        public override VisualElement CreateInspectorGUI()
        {
            serializedObject.Update();

            var root = InspectorUi.Layout.CreateRoot();

            root.Add(InspectorUi.Layout.CreateHeader(
                "Backpressure Subscription Scheduler",
                "Limit the subscriber backlog to pause dispatch when consumers fall behind."));

            root.Add(InspectorUi.Cards.Create(
                "Configuration",
                out var configContent,
                "Choose a backlog limit that balances throughput with memory usage."));
            configContent.Add(InspectorUi.Controls.CreatePropertyField(_defaultBacklogLimit, "Default Backlog Limit"));

            root.Add(InspectorUi.Cards.Create("Behaviour", out var behaviourContent, null));
            InspectorUi.Controls.AddHelpBox(
                behaviourContent,
                "When backlog exceeds the limit, events are paused until consumers catch up.",
                HelpBoxMessageType.Info);

            return root;
        }
    }
}
