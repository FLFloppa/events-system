using FLFloppa.EditorHelpers;
using UnityEditor;
using UnityEngine.UIElements;

namespace FLFloppa.Events.Editor
{
    [CustomEditor(typeof(PrioritySubscriptionSchedulerAsset))]
    public sealed class PrioritySubscriptionSchedulerAssetInspector : UnityEditor.Editor
    {
        private SerializedProperty _defaultPriority;

        private void OnEnable()
        {
            _defaultPriority = serializedObject.FindProperty("defaultPriority");
        }

        public override VisualElement CreateInspectorGUI()
        {
            serializedObject.Update();

            var root = InspectorUi.Layout.CreateRoot();

            root.Add(InspectorUi.Layout.CreateHeader(
                "Priority Subscription Scheduler",
                "Assign priorities to control which subscribers execute first."));

            root.Add(InspectorUi.Cards.Create(
                "Configuration",
                out var configContent,
                "Use higher values for critical systems that must process events sooner."));
            configContent.Add(InspectorUi.Controls.CreatePropertyField(_defaultPriority, "Default Priority"));

            root.Add(InspectorUi.Cards.Create("Behaviour", out var behaviourContent, null));
            InspectorUi.Controls.AddHelpBox(
                behaviourContent,
                "Higher values run before lower values. Ties fall back to subscription order.",
                HelpBoxMessageType.Info);

            return root;
        }
    }
}
