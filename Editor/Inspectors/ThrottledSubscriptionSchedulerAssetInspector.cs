using FLFloppa.EditorHelpers;
using UnityEditor;
using UnityEngine.UIElements;

namespace FLFloppa.Events.Editor
{
    [CustomEditor(typeof(ThrottledSubscriptionSchedulerAsset))]
    public sealed class ThrottledSubscriptionSchedulerAssetInspector : UnityEditor.Editor
    {
        private SerializedProperty _defaultThrottleSeconds;

        private void OnEnable()
        {
            _defaultThrottleSeconds = serializedObject.FindProperty("defaultThrottleSeconds");
        }

        public override VisualElement CreateInspectorGUI()
        {
            serializedObject.Update();

            var root = InspectorUi.Layout.CreateRoot();

            root.Add(InspectorUi.Layout.CreateHeader(
                "Throttled Subscription Scheduler",
                "Limit dispatch frequency by enforcing a cooldown between subscriber invocations."));

            root.Add(InspectorUi.Cards.Create(
                "Configuration",
                out var configContent,
                "Choose a throttle duration that matches how frequently events should reach subscribers."));
            configContent.Add(InspectorUi.Controls.CreatePropertyField(_defaultThrottleSeconds, "Default Throttle (Seconds)"));

            root.Add(InspectorUi.Cards.Create("Behaviour", out var behaviourContent, null));
            InspectorUi.Controls.AddHelpBox(
                behaviourContent,
                "Events dispatched faster than this interval will be deferred until the cooldown elapses.",
                HelpBoxMessageType.Info);

            return root;
        }
    }
}
